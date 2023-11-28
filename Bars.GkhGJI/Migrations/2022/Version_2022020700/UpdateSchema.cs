namespace Bars.GkhGji.Migrations._2022.Version_2022020700
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh;

    [Migration("2022020700")]
    [MigrationDependsOn(typeof(Version_2022020300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_DISPOSAL", new Column("NOTICE_DATE", DbType.DateTime));
            Database.AddColumn("GJI_DISPOSAL", new Column("NOTICE_TIME", DbType.Time));
            Database.AddColumn("GJI_DISPOSAL", new Column("ADDRES_COM", DbType.String));
            Database.AddColumn("GJI_DISPOSAL", new Column("ADDRES_DEPARTURES", DbType.String));
            Database.AddColumn("GJI_DISPOSAL", new Column("POSTCODE", DbType.String));
        }
        public override void Down()
        {
            Database.RemoveColumn("GJI_DISPOSAL", "POSTCODE");
            Database.RemoveColumn("GJI_DISPOSAL", "ADDRES_DEPARTURES");
            Database.RemoveColumn("GJI_DISPOSAL", "ADDRES_COM");
            Database.RemoveColumn("GJI_DISPOSAL", "NOTICE_TIME");
            Database.RemoveColumn("GJI_DISPOSAL", "NOTICE_DATE");
        }
    }
}