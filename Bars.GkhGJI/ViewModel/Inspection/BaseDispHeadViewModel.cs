namespace Bars.GkhGji.ViewModel
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class BaseDispHeadViewModel : BaseDispHeadViewModel<BaseDispHead>
    {
    }

    public class BaseDispHeadViewModel<T> : BaseViewModel<T>
        where T: BaseDispHead
    {
        public override IDataResult List(IDomainService<T> domainService, BaseParams baseParams)
        {
            var service = Container.Resolve<IDomainService<ViewBaseDispHead>>();
            var serviceInsRo = Container.Resolve<IDomainService<InspectionGjiRealityObject>>();

            try
            {
                var loadParams = GetLoadParam(baseParams);

                var dateStart = baseParams.Params.GetAs("dateStart", DateTime.MinValue);
                var dateEnd = baseParams.Params.GetAs("dateEnd", DateTime.MinValue);
                var realityObjectId = baseParams.Params.GetAs<long>("realityObjectId");
                var showCloseInspections = baseParams.Params.GetAs("showCloseInspections", true);

                var data = service.GetAll()
                    .WhereIf(dateStart != DateTime.MinValue, x => x.DispHeadDate >= dateStart)
                    .WhereIf(dateEnd != DateTime.MinValue, x => x.DispHeadDate <= dateEnd)
                    .WhereIf(realityObjectId > 0, y => serviceInsRo.GetAll()
                        .Any(x => x.RealityObject.Id == realityObjectId && x.Inspection.Id == y.Id))
                    .WhereIf(!showCloseInspections, x => x.State == null || !x.State.FinalState)
                    .Select(x => new
                    {
                        x.Id,
                        x.MunicipalityNames,
                        MoSettlement = x.MoNames,
                        PlaceName = x.PlaceNames,
                        x.ContragentName,
                        x.DispHeadDate,
                        x.RealityObjectCount,
                        Head = x.HeadFio,
                        x.InspectorNames,
                        x.InspectionNumber,
                        DispHeadNumber = x.DocumentNumber,
                        x.DisposalTypeSurveys,
                        x.PersonInspection,
                        TypeJurPerson = x.PersonInspection == PersonInspection.PhysPerson ? null : x.TypeJurPerson,
                        x.State
                    }).Filter(loadParams, Container); ;
                  //  .Filter(loadParams, Container);

                int totalCount = data.Count();

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
            }
            finally 
            {
                Container.Release(service);
                Container.Release(serviceInsRo);
            }
        }

        public override IDataResult Get(IDomainService<T> domainService, BaseParams baseParams)
        {
            var serviceDisposal = Container.Resolve<IDomainService<Disposal>>();
            var serviceInsDocRef = Container.Resolve<IDomainService<InspectionDocGjiReference>>();

            try
            {
                var id = baseParams.Params["id"].To<long>();
                var obj = domainService.Get(id);

                // Получаем Распоряжение
                var disposal = serviceDisposal.GetAll()
                    .FirstOrDefault(x => x.Inspection.Id == id);

                //получаем предыдущий документ 
                var prevDoc = serviceInsDocRef.GetAll()
                    .FirstOrDefault(x => x.Inspection.Id == id);

                if (disposal != null)
                {
                    obj.DisposalId = disposal.Id;
                }

                if (prevDoc != null)
                {
                    obj.PrevDocument = prevDoc.Document;
                }

                return new BaseDataResult(obj);
            }
            finally 
            {
                Container.Release(serviceDisposal);
                Container.Release(serviceInsDocRef);
            }
            
        }
    }
}