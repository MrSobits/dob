namespace Bars.Gkh.Migrations._2021.Version_2021121900
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2021121900")]
    
    [MigrationDependsOn(typeof(Version_2021121800.UpdateSchema))]


    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_CIT_SUG", new Column("LATITUDE", DbType.Decimal, 5000, ColumnProperty.None));
            Database.AddColumn("GKH_CIT_SUG", new Column("LONGITUBE", DbType.Decimal, 5000, ColumnProperty.None));

            Database.AddColumn("GKH_CONTRAGENT_CONTACT", new Column("PLACE_RESIDENCE", DbType.String, ColumnProperty.NotNull));
            Database.AddColumn("GKH_CONTRAGENT_CONTACT", new Column("BIRTHPLACE", DbType.String, ColumnProperty.NotNull));
            Database.AddColumn("GKH_CONTRAGENT_CONTACT", new Column("PASSPORT_ID", DbType.Int32, ColumnProperty.NotNull));
            Database.AddColumn("GKH_CONTRAGENT_CONTACT", new Column("PASSPORT_SERIES", DbType.Int32, ColumnProperty.NotNull));
            Database.AddColumn("GKH_CONTRAGENT_CONTACT", new Column("PASSPORT_ISSUED", DbType.String, ColumnProperty.NotNull));
            Database.AddColumn("GKH_CONTRAGENT_CONTACT", new Column("DEPARTMENT_CODE", DbType.Int32, ColumnProperty.NotNull));
            Database.AddColumn("GKH_CONTRAGENT_CONTACT", new Column("DATE_ISSUE", DbType.DateTime, ColumnProperty.NotNull));

        }

        /// <summary>
        /// Откатить
        /// </summary>
        public override void Down()
        {

            Database.RemoveColumn("GKH_CIT_SUG", "LONGITUBE");
            Database.RemoveColumn("GKH_CIT_SUG", "LATITUDE");

            Database.RemoveColumn("GKH_CONTRAGENT_CONTACT", "DATE_ISSUE");
            Database.RemoveColumn("GKH_CONTRAGENT_CONTACT", "DEPARTMENT_CODE");
            Database.RemoveColumn("GKH_CONTRAGENT_CONTACT", "PASSPORT_ISSUED");
            Database.RemoveColumn("GKH_CONTRAGENT_CONTACT", "PASSPORT_SERIES");
            Database.RemoveColumn("GKH_CONTRAGENT_CONTACT", "PASSPORT_ID");
            Database.RemoveColumn("GKH_CONTRAGENT_CONTACT", "BIRTHPLACE");
            Database.RemoveColumn("GKH_CONTRAGENT_CONTACT", "PLACE_RESIDENCE");
        }
    }
}