namespace Bars.GkhGji.Controllers
{
    using System.Web.Mvc;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;

    public class AppealCitsStatSubjectController : B4.Alt.DataController<AppealCitsStatSubject>
    {
        public ActionResult AddStatementSubject(BaseParams baseParams)
        {
            var result = Container.Resolve<IAppealCitsStatSubjectService>().AddStatementSubject(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }
    }
}