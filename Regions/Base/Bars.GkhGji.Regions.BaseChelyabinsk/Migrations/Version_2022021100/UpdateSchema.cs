namespace Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2022021100
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;
  
    [Migration("2022021100")]
    [MigrationDependsOn(typeof(Version_2022021001.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_PROTOCOL197", new Column("TYPE_VIOLATOR", DbType.Int16, ColumnProperty.NotNull, (int)Bars.GkhGji.Enums.TypeViolator.IP));
        }
                       
        public override void Down()
        {
         
            Database.RemoveColumn("GJI_PROTOCOL197", "TYPE_VIOLATOR");
        }

    }
}