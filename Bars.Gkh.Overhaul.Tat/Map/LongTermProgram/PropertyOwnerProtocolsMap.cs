namespace Bars.Gkh.Overhaul.Tat.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Tat.Entities;

    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Tat.Entities.PropertyOwnerProtocols"</summary>
    public class PropertyOwnerProtocolsMap : BaseEntityMap<PropertyOwnerProtocols>
    {
        public PropertyOwnerProtocolsMap()
            :
                base("Bars.Gkh.Overhaul.Tat.Entities.PropertyOwnerProtocols", "OVRHL_PROP_OWN_PROTOCOLS")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.DocumentDate, "DocumentDate").Column("DOCUMENT_DATE");
            this.Property(x => x.DocumentNumber, "DocumentNumber").Column("DOCUMENT_NUMBER");
            this.Property(x => x.Description, "Description").Column("DESCRIPTION");
            this.Property(x => x.NumberOfVotes, "NumberOfVotes").Column("NUMBER_OF_VOTES");
            this.Property(x => x.TotalNumberOfVotes, "TotalNumberOfVotes").Column("TOTAL_NUMBER_OF_VOTES");
            this.Property(x => x.PercentOfParticipating, "PercentOfParticipating").Column("PERCENT_OF_PARTICIPATE");
            this.Property(x => x.TypeProtocol, "TypeProtocol").Column("TYPE_PROTOCOL").NotNull();
            this.Reference(x => x.RealityObject, "RealityObject").Column("REALITY_OBJECT_ID").NotNull().Fetch();
            this.Reference(x => x.DocumentFile, "DocumentFile").Column("DOCUMENT_FILE_ID").Fetch();
            this.Property(x => x.LoanAmount, "Сумма займа").Column("LOAN_AMOUNT");
            this.Reference(x => x.Borrower, "Заемщик").Column("BORROWER_CONTRAGENT_ID");
            this.Reference(x => x.Lender, "Кредитор").Column("LENDER_CONTRAGENT_ID");
        }
    }
}