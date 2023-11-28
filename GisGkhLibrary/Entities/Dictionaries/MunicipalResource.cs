using GisGkhLibrary.Enums;
using GisGkhLibrary.Helpers;
using GisGkhLibrary.NsiCommon;

namespace GisGkhLibrary.Entities.Dictionaries
{
    /// <summary>
    /// Запись справочника: Вид коммунального ресурса
    /// </summary>
    public class MunicipalResource : DictionaryBase
    {
        public MunicipalResource() { }

        public MunicipalResource(NsiElementType element) : base(element)
        {
            ShortName = DictionaryHelper.GetStringValue(element, "Сокращенное наименование");
            Unit = DictionaryHelper.GetOKEIValue(element, "Единица измерения");
            Value = DictionaryHelper.GetStringValue(element, "Вид коммунального ресурса");
            MeterConnect = DictionaryHelper.GetBoolValue(element, "Признак возможности установки связи с прибором учета");
            Sorting = DictionaryHelper.GetStringValue(element, "Порядок сортировки");
        }

        public override DictionaryType DictionaryType => DictionaryType.MunicipalResource;

        public override bool Paging => false;

        /// <summary>
        /// Сокращенное наименование
        /// </summary>
        public string ShortName { get; }

        /// <summary>
        /// Единица измерения
        /// </summary>
        public OKEI? Unit { get; }

        /// <summary>
        /// Вид коммунального ресурса
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Признак возможности установки связи с прибором учета
        /// </summary>
        public bool? MeterConnect { get; }

        /// <summary>
        /// Порядок сортировки
        /// </summary>
        public string Sorting { get; }
    }
}
