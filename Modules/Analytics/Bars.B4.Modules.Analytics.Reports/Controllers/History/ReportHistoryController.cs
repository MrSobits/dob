namespace Bars.B4.Modules.Analytics.Reports.Controllers.History
{
    using System.Collections;
    using System.Web.Mvc;
    using Domain.History;
    using Entities.History;
    using FileStorage;
    
    public class ReportHistoryController : FileStorageDataController<ReportHistory>
    {
        public IReportHistoryService Service { get; set; }

        /// <summary>
        /// Получить список параметров отчета для одной записи из журнала отчетов
        /// </summary>
        /// <param name="baseParams">baseParams</param>
        public ActionResult ReportHistoryParamList(BaseParams baseParams)
        {
            var result = (ListDataResult)this.Service.ReportHistoryParamList(baseParams);
            return new JsonListResult((IList)result.Data);
        }
    }
}