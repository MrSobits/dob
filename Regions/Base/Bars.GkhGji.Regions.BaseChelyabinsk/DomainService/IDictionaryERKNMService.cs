namespace Bars.GkhGji.Regions.BaseChelyabinsk.DomainService
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities;

    public interface IDictionaryERKNMService
    {
		IDataResult ListView(BaseParams baseParams);	
		IDataResult GetInfo(long? documentId);
		IDataResult AddRequirements(BaseParams baseParams);
		IDataResult AddDirections(BaseParams baseParams);
    }
}