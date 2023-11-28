namespace Bars.GkhDi.Controllers
{
    using System.Web.Mvc;
    using B4;
    using DomainService;
    using Entities;

    /// <summary>
    /// The plan work service repair controller.
    /// </summary>
    public class PlanWorkServiceRepairController : B4.Alt.DataController<PlanWorkServiceRepair>
    {
        public ActionResult AddTemplateService(BaseParams baseParams)
        {
            var result = (BaseDataResult)Resolve<IPlanWorkServiceRepairService>().AddTemplateService(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult ReloadWorkRepairList(BaseParams baseParams)
        {
            var result = (BaseDataResult)Resolve<IPlanWorkServiceRepairService>().ReloadWorkRepairList(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }
    }
}

