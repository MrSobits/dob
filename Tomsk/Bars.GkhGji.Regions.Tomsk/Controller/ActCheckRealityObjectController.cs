﻿namespace Bars.GkhGji.Regions.Tomsk.Controller
{
    using System.Web.Mvc;

    using Bars.B4;
    using Bars.Gkh.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tomsk.DomainService;
    using Bars.GkhGji.Regions.Tomsk.Entities;

    public class ActCheckRealityObjectController : GkhGji.Controllers.ActCheckRealityObjectController
    {
        public IBlobPropertyService<ActCheckRealityObject, ActCheckRealityObjectDescription> DescriptionService { get; set; }

        public virtual ActionResult SaveDescription(BaseParams baseParams)
        {
            var result = this.DescriptionService.Save(baseParams);
            return result.Success ? new JsonGetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        public virtual ActionResult GetDescription(BaseParams baseParams)
        {
            var result = this.DescriptionService.Get(baseParams);
            return result.Success ? new JsonGetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }
    }
}