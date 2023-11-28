/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.GkhGji.Entities;
///     using Bars.GkhGji.Enums;
/// 
///     using FluentNHibernate.Mapping;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Основание проверки ГЖИ по распоряжению руководителя"
///     /// </summary>
///     public class BaseDispHeadMap : SubclassMap<BaseDispHead>
///     {
///         public BaseDispHeadMap()
///         {
///             Table("GJI_INSPECTION_DISPHEAD");
///             KeyColumn("ID");
/// 
///             Map(x => x.ObjectVersion, "OBJECT_VERSION").Not.Nullable();
///             Map(x => x.ObjectCreateDate, "OBJECT_CREATE_DATE").Not.Nullable();
///             Map(x => x.ObjectEditDate, "OBJECT_EDIT_DATE").Not.Nullable();
///             Map(x => x.DispHeadDate, "DISPHEAD_DATE");
///             Map(x => x.DocumentDate, "DOCUMENT_DATE");
///             Map(x => x.DocumentName, "DOCUMENT_NAME").Length(300);
///             Map(x => x.DocumentNumber, "DOCUMENT_NUMBER").Length(50);
///             Map(x => x.TypeBaseDispHead, "TYPE_BASE_DISPHEAD").Not.Nullable().CustomType<TypeBaseDispHead>();
///             Map(x => x.TypeForm, "TYPE_FORM").Not.Nullable().CustomType<TypeFormInspection>();
/// 
///             References(x => x.Head, "HEAD_ID").LazyLoad();
///             References(x => x.File, "FILE_INFO_ID").LazyLoad();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Основание распоряжения руководителя ГЖИ"</summary>
    public class BaseDispHeadMap : JoinedSubClassMap<BaseDispHead>
    {
        
        public BaseDispHeadMap() : 
                base("Основание распоряжения руководителя ГЖИ", "GJI_INSPECTION_DISPHEAD")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.DispHeadDate, "Дата распоряжения").Column("DISPHEAD_DATE");
            Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE");
            Property(x => x.DocumentName, "Наименование документа").Column("DOCUMENT_NAME").Length(300);
            Property(x => x.DocumentNumber, "Номер документа").Column("DOCUMENT_NUMBER").Length(50);
            Property(x => x.TypeBaseDispHead, "Тип основания проверки поручения руководства").Column("TYPE_BASE_DISPHEAD").NotNull();
            Property(x => x.TypeForm, "Форма проверки").Column("TYPE_FORM").NotNull();
            Reference(x => x.Head, "Руководитель").Column("HEAD_ID");
            Reference(x => x.File, "Файл").Column("FILE_INFO_ID");
        }
    }
}
