namespace Bars.Gkh.Migrations._2020.Version_2020042400
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    
    [Migration("2020042400")]
    
    [MigrationDependsOn(typeof(Version_2020041600.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("CLW_LAWSUIT", new Column("DUTY_DEBT_APPROV", DbType.Decimal));

        }

        /// <summary>
        /// Откатить
        /// </summary>
        public override void Down()
        {
            Database.RemoveColumn("CLW_LAWSUIT", "DUTY_DEBT_APPROV");

        }
    }
}