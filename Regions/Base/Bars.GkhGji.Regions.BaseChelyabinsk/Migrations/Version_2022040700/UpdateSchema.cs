namespace Bars.GkhGji.Migrations._2022.Version_2022040700
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh;

    [Migration("2022040700")]
    [MigrationDependsOn(typeof(Version_2022032100.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_PROTOCOL197", new Column("REPEATED", DbType.Boolean, ColumnProperty.NotNull, false));

            this.Database.AddEntityTable(
                "GJI_PROTOCOL197_REPRESOLUTION",
                new RefColumn("PROTOCOL_ID", "GJI_PROT197_REPRESOLUTION_DOC", "GJI_PROTOCOL197", "ID"),
                new RefColumn("ARTICLELAW_ID", "GJI_PROT197_REPRESOLUTION_ARL", "GJI_DICT_ARTICLELAW", "ID"),
                new RefColumn("DOC_ID", "GJI_PROTOCOL197_REPRESOLUTION_RESOL", "GJI_RESOLUTION", "ID"),
                new Column("DESCRIPTION", DbType.String, 500));

        }
        public override void Down()
        {
            this.Database.RemoveTable("GJI_PROTOCOL197_REPRESOLUTION");
            Database.RemoveColumn("GJI_PROTOCOL197", "REPEATED");
        }
    }
}