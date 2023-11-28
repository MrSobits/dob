using GisGkhLibrary.Enums;
using GisGkhLibrary.Exceptions;
using GisGkhLibrary.Helpers;
using GisGkhLibrary.NsiCommon;
using System;
using System.Collections.Generic;

namespace GisGkhLibrary.Entities.Dictionaries
{
    /// <summary>
    /// Запись справочника: Связь вида коммунальной услуги, тарифицируемого ресурса и единиц измерения ставки тарифа
    /// </summary>
    public class UnitByMunicipalService : DictionaryBase
    {
        public UnitByMunicipalService() { }

        public UnitByMunicipalService(NsiElementType element) : base(element)
        {
            //ServiceTypeGuid
            var serviceType = DictionaryHelper.GetRefValue(element, "Вид коммунальной услуги");
            if (serviceType.type != DictionaryType.ServiceType)
                throw new GISGKHAnswerException($"UnitByMunicipalService: ошибка парсинга serviceType: ожидалась запись словаря {DictionaryType.ServiceType}, а пришла {serviceType.type}");
            Value = serviceType.guid;

            //Dict
            Dict = new Dictionary<Guid, List<RatedRecourceUnit>>();

            foreach (var child in element.ChildElement)
            {
                var ratedResource = DictionaryHelper.GetRefValue(child, "Тарифицируемый ресурс");
                if (ratedResource.type != DictionaryType.RatedResource)
                    throw new GISGKHAnswerException($"UnitByMunicipalService: ошибка парсинга ratedResource: ожидалась запись словаря {DictionaryType.RatedResource}, а пришла {ratedResource.type}");

                var ratedRecourceUnitList = new List<RatedRecourceUnit>();
                foreach (var subchild in child.ChildElement)
                {
                    ratedRecourceUnitList.Add(new RatedRecourceUnit {
                        Unit = DictionaryHelper.GetStringValue(subchild, "Единица измерения ставки тарифа для данного ресурса"),
                        OkeiUnit = DictionaryHelper.GetOKEIValue(subchild, "Код ОКЕИ для ЕИ")
                    });
                }
                Dict.Add(ratedResource.guid, ratedRecourceUnitList);
            }
        }

        //Вид коммунальной услуги

        public override DictionaryType DictionaryType => DictionaryType.UnitByMunicipalService;

        public override bool Paging => false;

        /// <summary>
        /// Вид коммунальной услуги
        /// </summary>
        public Guid Value { get;}

        /// <summary>
        /// Словарь тарифицируемый ресурс - единица измерения ставки тарифа
        /// </summary>
        public Dictionary<Guid, List<RatedRecourceUnit>> Dict { get; }

        public class RatedRecourceUnit
        {
            public string Unit { get; internal set; }

            public OKEI? OkeiUnit { get; internal set; }
        }
    }
}
