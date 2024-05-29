namespace Bars.GkhGji.Migrations._2024.Version_2024051600
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2024051600")]
    [MigrationDependsOn(typeof(_2022.Version_2024010900.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {
            Database.AddColumn("GJI_RESOLUTION", new Column("DISCHARGED_BY_COURT", DbType.Boolean, false));
            Database.AddColumn("GJI_RESOLUTION", new Column("SENT_TO_NEW_CONCEDERATION", DbType.Boolean, false));
            Database.AddColumn("GJI_RESOLUTION", new Column("CHANGED_BY_COURT", DbType.Boolean, false));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_RESOLUTION", "DISCHARGED_BY_COURT");
            Database.RemoveColumn("GJI_RESOLUTION", "SENT_TO_NEW_CONCEDERATION");
            Database.RemoveColumn("GJI_RESOLUTION", "CHANGED_BY_COURT");
        }
    }
}
