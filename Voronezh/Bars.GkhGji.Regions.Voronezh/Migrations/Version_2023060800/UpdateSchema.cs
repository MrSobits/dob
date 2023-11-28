namespace Bars.GkhGji.Regions.Voronezh.Migrations.Version_2023060800
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2023060800")]
    [MigrationDependsOn(typeof(Version_2021102600.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddEntityTable
            (
              "GJI_VR_ASFK",
              new Column("NUM_VER", DbType.String, ColumnProperty.NotNull),
              new Column("FORMER", DbType.String, ColumnProperty.NotNull),
              new Column("FORM_VER", DbType.String, ColumnProperty.NotNull),
              new Column("NORM_DOC", DbType.String),
              new Column("KOD_TOFK_FROM", DbType.String, 4, ColumnProperty.NotNull),
              new Column("NAME_TOFK_FROM", DbType.String, 2000, ColumnProperty.NotNull),
              new Column("BUDGET_LEVEL", DbType.Int16, ColumnProperty.NotNull, 1),
              new Column("KOD_UBP", DbType.String, 8, ColumnProperty.NotNull),
              new Column("NAME_UBP", DbType.String, 2000, ColumnProperty.NotNull),
              new Column("GUID_VT", DbType.String, ColumnProperty.NotNull),
              new Column("LS_ADB", DbType.String, 11, ColumnProperty.NotNull),
              new Column("DATE_OTCH", DbType.DateTime, ColumnProperty.NotNull),
              new Column("DATE_OLD", DbType.DateTime),
              new Column("VID_OTCH", DbType.Int16, ColumnProperty.NotNull),
              new Column("KOD_TOFK_VT", DbType.String, 4, ColumnProperty.NotNull),
              new Column("NAME_TOFK_VT", DbType.String, 2000, ColumnProperty.NotNull),
              new Column("KOD_UBP_ADB", DbType.String, 8, ColumnProperty.NotNull),
              new Column("NAME_UBP_ADB", DbType.String, 2000, ColumnProperty.NotNull),
              new Column("KOD_GADB", DbType.String, 3, ColumnProperty.NotNull),
              new Column("NAME_GADB", DbType.String, 2000, ColumnProperty.NotNull),
              new Column("NAME_BUD", DbType.String, 512, ColumnProperty.NotNull),
              new Column("OKTMO", DbType.String, 8, ColumnProperty.NotNull),
              new Column("OKPO_FO", DbType.String, 8),
              new Column("NAME_FO", DbType.String, 2000),
              new Column("DOL_ISP", DbType.String, 100),
              new Column("NAME_ISP", DbType.String, 50),
              new Column("TEL_ISP", DbType.String, 50),
              new Column("DATE_POD", DbType.DateTime),
              new Column("SUM_IN_ITOG_V", DbType.Decimal, ColumnProperty.NotNull),
              new Column("SUM_OUT_ITOG_V", DbType.Decimal, ColumnProperty.NotNull),
              new Column("SUM_ZACH_ITOG_V", DbType.Decimal, ColumnProperty.NotNull),
              new Column("SUM_N_OUT_ITOG_V", DbType.Decimal, ColumnProperty.NotNull),
              new Column("SUM_N_ZACH_ITOG_V", DbType.Decimal, ColumnProperty.NotNull),
              new Column("SUM_BEGIN_IN", DbType.Decimal, ColumnProperty.NotNull),
              new Column("SUM_BEGIN_OUT", DbType.Decimal, ColumnProperty.NotNull),
              new Column("SUM_BEGIN_ZACH", DbType.Decimal, ColumnProperty.NotNull),
              new Column("SUM_BEGIN_N_OUT", DbType.Decimal, ColumnProperty.NotNull),
              new Column("SUM_BEGIN_N_ZACH", DbType.Decimal, ColumnProperty.NotNull),
              new Column("SUM_END_IN", DbType.Decimal, ColumnProperty.NotNull),
              new Column("SUM_END_OUT", DbType.Decimal, ColumnProperty.NotNull),
              new Column("SUM_END_ZACH", DbType.Decimal, ColumnProperty.NotNull),
              new Column("SUM_END_N_OUT", DbType.Decimal, ColumnProperty.NotNull),
              new Column("SUM_END_N_ZACH", DbType.Decimal, ColumnProperty.NotNull)
            );

            Database.AddEntityTable
            (
              "GJI_VR_ASFK_VTOPER",
              new RefColumn("ASFK_ID", ColumnProperty.None, "GJI_VR_VTOPER_ASFK_ID", "GJI_VR_ASFK", "ID"),
              new RefColumn("PAYREG_ID", ColumnProperty.None, "GJI_VR_VTOPER_PAYREG_ID", "GJI_CH_PAY_REG", "ID"),
              new Column("GUID", DbType.String, ColumnProperty.NotNull),
              new Column("KOD_DOC", DbType.Int16, ColumnProperty.NotNull),
              new Column("NOM_DOC", DbType.String, 15),
              new Column("DATE_DOC", DbType.DateTime, ColumnProperty.NotNull),
              new Column("KOD_DOC_ADB", DbType.Int16),
              new Column("NOM_DOC_ADB", DbType.String, 15),
              new Column("DATE_DOC_ADB", DbType.DateTime),
              new Column("SUM_IN", DbType.Decimal, ColumnProperty.NotNull),
              new Column("SUM_OUT", DbType.Decimal, ColumnProperty.NotNull),
              new Column("SUM_ZACH", DbType.Decimal, ColumnProperty.NotNull),
              new Column("NOTE", DbType.String),
              new Column("TYPE_KBK", DbType.Int16),
              new Column("KBK", DbType.String, 20),
              new Column("ADD_KLASS", DbType.String, 20),
              new Column("OKATO", DbType.String, 8),
              new Column("INN_ADB", DbType.String, 10),
              new Column("KPP_ADB", DbType.String, 9)
            );
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_VR_ASFK");
            Database.RemoveTable("GJI_VR_ASFK_VTOPER");
        }
    }
}


