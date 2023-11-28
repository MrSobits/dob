namespace Bars.Gkh.Repair.Controllers
{
    using System.Web.Mvc;

    using Bars.B4;
    using Bars.Gkh.Repair.Entities;
    using Bars.Gkh.Repair.DomainService;

    public class RepairProgramController : B4.Alt.DataController<RepairProgram>
    {
        public IRepairProgramService RepairProgramService { get; set; }

        public ActionResult ListWithoutPaging(BaseParams baseParams)
        {
            var result = (ListDataResult)RepairProgramService.ListWithoutPaging(baseParams);
            return new JsonNetResult(new { success = true, data = result.Data, totalCount = result.TotalCount });
        }

        public ActionResult ListAvailableRealtyObjects(BaseParams baseParams)
        {
            var result = (ListDataResult)RepairProgramService.ListAvailableRealtyObjects(baseParams);
            return new JsonNetResult(new { success = true, data = result.Data, totalCount = result.TotalCount });
        }
    }
}
