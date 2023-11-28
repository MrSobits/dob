namespace Bars.Gkh.RegOperator.FormatDataExport.ProxySelectors.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.Decisions.Nso.Entities.Decisions;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Enums;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.FormatDataExport.ProxySelectors;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Селектор Протоколов общего собрания собственников, которыми принято решение о формирования фонда капитального ремонта
    /// </summary>
    public class KapremProtocolossSelectorService : BaseProxySelectorService<KapremProtocolossProxy>
    {
        /// <inheritdoc />
        protected override IDictionary<long, KapremProtocolossProxy> GetCache()
        {
            var roDecisionProtocolRepository = this.Container.ResolveRepository<RealityObjectDecisionProtocol>();
            var govDecisionRepository = this.Container.ResolveRepository<GovDecision>();

            var realityObjectManOrgService = this.Container.Resolve<IRealityObjectManOrgService>();

            var regopSchetDict = this.ProxySelectorFactory.GetSelector<RegopSchetProxy>()
                .ExtProxyListCache
                .Where(x => !x.IsRegOpAccount)
                .Where(x => x.ContragentId.HasValue)
                .GroupBy(x => x.ContragentId.Value)
                .ToDictionary(x => x.Key, x => x.FirstOrDefault()?.Id);

            using (this.Container.Using(roDecisionProtocolRepository, govDecisionRepository, realityObjectManOrgService))
            {
                var manOrgRepository = realityObjectManOrgService.GetActualManagingOrganizations();

                var roDecisionProtocols = this.FilterService.FilterByRealityObject(roDecisionProtocolRepository.GetAll(), x => x.RealityObject)
                    .Where(x => x.RealityObject.TypeHouse == TypeHouse.ManyApartments || x.RealityObject.TypeHouse == TypeHouse.BlockedBuilding)
                    .Select(x => new
                    {
                        Id = x.ExportId,
                        RoId = x.RealityObject.Id,
                        RoConditionHouse = x.RealityObject.ConditionHouse,
                        x.RealityObject.AccountFormationVariant,
                        x.ProtocolDate,
                        x.DateStart,
                        x.DocumentNum,
                        x.File
                    })
                    .AsEnumerable()
                    .Select(x =>
                    {
                        var contragentId = this.GetId(manOrgRepository.Get(x.RoId)?.Contragent);

                        return new KapremProtocolossProxy
                        {
                            Id = x.Id,
                            ContragentId = contragentId,
                            RealityObjectId = x.RoId,
                            Status = x.RoConditionHouse == ConditionHouse.Serviceable ? 1 : 2,
                            SolutionReason = 1, // Протокол общего собрания собственника
                            MethodFormFundCr = this.GetMethodFormFundCr(x.AccountFormationVariant),
                            ProtocolossId = null,
                            ProtocolNumber = x.DocumentNum,
                            DocumentNumber = x.DocumentNum,
                            ProtocolDate = x.ProtocolDate,
                            DateStart = x.DateStart,
                            RegopSchetId = regopSchetDict.Get(contragentId ?? 0),
                            File = x.File,
                            FileType = 1 // Протокол собрания собственников
                        };
                    });

                return this.FilterService.FilterByRealityObject(govDecisionRepository.GetAll(), x => x.RealityObject) 
                    .Where(x => x.RealityObject.TypeHouse == TypeHouse.ManyApartments || x.RealityObject.TypeHouse == TypeHouse.BlockedBuilding)
                    .Select(x => new
                    {
                        Id = x.ExportId,
                        RoId = x.RealityObject.Id,
                        RoConditionHouse = x.RealityObject.ConditionHouse,
                        x.RealityObject.AccountFormationVariant,
                        x.ProtocolDate,
                        x.DateStart,
                        DocumentNum = x.ProtocolNumber,
                        File = x.ProtocolFile
                    })
                    .AsEnumerable()
                    .Select(x =>
                    {
                        var contragentId = this.GetId(manOrgRepository.Get(x.RoId)?.Contragent);

                        return new KapremProtocolossProxy
                        {
                            Id = x.Id,
                            ContragentId = contragentId,
                            RealityObjectId = x.RoId,
                            Status = x.RoConditionHouse == ConditionHouse.Serviceable ? 1 : 2,
                            SolutionReason = 2, // Документ решения (решение ОМС)
                            MethodFormFundCr = this.GetMethodFormFundCr(x.AccountFormationVariant),
                            ProtocolossId = null,
                            ProtocolNumber = x.DocumentNum,
                            DocumentNumber = x.DocumentNum,
                            ProtocolDate = x.ProtocolDate,
                            DateStart = x.DateStart,
                            RegopSchetId = regopSchetDict.Get(contragentId ?? 0),
                            File = x.File,
                            FileType = 2, // Документы решения  ОМС
                            DocumentType = "решение ОМС"
                        };
                    })
                    .Union(roDecisionProtocols)
                    .ToDictionary(x => x.Id);
            }
        }

        private int? GetMethodFormFundCr(CrFundFormationType? type)
        {
            switch (type)
            {
                case CrFundFormationType.SpecialAccount:
                case CrFundFormationType.SpecialRegOpAccount:
                    return 1;
                case CrFundFormationType.RegOpAccount:
                    return 2;
                default:
                    return null;
            }
        }
    }
}