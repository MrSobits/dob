namespace Bars.GkhDi.Entities
{
    using Gkh.Entities;

    /// <summary>
    /// Прочие услуги
    /// </summary>
    public class OtherService : BaseGkhEntity
    {
        /// <summary>
        /// Раскрытие информации объекта недвижимости
        /// </summary>
        public virtual DisclosureInfoRealityObj DisclosureInfoRealityObj { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Ед измерения
        /// </summary>
        public virtual string UnitMeasure { get; set; }

        /// <summary>
        /// Тариф
        /// </summary>
        public virtual decimal? Tariff { get; set; }

        /// <summary>
        /// Поставщик
        /// </summary>
        public virtual string Provider { get; set; }
    }
}
