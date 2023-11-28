using GisGkhLibrary.Entities.Dictionaries;

namespace GisGkhLibrary.Entities.HouseMgmt
{
    /// <summary>
    /// Показатель качества (содержащийся в справочнике показателей качества). 
    /// </summary>
    public class Quality
    {
        /// <summary>
        /// Ссылка на ОЖФ, обязательно заполняется, если показатели качества ведутся в разрезе ОЖФ
        /// </summary>
        public string AddressObjectKey { get; internal set; }

        /// <summary>
        /// Ссылка на пару из коммунальной услуги и ресурса из предмета договора
        /// </summary>
        public string PairKey { get; internal set; }

        /// <summary>
        /// Показатель качества
        /// </summary>
        public QualityIndicator QualityIndicator { get; internal set; }

        /// <summary>
        /// Значение показателя качества
        /// </summary>
        public IndicatorValue IndicatorValue { get; internal set; }

        /// <summary>
        /// Дополнительная информация
        /// </summary>
        public string AdditionalInformation { get; internal set; }
    }
}
