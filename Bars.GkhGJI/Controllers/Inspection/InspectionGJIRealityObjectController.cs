namespace Bars.GkhGji.Controllers
{
    using System.Web.Mvc;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;

    public class InspectionGjiRealityObjectController : B4.Alt.DataController<InspectionGjiRealityObject>
    {
        public ActionResult AddRealityObjects(BaseParams baseParams)
        {
            var result = Container.Resolve<IInspectionGjiRealityObjectService>().AddRealityObjects(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }
    }
}