namespace Bars.B4.Modules.Analytics.Reports.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Web.Mvc;
    using DataAccess;
    using Extensions;
    using IoC;
    using Entities;
    using Enums;
    using Generators;
    using ViewModels;
    using B4.Utils;
    using Logging;

    /// <summary>
    /// 
    /// </summary>
    public class ExternalReportController : BaseController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public ActionResult Generate(BaseParams baseParams)
        {
            var reportDomain = Container.Resolve<IDomainService<StoredReport>>();
            var generator = Container.Resolve<IReportGenerator>();
            var logManager = Container.Resolve<ILogManager>();

            using (Container.Using(reportDomain, generator))
            {
                var reportId = baseParams.Params.GetAs<long>("reportId", ignoreCase: true);
                var token = baseParams.Params.GetAs<string>("token", ignoreCase: true);
                var format = baseParams.Params.GetAs("format", ignoreCase: true, defaultValue: ReportPrintFormat.xls);

                var report = reportDomain.FirstOrDefault(x => x.Id == reportId);
                if (report == null || !MD5.GetHashString64(report.Id + StoredReportViewModel.Salt).Equals(token))
                {
                    return JsFailure("Отчет по указанной ссылке не обнаружен. Попробуйте сформировать внешнюю ссылку на отчет повторно.");
                }

                try
                {
                    var file = generator.Generate(
                        report.GetDataSources(),
                        report.GetTemplate(),
                        baseParams,
                        format,
                        new Dictionary<string, object>
                            {
                                { "ExportSettings", report.GetExportSettings(format) },
                                { "UseTemplateConnectionString", report.UseTemplateConnectionString }
                            });
                    file.Seek(0, SeekOrigin.Begin);

                    var fileNameEncode =
                        System.Web.HttpUtility.UrlEncode(string.Format("{0}.{1}", report.Name, format.Extension()));

                    return File(file, System.Net.Mime.MediaTypeNames.Application.Octet, fileNameEncode);
                }
                catch (Exception e)
                {
                    logManager.Error("Ошибка формирования отчета", e);
                    return JsFailure("Ошибка формирования отчета");
                }
            }
        }
    }
}
