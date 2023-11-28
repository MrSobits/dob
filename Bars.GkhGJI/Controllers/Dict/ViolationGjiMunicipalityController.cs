namespace Bars.GkhGji.Controllers
{
    using System.Web.Mvc;
    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;

    public class ViolationGjiMunicipalityController : B4.Alt.DataController<ViolationGjiMunicipality>
    {
        public ActionResult AddMunicipalites(BaseParams baseParams)
        {
            var result = this.Container.Resolve<IViolationGjiMunicipality>().AddMunicipalities(baseParams);
            return result.Success ? JsonNetResult.Success : JsonNetResult.Failure(result.Message);
        }
    }
}
