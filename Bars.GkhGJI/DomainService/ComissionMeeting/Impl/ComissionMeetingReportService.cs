namespace Bars.GkhGji.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    using B4;
    using B4.DataAccess;
    using B4.IoC;
    using B4.Modules.FileStorage;
    using B4.Utils;

    using Bars.Gkh.Config;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Enums;
    using Bars.Gkh.TextValues;
    using Bars.Gkh.Utils;

    using Castle.Windsor;
    using Ionic.Zip;
    using Ionic.Zlib;
    using Bars.Gkh.Report;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Controllers;
    using Bars.B4.Modules.Reports;
    using Bars.Gkh.StimulReport;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Services.DataContracts.GetDocument;
    using static System.Net.Mime.MediaTypeNames;
    using Microsoft.Office.Interop.Word;

    /// <summary>
    /// Сервис для работы с претензиями неплательщиков
    /// </summary>
    public class ComissionMeetingReportService : IComissionMeetingReportService
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        public IMenuItemText MenuItemText { get; set; }

        public IGkhConfigProvider ConfigProv { get; set; }

        /// <summary>
        /// Вернуть список отчётов
        /// </summary>
        /// <param name="baseParams">Базовые параметры запросы</param>
        /// <returns>Результат операции</returns>
        public IList<ReportInfo> GetResolutionReportList(BaseParams baseParams)
        {
            var reports = this.Container.ResolveAll<IComissionMeetingCodedReport>();

            try
            {

                return reports.Where(x => x.CodeForm == "Resolution")
                .Select(x => new ReportInfo
                {
                    Id = x.ReportId,
                    Name = this.MenuItemText.GetText(x.Name),
                    Description = this.MenuItemText.GetText(x.Description)
                })
                .ToList();               

            }
            finally
            {
                foreach (var report in reports)
                {
                    this.Container.Release(report);
                }
            }
        }

        /// <summary>
        /// Вернуть список отчётов
        /// </summary>
        /// <param name="baseParams">Базовые параметры запросы</param>
        /// <returns>Результат операции</returns>
        public IList<ReportInfo> GetProtocol2025ReportList(BaseParams baseParams)
        {
            var reports = this.Container.ResolveAll<IComissionMeetingCodedReport>();

            try
            {

                return reports.Where(x => x.CodeForm == "Protocol" || x.CodeForm == "CourtPostRegistryStandaloneReport" || x.ReportId == "Resolution")
                .Select(x => new ReportInfo
                {
                    Id = x.ReportId,
                    Name = this.MenuItemText.GetText(x.Name),
                    Description = this.MenuItemText.GetText(x.Description)
                })
                .ToList();

            }
            finally
            {
                foreach (var report in reports)
                {
                    this.Container.Release(report);
                }
            }
        }

        /// <summary>
        /// Вернуть список отчётов
        /// </summary>
        /// <param name="baseParams">Базовые параметры запросы</param>
        /// <returns>Результат операции</returns>
        public IList<ReportInfo> GetResolutionDefinitionReportList(BaseParams baseParams)
        {
            var reports = this.Container.ResolveAll<IComissionMeetingCodedReport>();

            try
            {

                return reports.Where(x => x.CodeForm == "ResolutionDefinition")
                .Select(x => new ReportInfo
                {
                    Id = x.ReportId,
                    Name = this.MenuItemText.GetText(x.Name),
                    Description = this.MenuItemText.GetText(x.Description)
                })
                .ToList();

            }
            finally
            {
                foreach (var report in reports)
                {
                    this.Container.Release(report);
                }
            }
        }

        /// <summary>
        /// Вернуть список отчётов
        /// </summary>
        /// <param name="baseParams">Базовые параметры запросы</param>
        /// <returns>Результат операции</returns>
        public IList<ReportInfo> GetReportList(BaseParams baseParams)
        {
            var reports = this.Container.ResolveAll<IComissionMeetingCodedReport>();
            var reportType = baseParams.Params["reptype"].ToString();
            var codeForms = baseParams.Params["codeForm"].ToString().Split(StringSplitOptions.RemoveEmptyEntries, ","," ");
            var documentId = baseParams.Params.GetAsId("documentId");

            try
            {
                if (reportType == "res")
                {
                    return reports.Where(x => x.ReportId == "Resolution" || x.ReportId == "OSSPRegistry")
                    .Select(x => new ReportInfo
                    {
                        Id = x.ReportId,
                        Name = this.MenuItemText.GetText(x.Name),
                        Description = this.MenuItemText.GetText(x.Description)
                    })
                    .ToList();
                }

                else if (reportType == "resdef")
                {
                    var data = reports.Where(x => x.ReportId == "ResolutionDefinition")
                    .Select(x => new ReportInfo
                    {
                        Id = x.ReportId,
                        Name = this.MenuItemText.GetText(x.Name),
                        Description = this.MenuItemText.GetText(x.Description)
                    })
                    .ToList();
                    return data;
                }

                return reports.Where(x => x.CodeForm != "Resolution")
                    .Where(x => x.CodeForm != "ResolutionDefinition")
                    .Where(x => x.CodeForm != "OSSPPostRegistryReport")
                    .Where(x => x.CodeForm != "CourtPostRegistryStandaloneReport")
                    .Where(x => x.CodeForm != "OSSPPostRegistryReport")
                    .Where(x => x.CodeForm != "OSSPPostRegistryComissionReport")
                    .Where(x => x.CodeForm != "CourtPostRegistryReport")
                    .Select(x => new ReportInfo
                    {
                        Id = x.ReportId,
                        Name = this.MenuItemText.GetText(x.Name),
                        Description = this.MenuItemText.GetText(x.Description)
                    })
                    .ToList();
            }
            finally
            {
                foreach (var report in reports)
                {
                    this.Container.Release(report);
                }
            }
        }

        /// <summary>
        /// Напечатать отчёт
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Файл отчёта</returns>
        public IDataResult GetReport(BaseParams baseParams)
        {
            return null;
        }

        /// <summary>
        /// Печать массового отчёта
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Архив отчёта</returns>
        public IDataResult GetMassReport(BaseParams baseParams)
        {
            return null;
        }

        //Печать из вкладки собственников
        public IDataResult GetLawsuitOnwerReport(BaseParams baseParams)
        {
            var fileManager = this.Container.Resolve<IFileManager>();
            var claimWorkCodedReportDomain = this.Container.ResolveAll<IComissionMeetingCodedReport>();
            var comissionId = string.Empty;
            var reportId = baseParams.Params.GetAs<string>("reportId");
            var recIds = baseParams.Params.GetAs<long[]>("recIds");
            string recIdsstr = string.Join(",", recIds.ToList());
            var userParam = new UserParamsValues();
            long documentId = 0;
            bool registryLikeReport = false;
            if (reportId == "PostRegystry" || reportId == "CourtPostRegistryStandaloneReport")
            {
                registryLikeReport = true;
            }
            if (reportId == "OSSPRegistry")
            {
                return GetOSSLikeReport(baseParams);
            }
            if (reportId == "ProtocolToCourt")
            {
                return GetToCourtLikeReport(baseParams);
            }

            if (baseParams.Params.ContainsKey("userParams") && baseParams.Params["userParams"] is DynamicDictionary)
            {
                userParam.Values = (DynamicDictionary)baseParams.Params["userParams"];
              

                comissionId = userParam.GetValue("comissionId").ToString();
            }

            if (recIds.Length < 1)
            {
                throw new Exception("Необходимо выбрать хотя бы одну запись для печати");
            }

            if (reportId == "Resolution")
            {
                recIds = FromProtocolsToResolutions(recIds);
                recIdsstr = string.Join(",", recIds.ToList());
            }

            var commId = !string.IsNullOrEmpty(comissionId) ? Convert.ToInt64(comissionId) : 0;

            try
            {

                if (recIds.Length == 1 || registryLikeReport)
                {
                    //TODO: Унифицировать логику

                    documentId = recIds[0];                  
                    var report = claimWorkCodedReportDomain.FirstOrDefault(x => x.ReportId == reportId);

                    if (report == null)
                    {
                        throw new Exception("Не найдена реализация отчета для выбранного документа");
                    }

                    MemoryStream stream;

                    // Проставляем пользовательские параметры
                    userParam.AddValue("DocumentId", documentId);
                //    userParam.AddValue("comissionId", comissionId);
                    userParam.AddValue("recIds", recIdsstr);
                    report.SetUserParams(userParam);
                    var reportProvider = Container.Resolve<IGkhReportProvider>();
                    if (report is IReportGenerator && report.GetType().IsSubclassOf(typeof(StimulReport)))
                    {
                        //Вот такой вот костыльный этот метод Все над опеределывать
                        stream = (report as StimulReport).GetGeneratedReport();
                    }
                    else
                    {
                        var reportParams = new ReportParams();
                        report.PrepareReport(reportParams);

                        // получаем Генератор отчета
                        var generatorName = report.GetReportGenerator();

                        stream = new MemoryStream();
                        var generator = Container.Resolve<IReportGenerator>(generatorName);
                        reportProvider.GenerateReport(report, stream, generator, reportParams);
                    }

                    report.ReportInfo = new ComissionMeetingReportInfo();

                    report.DocId = documentId.ToString();

                    report.ReportInfo.DocumentIds = recIds;
                    report.GenerateMassReport();
                    var file = fileManager.SaveFile(stream, report.OutputFileName);
                    return new BaseDataResult(file.Id);
                }
                else
                {
                    var reports = new List<IComissionMeetingCodedReport>();

                    var tempReport = claimWorkCodedReportDomain.FirstOrDefault(x => x.ReportId == reportId);

                    if (tempReport != null)
                    {
                        var reportName = tempReport.GetType().FullName;

                        foreach (var recId in recIds)
                        {
                            var report = this.Container.Resolve<IComissionMeetingCodedReport>(reportName);

                            if (report == null)
                            {
                                throw new Exception("Не найдена реализация отчета для выбранного документа");
                            }

                            // Проставляем пользовательские параметры
                            userParam = new UserParamsValues();
                            //userParam.Values = (DynamicDictionary)baseParams.Params["userParams"];
                            userParam.AddValue("DocumentId", recId);
                            userParam.AddValue("comissionId", commId);
                            report.SetUserParams(userParam);
                            var reportProvider = Container.Resolve<IGkhReportProvider>();
                            if (report is IReportGenerator && report.GetType().IsSubclassOf(typeof(StimulReport)))
                            {
                                //Вот такой вот костыльный этот метод Все над опеределывать
                                report.ReportFileStream = (report as StimulReport).GetGeneratedReport();
                            }
                            else
                            {
                                var reportParams = new ReportParams();
                                report.PrepareReport(reportParams);

                                // получаем Генератор отчета
                                var generatorName = report.GetReportGenerator();                     
                                var generator = Container.Resolve<IReportGenerator>(generatorName);
                                reportProvider.GenerateReport(report, report.ReportFileStream, generator, reportParams);
                            }

                            report.ReportInfo = new ComissionMeetingReportInfo();

                            report.DocId = recId.ToString();
                            report.ReportInfo.DocumentIds = recIds;
                            report.GenerateMassReport();
                            reports.Add(report);
                            report.ReportFileStream.Seek(0, SeekOrigin.Begin);
                        }
                    }

                    var archive = new ZipFile(Encoding.UTF8)
                    {
                        CompressionLevel = CompressionLevel.Level9,
                        AlternateEncoding = Encoding.GetEncoding("cp866")
                    };

                    var tempDir = Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()));

                    Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
                    Microsoft.Office.Interop.Word.Document newDoc = wordApp.Documents.Add();
                    bool first = true;

                    foreach (var report in reports)
                    {
                        File.WriteAllBytes(
                            Path.Combine(tempDir.FullName, ValidateFileName(report.OutputFileName)),
                            report.ReportFileStream.ReadAllBytes());

                        if (report.OutputFileName.Contains(".docx"))
                        {
                            string sourceFilePath = Path.Combine(tempDir.FullName, ValidateFileName(report.OutputFileName));
                            if (first)
                            {
                                newDoc = wordApp.Documents.Open(sourceFilePath);
                                first = false;
                                newDoc.SaveAs2(Path.Combine(tempDir.FullName, "Общий документ.docx"));
                             //   newDoc.Close();
                            }
                            else
                            {
                                string destinationFilePath = (Path.Combine(tempDir.FullName, "Общий документ.docx"));
                                Microsoft.Office.Interop.Word.Document sourceDoc = wordApp.Documents.Open(sourceFilePath);
                                sourceDoc.Content.Select();
                                sourceDoc.ActiveWindow.Selection.Copy();
                                newDoc.Range(newDoc.Content.End - 1, newDoc.Content.End - 1).InsertBreak(WdBreakType.wdPageBreak);
                                newDoc.Range(newDoc.Content.End - 1, newDoc.Content.End - 1).Paste();                           
                                newDoc.Save();
                                sourceDoc.Close();
                               
                            }
                        }
                    }
                    newDoc.Close();
                    wordApp.Quit();

                    archive.AddDirectory(tempDir.FullName);

                    using (var ms = new MemoryStream())
                    {
                        archive.Save(ms);

                        var file = fileManager.SaveFile(
                            ms,
                            $"Документы.zip");
                        return new BaseDataResult(file.Id);
                    }
                }

            }
            finally
            {
                this.Container.Release(fileManager);
                this.Container.Release(claimWorkCodedReportDomain);
            }
        }

        private IDataResult GetToCourtLikeReport(BaseParams baseParams)
        {
            var fileManager = this.Container.Resolve<IFileManager>();
            var claimWorkCodedReportDomain = this.Container.ResolveAll<IComissionMeetingCodedReport>();
            var comissionId = string.Empty;
            var reportId = baseParams.Params.GetAs<string>("reportId");
            var recIds = baseParams.Params.GetAs<long[]>("recIds");
            string recIdsstr = string.Join(",", recIds.ToList());
            var userParam = new UserParamsValues();
            long documentId = 0;
            bool registryLikeReport = true;

            if (baseParams.Params.ContainsKey("userParams") && baseParams.Params["userParams"] is DynamicDictionary)
            {
                userParam.Values = (DynamicDictionary)baseParams.Params["userParams"];

                comissionId = userParam.GetValue("comissionId").ToString();
            }

            if (recIds.Length < 1)
            {
                throw new Exception("Необходимо выбрать хотя бы одну запись для печати");
            }

            var commId = !string.IsNullOrEmpty(comissionId) ? Convert.ToInt64(comissionId) : 0;

            try
            {
                var reports = new List<IComissionMeetingCodedReport>();

                //формируем файл реестра
                try
                {
                    var templ = claimWorkCodedReportDomain.FirstOrDefault(x => x.ReportId == reportId);
                    reports.Add(GetRegistryReport(templ, recIdsstr));
                }
                catch (Exception e)
                {

                }

                var tempPostReg = claimWorkCodedReportDomain.FirstOrDefault(x => x.ReportId == "CourtPostRegistryReport");

                if (tempPostReg != null)
                {
                    var reportName = tempPostReg.GetType().FullName;

                        var report = this.Container.Resolve<IComissionMeetingCodedReport>(reportName);

                        if (report == null)
                        {
                            throw new Exception("Не найдена реализация отчета для почтового реестра");
                        }

                        // Проставляем пользовательские параметры
                        userParam = new UserParamsValues();
                        //userParam.Values = (DynamicDictionary)baseParams.Params["userParams"];
                        userParam.AddValue("recIds", recIdsstr);
                        report.SetUserParams(userParam);
                        var reportProvider = Container.Resolve<IGkhReportProvider>();
                        if (report is IReportGenerator && report.GetType().IsSubclassOf(typeof(StimulReport)))
                        {
                            //Вот такой вот костыльный этот метод Все над опеределывать
                            report.ReportFileStream = (report as StimulReport).GetGeneratedReport();
                        }
                        else
                        {
                            var reportParams = new ReportParams();
                            report.PrepareReport(reportParams);

                            // получаем Генератор отчета
                            var generatorName = report.GetReportGenerator();
                            var generator = Container.Resolve<IReportGenerator>(generatorName);
                            reportProvider.GenerateReport(report, report.ReportFileStream, generator, reportParams);
                        }

                        report.ReportInfo = new ComissionMeetingReportInfo();

                        report.DocId = recIds.ToString();
                        report.ReportInfo.DocumentIds = recIds;
                        report.GenerateMassReport();
                        reports.Add(report);
                        report.ReportFileStream.Seek(0, SeekOrigin.Begin);
                }

                var archive = new ZipFile(Encoding.UTF8)
                {
                    CompressionLevel = CompressionLevel.Level9,
                    AlternateEncoding = Encoding.GetEncoding("cp866")
                };

                var tempDir = Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()));

                Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
                Microsoft.Office.Interop.Word.Document newDoc = wordApp.Documents.Add();
                bool first = true;

                foreach (var report in reports)
                {
                    File.WriteAllBytes(
                        Path.Combine(tempDir.FullName, ValidateFileName(report.OutputFileName)),
                        report.ReportFileStream.ReadAllBytes());

                    if (report.OutputFileName.Contains(".docx"))
                    {
                        string sourceFilePath = Path.Combine(tempDir.FullName, ValidateFileName(report.OutputFileName));
                        if (first)
                        {
                            newDoc = wordApp.Documents.Open(sourceFilePath);
                            first = false;
                            newDoc.SaveAs2(Path.Combine(tempDir.FullName, "Общий документ.docx"));
                            //   newDoc.Close();
                        }
                        else
                        {
                            string destinationFilePath = (Path.Combine(tempDir.FullName, "Общий документ.docx"));
                            Microsoft.Office.Interop.Word.Document sourceDoc = wordApp.Documents.Open(sourceFilePath);
                            sourceDoc.Content.Select();
                            sourceDoc.ActiveWindow.Selection.Copy();
                            newDoc.Range(newDoc.Content.End - 1, newDoc.Content.End - 1).InsertBreak(WdBreakType.wdPageBreak);
                            newDoc.Range(newDoc.Content.End - 1, newDoc.Content.End - 1).Paste();
                            newDoc.Save();
                            sourceDoc.Close();

                        }
                    }
                }
                newDoc.Close();
                wordApp.Quit();

                archive.AddDirectory(tempDir.FullName);

                using (var ms = new MemoryStream())
                {

                    archive.Save(ms);

                    var file = fileManager.SaveFile(
                        ms,
                        $"Документы.zip");
                    return new BaseDataResult(file.Id);
                }

            }
            finally
            {
                this.Container.Release(fileManager);
                this.Container.Release(claimWorkCodedReportDomain);
            }
        }

        private IDataResult GetOSSLikeReport(BaseParams baseParams)
        {
            var fileManager = this.Container.Resolve<IFileManager>();
            var claimWorkCodedReportDomain = this.Container.ResolveAll<IComissionMeetingCodedReport>();
            var comissionId = string.Empty;
            var reportId = baseParams.Params.GetAs<string>("reportId");
            var recIds = baseParams.Params.GetAs<long[]>("recIds");
            string recIdsstr = string.Join(",", recIds.ToList());
            string currUser = this.Container.Resolve<IGkhUserManager>().GetActiveOperator().Inspector.ShortFio;
            string currUserPhone = this.Container.Resolve<IGkhUserManager>().GetActiveOperator().Inspector.Phone;
            var userParam = new UserParamsValues();
            long documentId = 0;
            bool registryLikeReport = true;

            if (baseParams.Params.ContainsKey("userParams") && baseParams.Params["userParams"] is DynamicDictionary)
            {
                userParam.Values = (DynamicDictionary)baseParams.Params["userParams"];


                comissionId = userParam.GetValue("comissionId").ToString();
            }

            if (recIds.Length < 1)
            {
                throw new Exception("Необходимо выбрать хотя бы одну запись для печати");
            }

            var commId = !string.IsNullOrEmpty(comissionId) ? Convert.ToInt64(comissionId) : 0;

            try
            {
                var reports = new List<IComissionMeetingCodedReport>();

                //формируем файл реестра
                try
                {
                    var templ = claimWorkCodedReportDomain.FirstOrDefault(x => x.ReportId == reportId);
                    reports.Add(GetRegistryReport(templ, recIdsstr));
                }
                catch(Exception e)
                {
                    
                }

                var tempReport = claimWorkCodedReportDomain.FirstOrDefault(x => x.ReportId == "Resolution");

                if (tempReport != null)
                {
                    var reportName = tempReport.GetType().FullName;

                    foreach (var recId in recIds)
                    {
                        var report = this.Container.Resolve<IComissionMeetingCodedReport>(reportName);

                        if (report == null)
                        {
                            throw new Exception("Не найдена реализация отчета для выбранного документа");
                        }

                        // Проставляем пользовательские параметры
                        userParam = new UserParamsValues();
                        //userParam.Values = (DynamicDictionary)baseParams.Params["userParams"];
                        userParam.AddValue("DocumentId", recId);
                        userParam.AddValue("comissionId", commId);
                        userParam.AddValue("currUser", currUser);
                        userParam.AddValue("currUserPhone", currUserPhone);
                        report.SetUserParams(userParam);
                        var reportProvider = Container.Resolve<IGkhReportProvider>();
                        if (report is IReportGenerator && report.GetType().IsSubclassOf(typeof(StimulReport)))
                        {
                            //Вот такой вот костыльный этот метод Все над опеределывать
                            report.ReportFileStream = (report as StimulReport).GetGeneratedReport();
                        }
                        else
                        {
                            var reportParams = new ReportParams();
                            report.PrepareReport(reportParams);

                            // получаем Генератор отчета
                            var generatorName = report.GetReportGenerator();
                            var generator = Container.Resolve<IReportGenerator>(generatorName);
                            reportProvider.GenerateReport(report, report.ReportFileStream, generator, reportParams);
                        }

                        report.ReportInfo = new ComissionMeetingReportInfo();

                        report.DocId = recId.ToString();
                        report.ReportInfo.DocumentIds = recIds;
                        report.GenerateMassReport();
                        reports.Add(report);
                        report.ReportFileStream.Seek(0, SeekOrigin.Begin);
                    }
                }

                if (commId != 0)
                {
                    var tempPostReg = claimWorkCodedReportDomain.FirstOrDefault(x => x.ReportId == "OSSPPostRegistryComissionReport");

                    if (tempPostReg != null)
                    {
                        var reportName = tempPostReg.GetType().FullName;

                        var report = this.Container.Resolve<IComissionMeetingCodedReport>(reportName);

                        if (report == null)
                        {
                            throw new Exception("Не найдена реализация отчета для почтового реестра");
                        }

                        // Проставляем пользовательские параметры
                        userParam = new UserParamsValues();

                        //userParam.Values = (DynamicDictionary)baseParams.Params["userParams"];
                        userParam.AddValue("recIds", recIdsstr);
                        userParam.AddValue("comissionId", commId);
                        report.SetUserParams(userParam);
                        var reportProvider = Container.Resolve<IGkhReportProvider>();
                        if (report is IReportGenerator && report.GetType().IsSubclassOf(typeof(StimulReport)))
                        {
                            //Вот такой вот костыльный этот метод Все над опеределывать
                            report.ReportFileStream = (report as StimulReport).GetGeneratedReport();
                        }
                        else
                        {
                            var reportParams = new ReportParams();
                            report.PrepareReport(reportParams);

                            // получаем Генератор отчета
                            var generatorName = report.GetReportGenerator();
                            var generator = Container.Resolve<IReportGenerator>(generatorName);
                            reportProvider.GenerateReport(report, report.ReportFileStream, generator, reportParams);
                        }

                        report.ReportInfo = new ComissionMeetingReportInfo();

                        report.DocId = recIds.ToString();
                        report.ReportInfo.DocumentIds = recIds;
                        report.GenerateMassReport();
                        reports.Add(report);
                        report.ReportFileStream.Seek(0, SeekOrigin.Begin);
                    }
                }

                else
                {
                    var tempPostReg = claimWorkCodedReportDomain.FirstOrDefault(x => x.ReportId == "OSSPPostRegistryReport");

                    if (tempPostReg != null)
                    {
                        var reportName = tempPostReg.GetType().FullName;

                        var report = this.Container.Resolve<IComissionMeetingCodedReport>(reportName);

                        if (report == null)
                        {
                            throw new Exception("Не найдена реализация отчета для почтового реестра");
                        }

                        // Проставляем пользовательские параметры
                        userParam = new UserParamsValues();

                        //userParam.Values = (DynamicDictionary)baseParams.Params["userParams"];
                        userParam.AddValue("recIds", recIdsstr);
                        report.SetUserParams(userParam);
                        var reportProvider = Container.Resolve<IGkhReportProvider>();
                        if (report is IReportGenerator && report.GetType().IsSubclassOf(typeof(StimulReport)))
                        {
                            //Вот такой вот костыльный этот метод Все над опеределывать
                            report.ReportFileStream = (report as StimulReport).GetGeneratedReport();
                        }
                        else
                        {
                            var reportParams = new ReportParams();
                            report.PrepareReport(reportParams);

                            // получаем Генератор отчета
                            var generatorName = report.GetReportGenerator();
                            var generator = Container.Resolve<IReportGenerator>(generatorName);
                            reportProvider.GenerateReport(report, report.ReportFileStream, generator, reportParams);
                        }

                        report.ReportInfo = new ComissionMeetingReportInfo();

                        report.DocId = recIds.ToString();
                        report.ReportInfo.DocumentIds = recIds;
                        report.GenerateMassReport();
                        reports.Add(report);
                        report.ReportFileStream.Seek(0, SeekOrigin.Begin);
                    }
                }

                var archive = new ZipFile(Encoding.UTF8)
                {
                    CompressionLevel = CompressionLevel.Level9,
                    AlternateEncoding = Encoding.GetEncoding("cp866")
                };

                var tempDir = Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()));

              //  Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
              //  Microsoft.Office.Interop.Word.Document newDoc = wordApp.Documents.Add();
                bool first = true;

                foreach (var report in reports)
                {
                    File.WriteAllBytes(
                        Path.Combine(tempDir.FullName, ValidateFileName(report.OutputFileName)),
                        report.ReportFileStream.ReadAllBytes());

                    if (report.OutputFileName.Contains(".docx"))
                    {
                        //string sourceFilePath = Path.Combine(tempDir.FullName, ValidateFileName(report.OutputFileName));
                        //if (first)
                        //{
                        //    newDoc = wordApp.Documents.Open(sourceFilePath);
                        //    first = false;
                        //    newDoc.SaveAs2(Path.Combine(tempDir.FullName, "Общий документ.docx"));
                        //    //   newDoc.Close();
                        //}
                        //else
                        //{
                        //    string destinationFilePath = (Path.Combine(tempDir.FullName, "Общий документ.docx"));
                        //    Microsoft.Office.Interop.Word.Document sourceDoc = wordApp.Documents.Open(sourceFilePath);
                        //    sourceDoc.Content.Select();
                        //    sourceDoc.ActiveWindow.Selection.Copy();
                        //    newDoc.Range(newDoc.Content.End - 1, newDoc.Content.End - 1).InsertBreak(WdBreakType.wdPageBreak);
                        //    newDoc.Range(newDoc.Content.End - 1, newDoc.Content.End - 1).Paste();
                        //    newDoc.Save();
                        //    sourceDoc.Close();

                        //}
                    }
                }
                //newDoc.Close();
                //wordApp.Quit();

                archive.AddDirectory(tempDir.FullName);

                using (var ms = new MemoryStream())
                {                

                    archive.Save(ms);

                    var file = fileManager.SaveFile(
                        ms,
                        $"Документы.zip");
                    return new BaseDataResult(file.Id);
                }

            }
            finally
            {
                this.Container.Release(fileManager);
                this.Container.Release(claimWorkCodedReportDomain);
            }
        }

        private IComissionMeetingCodedReport GetRegistryReport(IComissionMeetingCodedReport tempReport, string recIdsstr)
        {
       
            var reportName = tempReport.GetType().FullName;
            string currUser = this.Container.Resolve<IGkhUserManager>().GetActiveOperator().Inspector.ShortFio;
            string currUserPhone = this.Container.Resolve<IGkhUserManager>().GetActiveOperator().Inspector.Phone;
            var report = this.Container.Resolve<IComissionMeetingCodedReport>(reportName);
            if (report == null)
            {
                throw new Exception("Не найдена реализация отчета для выбранного документа");
            }          
            var userParam = new UserParamsValues();
            userParam.AddValue("comissionId", 0);
            userParam.AddValue("currUser", currUser);
            userParam.AddValue("currUserPhone", currUserPhone);
            userParam.AddValue("recIds", recIdsstr);
            report.SetUserParams(userParam);
            var reportProvider = Container.Resolve<IGkhReportProvider>();
            if (report is IReportGenerator && report.GetType().IsSubclassOf(typeof(StimulReport)))
            {
                //Вот такой вот костыльный этот метод Все над опеределывать
                report.ReportFileStream = (report as StimulReport).GetGeneratedReport();
            }
            else
            {
                var reportParams = new ReportParams();
                report.PrepareReport(reportParams);

                // получаем Генератор отчета
                var generatorName = report.GetReportGenerator();
                var generator = Container.Resolve<IReportGenerator>(generatorName);
                reportProvider.GenerateReport(report, report.ReportFileStream, generator, reportParams);
            }
            report.ReportInfo = new ComissionMeetingReportInfo();
            report.GenerateMassReport();         
            report.ReportFileStream.Seek(0, SeekOrigin.Begin);
            return report;
        }

        private static string ValidateFileName(string fileName)
        {
            return string.Join("-",fileName.Split(Path.GetInvalidFileNameChars())).Replace("--", "-");
        }


        /// <summary>
        /// Проверяет откуда печатается постановление
        /// Если из протокола, то ищет ID родительских постановлений
        /// Если из постановления, то возвращает ID постановлений
        /// </summary>
        private long[] FromProtocolsToResolutions(long[] ids)
        {
            List<long> resList = new List<long>();
            foreach (var id in ids)
            {
                var docType = Container.Resolve<IDomainService<DocumentGji>>().GetAll()
                    .Where(x => x.Id == id)
                    .Select(x => x.TypeDocumentGji)
                    .FirstOrDefault();
                switch (docType)
                {
                    case Enums.TypeDocumentGji.Protocol:
                        var parentId = Container.Resolve<IDomainService<DocumentGjiChildren>>().GetAll()
                        .Where(x => x.Children.Id == id)
                        .Select(x => x.Parent.Id)
                        .FirstOrDefault();

                        var resolutionId = Container.Resolve<IDomainService<Resolution>>().GetAll()
                            .Where(x => x.Id == parentId)
                            .Select(x => x.Id)
                            .FirstOrDefault();

                        resList.Add(resolutionId);
                        break;

                    case Enums.TypeDocumentGji.Resolution:
                        resList.Add(id);
                        break;

                    default:
                        break;
                }
            }
            return resList.ToArray();
        }
    }
}