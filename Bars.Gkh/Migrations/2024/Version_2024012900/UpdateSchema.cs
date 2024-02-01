namespace Bars.Gkh.Migrations._2024.Version_2024012900
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2024012900")]
    
    [MigrationDependsOn(typeof(_2023.Version_2023041800.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("GKH_DICT_INSP_ZONALINSP_SUBSCRIP",
                new RefColumn("INSP_ID", "GKH_INSP_ZONALINSP_SUBSCR_INSPECTOR", "GKH_DICT_INSPECTOR", "ID"),
                new RefColumn("ZONAL_INSP_ID", "GKH_INSP_ZONALINSP_SUBSCR_ZONALINSP", "GKH_DICT_ZONAINSP", "ID"));

            Database.AddColumn("GKH_DICT_ZONAINSP", new Column ("USE_UFC", DbType.Boolean, false));
        }

        public override void Down()
        {       
            Database.RemoveTable("GKH_DICT_INSP_ZONALINSP_SUBSCRIP");
            Database.RemoveColumn("GKH_DICT_ZONAINSP", "USE_UFC");
        }
    }
}