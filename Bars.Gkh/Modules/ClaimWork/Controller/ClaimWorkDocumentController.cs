namespace Bars.Gkh.Modules.ClaimWork.Controllers
{
    using System.Web.Mvc;
    using B4;
    using DomainService;
    using System;

    /// <summary>
    /// Базовый контроллер для всех оснований претензеонно исковой работы
    /// </summary>
    public class ClaimWorkDocumentController : BaseController
    {
        public ActionResult CreateDocument(BaseParams baseParams)
        {
            var service = Container.Resolve<IClaimWorkDocumentProvider>();
            try
            {
                var result = service.CreateDocument(baseParams);
                return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
            }
            catch (Exception e)
            {
                Container.Release(service);
                return JsFailure(e.Message);
            }
        }
    }
}