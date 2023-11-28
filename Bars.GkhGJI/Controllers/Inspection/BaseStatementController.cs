namespace Bars.GkhGji.Controllers
{

    using System.Collections;
    using System.Web.Mvc;

    using B4;
    using B4.Modules.DataExport.Domain;
    using DomainService;
    using Entities;

    public class BaseStatementController : BaseStatementController<BaseStatement>
    {
    }
    
    public class BaseStatementController<T> : B4.Alt.DataController<T>
        where T : BaseStatement
    {
        public ActionResult Export(BaseParams baseParams)
        {
            var export = Container.Resolve<IDataExportService>("BaseStatementDataExport");

            try
            {
                if (export != null)
                {
                    return export.ExportData(baseParams);
                }

                return null;
            }
            finally 
            {
                Container.Release(export);
            }
            
        }

        public ActionResult AddAppealCitizens(BaseParams baseParams)
        {
            var service = Container.Resolve<IBaseStatementService>();
            try
            {
                var result = service.AddAppealCitizens(baseParams);
                return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
            }
            finally 
            {
                Container.Release(service);
            }
        }

        public ActionResult CreateWithAppealCits(BaseParams baseParams)
        {
            var service = Container.Resolve<IBaseStatementService>();
            try
            {
                var result = service.CreateWithAppealCits(baseParams);

                return result.Success
                    ? new JsonNetResult(new { success = true, message = result.Message, data = result.Data })
                    {
                        ContentType = "text/html; charset=utf-8"
                    }
                    : JsonNetResult.Failure(result.Message);
            }
            finally 
            {
                Container.Release(service);
            }
            
        }

        public ActionResult GetInfo(BaseParams baseParams)
        {
            var service = Container.Resolve<IBaseStatementService>();
            try
            {
                var result = service.GetInfo(baseParams);
                return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
            }
            finally 
            {
                Container.Release(service);
            }
        }

        public virtual ActionResult ListByAppealCits(BaseParams baseParams)
        {
            var service = Container.Resolve<IBaseStatementService>();
            try
            {
                var result = (ListDataResult)service.ListByAppealCits(baseParams);
                return result.Success ? new JsonListResult((IList)result.Data, result.TotalCount) : JsonNetResult.Failure(result.Message);
            }
            finally 
            {
                Container.Release(service);
            }
        }

        public ActionResult CheckAppealCits(BaseParams baseParams)
        {
            var service = Container.Resolve<IBaseStatementService>();
            try
            {
                var result = service.CheckAppealCits(baseParams);

                return result.Success ? JsSuccess() : JsFailure(result.Message);
            }
            finally
            {
                Container.Release(service);
            }
        }

        public ActionResult AnyThematics(BaseParams baseParams)
        {
            var service = Container.Resolve<IBaseStatementService>();
            try
            {
                var result = service.AnyThematics(baseParams);

                return result.Success ? new JsonNetResult(result) : JsFailure(result.Message);
            }
            finally
            {
                Container.Release(service);
            }
        }

    }
}