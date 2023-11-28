namespace Bars.Gkh.Controllers
{
    using System.Web.Mvc;

    using Bars.B4;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;

    public class BelayPolicyController : B4.Alt.DataController<BelayPolicy>
    {
        public ActionResult GetInfo(BaseParams baseParams)
        {
            var result = Container.Resolve<IBelayPolicyService>().GetInfo(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }
    }
}