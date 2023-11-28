namespace Bars.GkhGji.Migrations._2022.Version_2022013102
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh;

    [Migration("2022013102")]
    [MigrationDependsOn(typeof(Version_2022013101.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_DISPOSAL", new Column("PASSPORT_ISSUED", DbType.String));
        }
        public override void Down()
        {
            Database.RemoveColumn("GJI_DISPOSAL", "PASSPORT_ISSUED");
        }
    }
}