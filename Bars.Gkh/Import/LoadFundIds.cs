namespace Bars.Gkh.Import
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums.Import;
    using Bars.Gkh.Import;
    using Bars.B4.Logging;

    using Castle.Windsor;
    using Gkh.Import.Impl;

    public class LoadFundIds : GkhImportBase
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
            get { return "FundImport"; }
        }

        public override string Name
        {
            get { return "Загрузка идентификаторов Фонда"; }
        }

        public override string PossibleFileExtensions
        {
            get { return "csv"; }
        }

        public override string PermissionName
        {
            get { return "Import.LoadFundIdsImport.View"; }
        }

        /// <summary>
        /// Менеджер управляющий логами
        /// </summary>
        public ILogImportManager LogManager { get; set; }

        public override ImportResult Import(BaseParams baseParams)
        {
            var servRealityObject = Container.Resolve<IDomainService<RealityObject>>();
            var message = string.Empty;

            var fileData = baseParams.Files["FileImport"];
            InitLog(fileData.FileName);

            var realtyObjects = servRealityObject.GetAll()
                  .Where(x => x.FiasAddress.StreetCode != null)
                  .Select(x => new { x.Id, x.FiasAddress.StreetCode, x.FiasAddress.House, x.FiasAddress.Housing })
                  .AsEnumerable()
                  .GroupBy(x => x.StreetCode)
                  .ToDictionary(x => x.Key, x => x.ToList());

            var findRealtyObjects = new Dictionary<long, string>();

            using (var memoryStreamFile = new MemoryStream(fileData.Data))
            {
                memoryStreamFile.Seek(0, SeekOrigin.Begin);
                var reader = new StreamReader(memoryStreamFile, Encoding.GetEncoding(1251));
                var headers = reader.ReadLine().ToStr().Split(';').Select(x => x.Trim('"')).ToArray();

                var indexFederalNum = -1;
                var indexCodeKladrStreet = -1;
                var indexHouse = -1;
                var indexHousing = -1;
                for (var i = 0; i < headers.Length; i++)
                {
                    var header = headers[i];
                    switch (header.ToUpper())
                    {
                        case "ID_MKD":
                            indexFederalNum = i;
                            break;
                        case "KLADR_STREET":
                            indexCodeKladrStreet = i;
                            break;
                        case "NUM":
                            indexHouse = i;
                            break;
                        case "KORPUS":
                            indexHousing = i;
                            break;
                    }
                }

                while (true)
                {
                    var data = reader.ReadLine().ToStr().Split(';').Select(x => x.Trim('"')).ToArray();
                    if (data.Length <= 1)
                    {
                        break;
                    }

                    var federalNum = data[indexFederalNum];
                    var codeKladrStreet = data[indexCodeKladrStreet];
                    var house = data[indexHouse];
                    var housing = data[indexHousing];

                    if (string.IsNullOrEmpty(codeKladrStreet) || codeKladrStreet.Length != 17)
                    {
                        logImport.Warn(Name, string.Format("Не верный код КЛАДРа улицы {0}", codeKladrStreet));
                        continue;
                    }

                    if (string.IsNullOrEmpty(house))
                    {
                        logImport.Warn(Name, string.Format("Не задан номер дома {0}", codeKladrStreet));
                        continue;
                    }

                    var codeKladr = codeKladrStreet.Substring(0, 15);

                    if (!realtyObjects.ContainsKey(codeKladr))
                    {
                        logImport.Warn(Name, string.Format("Не найден объект {0}", codeKladrStreet));
                        continue;
                    }

                    var result = realtyObjects[codeKladr];
                    if (!string.IsNullOrEmpty(house) && !string.IsNullOrEmpty(housing))
                    {
                        result = result.Where(x => x.House == house.Trim() && x.Housing == housing.Trim()).ToList();
                    }
                    else if (!string.IsNullOrEmpty(house) && string.IsNullOrEmpty(housing))
                    {
                        result = result.Where(x => x.House == house.Trim() && string.IsNullOrEmpty(x.Housing)).ToList();
                    }
                   
                    if (result.Count == 0)
                    {
                        logImport.Warn(Name, string.Format("Не найден объект {0}", codeKladrStreet));
                        continue;
                    }

                    if (result.Count > 1)
                    {
                       logImport.Warn(Name, string.Format("{0} - найдено {1} объектов. ", codeKladrStreet, result.Count));
                        continue;
                    }
                                   
                    var home = result.First();

                    if (findRealtyObjects.ContainsKey(home.Id))
                    {
                        logImport.Warn("Дублирующаяся запись. {0}.", federalNum);

                        // предыдущий тоже не грузим
                        findRealtyObjects[home.Id] = null;
                        continue;
                    }

                    findRealtyObjects.Add(home.Id, federalNum);
                }
            }

            using (var transaction = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    foreach (var findRealtyObject in findRealtyObjects.Where(x => x.Value != null))
                    {
                        var realityObj = servRealityObject.Load(findRealtyObject.Key);
                        realityObj.FederalNum = findRealtyObject.Value;
                        servRealityObject.Update(realityObj);
                        logImport.Info("Обновлен объект с адресом {0}", realityObj.Address);
                    }
                }
                catch (Exception exc)
                {
                    try
                    {
                        logImport.IsImported = false;
                        Container.Resolve<ILogManager>().Error("Импорт", exc);
                        message = "Произошла неизвестная ошибка. Обратитесь к администратору";
                        logImport.Error(Name, "Произошла неизвестная ошибка. Обратитесь к администратору.");

                        transaction.Rollback();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message, exc);
                    }
                }
            }

            LogManager.Add(fileData, logImport);
            LogManager.Save();

            message += LogManager.GetInfo();
            var status = LogManager.CountError > 0 ? StatusImport.CompletedWithError : (LogManager.CountWarning > 0 ? StatusImport.CompletedWithWarning : StatusImport.CompletedWithoutError);
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
                if (headers.Length == 0)
                {
                    message = "Загаловок файла не соответствует формату";
                    return false;
                }

                var indexIdMkd = -1;
                var indexCodeKladrStreet = -1;
                var indexNumStreet = -1;
                var indexBlockStreet = -1;
                for (var i = 0; i < headers.Length; i++)
                {
                    var header = headers[i];
                    switch (header.ToUpper())
                    {
                        case "ID_MKD":
                            indexIdMkd = i;
                            break;
                        case "KLADR_STREET":
                            indexCodeKladrStreet = i;
                            break;
                        case "NUM":
                            indexNumStreet = i;
                            break;
                        case "KORPUS":
                            indexBlockStreet = i;
                            break;
                    }
                }

                if (indexIdMkd == -1)
                {
                    message = "Не найден столбец \"ID_MKD\"";
                    return false;
                }

                if (indexCodeKladrStreet == -1)
                {
                   message = "Не найден столбец \"KLADR_STREET\"";
                   return false;
                }

                if (indexNumStreet == -1)
                {
                    message = "Не найден заголовок \"NUM\"";
                    return false;
                }

                if (indexBlockStreet == -1)
                {
                    message = "Не найден заголовок \"KORPUS\"";
                    return false;
                }
            }

            return true;
        }

        public void InitLog(string fileName)
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
    }
}
