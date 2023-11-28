namespace Bars.Gkh.FormatDataExport.ProxyEntities
{
    using System;

    using Bars.B4.DataModels;

    /// <summary>
    /// Акты выполненных работ по договору на выполнение работ по капитальному ремонту
    /// </summary>
    public class ActWorkDogovProxy : IHaveId
    {
        /// <summary>
        /// 1. Уникальный код работы/услуги
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 2. Код договора на выполнение работ по капитальному ремонту
        /// </summary>
        public long? DogovorKprId { get; set; }

        /// <summary>
        /// 3. Статус
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// 4. Наименование акта
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 5. Номер акта
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// 6. Дата акта
        /// </summary>
        public DateTime? Date { get; set; }

        /// <summary>
        /// 7. Сумма акта
        /// </summary>
        public decimal? Sum { get; set; }

        /// <summary>
        /// 8. Сумма штрафных санкций Исполнителю
        /// </summary>
        public decimal? ExecutantPenaltySum { get; set; }

        /// <summary>
        /// 9. Сумма штрафных санкций Заказчику
        /// </summary>
        public decimal? CustomerPenaltySum { get; set; }

        /// <summary>
        /// 10. Акт подписан представителем собственников
        /// </summary>
        public int? IsSigned { get; set; }

        /// <summary>
        /// 11. Представитель собственников
        /// </summary>
        public long? AgentId { get; set; }

        /// <summary>
        /// 12. Рассрочка по оплате выполненных работ
        /// </summary>
        public int? IsInstallments { get; set; }
    }
}