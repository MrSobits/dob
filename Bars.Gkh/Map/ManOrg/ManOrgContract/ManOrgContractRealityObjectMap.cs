/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map.ManOrg
/// {
///     using Bars.Gkh.Entities;
/// 
///     /// <summary>
///     /// Маппинг сущности "Жилой дом договора управляющей организации"
///     /// </summary>
///     public class ManOrgContractRealityObjectMap : BaseGkhEntityMap<ManOrgContractRealityObject>
///     {
///         public ManOrgContractRealityObjectMap() : base("GKH_MORG_CONTRACT_REALOBJ")
///         {
///             References(x => x.RealityObject, "REALITY_OBJ_ID").Not.Nullable().Fetch.Join();
///             References(x => x.ManOrgContract, "MAN_ORG_CONTRACT_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Жилой дом договора управляющей организации"</summary>
    public class ManOrgContractRealityObjectMap : BaseImportableEntityMap<ManOrgContractRealityObject>
    {
        
        public ManOrgContractRealityObjectMap() : 
                base("Жилой дом договора управляющей организации", "GKH_MORG_CONTRACT_REALOBJ")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Reference(x => x.RealityObject, "Жилой дом").Column("REALITY_OBJ_ID").NotNull().Fetch();
            Reference(x => x.ManOrgContract, "Договор управления").Column("MAN_ORG_CONTRACT_ID").NotNull().Fetch();
        }
    }
}
