/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Bars.Gkh.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Материалы стен"
///     /// </summary>
///     public class WallMaterialMap : BaseGkhEntityMap<WallMaterial>
///     {
///         public WallMaterialMap() : base("GKH_DICT_WALL_MATERIAL")
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
    
    
    /// <summary>Маппинг для "Материал стен"</summary>
    public class WallMaterialMap : BaseImportableEntityMap<WallMaterial>
    {
        
        public WallMaterialMap() : 
                base("Материал стен", "GKH_DICT_WALL_MATERIAL")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Name, "Наименование").Column("NAME").Length(300);
        }
    }
}
