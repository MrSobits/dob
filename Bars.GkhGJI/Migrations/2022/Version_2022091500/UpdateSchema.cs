namespace Bars.GkhGji.Migrations._2022.Version_2022091500
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Enums;

    [Migration("2022091500")]
    [MigrationDependsOn(typeof(Version_2022060900.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_PROTOCOL", new Column("PLACE_RESIDENCE_OUTSTATE", DbType.String, 500));
            Database.AddColumn("GJI_PROTOCOL", new Column("ACTUALLY_RESIDENCE_OUTSTATE", DbType.String, 500));
            Database.AddColumn("GJI_PROTOCOL", new Column("IS_PLACE_RESIDENCE_OUTSTATE", DbType.Boolean, ColumnProperty.NotNull, false));
            Database.AddColumn("GJI_PROTOCOL", new Column("IS_ACTUALLY_RESIDENCE_OUTSTATE", DbType.Boolean, ColumnProperty.NotNull, false));
        }

        /// <summary>
        /// Откатить
        /// </summary>
        public override void Down()
        {
            Database.RemoveColumn("GJI_PROTOCOL", "IS_ACTUALLY_RESIDENCE_OUTSTATE");
            Database.RemoveColumn("GJI_PROTOCOL", "IS_PLACE_RESIDENCE_OUTSTATE");
            Database.RemoveColumn("GJI_PROTOCOL", "ACTUALLY_RESIDENCE_OUTSTATE");
            Database.RemoveColumn("GJI_PROTOCOL", "PLACE_RESIDENCE_OUTSTATE");
        }
    }
}
