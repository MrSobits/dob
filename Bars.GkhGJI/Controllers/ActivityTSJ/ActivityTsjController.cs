namespace Bars.GkhGji.Controllers
{
    using System.Web.Mvc;

    using B4;
    using B4.Modules.DataExport.Domain;
    using Entities;

    public class ActivityTsjController : B4.Alt.DataController<ActivityTsj>
    {
        public ActionResult Export(BaseParams baseParams)
        {
            var export = Container.Resolve<IDataExportService>("ActivityTsjDataExport");

            if (export != null)
            {
                return export.ExportData(baseParams);
            }

            return null;
        }
    }
}