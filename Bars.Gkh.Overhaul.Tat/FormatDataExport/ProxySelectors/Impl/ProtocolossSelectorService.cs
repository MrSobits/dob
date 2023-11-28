namespace Bars.Gkh.Overhaul.Tat.FormatDataExport.ProxySelectors.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Enums;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.FormatDataExport.ProxySelectors;
    using Bars.Gkh.FormatDataExport.ProxySelectors.SystemSelectors;
    using Bars.Gkh.Overhaul.Tat.Entities;

    /// <summary>
    /// Селектор Протоколов общего собрания собственников
    /// </summary>
    [Obsolete("СА: Не выгружаем", true)]
    public class ProtocolossSelectorService : BaseProxySelectorService<ProtocolossProxy>
    {
        /// <inheritdoc />
        protected override IDictionary<long, ProtocolossProxy> GetCache()
        {
            var propertyOwnerProtocolsRepository = this.Container.ResolveRepository<PropertyOwnerProtocols>();
            var realityObjectManOrgService = this.Container.Resolve<IRealityObjectManOrgService>();

            using (this.Container.Using(propertyOwnerProtocolsRepository, realityObjectManOrgService))
            {
                var manOrgDict = this.ProxySelectorFactory.GetSelector<ActualManOrgByRealityObject>().ProxyListCache;

                return propertyOwnerProtocolsRepository.GetAll()
                    .Where(x => manOrgDict.ContainsKey(x.RealityObject.Id))
                    .Select(x => new
                    {
                        x.Id,
                        RoId = x.RealityObject.Id,
                        x.DocumentDate,
                        x.DocumentNumber,
                        x.RealityObject.AccountFormationVariant,
                        x.RealityObject.Address,
                        x.DocumentFile
                    })
                    .AsEnumerable()
                    .Select(x => new ProtocolossProxy
                    {
                        Id = x.Id,
                        RealityObjectId = x.RoId,
                        ContragentId = this.GetId(manOrgDict.Get(x.RoId)?.Contragent),
                        DocumentDate = x.DocumentDate,
                        DocumentNumber = x.DocumentNumber,
                        StartDate = x.DocumentDate,
                        MethodFormFundCr = this.GetMethodFormFundCr(x.AccountFormationVariant),
                        VotingForm = 2, // очное
                        EndDate = null,
                        DecisionPlace = x.Address,
                        MeetingPlace = x.Address,
                        MeetingDateTime = x.DocumentDate.Value.AddHours(18),
                        VoteStartDateTime = null,
                        VoteEndDateTime = null,
                        ReceptionProcedure = null,
                        ReviewProcedure = null,
                        IsAnnualMeeting = 2, // нет
                        IsCompetencyMeeting = 1, // да
                        Status = 1, // размещен

                        AttachmentFile = x.DocumentFile
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