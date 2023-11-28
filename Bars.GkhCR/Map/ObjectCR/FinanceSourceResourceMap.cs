/// <mapping-converter-backup>
/// using Bars.B4.DataAccess.ByCode;
/// 
/// namespace Bars.GkhCr.Map
/// {
///     using Bars.GkhCr.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Средства источника финансирования"
///     /// </summary>
///     public class FinanceSourceResourceMap : BaseImportableEntityMap<FinanceSourceResource>
///     {
///         public FinanceSourceResourceMap() : base("CR_OBJ_FIN_SOURCE_RES")
///         {
///             Map(x => x.BudgetMu, "BUDGET_MU");
///             Map(x => x.BudgetSubject, "BUDGET_SUB");
///             Map(x => x.OwnerResource, "OWNER_RES");
///             Map(x => x.FundResource, "FUND_RES");
///             Map(x => x.BudgetMuIncome, "BUDGET_MU_INCOME");
///             Map(x => x.BudgetSubjectIncome, "BUDGET_SUB_INCOME");
///             Map(x => x.FundResourceIncome, "FUND_RES_INCOME");
///             Map(x => x.ExternalId, "EXTERNAL_ID");
///             Map(x => x.Year, "YEAR");
/// 
///             References(x => x.TypeWorkCr, "TYPE_WORK_ID", ReferenceMapConfig.Fetch);
///             References(x => x.ObjectCr, "OBJECT_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.FinanceSource, "FIN_SOURCE_ID", ReferenceMapConfig.Fetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhCr.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;
    
    
    /// <summary>Маппинг для "Средства источника финансирования"</summary>
    public class FinanceSourceResourceMap : BaseImportableEntityMap<FinanceSourceResource>
    {
        
        public FinanceSourceResourceMap() : 
                base("Средства источника финансирования", "CR_OBJ_FIN_SOURCE_RES")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.ObjectCr, "Объект капитального ремонта").Column("OBJECT_ID").NotNull().Fetch();
            Reference(x => x.FinanceSource, "Разрез финансирования").Column("FIN_SOURCE_ID").Fetch();
            Property(x => x.BudgetMu, "Бюджет МО").Column("BUDGET_MU");
            Property(x => x.BudgetSubject, "Бюджет субъекта").Column("BUDGET_SUB");
            Property(x => x.OwnerResource, "Средства собственника").Column("OWNER_RES");
            Property(x => x.FundResource, "Средства фонда").Column("FUND_RES");
            Property(x => x.BudgetMuIncome, "Поступило из Бюджет МО").Column("BUDGET_MU_INCOME");
            Property(x => x.BudgetSubjectIncome, "Поступило из Бюджет субъекта").Column("BUDGET_SUB_INCOME");
            Property(x => x.FundResourceIncome, "Поступило из Средства фонда").Column("FUND_RES_INCOME");
            Reference(x => x.TypeWorkCr, "Вид работ").Column("TYPE_WORK_ID").Fetch();
            Property(x => x.Year, "Год").Column("YEAR");
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID").Length(250);
        }
    }
}
