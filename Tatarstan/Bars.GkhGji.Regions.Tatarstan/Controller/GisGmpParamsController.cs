namespace Bars.GkhGji.Regions.Tatarstan.Controller
{
    using System.Web.Mvc;
    using B4;
    using DomainService;

    public class GisGmpParamsController : BaseController
    {
        public ActionResult GetParams()
        {
            var result = Resolve<IGjiTatParamService>().GetConfig();

            return new JsonNetResult(result);
        }

        public ActionResult SaveParams(BaseParams baseParams)
        {
            var result = Resolve<IGjiTatParamService>().SaveConfig(baseParams);

            return result.Success ? new JsonNetResult() : JsonNetResult.Failure(result.Message);
        }
    }
}