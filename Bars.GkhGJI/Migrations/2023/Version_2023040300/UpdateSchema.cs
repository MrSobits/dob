namespace Bars.GkhGji.Migrations._2022.Version_2023040300
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Enums;

    [Migration("2023040300")]
    [MigrationDependsOn(typeof(Version_2023021700.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {
            Database.AddColumn("GJI_DICT_PROD_CALENDAR", new Column("WORK_DAY", DbType.Boolean, ColumnProperty.None, false));
           
        }

        public override void Down()
        {
           // Database.RemoveColumn("GJI_RESOLUTION", "SEND_DATE");
            Database.RemoveColumn("GJI_DICT_PROD_CALENDAR", "WORK_DAY");

        }
    }
}
