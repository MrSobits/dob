namespace Bars.GkhGji.Regions.Tomsk.Controller
{
    using System.Web.Mvc;

    using Bars.B4;
    using Bars.GkhGji.Regions.Tomsk.DomainService;
    using Bars.GkhGji.Regions.Tomsk.Entities;

    public class AdministrativeCaseArticleLawController : B4.Alt.DataController<AdministrativeCaseArticleLaw>
    {
        public ActionResult AddArticles(BaseParams baseParams)
        {
            var result = Container.Resolve<IAdministrativeCaseArticleLawService>().AddArticles(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }
    }
}