
namespace Bars.GkhGji.Map.Dict
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.Dict;
    
    
    /// <summary>Маппинг для "Цель проведения проверки"</summary>
    public class MaterialTypeMap : BaseEntityMap<MaterialType>
    {
        
        public MaterialTypeMap() : 
                base("Цель проведения проверки", "GJI_DICT_MATERIAL_TYPE")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Name, "Наименование цели").Column("NAME").Length(1500);
            Property(x => x.Code, "Код").Column("CODE").Length(250);
            Property(x => x.IsProof, "Код").Column("IS_PROOF").Length(250);
        }
    }
}
