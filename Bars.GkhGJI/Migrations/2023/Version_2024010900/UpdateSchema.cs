namespace Bars.GkhGji.Migrations._2022.Version_2024010900
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2024010900")]
    [MigrationDependsOn(typeof(Version_2023042400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {
            Database.AddEntityTable(
                "GJI_RESOLPROS_LTEXT",
                new RefColumn("RESOLPROS_ID", ColumnProperty.None, "GJI_RESOLPROS_LTEXT_RESOLPROS", "GJI_RESOLPROS", "ID"),
                new Column("DESCRIPTION", DbType.Binary)
            );

            Database.AddColumn("GJI_RESOLPROS", new RefColumn("PHYSICALPERSON_DOCTYPE_ID", "GJI_RESOLPROS_PHYSICALPERSONDOCTYPE_ID", "GJI_DICT_PHYSICAL_PERSON_DOC_TYPE", "ID"));
            Database.AddColumn("GJI_RESOLPROS", new Column("PHYSICALPERSON_DOC_NUM", DbType.String, 300));
            Database.AddColumn("GJI_RESOLPROS", new Column("PHYSICALPERSON_DOC_SERIAL", DbType.String, 300));
            Database.AddColumn("GJI_RESOLPROS", new Column("PHYSICALPERSON_NOT_CITIZENSHIP", DbType.Boolean, false));
            Database.AddColumn("GJI_RESOLPROS", new Column("BIRTH_PLACE", DbType.String));
            Database.AddColumn("GJI_RESOLPROS", new Column("JOB", DbType.String));
            Database.AddColumn("GJI_RESOLPROS", new Column("DATE_BIRTH", DbType.DateTime));
            Database.AddColumn("GJI_RESOLPROS", new Column("PASSPORT_NUMBER", DbType.Int64));
            Database.AddColumn("GJI_RESOLPROS", new Column("PASSPORT_SERIES", DbType.Int64));
            Database.AddColumn("GJI_RESOLPROS", new Column("DEPARTMENT_CODE", DbType.Int64));
            Database.AddColumn("GJI_RESOLPROS", new Column("DATE_ISSUE", DbType.DateTime));
            Database.AddColumn("GJI_RESOLPROS", new Column("PASSPORT_ISSUED", DbType.String));
            Database.AddColumn("GJI_RESOLPROS", new Column("FAMILY_STATUS", DbType.Int16, ColumnProperty.NotNull, (int)Gkh.Enums.FamilyStatus.Default));
            Database.AddColumn("GJI_RESOLPROS", new Column("DEPENDENTS_NUMBER", DbType.Int16, ColumnProperty.None));
            Database.AddColumn("GJI_RESOLPROS", new RefColumn("SOCIAL_STATE", "GJI_RESOLPROS_SOCIAL_STATE", "GJI_DICT_SOC_ST", "ID"));
            Database.AddColumn("GJI_RESOLPROS", new Column("IS_PLACE_RESIDENCE_OUTSTATE", DbType.Boolean, ColumnProperty.NotNull, false));
            Database.AddColumn("GJI_RESOLPROS", new Column("IS_ACTUALLY_RESIDENCE_OUTSTATE", DbType.Boolean, ColumnProperty.NotNull, false));
            Database.AddColumn("GJI_RESOLPROS", new Column("PHONE_NUMBER", DbType.String, 20));
            Database.AddColumn("GJI_RESOLPROS", new Column("DESCRIPTION", DbType.String, 2000));
            Database.AddColumn("GJI_RESOLPROS", new RefColumn("FIAS_REG_ADDRESS", "GJI_RESOLPROS_FIAS_REG_ADDRESS", "B4_FIAS_ADDRESS", "ID"));
            Database.AddColumn("GJI_RESOLPROS", new RefColumn("FIAS_FACT_ADDRESS", "GJI_RESOLPROS_FIAS_FACT_ADDRESS", "B4_FIAS_ADDRESS", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_RESOLPROS_LTEXT");
            Database.RemoveColumn("GJI_RESOLPROS", "PHYSICALPERSON_NOT_CITIZENSHIP");
            Database.RemoveColumn("GJI_RESOLPROS", "PHYSICALPERSON_DOC_SERIAL");
            Database.RemoveColumn("GJI_RESOLPROS", "PHYSICALPERSON_DOC_NUM");
            Database.RemoveColumn("GJI_RESOLPROS", "PHYSICALPERSON_DOCTYPE_ID");
            Database.RemoveColumn("GJI_RESOLPROS", "FAMILY_STATUS");
            Database.RemoveColumn("GJI_RESOLPROS", "PASSPORT_ISSUED");
            Database.RemoveColumn("GJI_RESOLPROS", "DATE_ISSUE");
            Database.RemoveColumn("GJI_RESOLPROS", "DEPARTMENT_CODE");
            Database.RemoveColumn("GJI_RESOLPROS", "PASSPORT_SERIES");
            Database.RemoveColumn("GJI_RESOLPROS", "PASSPORT_NUMBER");
            Database.RemoveColumn("GJI_RESOLPROS", "DATE_BIRTH");
            Database.RemoveColumn("GJI_RESOLPROS", "JOB");
            Database.RemoveColumn("GJI_RESOLPROS", "BIRTH_PLACE");
            Database.RemoveColumn("GJI_RESOLPROS", "SOCIAL_STATE");
            Database.RemoveColumn("GJI_RESOLPROS", "DEPENDENTS_NUMBER");
            Database.RemoveColumn("GJI_RESOLPROS", "IS_ACTUALLY_RESIDENCE_OUTSTATE");
            Database.RemoveColumn("GJI_RESOLPROS", "IS_PLACE_RESIDENCE_OUTSTATE");
            Database.RemoveColumn("GJI_RESOLPROS", "PHONE_NUMBER");
            Database.RemoveColumn("GJI_RESOLPROS", "DESCRIPTION");
            Database.RemoveColumn("GJI_RESOLPROS", "FIAS_FACT_ADDRESS");
            Database.RemoveColumn("GJI_RESOLPROS", "FIAS_REG_ADDRESS");
        }
    }
}
