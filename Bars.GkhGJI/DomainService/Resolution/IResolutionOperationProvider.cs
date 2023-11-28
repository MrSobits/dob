namespace Bars.GkhGji.DomainService
{
    using System.Collections.Generic;
    using B4;

    public interface IResolutionOperationProvider
    {
        List<ResolutionOperationProxy> GetAllOperations();

        IDataResult Execute(BaseParams baseParams);

        IDataResult ChangeSumAmount(BaseParams baseParams);

        IDataResult CreateProtocols(BaseParams baseParams);

        IDataResult ChangeSentToOSP(BaseParams baseParams);

        IDataResult ChangeOSP(BaseParams baseParams);
    }
}