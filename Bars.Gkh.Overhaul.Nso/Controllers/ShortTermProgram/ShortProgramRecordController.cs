namespace Bars.Gkh.Overhaul.Nso.Controllers
{
    using System.Web.Mvc;
    using B4;
    using B4.Modules.DataExport.Domain;
    using Entities;

    public class ShortProgramRecordController : B4.Alt.DataController<ShortProgramRecord>
    {
        public ActionResult Export(BaseParams baseParams)
        {
            var export = Container.Resolve<IDataExportService>("ShortProgramRecordExport");
            return export != null ? export.ExportData(baseParams) : null;
        }
    }
}