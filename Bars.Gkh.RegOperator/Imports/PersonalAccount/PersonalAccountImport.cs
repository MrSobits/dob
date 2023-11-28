﻿namespace Bars.Gkh.RegOperator.Imports
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.Gkh.Domain.TableLocker;
    using Bars.Gkh.Enums.Import;
    using Bars.Gkh.Import;
    using Bars.Gkh.Import.Impl;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.Utils;

    using Castle.Core.Internal;

    using ExecutionContext = Bars.B4.Modules.Tasks.Common.Contracts.ExecutionContext;

    // !!! Важно! Регистрируйте в контейнере как синглтон.
    /// <summary>
    ///     Импорт данных из Биллинга
    /// </summary>
    public class PersonalAccountImport : GkhImportBase
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        #region IGkhImport Members
        /// <summary>
        /// Ключ
        /// </summary>
        public override string Key => PersonalAccountImport.Id;

        /// <summary>
        /// Код
        /// </summary>
        public override string CodeImport => "PersonalAccountImport";

        /// <summary>
        /// Название
        /// </summary>
        public override string Name => "Импорт лицевых счетов";

        /// <summary>
        /// Допустимые расширения
        /// </summary>
        public override string PossibleFileExtensions => "csv";

        /// <summary>
        /// Код для прав доступа
        /// </summary>
        public override string PermissionName => "Import.PersonalAccountImport";

        private readonly Stopwatch sw = new Stopwatch();

        /// <summary>
        /// Импорт
        /// </summary>
        protected override ImportResult Import(
            BaseParams baseParams,
            ExecutionContext ctx,
            IProgressIndicator indicator,
            CancellationToken ct)
        {
            var file = baseParams.Files["FileImport"];
            var userId = baseParams.Params.GetAs<long>("userId");

            var importers = this.Container.ResolveAll<IBillingFileImporter>();

            var importer = file.FileName == "uids"
                ? importers.FirstOrDefault(x => x.FileName == "addressUids")
                : // импорт сопоставлений
                importers.FirstOrDefault(x => x.FileName == "kaprem");

            if (importer == null)
            {
                importers.ForEach(x => this.Container.Release(x));
                this.sw.Stop();
                throw new NotImplementedException("Отсутствует реализация импорта лицевых счетов");
            }

            var stream = new MemoryStream(file.Data) {Position = 0};
            StatusImport statusImport;

            var logImport = this.Container.Resolve<ILogImport>();
            var logImportManager = this.Container.Resolve<ILogImportManager>();
            var sessionProvider = this.Container.Resolve<ISessionProvider>();

            long logFileId;
            using (this.Container.Using(importers, logImport, logImportManager, sessionProvider))
            {
                logImport.Info(
                    "Старт импорта лицевых счетов",
                    $"Инициализация импорта лицевых счетов: {file.FileName}.{file.Extention}, импортер {importer.GetType().Name}");
                try
                {
                    importer.Import(stream, file.GetFullName(), logImport, indicator, userId);
                }
                catch (ValidationException exception)
                {
                    logImport.Error(
                        "Ошибка валидации импорта",
                        $"Message = {exception.Message}");
                    logImport.CountError += 1;

                    throw;
                }
                catch (Exception exception)
                {
                    logImport.Error(
                        "Ошибка импорта",
                        $"Message = {exception.Message} | InnerMessage = {exception.InnerException?.Message ?? ""}");
                    logImport.CountError += 1;
                }
                finally
                {
                    // принудительно создать новую сессию, так как после импорта ЛС предыдущая сессия закрывается
                    // и дальнейшее сохранение объектов не производится
                    sessionProvider.CreateNewSession();

                    importers.ForEach(x => this.Container.Release(x));
                    logImport.SetFileName(file.FileName);
                    logImport.ImportKey = this.CodeImport;

                    logImportManager.FileNameWithoutExtention = file.FileName;
                    logImportManager.Add(file, logImport);
                    logFileId = logImportManager.Save();

                    statusImport = logImport.CountError > 0
                        ? StatusImport.CompletedWithError
                        : logImport.CountWarning > 0
                            ? StatusImport.CompletedWithWarning
                            : StatusImport.CompletedWithoutError;

                    this.sw.Stop();
                }
            }

            var message = $"Время выполнения: {this.sw.ElapsedMilliseconds} мс;";
            if (logFileId == 0)
            {
                message += "Лог файл не сохранен;";
            }

            return new ImportResult(statusImport, message)
            {
                Success = true,
                LogFileId = logFileId
            };
        }

        /// <summary>
        /// Валидация
        /// </summary>
        public override bool Validate(BaseParams baseParams, out string message)
        {
            message = null;

            var tableLocker = this.Container.Resolve<ITableLocker>();
            try
            {
                if (tableLocker.CheckLocked<BasePersonalAccount>("INSERT"))
                {
                    message = TableLockedException.StandardMessage;
                    return false;
                }
            }
            finally
            {
                this.Container.Release(tableLocker);
            }

            if (!baseParams.Files.ContainsKey("FileImport"))
            {
                message = "Не выбран файл для импорта";
                return false;
            }

            var fileData = baseParams.Files["FileImport"];
            var extention = fileData.Extention;

            var fileExtentions = this.PossibleFileExtensions.Contains(",")
                ? this.PossibleFileExtensions.Split(',')
                : new[] {this.PossibleFileExtensions};
            if (fileExtentions.All(x => x != extention))
            {
                message = $"Необходимо выбрать файл с допустимым расширением: {this.PossibleFileExtensions}";
                return false;
            }

            return true;
        }
        #endregion
    }
}