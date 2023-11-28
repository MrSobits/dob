/// <mapping-converter-backup>
/// namespace Bars.GkhDi.Map
/// {
///     using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
///     using Entities;
/// 
///     public class OtherServiceMap : BaseGkhEntityMap<OtherService>
///     {
///         public OtherServiceMap()
///             : base("DI_OTHER_SERVICE")
///         {
///             Map(x => x.Name, "NAME").Length(300);
///             Map(x => x.Code, "CODE").Length(50);
///             Map(x => x.UnitMeasure, "UNIT_MEASURE").Length(300);
///             Map(x => x.Tariff, "TARIFF");
///             Map(x => x.Provider, "PROVIDER").Length(300);
/// 
///             References(x => x.DisclosureInfoRealityObj, "DISINFO_RO_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhDi.Map
{
    using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhDi.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhDi.Entities.OtherService"</summary>
    public class OtherServiceMap : BaseImportableEntityMap<OtherService>
    {
        
        public OtherServiceMap() : 
                base("Bars.GkhDi.Entities.OtherService", "DI_OTHER_SERVICE")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Name, "Name").Column("NAME").Length(300);
            Property(x => x.Code, "Code").Column("CODE").Length(50);
            Property(x => x.UnitMeasure, "UnitMeasure").Column("UNIT_MEASURE").Length(300);
            Property(x => x.Tariff, "Tariff").Column("TARIFF");
            Property(x => x.Provider, "Provider").Column("PROVIDER").Length(300);
            Reference(x => x.DisclosureInfoRealityObj, "DisclosureInfoRealityObj").Column("DISINFO_RO_ID").NotNull().Fetch();
        }
    }
}
