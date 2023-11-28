namespace Bars.GkhDi.DomainService
{
    using System.Linq;
    using B4;

    using Entities;

    public class OtherServiceViewModel : BaseViewModel<OtherService>
    {
        public override IDataResult List(IDomainService<OtherService> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var disclosureInfoRealityObjId = baseParams.Params.GetAs<long>("disclosureInfoRealityObjId");

            var data = domainService.GetAll()
                .Where(x => x.DisclosureInfoRealityObj.Id == disclosureInfoRealityObjId)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Code,
                    x.UnitMeasure,
                    x.Tariff,
                    x.Provider
                })
                .Filter(loadParams, this.Container);

            var totalCount = data.Count();

            data = data.Order(loadParams).Paging(loadParams);

            return new ListDataResult(data.ToList(), totalCount);
        }
    }
}