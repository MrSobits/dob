using GisGkhLibrary.Enums;
using GisGkhLibrary.Helpers;
using GisGkhLibrary.NsiCommon;

namespace GisGkhLibrary.Entities.Dictionaries
{
    /// <summary>
    /// Запись справочника: Основание заключения договора
    /// </summary>
    public class ContractConclusionReason : DictionaryBase
    {
        public ContractConclusionReason() { }

        public ContractConclusionReason(NsiElementType element) : base(element)
        {
            Value = DictionaryHelper.GetStringValue(element, "Основание заключения договора");
            MayUseInManagementContract = DictionaryHelper.GetBoolValue(element, "Применимо к договорам управления");
            MayUseInSupplyContract = DictionaryHelper.GetBoolValue(element, "Применимо к договорам ресурсоснабжения");
        }

        public override DictionaryType DictionaryType => DictionaryType.ContractConclusionReason;

        public override bool Paging => false;

        /// <summary>
        /// Основание заключения договора
        /// </summary>
        public string Value { get; internal set; }

        /// <summary>
        /// Применимо к договорам управления
        /// </summary>
        public bool? MayUseInManagementContract { get; internal set; }

        /// <summary>
        /// Применимо к договорам ресурсоснабжения
        /// </summary>
        public bool? MayUseInSupplyContract { get; internal set; }
    }
}
