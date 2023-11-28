namespace Bars.GkhGji.Migrations._2022.Version_2022020200
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh;

    [Migration("2022020200")]
    [MigrationDependsOn(typeof(Version_2022013102.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_DISPOSAL", new Column("NAME_TRANSPORT", DbType.String));
            Database.AddColumn("GJI_DISPOSAL", new Column("NAMBER_TRANSPORT", DbType.String));
            Database.AddColumn("GJI_DISPOSAL", new Column("REGISTRATION_NAMBER_TRANSPORT", DbType.Int64));
            Database.AddColumn("GJI_DISPOSAL", new Column("SERIES_TRANSPORT", DbType.String));
            Database.AddColumn("GJI_DISPOSAL", new Column("REG_NAMBER_TRANSPORT", DbType.Int64));
        }
        public override void Down()
        {
            Database.RemoveColumn("GJI_DISPOSAL", "REG_NAMBER_TRANSPORT");
            Database.RemoveColumn("GJI_DISPOSAL", "SERIES_TRANSPORT");
            Database.RemoveColumn("GJI_DISPOSAL", "REGISTRATION_NAMBER_TRANSPORT");
            Database.RemoveColumn("GJI_DISPOSAL", "NAMBER_TRANSPORT");
            Database.RemoveColumn("GJI_DISPOSAL", "NAME_TRANSPORT");
        }
    }
}