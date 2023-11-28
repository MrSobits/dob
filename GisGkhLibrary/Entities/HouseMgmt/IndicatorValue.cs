namespace GisGkhLibrary.Entities.HouseMgmt
{
    public class IndicatorValue
    {
        /// <summary>
        /// Код ОКЕИ
        /// </summary>
        public string OKEI { get; set; }

        /// <summary>
        /// Значение соответствует
        /// </summary>
        public bool? Correspond { get; set; }

        /// <summary>
        /// Начало диапазона
        /// </summary>
        public decimal? StartRange { get; set; }

        /// <summary>
        /// Число
        /// </summary>
        public decimal? Number { get; set; }

        /// <summary>
        /// Конец диапазона
        /// </summary>
        public decimal? EndRange { get; set; }
    }
}
