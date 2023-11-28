/// <mapping-converter-backup>
/// namespace Bars.GkhCr.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.GkhCr.Entities;
///     using NHibernate.Mapping.ByCode;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Акт выполненных работ мониторинга СМР"
///     /// </summary>
///     public class PerformedWorkActMap : BaseImportableEntityMap<PerformedWorkAct>
///     {
///         public PerformedWorkActMap() : base("CR_OBJ_PERFOMED_WORK_ACT")
///         {
///             Map(x => x.ExternalId, "EXTERNAL_ID");
///             Map(x => x.Volume, "VOLUME");
///             Map(x => x.Sum, "SUM");
///             Map(x => x.DocumentNum, "DOCUMENT_NUM", false, 300);
///             Map(x => x.DateFrom, "DATE_FROM");
/// 
///             References(x => x.ObjectCr, "OBJECT_ID", ReferenceMapConfig.Fetch);
///             References(x => x.State, "STATE_ID", ReferenceMapConfig.Fetch);
///             References(x => x.CostFile, "COST_FILE_ID", ReferenceMapConfig.Fetch);
///             References(x => x.DocumentFile, "DOC_FILE_ID", ReferenceMapConfig.Fetch);
///             References(x => x.AdditionFile, "ADDIT_FILE_ID", ReferenceMapConfig.Fetch);
/// 
///             ManyToOne(x => x.TypeWorkCr, m =>
///             {
///                 m.Column("TYPE_WORK_CR_ID");
///                 m.Cascade(Cascade.DeleteOrphans);
///                 m.Lazy(LazyRelation.Proxy);
///                 m.Fetch(FetchKind.Select);
///                 m.NotNullable(true);
///             });
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhCr.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;
    
    
    /// <summary>Маппинг для "Акт выполненных работ"</summary>
    public class PerformedWorkActMap : BaseImportableEntityMap<PerformedWorkAct>
    {
        
        public PerformedWorkActMap() : 
                base("Акт выполненных работ", "CR_OBJ_PERFOMED_WORK_ACT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID").Length(250);
            Reference(x => x.ObjectCr, "Объект капитального ремонта").Column("OBJECT_ID").Fetch();
            Reference(x => x.TypeWorkCr, "Работа").Column("TYPE_WORK_CR_ID").NotNull();
            Property(x => x.DocumentNum, "Номер акта").Column("DOCUMENT_NUM").Length(300);
            Property(x => x.Volume, "Объем").Column("VOLUME");
            Property(x => x.Sum, "Сумма").Column("SUM");
            Property(x => x.OverLimits, "Превышение плановой суммы").Column("OVER_LIMITS");
            Property(x => x.DateFrom, "Дата от").Column("DATE_FROM");
            Property(x => x.SumTransfer, "Сумма перевода").Column("CR_OBJ_PERFOMED_WORK_ACT");
            Property(x => x.DateFromTransfer, "Дата перевода").Column("DATE_FROM_TRANSFER");
            Reference(x => x.State, "Статус").Column("STATE_ID").Fetch();
            Reference(x => x.CostFile, "Справка о стоимости выполненных работ и затрат").Column("COST_FILE_ID").Fetch();
            Reference(x => x.DocumentFile, "Документ акта").Column("DOC_FILE_ID").Fetch();
            Reference(x => x.AdditionFile, "Приложение к акту").Column("ADDIT_FILE_ID").Fetch();
            Property(x => x.UsedInExport, "Выводить документ на портал").Column("USED_IN_EXPORT");
        }
    }
}
