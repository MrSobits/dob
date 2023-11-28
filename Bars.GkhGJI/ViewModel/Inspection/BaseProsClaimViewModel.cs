namespace Bars.GkhGji.ViewModel
{
    using System;
    using System.Linq;

    using B4;
    using B4.DataAccess;
    using B4.Utils;
    using Entities;
    using Enums;

    public class BaseProsClaimViewModel: BaseProsClaimViewModel<BaseProsClaim>
    {
    }

    public class BaseProsClaimViewModel<T> : BaseViewModel<T>
        where T : BaseProsClaim
    {
        public override IDataResult List(IDomainService<T> domainService, BaseParams baseParams)
        {
            /*
            * параметры:
            * dateStart - период с
            * dateEnd - период по
            */

            var serviceView = Container.ResolveDomain<ViewBaseProsClaim>();

            try
            {
                var loadParams = GetLoadParam(baseParams);

                var dateStart = baseParams.Params.GetAs<DateTime>("dateStart");
                var dateEnd = baseParams.Params.GetAs<DateTime>("dateEnd");
                var showCloseInspections = baseParams.Params.GetAs("showCloseInspections", true);

                var data = serviceView.GetAll()
                    .WhereIf(dateStart != DateTime.MinValue, x => x.ProsClaimDateCheck >= dateStart)
                    .WhereIf(dateEnd != DateTime.MinValue, x => x.ProsClaimDateCheck <= dateEnd)
                    .WhereIf(!showCloseInspections, x => x.State == null || !x.State.FinalState)
                    .Select(x => new
                    {
                        x.Id,
                        Municipality = x.MunicipalityNames,
                        MoSettlement = x.MoNames,
                        PlaceName = x.PlaceNames,
                        x.ContragentName,
                        x.ProsClaimDateCheck,
                        x.RealityObjectCount,
                        x.DocumentNumber,
                        x.InspectionNumber,
                        x.InspectorNames,
                        x.PersonInspection,
                        TypeJurPerson = x.PersonInspection == PersonInspection.PhysPerson ? null : x.TypeJurPerson,
                        x.State
                    }).Filter(loadParams, Container);

                int totalCount = data.Count();

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
            }
            finally 
            {
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
                    .FirstOrDefault(x => x.Inspection.Id == id && x.TypeDisposal == TypeDisposalGji.Base);

                if (disposal != null)
                    obj.DisposalId = disposal.Id;

                return new BaseDataResult(obj);
            }
            finally 
            {
                Container.Release(serviceDisposal);
            }
        }
    }
}