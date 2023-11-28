namespace Bars.GkhGji.Migrations._2022.Version_2022110500
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Enums;

    [Migration("2022110500")]
    [MigrationDependsOn(typeof(Version_2022100200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {
            Database.AddRefColumn("GJI_DOCUMENT", new RefColumn("ZONAL_ID", "GJI_DOCUMENT_ZONAL_ID", "GKH_DICT_ZONAINSP", "ID"));
            this.Database.ExecuteQuery($"UPDATE GJI_DOCUMENT SET ZONAL_ID = (select id from GKH_DICT_ZONAINSP limit 1);");
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_DOCUMENT", "ZONAL_ID");
        }
    }
}
