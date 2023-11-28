namespace Bars.GkhGji.Migrations._2022.Version_2023021700
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Enums;

    [Migration("2023021700")]
    [MigrationDependsOn(typeof(Version_2023020900.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {
            Database.AddColumn("GJI_RESOLUTION", new Column("SEND_DATE", DbType.DateTime, ColumnProperty.None));
            Database.AddColumn("GJI_RESOLUTION", new Column("POST_DELIVERY_DATE", DbType.DateTime, ColumnProperty.None));
        }

        public override void Down()
        {
           // Database.RemoveColumn("GJI_RESOLUTION", "SEND_DATE");
            Database.RemoveColumn("GJI_RESOLUTION", "POST_DELIVERY_DATE");

        }
    }
}
