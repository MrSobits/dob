namespace Bars.GkhGji.Migrations._2022.Version_2023042400
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2023042400")]
    [MigrationDependsOn(typeof(Version_2023040300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {
            Database.AddColumn("GJI_PROTOCOL", new Column("CHARGE_AMOUNT", DbType.Decimal));
            Database.AddColumn("GJI_PROTOCOL", new Column("PAID_AMOUNT", DbType.Decimal));
            Database.AddColumn("GJI_PROTOCOL", new Column("FINE_CHARGE_DATE", DbType.DateTime));
            Database.AddColumn("GJI_PROTOCOL", new Column("IS_FINE_CHARGED", DbType.Int16, ColumnProperty.None, 30));
            Database.AddColumn("GJI_PROTOCOL", new Column("COURT_CASE_NUMBER", DbType.String));
            Database.AddColumn("GJI_PROTOCOL", new Column("COURT_CASE_DATE", DbType.DateTime));
            Database.AddColumn("GJI_PROTOCOL", new RefColumn("SANCTION_ID", "GJI_PROTOCOL_SANCTION_ID", "GJI_DICT_SANCTION", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_PROTOCOL", "CHARGE_AMOUNT");
            Database.RemoveColumn("GJI_PROTOCOL", "PAID_AMOUNT");
            Database.RemoveColumn("GJI_PROTOCOL", "FINE_CHARGE_DATE");
            Database.RemoveColumn("GJI_PROTOCOL", "IS_FINE_CHARGED");
            Database.RemoveColumn("GJI_PROTOCOL", "FINE_CHARGE_DATE");
            Database.RemoveColumn("GJI_PROTOCOL", "IS_FINE_CHARGED");
            Database.RemoveColumn("GJI_PROTOCOL", "SANCTION_ID");
        }
    }
}
