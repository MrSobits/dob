namespace Bars.GkhGji.Migrations._2022.Version_2022040700
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Enums;

    [Migration("2022040700")]
    [MigrationDependsOn(typeof(Version_2022032000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_DOCUMENT", new Column("CASE_NUMBER", DbType.String, 20));
        }
        public override void Down()
        {
            Database.RemoveColumn("GJI_DOCUMENT", "CASE_NUMBER");
        }
    }
}
