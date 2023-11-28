﻿namespace Bars.Gkh.Overhaul.Nso.Migration.Version_2013102801
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013102801")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Nso.Migration.Version_2013102800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.RemoveTable("OVRHL_DICT_COMMON_PARAM");
        }

        public override void Down()
        {
            Database.AddEntityTable(
                "OVRHL_DICT_COMMON_PARAM",
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("COMMON_PARAM_TYPE", DbType.Int16, ColumnProperty.NotNull));
        }
    }
}