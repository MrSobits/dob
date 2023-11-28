namespace Bars.Gkh.FormatDataExport.ProxySelectors.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Сервис получения <see cref="UstavOuProxy"/>
    /// </summary>
    public class UstavOuSelectorService : BaseProxySelectorService<UstavOuProxy>
    {
        /// <inheritdoc />
        protected override IDictionary<long, UstavOuProxy> GetCache()
        {
            return this.ProxySelectorFactory.GetSelector<UstavProxy>()
                .ProxyListCache.Values
                .Select(x => new UstavOuProxy
                {
                    Id = x.Id,
                    ContractId = x.Id,
                    RealityObjectId = x.RealityObjectId,
                    StartDate = x.StartDate,
                    EndDate = x.TerminationDate,
                    IsContractReason = 1
                })
                .ToDictionary(x => x.Id);
        }
    }
}