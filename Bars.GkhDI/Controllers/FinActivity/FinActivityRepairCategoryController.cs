namespace Bars.GkhDi.Controllers
{
    using System.Web.Mvc;
    using B4;
    using DomainService;
    using Entities;

    public class FinActivityRepairCategoryController : B4.Alt.DataController<FinActivityRepairCategory>
    {
        public ActionResult AddWorkMode(BaseParams baseParams)
        {
            var result = (BaseDataResult)Resolve<IFinActivityRepairCategoryService>().AddWorkMode(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult AddDataByRealityObj(BaseParams baseParams)
        {
            var result = (BaseDataResult)Resolve<IFinActivityRepairCategoryService>().AddDataByRealityObj(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }
    }
}
