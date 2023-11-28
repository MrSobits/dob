namespace Bars.Gkh.Regions.Tatarstan.FormatDataExport.ProxySelectors.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.FormatDataExport.ProxySelectors;
    using Bars.Gkh.Regions.Tatarstan.Entities.ContractService;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Сервис получения <see cref="DuOuUslugaProxy"/>
    /// </summary>
    public class DuOuUslugaSelectorService : BaseProxySelectorService<DuOuUslugaProxy>
    {
        /// <inheritdoc />
        protected override IDictionary<long, DuOuUslugaProxy> GetCache()
        {
            var ustavDict = this.ProxySelectorFactory.GetSelector<DuProxy>().ProxyListCache;

            var manOrgContractService = this.Container.ResolveRepository<ManOrgContractService>();

            using (this.Container.Using(manOrgContractService))
            {
                return manOrgContractService.GetAll()
                    .WhereContainsBulked(x => x.Contract.Id, ustavDict.Keys, 5000)
                    .Select(x => new
                    {
                        x.Id,
                        ContractId = x.Contract.Id,
                        x.StartDate,
                        x.EndDate,
                        ServiceId = x.Service.ExportId
                    })
                    .AsEnumerable()
                    .Select(x => new DuOuUslugaProxy
                    {
                        Id = x.Id,
                        OuId = x.ContractId,
                        ServiceId = x.ServiceId,
                        StartDate = x.StartDate,
                        EndDate = x.EndDate
                    })
                    .ToDictionary(x => x.Id);
            }
        }
    }
}