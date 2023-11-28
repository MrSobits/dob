namespace Bars.Gkh.Overhaul.Nso.Controllers
{
    using System.Web.Mvc;

    using Bars.B4;
    using Bars.Gkh.Overhaul.Nso.DomainService;

    public class LongTermProgramController : BaseController
    {
        public ActionResult List(BaseParams baseParams)
        {
            return new JsonNetResult(Resolve<ILongTermProgramService>().List(baseParams));
        }
    }
}
