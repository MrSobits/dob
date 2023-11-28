namespace Bars.Gkh.Migrations._2022.Version_2022032000
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Utils;

    [Migration("2022032000")]
    
    [MigrationDependsOn(typeof(Version_2022022100.UpdateSchema))]


    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AlterColumnSetNullable("GKH_INDIVIDUAL_PERSON", "PLACE_RESIDENCE", true);
            this.Database.AlterColumnSetNullable("GKH_INDIVIDUAL_PERSON", "ACTUALLY_RESIDENCE", true);
            this.Database.AlterColumnSetNullable("GKH_INDIVIDUAL_PERSON", "BIRTH_PLACE", true);
            this.Database.AlterColumnSetNullable("GKH_INDIVIDUAL_PERSON", "JOB", true);
            this.Database.AlterColumnSetNullable("GKH_INDIVIDUAL_PERSON", "DATE_BIRTH", true);
            this.Database.AlterColumnSetNullable("GKH_INDIVIDUAL_PERSON", "PASSPORT_NUMBER", true);
            this.Database.AlterColumnSetNullable("GKH_INDIVIDUAL_PERSON", "PASSPORT_SERIES", true);
            this.Database.AlterColumnSetNullable("GKH_INDIVIDUAL_PERSON", "DEPARTMENT_CODE", true);
            this.Database.AlterColumnSetNullable("GKH_INDIVIDUAL_PERSON", "DATE_ISSUE", true);
            this.Database.ChangeColumn("GKH_INDIVIDUAL_PERSON", new Column("PASSPORT_NUMBER", DbType.String, 10, ColumnProperty.None));
            this.Database.ChangeColumn("GKH_INDIVIDUAL_PERSON", new Column("PASSPORT_SERIES", DbType.String, 10, ColumnProperty.None));
            this.Database.ChangeColumn("GKH_INDIVIDUAL_PERSON", new Column("DEPARTMENT_CODE", DbType.String, 10, ColumnProperty.None));

        }
        /// <summary>
        /// Откатить
        /// </summary>
        public override void Down()
        {
          
        }
    }
}