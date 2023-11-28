/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Entities;
/// 
///     public class PeriodMap : BaseImportableEntityMap<ChargePeriod>
///     {
///         public PeriodMap() : base("REGOP_PERIOD")
///         {
///             Map(x => x.StartDate, "CSTART", true);
///             Map(x => x.EndDate, "CEND", false);
///             Map(x => x.IsClosed, "CIS_CLOSED", true);
///             Map(x => x.Name, "PERIOD_NAME", true);
///             Map(x => x.IsClosing, "IS_CLOSING", true);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities;
    
    
    /// <summary>Маппинг для "Период начислений"</summary>
    public class ChargePeriodMap : BaseImportableEntityMap<ChargePeriod>
    {
        
        public ChargePeriodMap() : 
                base("Период начислений", "REGOP_PERIOD")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Name, "Наименование периода").Column("PERIOD_NAME").Length(250).NotNull();
            Property(x => x.StartDate, "Дата открытия периода").Column("CSTART").NotNull();
            Property(x => x.EndDate, "Дата закрытия периода").Column("CEND");
            Property(x => x.IsClosed, "Флаг: период закрыт").Column("CIS_CLOSED").NotNull();
            Property(x => x.IsClosing, "Признак, что период закрывается").Column("IS_CLOSING").NotNull();
        }
    }
}
