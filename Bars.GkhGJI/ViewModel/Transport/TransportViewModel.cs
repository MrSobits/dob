namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    public class TransportViewModel : BaseViewModel<Transport>
    {
        public override IDataResult List(IDomainService<Transport> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var protocolId = baseParams.Params.ContainsKey("protocolId")
                                   ? baseParams.Params["protocolId"].ToLong()
                                   : 0;

            var data = domainService.GetAll()
          
                .Select(x => new
                {
                    x.Id,
                    x.NameTransport,
                    x.NamberTransport,
                    x.RegistrationNamberTransport,
                    x.SeriesTransport,
                    x.RegNamberTransport,
                })
                .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).ToList(), totalCount);
        }
    }
}