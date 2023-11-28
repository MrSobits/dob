namespace Bars.GkhGji.Migrations._2022.Version_2022010500
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh;

    [Migration("2022010500")]
    [MigrationDependsOn(typeof(_2021.Version_2021121400.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
        
            
            Database.AddEntityTable(
             "GJI_TRANSPORT",
             new Column("NAME_TRANSPORT", DbType.String),
             new Column("NAMBER_TRANSPORT", DbType.String),
             new Column("REGISTRATION_NAMBER_TRANSPORT", DbType.Int64),
             new Column("SERIES_TRANSPORT", DbType.String),
             new Column("REG_NAMBER_TRANSPORT", DbType.Int64)
             );
            Database.AddEntityTable(
           "GJI_OWNER",
           new Column("TYPE_VIOLATOR", DbType.Int16, ColumnProperty.NotNull, (int)Enums.TypeViolator.IP),
           new RefColumn("INDIVIDUAL_PERSON_ID", ColumnProperty.None, "GJI_OWNER_INDIVIDUAL_PERSON_ID_GKH_INDIVIDUAL_PERSON", "GKH_INDIVIDUAL_PERSON", "ID"),
           new RefColumn("CONTRAGENT_ID", ColumnProperty.None, "GJI_OWNER_CONTRAGENT_ID_GKH_CONTRAGENT", "GKH_CONTRAGENT", "ID"),
           new RefColumn("CONTRAGENT_CONTACT_ID", ColumnProperty.None, "GJI_OWNER_CONTRAGENT_CONTACT_ID_GKH_CONTRAGENT_CONTACT", "GKH_CONTRAGENT_CONTACT", "ID"),
           new RefColumn("TRANSPORT_ID", ColumnProperty.None, "GJI_OWNER_TRANSPORT_ID_GJI_TRANSPORT", "GJI_TRANSPORT","ID")

           );
            Database.AddEntityTable(
                "GJI_OWNER_ROOM",
                new Column("TYPE_VIOLATOR_OWNER_ROOM", DbType.Int16, ColumnProperty.NotNull, (int)Bars.GkhGji.Enums.TypeViolatorOwnerRoom.PhisicalPerson),
                new Column("DATA_OWNER_START", DbType.DateTime),
                new Column("DATA_OWNER_EDIT", DbType.DateTime),
                new RefColumn("INDIVIDUAL_PERSON_ID", ColumnProperty.None, "GJI_OWNER_ROOM_INDIVIDUAL_PERSON_ID_GKH_INDIVIDUAL_PERSON", "GKH_INDIVIDUAL_PERSON", "ID"),
                new RefColumn("CONTRAGENT_ID", ColumnProperty.None, "GJI_OWNER_ROOM_CONTRAGENT_ID_GKH_CONTRAGENT", "GKH_CONTRAGENT", "ID"),
                new RefColumn("ROOM_ID", ColumnProperty.None, "GJI_OWNER_ROOM_ROOM_ID_GKH_ROOM", "GKH_ROOM","ID")
                );
            Database.AddEntityTable(
                "GJI_TRANSPORT_DISPOSAL",
                new RefColumn("TRANSPORT_ID",ColumnProperty.None, "GJI_TRANSPORT_PROTOCOL_ID_GJI_TRANSPORT", "GJI_TRANSPORT","ID"),
                new RefColumn("DISPOSAL_ID", ColumnProperty.None, "GJI_TRANSPORT_PROTOCOL_ID_GJI_DISPOSAL", "GJI_DISPOSAL", "ID")
                );


        }
        public override void Down()
        {
            Database.RemoveTable("GJI_TRANSPORT_DISPOSAL");
            Database.RemoveTable("GJI_OWNER_ROOM");
            Database.RemoveTable("GJI_OWNER");
            Database.RemoveTable("GJI_TRANSPORT");
        }
    }
}