using System;

namespace GisGkhLibrary.Entities.HouseMgmt.Person
{
    public class JuridicalPerson : PersonBase
    {
        /// <summary>
        /// Идентификатор корневой сущности организации в реестре организаций
        /// </summary>
        public Guid OrgRootEntityGUID { get; internal set; }
    }
}
