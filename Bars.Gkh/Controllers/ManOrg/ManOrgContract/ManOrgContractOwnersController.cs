namespace Bars.Gkh.Controllers
{
    using System.Web.Mvc;

    using B4;
    using B4.Modules.FileStorage;
    using DomainService;
    using Entities;

    /// <summary>
    /// Контроллер для ManOrgContractOwners
    /// </summary>
    public class ManOrgContractOwnersController : FileStorageDataController<ManOrgContractOwners>
    {
        /// <summary>
        /// Получить информацию по жилому дому
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат запроса</returns>
        public ActionResult GetInfo(BaseParams baseParams)
        {
            var result = this.Container.Resolve<IManOrgContractOwnersService>().GetInfo(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }
    }
}