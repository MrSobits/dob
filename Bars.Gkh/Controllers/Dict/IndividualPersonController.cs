namespace Bars.Gkh.Controllers
{
    using System.Collections;
    using System.Web.Mvc;

    using Bars.B4;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;

    public class IndividualPersonController : B4.Alt.DataController<IndividualPerson>
    {
        public IIndividualPersonService service { get; set; }

        public ActionResult GetInfo(BaseParams baseParams)
        {
            var result = service.GetInfo(baseParams);
            if (result.Success)
            {
                return new JsonNetResult(result.Data);
            }

            return JsonNetResult.Failure(result.Message);
        }

    }
}