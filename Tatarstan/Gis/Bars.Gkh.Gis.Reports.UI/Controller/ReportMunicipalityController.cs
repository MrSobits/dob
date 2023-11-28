namespace Bars.Gkh.Gis.Reports.UI.Controller
{
    using System.Web.Mvc;
    using B4;
    using Billing.Core.Controllers.Alt;
    using DomainService;
    using Entities;

    class ReportMunicipalityController : BillingDataController<ReportMunicipality>
    {
        public ActionResult ListWithoutPaging(BaseParams baseParams)
        {
            var result = (ListDataResult)this.Container.Resolve<IReportMunicipalityService>().ListWithoutPaging(baseParams);

            if (result.Success)
            {
                return new JsonNetResult(new { success = true, data = result.Data, totalCount = result.TotalCount });
            }

            return JsonNetResult.Message(result.Message);
        }
    }
}
