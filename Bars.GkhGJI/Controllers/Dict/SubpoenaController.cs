namespace Bars.GkhGji.Controllers
{
    using System.Collections;
    using System.Web.Mvc;
    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.DomainService.Dict;
    using Bars.GkhGji.Entities;

    public class SubpoenaController : B4.Alt.DataController<Subpoena>
    {
        
        public ActionResult ComissionListSubpoena(BaseParams baseParams)
        {
            var SubpoenaService = Container.Resolve<ISubpoena>();
            try
            {
                return SubpoenaService.ComissionListSubpoena(baseParams).ToJsonResult();
            }
            catch
            {
                return null;
            }
            finally
            {
            }
        }

        public ActionResult ListView(BaseParams baseParams) 
        {
            var SubpoenaService = Container.Resolve<ISubpoena>();
            try
            {
                return SubpoenaService.ListView(baseParams).ToJsonResult();
            }
            catch
            {
                return null;
            }
            finally
            {
            }

        }
    }
 
}