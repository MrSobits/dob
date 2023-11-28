namespace Bars.Gkh.Report
{
    using System.Collections;
    using System.IO;
    using System.Linq;
    using System.Web.Mvc;

    using B4;
    using B4.Modules.Reports;
    using B4.Utils;
    using Castle.Core.Internal;
    using Castle.Windsor;
    using StimulReport;

    public class GkhReportService : IGkhReportService
    {
        public virtual IWindsorContainer Container { get; set; }

        public IList GetReportList(BaseParams baseParams)
        {
            var reports = Container.ResolveAll<IGkhBaseReport>();
            var authService = Container.Resolve<IAuthorizationService>();
            var userIdentity = Container.Resolve<IUserIdentity>();

            var codeForm = baseParams.Params["codeForm"].ToString();

            try
            {
                return reports
                    .Where(x => !x.CodeForm.IsEmpty() && x.CodeForm.Split(',').Contains(codeForm))
                    .Where(x => x.PrintingAllowed)
                    .Where(x => x.Permission.IsEmpty() || authService.Grant(userIdentity, x.Permission))
                    .Select(x => new { x.Id, x.Name, x.Description })
                    .ToList();
            }
            finally
            {
                foreach (var report in reports)
                {
                    Container.Release(report);
                    Container.Release(authService);
                    Container.Release(userIdentity);
                }
            }
        }

        public FileStreamResult GetReport(BaseParams baseParams)
        {
            var reportProvider = Container.Resolve<IGkhReportProvider>();

            MemoryStream stream;
            
            var report = Container.ResolveAll<IGkhBaseReport>().First(x => x.Id == baseParams.Params.GetAs<string>("reportId"));

            var userParam = new UserParamsValues();

            if (baseParams.Params.ContainsKey("userParams") && baseParams.Params["userParams"] is DynamicDictionary)
            {
                userParam.Values = (DynamicDictionary)baseParams.Params["userParams"];
            }

            // Проставляем пользовательские параметры
            report.SetUserParams(userParam);

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

#warning Короче следующий код поулчения Extention Пишу всчвязи с кашей которая получилась в резульатте внедрения Стимул репорт
            // потому чт оне понятно как предоставлять резулитрующее расширение файла, поскольку у Стимул РЕпорт шаблон в формате mrt а резулбтат может быть doc, pdf, xls
            var fileName = string.Empty;

            if (report is IReportGeneratorFileName)
            {
                var repFn = report as IReportGeneratorFileName;
                fileName = repFn.GetFileName();
            }
            else
            {
                // сначала берем расширение которое указали в отчете
                var extention = report.Extention;
                
                // посколкьу у отчета может быть много шаблонов 
                // и к томуже каждый шаблон может быть своего формата один doc а другой xls а другой mrt
                // то тогда считаем более приоритетным тот шаблон который заменили
                var replaceExtention = report.GetFileExtention();

                if (!string.IsNullOrEmpty(replaceExtention))
                {
                    extention = replaceExtention;
                }
                
                // если совсем неудалось определеить расширение то тогда ставим по Генератору
                if (string.IsNullOrEmpty(extention))
                {
                    // Если программист забыл заполнить поле Extention при указани шаблона то остается только по генератору определять что он хотел
                    switch (report.GetReportGenerator())
                    {
                        case "XlsIoGenerator": extention = "xls"; break;
                        case "DocIoGenerator": extention = "doc"; break;
                        case "StimulReportGenerator": extention = "mrt"; break;
                    }    
                }

                fileName = string.Format("{0}.{1}", report.Name, extention);
            }

            if (string.IsNullOrEmpty(Path.GetExtension(fileName)))
            {
                fileName += ".xls";
            }

            var mimeType = MimeTypeHelper.GetMimeType(Path.GetExtension(fileName));

            return new FileStreamResult(stream, mimeType) { FileDownloadName = fileName };
            
        }
    }
}
