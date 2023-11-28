using System.Linq;

namespace Bars.GkhGji.DomainService
{
    using B4;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.DomainService.Contracts;
    using Entities;
    using System.Collections.Generic;

    public interface IDisposalService
    {
        DisposalInfo GetInfo(long documentId);

        IDataResult GetInfo(BaseParams baseParams);

        IDataResult ListView(BaseParams baseParams);

        IDataResult ListNullInspection(BaseParams baseParams);

        // для Сахи
        IDataResult AutoAddProvidedDocuments(BaseParams baseParams);

        IQueryable<ViewDisposal> GetViewList();

        List<IndividualPerson> GetNameList(BaseParams baseParams);

        List<RealityObject> GetAddressList(BaseParams baseParams);

        List<RealityObject> GetFactAddressList(BaseParams baseParams);
    }
}