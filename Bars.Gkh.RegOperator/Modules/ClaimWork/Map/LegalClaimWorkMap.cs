namespace Bars.Gkh.RegOperator.Map
{
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities;
    
    public class LegalClaimWorkMap : GkhJoinedSubClassMap<LegalClaimWork>
    {       
        public LegalClaimWorkMap() : base("CLW_LEGAL_CLAIM_WORK")
        {
        }
        
        protected override void Map()
        {
            this.Reference(x => x.JurisdictionAddress, "Адрес зоны подсудности").Column("JURISDICTION_RO_ID").Fetch();
        }
    }
}