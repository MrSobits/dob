using System;

namespace GisGkhLibrary.Entities.HouseMgmt
{
    /// <summary>
    /// Иной показатель качества коммунального ресурса (не содержащийся в справочнике показателей качества). 
    /// </summary>
    public class OtherQualityIndicator
    {      
        /// <summary>
        /// Ссылка на ОЖФ, обязательно заполняется, если показатели качества ведутся в разрезе ОЖФ
        /// </summary>
        public Guid AddressObjectKey { get; set; }

        /// <summary>
        /// Ссылка на пару из коммунальной услуги и ресурса из предмета договора
        /// </summary>
        public Guid PairKey { get; set; }

        /// <summary>
        /// Наименование показателя (макс. 1000 символов)
        /// </summary>
        public string IndicatorName { get; set; }

        /// <summary>
        /// Дополнительная информация (макс. 500 символов)
        /// </summary>
        public string AdditionalInformation { get; set; }

        //--необязательные--

        /// <summary>
        /// Код ОКЕИ формата A{0,1}\d{3,4}
        /// </summary>
        public string OKEI { get; set; }

        /// <summary>
        /// Значение показателя соответствует
        /// </summary>
        public bool? Correspond { get; set; }

        /// <summary>
        /// Число
        /// </summary>
        public decimal? Number { get; set; }

        /// <summary>
        /// Начало диапазона
        /// </summary>
        public decimal? StartRange { get; set; }

        /// <summary>
        /// Конец диапазона
        /// </summary>
        public decimal? EndRange { get; set; }
    }
}
