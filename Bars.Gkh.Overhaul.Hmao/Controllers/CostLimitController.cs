using Bars.B4;
using Bars.Gkh.Entities.Dicts;
using Bars.Gkh.Overhaul.Hmao.DomainService.Version;
using Bars.Gkh.Overhaul.Hmao.Entities;
using Bars.GkhCr.Entities;
using System;
using System.Web.Mvc;

namespace Bars.Gkh.Overhaul.Hmao.Controllers
{
    public class CostLimitController : B4.Alt.DataController<CostLimit>
    {
        #region Properties

        public ICostService CostService { get; set; }

        public IDomainService<TypeWorkCr> TypeWorkCrDomainService { get; set; }

        public IDomainService<Work> WorkDomainService { get; set; }

        #endregion

        public ActionResult GetCostKpkr(BaseParams baseParams)
        {
            try
            {
                var WorkId = baseParams.Params.GetAs<long>("WorkId");
                if (WorkId == 0)
                    return new JsonNetResult(null);
                var work = WorkDomainService.Get(WorkId);
                if (work == null)
                    return JsonNetResult.Failure($"Не найден ork с id {WorkId}");

                var YearRepair = baseParams.Params.GetAs<short>("YearRepair");
                var Volume = baseParams.Params.GetAs<decimal>("Volume");

                var TypeWorkCrId = baseParams.Params.GetAs<long>("TypeWorkCrId");
                if (TypeWorkCrId == 0)
                    return new JsonNetResult(null);
                var typeWork = TypeWorkCrDomainService.Get(TypeWorkCrId);
                if (typeWork == null)
                    return JsonNetResult.Failure($"Не найден TypeWorkCr с id {TypeWorkCrId}");

                var municipality = typeWork.ObjectCr.RealityObject.Municipality;
                var floors = typeWork.ObjectCr.RealityObject.Floors;

                var cost = CostService.GetCost(typeWork.ObjectCr.RealityObject, work, YearRepair, Volume);

                return new JsonNetResult(cost);
            }
            catch (Exception e)
            {
                return JsonNetResult.Failure(e.Message);
            }
        }
    }
}
