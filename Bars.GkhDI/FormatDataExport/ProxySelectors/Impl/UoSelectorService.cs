namespace Bars.GkhDi.FormatDataExport.ProxySelectors.Impl
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
    using Bars.Gkh.FormatDataExport.ProxySelectors;
    using Bars.GkhDi.Entities;

    using NHibernate.Criterion;

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
            var disclosureInfoRepository = this.Container.ResolveRepository<DisclosureInfo>();

            using (this.Container.Using(contragentContactRepository, managingOrganizationRepository, disclosureInfoRepository))
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

                var disclosureInfoDict = this.FilterService
                    .FilterByContragent(disclosureInfoRepository.GetAll(), x => x.ManagingOrganization.Contragent)
                    .Select(x => new
                    {
                        Id = x.ManagingOrganization.Contragent.ExportId,
                        x.AdminPersonnel,
                        x.Engineer,
                        x.Work,
                        x.PeriodDi.DateStart
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, x => x.OrderByDescending(d => d.DateStart).First());

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
                    .Select(x =>
                    {
                        var diInfo = disclosureInfoDict.Get(x.Id);
                        return new UoProxy
                        {
                            Id = x.Id,
                            LeaderPhone = contactDict.Get(x.Id),
                            AdministrativeStaffCount = diInfo?.AdminPersonnel,
                            EngineersCount = diInfo?.Engineer,
                            EmployeesCount = diInfo?.Work,
                            ShareSf = x.ShareSf,
                            ShareMo = x.ShareMo,
                            IsTsj = x.TypeManagement == TypeManagementManOrg.TSJ || x.TypeManagement == TypeManagementManOrg.JSK
                                ? 1
                                : 2
                        };
                    })
                    .ToDictionary(x => x.Id);
            }
        }
    }
}