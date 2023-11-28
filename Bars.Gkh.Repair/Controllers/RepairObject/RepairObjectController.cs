namespace Bars.Gkh.Repair.Controllers
{
    using System.Web.Mvc;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.Gkh.Repair.DomainService;

    public class RepairObjectController : B4.Alt.DataController<Entities.RepairObject>
    {
        public IRepairObjectService Service { get; set; }

        public ActionResult Export(BaseParams baseParams)
        {
            var export = Container.Resolve<IDataExportService>("RepairObjectDataExport");
            return export != null ? export.ExportData(baseParams) : null;
        }

        public ActionResult MassStateChange(BaseParams baseParams)
        {
            var result = Service.MassStateChange(baseParams);
            return result.Success ? new JsonNetResult() : JsonNetResult.Failure(result.Message);
        }
    }
}