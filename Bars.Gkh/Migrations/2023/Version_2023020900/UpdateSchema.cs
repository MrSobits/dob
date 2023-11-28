namespace Bars.Gkh.Migrations._2023.Version_2023020900
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Utils;

    [Migration("2023020900")]
    
    [MigrationDependsOn(typeof(Version_2023020700.UpdateSchema))]


    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_DICT_MUNICIPALITY", new Column("BIK", DbType.String));
            Database.AddColumn("GKH_DICT_MUNICIPALITY", new Column("INN", DbType.String));
            Database.AddColumn("GKH_DICT_MUNICIPALITY", new Column("KPP", DbType.String));
            Database.AddColumn("GKH_DICT_MUNICIPALITY", new Column("UFC", DbType.String));
            Database.AddColumn("GKH_DICT_MUNICIPALITY", new Column("UIN", DbType.String));
            Database.AddColumn("GKH_DICT_MUNICIPALITY", new Column("BANKNAME", DbType.String));
            Database.AddColumn("GKH_DICT_MUNICIPALITY", new Column("PERSACC", DbType.String));
            Database.AddColumn("GKH_DICT_MUNICIPALITY", new Column("CORRACC", DbType.String));
            Database.AddColumn("GKH_DICT_MUNICIPALITY", new Column("INDEX", DbType.String));
        }

        /// <summary>
        /// Откатить
        /// </summary>
        public override void Down()
        {       
            Database.RemoveColumn("GKH_DICT_MUNICIPALITY", "BIK");
            Database.RemoveColumn("GKH_DICT_MUNICIPALITY", "INN");
            Database.RemoveColumn("GKH_DICT_MUNICIPALITY", "KPP");
            Database.RemoveColumn("GKH_DICT_MUNICIPALITY", "UFC");
            Database.RemoveColumn("GKH_DICT_MUNICIPALITY", "UIN");
            Database.RemoveColumn("GKH_DICT_MUNICIPALITY", "BANKNAME");
            Database.RemoveColumn("GKH_DICT_MUNICIPALITY", "PERSACC");
            Database.RemoveColumn("GKH_DICT_MUNICIPALITY", "CORRACC");
            Database.RemoveColumn("GKH_DICT_MUNICIPALITY", "INDEX");
        }
    }
}