﻿namespace Bars.GkhGji.Regions.Zabaykalye.Controllers
{
    using System.Web.Mvc;

    using Bars.B4;
    using Bars.Gkh.DomainService;
    using Bars.GkhGji.Controllers;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Zabaykalye.Entities;

    public class ZabaykalyeActSurveyController : ActSurveyController<ZabaykalyeActSurvey>
    {
        public IBlobPropertyService<ActSurvey, ActSurveyLongDescription> DescriptionService { get; set; }

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