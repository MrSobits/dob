using System.Web.Mvc;
using Bars.B4;
using Bars.Gkh.DomainService;
using System.Collections;

namespace Bars.GkhGji.Regions.Tyumen.Controllers
{
    using Bars.Gkh.Entities;
    using DomainService;
    using System.Web.Mvc;
    using B4;
    using Bars.Gkh.Domain;


    public class ManOrgLicenseGisController : B4.Alt.DataController<ManOrgLicense>
    {

        public virtual IManOrgLicenseGisService Service { get; set; }

        public ActionResult GetListWithRO(BaseParams baseParams)
        {
            var totalCount = 0;
            var result = Service.GetListWithRO(baseParams, true, ref totalCount);
            return new JsonListResult(result, totalCount);
        }

        public override ActionResult Get(BaseParams baseParams)
        {
            var totalCount = 0;

            var result = Service.GetRO(baseParams, true, ref totalCount);

            return new JsonListResult(result, totalCount);
        }
    }
}