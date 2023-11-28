namespace Bars.GkhGji.Migrations._2022.Version_2022120100
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2022120100")]
    [MigrationDependsOn(typeof(Version_2022112300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {
            Database.RemoveColumn("GJI_PROTOCOL197_PETITION", "INSPECTOR_ID");
            Database.AddColumn("GJI_PROTOCOL197_PETITION", new Column("PETITION_DEC_TEXT", DbType.String, 3500));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_PROTOCOL197_PETITION", "PETITION_DEC_TEXT");
            Database.AddRefColumn("GJI_PROTOCOL197_PETITION", new RefColumn("INSPECTOR_ID", "GJI_PROTOCOL197_PETITION_INSPECTOR", "GKH_DICT_INSPECTOR", "ID"));
        
        }
    }
}
