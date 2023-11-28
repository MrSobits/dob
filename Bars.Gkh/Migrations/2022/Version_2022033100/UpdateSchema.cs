namespace Bars.Gkh.Migrations._2022.Version_2022033100
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Utils;

    [Migration("2022033100")]
    
    [MigrationDependsOn(typeof(Version_2022032001.UpdateSchema))]


    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_DICT_ZONAINSP", new Column("UFC", DbType.String));
            Database.AddColumn("GKH_DICT_ZONAINSP", new Column("PERSACC", DbType.String));
            Database.AddColumn("GKH_DICT_ZONAINSP", new Column("BANKNAME", DbType.String));
            Database.AddColumn("GKH_DICT_ZONAINSP", new Column("CORRACC", DbType.String));
   
        }

        /// <summary>
        /// Откатить
        /// </summary>
        public override void Down()
        {
     
            Database.RemoveColumn("GKH_DICT_ZONAINSP", "CORRACC");
            Database.RemoveColumn("GKH_DICT_ZONAINSP", "BANKNAME");
            Database.RemoveColumn("GKH_DICT_ZONAINSP", "PERSACC");
            Database.RemoveColumn("GKH_DICT_ZONAINSP", "UFC");
        }
    }
}