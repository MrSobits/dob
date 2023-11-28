namespace Bars.GkhGji.Regions.BaseChelyabinsk.DomainService
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Views;

    public interface IProtocol197Service
    {
		IDataResult ListView(BaseParams baseParams);
		IQueryable<ViewProtocol197> GetViewList();
		IDataResult GetInfo(long? documentId);
        IDataResult GetRepeatInfo(long? documentId);
        IDataResult AddRequirements(BaseParams baseParams);
		IDataResult AddDirections(BaseParams baseParams);
        IDataResult DoMassAddition(BaseParams baseParams);
        object GetFiasProperty(BaseParams baseParams);

        IDataResult UpdateComissionDocumentState(BaseParams baseParams);
        IDataResult UpdateComissionDocumentsState(BaseParams baseParams);

        List<IndividualPerson> GetNameList(BaseParams baseParams);
    }
}