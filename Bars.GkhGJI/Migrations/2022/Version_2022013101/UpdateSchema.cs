namespace Bars.GkhGji.Migrations._2022.Version_2022013101
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh;

    [Migration("2022013101")]
    [MigrationDependsOn(typeof(Version_2022013100.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_DISPOSAL", new Column("INN", DbType.String));
            Database.AddColumn("GJI_DISPOSAL", new Column("FIO", DbType.String));
            Database.AddColumn("GJI_DISPOSAL", new Column("PLACE_RESIDENCE", DbType.String));
            Database.AddColumn("GJI_DISPOSAL", new Column("ACTUALLY_RESIDENCE", DbType.String));
            Database.AddColumn("GJI_DISPOSAL", new Column("BIRTH_PLACE", DbType.String));
            Database.AddColumn("GJI_DISPOSAL", new Column("JOB", DbType.String));
            Database.AddColumn("GJI_DISPOSAL", new Column("DATE_BIRTH", DbType.DateTime));
            Database.AddColumn("GJI_DISPOSAL", new Column("PASSPORT_NUMBER", DbType.Int64));
            Database.AddColumn("GJI_DISPOSAL", new Column("PASSPORT_SERIES", DbType.Int64));
            Database.AddColumn("GJI_DISPOSAL", new Column("DEPARTMENT_CODE", DbType.Int64));
            Database.AddColumn("GJI_DISPOSAL", new Column("DATE_ISSUE", DbType.DateTime));
        }
        public override void Down()
        {
            Database.RemoveColumn("GJI_DISPOSAL","DATE_ISSUE");
            Database.RemoveColumn("GJI_DISPOSAL","DEPARTMENT_CODE");
            Database.RemoveColumn("GJI_DISPOSAL","PASSPORT_SERIES");
            Database.RemoveColumn("GJI_DISPOSAL","PASSPORT_NUMBER");
            Database.RemoveColumn("GJI_DISPOSAL","DATE_BIRTH");
            Database.RemoveColumn("GJI_DISPOSAL","JOB");
            Database.RemoveColumn("GJI_DISPOSAL","BIRTH_PLACE");
            Database.RemoveColumn("GJI_DISPOSAL","ACTUALLY_RESIDENCE");
            Database.RemoveColumn("GJI_DISPOSAL","PLACE_RESIDENCE");
            Database.RemoveColumn("GJI_DISPOSAL", "FIO");
            Database.RemoveColumn("GJI_DISPOSAL", "INN");
        }
    }
}