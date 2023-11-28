using GisGkhLibrary.Entities.OrganizationTypes;
using GisGkhLibrary.Enums.HouseMgmt;
using GisGkhLibrary.RegOrgCommon;
using System;

namespace GisGkhLibrary.Entities
{
    public class Organization
    {
        /// <summary>
        /// Идентификатор корневой сущности организации в реестре организаций
        /// </summary>
        public Guid RootGUID { get; set; }

        /// <summary>
        /// Идентификатор зарегистрированной организации
        /// </summary>
        public Guid PPAGUID { get; internal set; }

        /// <summary>
        /// Зарегистрирована в ГИС ЖКХ
        /// </summary>
        public bool? IsRegistered { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public nsiRef[] organizationRoles { get; set; }

        /// <summary>
        /// Признак актуальности записи
        /// </summary>
        public bool IsActual { get; set; }

        /// <summary>
        /// Тип организации
        /// </summary>
        public OrganizationTypeBase OrganizationType { get; set; }

        /// <summary>
        /// Время последнего изменения
        /// </summary>
        public DateTime LastEditingDate { get; set; }

        /// <summary>
        /// Идентификатор версии записи в реестре организаций
        /// </summary>
        public Guid OrgVersionGUID { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public RegistryOrganizationStatusType? RegistryOrganizationStatus { get; set; }
    }
}
