namespace Bars.GkhGji.Migrations._2022.Version_2023020900
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Enums;

    [Migration("2023020900")]
    [MigrationDependsOn(typeof(Version_2023020200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {
            Database.AddColumn("GJI_DICT_ARTICLELAW", new Column("OMS_REGION", DbType.Int16, defaultValue: 5, property: ColumnProperty.NotNull));

            Database.ExecuteNonQuery(@"UPDATE GJI_DICT_ARTICLELAW SET OMS_REGION = 5 WHERE OMS = 'РЕГИОН'");

            Database.ExecuteNonQuery(@"UPDATE GJI_DICT_ARTICLELAW SET OMS_REGION = 10 WHERE OMS = 'ОМС'");

            Database.RemoveColumn("GJI_DICT_ARTICLELAW", "KBK_CENTR");
            Database.RemoveColumn("GJI_DICT_ARTICLELAW", "KBK_COMSOMOL");
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_DICT_ARTICLELAW", "OMS_REGION");

            Database.AddColumn("GJI_DICT_ARTICLELAW", new Column("KBK_CENTR", DbType.String, 255));
            Database.AddColumn("GJI_DICT_ARTICLELAW", new Column("KBK_COMSOMOL", DbType.String, 36));

            Database.ExecuteNonQuery(@"UPDATE GJI_DICT_ARTICLELAW SET KBK_CENTR = KBK, KBK_COMSOMOL = KBK");
        }
    }
}
