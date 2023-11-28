namespace Bars.GkhGji.Regions.BaseChelyabinsk.ViewModel.Protocol197
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol197;

    public class Protocol197AnotherResolutionViewModel : BaseViewModel<Protocol197AnotherResolution>
    {
        public override IDataResult List(IDomainService<Protocol197AnotherResolution> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var documentId = baseParams.Params.GetAs<long>("documentId");

            var data = domainService.GetAll()
                .Where(x => x.Protocol197.Id == documentId)
                .Select(x => new
                    {
                        x.Id,
                        ArticleLaw = x.ArticleLaw.Name,
                        x.DocumentGji.DocumentNumber,
                        x.DocumentGji.DocumentDate,
                        x.DocumentGji.TypeDocumentGji,
                        Sanction = x.DocumentGji.Sanction.Name,
                        x.DocumentGji.Paided,
                        x.Description
                    })
                .Filter(loadParam, this.Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }
    }
}