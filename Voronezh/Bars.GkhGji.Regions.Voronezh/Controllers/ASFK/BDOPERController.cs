namespace Bars.GkhGji.Regions.Voronezh.Controllers
{
    using Bars.B4;
    using Bars.GkhGji.Regions.Voronezh.Entities.ASFK;
    using System.Web.Mvc;
    using System;
    using Bars.GkhGji.Regions.Voronezh.DomainService;
    using Bars.Gkh.Domain;

    public class BDOPERController : B4.Alt.DataController<BDOPER>
    {
        public ActionResult AddPayFines(BaseParams baseParams)
        {
            var BDOPERService = Container.Resolve<IBDOPERService>();

            try
            {
                BDOPERService.AddPayFines(baseParams);
            }
            catch (Exception e)
            {

            }
            finally
            {
                Container.Release(BDOPERService);
            }

            return JsSuccess();
        }

        public ActionResult GetResolution(BaseParams baseParams)
        {
            var BDOPERService = Container.Resolve<IBDOPERService>();
            try
            {
                return BDOPERService.GetResolution(baseParams).ToJsonResult();
            }
            finally
            {
                Container.Release(BDOPERService);
            }
        }

        public ActionResult GetListResolutionsForSelect(BaseParams baseParams)
        {
            var BDOPERService = Container.Resolve<IBDOPERService>();
            try
            {
                return BDOPERService.GetListResolutionsForSelect(baseParams).ToJsonResult();
            }
            finally
            {
                Container.Release(BDOPERService);
            }
        }
    }
}
