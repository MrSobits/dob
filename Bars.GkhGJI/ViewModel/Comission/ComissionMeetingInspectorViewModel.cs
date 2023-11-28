namespace Bars.GkhGji.ViewModel
{
    using Entities;
    using B4;
    using B4.Utils;
    using System.Linq;
    using System;

    public class ComissionMeetingInspectorViewModel : BaseViewModel<ComissionMeetingInspector>
    {
        public override IDataResult List(IDomainService<ComissionMeetingInspector> domain, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var id = loadParams.Filter.GetAs("comissionmeetId", 0L);

            var data = domain.GetAll()
             .Where(x => x.ComissionMeeting.Id == id)
            .Select(x => new
            {
                x.Id,
                Inspector = x.Inspector.Fio,
                x.Inspector.Position,
                x.YesNoNotSet,
                TypeCommissionMember = x.TypeCommissionMember >0 ?x.TypeCommissionMember:x.Inspector.TypeCommissionMember
            })
            .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());



        }
    }
}