namespace Bars.GkhGji.Regions.Voronezh.Import
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums.Import;
    using Bars.Gkh.Import;
    using Bars.GkhExcel;
    using Bars.GkhGji.Regions.Voronezh.Entities.ASFK;
    using Bars.GkhGji.Regions.Voronezh.Enums;
    using Castle.Windsor;
    using Gkh.Import.Impl;
    using Ionic.Zip;

    /// <summary>
    /// Импорт файлов Федерального казначейства.
    /// </summary>
    public sealed class ASFKImport : GkhImportBase
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        /// <summary>
        /// Ключ импорта
        /// </summary>
        public override string Key
        {
            get { return Id; }
        }

        /// <summary>
        /// Код группировки импорта (например группировка в меню)
        /// </summary>
        public override string CodeImport
        {
            get { return "ASFKImport"; }
        }

        /// <summary>
        /// Наименование импорта
        /// </summary>
        public override string Name
        {
            get { return "Импорт из Федерального казначейства"; }
        }

        /// <summary>
        /// Разрешенные расширения файлов
        /// </summary>
        public override string PossibleFileExtensions
        {
            get { return "zip"; }
        }

        /// <summary>
        /// Права
        /// </summary>
        public override string PermissionName
        {
            get { return "Import.ASFK.View"; }
        }    

        /// <summary>
        /// Импорт данных
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public override ImportResult Import(BaseParams baseParams)
        {
            var fileImport = baseParams.Files["FileImport"];
            var asfkDomain = Container.Resolve<IDomainService<ASFK>>();

            try
            {
                var zipfileMemoryStream = new MemoryStream(fileImport.Data);
                var zipFile = ZipFile.Read(zipfileMemoryStream);
                var zipEntries = zipFile.ToArray();

                if (zipEntries.Length < 1)
                {
                    return new ImportResult(StatusImport.CompletedWithError, "Отсутствуют файлы для импорта");
                }

                var NewASFK = new ASFK();
                List<string> stringsVtList = new List<string>();
                List<string> stringsBdList = new List<string>();

                var vtFile = zipEntries.FirstOrDefault(x => x.FileName.ToLower().Contains(".vt"));
                if (vtFile != null)
                {
                    var ms = new MemoryStream();
                    vtFile.Extract(ms);
                    ms.Seek(0, SeekOrigin.Begin);
                    var reader = new StreamReader(ms, Encoding.GetEncoding(1251));
                    List<string> strList = new List<string>();
                    string[] lines = reader.ReadToEnd().Split('|');
                    foreach (string line in lines)
                    {
                        strList.Add(line);
                        stringsVtList.Add(line);
                    }
                    string[] strings = strList.ToArray();

                    NewASFK.NumVer = strings[1];
                    NewASFK.Former = strings[2];
                    NewASFK.FormVer = strings[3];
                    NewASFK.NormDoc = strings[4];
                    NewASFK.KodTofkFrom = strings[6];
                    NewASFK.NameTofkFrom = strings[7];
                    NewASFK.BudgetLevel = GetBudgetLevel(strings[9]);
                    NewASFK.KodUbp = strings[10];
                    NewASFK.NameUbp = strings[11];
                    NewASFK.GuidVT = Guid.Parse(strings[13]);
                    NewASFK.LsAdb = strings[14];
                    NewASFK.DateOtch = Convert.ToDateTime(strings[15]);
                    NewASFK.DateOld = Convert.ToDateTime(strings[16]);
                    NewASFK.VidOtch = GetReportType(strings[17]);
                    NewASFK.KodTofkVT = strings[18];
                    NewASFK.NameTofkVT = strings[19];
                    NewASFK.KodUbpAdb = strings[20];
                    NewASFK.NameUbpAdb = strings[21];
                    NewASFK.KodGadb = strings[22];
                    NewASFK.NameGadb = strings[23];
                    NewASFK.NameBud = strings[24];
                    NewASFK.Oktmo = strings[25];
                    NewASFK.OkpoFo = strings[26];
                    NewASFK.NameFo = strings[27];
                    NewASFK.DolIsp = strings[28];
                    NewASFK.NameIsp = strings[29];
                    NewASFK.TelIsp = strings[30];
                    NewASFK.DatePod = Convert.ToDateTime(strings[31]);
                    NewASFK.SumInItogV = Convert.ToDecimal(strings[32], CultureInfo.InvariantCulture);
                    NewASFK.SumOutItogV = Convert.ToDecimal(strings[33], CultureInfo.InvariantCulture);
                    NewASFK.SumZachItogV = Convert.ToDecimal(strings[34], CultureInfo.InvariantCulture);
                    NewASFK.SumNOutItogV = Convert.ToDecimal(strings[35], CultureInfo.InvariantCulture);
                    NewASFK.SumNZachItogV = Convert.ToDecimal(strings[36], CultureInfo.InvariantCulture);
                    NewASFK.SumBeginIn = Convert.ToDecimal(strings[38], CultureInfo.InvariantCulture);
                    NewASFK.SumBeginOut = Convert.ToDecimal(strings[39], CultureInfo.InvariantCulture);
                    NewASFK.SumBeginZach = Convert.ToDecimal(strings[40], CultureInfo.InvariantCulture);
                    NewASFK.SumBeginNOut = Convert.ToDecimal(strings[41], CultureInfo.InvariantCulture);
                    NewASFK.SumBeginNZach = Convert.ToDecimal(strings[42], CultureInfo.InvariantCulture);
                    NewASFK.SumEndIn = Convert.ToDecimal(strings[43], CultureInfo.InvariantCulture);
                    NewASFK.SumEndOut = Convert.ToDecimal(strings[44], CultureInfo.InvariantCulture);
                    NewASFK.SumEndZach = Convert.ToDecimal(strings[45], CultureInfo.InvariantCulture);
                    NewASFK.SumEndNOut = Convert.ToDecimal(strings[46], CultureInfo.InvariantCulture);
                    NewASFK.SumEndNZach = Convert.ToDecimal(strings[47], CultureInfo.InvariantCulture);
                }

                var bdFile = zipEntries.FirstOrDefault(x => x.FileName.ToLower().Contains(".bd"));
                if (bdFile != null)
                {
                    var ms = new MemoryStream();
                    bdFile.Extract(ms);
                    ms.Seek(0, SeekOrigin.Begin);
                    var reader = new StreamReader(ms, Encoding.GetEncoding(1251));
                    List<string> strList = new List<string>();
                    string[] lines = reader.ReadToEnd().Split('|');
                    foreach (string line in lines)
                    {
                        strList.Add(line);
                        stringsBdList.Add(line);
                    }
                    string[] strings = strList.ToArray();
                }

                asfkDomain.Save(NewASFK);

                long asfkId = asfkDomain.GetAll().OrderByDescending(x => x.ObjectCreateDate).First().Id;

                CreateVTOPERs(stringsVtList.ToArray(), asfkId, asfkDomain);
                CreateBDOPERs(stringsBdList.ToArray(), asfkId, asfkDomain);

                return new ImportResult(
                    StatusImport.CompletedWithoutError,
                    string.Format("Запись успешно имортирована"),
                    string.Empty);
            }
            catch (Exception ex)
            {
                return new ImportResult(
                            StatusImport.CompletedWithError,
                            string.Format(ex.Message),
                            string.Empty);
            }
            finally
            {
                Container.Release(asfkDomain);
            }
        }

        /// <summary>
        /// Первоночальная валидация файла перед импортом
        /// </summary>
        /// <param name="baseParams"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public override bool Validate(BaseParams baseParams, out string message)
        {
            message = null;
            if (!baseParams.Files.ContainsKey("FileImport"))
            {
                message = "Не выбран файл для импорта";
                return false;
            }

            var fileData = baseParams.Files["FileImport"];
            var extention = fileData.Extention;

            var fileExtentions = PossibleFileExtensions.Contains(",")
                ? PossibleFileExtensions.Split(',')
                : new[] { PossibleFileExtensions };
            if (fileExtentions.All(x => x != extention))
            {
                message = string.Format("Необходимо выбрать файл с допустимым расширением: {0}", PossibleFileExtensions);
                return false;
            }

            return true;
        }

        public void CreateVTOPERs(string[] strings, long asfkId, IDomainService asfkDomain)
        {
            var vtoperDomain = Container.Resolve<IDomainService<VTOPER>>();
            try
            {
                for (int i = 0; i < strings.Length; i++)
                {
                    if (strings[i].Contains("VTOPER"))
                    {
                        var guid = vtoperDomain.GetAll()
                            .Where(x => x.GUID == strings[i + 1])
                            .Select(x => x.GUID)
                            .FirstOrDefault();

                        if (guid == null)
                        {
                            vtoperDomain.Save(new VTOPER
                            {
                                ASFK = (ASFK)asfkDomain.Get(asfkId),
                                GUID = strings[i + 1],
                                KodDoc = GetConfirmingDocCode(strings[i + 2]),
                                NomDoc = strings[i + 3],
                                DateDoc = Convert.ToDateTime(strings[i + 4]),
                                KodDocAdb = GetADBDocCode(strings[i + 5]),
                                NomDocAdb = strings[i + 6],
                                DateDocAdb = ConvertToDateTimeWithNullCheck(strings[i + 7]),
                                SumIn = Convert.ToDecimal(strings[i + 8], CultureInfo.InvariantCulture),
                                SumOut = Convert.ToDecimal(strings[i + 9], CultureInfo.InvariantCulture),
                                SumZach = Convert.ToDecimal(strings[i + 10], CultureInfo.InvariantCulture),
                                Note = strings[i + 11],
                                TypeKbk = GetKBKType(strings[i + 12]),
                                Kbk = strings[i + 13],
                                AddKlass = strings[i + 14],
                                Okato = strings[i + 15],
                                InnAdb = strings[i + 16],
                                KppAdb = strings[i + 17]
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var debug = ex;
            }
            finally
            {
                Container.Release(vtoperDomain);
            }
        }

        public void CreateBDOPERs(string[] strings, long asfkId, IDomainService asfkDomain)
        {
            var bdoperDomain = Container.Resolve<IDomainService<BDOPER>>();
            try
            {
                for (int i = 0; i < strings.Length; i++)
                {
                    if (strings[i].Contains("BDPD") && !strings[i].Contains("ST") && !strings[i].Contains("CONTR"))
                    {
                        var guid = bdoperDomain.GetAll()
                            .Where(x => x.GUID == strings[i + 46])
                            .Select(x => x.GUID)
                            .FirstOrDefault();

                        if (guid == null)
                        {
                            bdoperDomain.Save(new BDOPER
                            {
                                ASFK = (ASFK)asfkDomain.Get(asfkId),
                                GUID = strings[i + 46],
                                Sum = Convert.ToDecimal(strings[i + 6], CultureInfo.InvariantCulture),
                                InnPay = strings[i + 11],
                                KppPay = strings[i + 12],
                                NamePay = strings[i + 13],
                                Purpose = strings[i + 30]
                            });
                        }
                    }
                    else if (strings[i].Contains("BDPL") && !strings[i].Contains("ST") && !strings[i].Contains("CONTR"))
                    {
                        var guid = bdoperDomain.GetAll()
                            .Where(x => x.GUID == strings[i + 1])
                            .Select(x => x.GUID)
                            .FirstOrDefault();

                        if (guid == null)
                        {
                            bdoperDomain.Save(new BDOPER
                            {
                                ASFK = (ASFK)asfkDomain.Get(asfkId),
                                GUID = strings[i + 1],
                                Sum = Convert.ToDecimal(strings[i + 6], CultureInfo.InvariantCulture),
                                InnPay = strings[i + 15],
                                KppPay = strings[i + 16],
                                NamePay = strings[i + 12],
                                Purpose = strings[i + 9]
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var debug = ex;
            }
            finally
            {
                Container.Release(bdoperDomain);
            }
        }

        public static DateTime? ConvertToDateTimeWithNullCheck(string str)
        {
            if (str != null && str != string.Empty)
                return Convert.ToDateTime(str);
            else
                return null;
        }

        public string ADBDocCodeToString(ASFKADBDocCode adbDocCode)
        {
            if (adbDocCode == ASFKADBDocCode.NoValue)
            {
                return "";
            }
            else return ((int)adbDocCode).ToString();
        }

        public ASFKBudgetLevel GetBudgetLevel(string str)
        {
            switch (str)
            {
                case "1":
                    return ASFKBudgetLevel.Federal;
                case "2":
                    return ASFKBudgetLevel.SubjectRF;
                case "3":
                    return ASFKBudgetLevel.Local;
                case "4":
                    return ASFKBudgetLevel.GVFRF;
                case "5":
                    return ASFKBudgetLevel.TGVF;
                default:
                    return ASFKBudgetLevel.Federal;
            }
        }

        public ASFKReportType GetReportType(string str)
        {
            switch (str)
            {
                case "0":
                    return ASFKReportType.FinalOrNull;
                case "1":
                    return ASFKReportType.Intermediate;
                default:
                    return ASFKReportType.FinalOrNull;
            }
        }

        public ASFKConfirmingDocCode GetConfirmingDocCode(string str)
        {
            switch (str)
            {
                case "XX":
                    return ASFKConfirmingDocCode.XX;
                case "IZ":
                    return ASFKConfirmingDocCode.IZ;
                case "PP":
                    return ASFKConfirmingDocCode.PP;
                case "PL":
                    return ASFKConfirmingDocCode.PL;
                case "SF":
                    return ASFKConfirmingDocCode.SF;
                case "UF":
                    return ASFKConfirmingDocCode.UF;
                case "UN":
                    return ASFKConfirmingDocCode.UN;
                case "SP":
                    return ASFKConfirmingDocCode.SP;
                case "RC":
                    return ASFKConfirmingDocCode.RC;
                case "RD":
                    return ASFKConfirmingDocCode.RD;
                case "UZ":
                    return ASFKConfirmingDocCode.UZ;
                default:
                    return ASFKConfirmingDocCode.XX;
            }
        }

        public ASFKADBDocCode GetADBDocCode(string str)
        {
            switch (str)
            {
                case "XX":
                    return ASFKADBDocCode.XX;
                case "ZV":
                    return ASFKADBDocCode.ZV;
                case "UF":
                    return ASFKADBDocCode.UF;
                case "UN":
                    return ASFKADBDocCode.UN;
                case "UM":
                    return ASFKADBDocCode.UM;
                default:
                    return ASFKADBDocCode.NoValue;
            }
        }

        public ASFKKBKType GetKBKType(string str)
        {
            switch (str)
            {
                case "20":
                    return ASFKKBKType.Income;
                case "31":
                    return ASFKKBKType.IVFDB;
                case "32":
                    return ASFKKBKType.IVnFDB;
                default:
                    return ASFKKBKType.Income;
            }
        }
    }
}