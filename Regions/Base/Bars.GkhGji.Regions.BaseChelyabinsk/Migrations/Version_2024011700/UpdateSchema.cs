namespace Bars.GkhGji.Migrations._2024.Version_2024011700
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2024011700")]
    [MigrationDependsOn(typeof(_2023.Version_2023031300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {
            Database.AddColumn("ZONAL_INSPECTION_PREFIX", new Column("NUMERATION_PREFIX", DbType.String, 255));
        }

        public override void Down()
        {
            Database.RemoveColumn("ZONAL_INSPECTION_PREFIX", "NUMERATION_PREFIX");
        }
    }
}
