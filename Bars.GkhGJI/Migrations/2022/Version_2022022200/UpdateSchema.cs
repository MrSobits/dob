namespace Bars.GkhGji.Migrations._2022.Version_2022022200
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Enums;

    [Migration("2022022200")]
    [MigrationDependsOn(typeof(Version_2022022000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_COMISSION_MEETING_INSPECTOR", new Column("TYPE_COMISSION_MEMBER", DbType.Int16, ColumnProperty.NotNull, (int)Bars.Gkh.Enums.TypeCommissionMember.Member));
        }
        public override void Down()
        {
            Database.RemoveColumn("GJI_COMISSION_MEETING_INSPECTOR", "TYPE_COMISSION_MEMBER");    
        }
    }
}