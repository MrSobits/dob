namespace Bars.GkhGji.Migrations._2022.Version_2022020300
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh;
   
    [Migration("2022020300")]
    [MigrationDependsOn(typeof(Version_2022020200.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_DISPOSAL", new Column("FAMILY_STATUS", DbType.Int16, ColumnProperty.NotNull, (int)Bars.Gkh.Enums.FamilyStatus.Default));
        }
        public override void Down()
        {
            Database.RemoveColumn("GJI_DISPOSAL", "FAMILY_STATUS");

        }
    }
}