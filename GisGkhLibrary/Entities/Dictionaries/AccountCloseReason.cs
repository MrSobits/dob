using GisGkhLibrary.Enums;
using GisGkhLibrary.Helpers;
using GisGkhLibrary.NsiCommon;

namespace GisGkhLibrary.Entities.Dictionaries
{
    /// <summary>
    /// Запись справочника: Причина закрытия лицевого счета
    /// </summary>
    public class AccountCloseReason : DictionaryBase
    {
        public AccountCloseReason() { }

        public AccountCloseReason(NsiElementType element) : base(element)
        {
            Value = DictionaryHelper.GetStringValue(element, "Основание заключения договора");
            //MayUseInManagementContract = DictionaryHelper.GetBoolValue(element, "Применимо к договорам управления");
            //MayUseInSupplyContract = DictionaryHelper.GetBoolValue(element, "Применимо к договорам ресурсоснабжения");
        }

        public override DictionaryType DictionaryType => DictionaryType.AccountCloseReason;

        public override bool Paging => false;

        /// <summary>
        /// Причина закрытия лицевого счета
        /// </summary>
        public string Value { get; internal set; }
    }
}
