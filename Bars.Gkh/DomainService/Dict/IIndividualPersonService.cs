namespace Bars.Gkh.DomainService
{
    using Bars.B4;
    using Bars.Gkh.Entities;

    public interface IIndividualPersonService
    {

        IDataResult GetInfo(BaseParams baseParams);
        
    }
}