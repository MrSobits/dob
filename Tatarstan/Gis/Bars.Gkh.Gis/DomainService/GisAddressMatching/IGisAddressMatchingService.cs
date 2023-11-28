namespace Bars.Gkh.Gis.DomainService.GisAddressMatching
{
    using System.Linq;
    using B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FIAS;

    public interface IGisAddressMatchingService
    {
        IDataResult ManualMathAddress(BaseParams baseParams);
        IDataResult ManualBillingAddressMatch(BaseParams baseParams);
        IDataResult MassManualMathAddress(BaseParams baseParams);
        IDataResult DetectSimilarAddresses(BaseParams baseParams);
        Fias GetFiasByLevel(string guid, FiasLevelEnum level, IDomainService<Fias> fiasRepository);
    }
}