namespace Bars.Gkh.Migrations._2022.Version_2022091500
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Utils;

    [Migration("2022091500")]
    
    [MigrationDependsOn(typeof(Version_2022051000.UpdateSchema))]


    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_INDIVIDUAL_PERSON", new Column("PLACE_RESIDENCE_OUTSTATE", DbType.String, 500));
            Database.AddColumn("GKH_INDIVIDUAL_PERSON", new Column("ACTUALLY_RESIDENCE_OUTSTATE", DbType.String, 500));
            Database.AddColumn("GKH_INDIVIDUAL_PERSON", new Column("IS_PLACE_RESIDENCE_OUTSTATE", DbType.Boolean, ColumnProperty.NotNull, false));
            Database.AddColumn("GKH_INDIVIDUAL_PERSON", new Column("IS_ACTUALLY_RESIDENCE_OUTSTATE", DbType.Boolean, ColumnProperty.NotNull, false));
        }

        /// <summary>
        /// Откатить
        /// </summary>
        public override void Down()
        {
            Database.RemoveColumn("GKH_INDIVIDUAL_PERSON", "IS_ACTUALLY_RESIDENCE_OUTSTATE");
            Database.RemoveColumn("GKH_INDIVIDUAL_PERSON", "IS_PLACE_RESIDENCE_OUTSTATE");
            Database.RemoveColumn("GKH_INDIVIDUAL_PERSON", "ACTUALLY_RESIDENCE_OUTSTATE");
            Database.RemoveColumn("GKH_INDIVIDUAL_PERSON", "PLACE_RESIDENCE_OUTSTATE");
        }
    }
}