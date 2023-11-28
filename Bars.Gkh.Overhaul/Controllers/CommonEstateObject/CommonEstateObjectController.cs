namespace Bars.Gkh.Overhaul.Controllers
{
    using System.Collections;
    using System.Web.Mvc;

    using B4;
    using DomainService;
    using Entities;
    using Gkh.Entities.CommonEstateObject;

    public class CommonEstateObjectController : B4.Alt.DataController<CommonEstateObject>
    {
        public ActionResult ListTree(BaseParams baseParams)
        {
            var result = Container.Resolve<IListCeoService>().ListTree(baseParams);
            return new JsonNetResult(result.Data);
        }

        public ActionResult AddWorks(BaseParams baseParams)
        {
            var result = (BaseDataResult)Resolve<ICommonEstateObjectService>().AddWorks(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }
        
        public ActionResult Export(BaseParams baseParams)
        {
            return new ReportStreamResult(Resolve<ICommonEstateObjectService>().PrintReport(baseParams), "ceo_export.xlsx");
        }

        public ActionResult ListForRealObj(BaseParams baseParams)
        {
            var result = (ListDataResult)Container.Resolve<ICommonEstateObjectService>().ListForRealObj(baseParams);
            return result.Success ? new JsonListResult((IList)result.Data, result.TotalCount) : JsonNetResult.Failure(result.Message);
        }
    }
}