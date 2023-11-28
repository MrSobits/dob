namespace Bars.Gkh.Overhaul.Nso.Controllers
{
    using System.Collections;
    using System.Web.Mvc;
    using B4;
    using DomainService;
    using Gkh.Controllers.Dict.RealEstateType;
    using Overhaul.Controllers;

    public class RealEstateTypeController : BaseRealEstateTypeController
    {
        public INsoRealEstateTypeService Service { get; set; }

        public ActionResult GetMuList(BaseParams baseParams)
        {
            var result = (ListDataResult)Service.GetMuList(baseParams);
            return new JsonListResult((IList)result.Data);
        }
    }
}
