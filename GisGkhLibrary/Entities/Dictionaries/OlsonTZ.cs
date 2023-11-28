using GisGkhLibrary.Enums;
using GisGkhLibrary.NsiCommon;

namespace GisGkhLibrary.Entities.Dictionaries
{
    /// <summary>
    /// Запись справочника: Часовые зоны по Olson
    /// </summary>
    public class OlsonTZ : DictionaryBase
    {
        public OlsonTZ() { }

        public OlsonTZ(NsiElementType element) : base(element)
        {

            //Value = DictionaryHelper.GetStringValue(element, "Основание заключения договора");
            //MayUseInManagementContract = DictionaryHelper.GetBoolValue(element, "Применимо к договорам управления");
            //MayUseInSupplyContract = DictionaryHelper.GetBoolValue(element, "Применимо к договорам ресурсоснабжения");
        }

        public override DictionaryType DictionaryType => DictionaryType.OlsonTZ;

        public override bool Paging => false;
    }
}
