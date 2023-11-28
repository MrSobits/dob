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
    public class OwnerRoomMap : BaseEntityMap<OwnerRoom>
    {
        
        public OwnerRoomMap() : 
                base("Мероприятия по устранению нарушений", "GJI_OWNER_ROOM")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.DataOwnerStart, "Дата начала владения собственностью").Column("DATA_OWNER_START");
            Property(x => x.DataOwnerEdit, "Дата конца владения собственностью").Column("DATA_OWNER_EDIT");
            Property(x => x.TypeViolatorOwnerRoom, "Тип нарушителя для адресов").Column("TYPE_VIOLATOR_OWNER_ROOM");
            Reference(x => x.IndividualPerson, "Связь с таблицей физ лиц ").Column("INDIVIDUAL_PERSON_ID").Fetch();
            Reference(x => x.Contragent, "Связь с таблицей контрагентов").Column("CONTRAGENT_ID").Fetch();
            Reference(x => x.Room, "Связь с таблицей помещений").Column("ROOM_ID").Fetch();
        }
    }
}
