/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.GkhGji.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Мероприятия по устранению нарушений"
///     /// </summary>
///     public class ActionsRemovViolMap : BaseEntityMap<ActionsRemovViol>
///     {
///         public ActionsRemovViolMap()
///             : base("GJI_DICT_ACTREMOVVIOL")
///         {
///             Map(x => x.Name, "NAME").Length(500).Not.Nullable();
///             Map(x => x.Code, "CODE").Length(300);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Мероприятия по устранению нарушений"</summary>
    public class OwnerMap : BaseEntityMap<Owner>
    {
        
        public OwnerMap() : 
                base("Мероприятия по устранению нарушений", "GJI_OWNER")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.NameTransport, "Наименование транспорта").Column("NAME_TRANSPORT");
            Property(x => x.NamberTransport, "Номер транспорта").Column("NAMBER_TRANSPORT");
            Property(x => x.TypeViolator, "Тип нарушителя").Column("TYPE_VIOLATOR");
            Property(x => x.DataOwnerStart, "Тип нарушителя").Column("DATESTART");
            Property(x => x.DataOwnerEdit, "Тип нарушителя").Column("DATEEND");
            Reference(x => x.IndividualPerson, "Связь с таблицей физ лица").Column("INDIVIDUAL_PERSON_ID").Fetch();
            Reference(x => x.Contragent, "Связь с таблицей контрагент").Column("CONTRAGENT_ID").Fetch();
            Reference(x => x.ContragentContact, "Связь с таблицей контакты контрагетов").Column("CONTRAGENT_CONTACT_ID").Fetch();
            Reference(x => x.Transport, "Связь с таблицей транспорт").Column("TRANSPORT_ID").Fetch();
        }
    }
}
