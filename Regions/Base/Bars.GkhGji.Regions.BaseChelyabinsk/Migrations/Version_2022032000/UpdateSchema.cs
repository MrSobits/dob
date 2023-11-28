namespace Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2022032000
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;
    using Bars.GkhGji.Enums;

    [Migration("2022032000")]
    [MigrationDependsOn(typeof(Version_2022021100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
        
            Database.AddRefColumn("GJI_PROTOCOL197", new RefColumn("FIAS_REG_ADDRESS", "GJI_PROTOCOL197_FIAS_REG_ADDRESS", "B4_FIAS_ADDRESS", "ID"));
            Database.AddRefColumn("GJI_PROTOCOL197", new RefColumn("FIAS_FACT_ADDRESS", "GJI_PROTOCOL197_FIAS_FACT_ADDRESS", "B4_FIAS_ADDRESS", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_PROTOCOL197", "FIAS_FACT_ADDRESS");
            Database.RemoveColumn("GJI_PROTOCOL197", "FIAS_REG_ADDRESS");
        }

    }
}