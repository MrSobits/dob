namespace Bars.Gkh.Migrations._2023.Version_2023020700
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Utils;

    [Migration("2023020700")]
    
    [MigrationDependsOn(typeof(_2022.Version_2022091600.UpdateSchema))]


    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_INDIVIDUAL_PERSON", new Column("PHONE_NUMBER", DbType.String, 20));
        }

        /// <summary>
        /// Откатить
        /// </summary>
        public override void Down()
        {       
            Database.RemoveColumn("GKH_INDIVIDUAL_PERSON", "PHONE_NUMBER");
        }
    }
}