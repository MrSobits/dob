using GisGkhLibrary.Enums;
using GisGkhLibrary.Helpers;
using GisGkhLibrary.NsiCommon;

namespace GisGkhLibrary.Entities.Dictionaries
{
    /// <summary>
    /// Запись справочника: Причина расторжения договора
    /// </summary>
    public class ContractTerminationReason : DictionaryBase
    {
        public ContractTerminationReason() { }

        public ContractTerminationReason(NsiElementType element): base(element)
        {
            Value = DictionaryHelper.GetStringValue(element, "Причина расторжения договора");
        }

        public override DictionaryType DictionaryType => DictionaryType.ContractTerminationReason;

        public override bool Paging => false;

        /// <summary>
        /// Причина расторжения договора
        /// </summary>
        public string Value { get; internal set; }
    }
}
