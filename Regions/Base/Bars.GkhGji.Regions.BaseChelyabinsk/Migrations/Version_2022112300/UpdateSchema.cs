namespace Bars.GkhGji.Migrations._2022.Version_2022112300
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2022112300")]
    [MigrationDependsOn(typeof(Version_2022100200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {
            //Ходатайства протокола
            Database.AddEntityTable(
                "GJI_PROTOCOL197_PETITION",
                new RefColumn("PROTOCOL197_ID", "GJI_PROTOCOL197_PETITION_DOC", "GJI_PROTOCOL197", "ID"),
                new Column("AUTHOR_FIO", DbType.String),
                new Column("AUTHOR_DUTY", DbType.String),
                new Column("WORKPLACE", DbType.String),
                new RefColumn("INSPECTOR_ID", "GJI_PROTOCOL197_PETITION_INSPECTOR", "GKH_DICT_INSPECTOR", "ID"),
                new Column("PETITION_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("PETITION_TEXT", DbType.String, 3500));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_PROTOCOL197_PETITION");
        }
    }
}
