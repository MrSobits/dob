namespace Bars.GkhGji.Migrations._2022.Version_2022020901
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh;
    
    [Migration("2022020901")]
    [MigrationDependsOn(typeof(Version_2022020900.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_PROTOCOL", new Column("INN", DbType.String));
            Database.AddColumn("GJI_PROTOCOL", new Column("FIO", DbType.String));
            Database.AddColumn("GJI_PROTOCOL", new Column("PLACE_RESIDENCE", DbType.String));
            Database.AddColumn("GJI_PROTOCOL", new Column("ACTUALLY_RESIDENCE", DbType.String));
            Database.AddColumn("GJI_PROTOCOL", new Column("BIRTH_PLACE", DbType.String));
            Database.AddColumn("GJI_PROTOCOL", new Column("JOB", DbType.String));
            Database.AddColumn("GJI_PROTOCOL", new Column("DATE_BIRTH", DbType.DateTime));
            Database.AddColumn("GJI_PROTOCOL", new Column("PASSPORT_NUMBER", DbType.Int64));
            Database.AddColumn("GJI_PROTOCOL", new Column("PASSPORT_SERIES", DbType.Int64));
            Database.AddColumn("GJI_PROTOCOL", new Column("DEPARTMENT_CODE", DbType.Int64));
            Database.AddColumn("GJI_PROTOCOL", new Column("DATE_ISSUE", DbType.DateTime));
            Database.AddColumn("GJI_PROTOCOL", new Column("PASSPORT_ISSUED", DbType.String));
            Database.AddColumn("GJI_PROTOCOL", new Column("FAMILY_STATUS", DbType.Int16, ColumnProperty.NotNull, (int)Bars.Gkh.Enums.FamilyStatus.Default));

        }
        public override void Down()
        {
            Database.RemoveColumn("GJI_PROTOCOL", "FAMILY_STATUS");
            Database.RemoveColumn("GJI_PROTOCOL", "PASSPORT_ISSUED");
            Database.RemoveColumn("GJI_PROTOCOL", "DATE_ISSUE");
            Database.RemoveColumn("GJI_PROTOCOL", "DEPARTMENT_CODE");
            Database.RemoveColumn("GJI_PROTOCOL", "PASSPORT_SERIES");
            Database.RemoveColumn("GJI_PROTOCOL", "PASSPORT_NUMBER");
            Database.RemoveColumn("GJI_PROTOCOL", "DATE_BIRTH");
            Database.RemoveColumn("GJI_PROTOCOL", "JOB");
            Database.RemoveColumn("GJI_PROTOCOL", "BIRTH_PLACE");
            Database.RemoveColumn("GJI_PROTOCOL", "ACTUALLY_RESIDENCE");
            Database.RemoveColumn("GJI_PROTOCOL", "PLACE_RESIDENCE");
            Database.RemoveColumn("GJI_PROTOCOL", "FIO");
            Database.RemoveColumn("GJI_PROTOCOL", "INN");
        }
    }
}