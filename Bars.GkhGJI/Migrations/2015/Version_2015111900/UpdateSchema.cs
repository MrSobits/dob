namespace Bars.GkhGji.Migrations._2015.Version_2015111900
{
    using Bars.Gkh;
    using ECM7.Migrator.Framework;

    [Migration(2015111900)]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            ViewManager.Create(Database, "GkhGji", "CreateViewPrescription");
        }

        public override void Down()
        {

        }
    }
}
