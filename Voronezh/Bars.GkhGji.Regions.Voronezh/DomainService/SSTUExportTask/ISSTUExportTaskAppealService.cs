﻿namespace Bars.GkhGji.Regions.Voronezh.DomainService
{
    using System.Collections;
    using System.Web.Mvc;
    using B4;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.GkhGji.Entities;

    public interface ISSTUExportTaskAppealService
    {
        IDataResult GetListExportableAppeals(BaseParams baseParams, bool isPaging, out int totalCount);
        IDataResult AddAppeal(BaseParams baseParams);
        IDataResult ActualizeSopr(BaseParams baseParams);
    }
}