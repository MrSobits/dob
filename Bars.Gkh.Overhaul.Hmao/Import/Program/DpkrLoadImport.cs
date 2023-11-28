﻿namespace Bars.Gkh.Overhaul.Hmao.Import.Program
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using B4.Logging;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums.Import;
    using Bars.Gkh.Import;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Castle.Windsor;
    using Gkh.Import.Impl;

    /// <summary>
    /// Импорт программы капитального ремонта. В Новосибирске уже была подписана программа и они 
    ///     хотят ее видеть в результате на сервисе.
    /// </summary>
    public sealed class DpkrLoadImport : GkhImportBase
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        private ILogImport logImport;

        public IWindsorContainer Container { get; set; }

        public override string Key
        {
            get { return Id; }
        }

        public override string CodeImport
        {
            get { return "DpkrLoad"; }
        }

        public override string Name
        {
            get { return "Импорт программы капитального ремонта"; }
        }

        public override string PossibleFileExtensions
        {
            get { return "csv"; }
        }

        public override string PermissionName
        {
            get { return "Import.DpkrLoad.View"; }
        }

        /// <summary>
        /// Менеджер управляющий логами
        /// </summary>
        public ILogImportManager LogManager { get; set; }

        public override ImportResult Import(BaseParams baseParams)
        {
            var message = string.Empty;
            var fileData = baseParams.Files["FileImport"];

            InitLog(fileData.FileName);

            using (var memoryStreamFile = new MemoryStream(fileData.Data))
            {
                memoryStreamFile.Seek(0, SeekOrigin.Begin);
                var reader = new StreamReader(memoryStreamFile, Encoding.GetEncoding(1251));
                var headers = reader.ReadLine().ToStr().Split(';').Select(x => x.Trim('"')).ToArray();

                if (headers.Length < 10)
                {
                    throw new Exception("Неверный формат файла");
                }

                List<LoadProgram> listLoadProgram = null;
                try
                {
                    listLoadProgram = this.ProcessData(reader);
                }
                catch (Exception exc)
                {
                    try
                    {
                        logImport.IsImported = false;
                        Container.Resolve<ILogManager>().Error("Импорт", exc);

                        logImport.Error(
                            string.Format("{0}.", Name),
                            "Произошла неизвестная ошибка. Обратитесь к администратору.");
                        message = "Произошла неизвестная ошибка. Обратитесь к администратору";
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message, exc);
                    }
                }
                finally
                {
                    memoryStreamFile.Close();
                }

                var session = Container.Resolve<ISessionProvider>().GetCurrentSession(); 
                using (var transaction = session.BeginTransaction())
                {

                    try
                    {
                        if (listLoadProgram != null)
                        {
                            // Перед загрузкой удаляем показатели
                            session.CreateSQLQuery("DELETE FROM OVRHL_LOADED_PROGRAM").ExecuteUpdate();

                            listLoadProgram.ForEach(x => session.Save(x));
                            transaction.Commit();

                            logImport.IsImported = true;
                            logImport.CountAddedRows = listLoadProgram.Count;
                        }
                    }
                    catch (Exception exc)
                    {
                        try
                        {
                            logImport.IsImported = false;
                            Container.Resolve<ILogManager>().Error("Импорт", exc);

                            logImport.Error(string.Format("{0}.", Name), "Произошла неизвестная ошибка. Обратитесь к администратору.");
                            message = "Произошла неизвестная ошибка. Обратитесь к администратору";
                            transaction.Rollback();
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(ex.Message, exc);
                        }
                    }
                }
            }

            LogManager.Add(fileData, logImport);
            message += LogManager.GetInfo();
            LogManager.Save();

            var status = !logImport.IsImported ? StatusImport.CompletedWithError : (logImport.CountWarning > 0 ? StatusImport.CompletedWithWarning : StatusImport.CompletedWithoutError);
            return new ImportResult(status, message, string.Empty, LogManager.LogFileId);
        }

        public override bool Validate(BaseParams baseParams, out string message)
        {
            message = null;

            if (!baseParams.Files.ContainsKey("FileImport"))
            {
                message = "Не выбран файл для импорта";
                return false;
            }

            var fileData = baseParams.Files["FileImport"];
            var extention = baseParams.Files["FileImport"].Extention;

            var fileExtentions = PossibleFileExtensions.Contains(",") ? PossibleFileExtensions.Split(',') : new[] { PossibleFileExtensions };
            if (fileExtentions.All(x => x != extention))
            {
                message = string.Format("Необходимо выбрать файл с допустимым расширением: {0}", PossibleFileExtensions);
                return false;
            }

            using (var memoryStreamFile = new MemoryStream(fileData.Data))
            {
                memoryStreamFile.Seek(0, SeekOrigin.Begin);
                var reader = new StreamReader(memoryStreamFile, Encoding.GetEncoding(1251));
                var headers = reader.ReadLine().ToStr().Split(';').Select(x => x.Trim('"')).ToArray();
                if (headers.Length < 10)
                {
                    message = "Загаловок файла не соответствует формату";
                    return false;
                }
            }

            return true;
        }

        private void InitLog(string fileName)
        {
            LogManager = Container.Resolve<ILogImportManager>();
            if (LogManager == null)
            {
                throw new Exception("Не найдена реализация интерфейса ILogImportManager");
            }

            LogManager.FileNameWithoutExtention = fileName;
            LogManager.UploadDate = DateTime.Now;

            logImport = Container.ResolveAll<ILogImport>().FirstOrDefault(x => x.Key == MainLogImportInfo.Key);
            if (logImport == null)
            {
                throw new Exception("Не найдена реализация интерфейса ILogImport");
            }

            logImport.SetFileName(fileName);
            logImport.ImportKey = Key;
        }

        private List<LoadProgram> ProcessData(StreamReader reader)
        {
            var resultList = new List<LoadProgram>();

            while (true)
            {
                var data = reader.ReadLine().ToStr().Split(';').Select(x => x.Trim('"')).ToArray();
                if (data.Length <= 1)
                {
                    break;
                }

                var indexNumber = data[0].To<int>();
                var locality = data[1];
                var street = data[2];
                var house = data[3];
                var housing = data[4];
                var address = string.Format(
                    "{0}, {1}, д. {2}{3}",
                    locality,
                    street,
                    house,
                    housing ?? " , корп." + housing
                    );

                var loadProgram = new LoadProgram
                {
                    IndexNumber = indexNumber,
                    Locality = locality,
                    Street = street,
                    House = house,
                    Housing = housing,
                    Address = address,
                    CommissioningYear = data[5].To<int>(),
                    CommonEstateobject = data[6],
                    Wear = data[7],
                    LastOverhaulYear = data[8].To<int>(),
                    PlanOverhaulYear = data[9].To<int>()
                };

                resultList.Add(loadProgram);
            }

            return resultList;
        }
    }
}
