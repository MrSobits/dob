namespace Bars.GkhGji.ViewModel
{
    using System;
    using System.Linq;

    using B4;
    using B4.Utils;
    using Entities;
    using Enums;

    public class BaseJurPersonViewModel : BaseJurPersonViewModel<BaseJurPerson>
    {

    }

    public class BaseJurPersonViewModel<T> : BaseViewModel<T>
        where T: BaseJurPerson
    {
        public override IDataResult List(IDomainService<T> domainService, BaseParams baseParams)
        {
            var serviceInspInspector = Container.Resolve<IDomainService<InspectionGjiInspector>>();
            var serviceInspZonalInspection = Container.Resolve<IDomainService<InspectionGjiZonalInspection>>();
            var serviceView = Container.Resolve<IDomainService<ViewBaseJurPerson>>();

            try
            {
                var loadParams = baseParams.GetLoadParam();

                /*
                 * dateStart - период с
                 * dateEnd - период по
                 * planId - идентификатор плана
                 * inspectorIds - идентификаторы инспекторов
                 * zonalIsnpectionIds - идентификаторы отделов
                 */
                var dateStart = baseParams.Params.ContainsKey("dateStart") ? baseParams.Params["dateStart"].ToDateTime() : DateTime.MinValue;
                var dateEnd = baseParams.Params.ContainsKey("dateEnd") ? baseParams.Params["dateEnd"].ToDateTime() : DateTime.MinValue;
                var planId = baseParams.Params.ContainsKey("planId") ? baseParams.Params["planId"].ToLong() : 0;
                var showCloseInspections = baseParams.Params.GetAs("showCloseInspections", true);
                var inspectorIds = baseParams.Params.GetAs<long[]>("inspectorIds");
                var zonalIsnpectionIds = baseParams.Params.GetAs<long[]>("zonalIsnpectionIds");

#warning Здесь зачем тостояла эта проверка. Но при ней приходит = null и возвращается сразу пустой список. Незнаю кто и зачем так сделал но надо разобратся в дальнейшем
                /*
            if (inspectorIds == null)
            {
                return new ListDataResult(null,0);
            }
            */

                var data = serviceView.GetAll()
                    .WhereIf(inspectorIds != null && inspectorIds.Length > 0, y => serviceInspInspector.GetAll().Any(x => x.Inspection.Id == y.Id && inspectorIds.Contains(x.Inspector.Id)))
                    .WhereIf(zonalIsnpectionIds != null && zonalIsnpectionIds.Length > 0, y => serviceInspZonalInspection.GetAll().Any(x => x.Inspection.Id == y.Id && zonalIsnpectionIds.Contains(x.ZonalInspection.Id)))
                    .WhereIf(planId > 0, x => x.PlanId == planId)
                    .WhereIf(dateStart != DateTime.MinValue, x => x.DateStart >= dateStart)
                    .WhereIf(dateEnd != DateTime.MinValue, x => x.DateStart <= dateEnd)
                    .WhereIf(!showCloseInspections, x => x.State == null || !x.State.FinalState)
                    .Select(x => new
                    {
                        x.Id,
                        Municipality = x.MunicipalityNames,
                        MoSettlement = x.MoNames,
                        PlaceName = x.PlaceNames,
                        Contragent = x.ContragentName,
                        Plan = x.PlanName,
                        x.InspectionNumber,
                        x.DateStart,
                        x.CountDays,
                        x.RealityObjectCount,
                        x.DisposalNumber,
                        x.InspectorNames,
                        x.ZonalInspectionNames,
                        x.TypeFact,
                        x.State
                    })
                    .Filter(loadParams, Container);

                int totalCount = data.Count();

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
            }
            finally 
            {
                Container.Release(serviceInspZonalInspection);
                Container.Release(serviceInspInspector);
                Container.Release(serviceView);
            }
        }

        public override IDataResult Get(IDomainService<T> domainService, BaseParams baseParams)
        {
            var serviceDisposal = Container.Resolve<IDomainService<Disposal>>();

            try
            {
                var id = baseParams.Params["id"].To<long>();
                var obj = domainService.Get(id);

                // Получаем Распоряжение
                var disposal = serviceDisposal.GetAll()
                    .FirstOrDefault(x => x.Inspection.Id == id.ToLong() && x.TypeDisposal == TypeDisposalGji.Base);

                if (disposal != null)
                {
                    obj.DisposalId = disposal.Id;
                }

                return new BaseDataResult(obj);
            }
            finally 
            {
                Container.Release(serviceDisposal);
            }
            
        }
    }
}