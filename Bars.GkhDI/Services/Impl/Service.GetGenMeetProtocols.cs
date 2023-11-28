namespace Bars.GkhDi.Services.Impl
{
    using System.Linq;
    using B4;
    using B4.Utils;
    using Entities;
    using Gkh.Services.DataContracts;

    public partial class Service
    {
        public GetGenMeetProtocolsResponse GetGenMeetProtocols(string houseId, string periodId)
        {
            var idHouse = houseId.ToLong();
            var idPeriod = periodId.ToLong();

            if (idHouse != 0 && idPeriod != 0)
            {
                var diRealObj = Container.Resolve<IDomainService<DisclosureInfoRealityObj>>()
                    .GetAll()
                    .FirstOrDefault(x => x.RealityObject.Id == idHouse && x.PeriodDi.Id == idPeriod);

                if (diRealObj == null)
                {
                    return new GetGenMeetProtocolsResponse { Result = Result.DataNotFound };
                }

                var protocols = Container.Resolve<IDomainService<DocumentsRealityObjProtocol>>()
                             .GetAll()
                             .Where(x => x.RealityObject.Id == diRealObj.RealityObject.Id && x.Year == diRealObj.PeriodDi.DateEnd.Value.Year && x.File != null)
                             .Select(x => x.File)
                             .ToList()
                             .AsQueryable()
                             .Select(x => new Protocol
                                     {
                                         IdFile = x.Id, 
                                         NameFile = x.FullName
                                     })
                             .ToArray();

                return new GetGenMeetProtocolsResponse { Protocols = protocols, Result = Result.NoErrors };
            }

            return new GetGenMeetProtocolsResponse { Result = Result.DataNotFound };
        }
    }
}
