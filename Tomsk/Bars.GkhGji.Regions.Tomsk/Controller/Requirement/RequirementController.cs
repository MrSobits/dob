namespace Bars.GkhGji.Regions.Tomsk.Controller
{
    using System.Web.Mvc;
    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.GkhGji.Regions.Tomsk.DomainService;
    using Bars.GkhGji.Regions.Tomsk.Entities;

    public class RequirementController : FileStorageDataController<Requirement>
    {
        public ActionResult GetInfo(BaseParams baseParams)
        {
            var result = Container.Resolve<IRequirementService>().GetInfo(baseParams);
            return JsSuccess(result.Data);
        }
    }
}
