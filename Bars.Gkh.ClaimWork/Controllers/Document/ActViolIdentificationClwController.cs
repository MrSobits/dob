namespace Bars.Gkh.ClaimWork.Controllers.Document
{
    using System.Web.Mvc;
    using B4;
    using B4.Modules.DataExport.Domain;
    using Entities;

    public class ActViolIdentificationClwController : B4.Alt.DataController<ActViolIdentificationClw>
    {
        public ActionResult Export(BaseParams baseParams)
        {
            var export = Container.Resolve<IDataExportService>("ActViolIdentificationExport");
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