namespace Bars.GkhGji.Migrations._2022.Version_2022100200
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Enums;

    [Migration("2022100200")]
    [MigrationDependsOn(typeof(Version_2022091600.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {
            Database.AddColumn("GJI_RESOLUTION", new Column("PENALTY_BY_COURT", DbType.Decimal));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_RESOLUTION", "PENALTY_BY_COURT");
        }
    }
}
