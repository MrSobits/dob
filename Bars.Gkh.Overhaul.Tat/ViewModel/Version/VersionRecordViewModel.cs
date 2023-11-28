namespace Bars.Gkh.Overhaul.Tat.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Entities;

    public class VersionRecordViewModel : BaseViewModel<VersionRecord>
    {
        public override IDataResult List(IDomainService<VersionRecord> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var version = loadParams.Filter.GetAs<long>("version");

            var municipalityId = loadParams.Filter.GetAs<long>("municipalityId");

            if (municipalityId == 0)
            {
                return new BaseDataResult(false, "Не задан параметр \"Муниципальное образование\"");   
            }

            if (loadParams.Order == null || loadParams.Order.Length == 0)
            {
                loadParams.Order = new[] { new OrderField { Asc = true, Name = "IndexNumber" } };
            }

            var data =
                domainService.GetAll()
                    .Where(x => x.ProgramVersion.Id == version && x.ProgramVersion.Municipality.Id == municipalityId)
                    .Select(x => new
                    {
                        x.Id,
                        Municipality = x.RealityObject.Municipality.Name,
                        RealityObject = x.RealityObject.Address,
                        x.CommonEstateObjects,
                        x.Year,
                        x.CorrectYear,
                        x.IndexNumber,
                        x.Point,
                        x.Sum,
                        x.TypeDpkrRecord
                    })
                    .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }
    }
}