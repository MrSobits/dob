namespace Bars.Gkh.Controllers
{
    using System.Web.Mvc;

    using B4;

    using Entities;
    using DomainService;

    public class SupplyResourceOrgRealtyObjectController : B4.Alt.DataController<SupplyResourceOrgRealtyObject>
    {
        public ActionResult AddRealtyObjects(BaseParams baseParams)
        {
            var result = Container.Resolve<ISupplyResourceOrgRealtyObjectService>().AddRealtyObjects(baseParams);
            return result.Success ? new JsonNetResult() : JsonNetResult.Failure(result.Message);
        }
    }
}