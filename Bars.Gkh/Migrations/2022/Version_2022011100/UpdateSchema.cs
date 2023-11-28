namespace Bars.Gkh.Migrations._2022.Version_2022011100
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2022011100")]
    
    [MigrationDependsOn(typeof(_2021.Version_2021122600.UpdateSchema))]


    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
             "GKH_INDIVIDUAL_PERSON",

        new Column("NAME", DbType.String,ColumnProperty.NotNull),
        new Column("PLACE_RESIDENCE", DbType.String, ColumnProperty.NotNull),
        new Column("ACTUALLY_RESIDENCE", DbType.String, ColumnProperty.NotNull),
        new Column("BIRTH_PLACE", DbType.String, ColumnProperty.NotNull),
        new Column("JOB", DbType.String),
        new Column("DATE_BIRTH", DbType.DateTime, ColumnProperty.NotNull),
        new Column("PASSPORT_NUMBER", DbType.Int64, ColumnProperty.NotNull),
        new Column("PASSPORT_SERIES", DbType.Int64, ColumnProperty.NotNull),
        new Column("DEPARTMENT_CODE", DbType.Int64, ColumnProperty.NotNull),
        new Column("DATE_ISSUE", DbType.DateTime, ColumnProperty.NotNull)
             );
            Database.AddRefColumn("GKH_CONTRAGENT_CONTACT", new RefColumn("INDIVIDUAL_PERSON_ID", ColumnProperty.None, "GKH_CONTRAGENT_CONTACT_ID_GKH_INDIVIDUAL_PERSON", "GKH_INDIVIDUAL_PERSON","ID"));
        }
        /// <summary>
        /// Откатить
        /// </summary>
        public override void Down()
        {
            Database.RemoveColumn("GKH_CONTRAGENT_CONTACT", "INDIVIDUAL_PERSON_ID");
            Database.RemoveTable("GKH_INDIVIDUAL_PERSON");


        }
    }
}