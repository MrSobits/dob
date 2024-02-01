namespace Bars.Gkh.ViewModel
{
    using System.Linq;

    using B4;

    using Entities;

    public class InspectorZonalInspSubscriptionViewModel : BaseViewModel<InspectorZonalInspSubscription>
    {
        public override IDataResult List(IDomainService<InspectorZonalInspSubscription> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var inpectorId = baseParams.Params.GetAs<long>("inpectorId");

            var data = domainService.GetAll()
                .Where(x => x.Inspector.Id == inpectorId)
                .Select(x => new
                    {
                        x.Id,
                        ZonalInspName = x.ZonalInspection.Name,
                        ZonalInspAddress = x.ZonalInspection.Address
                    })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}