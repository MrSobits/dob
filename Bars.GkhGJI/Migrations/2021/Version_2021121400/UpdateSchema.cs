namespace Bars.GkhGji.Migrations._2021.Version_2021121400
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh;

    [Migration("2021121400")]
    [MigrationDependsOn(typeof(Version_2021120900.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.RenameColumn("GJI_DICT_ARTICLELAW", "GIS_GKH_CODE", "KBK_CENTR");
            Database.RenameColumn("GJI_DICT_ARTICLELAW", "GIS_GKH_GUID", "KBK_COMSOMOL");
            Database.RenameColumn("GJI_DICT_ARTICLELAW", "CODE", "OMS");
            Database.RenameColumn("GJI_DICT_ARTICLELAW", "DESCRIPTION", "NAME_OMS");
            Database.AddColumn("GJI_DICT_ARTICLELAW", new Column("BANK", DbType.String, 2000));
        }
        public override void Down()
        {
            Database.RemoveColumn("GJI_DICT_ARTICLELAW", "BANK");
            Database.RenameColumn("GJI_DICT_ARTICLELAW", "NAME_OMS", "DESCRIPTION");
            Database.RenameColumn("GJI_DICT_ARTICLELAW", "OMS", "CODE");
            Database.RenameColumn("GJI_DICT_ARTICLELAW", "KBK_COMSOMOL", "GIS_GKH_GUID");
            Database.RenameColumn("GJI_DICT_ARTICLELAW", "KBK_CENTR", "GIS_GKH_CODE");
        }
    }
}