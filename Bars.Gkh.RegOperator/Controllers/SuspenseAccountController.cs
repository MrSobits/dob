using Bars.Gkh.RegOperator.Distribution;

namespace Bars.Gkh.RegOperator.Controllers
{
    using System.Web.Mvc;

    using B4;
    using Entities;

    public class SuspenseAccountController : B4.Alt.DataController<SuspenseAccount>
    {
        public ActionResult ListDistribution(BaseParams baseParams)
        {
            var distributionProvider = Container.Resolve<IDistributionProvider>();

            return new JsonNetResult(distributionProvider.List(baseParams));
        }
    }
}