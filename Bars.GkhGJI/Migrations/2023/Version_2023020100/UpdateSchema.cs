namespace Bars.GkhGji.Migrations._2022.Version_2023020100
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Enums;

    [Migration("2023020100")]
    [MigrationDependsOn(typeof(Version_2023011900.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {
            Database.AddColumn("GJI_DICT_VIOLATION", new RefColumn("ARTICLELAW_ID", "GJI_DICT_VIOLATION_ARTICLELAW_ID", "GJI_DICT_ARTICLELAW", "ID"));
            Database.AddColumn("GJI_DICT_VIOLATION", new RefColumn("ARTICLELAWREPEAT_ID", "GJI_DICT_VIOLATION_ARTICLELAWREPEAT_ID", "GJI_DICT_ARTICLELAW", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_DICT_VIOLATION", "ARTICLELAW_ID");
            Database.RemoveColumn("GJI_DICT_VIOLATION", "ARTICLELAWREPEAT_ID");
        }
    }
}
