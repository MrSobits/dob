﻿namespace Bars.GkhRf.Migrations.Version_2014031200
{
    using Gkh;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014031200")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhRf.Migrations.Version_2014030400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            ViewManager.Drop(Database, "GkhRf");
            ViewManager.Create(Database, "GkhRf");
        }

        public override void Down()
        {
            ViewManager.Drop(Database, "GkhRf");
        }
    }
}