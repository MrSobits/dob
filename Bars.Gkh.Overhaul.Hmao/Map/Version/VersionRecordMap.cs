/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Hmao.Map
/// {
///     using System.Collections.Generic;
/// 
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.B4.DataAccess.UserTypes;
///     using Bars.Gkh.Overhaul.Hmao.Entities;
/// 
///     public class VersionRecordMap : BaseImportableEntityMap<VersionRecord>
///     {
///         public VersionRecordMap()
///             : base("OVRHL_VERSION_REC")
///         {
///             References(x => x.ProgramVersion, "VERSION_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.RealityObject, "RO_ID", ReferenceMapConfig.NotNullAndFetch);
/// 
///             Map(x => x.Year, "YEAR", true, 0);
///             Map(x => x.FixedYear, "FIXED_YEAR", true);
///             Map(x => x.Sum, "SUM", true, 0);
///             Map(x => x.CommonEstateObjects, "CEO_STRING", true);
///             Map(x => x.Point, "POINT", true, 0m);
///             Map(x => x.IndexNumber, "INDEX_NUM", true, 0);
///             Map(x => x.IsChangedYear, "IS_CHANGED_YEAR", false, false);
///             Map(x => x.IsManuallyCorrect, "IS_MANUALLY_CORRECT", true, false);
///             Map(x => x.Changes, "CHANGES");
///             Map(x => x.IsAddedOnActualize, "IS_ADD_ACTUALIZE");
///             Map(x => x.IsChangedYearOnActualize, "IS_CH_YEAR_ACTUALIZE");
///             Map(x => x.IsChangeSumOnActualize, "IS_CH_SUM_ACTUALIZE");
///             Property(x => x.StoredCriteria,
///                      m =>
///                      {
///                          m.Type<JsonSerializedType<List<StoredPriorityParam>>>();
///                          m.Column("CRITERIA");
///                      });
/// 
/// 
///             Property(x => x.StoredPointParams,
///                     m =>
///                     {
///                         m.Type<JsonSerializedType<List<StoredPointParam>>>();
///                         m.Column("POINT_PARAMS");
///                     });
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Hmao.Map
{
    using Bars.B4.DataAccess.UserTypes;
    using Bars.Gkh.Map;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using NHibernate.Mapping.ByCode.Conformist;
    using System.Collections.Generic;

    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Hmao.Entities.VersionRecord"</summary>
    public class VersionRecordMap : BaseImportableEntityMap<VersionRecord>
    {
        
        public VersionRecordMap() : 
                base("Bars.Gkh.Overhaul.Hmao.Entities.VersionRecord", "OVRHL_VERSION_REC")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.ProgramVersion, "Версия программы").Column("VERSION_ID").NotNull().Fetch();
            Reference(x => x.RealityObject, "Жилой дом").Column("RO_ID").NotNull().Fetch();
            Property(x => x.Year, "Плановый Год").Column("YEAR").NotNull();
            Property(x => x.FixedYear, "Год зафиксирован").Column("FIXED_YEAR").NotNull();
            Property(x => x.CommonEstateObjects, "Строка объектов общего имущества").Column("CEO_STRING").Length(250).NotNull();
            Property(x => x.Sum, "Сумма").Column("SUM").NotNull();
            Property(x => x.IsChangedYear, "Изменялся ли год ремонта").Column("IS_CHANGED_YEAR").DefaultValue(false);
            Property(x => x.IndexNumber, "Порядковый номер").Column("INDEX_NUM").NotNull();
            Property(x => x.Point, "Балл").Column("POINT").DefaultValue(0m).NotNull();
            Property(x => x.StoredCriteria, "Значения критериев сортировки").Column("CRITERIA");
            Property(x => x.StoredPointParams, "Значения параметров очередности по баллам").Column("POINT_PARAMS");
            Property(x => x.IsManuallyCorrect, "Была ли ручная корректировка записи").Column("IS_MANUALLY_CORRECT").DefaultValue(false).NotNull();
            Property(x => x.IsAddedOnActualize, "Была добавлена при актуализации \"Добавить новые записи\"").Column("IS_ADD_ACTUALIZE");
            Property(x => x.IsChangedYearOnActualize, "Была добавлена при актуализации \"Актуализировать год\"").Column("IS_CH_YEAR_ACTUALIZE");
            Property(x => x.IsChangeSumOnActualize, "Была добавлена при актуализации \"Актуализировать сумму\"").Column("IS_CH_SUM_ACTUALIZE");
            Property(x => x.IsDividedRec, "Была добавлена в результате разделения").Column("IS_DIVIDED_REC");
            Property(x => x.PublishYearForDividedRec, "Опубликованный год (только для отщепенцев!!!!)").Column("PUBLISH_YEAR_FOR_DIV_REC");
            Property(x => x.Changes, "Изменения записи").Column("CHANGES").Length(250);
            Property(x => x.Remark, "Изменения записи").Column("REMARK").Length(500);
            Property(x => x.Show, "Показывать в ДПКР").Column("IS_SHOW").Length(250);
            Property(x => x.SubProgram, "Переведен в подпрограмму").Column("IS_SUBPROGRAM");
        }
    }

    public class VersionRecordNHibernateMapping : ClassMapping<VersionRecord>
    {
        public VersionRecordNHibernateMapping()
        {
            Property(
                x => x.StoredCriteria,
                m =>
                    {
                        m.Type<JsonSerializedType<List<StoredPriorityParam>>>();
                    });


            Property(
                x => x.StoredPointParams,
                m =>
                    {
                        m.Type<JsonSerializedType<List<StoredPointParam>>>();
                    });
        }
    }
}
