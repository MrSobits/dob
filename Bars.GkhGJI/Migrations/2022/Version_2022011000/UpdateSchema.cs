namespace Bars.GkhGji.Migrations._2022.Version_2022011000
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh;

    [Migration("2022011000")]
    [MigrationDependsOn(typeof(Version_2022010500.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_DISPOSAL", new Column("TYPE_VIOLATOR", DbType.Int16, ColumnProperty.NotNull, (int)Enums.TypeViolator.IP));
            Database.AddRefColumn("GJI_DISPOSAL", new RefColumn("CONTRAGENT_ID", ColumnProperty.None, "GJI_DISPOSAL_GKH_CONTRAGENT", "GKH_CONTRAGENT", "ID"));
            Database.AddRefColumn("GJI_DISPOSAL", new RefColumn("CONTRAGENT_CONTACT_ID", ColumnProperty.None, "GJI_DISPOSAL_GKH_CONTRAGENT_CONTACT", "GKH_CONTRAGENT_CONTACT", "ID"));
            Database.AddRefColumn("GJI_DISPOSAL", new RefColumn("INDIVIDUAL_PERSON_ID", ColumnProperty.None, "GJI_DISPOSAL_GKH_INDIVIDUAL_PERSON", "GKH_INDIVIDUAL_PERSON", "ID"));
        
        }
        public override void Down()
        {
            Database.RemoveColumn("GJI_DISPOSAL", "CONTRAGENT_ID");
            Database.RemoveColumn("GJI_DISPOSAL", "CONTRAGENT_CONTACT_ID");
            Database.RemoveColumn("GJI_DISPOSAL", "INDIVIDUAL_PERSON_ID");
            Database.RemoveColumn("GJI_DISPOSAL", "TYPE_VIOLATOR");
        }
    }
}