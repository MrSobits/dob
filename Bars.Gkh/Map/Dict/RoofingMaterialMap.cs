/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Bars.Gkh.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Материалы кровли"
///     /// </summary>
///     public class RoofingMaterialMap : BaseGkhEntityMap<RoofingMaterial>
///     {
///         public RoofingMaterialMap() : base("GKH_DICT_ROOFING_MATERIAL")
///         {
///             Map(x => x.Name, "NAME").Not.Nullable().Length(300);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Материал кровли"</summary>
    public class RoofingMaterialMap : BaseImportableEntityMap<RoofingMaterial>
    {
        
        public RoofingMaterialMap() : 
                base("Материал кровли", "GKH_DICT_ROOFING_MATERIAL")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Name, "Наименование").Column("NAME").Length(300);
        }
    }
}
