namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    public class OwnerRoomViewModel : BaseViewModel<OwnerRoom>
    {
        public override IDataResult List(IDomainService<OwnerRoom> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var protocolId = baseParams.Params.ContainsKey("protocolId")
                                   ? baseParams.Params["protocolId"].ToLong()
                                   : 0;

            var data = domainService.GetAll()
     
                .Select(x => new
                {
                    x.Id,
                    x.IndividualPerson,
                    x.Room,
                    x.Contragent,
                    x.TypeViolatorOwnerRoom,
                    x.DataOwnerStart,
                    x.DataOwnerEdit
                })
                .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).ToList(), totalCount);
        }
    }
}