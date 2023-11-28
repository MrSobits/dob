using GisGkhLibrary.Enums;
using GisGkhLibrary.Helpers;
using GisGkhLibrary.NsiCommon;

namespace GisGkhLibrary.Entities.Dictionaries
{
    /// <summary>
    /// Запись справочника: Полномочие организации
    /// </summary>
    public class OrganizationRole : DictionaryBase
    {
        public OrganizationRole() { }

        public OrganizationRole(NsiElementType element) : base(element)
        {
            Value = DictionaryHelper.GetStringValue(element, "Полномочие организации");
            OrgName = DictionaryHelper.GetStringValue(element, "Краткое наименование");
        }

        public override DictionaryType DictionaryType => DictionaryType.OrganizationRole;

        public override bool Paging => false;

        /// <summary>
        /// Полномочие организации
        /// </summary>
        public string Value { get; internal set; }

        /// <summary>
        /// Краткое наименование
        /// </summary>
        public string OrgName { get; internal set; }        
    }
}
