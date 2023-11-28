namespace Bars.Gkh.Migrations._2022.Version_2022022100
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2022022100")]
    
    [MigrationDependsOn(typeof(Version_2022020300.UpdateSchema))]


    public class UpdateSchema : Migration
    {
        public override void Up()
        {
           Database.AddEntityTable
                (
                    "GKH_DICT_SUBDIVISION",
                    new Column("NAME", DbType.String, ColumnProperty.NotNull),
                    new Column("CODE", DbType.String, ColumnProperty.NotNull)
                );

           Database.AddRefColumn("GKH_DICT_INSPECTOR", new RefColumn("SUBDIVISION_ID", ColumnProperty.None, "GKH_DICT_SUBDIVISION_ID_GKH_DICT_INSPECTOR", "GKH_DICT_SUBDIVISION", "ID"));

        }
        /// <summary>
        /// Откатить
        /// </summary>
        public override void Down()
        {
            Database.RemoveColumn("GKH_DICT_SUBDIVISION", "CODE");
            Database.RemoveColumn("GKH_DICT_SUBDIVISION", "NAME");
            Database.RemoveColumn("GKH_DICT_SUBDIVISION", "SUBDIVISION_ID");
        }
    }
}