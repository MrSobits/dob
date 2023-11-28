namespace Bars.GkhGji.Regions.BaseChelyabinsk.DomainService
{
    using Bars.B4;

    public interface ITaskCalendarService
    {
        IDataResult GetDays(BaseParams baseParams);
        IDataResult GetListDisposal(BaseParams baseParams);
        IDataResult GetListProtocolsGji(BaseParams baseParams);
        IDataResult GetListProtocolsInCommission(BaseParams baseParams);
        IDataResult GetListResolutionsInCommission(BaseParams baseParams);
        IDataResult GetListResolutionDefinitionsInCommission(BaseParams baseParams);
        IDataResult GetListListComissionsInDocument(BaseParams baseParams);
        IDataResult GetListCourtPractice(BaseParams baseParams);
    }
}