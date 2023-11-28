namespace Bars.Gkh.Migrations._2022.Version_2022032001
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Utils;

    [Migration("2022032001")]
    
    [MigrationDependsOn(typeof(Version_2022032000.UpdateSchema))]


    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddRefColumn("GKH_INDIVIDUAL_PERSON", new RefColumn("FIAS_REG_ADDRESS", "GKH_INDIVIDUAL_PERSON_FIAS_REG_ADDRESS", "B4_FIAS_ADDRESS", "ID"));
            Database.AddRefColumn("GKH_INDIVIDUAL_PERSON", new RefColumn("FIAS_FACT_ADDRESS", "GKH_INDIVIDUAL_PERSON_FIAS_FACT_ADDRESS", "B4_FIAS_ADDRESS", "ID"));
        }
        /// <summary>
        /// Откатить
        /// </summary>
        public override void Down()
        {
            Database.RemoveColumn("GKH_INDIVIDUAL_PERSON", "FIAS_FACT_ADDRESS");
            Database.RemoveColumn("GKH_INDIVIDUAL_PERSON", "FIAS_REG_ADDRESS");
        }
    }
}