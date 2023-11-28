namespace Bars.Gkh.RegOperator.ViewModels
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.RegOperator.Entities;

    public class CalcAccountRealityObjectViewModel : BaseViewModel<CalcAccountRealityObject>
    {
        public override IDataResult List(IDomainService<CalcAccountRealityObject> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);

            var accountId = baseParams.Params.GetAsId("accId");

            var data = domainService.GetAll()
                .Where(x => x.Account.Id == accountId)
                .Where(x => x.DateStart <= DateTime.Today)
                .Where(x => !x.DateEnd.HasValue || x.DateEnd > DateTime.Today)
                .Select(
                    x => new
                    {
                        x.Id,
                        x.RealityObject.Address
                    })
                .Filter(loadParams, this.Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}