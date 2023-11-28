﻿namespace Bars.GkhCr.DomainService
{
    using B4;

    public interface IBuildContractService
    {
        IDataResult AddTypeWorks(BaseParams baseParams);

        IDataResult ListAvailableBuilders(BaseParams baseParams);
        IDataResult GetForMap(BaseParams baseParams);
    }
}
