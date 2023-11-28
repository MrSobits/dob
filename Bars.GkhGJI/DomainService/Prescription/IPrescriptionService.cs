namespace Bars.GkhGji.DomainService
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;

    public interface IPrescriptionService
    {
        IDataResult GetInfo(long? documentId);

        IDataResult ListView(BaseParams baseParams);

        IDataResult ListForStage(BaseParams baseParams);

        IQueryable<ViewPrescription> GetViewList();
    }
}