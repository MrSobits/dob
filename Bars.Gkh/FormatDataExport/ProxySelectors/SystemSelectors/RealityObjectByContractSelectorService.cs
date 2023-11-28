namespace Bars.Gkh.FormatDataExport.ProxySelectors.SystemSelectors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.DataModels;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Сервис получения идентификаторов домов по идентификатору договора
    /// </summary>
    public class RealityObjectByContractSelectorService : BaseProxySelectorService<RealityObjectByContract>
    {
        /// <inheritdoc />
        protected override IDictionary<long, RealityObjectByContract> GetCache()
        {
            var contractRealityObjectRepository = this.Container.ResolveRepository<ManOrgContractRealityObject>();

            using (this.Container.Using(contractRealityObjectRepository))
            {
                var roIds = this.ProxySelectorFactory.GetSelector<DomProxy>().ProxyListCache.Keys;

                return this.FilterService
                    .FilterByContragent(
                        contractRealityObjectRepository.GetAll(),
                        x => x.ManOrgContract.ManagingOrganization.Contragent)
                    .WhereContainsBulked(x => x.RealityObject.Id, roIds)
                    .Select(
                        x => new
                        {
                            Id = x.ManOrgContract.Id,
                            RealityObjectId = x.RealityObject.Id,
                            x.ManOrgContract.EndDate,
                        })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(
                        x => x.Key,
                        x => x.OrderByDescending(z => z.EndDate ?? DateTime.MaxValue)
                            .Select(
                                z => new RealityObjectByContract
                                {
                                    Id = z.Id,
                                    RealityObjectId = z.RealityObjectId
                                }).First());
            }
        }
    }

    public class RealityObjectByContract : IHaveId
    {

        /// <summary>
        /// Идентификатор договора <see cref="ManOrgBaseContract"/>
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Идентификатор жилого дома <see cref="RealityObject"/>
        /// </summary>
        public long RealityObjectId { get; set; }
    }
}