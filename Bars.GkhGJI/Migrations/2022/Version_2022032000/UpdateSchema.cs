namespace Bars.GkhGji.Migrations._2022.Version_2022032000
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Enums;

    [Migration("2022032000")]
    [MigrationDependsOn(typeof(Version_2022022201.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
               "GJI_COMISSION_MEETING_DOCUMENT",
               new Column("DESCRIPTION", DbType.String, 1500),
               new Column("DECISION", DbType.Int32, 4, ColumnProperty.NotNull, 0),
               new RefColumn("DOCUMENT_ID", "GJI_CMDOCUMENT_DOCUMENT_ID", "GJI_DOCUMENT", "ID"),
               new RefColumn("MEETING_ID", "GJI_CMDOCUMENT_MEETING_ID", "GJI_COMISSION_MEETING", "ID")
               );
            Database.AddEntityTable(
         "GJI_DICT_MATERIAL_TYPE",
         new Column("NAME", DbType.String, 1500),
         new Column("CODE", DbType.String, 1500),
         new Column("IS_PROOF", DbType.Boolean, ColumnProperty.NotNull, false));
        }
        public override void Down()
        {
            Database.RemoveTable("GJI_DICT_MATERIAL_TYPE");
            Database.RemoveTable("GJI_COMISSION_MEETING_DOCUMENT");
        }
    }
}
