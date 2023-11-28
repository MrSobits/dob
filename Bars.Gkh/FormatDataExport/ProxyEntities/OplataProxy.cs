namespace Bars.Gkh.FormatDataExport.ProxyEntities
{
    using System;

    using Bars.B4.DataModels;

    /// <summary>
    /// Оплата ЖКУ
    /// </summary>
    public class OplataProxy : IHaveId
    {
        /// <summary>
        /// 1. Уникальный код оплаты
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 2. Уникальный код лицевого счета в системе отправителя
        /// </summary>
        public long? KvarId { get; set; }

        /// <summary>
        /// 3. Тип операции
        /// </summary>
        public int OperationType { get; set; }

        /// <summary>
        /// 4. Номер платежного документа(распоряжения)
        /// </summary>
        public string DocumentNumber { get; set; }

        /// <summary>
        /// 5. Дата оплаты
        /// </summary>
        public DateTime PaymentDate { get; set; }

        /// <summary>
        /// 6. Дата учета
        /// </summary>
        public DateTime? OperationDate { get; set; }

        /// <summary>
        /// 7. Сумма оплаты
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 8. Источник оплаты
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// 9. Месяц, за который произведена оплата
        /// </summary>
        public DateTime? Month { get; set; }

        /// <summary>
        /// 10. Уникальный код пачки оплат
        /// </summary>
        public long? OplataPackId { get; set; }

        /// <summary>
        /// 11. Расчетный счет получателя
        /// </summary>
        public long? ContragentRschetId { get; set; }

        /// <summary>
        /// 12. Платежный документ
        /// </summary>
        public long? EpdId { get; set; }

        /// <summary>
        /// 13. Уникальный идентификатор плательщика
        /// </summary>
        public long? IndId { get; set; }

        /// <summary>
        /// 14. Наименование плательщика
        /// </summary>
        public string PayerName { get; set; }

        /// <summary>
        /// 15. Назначение платежа
        /// </summary>
        public string Destination { get; set; }

        /// <summary>
        /// 16. Произвольный комментарий
        /// </summary>
        public string Remark { get; set; }

    }
}