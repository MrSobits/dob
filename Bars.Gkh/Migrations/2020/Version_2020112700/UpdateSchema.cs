namespace Bars.Gkh.Migrations._2020.Version_2020112700
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2020112700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(Version_2020112000.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("GKH_DICT_TYPE_PROBLEM",
                new Column("NAME", DbType.String, ColumnProperty.NotNull, 200),
                new Column("REQUEST_TEMPLATE", DbType.String, ColumnProperty.NotNull, 10000),
                new Column("RESPONCE_TEMPLATE", DbType.String, ColumnProperty.NotNull,10000),
                new RefColumn("RUBRIC_ID", ColumnProperty.NotNull, "GKH_DICT_TYPE_PROBLEM_RUBRIC_ID", "GKH_SUG_RUBRIC", "ID"));
        }

        public override void Down()
        {

            Database.RemoveTable("GKH_DICT_TYPE_PROBLEM");
        }
    }
}