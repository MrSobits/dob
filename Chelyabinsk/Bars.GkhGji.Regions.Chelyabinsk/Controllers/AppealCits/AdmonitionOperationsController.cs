namespace Bars.GkhGji.Regions.Chelyabinsk.Controllers
{
    using System.Web.Mvc;
    using Bars.B4;
    using DomainService;
    using Bars.GkhGji.Entities;
    using Entities;
    using Bars.Gkh.Domain;

    public class AdmonitionOperationsController : BaseController
    {
        public ActionResult ListAdmonitionForSelect(BaseParams baseParams)
        {
            var service = Container.Resolve<IAdmonitionOperationsService>();
            try
            {
                var result = service.ListDocsForSelect(baseParams);
                return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
            }
            finally
            {
                Container.Release(service);
            }
        }


    }
}