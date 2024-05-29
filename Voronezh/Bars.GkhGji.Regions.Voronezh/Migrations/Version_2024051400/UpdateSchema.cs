namespace Bars.GkhGji.Regions.Voronezh.Migrations.Version_2024051400
{
    using Bars.B4.Modules.Ecm7.Framework;
    using System.Data;

    [Migration("2024051400")]
    [MigrationDependsOn(typeof(Version_2024012900.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_VR_COURT_PRACTICE", new Column("CLOSURE_DATE", DbType.DateTime, ColumnProperty.None));
            Database.AddColumn("GJI_VR_COURT_PRACTICE", new Column("CD_REASON", DbType.Int32, 4, ColumnProperty.NotNull, 0));

        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_VR_COURT_PRACTICE", "CLOSURE_DATE");
            Database.RemoveColumn("GJI_VR_COURT_PRACTICE", "CD_REASON");
        }
    }
}


