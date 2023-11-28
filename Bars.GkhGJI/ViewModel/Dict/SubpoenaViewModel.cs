namespace Bars.GkhGji.ViewModel
{
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    public class SubpoenaViewModel : BaseViewModel<Subpoena>
    {
        public override IDataResult List(IDomainService<Subpoena> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var data = domainService.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.DateOfProceedings,
                    x.HourOfProceedings,
                    x.ProceedingCopyNum,
                    x.ProceedingsPlace,
                    Comission = x.ComissionMeeting,
                    ComissionName = x.ComissionMeeting.ComissionName
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}