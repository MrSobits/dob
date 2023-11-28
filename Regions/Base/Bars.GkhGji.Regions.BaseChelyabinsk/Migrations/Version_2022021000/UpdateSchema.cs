namespace Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2022021000
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;
  
    [Migration("2022021000")]
    [MigrationDependsOn(typeof(Version_2021120900.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_PROTOCOL197", new Column("INN", DbType.String));
            Database.AddColumn("GJI_PROTOCOL197", new Column("FIO", DbType.String));
            Database.AddColumn("GJI_PROTOCOL197", new Column("PLACE_RESIDENCE", DbType.String));
            Database.AddColumn("GJI_PROTOCOL197", new Column("ACTUALLY_RESIDENCE", DbType.String));
            Database.AddColumn("GJI_PROTOCOL197", new Column("BIRTH_PLACE", DbType.String));
            Database.AddColumn("GJI_PROTOCOL197", new Column("JOB", DbType.String));
            Database.AddColumn("GJI_PROTOCOL197", new Column("DATE_BIRTH", DbType.DateTime));
            Database.AddColumn("GJI_PROTOCOL197", new Column("PASSPORT_NUMBER", DbType.Int64));
            Database.AddColumn("GJI_PROTOCOL197", new Column("PASSPORT_SERIES", DbType.Int64));
            Database.AddColumn("GJI_PROTOCOL197", new Column("DEPARTMENT_CODE", DbType.Int64));
            Database.AddColumn("GJI_PROTOCOL197", new Column("DATE_ISSUE", DbType.DateTime));
            Database.AddColumn("GJI_PROTOCOL197", new Column("PASSPORT_ISSUED", DbType.String));
            Database.AddColumn("GJI_PROTOCOL197", new Column("FAMILY_STATUS", DbType.Int16, ColumnProperty.NotNull, (int)Bars.Gkh.Enums.FamilyStatus.Default));
        }
                       
        public override void Down()
        {
            Database.RemoveColumn("GJI_PROTOCOL197", "FAMILY_STATUS");
            Database.RemoveColumn("GJI_PROTOCOL197", "PASSPORT_ISSUED");
            Database.RemoveColumn("GJI_PROTOCOL197", "DATE_ISSUE");
            Database.RemoveColumn("GJI_PROTOCOL197", "DEPARTMENT_CODE");
            Database.RemoveColumn("GJI_PROTOCOL197", "PASSPORT_SERIES");
            Database.RemoveColumn("GJI_PROTOCOL197", "PASSPORT_NUMBER");
            Database.RemoveColumn("GJI_PROTOCOL197", "DATE_BIRTH");
            Database.RemoveColumn("GJI_PROTOCOL197", "JOB");
            Database.RemoveColumn("GJI_PROTOCOL197", "BIRTH_PLACE");
            Database.RemoveColumn("GJI_PROTOCOL197", "ACTUALLY_RESIDENCE");
            Database.RemoveColumn("GJI_PROTOCOL197", "PLACE_RESIDENCE");
            Database.RemoveColumn("GJI_PROTOCOL197", "FIO");
            Database.RemoveColumn("GJI_PROTOCOL197", "INN");
        }

    }
}