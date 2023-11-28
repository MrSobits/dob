﻿namespace Bars.GkhDi.Migrations.Version_2015090700
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015090700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhDi.Migrations.Version_2015082500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        private static readonly string[] TableNames =
            {
                "DI_ADDITION_SERVICE",
                "DI_CAP_REP_SERVICE",
                "DI_COMMUNAL_SERVICE",
                "DI_CONTROL_SERVICE",
                "DI_HOUSING_SERVICE",
                "DI_REPAIR_SERVICE"
            };

        public override void Up()
        {
            foreach (var tableName in TableNames)
            {
                DropColumns(tableName);
            }
        }

        public override void Down()
        {
            foreach (var tableName in TableNames)
            {
                CreateColumns(tableName);
            }
        }

        private void CreateColumns(string tableName)
        {
            Database.AddColumn(tableName, new Column("OBJECT_VERSION", DbType.Int64, ColumnProperty.NotNull));
            Database.AddColumn(tableName, new Column("OBJECT_CREATE_DATE", DbType.DateTime, ColumnProperty.NotNull));
            Database.AddColumn(tableName, new Column("OBJECT_EDIT_DATE", DbType.DateTime, ColumnProperty.NotNull));
        }

        private void DropColumns(string tableName)
        {
            Database.RemoveColumn(tableName, "OBJECT_VERSION");
            Database.RemoveColumn(tableName, "OBJECT_CREATE_DATE");
            Database.RemoveColumn(tableName, "OBJECT_EDIT_DATE");
        }
    }
}