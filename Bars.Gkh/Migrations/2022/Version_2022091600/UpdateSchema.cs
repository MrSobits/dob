namespace Bars.Gkh.Migrations._2022.Version_2022091600
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Utils;

    [Migration("2022091600")]
    
    [MigrationDependsOn(typeof(Version_2022091500.UpdateSchema))]


    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddRefColumn("GKH_DICT_INSPECTOR", new RefColumn("POSITION_ID", "GKH_DICT_INSPECTOR_POSITION_ID", "GKH_DICT_POSITION", "ID"));
        }

        /// <summary>
        /// Откатить
        /// </summary>
        public override void Down()
        {       
            Database.RemoveColumn("GKH_DICT_INSPECTOR", "POSITION_ID");
        }
    }
}