namespace Bars.GkhGji.Regions.Tomsk.Controllers
{
    using System.Web.Mvc;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Regions.Tomsk.Entities;

    public class AdministrativeCaseViolController : B4.Alt.DataController<AdministrativeCaseViolation>
    {
        /// <summary>
        /// Метод добавления нарушений 
        /// </summary>
        public ActionResult AddViolations(BaseParams baseParams)
        {
            var result = Container.Resolve<IAdminCaseViolService>().AddViolations(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }
    }
}