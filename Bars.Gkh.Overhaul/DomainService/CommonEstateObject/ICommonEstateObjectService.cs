namespace Bars.Gkh.Overhaul.DomainService
{
    using System.IO;

    using Bars.B4;

    public interface ICommonEstateObjectService
    {
        IDataResult AddWorks(BaseParams baseParams);

        Stream PrintReport(BaseParams baseParams);

        IDataResult ListForRealObj(BaseParams baseParams);
    }
}
