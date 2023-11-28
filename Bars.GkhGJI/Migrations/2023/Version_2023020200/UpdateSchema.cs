namespace Bars.GkhGji.Migrations._2022.Version_2023020200
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Enums;

    [Migration("2023020200")]
    [MigrationDependsOn(typeof(Version_2023020100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {
            Database.AddColumn("GJI_RESOLUTION_DEFINITION", new RefColumn("COMISSION_MEETING_ID", "GJI_RESOLUTION_DEFINITION_COMISSION_MEETING_ID", "GJI_COMISSION_MEETING", "ID"));
            Database.AddColumn("GJI_RESOLUTION_DEFINITION", new Column("CONCIDERATION_DATE", DbType.DateTime));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_RESOLUTION_DEFINITION", "COMISSION_MEETING_ID");
            Database.RemoveColumn("GJI_RESOLUTION_DEFINITION", "CONCIDERATION_DATE");
        }
    }
}
