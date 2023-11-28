using GisGkhLibrary.Enums;
using GisGkhLibrary.Helpers;
using GisGkhLibrary.NsiCommon;

namespace GisGkhLibrary.Entities.Dictionaries
{
    /// <summary>
    /// Запись справочника: Документ, удостоверяющий личность
    /// </summary>
    public class IdentifierType : DictionaryBase
    {
        public IdentifierType() { }

        public IdentifierType(NsiElementType element) : base(element)
        {
            Value = DictionaryHelper.GetStringValue(element, "Вид документа, удостоверяющего личность");
        }

        public override DictionaryType DictionaryType => DictionaryType.IdentifierType;

        public override bool Paging => false;

        /// <summary>
        /// Документ, удостоверяющий личность
        /// </summary>
        public string Value { get; internal set; }
    }
}
