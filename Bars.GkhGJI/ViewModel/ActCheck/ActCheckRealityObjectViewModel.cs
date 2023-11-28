namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    public class ActCheckRealityObjectViewModel : BaseViewModel<ActCheckRealityObject>
    {
        public override IDataResult List(IDomainService<ActCheckRealityObject> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var documentId = baseParams.Params.ContainsKey("documentId") ? baseParams.Params["documentId"].ToLong() : 0;

            var serviceViolations = Container.Resolve<IDomainService<ActCheckViolation>>();

            var dictViolations = serviceViolations.GetAll()
                   .Where(x => x.ActObject.ActCheck.Id == documentId)
                   .GroupBy(x => x.ActObject.Id)
                   .AsEnumerable()
                   .ToDictionary(x => x.Key, y => y.Count());

            var data = domainService.GetAll()
                .Where(x => x.ActCheck.Id == documentId)
                .Select(x => new
                {
                    x.Id,
                    RealityObject = x.RealityObject.Address,
                    Municipality = x.RealityObject.Municipality.Name,
                    x.HaveViolation,
                    x.Description,
                    x.NotRevealedViolations
                })
                .OrderIf(loadParam.Order.Length == 0, true, x => x.Municipality)
                .OrderThenIf(loadParam.Order.Length == 0, true, x => x.RealityObject)
                .AsEnumerable()
                .Select(x => new
                {
                    x.Id,
                    x.RealityObject,
                    x.Municipality,
                    x.HaveViolation,
                    x.Description,
                    ViolationCount = dictViolations.ContainsKey(x.Id) ? dictViolations[x.Id] : 0,
                    x.NotRevealedViolations
                })
                .AsQueryable()
                .Filter(loadParam, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }
    }
}