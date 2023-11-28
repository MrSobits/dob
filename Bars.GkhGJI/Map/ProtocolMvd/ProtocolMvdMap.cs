/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.GkhGji.Entities;
///     using Bars.GkhGji.Enums;
/// 
///     using FluentNHibernate.Mapping;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Протокол МВД"
///     /// </summary>
///     public class ProtocolMvdMap : SubclassMap<ProtocolMvd>
///     {
///         public ProtocolMvdMap()
///         {
///             Table("GJI_PROTOCOL_MVD");
///             KeyColumn("ID");
/// 
///             Map(x => x.ObjectVersion, "OBJECT_VERSION").Not.Nullable();
///             Map(x => x.ObjectCreateDate, "OBJECT_CREATE_DATE").Not.Nullable();
///             Map(x => x.ObjectEditDate, "OBJECT_EDIT_DATE").Not.Nullable();
/// 
///             Map(x => x.PhysicalPerson, "PHYSICAL_PERSON").Length(300);
///             Map(x => x.TypeExecutant, "TYPE_EXECUTANT").Not.Nullable().CustomType<TypeExecutantProtocolMvd>();
///             Map(x => x.PhysicalPersonInfo, "PHYSICAL_PERSON_INFO").Length(500);
///             Map(x => x.DateSupply, "DATE_SUPPLY");
/// 
///             References(x => x.Municipality, "MUNICIPALITY_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Протокол МВД"</summary>
    public class ProtocolMvdMap : JoinedSubClassMap<ProtocolMvd>
    {
        
        public ProtocolMvdMap() : 
                base("Протокол МВД", "GJI_PROTOCOL_MVD")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.PhysicalPerson, "Физическое лицо").Column("PHYSICAL_PERSON").Length(300);
            Property(x => x.TypeExecutant, "Тип исполнителя документа").Column("TYPE_EXECUTANT").NotNull();
            Property(x => x.PhysicalPersonInfo, "Реквизиты физ. лица").Column("PHYSICAL_PERSON_INFO").Length(500);
            Property(x => x.DateSupply, "Дата поступления в ГЖИ").Column("DATE_SUPPLY");

            Property(x => x.DateOffense, "Дата поступления в ГЖИ").Column("DATE_OFFENSE");
            Property(x => x.SerialAndNumber, "Дата поступления в ГЖИ").Column("SERIAL_AND_NUMBER");
            Property(x => x.BirthDate, "Дата поступления в ГЖИ").Column("BIRTH_DATE");
            Property(x => x.IssueDate, "Дата поступления в ГЖИ").Column("ISSUE_DATE");
            Property(x => x.BirthPlace, "Дата поступления в ГЖИ").Column("BIRTH_PLACE");
            Property(x => x.IssuingAuthority, "Дата поступления в ГЖИ").Column("ISSUING_AUTHORITY");
            Property(x => x.Company, "Дата поступления в ГЖИ").Column("COMPANY");
            Reference(x => x.Municipality, "Муниципальное образование").Column("MUNICIPALITY_ID").Fetch();
            Reference(x => x.OrganMvd, "Орган МВД, оформивший протокол").Column("ORGAN_MVD_ID").Fetch();
        }
    }
}
