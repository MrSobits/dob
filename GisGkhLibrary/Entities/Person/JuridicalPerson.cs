using System;

namespace GisGkhLibrary.Entities.Person
{
    public class JuridicalPerson : PersonBase
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid OrgRootEntityGUID { get; internal set; }
    }
}
