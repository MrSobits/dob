namespace Bars.Gkh.Migrations._2020.Version_2020102700
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2020102700")]
    
    [MigrationDependsOn(typeof(Version_2020101400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable(
                      "GKH_CS_CALCULATION",
                      new Column("NAME", DbType.String, ColumnProperty.None),
                      new Column("DESCRIPTION", DbType.String, ColumnProperty.None),
                      new Column("RESULT", DbType.Decimal, ColumnProperty.None));

            this.Database.AddEntityTable(
                "GKH_CS_CALCULATION_ROW",
                new Column("NAME", DbType.String, ColumnProperty.NotNull),
                new Column("CODE", DbType.String, ColumnProperty.NotNull),
                new Column("DISPLAY_TEXT", DbType.String, ColumnProperty.NotNull),
                new Column("VALUE", DbType.Decimal, ColumnProperty.None),
                new RefColumn("CALC_ID", ColumnProperty.NotNull, "GKH_CS_CALCULATION_ROW_CALC_ID", "GKH_CS_CALCULATION", "ID"));
        }

        /// <summary>
        /// Откатить
        /// </summary>
        public override void Down()
        {
            Database.RemoveTable("GKH_CS_CALCULATION_ROW");
            Database.RemoveTable("GKH_CS_CALCULATION");
        }
    }
}