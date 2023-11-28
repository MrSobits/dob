namespace Bars.Gkh.Migrations._2023.Version_2023041200
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2023041200")]
    
    [MigrationDependsOn(typeof(Version_2023020900.UpdateSchema))]


    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_DICT_ZONAINSP", new Column("GISGMPID", DbType.String));
        }

        /// <summary>
        /// Откатить
        /// </summary>
        public override void Down()
        {       
            Database.RemoveColumn("GKH_DICT_ZONAINSP", "GISGMPID");
        }
    }
}