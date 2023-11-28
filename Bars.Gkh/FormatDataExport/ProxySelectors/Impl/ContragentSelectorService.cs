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
    using Bars.Gkh.Utils;

    /// <summary>
    /// Сервис получения <see cref="ContragentProxy"/>
    /// </summary>
    public class ContragentSelectorService : BaseProxySelectorService<ContragentProxy>
    {
        /// <inheritdoc />
        protected override IDictionary<long, ContragentProxy> GetCache()
        {
            var contragentRepository = this.Container.ResolveRepository<Contragent>();
            var contragentContactRepository = this.Container.ResolveRepository<ContragentContact>();
            
            using (this.Container.Using(contragentRepository, contragentContactRepository))
            {
                var query = this.FilterService.FilterByContragent(contragentRepository.GetAll());
                var contactQuery = this.FilterService
                    .FilterByContragent(contragentContactRepository.GetAll(), x => x.Contragent);

                return this.GetProxies(query, contactQuery)
                    .ToDictionary(x => x.Id);
            }
        }

        /// <inheritdoc />
        protected override ICollection<ContragentProxy> GetAdditionalCache()
        {
            var contragentRepository = this.Container.ResolveRepository<Contragent>();
            var contragentContactRepository = this.Container.ResolveRepository<ContragentContact>();

            using (this.Container.Using(contragentRepository, contragentContactRepository))
            {
                var query = contragentRepository.GetAll()
                    .WhereContainsBulked(x => x.ExportId, this.additionalIds);
                var contactQuery = contragentContactRepository.GetAll()
                    .WhereContainsBulked(x => x.Contragent.ExportId, this.additionalIds);

                return this.GetProxies(query, contactQuery).ToList();
            }
        }

        protected virtual IEnumerable<ContragentProxy> GetProxies(IQueryable<Contragent> contragentQuery, IQueryable<ContragentContact> contactQuery)
        {
            var leaderId = this.SelectParams.GetAsId("LeaderPositionId");
            var contactContragentDict = contactQuery
                .WhereNotNull(x => x.Contragent)
                .WhereNotNull(x => x.Position)
                .Where(x => x.Position.Id == leaderId)
                .AsEnumerable()
                .GroupBy(x => x.Contragent.Id)
                .ToDictionary(x => x.Key, x => x.FirstOrDefault());

            return contragentQuery
                .Select(x => new
                {
                    x.Id,
                    x.ExportId,
                    OrganizationFormCode = x.OrganizationForm.Code,
                    x.Name,
                    x.ShortName,
                    LegalFiasAddress = x.FiasJuridicalAddress.HouseGuid,
                    LegalAddress = x.FiasJuridicalAddress.AddressName,
                    ActualFiasAddress = x.FiasFactAddress.HouseGuid,
                    ActualAddress = x.FiasFactAddress.AddressName,
                    PostFiasAddress = x.FiasMailingAddress.HouseGuid,
                    PostAddress = x.FiasMailingAddress.AddressName,
                    x.Ogrn,
                    x.Inn,
                    x.Kpp,
                    x.OgrnRegistration,
                    x.DateRegistration,
                    x.OrganizationForm.OkopfCode,
                    x.DateTermination,
                    IsActive = x.ContragentState == ContragentState.Active,
                    x.OfficialWebsite,
                    x.Phone,
                    x.Fax,
                    x.Email,
                    ParentId = this.GetId(x.Parent),
                    x.Oktmo
                })
                .AsEnumerable()
                .Select(x => new ContragentProxy
                {
                    Id = x.ExportId,
                    ContragentId = x.Id,
                    Type = this.GetContragentType(x.OrganizationFormCode),
                    FullName = x.Name,
                    ShortName = x.ShortName,
                    LegalFiasAddress = x.LegalFiasAddress.ToStr(),
                    LegalAddress = x.LegalAddress,
                    ActualFiasAddress = x.ActualFiasAddress.ToStr(),
                    ActualAddress = x.ActualAddress,
                    PostFiasAddress = x.PostFiasAddress.ToStr(),
                    PostAddress = x.PostAddress,
                    Ogrn = x.Ogrn,
                    Inn = x.Inn,
                    Kpp = x.Kpp,
                    Registrator = x.OgrnRegistration,
                    RegistrationDate = x.DateRegistration,
                    Okopf = x.OkopfCode,
                    TerminationDate = x.DateTermination,
                    IsActive = x.IsActive,
                    WebSite = x.OfficialWebsite,
                    Contact = contactContragentDict.Get(x.Id),
                    Phone = x.Phone,
                    Fax = x.Fax,
                    Email = x.Email,
                    ParentId = x.ParentId,
                    Oktmo = x.Oktmo
                });
        }

        protected int GetContragentType(string organizationFormCode)
        {
            switch (organizationFormCode.ToInt(0))
            {
                case 90:
                    return 2;
                case 91:
                    return 3;
                case 102:
                    return 4;
                default:
                    return 1;
            }
        }
    }
}