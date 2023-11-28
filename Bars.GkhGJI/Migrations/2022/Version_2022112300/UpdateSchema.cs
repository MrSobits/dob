namespace Bars.GkhGji.Migrations._2022.Version_2022112300
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Enums;

    [Migration("2022112300")]
    [MigrationDependsOn(typeof(Version_2022110500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {
            this.Database.AddEntityTable("GJI_COMISSION_MEETING_DOCUMENT_LTEXT",
               new RefColumn("DOC_ID", ColumnProperty.NotNull, "GJI_CMD_LTEXT_DOC", "GJI_COMISSION_MEETING_DOCUMENT", "ID"),
               new Column("DESCRIPTION", DbType.Binary));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_COMISSION_MEETING_DOCUMENT_LTEXT");
        }
    }
}
