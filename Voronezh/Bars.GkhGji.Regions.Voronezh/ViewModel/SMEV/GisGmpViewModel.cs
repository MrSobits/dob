namespace Bars.GkhGji.Regions.Voronezh.ViewModel
{
    using B4;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol197;
    using Castle.Core.Internal;
    using Entities;
    using System;
    using System.Linq;

    public class GisGmpViewModel : BaseViewModel<GisGmp>
    {
        public override IDataResult List(IDomainService<GisGmp> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var inspectorId = this.Container.Resolve<IGkhUserManager>().GetActiveOperator().Inspector.Id;
            var zonaInspId = Container.Resolve<IDomainService<ZonalInspectionInspector>>().GetAll()
                .Where(x => x.Inspector.Id == inspectorId)
                .Select(x => x.ZonalInspection.Id)
                .FirstOrDefault();
            var protocolList = this.Container.Resolve<IDomainService<Protocol197>>().GetAll()
                .Where(x => x.ComissionMeeting.ZonalInspection.Id == zonaInspId)
                .ToList();

            var payments = this.Container.Resolve<IDomainService<PayReg>>().GetAll()
                .Where(x => x.GisGmp != null)
                .GroupBy(x => x.GisGmp.Id)
                .Select(x => new
                {
                    x.Key,
                    Sum = (decimal)x.Sum(y => y.Amount)
                }).ToDictionary(x => x.Key, x => x.Sum);

            var data = domainService.GetAll()
                .Where(x => protocolList.Contains(x.Protocol))
                .AsEnumerable()
                .Select(x=> new
                {
                    x.Id,
                    RequestDate = x.ObjectCreateDate,
                    x.GisGmpPaymentsType,
                    Inspector = x.Inspector.Fio,
                    x.RequestState,
                    x.AltPayerIdentifier,
                    x.UIN,
                    x.BillFor,
                    x.TotalAmount,
                    PaymentsAmount = payments.ContainsKey(x.Id)? payments[x.Id]:0,
                    x.GisGmpChargeType,
                    x.MessageId, 
                    x.CalcDate,
                    ViolatorFio = x.Protocol.IndividualPerson != null ? x.Protocol.IndividualPerson.Fio : !x.Protocol.Fio.IsNullOrEmpty() ? x.Protocol.Fio : "",
                }).AsQueryable()
                .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}
