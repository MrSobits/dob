namespace Bars.GkhGji.Migrations._2022.Version_2023011900
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Enums;

    [Migration("2023011900")]
    [MigrationDependsOn(typeof(Version_2022112300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {
            Database.AddColumn("GJI_RESOLUTION", new Column("SENT_TO_OSP", DbType.Boolean, false));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_RESOLUTION", "SENT_TO_OSP");
        }
    }
}
