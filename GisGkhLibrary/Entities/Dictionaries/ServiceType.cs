using GisGkhLibrary.Enums;
using GisGkhLibrary.Exceptions;
using GisGkhLibrary.Helpers;
using GisGkhLibrary.HouseManagement;
using GisGkhLibrary.NsiCommon;
using System;

namespace GisGkhLibrary.Entities.Dictionaries
{
    /// <summary>
    /// Запись справочника: Вид коммунальной услуги
    /// </summary>
    public class ServiceType : DictionaryBase
    {
        public ServiceType() { }

        public ServiceType(NsiElementType element) : base(element)
        {
            Value = DictionaryHelper.GetStringValue(element, "Вид коммунальной услуги");
            Unit = DictionaryHelper.GetOKEIValue(element, "Единица измерения");
            PowerUnit = DictionaryHelper.GetOKEIValue(element, "Единица измерения мощности и присоединенной нагрузки");

            var municipalResource = DictionaryHelper.GetRefValue(element, "Вид коммунального ресурса для ОКИ");
            if (municipalResource.type != DictionaryType.MunicipalResource)
                throw new GISGKHAnswerException($"ServiceType: ошибка парсинга municipalResource: ожидалась запись словаря {DictionaryType.MunicipalResource}, а пришла {municipalResource.type}");
            MunicipalResourceGuid = municipalResource.guid;

            Sorting = DictionaryHelper.GetStringValue(element, "Порядок сортировки");
        }

        public override DictionaryType DictionaryType => DictionaryType.ServiceType;

        public override bool Paging => false;

        /// <summary>
        /// Вид коммунальной услуги
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Единица измерения
        /// </summary>
        public OKEI? Unit { get; }

        /// <summary>
        /// Единица измерения мощности и присоединенной нагрузки
        /// </summary>
        public OKEI? PowerUnit { get; }

        /// <summary>
        /// Вид коммунального ресурса для ОКИ
        /// </summary>
        public Guid MunicipalResourceGuid { get;}

        /// <summary>
        /// Порядок сортировки
        /// </summary>
        public string Sorting { get; }

        internal ContractSubjectTypeServiceType GetContractSubjectTypeServiceType()
        {
            return new ContractSubjectTypeServiceType
            {
                Code = Code,
                GUID = GUID.ToString()
            };
        }
    }
}
