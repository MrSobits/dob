using Bars.B4;

namespace Bars.GkhGji.Regions.Voronezh.DomainService
{
    /// <summary>
    /// Сервис работы BDOPER
    /// </summary>
    public interface IBDOPERService
    {
        IDataResult AddPayFines(BaseParams baseParams);

        IDataResult GetResolution(BaseParams baseParams);

        IDataResult GetListResolutionsForSelect(BaseParams baseParams);
    }
}
