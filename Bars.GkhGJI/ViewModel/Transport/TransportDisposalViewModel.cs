namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    public class TransportDisposalViewModel : BaseViewModel<TransportDisposal>
    {
        public override IDataResult List(IDomainService<TransportDisposal> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var disposalId = baseParams.Params.ContainsKey("disposalId")
                                   ? baseParams.Params["disposalId"].ToLong()
                                   : 0;

            var data = domainService.GetAll()
                .Where(x => x.Disposal.Id == disposalId)
                .Select(x => new
                {
                    x.Id,
                    x.Disposal,
                    x.Transport,
                })
                .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).ToList(), totalCount);
        }
    }
}