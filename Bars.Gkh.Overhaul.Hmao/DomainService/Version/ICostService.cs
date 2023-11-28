using Bars.Gkh.Entities;
using Bars.Gkh.Entities.CommonEstateObject;
using Bars.Gkh.Entities.Dicts;
using Bars.Gkh.Enums;
using Bars.Gkh.Overhaul.Hmao.Entities;

namespace Bars.Gkh.Overhaul.Hmao.DomainService.Version
{
    public interface ICostService
    {
        decimal? GetCost(RealityObject house, Work work, short yearRepair, decimal volume);

        decimal? GetCost(RealityObject house, CommonEstateObject ooi, short yearRepair, decimal volume, PriceCalculateBy calcBy);
        
        /// <summary>
        /// Пересчитать стоимости всех элементов версии
        /// </summary>
        void CalculateVersion(ProgramVersion version);
    }
}
