namespace Bars.GkhGji.Migrations._2022.Version_2022021400
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh;

    [Migration("2022021400")]
    [MigrationDependsOn(typeof(Version_2022021100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_PROTOCOL", new Column("DATE_VIOLATION", DbType.DateTime));
        }
        public override void Down()
        {
            Database.RemoveColumn("GJI_PROTOCOL", "DATE_VIOLATION");    
        }
    }
}