namespace Bars.GkhGji.Regions.Stavropol.Controller
{
    using System.Web.Mvc;

    using Bars.B4;
    using Bars.GkhGji.Regions.Stavropol.DomainService.ActCheck;

    public class ActCheckTimeController : BaseController
    {
        public IActCheckTimeService Service { get; set; }
        public ActionResult CreateActCheckTime(BaseParams baseParams)
        {
            var result = this.Service.CreateActCheckTime(baseParams);

            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }
    }
}
