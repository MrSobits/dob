namespace Bars.Gkh.Migrations._2023.Version_2023041800
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2023041800")]
    
    [MigrationDependsOn(typeof(Version_2023041200.UpdateSchema))]


    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_DICT_ZONAINSP", new Column("OGRN", DbType.String));
        }

        /// <summary>
        /// Откатить
        /// </summary>
        public override void Down()
        {       
            Database.RemoveColumn("GKH_DICT_ZONAINSP", "OGRN");
        }
    }
}