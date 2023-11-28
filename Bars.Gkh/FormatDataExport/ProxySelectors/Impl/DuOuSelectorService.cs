namespace Bars.Gkh.FormatDataExport.ProxySelectors.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.Gkh.FormatDataExport.ProxyEntities;

    /// <summary>
    /// Сервис получения <see cref="DuOuProxy"/>
    /// </summary>
    public class DuOuSelectorService : BaseProxySelectorService<DuOuProxy>
    {
        /// <inheritdoc />
        protected override IDictionary<long, DuOuProxy> GetCache()
        {
            return this.ProxySelectorFactory.GetSelector<DuProxy>()
                .ProxyListCache.Values
                .Select(x => new DuOuProxy
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