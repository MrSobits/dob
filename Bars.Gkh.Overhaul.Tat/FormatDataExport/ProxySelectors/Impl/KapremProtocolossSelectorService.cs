namespace Bars.Gkh.Overhaul.Tat.FormatDataExport.ProxySelectors.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.FormatDataExport.ProxySelectors;
    using Bars.Gkh.Overhaul.Tat.Entities;
    using Bars.Gkh.Overhaul.Tat.Enum;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Селектор Протоколов общего собрания собственников, которыми принято решение о формирования фонда капитального ремонта
    /// </summary>
    public class KapremProtocolossSelectorService : BaseProxySelectorService<KapremProtocolossProxy>
    {
        /// <inheritdoc />
        protected override IDictionary<long, KapremProtocolossProxy> GetCache()
        {
            var protocolRepository = this.Container.ResolveRepository<BasePropertyOwnerDecision>();

            using (this.Container.Using(protocolRepository))
            {
                return this.FilterService.FilterByRealityObject(protocolRepository.GetAll(), x => x.RealityObject)
                    .Where(x => x.RealityObject.TypeHouse == TypeHouse.ManyApartments || x.RealityObject.TypeHouse == TypeHouse.BlockedBuilding)
                    .AsEnumerable()
                    .Select(x => new KapremProtocolossProxy
                    {
                        Id = x.Id,
                        RealityObjectId = x.RealityObject.Id,
                        Status = x.RealityObject.ConditionHouse != ConditionHouse.Razed ? 1 : 2,
                        SolutionReason = this.GetPropertyOwnerProtocol(x.PropertyOwnerProtocol.TypeProtocol),
                        MethodFormFundCr = this.GetMethodFormFundCr(x.MethodFormFund),
                        ProtocolDate = x.PropertyOwnerProtocol.DocumentDate,
                        DateStart = x.PropertyOwnerProtocol.DocumentDate,
                        File = x.PropertyOwnerProtocol.DocumentFile,
                        FileType = 1,
                        DocumentType = this.GetPropertyOwnerProtocol(x.PropertyOwnerProtocol.TypeProtocol) == 2 ? "решение ОМС" : string.Empty
                    })
                    .AsEnumerable()
                    .ToDictionary(x => x.Id);
            }
        }

        private int? GetPropertyOwnerProtocol(PropertyOwnerProtocolType? propertyOwnerDecisionType)
        {
            switch (propertyOwnerDecisionType)
            {
                case PropertyOwnerProtocolType.FormationFund:
                    return 1;
                case PropertyOwnerProtocolType.ResolutionOfTheBoard:
                    return 2;
                default:
                    return null;
            }
        }

        private int? GetMethodFormFundCr(MethodFormFundCr? methodFormFundCr)
        {
            switch (methodFormFundCr)
            {
                case MethodFormFundCr.SpecialAccount:
                    return 1;
                case MethodFormFundCr.RegOperAccount:
                    return 2;
                default:
                    return null;
            }
        }
    }
}