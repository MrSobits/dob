namespace Bars.GkhDi.DomainService
{
    using System;
    using System.Linq;
    using B4;
    using B4.Utils;
    using Entities;
    using Gkh.Domain;

    public class InformationOnContractsViewModel : BaseViewModel<InformationOnContracts>
    {
        public override IDataResult List(IDomainService<InformationOnContracts> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var disclosureInfoId = baseParams.Params.GetAs<long>("disclosureInfoId");

            var disclosureInfo =
                Container.Resolve<IDomainService<DisclosureInfo>>().GetAll()
                    .Where(x => x.Id == disclosureInfoId)
                    .Select(x => new {x.PeriodDi, x.ManagingOrganization})
                    .FirstOrDefault();

            if (disclosureInfo == null)
            {
                return new ListDataResult();
            }

            var periodDi = disclosureInfo.PeriodDi;

            var data = domainService.GetAll()
                .Where(x => x.DisclosureInfo.ManagingOrganization.Id == disclosureInfo.ManagingOrganization.Id)

                // Фильтрация по датам документа. Период дата начала - дата конца документа должен пересекаться
                // с периодом дата начала - дата конца периода раскрытия
                .Where(x => periodDi != null
                        && (((x.DateStart.HasValue && x.DateStart >= periodDi.DateStart.Value || !x.DateStart.HasValue)
                            && (x.DateStart.HasValue && periodDi.DateEnd.Value >= x.DateStart))
                            || ((x.DateStart.HasValue && periodDi.DateStart.Value >= x.DateStart || !x.DateStart.HasValue)
                            && (x.DateEnd.HasValue && x.DateEnd >= periodDi.DateStart.Value || !x.DateEnd.HasValue || x.DateEnd <= DateTime.MinValue))
                            )
                        // ToDo проверять данный дом вообще попадает в данный период
                )
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.From,
                    x.Number,
                    x.DateStart,
                    x.DateEnd,
                    x.PartiesContract,
                    x.Cost,
                    x.Comments,
                    x.RealityObject.FiasAddress.AddressName
                })
                .OrderIf(loadParams.Order.Length == 0, true, x => x.AddressName)
                .Filter(loadParams, Container);
            var totalCount = data.Count();

            data = data.Order(loadParams).Paging(loadParams);

            return new ListDataResult(data.ToList(), totalCount);
        }

        public override IDataResult Get(IDomainService<InformationOnContracts> domainService, BaseParams baseParams)
        {
            var obj = domainService.Get(baseParams.Params.GetAsId("id"));

            return new BaseDataResult(
                new
                {
                    obj.Id,
                    DisclosureInfo = obj.DisclosureInfo.Id,
                    obj.RealityObject,
                    obj.Name,
                    obj.Number,
                    obj.Cost,
                    obj.From,
                    obj.DateStart,
                    obj.DateEnd,
                    obj.PartiesContract,
                    obj.Comments
                });
        }
    }
}