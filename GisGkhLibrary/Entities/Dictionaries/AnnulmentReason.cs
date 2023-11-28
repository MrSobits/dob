using GisGkhLibrary.Enums;
using GisGkhLibrary.NsiCommon;

namespace GisGkhLibrary.Entities.Dictionaries
{
    /// <summary>
    /// Запись справочника: Причина аннулирования
    /// </summary>
    public class AnnulmentReason : DictionaryBase
    {
        public AnnulmentReason() { }

        public AnnulmentReason(NsiElementType element) : base(element)
        {

            //Value = DictionaryHelper.GetStringValue(element, "Основание заключения договора");
            //MayUseInManagementContract = DictionaryHelper.GetBoolValue(element, "Применимо к договорам управления");
            //MayUseInSupplyContract = DictionaryHelper.GetBoolValue(element, "Применимо к договорам ресурсоснабжения");
        }

        public override DictionaryType DictionaryType => DictionaryType.AnnulmentReason;

        public override bool Paging => false;
    }
}
