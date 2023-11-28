using GisGkhLibrary.Enums;
using GisGkhLibrary.NsiCommon;

namespace GisGkhLibrary.Entities.Dictionaries
{
    /// <summary>
    /// Запись справочника: Характеристика помещения
    /// </summary>
    public class ResidentPremiseType : DictionaryBase
    {
        public ResidentPremiseType() { }

        public ResidentPremiseType(NsiElementType element) : base(element)
        {

            //Value = DictionaryHelper.GetStringValue(element, "Основание заключения договора");
            //MayUseInManagementContract = DictionaryHelper.GetBoolValue(element, "Применимо к договорам управления");
            //MayUseInSupplyContract = DictionaryHelper.GetBoolValue(element, "Применимо к договорам ресурсоснабжения");
        }

        public override DictionaryType DictionaryType => DictionaryType.ResidentPremiseType;

        public override bool Paging => false;
    }
}
