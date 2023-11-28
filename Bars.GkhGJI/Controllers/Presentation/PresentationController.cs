namespace Bars.GkhGji.Controllers
{
    using System;
    using System.Web.Mvc;

    using B4;
    using B4.Modules.DataExport.Domain;
    using DomainService;
    using Entities;

    public class PresentationController : PresentationController<Presentation>
    {
    }

    public class PresentationController<T> : B4.Alt.DataController<T>
        where T : Presentation
    {
        public ActionResult GetInfo(BaseParams baseParams)
        {
            var service = Container.Resolve<IPresentationService>();

            try
            {
                var result = service.GetInfo(baseParams);
                return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
            }
            finally
            {
                Container.Release(service);   
            }
        }

        public ActionResult Export(BaseParams baseParams)
        {
            var export = Container.Resolve<IDataExportService>("PresentationDataExport");

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