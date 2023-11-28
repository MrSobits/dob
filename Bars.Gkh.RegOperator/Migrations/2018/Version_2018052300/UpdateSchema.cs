namespace Bars.Gkh.RegOperator.Migrations._2018.Version_2018052300
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2018052300")]
   
    [MigrationDependsOn(typeof(Version_2018051700.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            this.Database.AddColumn("CLW_DEBTOR_CLAIM_WORK", "PIR_CREATE_DATE", DbType.DateTime, ColumnProperty.None);
            this.Database.AddColumn("CLW_DEBTOR_CLAIM_WORK", "SUB_CONTRACT_NUM", DbType.String, ColumnProperty.None);
            this.Database.AddColumn("CLW_DEBTOR_CLAIM_WORK", "SUB_CONTRACT_DATE", DbType.DateTime, ColumnProperty.None);
            Database.AddRefColumn("CLW_DEBTOR_CLAIM_WORK", new RefColumn("SUBCONTRAGENT_ID", ColumnProperty.None, "FK_CLW_DEBTOR_CLAIM_WORK_CONTRAGENT", "GKH_CONTRAGENT", "ID"));

        }
        public override void Down()
        {
            this.Database.RemoveColumn("CLW_DEBTOR_CLAIM_WORK", "PIR_CREATE_DATE");
            this.Database.RemoveColumn("CLW_DEBTOR_CLAIM_WORK", "SUB_CONTRACT_NUM");
            this.Database.RemoveColumn("CLW_DEBTOR_CLAIM_WORK", "SUB_CONTRACT_DATE");

        }
    }
}