namespace Bars.GkhGji.Controllers
{
    using System.Collections;
    using System.Web.Mvc;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;

    public class HeatSeasonController : B4.Alt.DataController<HeatSeason>
    {
        public ActionResult Export(BaseParams baseParams)
        {
            var export = Container.Resolve<IDataExportService>("HeatSeasonDataExport");

            return export != null ? export.ExportData(baseParams) : null;
        }

        public ActionResult ListView(BaseParams baseParams)
        {
            var result = (ListDataResult)Container.Resolve<IHeatSeasonService>().ListView(baseParams);
            return result.Success ? new JsonListResult((IList)result.Data, result.TotalCount) : JsonNetResult.Failure(result.Message);
        }
    }
}