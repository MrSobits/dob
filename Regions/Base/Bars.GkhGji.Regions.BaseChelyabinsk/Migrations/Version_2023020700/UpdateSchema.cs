namespace Bars.GkhGji.Migrations._2023.Version_2023020700
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2023020700")]
    [MigrationDependsOn(typeof(Version_2023020500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {
            Database.AddColumn("GJI_PROTOCOL197", new Column("PHONE_NUMBER", DbType.String, 20));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_PROTOCOL197", "PHONE_NUMBER");
        }
    }
}
