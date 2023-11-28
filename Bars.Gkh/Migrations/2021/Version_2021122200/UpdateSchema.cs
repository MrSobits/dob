namespace Bars.Gkh.Migrations._2021.Version_2021122200
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2021122200")]
    
    [MigrationDependsOn(typeof(Version_2021121900.UpdateSchema))]


    public class UpdateSchema : Migration
    {
        public override void Up()
        {

            Database.AddEntityTable(
            "GKH_DICT_VEHICLE_INFORMATION_OWNER",
            new Column("FIO_OWNER_TRANSPORT", DbType.String, 36, ColumnProperty.NotNull),
            new Column("PLACE_RESIDENCE", DbType.String, 36, ColumnProperty.NotNull),
            new Column("PASSPORT_ID", DbType.Int32, 4, ColumnProperty.NotNull),
            new Column("PASSPORT_SERIES", DbType.String, 6, ColumnProperty.NotNull),
            new Column("PASSPORT_ISSUED", DbType.String, 100, ColumnProperty.NotNull),
            new Column("DEPARTMENT_CODE", DbType.Int32, 36, ColumnProperty.NotNull),
            new Column("DATE_ISSUE", DbType.DateTime, ColumnProperty.NotNull));

            Database.AddEntityTable(
             "GKH_DICT_VEHICLE_INFORMATION",
             new Column("NAME_TRANSPORT", DbType.String, 36),
             new Column("NAMBER_TRANSPORT", DbType.String, 36, ColumnProperty.NotNull),
             new Column("REGISTRATION_NAMBER_TRANSPORT", DbType.Int32,36),
             new Column("SERIES_TRANSPORT", DbType.String, 36),
             new Column("REG_NAMBERTRANSPORT", DbType.Int32, 36),
             new RefColumn("OWNER_TRANSPORT_ID", ColumnProperty.None, "GKH_DICT_VEHICLE_INFORMATION_TO_OWNER", "GKH_DICT_VEHICLE_INFORMATION_OWNER","ID"));   
        }

        /// <summary>
        /// Откатить
        /// </summary>
        public override void Down()
        {

            
            Database.RemoveTable("GKH_DICT_VEHICLE_INFORMATION");
            Database.RemoveTable("GKH_DICT_VEHICLE_INFORMATION_OWNER");
        }
    }
}