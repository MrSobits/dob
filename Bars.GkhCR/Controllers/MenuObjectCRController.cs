namespace Bars.GkhCr.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Controllers;
    using Bars.GkhCr.Entities;
    using Gkh.Domain;

    public class MenuObjectCrController : MenuController
    {
        public ActionResult GetObjectCrMenu(StoreLoadParams storeParams)
        {
            var id = storeParams.Params.GetAsId("objectId");
            if (id > 0)
            {
                InitActiveOperatorAndRoles();

                var objectCr = Container.Resolve<IDomainService<Entities.ObjectCr>>().Get(id);

                InitStatePermissions(objectCr.State);

                var menuItems = this.FilterInacessibleStateItems(GetMenuItems("ObjectCr"));

                var monitoringSmr = this.Container.Resolve<IDomainService<MonitoringSmr>>().GetAll().FirstOrDefault(x => x.ObjectCr.Id == id);
                if (monitoringSmr != null && monitoringSmr.State != null)
                {
                    InitStatePermissions(monitoringSmr.State);

                    var monitoringCmpMenuItems = this.FilterInacessibleStateItems(GetMenuItems("MonitoringSmr"));

                    var monitoringSmrMenuItem = monitoringCmpMenuItems.FirstOrDefault();
                    if (monitoringSmrMenuItem != null)
                    {
                        var monSmrMainMenu = menuItems.FirstOrDefault(x => x.Caption == monitoringSmrMenuItem.Caption);
                        if (monSmrMainMenu != null)
                        {
                            monSmrMainMenu.Items = monitoringSmrMenuItem.Items;
                        }
                    }
                }

                return new JsonNetResult(menuItems);
            }

            return new JsonNetResult(null);
        }

        public ActionResult GetBankStatementMenu(StoreLoadParams storeParams)
        {
            var id = storeParams.Params.GetAsId("objectId");
            return id > 0 ? new JsonNetResult(GetMenuItems("BankStatement")) : new JsonNetResult(null);
        }
        public ActionResult GetCompetitionMenu(StoreLoadParams storeParams)
        {
            var id = storeParams.Params.GetAsId("objectId");
            return id > 0 ? new JsonNetResult(GetMenuItems("Competition")) : new JsonNetResult(null);
        }
    }
}