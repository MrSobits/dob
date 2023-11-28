﻿namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using B4;
    using B4.Utils;
    using Entities;
    using Enums;

    public class BaseStatementViewModel: BaseStatementViewModel<BaseStatement>
    {
    }

    public class BaseStatementViewModel<T> : BaseViewModel<T>
        where T: BaseStatement
    {
        public override IDataResult List(IDomainService<T> domainService, BaseParams baseParams)
        {
            /*
             * параметры:
             * realityObjectId - жилой дом
             * showCloseInspections - признак bool показыват ьили нет закрытые проверки
             */

            var serviceInspRobject = Container.Resolve<IDomainService<InspectionGjiRealityObject>>();
            var serviceView = Container.Resolve<IDomainService<ViewBaseStatement>>();

            try
            {
                var loadParam = GetLoadParam(baseParams);

                var realityObjectId = baseParams.Params.GetAs<long>("realityObjectId");
                var showCloseInspections = baseParams.Params.GetAs("showCloseInspections", true);

                var data = serviceView.GetAll()
                    .WhereIf(realityObjectId > 0, y => serviceInspRobject.GetAll().Any(x => x.RealityObject.Id == realityObjectId && x.Inspection.Id == y.Id))
                    .WhereIf(!showCloseInspections, x => x.State == null || !x.State.FinalState)
                    .Select(x => new
                    {
                        x.Id,
                        Municipality = x.MunicipalityNames,
                        MoSettlement = x.MoNames,
                        PlaceName = x.PlaceNames,
                        x.RealityObjectCount,
                        x.ContragentName,
                        x.PersonInspection,
                        x.InspectionNumber,
                        TypeJurPerson = x.PersonInspection == PersonInspection.PhysPerson ? null : x.TypeJurPerson,
                        x.IsDisposal,
                        x.RealObjAddresses,
                        x.DocumentNumber,
                        x.State
                    })
                    .Filter(loadParam, Container);

                var totalCount = data.Count();

                return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
            }
            finally 
            {
                Container.Release(serviceView);
                Container.Release(serviceInspRobject);
            }
        }

        public override IDataResult Get(IDomainService<T> domainService, BaseParams baseParams)
        {
            var serviceDiposal = Container.Resolve<IDomainService<Disposal>>();
            try
            {
                var id = baseParams.Params["id"].To<long>();
                var obj = domainService.Get(id);

                // Получаем Распоряжение
                var disposal = serviceDiposal.GetAll()
                    .FirstOrDefault(x => x.Inspection.Id == id && x.TypeDisposal == TypeDisposalGji.Base);

                if (disposal != null)
                    obj.DisposalId = disposal.Id;

                return new BaseDataResult(obj);
            }
            finally 
            {
                Container.Release(serviceDiposal);
            }
        }
    }
}