﻿namespace Bars.GkhGji.Regions.Tula.Migrations.Version_2014082800
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014082800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Tula.Migrations.Version_2014080601.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            // Определение протокола для Тулы как subclass
            Database.AddTable(
                "TULA_GJI_PROTOCOLDEF",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("TIME_END", DbType.DateTime),
                new Column("TIME_START", DbType.DateTime),
                new Column("FILE_DESC_ID", DbType.Int64, 22));
            Database.AddIndex("IND_TULA_GJI_PROTOCOLDEF_ID", false, "TULA_GJI_PROTOCOLDEF", "ID");
            Database.AddForeignKey("FK_TULA_GJI_PROTOCOLDEF_ID", "TULA_GJI_PROTOCOLDEF", "ID", "GJI_PROTOCOL_DEFINITION", "ID");

            Database.AddIndex("IND_TULA_GJI_PROTOCOLDEF_FD", false, "TULA_GJI_PROTOCOLDEF", "FILE_DESC_ID");
            Database.AddForeignKey("FK_TULA_GJI_PROTOCOLDEF_FD", "TULA_GJI_PROTOCOLDEF", "FILE_DESC_ID", "B4_FILE_INFO", "ID");

            // делаем sql-скрипт чтобы сразу создать в новой таблицы записи для тех которые уже имеются в БД 
            Database.ExecuteNonQuery(@"insert into TULA_GJI_PROTOCOLDEF (id)
                                     select id from GJI_PROTOCOL_DEFINITION");

            // Определение постановления для Тулы как subclass
            Database.AddTable(
                "TULA_GJI_RESOLUTIONDEF",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("TIME_END", DbType.DateTime),
                new Column("TIME_START", DbType.DateTime),
                new Column("FILE_DESC_ID", DbType.Int64, 22));
            Database.AddIndex("IND_TULA_GJI_RESOLUTIONDEF_ID", false, "TULA_GJI_RESOLUTIONDEF", "ID");
            Database.AddForeignKey("FK_TULA_GJI_RESOLUTIONDEF_ID", "TULA_GJI_RESOLUTIONDEF", "ID", "GJI_RESOLUTION_DEFINITION", "ID");

            Database.AddIndex("IND_TULA_GJI_RESOLUTIONDEF_FD", false, "TULA_GJI_RESOLUTIONDEF", "FILE_DESC_ID");
            Database.AddForeignKey("FK_TULA_GJI_RESOLUTIONDEF_FD", "TULA_GJI_RESOLUTIONDEF", "FILE_DESC_ID", "B4_FILE_INFO", "ID");

            // делаем sql-скрипт чтобы сразу создать в новой таблицы записи для тех которые уже имеются в БД 
            Database.ExecuteNonQuery(@"insert into TULA_GJI_RESOLUTIONDEF (id)
                                     select id from GJI_RESOLUTION_DEFINITION");
        }

        public override void Down()
        {
            Database.RemoveConstraint("TULA_GJI_PROTOCOLDEF", "FK_TULA_GJI_PROTOCOLDEF_ID");
            Database.RemoveConstraint("TULA_GJI_PROTOCOLDEF", "FK_TULA_GJI_PROTOCOLDEF_FD");
            Database.RemoveTable("TULA_GJI_PROTOCOLDEF");

            Database.RemoveConstraint("TULA_GJI_RESOLUTIONDEF", "FK_TULA_GJI_RESOLUTIONDEF_ID");
            Database.RemoveConstraint("TULA_GJI_RESOLUTIONDEF", "FK_TULA_GJI_RESOLUTIONDEF_FD");
            Database.RemoveTable("TULA_GJI_RESOLUTIONDEF");
        }
    }
}