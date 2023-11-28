namespace Bars.GkhGji.Regions.Tomsk.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Regions.Tomsk.Entities;

    public class DisposalSubjectVerificationViewModel : BaseViewModel<DisposalSubjectVerification>
        {
        public override IDataResult List(IDomainService<DisposalSubjectVerification> domain, BaseParams baseParams)
            {
                var loadParam = baseParams.GetLoadParam();

                var documentId = baseParams.Params.ContainsKey("documentId")
                                     ? baseParams.Params["documentId"].ToInt()
                                     : 0;

                var data = domain
                    .GetAll()
                    .Where(x => x.Disposal.Id == documentId)
                    .Select(x => new
                    {
                        x.Id,
                        x.SubjectVerification.Name,
                        x.SubjectVerification.Code
                    })
                    .Filter(loadParam, Container);

                int totalCount = data.Count();

                return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
            }
        } 
    
}