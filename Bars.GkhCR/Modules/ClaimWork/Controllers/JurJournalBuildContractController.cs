namespace Bars.GkhCr.Modules.ClaimWork.Controllers
{
    using System.Web.Mvc;
    using B4;
    using B4.Modules.DataExport.Domain;
    using DomainService;

    /// <summary>
    /// 
    /// </summary>
    public class JurJournalBuildContractController : BaseController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public ActionResult List(BaseParams baseParams)
        {
            var service = Resolve<IJurJournalBuildContractService>();
            try
            {
                int totalCount;
                var data = service.GetList(baseParams, true, out totalCount);
                return new JsonListResult(data, totalCount);
            }
            finally
            {
                Container.Release(service);
            }
        }

        public ActionResult Export(BaseParams baseParams)
        {
            var export = Container.Resolve<IDataExportService>("JurJournalBuildContractExport");
            try
            {
                return export.ExportData(baseParams);
            }
            finally
            {
                Container.Release(export);
            }
        }
    }
}