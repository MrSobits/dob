namespace Bars.Gkh.RegOperator.FormatDataExport.ProxySelectors.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.FormatDataExport.ProxySelectors;

    /// <summary>
    /// Селектор Расчетные счета фонда капитального ремонта
    /// </summary>
    public class RegopSchetSelectorService : BaseProxySelectorService<RegopSchetProxy>
    {
        /// <inheritdoc />
        protected override IDictionary<long, RegopSchetProxy> GetCache()
        {
            return this.ProxySelectorFactory.GetSelector<ContragentRschetProxy>()
                .ProxyListCache
                .Values
                .Where(x => x.IsRegopAccount)
                .Select(x => new RegopSchetProxy
                {
                    Id = x.Id,
                    TypeAccount = x.RegopAccountType,
                    ContragentId = x.ContragentId,
                    ContragentRschetId = x.Id,
                    Status = x.CloseDate.HasValue ? 2 : 1,
                })
                .ToDictionary(x => x.Id);
        }
    }
}