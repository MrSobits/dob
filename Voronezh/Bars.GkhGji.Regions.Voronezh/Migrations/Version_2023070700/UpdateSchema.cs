namespace Bars.GkhGji.Regions.Voronezh.Migrations.Version_2023070700
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2023070700")]
    [MigrationDependsOn(typeof(Version_2023060800.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddEntityTable
            (
              "GJI_VR_ASFK_BDOPER",
              new RefColumn("ASFK_ID", ColumnProperty.None, "GJI_VR_BDOPER_ASFK_ID", "GJI_VR_ASFK", "ID"),
              new Column("IS_PAYFINE_ADDED", DbType.Boolean, false),
              new Column("GUID", DbType.String, ColumnProperty.NotNull),
              new Column("SUM", DbType.Decimal, ColumnProperty.NotNull),
              new Column("PURPOSE", DbType.String),
              new Column("NAME_PAY", DbType.String),
              new Column("INN_PAY", DbType.String, 12),
              new Column("KPP_PAY", DbType.String, 9)
            );
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_VR_ASFK_BDOPER");
        }
    }
}


