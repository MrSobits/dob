/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Bars.Gkh.Entities.Dicts;
///     using Bars.Gkh.Enums;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Работы"
///     /// </summary>
///     public class WorkMap : BaseGkhEntityMap<Work>
///     {
///         public WorkMap()
///             : base("GKH_DICT_WORK")
///         {
///             Map(x => x.Name, "NAME").Not.Nullable().Length(300);
///             Map(x => x.Description, "DESCRIPTION").Length(500);
///             Map(x => x.Code, "CODE").Length(10);
///             Map(x => x.Consistent185Fz, "CONSISTENT185FZ").Not.Nullable();
///             Map(x => x.IsAdditionalWork, "IS_ADDITIONAL_WORK").Not.Nullable();
///             Map(x => x.Normative, "NORMATIVE");
///             Map(x => x.TypeWork, "TYPE_WORK").Not.Nullable().CustomType<TypeWork>();
/// 
///             References(x => x.UnitMeasure, "UNIT_MEASURE_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map.Dicts
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Dicts;

    /// <summary>Маппинг для "Работы"</summary>
    public class WorkMap : BaseImportableEntityMap<Work>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public WorkMap() :
                base("Работы", "GKH_DICT_WORK")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            this.Property(x => x.Name, "Наименование").Column("NAME").Length(300).NotNull();
            this.Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(500);
            this.Property(x => x.Code, "Код").Column("CODE").Length(10);
            this.Property(x => x.ReformCode, "Код реформы").Column("REFORM_CODE").Length(10);
            this.Property(x => x.WorkAssignment, "Назначение работ").Column("WORK_ASSIGNMENT");
            this.Property(x => x.Consistent185Fz, "Соответсвие 185 ФЗ").Column("CONSISTENT185FZ").NotNull();
            this.Property(x => x.IsAdditionalWork, "Дополнительная работа, нужно для ДПКР").Column("IS_ADDITIONAL_WORK").NotNull();
            this.Property(x => x.IsConstructionWork, "Работа (услуга) по строительству (Татарстан)").Column("IS_CONSTRUCTION_WORK").NotNull();
            this.Property(x => x.IsPSD, "Работа (услуга) по ПСД").Column("IS_PSD");
            this.Property(x => x.Normative, "Норматив").Column("NORMATIVE");
            this.Property(x => x.TypeWork, "Тип работ").Column("TYPE_WORK").NotNull();
            this.Reference(x => x.UnitMeasure, "Ед. измерения").Column("UNIT_MEASURE_ID").Fetch();
            this.Property(x => x.GisGkhCode, "Код ГИС ЖКХ").Column("GIS_GKH_CODE");
            this.Property(x => x.GisGkhGuid, "ГИС ЖКХ GUID").Column("GIS_GKH_GUID").Length(36);
        }
    }
}
