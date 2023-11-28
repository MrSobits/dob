/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map.RealityAccount
/// {
///     using B4.DataAccess.ByCode;
///     using Entities;
/// 
///     public class RealityObjectChargeAccountOperationMap : BaseImportableEntityMap<RealityObjectChargeAccountOperation>
///     {
///         public RealityObjectChargeAccountOperationMap() : base("REGOP_RO_CHARGE_ACC_CHARGE")
///         {
///             Map(x => x.ChargedTotal, "CCHARGED", true);
///             Map(x => x.PaidTotal, "CPAID", true);
///             Map(x => x.ChargedPenalty, "CCHARGED_PENALTY", true);
///             Map(x => x.PaidPenalty, "CPAID_PENALTY", true);
///             Map(x => x.SaldoIn, "CSALDO_IN", true);
///             Map(x => x.SaldoOut, "CSALDO_OUT", true);
///             Map(x => x.Date, "CDATE", true);
///             Map(x => x.BalanceChange, "BALANCE_CHANGE", true, 0m);
/// 
///             References(x => x.Account, "ACC_ID", ReferenceMapConfig.CascadeDelete);
///             References(x => x.Period, "PERIOD_ID", ReferenceMapConfig.CascadeDelete);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.RegOperator.Entities;
    using System;

    using Bars.Gkh.Map;

    /// <summary>Маппинг для "Начисления по счету начисления дома (группировка по периодам)"</summary>
    public class RealityObjectChargeAccountOperationMap : BaseImportableEntityMap<RealityObjectChargeAccountOperation>
    {
        
        public RealityObjectChargeAccountOperationMap() : 
                base("Начисления по счету начисления дома (группировка по периодам)", "REGOP_RO_CHARGE_ACC_CHARGE")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Date, "Дата операции").Column("CDATE").NotNull();
            Reference(x => x.Period, "Период начислений").Column("PERIOD_ID");
            Reference(x => x.Account, "Счет начислений дома").Column("ACC_ID");
            Property(x => x.ChargedTotal, "Сумма по всем начисленям ЛС по дому за период").Column("CCHARGED").NotNull();
            Property(x => x.SaldoIn, "Входящее сальдо").Column("CSALDO_IN").NotNull();
            Property(x => x.SaldoOut, "Исходящее сальдо").Column("CSALDO_OUT").NotNull();
            Property(x => x.PaidTotal, "Оплачено всего").Column("CPAID").NotNull();
            Property(x => x.ChargedPenalty, "Начислено пени").Column("CCHARGED_PENALTY").NotNull();
            Property(x => x.PaidPenalty, "Оплачено пени").Column("CPAID_PENALTY").NotNull();
            Property(x => x.BalanceChange, "Сумма по операциям установки/изменения сальдо").Column("BALANCE_CHANGE").DefaultValue(0m).NotNull();
        }
    }
}
