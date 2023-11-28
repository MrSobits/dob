﻿namespace Bars.GkhGji.Controllers
{
    using System.Collections;
    using System.Web.Mvc;
    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.DomainService.Dict;
    using Bars.GkhGji.Entities;

    public class ViolationGjiController : ViolationGjiController<ViolationGji>
    {
    }


    public class ViolationGjiController<T> : B4.Alt.DataController<T>
        where T : ViolationGji
    {
        public ActionResult Export(BaseParams baseParams)
        {
            var export = Container.Resolve<IDataExportService>("ViolationGjiDataExport");

            if (export != null)
            {
                return export.ExportData(baseParams);
            }

            return null;
        }
    }
 
}