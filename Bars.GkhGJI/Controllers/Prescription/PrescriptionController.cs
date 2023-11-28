namespace Bars.GkhGji.Controllers
{
    using System.Collections;
    using System.Web.Mvc;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;

    public class PrescriptionController : PrescriptionController<Entities.Prescription>
    {
    }

    public class PrescriptionController<T> : B4.Alt.DataController<T>
        where T : Entities.Prescription
    {
        public ActionResult GetInfo(long? documentId)
        {
            var service = Container.Resolve<IPrescriptionService>();

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
            var service = Container.Resolve<IPrescriptionService>();
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

        public ActionResult ListForStage(BaseParams baseParams)
        {
            var service = Container.Resolve<IPrescriptionService>();
            try
            {
                var result = (ListDataResult)service.ListForStage(baseParams);
                return result.Success ? new JsonListResult((IList)result.Data, result.TotalCount) : JsonNetResult.Failure(result.Message);
            }
            finally 
            {
                Container.Release(service);
            }
        }

        public ActionResult Export(BaseParams baseParams)
        {
            var export = Container.Resolve<IDataExportService>("PrescriptionDataExport");

            try
            {
                if (export != null)
                {
                    return export.ExportData(baseParams);
                }

                return null;
            }
            finally 
            {
                Container.Release(export);
            }
        }
    }
}