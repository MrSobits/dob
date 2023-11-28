namespace Bars.GkhGji.Migrations._2022.Version_2022032100
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh;

    [Migration("2022032100")]
    [MigrationDependsOn(typeof(Version_2022032000.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_PROTOCOL197_ANNEX", new Column("IS_PROOF", DbType.Boolean, ColumnProperty.NotNull, false));
        }
        public override void Down()
        {
            Database.RemoveColumn("GJI_PROTOCOL197_ANNEX", "IS_PROOF");
        }
    }
}