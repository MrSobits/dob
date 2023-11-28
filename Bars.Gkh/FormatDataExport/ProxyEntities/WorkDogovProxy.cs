namespace Bars.Gkh.FormatDataExport.ProxyEntities
{
    using System;

    using Bars.B4.DataModels;

    /// <summary>
    /// Работы договора на выполнение работ по капитальному ремонту
    /// </summary>
    public class WorkDogovProxy : IHaveId
    {
        /// <summary>
        /// 1. Уникальный код
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 2. Код договора на выполнение работ по капитальному ремонту
        /// </summary>
        public long? DogovorKprId { get; set; }

        /// <summary>
        /// 3. Код работы в КПР
        /// </summary>
        public long? KprId { get; set; }

        /// <summary>
        /// 4. Дата начала выполнения работы
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 5. Дата окончания выполнения работы
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 6. Стоимость работы в договоре
        /// </summary>
        public decimal? ContractAmount { get; set; }

        /// <summary>
        /// 7. Стоимость работы в КПР
        /// </summary>
        public decimal? KprAmount { get; set; }

        /// <summary>
        /// 8. Объём работы
        /// </summary>
        public decimal? WorkVolume { get; set; }

        /// <summary>
        /// 9. Код ОКЕИ
        /// </summary>
        public string Okei { get; set; }

        /// <summary>
        /// 10. Другая единица измерения
        /// </summary>
        public string AnotherUnit { get; set; }

        /// <summary>
        /// 11. Дополнительная информация
        /// </summary>
        public string Description { get; set; }
    }
}