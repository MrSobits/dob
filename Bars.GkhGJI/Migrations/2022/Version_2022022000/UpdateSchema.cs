namespace Bars.GkhGji.Migrations._2022.Version_2022022000
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Enums;

    [Migration("2022022000")]
    [MigrationDependsOn(typeof(Version_2022021700.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_DISPOSAL", new Column("TO_ATTRACTED", DbType.Boolean, ColumnProperty.NotNull, false));
        }
        public override void Down()
        {
            Database.RemoveColumn("GJI_DISPOSAL", "TO_ATTRACTED");    
        }
    }
}