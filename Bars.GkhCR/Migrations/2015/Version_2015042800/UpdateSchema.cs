﻿namespace Bars.GkhCr.Migration.Version_2015042800
{
    using Bars.Gkh;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015042800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhCr.Migration.Version_2015042000.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //не удалять миграцию
            /*ViewManager.Drop(Database, "GkhCr");
            ViewManager.Create(Database, "GkhCr");*/
        }

        public override void Down()
        {
        }
    }
}