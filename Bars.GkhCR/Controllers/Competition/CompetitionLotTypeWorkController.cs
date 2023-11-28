namespace Bars.GkhCr.Controllers
{
    using System.Web.Mvc;

    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.GkhCr.DomainService;
    using Bars.GkhCr.Entities;

    public class CompetitionLotTypeWorkController : FileStorageDataController<CompetitionLotTypeWork>
    {
        public ActionResult AddWorks(BaseParams baseParams)
        {
            var service = Container.Resolve<ICompetitionLotTypeWorkService>();
            try
            {
                var result = service.AddWorks(baseParams);
                return result.Success ? new JsonNetResult() : JsFailure(result.Message);
            }
            finally 
            {
                Container.Release(service);
            }
        }
    }
}
