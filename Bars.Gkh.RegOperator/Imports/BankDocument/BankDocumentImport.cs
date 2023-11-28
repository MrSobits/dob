﻿namespace Bars.Gkh.RegOperator.Imports
{
    using DomainService.BankDocumentImport;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    using B4;
    using B4.DataAccess;
    using B4.IoC;
    using B4.Utils;

    using Bars.Gkh.Domain.CollectionExtensions;

    using Gkh.Entities;
    using Gkh.Enums;
    using Gkh.Enums.Import;
    using Import;
    using Domain.ImportExport;
    using Domain.ImportExport.IR;
    using Domain.ImportExport.Mapping;
    using DomainService;
    using Entities;
    using Enums;
    using Wcf.Contracts.PersonalAccount;
    using Import.Impl;
    using Newtonsoft.Json;

    /// <summary>
    /// Импорт документа из банка
    /// </summary>
    public class BankDocumentImport : GkhImportBase
    {
        public const string OverrideFileInfoKey = "OverrideFileInfo";

        private ILogImport logImport;
        private DateTime fileDate;
        private string fileNumber;
        private string agentId;
        private string agentName;
        private string importTypeDisplay;

        /// <summary>
        /// Лог менеджер
        /// </summary>
        public ILogImportManager LogManager { get; set; }

        /// <summary>
        /// Провайдер импорта документа из банка
        /// </summary>
        public IBankDocumentImportProvider BankDocumentImportProvider { get; set; }

        /// <summary>
        /// Идентификатор импорта
        /// </summary>
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        /// <summary>
        /// Ключ импорта
        /// </summary>
        public override string Key => BankDocumentImport.Id;

        /// <summary>
        /// Код импорта
        /// </summary>
        public override string CodeImport => "BankDocument";

        /// <summary>
        /// Наименование импорта
        /// </summary>
        public override string Name => "Импорт реестров оплат";

        /// <summary>
        /// Допустимые расширения файла
        /// </summary>
        public override string PossibleFileExtensions => string.Empty;

        /// <summary>
        /// Ограничение
        /// </summary>
        public override string PermissionName => "Import.BankDocument";

        /// <summary>
        /// Импорт/Экспорт провайдер
        /// </summary>
        public ImportExportDataProvider ImportExportProvider { get; set; }

        /// <summary>
        /// Провести валидацию
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <param name="message">Сообщение</param>
        /// <returns>Результат валидации</returns>
        public override bool Validate(BaseParams baseParams, out string message)
        {
            message = string.Empty;
            return base.Validate(baseParams, out message);
        }

        /// <summary>
        /// Проинициализировать лог
        /// </summary>
        /// <param name="fileName">Имя файла</param>
        public new void InitLog(string fileName)
        {
            this.LogManager = this.Container.Resolve<ILogImportManager>();
            if (this.LogManager == null)
            {
                throw new Exception("Не найдена реализация интерфейса ILogImportManager");
            }

            this.LogManager.FileNameWithoutExtention = fileName;
            this.LogManager.UploadDate = DateTime.Now;

            this.logImport = this.Container.ResolveAll<ILogImport>().FirstOrDefault(x => x.Key == MainLogImportInfo.Key);
            if (this.logImport == null)
            {
                throw new Exception("Не найдена реализация интерфейса ILogImport");
            }

            this.logImport.SetFileName(fileName);
            this.logImport.ImportKey = this.Key;
        }

        /// <summary>
        /// Импортировать
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат импорта</returns>
        public override ImportResult Import(BaseParams baseParams)
        {
            return this.StartSingleFileImport(baseParams.Files["FileImport"], baseParams);
        }

        /// <summary>
        /// Стартует импорт одного файла
        /// </summary>
        /// <param name="file">Файл для импорта</param>
        /// <param name="baseParams">Основные параметры</param>
        /// <returns>Результат выполнения импорта</returns>
        private ImportResult StartSingleFileImport(FileData file, BaseParams baseParams)
        {
            var overwrite = baseParams.Params.GetAs<YesNo>("overwrite");
            var distributePenalty = baseParams.Params.GetAs<YesNo>("distrPenalty");
            var providerCode = baseParams.Params.GetAs<string>("providerCode");

            this.InitLog(file.FileName);

            var message = new StringBuilder();
            Entities.BankDocumentImport bankDocImport = null;
            try
            {
                var payments = this.GetRows(file, baseParams).ToArray();
                var firstPayment = payments.FirstOrDefault();

                if (!this.fileNumber.IsEmpty())
                {
                    var query = this.GetSameDocumentPayments();
                    if (query.Any(x => x.PaymentConfirmationState == ImportedPaymentPaymentConfirmState.Distributed))
                    {
                        var description = "Имеются подтвержденные оплаты по документу с номером {0} от {1:dd.MM.yyyy}"
                            .FormatUsing(this.fileNumber, this.fileDate);

                        this.logImport.Error("Ошибка", description);
                        return new ImportResult(StatusImport.CompletedWithError, "Имеются подтвержденные оплаты по документу с такими номером и датой");
                    }

                    if (query.Any())
                    {
                        var description = "Имеются неподтвержденные оплаты по документу с номером {0} от {1:dd.MM.yyyy}"
                            .FormatUsing(this.fileNumber, this.fileDate);

                        this.logImport.Info("Информация", description);

                        if (overwrite == YesNo.Yes)
                        {
                            var bankDocImportDomain = this.Container.ResolveDomain<Entities.BankDocumentImport>();
                            var bankDocImportService = this.Container.Resolve<IBankDocumentImportService>();
                            try
                            {
                                var bankDocIds = bankDocImportDomain.GetAll().Where(x => x.DocumentNumber == this.fileNumber && x.DocumentDate == this.fileDate)
                                    .Where(x => x.PaymentConfirmationState == PaymentConfirmationState.NotDistributed)
                                    .Select(x => x.Id)
                                    .ToList();

                                this.CleanupSameDocumentImports(bankDocIds);
                            }
                            finally
                            {
                                this.Container.Release(bankDocImportDomain);
                                this.Container.Release(bankDocImportService);
                            }
                            
                        }
                        else
                        {
                            this.logImport.Error("Ошибка", "Перезапись существующих неподтвержденных оплат не разрешена");

                            return new ImportResult(StatusImport.CompletedWithError, description);
                        }
                    }
                }

                var paymentInfoArr = firstPayment == null ? new PersonalAccountPaymentInfoIn[0] : payments;

                paymentInfoArr.ForEach(x =>
                {
                    x.Refund = Math.Abs(x.Refund);
                    x.PenaltyRefund = Math.Abs(x.PenaltyRefund);
                });

                var expectedSum = paymentInfoArr.Select(x => x.TargetPaid)
                    .Concat(paymentInfoArr.Select(x => x.SumPaid))
                    .Concat(paymentInfoArr.Select(x => x.PenaltyPaid))
                    .Concat(paymentInfoArr.Select(x => x.SocialSupport))
                    .Concat(paymentInfoArr.Select(x => x.Refund))
                    .Concat(paymentInfoArr.Select(x => x.PenaltyRefund))
                    .Where(x => x > 0)
                    .SafeSum();

                this.BankDocumentImportProvider.IndicateAction = this.Indicate;
                bankDocImport = this.BankDocumentImportProvider.CreateBankDocumentImport(paymentInfoArr,
                    BankDocumentImportType.BankDocument,
                    this.logImport,
                    providerCode,
                    file.FileName);

                decimal actualSum;
                if (!this.ValidateImport(bankDocImport, expectedSum, out actualSum))
                {
                    this.logImport.Error(
                        "Ошибка",
                        "Сумма оплат по файлу ({0:0.00} руб.) не совпала с суммой фактически загруженных оплат ({1:0.00} руб.). Загрузка реестра будет отменена"
                            .FormatUsing(expectedSum, actualSum));

                    this.CleanupImport(bankDocImport);
                    bankDocImport = null;
                }

                if (bankDocImport != null)
                {
                    bankDocImport.DistributePenalty = distributePenalty;

                    if (!bankDocImport.DocumentDate.HasValue)
                    {
                        bankDocImport.DocumentDate = this.fileDate;
                    }

                    if (string.IsNullOrEmpty(bankDocImport.DocumentNumber))
                    {
                        bankDocImport.DocumentNumber = this.fileNumber;
                    }

                    if (string.IsNullOrEmpty(bankDocImport.PaymentAgentCode) && !string.IsNullOrEmpty(this.agentId))
                    {
                        bankDocImport.PaymentAgentCode = this.agentId;
                    }

                    if (string.IsNullOrEmpty(bankDocImport.PaymentAgentName) && !string.IsNullOrEmpty(this.agentName))
                    {
                        bankDocImport.PaymentAgentName = this.agentName;
                    }

                    if (string.IsNullOrEmpty(bankDocImport.ImportType) && !string.IsNullOrEmpty(this.importTypeDisplay))
                    {
                        bankDocImport.ImportType = this.importTypeDisplay;
                    }
                }
            }
            catch (DbfIRTranslatorException e)
            {
                this.LogFileFormatError(e.Message);
            }
            catch (ArgumentOutOfRangeException)
            {
                this.LogFileFormatError();
            }
            catch (ArgumentException)
            {
                this.LogFileFormatError();
            }
            catch (JsonSerializationException)
            {
                this.logImport.Error("Исключение", "Ошибка чтения файла. Убедитесь, что файл содержит корректный JSON.");
            }
            catch (JsonReaderException e)
            {
                this.logImport.Error("Исключение", "Ошибка чтения файла. Строка {0}, символ {1}.".FormatUsing(e.LineNumber, e.LinePosition));
            }
            catch (Exception e)
            {
                this.logImport.Error("Исключение", "Критическая ошибка: {0}. StackTrace: {1}".FormatUsing(e.Message, e.StackTrace));
            }
            finally
            {
                this.LogManager.Add(file, this.logImport);
                this.LogManager.Save();
            }

            if (bankDocImport != null)
            {
                bankDocImport.LogImport = this.Container.Resolve<IRepository<LogImport>>().GetAll()
                    .FirstOrDefault(x => x.LogFile.Id == this.LogManager.LogFileId);

                var docImportDomain = this.Container.ResolveDomain<Entities.BankDocumentImport>();
                using (this.Container.Using(docImportDomain))
                {
                    docImportDomain.Update(bankDocImport);
                }
            }

            message.Append(this.LogManager.GetInfo());
            var status = this.LogManager.GetStatus();

            return new ImportResult(status, message.ToString(), string.Empty, this.LogManager.LogFileId);
        }

        private bool ValidateImport(Entities.BankDocumentImport import, decimal expectedSum, out decimal actualSum)
        {
            var importedPaymentDomain = this.Container.ResolveDomain<ImportedPayment>();
            try
            {
                actualSum = importedPaymentDomain.GetAll().Where(x => x.BankDocumentImport.Id == import.Id).Select(x => x.Sum).SafeSum();
                return actualSum == expectedSum;
            }
            finally
            {
                this.Container.Release(importedPaymentDomain);
            }
        }

        private void CleanupImport(Entities.BankDocumentImport import)
        {
            var bankDocImportDomain = this.Container.ResolveDomain<Entities.BankDocumentImport>();
            var importedPaymentDomain = this.Container.ResolveDomain<ImportedPayment>();
            using (this.Container.Using(bankDocImportDomain, importedPaymentDomain))
            {
                var payments =
                    importedPaymentDomain.GetAll()
                        .Where(x => x.BankDocumentImport.Id == import.Id)
                        .Select(x => x.Id)
                        .ToList();

                foreach (var payment in payments)
                {
                    importedPaymentDomain.Delete(payment);
                }

                bankDocImportDomain.Delete(import.Id);
            }
        }

        private IQueryable<ImportedPayment> GetSameDocumentPayments()
        {
            var importedPaymentDomain = this.Container.ResolveDomain<ImportedPayment>();
            using (this.Container.Using(importedPaymentDomain))
            {
                var importedPayments = importedPaymentDomain.GetAll()
                    .Where(x => x.BankDocumentImport.DocumentNumber == this.fileNumber
                        && (x.BankDocumentImport.DocumentDate == this.fileDate || this.fileDate == DateTime.MinValue))
                    .Where(x => x.PaymentConfirmationState != ImportedPaymentPaymentConfirmState.Deleted);

                return importedPayments;
            }
        }

        private void CleanupSameDocumentImports(List<long> bankDocIds)
        {
            var bankDocImportDomain = this.Container.ResolveDomain<Entities.BankDocumentImport>();
            var importedPaymentDomain = this.Container.ResolveDomain<ImportedPayment>();
            using (this.Container.Using(bankDocImportDomain, importedPaymentDomain))
            {
                var paymentsByDocImport = importedPaymentDomain.GetAll()
                    .WhereContains(x => x.BankDocumentImport.Id, bankDocIds)
                    .Select(x => new { x.Id, x.BankDocumentImport, x.Sum })
                    .AsEnumerable()
                    .GroupBy(x => x.BankDocumentImport)
                    .ToDictionary(x => x.Key, x => x.ToList());

                foreach (var paymentGroup in paymentsByDocImport)
                {
                    paymentGroup.Value.ForEach(x => importedPaymentDomain.Delete(x.Id));
                    bankDocImportDomain.Delete(paymentGroup.Key.Id);
                }
            }
        }

        private IEnumerable<PersonalAccountPaymentInfoIn> GetRows(FileData file, BaseParams baseParams)
        {
            var providerCode = baseParams.Params.GetAs<string>("providerCode");
            var serializerCode = baseParams.Params.GetAs("serializerCode", "default");
            var map = ImportMapHelper.GetMapByKey(providerCode, this.Container);

            if (map == null)
            {
                this.logImport.Error("Ошибка", "Не удалось найти описание формата импорта по коду '{0}'".FormatUsing(providerCode));
                yield break;
            }

            var data = this.ImportExportProvider.Deserialize<PersonalAccountPaymentInfoIn>(file, baseParams.Params, providerCode, serializerCode);

            this.agentId = data.GeneralData?.GetAs<string>("AgentId") ?? string.Empty;
            this.agentName = data.GeneralData?.GetAs<string>("AgentName") ?? string.Empty;
            this.importTypeDisplay = data.GeneralData?.GetAs<string>("importType") ?? string.Empty;

            // OverrideFileInfoKey - такие данные должен устанавливать сериализатор меняющий идентификатор fileNumber 
            // который используется как ключь импорта
            var overrideFileInfo = data.GeneralData != null && data.GeneralData.Contains(OverrideFileInfoKey) &&
                data.GeneralData.GetAs<bool>(OverrideFileInfoKey);

            var dateNumberAssigned = false;
            if (!map.Format.Equals("dbf", StringComparison.InvariantCultureIgnoreCase) || overrideFileInfo)
            {
                this.fileDate = data.FileDate;
                this.fileNumber = data.FileNumber;
                dateNumberAssigned = true;
            }

            foreach (var importRow in data.Rows)
            {
                if (importRow.Error.IsNotEmpty())
                {
                    this.logImport.Error(importRow.Title, importRow.Error);
                    continue;
                }

                if (importRow.Warning.IsNotEmpty())
                {
                    this.logImport.Warn(importRow.Title, importRow.Warning);
                }

                if (importRow.Info.IsNotEmpty())
                {
                    this.logImport.Info(importRow.Title, importRow.Info);
                }

                if (importRow.Value == null)
                {
                    continue;
                }

                if (!dateNumberAssigned)
                {
                    this.fileDate = importRow.Value.DocumentDate;
                    this.fileNumber = importRow.Value.DocumentNumber;
                    dateNumberAssigned = true;
                }

                yield return importRow.Value;
            }
        }

        private void LogFileFormatError(string msg = "Невозможно прочитать файл. Неверный формат файла.")
        {
            this.logImport.Error("Исключение", msg);
        }
    }
}