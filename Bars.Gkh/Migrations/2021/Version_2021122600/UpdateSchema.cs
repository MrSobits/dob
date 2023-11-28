namespace Bars.Gkh.Migrations._2021.Version_2021122600
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2021122600")]
    
    [MigrationDependsOn(typeof(Version_2021122200.UpdateSchema))]


    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_DICT_ZONAINSP", new Column("BIK", DbType.String));
            Database.AddColumn("GKH_DICT_ZONAINSP", new Column("INN", DbType.String));
            Database.AddColumn("GKH_DICT_ZONAINSP", new Column("KBK", DbType.String));
            Database.AddColumn("GKH_DICT_ZONAINSP", new Column("KPP", DbType.String));
            Database.AddColumn("GKH_DICT_ZONAINSP", new Column("UIN", DbType.String));
        }

        /// <summary>
        /// Откатить
        /// </summary>
        public override void Down()
        {
            Database.RemoveColumn("GKH_DICT_ZONAINSP", "UIN");
            Database.RemoveColumn("GKH_DICT_ZONAINSP", "KPP");
            Database.RemoveColumn("GKH_DICT_ZONAINSP", "KBK");
            Database.RemoveColumn("GKH_DICT_ZONAINSP", "INN");
            Database.RemoveColumn("GKH_DICT_ZONAINSP", "BIK");
        }
    }
}