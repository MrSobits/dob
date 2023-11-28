namespace Bars.GkhDi.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using B4;
    using B4.Modules.FileStorage;
    using DomainService;
    using Entities;

    public class AdminRespController : FileStorageDataController<AdminResp>
    {
        public ActionResult AddAdminRespByResolution(BaseParams baseParams)
        {
            var result = (BaseDataResult)Resolve<IAdminRespService>().AddAdminRespByResolution(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }
    }
}