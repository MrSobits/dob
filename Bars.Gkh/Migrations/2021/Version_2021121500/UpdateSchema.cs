namespace Bars.Gkh.Migrations._2021.Version_2021121500
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2021121500")]
    
    [MigrationDependsOn(typeof(Version_2021092000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_DICT_INSPECTOR", new Column("TYPE_COMMISSION_MEMBER", DbType.Int16, ColumnProperty.NotNull, (int)Enums.TypeCommissionМember.Chairman));      
        }

        /// <summary>
        /// Откатить
        /// </summary>
        public override void Down()
        {
            Database.RemoveColumn("GKH_DICT_INSPECTOR", "TYPE_COMMISSION_MEMBER");
        }
    }
}