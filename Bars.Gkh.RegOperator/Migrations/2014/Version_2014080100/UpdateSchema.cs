﻿namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014080100
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014080100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014073099.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.ChangeColumn("REGOP_CASHPAYMENT_CENTER", new Column("IDENTIFIER", DbType.String, 250));
        }

        public override void Down()
        {
        }
    }
}
