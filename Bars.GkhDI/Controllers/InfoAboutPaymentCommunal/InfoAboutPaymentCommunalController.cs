namespace Bars.GkhDi.Controllers
{
    using System.Web.Mvc;
    using B4;
    using DomainService;
    using Entities;

    public class InfoAboutPaymentCommunalController : B4.Alt.DataController<InfoAboutPaymentCommunal>
    {
        public ActionResult SaveInfoAboutPaymentCommunal(BaseParams baseParams)
        {
            var result = (BaseDataResult)Resolve<IInfoAboutPaymentCommunalService>().SaveInfoAboutPaymentCommunal(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }
    }
}

