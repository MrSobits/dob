namespace Bars.GkhGji.Regions.Voronezh.ViewModel
{
    using B4;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.GkhGji.Regions.Voronezh.Entities.ASFK;
    using System.Linq;

    public class ASFKViewModel : BaseViewModel<ASFK>
    {
        public override IDataResult List(IDomainService<ASFK> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var data = domainService.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.NumVer,
                    x.Former,
                    x.FormVer,
                    x.NormDoc,
                    x.KodTofkFrom,
                    x.NameTofkFrom,
                    x.BudgetLevel,
                    x.KodUbp,
                    x.NameUbp,
                    x.GuidVT,
                    x.LsAdb,
                    x.DateOtch,
                    x.DateOld,
                    x.VidOtch,
                    x.KodTofkVT,
                    x.NameTofkVT,
                    x.KodUbpAdb,
                    x.NameUbpAdb,
                    x.KodGadb,
                    x.NameGadb,
                    x.NameBud,
                    x.Oktmo,
                    x.OkpoFo,
                    x.NameFo,
                    x.DolIsp,
                    x.NameIsp,
                    x.TelIsp,
                    x.DatePod,
                    x.SumInItogV,
                    x.SumOutItogV,
                    x.SumZachItogV,
                    x.SumNOutItogV,
                    x.SumNZachItogV,
                    x.SumBeginIn,
                    x.SumBeginOut,
                    x.SumBeginZach,
                    x.SumBeginNOut,
                    x.SumBeginNZach,
                    x.SumEndIn,
                    x.SumEndOut,
                    x.SumEndZach,
                    x.SumEndNOut,
                    x.SumEndNZach,
                    DistributedSum = GetDistrSum(x.Id)
                })
                .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

        private decimal GetDistrSum(long asfkId)
        {
            var bdoperDomain = Container.Resolve<IDomainService<BDOPER>>();
            try
            {
                return bdoperDomain.GetAll()
                    .Where(x => x.ASFK.Id == asfkId)
                    .Where(x => x.IsPayFineAdded == true)
                    .SafeSum(x => x.Sum);
            }
            finally
            {
                Container.Release(bdoperDomain);
            }
        }
    }
}
