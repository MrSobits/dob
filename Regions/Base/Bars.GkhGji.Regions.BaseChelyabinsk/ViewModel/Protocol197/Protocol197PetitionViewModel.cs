namespace Bars.GkhGji.Regions.BaseChelyabinsk.ViewModel.Protocol197
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol197;

    public class Protocol197PetitionViewModel : BaseViewModel<Protocol197Petition>
    {
        public override IDataResult List(IDomainService<Protocol197Petition> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var documentId = baseParams.Params.ContainsKey("documentId")
                                   ? baseParams.Params["documentId"].ToLong()
                                   : 0;

            var data = domainService.GetAll()
                .Where(x => x.Protocol197.Id == documentId)
                .Select(x => new
                {
                    x.Id,
                    x.PetitionAuthorFIO,
                    x.PetitionAuthorDuty,
                    x.Workplace,
                    x.Aprooved,
                    x.PetitionDate
                })
                .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).ToList(), totalCount);
        }
    }
}