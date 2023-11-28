namespace Bars.GkhCr.Controllers
{
    using System.Web.Mvc;

    using Bars.B4;
    using Bars.GkhCr.DomainService;
    using Bars.GkhCr.Entities;

    public class FinanceSourceWorkController : B4.Alt.DataController<FinanceSourceWork>
    {
        public ActionResult AddWorks(BaseParams baseParams)
        {
            var result = (BaseDataResult)Container.Resolve<IFinanceSourceWorkService>().AddWorks(baseParams);
            return result.Success ? JsonNetResult.Success : JsonNetResult.Failure(result.Message);
        }

        /// <summary>
        /// метод для получения списка работ по финансовой деятельности, используется в реестре объекте КР -> виды работ
        /// </summary>
        /// <returns></returns>
        public ActionResult ListWorksByFinSource(BaseParams baseParams)
        {
            var result = (BaseDataResult)Container.Resolve<IFinanceSourceWorkService>().ListWorksByFinSource(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }
    }
}
