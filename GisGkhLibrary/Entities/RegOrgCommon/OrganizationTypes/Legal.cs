using System;

namespace GisGkhLibrary.Entities.OrganizationTypes
{
    /// <summary>
    /// Юридическое лицо
    /// </summary>
    public class Legal : OrganizationTypeBase
    {
        public DateTime? StateRegistrationDate { get; internal set; }
    }
}
