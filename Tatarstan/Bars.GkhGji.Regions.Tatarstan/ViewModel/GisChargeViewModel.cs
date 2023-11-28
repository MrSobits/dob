namespace Bars.GkhGji.Regions.Tatarstan.ViewModel
{
    using System.Linq;
    using B4;
    using B4.Utils;
    using Entities;

    public class GisChargeViewModel : BaseViewModel<GisChargeToSend>
    {
        public override IDataResult List(IDomainService<GisChargeToSend> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var onlyUnsend = baseParams.Params.GetAs<bool>("onlyUnsend");

            var data = domainService.GetAll()
                .WhereIf(onlyUnsend, x => !x.IsSent)
                .Select(x => new
                {
                    x.Id,
                    Resolution =
                        x.Resolution.DocumentDate.HasValue
                            ? x.Resolution.DocumentNumber + " от " + x.Resolution.DocumentDate.Value.ToString("d")
                            : x.Resolution.DocumentNumber,
                    x.DateSend,
                    x.IsSent,
                    x.JsonObject
                })
                /*.AsEnumerable()
                .Select(x => new
                {
                    x.Id,
                    x.Resolution,
                    x.DateSend,
                    x.IsSent,
                    JsonObject = JsonConvert.SerializeObject(x.JsonObject)
                })
                .AsQueryable()*/
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}