namespace Bars.Gkh.RegOperator.FormatDataExport.ProxySelectors.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.FormatDataExport.ProxySelectors;
    using Bars.Gkh.FormatDataExport.ProxySelectors.SystemSelectors;

    /// <summary>
    /// Селектор Протоколов общего собрания собственников
    /// </summary>
    [Obsolete("СА: Не выгружаем", true)]
    public class ProtocolossSelectorService : BaseProxySelectorService<ProtocolossProxy>
    {
        /// <inheritdoc />
        protected override IDictionary<long, ProtocolossProxy> GetCache()
        {
            var propertyOwnerProtocolsRepository = this.Container.ResolveRepository<RealityObjectDecisionProtocol>();

            using (this.Container.Using(propertyOwnerProtocolsRepository))
            {
                var manOrgDict = this.ProxySelectorFactory.GetSelector<ActualManOrgByRealityObject>().ProxyListCache;

                return propertyOwnerProtocolsRepository.GetAll()
                    .Where(x => manOrgDict.ContainsKey(x.RealityObject.Id))
                    .Select(x => new
                    {
                        x.Id,
                        RoId = x.RealityObject.Id,
                        x.ProtocolDate,
                        x.DateStart,
                        x.DocumentNum,
                        x.RealityObject.AccountFormationVariant,
                        x.RealityObject.Address,
                        x.File
                    })
                    .AsEnumerable()
                    .Select(x => new ProtocolossProxy
                    {
                        Id = x.Id,
                        RealityObjectId = x.RoId,
                        ContragentId = this.GetId(manOrgDict.Get(x.RoId)?.Contragent),
                        DocumentDate = x.ProtocolDate,
                        DocumentNumber = x.DocumentNum,
                        StartDate = x.DateStart,
                        MethodFormFundCr = this.GetMethodFormFundCr(x.AccountFormationVariant),
                        VotingForm = null,
                        EndDate = null,
                        DecisionPlace = x.Address,
                        MeetingPlace = x.Address,
                        MeetingDateTime = null,
                        VoteStartDateTime = null,
                        VoteEndDateTime = null,
                        ReceptionProcedure = null,
                        ReviewProcedure = null,
                        IsAnnualMeeting = null,
                        IsCompetencyMeeting = null,
                        Status = null,

                        AttachmentFile = x.File
                    })
                    .ToDictionary(x => x.Id);
            }
        }

        private int GetMethodFormFundCr(CrFundFormationType? type)
        {
            return type.HasValue
                ? type.Value == CrFundFormationType.SpecialRegOpAccount || type.Value == CrFundFormationType.SpecialAccount // Специальный счет
                    ? 3
                    : type.Value == CrFundFormationType.RegOpAccount // Счет регионального оператора
                        ? 4
                        : 1
                : 1;
        }
    }
}