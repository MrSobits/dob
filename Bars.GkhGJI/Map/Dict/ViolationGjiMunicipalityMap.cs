/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.Gkh.Map;
///     using Bars.GkhGji.Entities;
/// 
///     /// <summary>
///     ///     Маппинг для сущности "Нарушения"
///     /// </summary>
///     public class ViolationGjiMap : BaseGkhEntityMap<ViolationGji>
///     {
///         public ViolationGjiMap()
///             : base("GJI_DICT_VIOLATION")
///         {
///             Map(x => x.Name, "NAME").Length(2000);
///             Map(x => x.Description, "DESCRIPTION").Length(2000);
///             Map(x => x.GkRf, "GKRF").Length(2000);
///             Map(x => x.CodePin, "CODEPIN").Length(2000);
///             Map(x => x.NormativeDocNames, "NPD_NAME").Length(2000);
///             Map(x => x.PpRf25, "PPRF25").Length(2000);
///             Map(x => x.PpRf307, "PPRF307").Length(2000);
///             Map(x => x.PpRf491, "PPRF491").Length(2000);
///             Map(x => x.PpRf170, "PPRF170").Length(2000);
///             Map(x => x.OtherNormativeDocs, "OTHER_DOCS").Length(2000);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Нарушение ГЖИ"</summary>
    public class ViolationGjiMunicipalityMap : BaseEntityMap<ViolationGjiMunicipality>
    {
        
        public ViolationGjiMunicipalityMap() : 
                base("Нарушение ГЖИ", "GJI_DICT_VIOLATION_MUNICIPALITY")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.ViolationGji, "Связь с таблицей нарушений").Column("VIOLATION_ID").Fetch();
            Reference(x => x.Municipality, "Связь с таблицей муниципальных образований").Column("MUNICIPALITY_ID").Fetch();
        }
    }
}
