namespace Bars.Gkh.Controllers
{
    using System.Web.Mvc;

    using B4;
    using B4.Modules.FileStorage;
    using DomainService;
    using Entities;

    public class ManOrgContractTransferController : FileStorageDataController<ManOrgContractTransfer>
    {
        public ActionResult GetInfo(BaseParams baseParams)
        {
            var result = Container.Resolve<IManOrgContractTransferService>().GetInfo(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }
    }
}