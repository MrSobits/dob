namespace Bars.GkhGji.Migrations._2022.Version_2022091600
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Enums;

    [Migration("2022091600")]
    [MigrationDependsOn(typeof(Version_2022091500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {
            this.Database.AddEntityTable("GJI_RESOLUTION_LTEXT",
               new RefColumn("RESOLUTION_ID", ColumnProperty.NotNull, "GJI_RESOLUTION_LTEXT_RES", "GJI_RESOLUTION", "ID"),
               new Column("DESCRIPTION", DbType.Binary));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_RESOLUTION_LTEXT");
        }
    }
}
