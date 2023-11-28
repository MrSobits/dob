/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Tat.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Overhaul.Tat.Entities;
/// 
///     public class SubsidyMunicipalityRecordMap : BaseEntityMap<SubsidyMunicipalityRecord>
///     {
///         public SubsidyMunicipalityRecordMap()
///             : base("OVRHL_SUBSIDY_MU_REC")
///         {
///             References(x => x.SubsidyMunicipality, "SUBSIDY_MU_ID", ReferenceMapConfig.NotNullAndFetch);
/// 
///             Map(x => x.SubsidyYear, "SUBCIDY_YEAR", true, 0);
///             Map(x => x.BudgetFcr, "BUDGET_FCR", true, 0);
///             Map(x => x.BudgetRegion, "BUDGET_REGION", true, 0);
///             Map(x => x.BudgetMunicipality, "BUDGET_MUNICIPALITY", true, 0);
///             Map(x => x.OwnerSource, "OWNER_SOURCE", true, 0);
///             Map(x => x.BudgetCr, "BUDGET_CR", true, 0);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Tat.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Tat.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Tat.Entities.SubsidyMunicipalityRecord"</summary>
    public class SubsidyMunicipalityRecordMap : BaseEntityMap<SubsidyMunicipalityRecord>
    {
        
        public SubsidyMunicipalityRecordMap() : 
                base("Bars.Gkh.Overhaul.Tat.Entities.SubsidyMunicipalityRecord", "OVRHL_SUBSIDY_MU_REC")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.SubsidyMunicipality, "SubsidyMunicipality").Column("SUBSIDY_MU_ID").NotNull().Fetch();
            Property(x => x.SubsidyYear, "SubsidyYear").Column("SUBCIDY_YEAR").NotNull();
            Property(x => x.BudgetFcr, "BudgetFcr").Column("BUDGET_FCR").NotNull();
            Property(x => x.BudgetRegion, "BudgetRegion").Column("BUDGET_REGION").NotNull();
            Property(x => x.BudgetMunicipality, "BudgetMunicipality").Column("BUDGET_MUNICIPALITY").NotNull();
            Property(x => x.OwnerSource, "OwnerSource").Column("OWNER_SOURCE").NotNull();
            Property(x => x.BudgetCr, "BudgetCr").Column("BUDGET_CR").NotNull();
        }
    }
}
