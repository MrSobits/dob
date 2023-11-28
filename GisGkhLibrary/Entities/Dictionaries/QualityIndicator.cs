using GisGkhLibrary.Enums;
using GisGkhLibrary.Exceptions;
using GisGkhLibrary.Helpers;
using GisGkhLibrary.NsiCommon;
using System;

namespace GisGkhLibrary.Entities.Dictionaries
{
    /// <summary>
    /// Запись справочника: Показатели качества коммунальных ресурсов
    /// </summary>
    public class QualityIndicator : DictionaryBase
    {
        public QualityIndicator() { }

        public QualityIndicator(NsiElementType element) : base(element)
        {
            Value = DictionaryHelper.GetStringValue(element, "Наименование показателя качества");

            var ratedResource = DictionaryHelper.GetRefValue(element, "Ресурс, к которому относится показатель");
            if (ratedResource.type != DictionaryType.RatedResource)
                throw new GISGKHAnswerException($"QualityIndicator: ошибка парсинга ratedResource: ожидалась запись словаря {DictionaryType.RatedResource}, а пришла {ratedResource.type}");
            RatedResourceGuid = ratedResource.guid;

            ValueType = DictionaryHelper.GetStringValue(element, "Тип поля");
        }

        public override DictionaryType DictionaryType => DictionaryType.QualityIndicator;

        public override bool Paging => false;

        /// <summary>
        /// Наименование показателя качества
        /// </summary>
        public string Value { get; internal set; }

        /// <summary>
        /// Ресурс, к которому относится показатель
        /// </summary>
        public Guid RatedResourceGuid { get; internal set; }

        /// <summary>
        /// Тип поля
        /// </summary>
        public string ValueType { get; internal set; }
    }
}
