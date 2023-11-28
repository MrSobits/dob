namespace Bars.GkhGji.Controllers
{
    using System.Web.Mvc;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;

    public class DocumentGjiInspectorController : B4.Alt.DataController<DocumentGjiInspector>
    {
        public ActionResult AddInspectors(BaseParams baseParams)
        {
            var result = Container.Resolve<IDocumentGjiInspectorService>().AddInspectors(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }
    }
}