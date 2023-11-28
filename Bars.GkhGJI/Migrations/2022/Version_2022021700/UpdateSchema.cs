namespace Bars.GkhGji.Migrations._2022.Version_2022021700
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Enums;

    [Migration("2022021700")]
    [MigrationDependsOn(typeof(Version_2022021600.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_DOCUMENT", new RefColumn("COMISSION_ID", "GJI_DOCUMENT_COMISSION_MEETING", "GJI_COMISSION_MEETING", "ID"));
        }
        public override void Down()
        {
            Database.RemoveColumn("GJI_DOCUMENT", "COMISSION_ID");    
        }
    }
}