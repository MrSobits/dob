namespace Bars.Gkh.Controllers
{
    using System.Web.Mvc;

    using B4;

    using Entities;
    using DomainService;

    public class SupplyResourceOrgMunicipalityController : B4.Alt.DataController<SupplyResourceOrgMunicipality>
    {
        public ActionResult AddMunicipalities(BaseParams baseParams)
        {
            var result = Container.Resolve<ISupplyResourceOrgMunicipalityService>().AddMunicipalities(baseParams);
            return result.Success ? new JsonNetResult() : JsonNetResult.Failure(result.Message);
        }
    }
}