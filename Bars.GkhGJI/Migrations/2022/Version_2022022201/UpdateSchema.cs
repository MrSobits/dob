namespace Bars.GkhGji.Migrations._2022.Version_2022022201
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Enums;

    [Migration("2022022201")]
    [MigrationDependsOn(typeof(Version_2022022000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
               "GJI_DICT_SUBPOENA",
               new Column("NAME", DbType.String),
               new Column("DATE_OF_PROCEEDINGS", DbType.DateTime),
               new Column("HOUR_OF_PROCEEDINGS", DbType.Int32),
               new Column("MINUTE_OF_PROCEEDINGS", DbType.Int32),
               new Column("PROCEEDING_COPY_NUM", DbType.Int32),
               new Column("PROCEEDINGS_PLACE", DbType.String),
               new RefColumn("PROTOCOL_ID", "GJI_PROTOCOL_REMOV_GJI_DICT_SUBPOENA", "GJI_PROTOCOL", "ID"),
               new RefColumn("COMISSION_MEETING_ID", "GJI_PROTOCOL_REMOV_GJI_DICT_SUBPOENA", "GJI_COMISSION_MEETING", "ID")
               );
        }
        public override void Down()
        {
            Database.RemoveTable("GJI_DICT_SUBPOENA");
        }
    }
}
