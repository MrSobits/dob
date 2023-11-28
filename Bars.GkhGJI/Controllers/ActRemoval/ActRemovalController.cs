namespace Bars.GkhGji.Controllers
{
    using System.Collections;
    using System.Web.Mvc;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;

    public class ActRemovalController : ActRemovalController<ActRemoval>
    {
    }

    public class ActRemovalController<T> : B4.Alt.DataController<T>
        where T : ActRemoval
    {
        public ActionResult GetInfo(long? documentId)
        {
            var service = Container.Resolve<IActRemovalService>();

            try
            {
                var result = service.GetInfo(documentId);
                return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
            }
            finally 
            {
                Container.Release(service);
            }
        }

        public ActionResult ListView(BaseParams baseParams)
        {
            var service = Container.Resolve<IActRemovalService>();

            try
            {
                var result = (ListDataResult)service.ListView(baseParams);
                return result.Success ? new JsonListResult((IList)result.Data, result.TotalCount) : JsonNetResult.Failure(result.Message);
            }
            finally
            {
                Container.Release(service);
            }
        }

        public ActionResult Export(BaseParams baseParams)
        {
            var export = Container.Resolve<IDataExportService>("ActRemovalDataExport");
            try
            {
                return export != null ? export.ExportData(baseParams) : null;
            }
            finally 
            {
                Container.Release(export);

            }
        }
    }
}