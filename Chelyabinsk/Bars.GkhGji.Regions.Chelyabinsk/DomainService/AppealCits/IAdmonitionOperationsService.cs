namespace Bars.GkhGji.Regions.Chelyabinsk.DomainService
{
    using Entities;
    using System.Collections;

    using B4;
    
    public interface IAdmonitionOperationsService
    {
        IDataResult ListDocsForSelect(BaseParams baseParams);
    }
}