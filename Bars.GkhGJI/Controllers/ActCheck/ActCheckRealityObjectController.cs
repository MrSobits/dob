namespace Bars.GkhGji.Controllers
{
    using System.Web.Mvc;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;

    public class ActCheckRealityObjectController : B4.Alt.DataController<ActCheckRealityObject>
    {
        public ActionResult SaveParams(BaseParams baseParams)
        {
            var result = Container.Resolve<IActCheckRealityObjectService>().SaveParams(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }
    }
}