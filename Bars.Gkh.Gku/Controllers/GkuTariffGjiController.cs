namespace Bars.Gkh.Gku.Controllers
{
    using System.Web.Mvc;

    using Bars.B4;
    using DomainService;
    using Entities;

    public class GkuTariffGjiController : B4.Alt.DataController<GkuTariffGji>
    {
        public ActionResult GetContragentsList(BaseParams baseParams)
        {
            var result = Resolve<IGkuTariffGjiService>().GetContragents(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }
    }
}
