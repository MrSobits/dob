namespace Bars.Gkh.Overhaul.Controllers
{
    using System.Web.Mvc;
    using B4;
    using DomainService;
    using Entities;

    public class PaysizeRecordController : B4.Alt.DataController<PaysizeRecord>
    {
        public ActionResult ListTree(BaseParams baseParams)
        {
            var result = Resolve<IPaysizeRecordService>().ListTree(baseParams);
            return new JsonNetResult(result.Data);
        }
    }
}