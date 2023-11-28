namespace Bars.GkhGji.Controllers
{
    using System.Collections;
    using System.Web.Mvc;

    using Bars.B4;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.InspectionRules;

    public class InspectionGjiController : InspectionGjiController<InspectionGji>
    {
    }

    public class InspectionGjiController<T> : B4.Alt.DataController<T>
        where T : InspectionGji
    {
        public ActionResult CreateDocument(BaseParams baseParams)
        {
            var service = Container.Resolve<IInspectionGjiProvider>();
            try
            {
                var result = service.CreateDocument(baseParams);
                return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
            }
            finally 
            {
                Container.Release(service);
            }
        }

        public ActionResult GetListRules(BaseParams baseParams)
        {
            var service = Container.Resolve<IInspectionGjiProvider>();
            try
            {
                var result = service.GetRules(baseParams);
                return result.Success ? new JsonListResult((IEnumerable)result.Data) : JsonNetResult.Failure(result.Message);
            }
            finally
            {
                Container.Release(service);
            }
        }
    }
}