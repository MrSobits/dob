using System;
using GisGkhLibrary.Enums;
using GisGkhLibrary.Helpers;
using GisGkhLibrary.HouseManagement;
using GisGkhLibrary.NsiCommon;

namespace GisGkhLibrary.Entities.Dictionaries
{
    /// <summary>
    /// Запись справочника: Тарифицируемый ресурс
    /// </summary>
    public class RatedResource : DictionaryBase
    {
        public RatedResource() { }

        public RatedResource(NsiElementType element) : base(element)
        {
            Value = DictionaryHelper.GetStringValue(element, "Тарифицируемый ресурс");
        }

        public override DictionaryType DictionaryType => DictionaryType.RatedResource;

        public override bool Paging => false;

        /// <summary>
        /// Тарифицируемый ресурс
        /// </summary>
        public string Value { get; internal set; }

        internal ContractSubjectTypeMunicipalResource GetContractSubjectTypeMunicipalResource()
        {
            return new ContractSubjectTypeMunicipalResource
            {
                Code = Code,
                GUID = GUID.ToString()
            };
        }
    }
}
