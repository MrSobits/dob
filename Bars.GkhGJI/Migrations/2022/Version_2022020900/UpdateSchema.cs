namespace Bars.GkhGji.Migrations._2022.Version_2022020900
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh;
    
    [Migration("2022020900")]
    [MigrationDependsOn(typeof(Version_2022020800.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("GJI_DICT_VIOLATION_MUNICIPALITY",
             new RefColumn("MUNICIPALITY_ID", ColumnProperty.None, "GJI_DICT_VIOLATION_MUNICIPALITY_REQUEST_MUNICIPALITY", "GKH_DICT_MUNICIPALITY", "ID"),
             new RefColumn("VIOLATION_ID", ColumnProperty.None, "GJI_DICT_VIOLATION_MUNICIPALITY_REQUEST_VIOLATION", "GJI_DICT_VIOLATION", "ID")
             );

        }
        public override void Down()
        {
            Database.RemoveTable("GJI_DICT_VIOLATION_MUNICIPALITY");
        }
    }
}