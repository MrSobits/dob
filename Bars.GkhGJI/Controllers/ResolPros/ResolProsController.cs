namespace Bars.GkhGji.Controllers
{
    using System.Web.Mvc;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.Gkh.DomainService;
    using Bars.GkhGji.Entities;

    public class ResolProsController : ResolProsController<ResolPros>
    {
    }

    public class ResolProsController<T> : B4.Alt.DataController<T>
        where T : ResolPros
    {
        public ActionResult Export(BaseParams baseParams)
        {
            var export = Container.Resolve<IDataExportService>("ResolProsDataExport");

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

        public IBlobPropertyService<ResolPros, ResolProsLongText> LongTextService { get; set; }

        public ActionResult GetDescription(BaseParams baseParams)
        {
            return this.GetBlob(baseParams);
        }

        public ActionResult SaveDescription(BaseParams baseParams)
        {
            return this.SaveBlob(baseParams);
        }

        private ActionResult SaveBlob(BaseParams baseParams)
        {
            var result = this.LongTextService.Save(baseParams);

            return result.Success ? new JsonGetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        private ActionResult GetBlob(BaseParams baseParams)
        {
            var result = this.LongTextService.Get(baseParams);

            return result.Success ? new JsonGetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }
    }
}