namespace Bars.GkhGji.ViewModel
{
    using B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Utils;
    using Entities;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ComissionMeetingViewModel : BaseViewModel<ComissionMeeting>
    {
        public IDomainService<ZonalInspectionInspector> ZonalInspectionInspectorDomain { get; set; }
        public IGkhUserManager UserManager { get; set; }
        public override IDataResult List(IDomainService<ComissionMeeting> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);


            Operator thisOperator = UserManager.GetActiveOperator();
            if (thisOperator?.Inspector == null)
            {

                var data = domainService.GetAll()
                    .Select(x => new
                    {
                         x.Id,
                         ZonalInspection = x.ZonalInspection.Name,
                         x.CommissionNumber,
                         x.ComissionName,
                         x.CommissionDate,
                         x.State,
                         x.TimeEnd,
                         x.TimeStart
                    })
                    .Filter(loadParams, this.Container);

                int totalCount = data.Count();

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
            }
            else
            {
                var zonal = ZonalInspectionInspectorDomain.GetAll()
                    .FirstOrDefault(x => x.Inspector == thisOperator.Inspector)?.ZonalInspection;
                if (zonal != null)
                {
                    var data = domainService.GetAll()
                        .Where(x=> x.ZonalInspection == zonal)
                    .Select(x => new
                    {
                        x.Id,
                        ZonalInspection = x.ZonalInspection.Name,
                        x.CommissionNumber,
                        x.ComissionName,
                        x.CommissionDate,
                        x.State,
                        x.TimeEnd,
                        x.TimeStart
                    })
                    .Filter(loadParams, this.Container);

                    int totalCount = data.Count();

                    return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
                }
                return null;
            }
        }


    }
}
