﻿namespace Bars.Gkh.Ris.Migrations.Version_2016100300
{
    using System.Data;
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Миграция
    /// </summary>
    [Migration("2016100300")]
    [MigrationDependsOn(typeof(Version_2016082400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Применить миграцию
        /// </summary>
        public override void Up()
        {
            //this.Database.AddRisTable("RIS_CAPITAL_REPAIR_CHARGE",
            //new RefColumn("PAYMENT_DOC_ID", "RIS_CAPITAL_REPAIR_CHARGE_PAYMENT_DOC_ID", "RIS_PAYMENT_DOC", "ID"),
            //new Column("CONTRIBUTION", DbType.Decimal),
            //new Column("ACCOUNTING_PERIOD_TOTAL", DbType.Decimal),
            //new Column("MONEY_RECALCULATION", DbType.Decimal),
            //new Column("MONEY_DISCOUNT", DbType.Decimal),
            //new Column("TOTAL_PAYABLE", DbType.Decimal));

            //if (!this.Database.TableExists("RIS_CAPITAL_REPAIR_DEBT"))
            //{
            //    this.Database.AddRisTable(
            //        "RIS_CAPITAL_REPAIR_DEBT",
            //        new RefColumn("PAYMENT_DOC_ID", "RIS_CAPITAL_REPAIR_DEBT_PAYMENT_DOC_ID", "RIS_PAYMENT_DOC", "ID"),
            //        new Column("MONTH", DbType.Decimal),
            //        new Column("YEAR", DbType.Decimal),
            //        new Column("TOTAL_PAYABLE", DbType.Decimal));
            //}
        }

        /// <summary>
        /// Откатить миграцию
        /// </summary>
        public override void Down()
        {
            //this.Database.RemoveTable("RIS_CAPITAL_REPAIR_CHARGE");
            //this.Database.RemoveTable("RIS_CAPITAL_REPAIR_DEBT");
        }
    }
}