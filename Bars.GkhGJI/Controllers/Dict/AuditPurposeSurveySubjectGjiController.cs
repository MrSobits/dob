namespace Bars.GkhGji.Controllers
{
    using System.Web.Mvc;
    using B4;
    using DomainService;
    using Entities.Dict;

    public class AuditPurposeSurveySubjectGjiController : B4.Alt.DataController<AuditPurposeSurveySubjectGji>
    {
        public ActionResult AddAuditPurposeSurveySubjectGji(BaseParams baseParams)
        {
            var service = Container.Resolve<IAuditPurposeSurveySubjectGjiService>();

            try
            {
                var result = service.AddAuditPurposeSurveySubjectGji(baseParams);

                return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
            }
            finally
            {
                Container.Release(service);
            }
        }
    }

}