using System;

namespace GisGkhLibrary.Entities.Owners
{
    /// <summary>
    /// Управляющая организация
    /// </summary>
    public class OrganizationOwner : OwnerBase
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid OrgRootEntityGUID { get; internal set; }
    }
}
