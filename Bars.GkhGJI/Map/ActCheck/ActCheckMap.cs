/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Gkh.Enums;
///     using Entities;
///     using Enums;
/// 
///     using FluentNHibernate.Mapping;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Акт проверки"
///     /// </summary>
///     public class ActCheckMap : SubclassMap<ActCheck>
///     {
///         public ActCheckMap()
///         {
///             Table("GJI_ACTCHECK");
///             KeyColumn("ID");
/// 
///             Map(x => x.ObjectVersion, "OBJECT_VERSION").Not.Nullable();
///             Map(x => x.ObjectCreateDate, "OBJECT_CREATE_DATE").Not.Nullable();
///             Map(x => x.ObjectEditDate, "OBJECT_EDIT_DATE").Not.Nullable();
/// 
///             Map(x => x.TypeActCheck, "TYPE_ACTCHECK").Not.Nullable().CustomType<TypeActCheckGji>();
///             Map(x => x.ToProsecutor, "TO_PROSECUTOR").Not.Nullable().CustomType<YesNoNotSet>();
///             Map(x => x.Area, "AREA");
///             Map(x => x.DateToProsecutor, "DATE_TO_PROSECUTOR");
///             Map(x => x.Flat, "FLAT").Length(10);
///             Map(x => x.ActToPres, "ACT_TO_PRES").Not.Nullable();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;

    /// <summary>Маппинг для "Акт проверки"</summary>
    public class ActCheckMap : JoinedSubClassMap<ActCheck>
    {

        public ActCheckMap() :
                base("Акт проверки", "GJI_ACTCHECK")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.TypeActCheck, "Тип акта").Column("TYPE_ACTCHECK").NotNull();
            this.Property(x => x.ToProsecutor, "Передано в прокуратуру").Column("TO_PROSECUTOR").NotNull();
            this.Property(x => x.Area, "Проверяемая площадь").Column("AREA");
            this.Property(x => x.DateToProsecutor, "Дата передачи").Column("DATE_TO_PROSECUTOR");
            this.Property(x => x.Flat, "Квартира").Column("FLAT").Length(10);
            this.Property(x => x.ActToPres, "Акт направлен в прокуратуру").Column("ACT_TO_PRES").NotNull();
            this.Property(x => x.Unavaliable, "Невозможность проведения проверки").Column("UNAVALIABLE_CHECK").NotNull();
            this.Property(x => x.UnavaliableComment, "Невозможность проведения проверки").Column("UNAVALIABLE_REASON");
            this.Property(x => x.ReferralResolutionToRospotrebnadzor, "Требуется ли направление в Роспотребнадзор")
                .Column("ACT_TO_ROSPOTREBNADZOR").DefaultValue(YesNo.No);
            this.Property(x => x.DocumentPlace, "DocumentPlace").Column("DOCUMENT_PLACE").Length(1000);
            this.Reference(x => x.DocumentPlaceFias, "Место составления (выбор из ФИАС)").Column("DOCUMENT_PLACE_FIAS_ID");
            this.Property(x => x.DocumentTime, "DocumentTime").Column("DOCUMENT_TIME");
            this.Property(x => x.AcquaintState, "Статус ознакомления с результатами проверки").Column("ACQUAINT_STATE");
            this.Property(x => x.RefusedToAcquaintPerson, "ФИО должностного лица, отказавшегося от ознакомления с актом проверки").Column("REFUSED_TO_ACQUAINT_PERSON");
            this.Property(x => x.AcquaintedPerson, "ФИО должностного лица, ознакомившегося с актом проверки").Column("ACQUAINTED_PERSON");
            this.Property(x => x.AcquaintedDate, "Дата ознакомления").Column("ACQUAINTED_DATE");
        }
    }
}
