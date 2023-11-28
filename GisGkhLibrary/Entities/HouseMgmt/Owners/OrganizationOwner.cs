using System;

namespace GisGkhLibrary.Entities.HouseMgmt.Owners
{
    /// <summary>
    /// Управляющая организация
    /// </summary>
    public class OrganizationOwner : OwnerBase
    {
        /// <summary>
        /// Идентификатор корневой сущности организации в реестре организаций
        /// </summary>
        public Guid OrgRootEntityGUID { get; internal set; }
    }
}
