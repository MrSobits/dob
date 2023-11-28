namespace Bars.GkhGji.Regions.Chelyabinsk.Controllers
{
    using System.Web.Mvc;
    using Bars.B4;
    using DomainService;
    using Bars.GkhGji.Entities;
    using Entities;
    using Bars.Gkh.Domain;

    public class ResolutionArticleLawController : B4.Alt.DataController<ResolutionArtLaw>
    {
        public ActionResult AddArticles(BaseParams baseParams)
        {
            var result = Container.Resolve<IResolutionArticleLawService>().AddArticles(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }
        public ActionResult GetListResolution(BaseParams baseParams)
        {
            var resolutionService = Container.Resolve<IResolutionArticleLawService>();
            try
            {
                return resolutionService.GetListResolution(baseParams).ToJsonResult();
            }
            finally
            {

            }
        }

        public ActionResult GetListDisposal(BaseParams baseParams)
        {
            var resolutionService = Container.Resolve<IResolutionArticleLawService>();
            try
            {
                return resolutionService.GetListDisposal(baseParams).ToJsonResult();
            }
            finally
            {

            }
        }

    }
}