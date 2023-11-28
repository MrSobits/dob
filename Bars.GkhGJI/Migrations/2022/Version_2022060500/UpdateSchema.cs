namespace Bars.GkhGji.Migrations._2022.Version_2022060500
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Enums;

    [Migration("2022060500")]
    [MigrationDependsOn(typeof(Version_2022050900.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_OWNER", new Column("DATESTART", DbType.DateTime, ColumnProperty.None));
            Database.AddColumn("GJI_OWNER", new Column("DATEEND", DbType.DateTime, ColumnProperty.None));
        }
        public override void Down()
        {
            Database.RemoveColumn("GJI_OWNER", "DATESTART");
            Database.RemoveColumn("GJI_OWNER", "DATEEND");
        }
    }
}
