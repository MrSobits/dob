namespace Bars.GkhGji.Migrations.Version_2015092900
{
    using ECM7.Migrator.Framework;

    [Migration(2015092900)]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            if (Database.TableExists("GJI_INSPECTION_LIC_APP") &&
                Database.ColumnExists("GJI_INSPECTION_LIC_APP", "FORM_CHECK"))
            {
                Database.ExecuteNonQuery("create table TEMP_LIC_VISUAL(ID bigint)");
                Database.ExecuteNonQuery(@"insert into TEMP_LIC_VISUAL(ID) 
                                           select ID from GJI_INSPECTION_LIC_APP where FORM_CHECK = 30");
                Database.ExecuteNonQuery("update GJI_INSPECTION_LIC_APP set FORM_CHECK = 30 where FORM_CHECK = 40");
                Database.ExecuteNonQuery("update GJI_INSPECTION_LIC_APP set FORM_CHECK = 40 where ID in (select ID from TEMP_LIC_VISUAL)");
                Database.ExecuteNonQuery("drop table TEMP_LIC_VISUAL");
            }

            if (Database.TableExists("GJI_INSPECTION_STATEMENT") &&
                Database.ColumnExists("GJI_INSPECTION_STATEMENT", "FORM_CHECK"))
            {
                Database.ExecuteNonQuery("create table TEMP_STATEMENT_VISUAL(ID bigint)");
                Database.ExecuteNonQuery(@"insert into TEMP_STATEMENT_VISUAL(ID) 
                                           select ID from GJI_INSPECTION_STATEMENT where FORM_CHECK = 30");
                Database.ExecuteNonQuery("update GJI_INSPECTION_STATEMENT set FORM_CHECK = 30 where FORM_CHECK = 40");
                Database.ExecuteNonQuery("update GJI_INSPECTION_STATEMENT set FORM_CHECK = 40 where ID in (select ID from TEMP_STATEMENT_VISUAL)");
                Database.ExecuteNonQuery("drop table TEMP_STATEMENT_VISUAL");
            }
        }

        public override void Down()
        {
            if (Database.TableExists("GJI_INSPECTION_STATEMENT") &&
                Database.ColumnExists("GJI_INSPECTION_STATEMENT", "FORM_CHECK"))
            {
                Database.ExecuteNonQuery("create table TEMP_STATEMENT_VISUAL(ID bigint)");
                Database.ExecuteNonQuery(@"insert into TEMP_STATEMENT_VISUAL(ID) 
                                           select ID from GJI_INSPECTION_STATEMENT where FORM_CHECK = 40");
                Database.ExecuteNonQuery("update GJI_INSPECTION_STATEMENT set FORM_CHECK = 40 where FORM_CHECK = 30");
                Database.ExecuteNonQuery("update GJI_INSPECTION_STATEMENT set FORM_CHECK = 30 where ID in (select ID from TEMP_STATEMENT_VISUAL)");
                Database.ExecuteNonQuery("drop table TEMP_STATEMENT_VISUAL");
            }

            if (Database.TableExists("GJI_INSPECTION_LIC_APP") &&
                Database.ColumnExists("GJI_INSPECTION_LIC_APP", "FORM_CHECK"))
            {
                Database.ExecuteNonQuery("create table TEMP_LIC_VISUAL(ID bigint)");
                Database.ExecuteNonQuery(@"insert into TEMP_LIC_VISUAL(ID) 
                                           select ID from GJI_INSPECTION_LIC_APP where FORM_CHECK = 40");
                Database.ExecuteNonQuery("update GJI_INSPECTION_LIC_APP set FORM_CHECK = 40 where FORM_CHECK = 30");
                Database.ExecuteNonQuery("update GJI_INSPECTION_LIC_APP set FORM_CHECK = 30 where ID in (select ID from TEMP_LIC_VISUAL)");
                Database.ExecuteNonQuery("drop table TEMP_LIC_VISUAL");
            }
        }
    }
}