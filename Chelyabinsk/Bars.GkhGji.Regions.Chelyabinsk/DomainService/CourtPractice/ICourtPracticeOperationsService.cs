﻿namespace Bars.GkhGji.Regions.Chelyabinsk.DomainService
{
    using Entities;
    using System.Collections;

    using B4;
    
    public interface ICourtPracticeOperationsService
    {
        IDataResult AddCourtPracticeRealityObjects(BaseParams baseParams);

        IDataResult ListDocsForSelect(BaseParams baseParams);

        IDataResult AddDocs(BaseParams baseParams);   

        IDataResult ListDocs(BaseParams baseParams);

        IDataResult GetInfo(BaseParams baseParams);
    }
}