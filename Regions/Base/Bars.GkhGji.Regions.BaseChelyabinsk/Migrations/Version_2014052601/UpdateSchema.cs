﻿namespace Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2014052601
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014052601")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2014052600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            this.Database.AddTable(
                "GJI_NSO_PROTOCOL",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("FORMAT_DATE", DbType.DateTime),
                new Column("NOTIF_NUM", DbType.String, 100),
                new Column("PROCEEDINGS_PLACE", DbType.String, 1000),
                new Column("REMARKS", DbType.String, 1000));

            this.Database.ExecuteNonQuery(@"insert into GJI_NSO_PROTOCOL (id)
                                     select id from GJI_PROTOCOL");
        }

        public override void Down()
        {
            this.Database.RemoveTable("GJI_NSO_PROTOCOL");
        }
    }
}