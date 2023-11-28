namespace Bars.GkhGji.Controllers
{
    using System.Collections;
    using System.Linq;
    using System.Web.Mvc;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.Gkh.DomainService;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;

    public class ResolutionController : ResolutionController<Resolution>
    {
    }

    public class ResolutionController<T> : B4.Alt.DataController<T>
        where T : Resolution
    {
        public ActionResult GetInfo(long? documentId)
        {
            var service = Container.Resolve<IResolutionService>();
            try
            {
                var result = service.GetInfo(documentId);
                return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
            }
            finally 
            {
                Container.Release(service);
            }
        }

        public ActionResult GetResolutionInfo(BaseParams baseParams)
        {
            var service = Container.Resolve<IResolutionService>();
            try
            {
                var result = service.GetResolutionInfo(baseParams);
                return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
            }
            finally 
            {
                Container.Release(service);
            }
        }

        public ActionResult ListView(BaseParams baseParams)
        {
            var service = Container.Resolve<IResolutionService>();
            try
            {
                var result = (ListDataResult)service.ListView(baseParams);
                return result.Success ? new JsonListResult((IList)result.Data, result.TotalCount) : JsonNetResult.Failure(result.Message);
            }
            finally 
            {
                Container.Release(service);
            }
        }

        public ActionResult SetSumAmount(BaseParams baseParams)
        {
            var result = this.Resolve<IResolutionOperationProvider>().ChangeSumAmount(baseParams);
            return result.Success ? this.JsSuccess(result) : this.JsFailure(result.Message, result);
        }

        public ActionResult CreateProtocols(BaseParams baseParams)
        {
            var result = this.Resolve<IResolutionOperationProvider>().CreateProtocols(baseParams);
            return result.Success ? this.JsSuccess(result) : this.JsFailure(result.Message, result);
        }

        public ActionResult ChangeOSP(BaseParams baseParams)
        {
            var result = this.Resolve<IResolutionOperationProvider>().ChangeOSP(baseParams);
            return result.Success ? this.JsSuccess(result) : this.JsFailure(result.Message, result);
        }

        public ActionResult ChangeSentToOSP(BaseParams baseParams)
        {
            var result = this.Resolve<IResolutionOperationProvider>().ChangeSentToOSP(baseParams);
            return result.Success ? this.JsSuccess(result) : this.JsFailure(result.Message, result);
        }

        public ActionResult ListResolutionOperations()
        {
            //var authService = this.Container.ResolveAll<IAuthorizationService>().FirstOrDefault();
            //var userIdentity = this.Resolve<IUserIdentity>();

            var result = this.Resolve<IResolutionOperationProvider>().GetAllOperations()
              //  .Where(x => string.IsNullOrEmpty(x.PermissionKey) || authService.Grant(userIdentity, x.PermissionKey))
                .ToList();

            return new JsonListResult(result, result.Count);
        }

        public ActionResult ListIndividualPerson(BaseParams baseParams)
        {
            var service = Container.Resolve<IResolutionService>();
            try
            {
                var result = (ListDataResult)service.ListIndividualPerson(baseParams);
                return result.Success ? new JsonListResult((IList)result.Data, result.TotalCount) : JsonNetResult.Failure(result.Message);
            }
            finally
            {
                Container.Release(service);
            }
        }

        public ActionResult ListResolutionIndividualPerson(BaseParams baseParams) 
        {
            var service = Container.Resolve<IResolutionService>();
            try
            {
                var result = (ListDataResult)service.ListResolutionIndividualPerson(baseParams);
                return result.Success ? new JsonListResult((IList)result.Data, result.TotalCount) : JsonNetResult.Failure(result.Message);
            }
            finally
            {
                Container.Release(service);
            }
        }


        public ActionResult Export(BaseParams baseParams)
        {
            var export = Container.Resolve<IDataExportService>("ResolutionDataExport");

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

        public IBlobPropertyService<Resolution, ResolutionLongText> LongTextService { get; set; }

        public ActionResult GetDescription(BaseParams baseParams)
        {
            return this.GetBlob(baseParams);
        }

        public ActionResult SaveDescription(BaseParams baseParams)
        {
            return this.SaveBlob(baseParams);
        }

        private ActionResult SaveBlob(BaseParams baseParams)
        {
            var result = this.LongTextService.Save(baseParams);

            return result.Success ? new JsonGetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        private ActionResult GetBlob(BaseParams baseParams)
        {
            var result = this.LongTextService.Get(baseParams);

            return result.Success ? new JsonGetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }
    }
}