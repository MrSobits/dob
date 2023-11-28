namespace Bars.GkhGji.Migrations._2022.Version_2022100200
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh;

    [Migration("2022100200")]
    [MigrationDependsOn(typeof(Version_2022091500.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_PROTOCOL197_LTEXT", new Column("VIOLATION", DbType.Binary));          
        }

        /// <summary>
        /// Откатить
        /// </summary>
        public override void Down()
        {
            Database.RemoveColumn("GJI_PROTOCOL197_LTEXT", "VIOLATION");
        }
    }
}