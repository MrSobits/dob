namespace Bars.Gkh.Migrations._2022.Version_2022012600
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2022012600")]
    
    [MigrationDependsOn(typeof(Version_2022011300.UpdateSchema))]


    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_INDIVIDUAL_PERSON", new Column("INN", DbType.String));
        }
        /// <summary>
        /// Откатить
        /// </summary>
        public override void Down()
        {
            Database.RemoveColumn("GKH_INDIVIDUAL_PERSON", "INN");
        }
    }
}