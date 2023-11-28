namespace Bars.Gkh.Controllers
{
    using System;
    using System.Web.Mvc;

    using Bars.B4;
    using Bars.B4.Utils.Web;
    using Bars.Gkh.Report;

    public class GkhReportController : BaseController
    {
        public ActionResult GetReportList(BaseParams baseParams)
        {
            var reportProvider = Container.Resolve<IGkhReportService>();

            return new JsonListResult(reportProvider.GetReportList(baseParams));
        }

        public ActionResult ReportPrint(BaseParams baseParams)
        {

            var file = Container.Resolve<IGkhReportService>().GetReport(baseParams);

            // Хак для отображения русских имен файлов
            file.FileDownloadName = this.ControllerContext.HttpContext.Server.UrlEncode(file.FileDownloadName).Replace("+", "%20");

            return file;
        }
    }
}