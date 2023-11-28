namespace Bars.GkhGji.Migrations._2022.Version_2022021000
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh;
    
    [Migration("2022021000")]
    [MigrationDependsOn(typeof(Version_2022020901.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
                "GJI_COMISSION_MEETING",
                new Column("COMISSION_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("COMISSION_NAME", DbType.String, 500),
                new Column("COMISSION_NUMBER", DbType.String, 20),
                new Column("TIME_END", DbType.String, 20),
                new Column("TIME_START", DbType.String, 20),
                new Column("DESCRIPTION", DbType.String, 1500, ColumnProperty.None),
                new RefColumn("STATE_ID", "GJI_COMISSION_MEETING_STATE", "B4_STATE", "ID"),
                new RefColumn("ZONAL_ID", "GJI_COMISSION_MEETING_ZONAL", "GKH_DICT_ZONAINSP", "ID"));

            Database.AddEntityTable(
             "GJI_COMISSION_MEETING_INSPECTOR",
             new Column("PRESENT", DbType.Int32, 4, ColumnProperty.NotNull, 30),
             new Column("DESCRIPTION", DbType.String, 1500, ColumnProperty.None),
             new RefColumn("INSPECTOR_ID", "GJI_COMISSION_MEETING_INSPECTOR_ID", "GKH_DICT_INSPECTOR", "ID"),
             new RefColumn("MEETING_ID", "GJI_COMMEETING_INSPECTOR_MEETING_ID", "GJI_COMISSION_MEETING", "ID"));

        }
        public override void Down()
        {
            Database.RemoveTable("GJI_COMISSION_MEETING_INSPECTOR");
            Database.RemoveTable("GJI_COMISSION_MEETING");
        }
    }
}