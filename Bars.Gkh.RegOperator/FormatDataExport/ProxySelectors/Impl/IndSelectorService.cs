namespace Bars.Gkh.RegOperator.FormatDataExport.ProxySelectors.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.FormatDataExport.ProxySelectors;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Сервис получения <see cref="IndProxy"/>
    /// </summary>
    public class IndSelectorService : BaseProxySelectorService<IndProxy>
    {
        /// <inheritdoc />
        protected override IDictionary<long, IndProxy> GetCache()
        {
            var individualAccountOwnerRepository = this.Container.ResolveRepository<IndividualAccountOwner>();

            using (this.Container.Using(individualAccountOwnerRepository))
            {
                var ownersId = this.SelectParams.GetAs<List<long>>("IndFilterIds");

                var individualAccountOwnerQuery = individualAccountOwnerRepository.GetAll();
                if (ownersId != null)
                {
                    individualAccountOwnerQuery = individualAccountOwnerQuery
                        .WhereContainsBulked(x => x.Id, ownersId, 5000);
                }

                return individualAccountOwnerQuery
                    .Select(x => new IndProxy
                    {
                        Id = x.Id,
                        Surname = x.Surname,
                        FirstName = x.FirstName,
                        SecondName = x.SecondName,
                        BirthDate = x.BirthDate,
                        Gender = x.Gender,
                        IdentityType = this.GetIdentityType(x.IdentityType),
                        IdentitySerial = x.IdentitySerial,
                        IdentityNumber = x.IdentityNumber,
                        DateDocumentIssuance = x.DateDocumentIssuance.HasValue && x.DateDocumentIssuance.Value.IsValid() ? x.DateDocumentIssuance : null,
                        BirthPlace = x.BirthPlace
                    })
                    .AsEnumerable()
                    .ToDictionary(x => x.Id);
            }
        }

        private string GetIdentityType(IdentityType type)
        {
            switch (type)
            {
                case IdentityType.Passport:
                    return "1";
                case IdentityType.BirthCertificate:
                    return "11";
            }

            return string.Empty;
        }
    }
}