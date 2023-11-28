namespace Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2022021001
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;
  
    [Migration("2022021001")]
    [MigrationDependsOn(typeof(Version_2022021000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {  
            Database.AddRefColumn("GJI_PROTOCOL197", new RefColumn("INDIVIDUAL_PERSON_ID", "GJI_PROTOCOL197_REMOVE_GKH_INDIVIDUAL_PERSON", "GKH_INDIVIDUAL_PERSON", "ID"));
        }
                       
        public override void Down()
        {
            Database.RemoveColumn("GJI_PROTOCOL197", "INDIVIDUAL_PERSON_ID");
        }

    }
}