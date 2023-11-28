namespace Bars.Gkh.Migrations._2022.Version_2022051000
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Utils;

    [Migration("2022051000")]
    
    [MigrationDependsOn(typeof(Version_2022033100.UpdateSchema))]


    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_INDIVIDUAL_PERSON", new Column("DEPENDENTS_NUMBER", DbType.Int16, ColumnProperty.None));
            Database.AddRefColumn("GKH_INDIVIDUAL_PERSON", new RefColumn("SOCIAL_STATE", "GKH_INDIVIDUAL_PERSON_SOCIAL_STATE", "GJI_DICT_SOC_ST", "ID"));
        }

        /// <summary>
        /// Откатить
        /// </summary>
        public override void Down()
        {     
            Database.RemoveColumn("GKH_INDIVIDUAL_PERSON", "SOCIAL_STATE");
            Database.RemoveColumn("GKH_INDIVIDUAL_PERSON", "DEPENDENTS_NUMBER");
        }
    }
}