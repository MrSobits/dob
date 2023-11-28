namespace Bars.Gkh.FormatDataExport.ProxySelectors.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.FormatDataExport.ProxyEntities;

    /// <summary>
    /// Сервис получения <see cref="UoProxy"/>
    /// </summary>
    public class UoSelectorService : BaseProxySelectorService<UoProxy>
    {
        /// <inheritdoc />
        protected override IDictionary<long, UoProxy> GetCache()
        {
            var contragentContactRepository = this.Container.ResolveRepository<ContragentContact>();
            var managingOrganizationRepository = this.Container.ResolveRepository<ManagingOrganization>();

            using (this.Container.Using(contragentContactRepository, managingOrganizationRepository))
            {
                var leaderId = this.SelectParams.GetAsId("LeaderPositionId");

                var contactDict = this.FilterService
                    .FilterByContragent(contragentContactRepository.GetAll(), x => x.Contragent)
                    .Where(x => x.Position != null && x.Position.Id == leaderId)
                    .Select(x => new
                    {
                        Id = x.Contragent.ExportId,
                        x.Phone
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id, x => x.Phone)
                    .ToDictionary(x => x.Key, x => x.First());

                return this.FilterService
                    .FilterByContragent(managingOrganizationRepository.GetAll(), x => x.Contragent)
                    .Select(x => new
                    {
                        Id = x.Contragent.ExportId,
                        x.NumberEmployees,
                        x.ShareSf,
                        x.ShareMo,
                        x.TypeManagement
                    })
                    .AsEnumerable()
                    .Select(x => new UoProxy
                    {
                        Id = x.Id,
                        LeaderPhone = contactDict.Get(x.Id),
                        AdministrativeStaffCount = x.NumberEmployees,
                        ShareSf = x.ShareSf,
                        ShareMo = x.ShareMo,
                        IsTsj = x.TypeManagement == TypeManagementManOrg.TSJ || x.TypeManagement == TypeManagementManOrg.JSK
                            ? 1
                            : 2
                    })
                    .ToDictionary(x => x.Id);
            }
        }
    }
}