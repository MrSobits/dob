using Bars.B4;
using Bars.Gkh.Overhaul.Hmao.Entities;
using System.Linq;

namespace Bars.Gkh.Overhaul.Hmao.ViewModel
{
    public class CostLimitViewModel : BaseViewModel<CostLimit>
    {
        public override IDataResult List(IDomainService<CostLimit> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var data = domainService.GetAll()
                .Select(x => new
                {
                    x.Id,
                    Work = x.Work.Name,
                    x.Cost,
                    x.DateStart,
                    x.DateEnd,
                    x.FloorStart,
                    x.FloorEnd,
                    Municipality = x.Municipality != null ? x.Municipality.Name : "Все"
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}
