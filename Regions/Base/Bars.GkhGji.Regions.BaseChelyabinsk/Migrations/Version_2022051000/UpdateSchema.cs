namespace Bars.GkhGji.Migrations._2022.Version_2022051000
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh;

    [Migration("2022051000")]
    [MigrationDependsOn(typeof(Version_2022040700.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_PROTOCOL197", new Column("DEPENDENTS_NUMBER", DbType.Int16, ColumnProperty.None));
            Database.AddRefColumn("GJI_PROTOCOL197", new RefColumn("SOCIAL_STATE", "GJI_PROTOCOL197_SOCIAL_STATE", "GJI_DICT_SOC_ST", "ID"));
        }

        /// <summary>
        /// Откатить
        /// </summary>
        public override void Down()
        {
            Database.RemoveColumn("GJI_PROTOCOL197", "SOCIAL_STATE");
            Database.RemoveColumn("GJI_PROTOCOL197", "DEPENDENTS_NUMBER");
        }
    }
}