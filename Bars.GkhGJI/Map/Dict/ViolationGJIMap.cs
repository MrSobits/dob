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
    public class ViolationGjiMap : BaseEntityMap<ViolationGji>
    {
        
        public ViolationGjiMap() : 
                base("Нарушение ГЖИ", "GJI_DICT_VIOLATION")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Name, "Наименование").Column("NAME").Length(2000);
            Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(2000);
            Property(x => x.GkRf, "ЖК РФ").Column("GKRF").Length(2000);
            Property(x => x.CodePin, "Код ПИН").Column("CODEPIN").Length(2000);
            Property(x => x.NormativeDocNames, "Строка в которую будет сохранятся все Нормативные документы через запятую").Column("NPD_NAME").Length(2000);
            Property(x => x.PpRf25, "ПП РФ №25").Column("PPRF25").Length(2000);
            Property(x => x.PpRf307, "ПП РФ №307").Column("PPRF307").Length(2000);
            Property(x => x.PpRf491, "ПП РФ №491").Column("PPRF491").Length(2000);
            Reference(x => x.ParentViolationGji, "основное нарушение").Column("PARENT_VIOLATION").Fetch();
            Property(x => x.PpRf170, "ПП РФ №170").Column("PPRF170").Length(2000);
            Property(x => x.OtherNormativeDocs, "Прочие нормативные документы").Column("OTHER_DOCS").Length(2000);
            Reference(x => x.Municipality, "Связь с таблицей муниципальных образований").Column("MUNICIPALITY_ID").Fetch();
            Property(x => x.TypeMunicipality, "TypeMunicipality").Column("TYPE_MUNICIPALITY");
            Reference(x => x.ArticleLaw, "Статья закона").Column("ARTICLELAW_ID").Fetch();
            Reference(x => x.ArticleLawRepeatative, "Статья закона при повторном наршуении").Column("ARTICLELAWREPEAT_ID").Fetch();
        }
    }
}
