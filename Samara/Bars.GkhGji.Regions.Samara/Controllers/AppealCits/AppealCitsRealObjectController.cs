namespace Bars.GkhGji.Regions.Samara.Controllers
{
    using System.Web.Mvc;
    using Bars.B4;
    using Bars.GkhGji.Regions.Samara.DomainService;
    using Bars.GkhGji.Regions.Samara.Entities;

    public class AppealCitsTesterController : B4.Alt.DataController<AppealCitsTester>
    {
        public ActionResult AddTesters(BaseParams baseParams)
        {
            var service = Container.Resolve<IAppealCitsTesterService>();
            try
            {
                var result = service.AddTesters(baseParams);
                return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
            }
            finally 
            {
                Container.Release(service);
            }
        }
    }
}