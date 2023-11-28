namespace Bars.Gkh.Overhaul.Nso.Controllers
{
    using System.Collections;
    using System.Web.Mvc;
    using B4;
    using DomainService;

    public class PriorityParamController : BaseController
    {
        public IPriorityParamService Service { get; set; }

        public ActionResult List(BaseParams baseParams)
        {
            var result = (ListDataResult)Service.List(baseParams);
            return new JsonListResult((IList)result.Data, result.TotalCount);
        }
    }
}