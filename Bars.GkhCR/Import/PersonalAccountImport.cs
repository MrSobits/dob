namespace Bars.GkhCr.Import
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Enums.Import;
    using Bars.Gkh.Import;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;
    using Bars.GkhExcel;
    using Bars.B4.Logging;

    using Castle.Windsor;
    using Gkh.Import.Impl;

    public class PersonalAccountImport : GkhImportBase
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        private ILogImport logImport;

        public virtual IWindsorContainer Container { get; set; }

        public override string Key
        {
            get { return Id; }
        }

        public override string CodeImport
        {
            get { return "ObjectCr"; }
        }

        public override string Name
        {
            get { return "Импорт лицевых счетов"; }
        }

        public override string PossibleFileExtensions
        {
            get { return "xls"; }
        }

        public override string PermissionName
        {
            get { return "Import.PersonalAccount.View"; }
        }

        /// <summary>
        /// Менеджер управляющий логами
        /// </summary>
        public ILogImportManager LogManager { get; set; }

        public override ImportResult Import(BaseParams baseParams)
        {
           var message = string.Empty;

            var fileData = baseParams.Files["FileImport"];
            var programId = baseParams.Params["ProgramCr"].ToInt();

            InitLog(fileData.FileName);

            using (var transaction = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    using (var memoryStreamFile = new MemoryStream(fileData.Data))
                    {
                        using (var excel = Container.Resolve<IGkhExcelProvider>("ExcelEngineProvider"))
                        {
                            if (excel == null)
                            {
                                throw new Exception("Не найдена реализация интерфейса IGkhExcelProvider");
                            }

                            excel.Open(memoryStreamFile);

                            foreach (var row in excel.GetRows(0, 0).Where(row => row.Length >= 4 && !row[0].FontBold && !string.IsNullOrEmpty(row[1].Value)))
                            {
                                var serPersonalAccount = Container.Resolve<IDomainService<PersonalAccount>>();
                                var serObjectCr = Container.Resolve<IDomainService<ObjectCr>>();
                                ImportRow(row, programId, serPersonalAccount, serObjectCr);
                            }

                            transaction.Commit();
                            logImport.IsImported = true;
                        }
                    }
                }
                catch (Exception exp)
                {
                    try
                    {
                        logImport.IsImported = false;
                        Container.Resolve<ILogManager>().Error("Импорт", exp);
                        message = "Произошла неизвестная ошибка";
                        logImport.Error(Name, "Произошла неизвестная ошибка. Обратитесь к администратору");

                        transaction.Rollback();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message, exp);
                    }
                }
            }

            // добавляем лог в logManager для последующего сохранения в системе
            LogManager.Add(fileData, logImport);
            message += LogManager.GetInfo();
            LogManager.Save();

            var status = !logImport.IsImported ? StatusImport.CompletedWithError : (logImport.CountWarning > 0 ? StatusImport.CompletedWithWarning : StatusImport.CompletedWithoutError);
            return new ImportResult(status, message, string.Empty, LogManager.LogFileId);
        }

        public override bool Validate(BaseParams baseParams, out string message)
        {
            try
            {
                message = null;
                if (!baseParams.Files.ContainsKey("FileImport"))
                {
                    message = "Не выбран файл для импорта";
                    return false;
                }

                var bytes = baseParams.Files["FileImport"].Data;
                var extention = baseParams.Files["FileImport"].Extention;

                var fileExtentions = PossibleFileExtensions.Contains(",") ? PossibleFileExtensions.Split(',') : new[] { PossibleFileExtensions };
                if (fileExtentions.All(x => x != extention))
                {
                    message = string.Format("Необходимо выбрать файл с допустимым расширением: {0}", PossibleFileExtensions);
                    return false;
                }

                using (var memoryStreamFile = new MemoryStream(bytes))
                {
                    using (var excel = Container.Resolve<IGkhExcelProvider>("ExcelEngineProvider"))
                    {
                        if (excel == null)
                        {
                            throw new Exception("Не найдена реализация интерфейса IGkhExcelProvider");
                        }

                        excel.Open(memoryStreamFile);

                        if (excel.IsEmpty(0, 0))
                        {
                            message = string.Format("Не удалось обнаружить записи в файле: {0}", PossibleFileExtensions);
                            return false;
                        }

                        var title = excel.GetRows(0, 0)[0];

                        if (title[0].Value.Trim() != "Муниципальное образование" || title[1].Value.Trim() != "Адрес объекта"
                            || title[2].Value.Trim() != "Лицевой счет объекта по 185 ФЗ"
                            || title[3].Value.Trim() != "Лицевой счет на другие разрезы финансирования")
                        {
                            message = "Заголовки столбцов не соответствуют шаблону";
                            return false;
                        }

                        return true;
                    }
                }
            }
            catch (Exception exp)
            {
                Container.Resolve<ILogManager>().Error("Валидация файла импорта", exp);
                message = "Произошла неизвестная ошибка при проверки формата файла";
                return false;
            }
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

        private void ImportRow(GkhExcelCell[] row, int programId, IDomainService<PersonalAccount> serPersonalAccount, IDomainService<ObjectCr> serObjectCr)
        {
            var region = row[0].Value;
            var address = row[1].Value;

            var for185F3 = row[2].Value;
            var forOtherResurcesFin = row[3].Value;

            var objectCr = serObjectCr.GetAll().FirstOrDefault(x => x.ProgramCr.Id == programId && x.RealityObject.Address == address && x.RealityObject.Municipality.Name == region);
            if (objectCr != null)
            {
                var realtyObjName = objectCr.RealityObject.Address;

                if (!string.IsNullOrWhiteSpace(for185F3))
                {
                    var perAccountFor185F3 = serPersonalAccount.GetAll()
                        .FirstOrDefault(x => x.ObjectCr.Id == objectCr.Id && x.Account == for185F3);

                    if (perAccountFor185F3 == null)
                    {
                        perAccountFor185F3 = new PersonalAccount
                            {
                                Closed = false,
                                Account = for185F3,
                                FinanceGroup = TypeFinanceGroup.ProgramCr,
                                ObjectCr = objectCr
                            };

                        serPersonalAccount.Save(perAccountFor185F3);

                        var msg = string.Format("Добавлен лицевой счет {0} в объект КР {1} с группой финансирования Программа КР", for185F3, realtyObjName);
                        logImport.Info(Name, msg, LogTypeChanged.Added);
                    }
                    else
                    {
                        var msg = string.Format("Указанный лицевой счет {0} с группой финансирования Программа КР, уже внесен в объект КР {1}", for185F3, realtyObjName);
                        logImport.Info(Name, msg);
                    }
                }

                if (!string.IsNullOrWhiteSpace(forOtherResurcesFin))
                {
                    var perAccountForOtherResurcesFin = serPersonalAccount.GetAll()
                        .FirstOrDefault(x => x.ObjectCr.Id == objectCr.Id && x.Account == forOtherResurcesFin);
                    
                    if (perAccountForOtherResurcesFin == null)
                    {
                        perAccountForOtherResurcesFin = new PersonalAccount
                            {
                                Closed = false,
                                Account = forOtherResurcesFin,
                                FinanceGroup = TypeFinanceGroup.Other,
                                ObjectCr = objectCr
                            };

                        serPersonalAccount.Save(perAccountForOtherResurcesFin);

                        var msg = string.Format("Добавлен лицевой счет {0} в объект КР {1} с группой финансирования Другие", forOtherResurcesFin, realtyObjName);
                        logImport.Info(Name, msg, LogTypeChanged.Added);
                    }
                    else
                    {
                        var msg = string.Format("Указанный лицевой счет {0} с группой финансирования Другие, уже внесен в объект КР {1}", forOtherResurcesFin, realtyObjName);
                        logImport.Info(Name, msg);
                    }
                }
            }
            else
            {
                var msg = string.Format("Адрес дома {0} или район {1}, введен неправильно или дом не попадает в выбранную программу капитального ремонта", address, region);
                logImport.Warn(Name, msg);
            }
        }
    }
}
