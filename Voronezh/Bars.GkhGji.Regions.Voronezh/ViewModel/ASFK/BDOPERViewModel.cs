namespace Bars.GkhGji.Regions.Voronezh.ViewModel
{
    using B4;
    using Bars.GkhGji.Regions.Voronezh.Entities.ASFK;
    using Bars.GkhGji.Regions.Voronezh.Enums;
    using System.Linq;

    public class BDOPERViewModel : BaseViewModel<BDOPER>
    {
        public override IDataResult List(IDomainService<BDOPER> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var asfkId = loadParams.Filter.GetAs("asfkId", 0L);

            var data = domainService.GetAll()
                .Where(x => x.ASFK.Id == asfkId)
                .Select(x => new 
                { 
                    x.Id,
                    x.ASFK,
                    x.IsPayFineAdded,
                    x.GUID,
                    x.Sum,
                    x.InnPay,
                    x.KppPay,
                    x.NamePay,
                    x.Kbk,
                    x.KodDocAdb,
                    x.Purpose,
                    x.RelatedASFKId
                })
                .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}
