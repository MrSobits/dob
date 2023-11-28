/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Entities;
/// 
///     public class RegopCalcAccountMap : BaseJoinedSubclassMap<RegopCalcAccount>
///     {
///         public RegopCalcAccountMap()
///             : base("REGOP_CALC_ACC_REGOP", "ID")
///         {
///             References(x => x.ContragentCreditOrg, "CONTR_CREDIT_ORG_ID", ReferenceMapConfig.Fetch);
///             Map(x => x.IsTransit, "IS_TRANSIT");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.RegOperator.Entities;
    
    
    /// <summary>Маппинг для "Расчетный счет регоператора"</summary>
    public class RegopCalcAccountMap : JoinedSubClassMap<RegopCalcAccount>
    {
        
        public RegopCalcAccountMap() : 
                base("Расчетный счет регоператора", "REGOP_CALC_ACC_REGOP")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.ContragentCreditOrg, "Расчетный счет (Кредитная организация контрагента)").Column("CONTR_CREDIT_ORG_ID").Fetch();
            Property(x => x.IsTransit, "счет является транзитным").Column("IS_TRANSIT");
        }
    }
}
