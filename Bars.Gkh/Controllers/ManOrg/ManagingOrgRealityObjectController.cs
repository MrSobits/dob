namespace Bars.Gkh.Controllers
{
    using System.Web.Mvc;

    using Bars.B4;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;

    public class ManagingOrgRealityObjectController : B4.Alt.DataController<ManagingOrgRealityObject>
    {
        public ActionResult AddRealityObjects(BaseParams baseParams)
        {
            var result = Container.Resolve<IManagingOrgRealityObjectService>().AddRealityObjects(baseParams);
            return result.Success ? new JsonNetResult() : JsonNetResult.Failure(result.Message);
        }
    }
}