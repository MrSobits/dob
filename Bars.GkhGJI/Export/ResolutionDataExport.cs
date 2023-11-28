namespace Bars.GkhGji.Export
{
    using System;
    using System.Collections;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Utils;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class ResolutionDataExport : BaseDataExportService
    {
        public IDomainService<ProtocolMvdRealityObject> ProtocolMvdRealityObjectDomain { get; set; }

        public override IList GetExportData(BaseParams baseParams)
        {
            var loadParam = GetLoadParam(baseParams);

            /*
             * В качестве фильтров приходят следующие параметры
             * dateStart - Необходимо получить документы больше даты начала
             * dateEnd - Необходимо получить документы меньше даты окончания
             * realityObjectId - Необходимо получить документы по дому
            */

            var dateStart = baseParams.Params.GetAs<DateTime>("dateStart");
            var dateEnd = baseParams.Params.GetAs<DateTime>("dateEnd");
            var realityObjectId = baseParams.Params.GetAs<long>("realityObjectId");

            return Container.Resolve<IResolutionService>().GetViewList()
                .WhereIf(dateStart != DateTime.MinValue, x => x.DocumentDate >= dateStart)
                .WhereIf(dateEnd != DateTime.MaxValue, x => x.DocumentDate <= dateEnd)
                .WhereIf(realityObjectId > 0, x => x.RealityObjectIds.Contains("/" + realityObjectId.ToString() + "/"))
                .Select(x => new
                {
                    x.Id,
                    x.State,
                    x.ContragentName,
                    x.TypeExecutant,
                    MunicipalityNames = x.TypeBase == TypeBase.ProtocolMvd ? GetProtocolMvdMuName(x.InspectionId.ToLong()) : x.MunicipalityId != null ? x.MunicipalityNames : x.ContragentMuName,
                    MunicipalityId = x.MunicipalityId ?? x.ContragentMuId,
                    x.DocumentDate,
                    x.DocumentNumber,
                    x.DocumentNum,
                    x.OfficialName,
                    x.PenaltyAmount,
                    x.Sanction,
                    x.SumPays,
                    x.Protocol205Date,
                    x.InspectionId,
                    x.TypeBase,
                    x.TypeDocumentGji,
                    x.DeliveryDate,
                    x.Paided,
                    x.BecameLegal,
                    x.RoAddress
                })
                .Filter(loadParam, Container)
                .Order(loadParam)
                .ToList();
        }

        public virtual string GetProtocolMvdMuName(long? resolInspId)
        {
            if (resolInspId == null)
            {
                return string.Empty;
            }

            return ProtocolMvdRealityObjectDomain.GetAll().Where(x => x.ProtocolMvd.Inspection.Id == resolInspId).Select(x => x.RealityObject.Municipality.Name).FirstOrDefault();
        }
    }
}