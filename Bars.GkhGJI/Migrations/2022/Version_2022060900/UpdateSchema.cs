namespace Bars.GkhGji.Migrations._2022.Version_2022060900
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Enums;

    [Migration("2022060900")]
    [MigrationDependsOn(typeof(Version_2022060500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_OWNER", new Column("NAME_TRANSPORT", DbType.String));
            Database.AddColumn("GJI_OWNER", new Column("NAMBER_TRANSPORT", DbType.String));
        }
        public override void Down()
        {
            Database.RemoveColumn("GJI_OWNER", "NAME_TRANSPORT");
            Database.RemoveColumn("GJI_OWNER", "NAMBER_TRANSPORT");
        }
    }
}
