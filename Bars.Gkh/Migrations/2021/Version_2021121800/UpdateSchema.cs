namespace Bars.Gkh.Migrations._2021.Version_2021121800
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2021121800")]
    
    [MigrationDependsOn(typeof(Version_2021110200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddRefColumn("GKH_CIT_SUG", new RefColumn("INSPECTOR_ID", "GKH_CIT_SUG_INSP_ID", "GKH_DICT_INSPECTOR", "ID"));
        }

        /// <summary>
        /// Откатить
        /// </summary>
        public override void Down()
        {
            Database.RemoveColumn("GKH_CIT_SUG", "INSPECTOR_ID");
        }
    }
}