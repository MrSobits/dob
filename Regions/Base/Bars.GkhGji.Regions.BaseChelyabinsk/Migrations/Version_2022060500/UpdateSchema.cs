namespace Bars.GkhGji.Migrations._2022.Version_2022060500
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh;

    [Migration("2022060500")]
    [MigrationDependsOn(typeof(Version_2022051000.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddRefColumn("GJI_PROTOCOL197", new RefColumn("TRANSPORT_ID", "GJI_PROTOCOL197_TRANSPORT", "GJI_OWNER", "ID"));
        }

        /// <summary>
        /// Откатить
        /// </summary>
        public override void Down()
        {
            Database.RemoveColumn("GJI_PROTOCOL197", "TRANSPORT_ID");
        }
    }
}