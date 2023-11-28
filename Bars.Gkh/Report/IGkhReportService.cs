namespace Bars.Gkh.Report
{
    using System.Collections;
    using System.Web.Mvc;

    using B4;

    public interface IGkhReportService
    {
        IList GetReportList(BaseParams baseParams);

        FileStreamResult GetReport(BaseParams baseParams);
    }
}