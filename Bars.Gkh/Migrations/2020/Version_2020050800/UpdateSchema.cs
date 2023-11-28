namespace Bars.Gkh.Migrations._2020.Version_2020050800
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2020050800")]
    
    [MigrationDependsOn(typeof(Version_2020042400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable("GKH_DICT_ADDIT_WORK",
                new Column("NAME", DbType.String),
                new Column("DESCRIPTION", DbType.String, 500),
                new Column("CODE", DbType.String),
                new Column("PERCENTAGE", DbType.Decimal, ColumnProperty.NotNull),
                new RefColumn("WORK_ID", "FK_ADD_WORK_WORK", "GKH_DICT_WORK", "ID"));

        }

        /// <summary>
        /// Откатить
        /// </summary>
        public override void Down()
        {
            Database.RemoveTable("GKH_DICT_ADDIT_WORK");

        }
    }
}