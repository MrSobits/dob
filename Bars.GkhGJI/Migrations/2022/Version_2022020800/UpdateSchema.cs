namespace Bars.GkhGji.Migrations._2022.Version_2022020800
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh;

    [Migration("2022020800")]
    [MigrationDependsOn(typeof(Version_2022020700.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_DISPOSAL", new Column("NUMBER_KUSP", DbType.Int64));
            Database.AddRefColumn("GJI_DISPOSAL", new RefColumn("MUNICIPALITY_ID", ColumnProperty.None, "GJI_DISPOSAL_REQUEST_MUNICIPALITY", "GKH_DICT_MUNICIPALITY", "ID"));
            Database.AddRefColumn("GJI_DICT_VIOLATION", new RefColumn("MUNICIPALITY_ID", ColumnProperty.None, "GJI_DICT_VIOLATION_REQUEST_MUNICIPALITY", "GKH_DICT_MUNICIPALITY", "ID"));
            Database.AddRefColumn("GJI_RESOLUTION", new RefColumn("INDIVIDUAL_PERSON_ID", ColumnProperty.None, "GJI_DISPOSAL_REQUEST_INDIVIDUAL_PERSON", "GKH_INDIVIDUAL_PERSON", "ID"));

        }
        public override void Down()
        {
            Database.RemoveColumn("GJI_RESOLUTION", "INDIVIDUAL_PERSON_ID");
            Database.RemoveColumn("GJI_DICT_VIOLATION", "MUNICIPALITY_ID");
            Database.RemoveColumn("GJI_DISPOSAL", "MUNICIPALITY_ID");
            Database.RemoveColumn("GJI_DISPOSAL", "NUMBER_KUSP");
         
        }
    }
}