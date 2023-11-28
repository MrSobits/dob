namespace Bars.Gkh.Overhaul.Nso.Controllers
{
    using System.Web.Mvc;
    using B4;

    using DomainService;
    using Gkh.Entities.RealEstateType;

    public class RealEstateTypeRateController : B4.Alt.DataController<RealEstateTypeRate>
    {
        public ActionResult CalculateRates(BaseParams baseParams)
        {
            var result = (BaseDataResult)Container.Resolve<IRealEstateTypeRateService>().CalculateRates(baseParams);
            return result.Success ? JsSuccess() : JsFailure(result.Message);
        } 
    }
}