namespace Bars.GkhGji.Migrations._2022.Version_2022021600
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Enums;

    [Migration("2022021600")]
    [MigrationDependsOn(typeof(Version_2022021400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_DICT_VIOLATION", new Column("TYPE_MUNICIPALITY", DbType.Int16, ColumnProperty.NotNull, (int)TypeMunicipality.MunicipalArea));
        }
        public override void Down()
        {
            Database.RemoveColumn("GJI_DICT_VIOLATION", "TYPE_MUNICIPALITY");    
        }
    }
}