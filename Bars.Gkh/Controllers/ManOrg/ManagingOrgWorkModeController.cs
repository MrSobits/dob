﻿namespace Bars.Gkh.Controllers
{
    using System.Web.Mvc;

    using Bars.B4;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;

    public class ManagingOrgWorkModeController : B4.Alt.DataController<ManagingOrgWorkMode>
    {
        public ActionResult AddWorkMode(BaseParams baseParams)
        {
            var result = this.Container.Resolve<IManagingOrgWorkModeService>().AddWorkMode(baseParams);
            return result.Success ? new JsonNetResult() : JsonNetResult.Failure(result.Message);
        }
    }
}
