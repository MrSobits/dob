using GisGkhLibrary.Enums;
using GisGkhLibrary.Helpers;
using GisGkhLibrary.NsiCommon;

namespace GisGkhLibrary.Entities.Dictionaries
{
    /// <summary>
    /// Запись справочника: Тип внутренних стен
    /// </summary>
    public class InternalWallType : DictionaryBase
    {
        public InternalWallType() { }

        public InternalWallType(NsiElementType element) : base(element)
        {
            Value = DictionaryHelper.GetStringValue(element, "Тип внутренних стен");
        }

        public override DictionaryType DictionaryType => DictionaryType.InternalWallType;

        public override bool Paging => false;

        /// <summary>
        /// Тип внутренних стен
        /// </summary>
        public string Value { get; internal set; }
    }
}
