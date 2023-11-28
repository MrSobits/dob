namespace Bars.GkhGji.Regions.Voronezh.Migrations.Version_2023071800
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2023071800")]
    [MigrationDependsOn(typeof(Version_2023070700.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_VR_ASFK_BDOPER",
            new RefColumn("RESOLUTION_ID", ColumnProperty.None, "GJI_VR_BDOPER_RESOLUTION_ID", "GJI_RESOLUTION", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_VR_ASFK_BDOPER", "RESOLUTION_ID");
        }
    }
}


