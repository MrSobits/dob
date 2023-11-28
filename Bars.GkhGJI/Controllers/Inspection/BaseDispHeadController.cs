namespace Bars.GkhGji.Controllers
{
    using System;
    using System.Web.Mvc;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Modules.FileStorage;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;

    public class BaseDispHeadController : BaseDispHeadController<BaseDispHead>
    {
    }

    public class BaseDispHeadController<T> : FileStorageDataController<T>
        where T : BaseDispHead
    {
        public ActionResult GetInfo(BaseParams baseParams)
        {
            var service = Container.Resolve<IBaseDispHeadService>();
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
            var export = Container.Resolve<IDataExportService>("BaseDispHeadDataExport");
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