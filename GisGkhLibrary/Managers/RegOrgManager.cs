using GisGkhLibrary.Entities;
using GisGkhLibrary.Entities.OrganizationTypes;
using GisGkhLibrary.Enums;
using GisGkhLibrary.Enums.HouseMgmt;
using GisGkhLibrary.Exceptions;
using GisGkhLibrary.RegOrgCommon;
using GisGkhLibrary.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GisGkhLibrary.Managers
{
    /// <summary>
    /// Человекоюзабельная обертка для методов импорта\экспорта организаций
    /// </summary>
    public static class RegOrgManager
    {
        /// <summary>
        /// Найти организацию по ОГРН
        /// </summary>
        /// <param name="OGRN">ОГРН</param>
        /// <param name="KPP">КПП</param>
        public static List<Organization> GetOrganization(string OGRN, string KPP)
        {
            return RegOrgCommonService.ExportOrgRegistry(null, new Dictionary<ItemsChoiceType3, string>{
                    {ItemsChoiceType3.OGRN, OGRN },
                    { ItemsChoiceType3.KPP, KPP}
                    })
                    .OfType<exportOrgRegistryResultType>()
                    .Select(x => new Organization
                    {
                        PPAGUID = Guid.Parse(x.orgPPAGUID),
                        RootGUID = Guid.Parse(x.orgRootEntityGUID),
                        IsRegistered = x.isRegisteredSpecified ? new bool?(x.isRegistered) : null,
                        organizationRoles = x.organizationRoles,
                        IsActual = x.OrgVersion.IsActual,
                        OrganizationType = GetType(x.OrgVersion.Item),
                        LastEditingDate = x.OrgVersion.lastEditingDate,
                        OrgVersionGUID = Guid.Parse(x.OrgVersion.orgVersionGUID),
                        RegistryOrganizationStatus = x.OrgVersion.registryOrganizationStatusSpecified ? (RegistryOrganizationStatusType?)RegistryOrganizationStatusType.Published : null,
                    }).ToList();
        }

        private static OrganizationTypeBase GetType(object item)
        {
            if (item is EntpsType)
            {
                var entps = (EntpsType)item;
                return new Entrp
                {
                    INN = entps.INN,
                    OGRNIP = entps.OGRNIP,
                    FirstName = entps.FirstName,
                    Surname = entps.Surname,
                    Patronymic = entps.Patronymic,
                    Sex = entps.SexSpecified ? (entps.Sex == EntpsTypeSex.M ? (Gender?)Gender.Man : (Gender?)Gender.Women) : null,
                    StateRegistrationDate = entps.StateRegistrationDate
                };
            }
            else if (item is ForeignBranchType)
            {
                var entps = (ForeignBranchType)item;
                return new ForeignBranch
                {
                    INN = entps.INN
                };
            }
            else if (item is LegalType)
            {
                var entps = (LegalType)item;
                return new Legal
                {
                    INN = entps.INN,
                    StateRegistrationDate = entps.StateRegistrationDate
                };
            }
            else if (item is exportOrgRegistryResultTypeOrgVersionSubsidiary)
            {
                var entps = (exportOrgRegistryResultTypeOrgVersionSubsidiary)item;
                return new Subsidiary
                {
                    INN = entps.INN
                };
            }
            else throw new GISGKHAnswerException($"Не найден маппинг типа {item.GetType()} в OrganizationTypeBase");
        }
    }
}
