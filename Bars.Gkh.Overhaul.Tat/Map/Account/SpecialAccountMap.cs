/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Tat.Map
/// {
///     using Entities;
/// 
///     using FluentNHibernate.Mapping;
/// 
///     public class SpecialAccountMap: SubclassMap<SpecialAccount>
///     {
///         public SpecialAccountMap()
///         {
///             Table("OVRHL_SPECIAL_ACCOUNT");
///             KeyColumn("ID");
/// 
///             References(x => x.AccountOwner, "OWNER_ID");
///             References(x => x.CreditOrganization, "CREDIT_ORG_ID");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Tat.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Tat.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Tat.Entities.SpecialAccount"</summary>
    public class SpecialAccountMap : JoinedSubClassMap<SpecialAccount>
    {
        
        public SpecialAccountMap() : 
                base("Bars.Gkh.Overhaul.Tat.Entities.SpecialAccount", "OVRHL_SPECIAL_ACCOUNT")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.AccountOwner, "AccountOwner").Column("OWNER_ID");
            Reference(x => x.CreditOrganization, "CreditOrganization").Column("CREDIT_ORG_ID");
        }
    }
}
