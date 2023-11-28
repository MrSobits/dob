namespace Bars.GkhGji.Controllers
{
    using System.Web.Mvc;
    using System.Collections.Generic;
    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.Gkh.DomainService;
    using Bars.GkhGji.Entities;

    public class ComissionMeetingOperationController : BaseController
    {
        public IBlobPropertyService<ComissionMeetingDocument, ComissionMeetingDocumentLongText> LongTextService { get; set; }
        public ActionResult AddMembers(BaseParams baseParams)
        {
            var result = this.Container.Resolve<IComissionMeetingOperationService>().AddMembers(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }

        public virtual ActionResult GetDescription(BaseParams baseParams)
        {
            var result = this.LongTextService.Get(baseParams);
            return result.Success ? new JsonGetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        public virtual ActionResult SaveDescription(BaseParams baseParams)
        {
            var result = this.LongTextService.Save(baseParams);
            return result.Success ? new JsonGetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult ListProtocolOperations()
        {
            List<ResolutionOperationProxy> result = new List<ResolutionOperationProxy>();

            result.Add(new ResolutionOperationProxy
            {
                Code = "CreateResolution",
                Name = "Вынести постановления",
                PermissionKey = ""
            });
            result.Add(new ResolutionOperationProxy
            {
                Code = "ViewInComission",
                Name = "Рассмотрение на комиссии",
                PermissionKey = ""
            });

            return new JsonListResult(result, result.Count);
        }
    }
}