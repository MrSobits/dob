using System;
using System.Collections.Generic;
using System.Linq;

using Bars.B4;
using Bars.B4.Utils;
using Bars.Gkh.Authentification;
using Bars.GkhGji.Entities;
using Bars.GkhGji.Enums;
using Bars.GkhGji.Utils;

using Castle.Windsor;
using Bars.Gkh.Entities;
using Bars.GkhGji.Regions.Voronezh.Entities;
using Bars.GkhGji.Regions.Voronezh.Enums;

namespace Bars.GkhGji.Regions.Voronezh.DomainService
{
    public class ResolutionService : GkhGji.DomainService.ResolutionService
    {
        public override IDataResult ListView(BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            /*
             * В качестве фильтров приходят следующие параметры
             * dateStart - Необходимо получить документы больше даты начала
             * dateEnd - Необходимо получить документы меньше даты окончания
             * dateNotPayStart - Необходимо получить документы больше даты неоплаты
             * dateNotPayEnd - Необходимо получить документы меньше даты неоплаты
             * realityObjectId - Необходимо получить документы по дому
            */

            var dateStart = baseParams.Params.GetAs<DateTime>("dateStart");
            var dateEnd = baseParams.Params.GetAs<DateTime>("dateEnd");
            var dateNotPayStart = baseParams.Params.GetAs<DateTime>("dateNotPayStart");
            var dateNotPayEnd = baseParams.Params.GetAs<DateTime>("dateNotPayEnd");
            var realityObjectId = baseParams.Params.GetAs<long>("realityObjectId");

            var predata = GetViewList()
                .WhereIf(dateStart != DateTime.MinValue, x => x.DocumentDate >= dateStart)
                .WhereIf(dateEnd != DateTime.MinValue, x => x.DocumentDate <= dateEnd)
                .WhereIf(dateNotPayStart != DateTime.MinValue, x => x.Protocol205Date >= dateNotPayStart)
                .WhereIf(dateNotPayEnd != DateTime.MinValue, x => x.Protocol205Date <= dateNotPayEnd)
                .WhereIf(realityObjectId > 0, x => x.RealityObjectIds.Contains("/" + realityObjectId.ToString() + "/"))
                .AsEnumerable()
                .Select(x => new
                {
                    x.Id,
                    x.State,
                    x.ContragentName,
                    x.TypeExecutant,
                    MunicipalityNames = x.TypeBase == TypeBase.ProtocolMvd ? GetProtocolMvdMuName(x.InspectionId.ToLong()) : x.MunicipalityId != null ? x.MunicipalityNames : x.ContragentMuName,
                    MoSettlement = x.MoNames,
                    PlaceName = x.PlaceNames,
                    MunicipalityId = x.MunicipalityId ?? x.ContragentMuId,
                    x.DocumentDate,
                    x.DocumentNumber,
                    x.DocumentNum,
                    x.OfficialName,
                    x.OfficialPosition,
                    x.TypeInitiativeOrg,
                    x.PenaltyAmount,
                    x.Protocol205Date,
                    x.Sanction,
                    x.SumPays,
                    x.InspectionId,
                    x.TypeBase,
                    x.ConcederationResult,
                    x.TypeDocumentGji,
                    x.DeliveryDate,
                    x.Paided,
                    x.ControlType,
                    x.BecameLegal,
                    x.InLawDate,
                    x.DueDate,
                    x.PaymentDate,
                    x.RoAddress,
                    x.ArticleLaw,
                    //HasProtocol = GetProtocol(x.Id),
                    x.SentToOSP,
                    x.PhysicalPerson
                })
                .ToList();

            var plusdata = GetViewList()
                .WhereIf(dateStart != DateTime.MinValue, x => x.DocumentDate >= dateStart)
                .WhereIf(dateEnd != DateTime.MinValue, x => x.DocumentDate <= dateEnd)
                .WhereIf(realityObjectId > 0, x => x.RealityObjectIds.Contains("/" + realityObjectId.ToString() + "/"))
                .Where(x => x.Protocol205Date == null)
                .AsEnumerable()
                .Select(x => new
                {
                    x.Id,
                    x.State,
                    x.ContragentName,
                    x.TypeExecutant,
                    MunicipalityNames = x.TypeBase == TypeBase.ProtocolMvd ? GetProtocolMvdMuName(x.InspectionId.ToLong()) : x.MunicipalityId != null ? x.MunicipalityNames : x.ContragentMuName,
                    MoSettlement = x.MoNames,
                    PlaceName = x.PlaceNames,
                    MunicipalityId = x.MunicipalityId ?? x.ContragentMuId,
                    x.DocumentDate,
                    x.DocumentNumber,
                    x.DocumentNum,
                    x.OfficialName,
                    x.OfficialPosition,
                    x.TypeInitiativeOrg,
                    x.PenaltyAmount,
                    x.Protocol205Date,
                    x.Sanction,
                    x.SumPays,
                    x.InspectionId,
                    x.TypeBase,
                    x.ConcederationResult,
                    x.TypeDocumentGji,
                    x.DeliveryDate,
                    x.Paided,
                    x.ControlType,
                    x.BecameLegal,
                    x.InLawDate,
                    x.DueDate,
                    x.PaymentDate,
                    x.RoAddress,
                    x.ArticleLaw,
                    //HasProtocol = GetProtocol(x.Id),
                    x.SentToOSP,
                    x.PhysicalPerson
                })
                .ToList();

            var result = predata.Concat(plusdata);

            var data = result.Select(x => new
            {
                x.Id,
                x.State,
                x.ContragentName,
                x.TypeExecutant,
                x.MunicipalityNames,
                x.MoSettlement,
                x.PlaceName,
                x.MunicipalityId,
                x.DocumentDate,
                x.DocumentNumber,
                x.DocumentNum,
                x.OfficialName,
                x.OfficialPosition,
                x.TypeInitiativeOrg,
                x.PenaltyAmount,
                x.Protocol205Date,
                x.Sanction,
                x.SumPays,
                x.InspectionId,
                x.TypeBase,
                x.ConcederationResult,
                x.TypeDocumentGji,
                x.DeliveryDate,
                x.Paided,
                x.ControlType,
                x.BecameLegal,
                x.InLawDate,
                x.DueDate,
                x.PaymentDate,
                x.RoAddress,
                x.ArticleLaw,
                HasProtocol = GetProtocol(x.Id),
                x.SentToOSP,
                x.PhysicalPerson,
                IsAppealed = GetIsAppealed(x.Id),
                AppelationStatus = GetAppelationStatus(x.Id)
            })
            .AsQueryable()
            .Filter(loadParam, Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }

        private bool GetProtocol(long resolutionId)
        {
            var childrenId = this.Container.Resolve<IDomainService<DocumentGjiChildren>>().GetAll()
             .Where(x => x.Parent.Id == resolutionId && x.Children.TypeDocumentGji == TypeDocumentGji.Protocol)
             .Select(x => x.Children.Id).FirstOrDefault();

            return childrenId > 0;
        }

        private bool GetIsAppealed(long resolutionId)
        {
            var courtPracticeId = this.Container.Resolve<IDomainService<CourtPractice>>().GetAll()
             .Where(x => x.DocumentGji.Id == resolutionId && !x.State.FinalState)
             .Select(x => x.Id).FirstOrDefault();

            return courtPracticeId > 0;
        }

        private CourtMeetingResult GetAppelationStatus(long resolutionId)
        {
            var courtPractice = this.Container.Resolve<IDomainService<CourtPractice>>().GetAll()
             .Where(x => x.DocumentGji.Id == resolutionId && !x.State.FinalState)
             .Select(x => x).FirstOrDefault();

            if (courtPractice != null)
            {
                return courtPractice.CourtMeetingResult;
            }

            return CourtMeetingResult.NotSet;
        }
    }
}