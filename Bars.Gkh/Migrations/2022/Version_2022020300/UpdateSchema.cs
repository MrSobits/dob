namespace Bars.Gkh.Migrations._2022.Version_2022020300
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2022020300")]
    
    [MigrationDependsOn(typeof(Version_2022012600.UpdateSchema))]


    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_INDIVIDUAL_PERSON", new Column("FAMILY_STATUS", DbType.Int16, ColumnProperty.NotNull, (int)Bars.Gkh.Enums.FamilyStatus.Default));
        }
        /// <summary>
        /// Откатить
        /// </summary>
        public override void Down()
        {
            Database.RemoveColumn("GKH_INDIVIDUAL_PERSON", "FAMILY_STATUS");
        }
    }
}