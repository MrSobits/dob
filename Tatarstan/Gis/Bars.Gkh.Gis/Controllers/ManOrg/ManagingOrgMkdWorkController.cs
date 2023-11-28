namespace Bars.Gkh.Gis.Controllers.ManOrg
{
    using System.Web.Mvc;
    using B4;
    using DomainService.ManOrg;

    /// <summary>
    /// Контроллер для ManagingOrgMkdWork
    /// </summary>
    public class ManagingOrgMkdWorkController : BaseController
    {
        /// <summary>
        /// Добавить работы по МКД
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат выполнения</returns>
        public ActionResult AddMkdWorks(BaseParams baseParams)
        {
            var result = this.Container.Resolve<IManagingOrgMkdWorkService>().AddMkdWorks(baseParams);
            return result.Success ? new JsonNetResult() : JsonNetResult.Failure(result.Message);
        }
    }
}