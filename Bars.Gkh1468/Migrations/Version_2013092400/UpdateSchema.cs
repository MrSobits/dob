﻿namespace Bars.Gkh1468.Migrations.Version_2013092400
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013092400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh1468.Migrations.Version_2013091300.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_HOUSE_PROV_PASS_ROW", new Column("GROUP_KEY", DbType.Int32, ColumnProperty.Null));
            Database.AddColumn("GKH_OKI_PROV_PASSPORT_ROW", new Column("GROUP_KEY", DbType.Int32, ColumnProperty.Null));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_HOUSE_PROV_PASS_ROW", "GROUP_KEY");
            Database.RemoveColumn("GKH_OKI_PROV_PASSPORT_ROW", "GROUP_KEY");
        }
    }
}