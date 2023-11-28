namespace Bars.GkhGji.Migrations._2023.Version_2023031300
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2023031300")]
    [MigrationDependsOn(typeof(Version_2023020700.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {
            Database.AddColumn("ZONAL_INSPECTION_PREFIX", new Column("UIN_PREFIX", DbType.String, 255));
        }

        public override void Down()
        {
            Database.RemoveColumn("ZONAL_INSPECTION_PREFIX", "UIN_PREFIX");
        }
    }
}
