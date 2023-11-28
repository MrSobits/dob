namespace Bars.GkhGji.Migrations._2022.Version_2022050900
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Enums;

    [Migration("2022050900")]
    [MigrationDependsOn(typeof(Version_2022040700.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_RESOLUTION", new Column("PROTOCOL205_DATE", DbType.DateTime));
        }
        public override void Down()
        {
            Database.RemoveColumn("GJI_RESOLUTION", "PROTOCOL205_DATE");
        }
    }
}
