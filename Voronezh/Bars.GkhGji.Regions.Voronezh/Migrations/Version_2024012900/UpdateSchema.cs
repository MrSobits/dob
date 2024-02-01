namespace Bars.GkhGji.Regions.Voronezh.Migrations.Version_2024012900
{
    using Bars.B4.Modules.Ecm7.Framework;
    using System.Data;

    [Migration("2024012900")]
    [MigrationDependsOn(typeof(Version_2023071800.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_VR_ASFK_BDOPER", new Column("KOD_DOC_ADB", DbType.Int16, 0));
            Database.AddColumn("GJI_VR_ASFK_BDOPER", new Column("KBK", DbType.String, 20));
            Database.AddColumn("GJI_VR_ASFK_BDOPER", new Column("RELATED_ASFK_ID", DbType.Int64));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_VR_ASFK_BDOPER", "KOD_DOC_ADB");
            Database.RemoveColumn("GJI_VR_ASFK_BDOPER", "KBK");
            Database.RemoveColumn("GJI_VR_ASFK_BDOPER", "RELATED_ASFK_ID");
        }
    }
}


