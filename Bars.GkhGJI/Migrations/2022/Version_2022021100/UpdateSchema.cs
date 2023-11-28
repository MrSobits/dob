namespace Bars.GkhGji.Migrations._2022.Version_2022021100
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh;
  
    [Migration("2022021100")]
    [MigrationDependsOn(typeof(Version_2022021000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddRefColumn("GJI_PROTOCOL", new RefColumn("INDIVIDUAL_PERSON_ID", "GJI_PROTOCOL_REMOV_GKH_INDIVIDUAL_PERSON", "GKH_INDIVIDUAL_PERSON", "ID"));
         
        }
        public override void Down()
        {
            Database.RemoveColumn("GJI_PROTOCOL", "INDIVIDUAL_PERSON_ID");    
        }
    }
}