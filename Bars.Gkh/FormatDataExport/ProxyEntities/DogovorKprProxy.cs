namespace Bars.Gkh.FormatDataExport.ProxyEntities
{
    using System;

    using Bars.B4.DataModels;
    using Bars.B4.Modules.FileStorage;

    /// <summary>
    /// Договоры на выполнение работ по капитальному ремонту
    /// </summary>
    public class DogovorKprProxy : IHaveId
    {
        /// <summary>
        /// 1. Уникальный код
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Идентификатор договора КР
        /// </summary>
        public long? ContractCrId { get; set; }

        /// <summary>
        /// Идентификатор договора подряда КР
        /// </summary>
        public long? BuildContractId { get; set; }

        /// <summary>
        /// 2. Код КПР
        /// </summary>
        public long KprId { get; set; }

        /// <summary>
        /// 3. Номер договора
        /// </summary>
        public string DocumentNumber { get; set; }

        /// <summary>
        /// 4. Дата договора
        /// </summary>
        public DateTime? DocumentDate { get; set; }

        /// <summary>
        /// 5. Дата начала выполнения
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 6. Дата окончания выполнения работ
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 7. Сумма договора
        /// </summary>
        public decimal? Sum { get; set; }

        /// <summary>
        /// 8. Заказчик
        /// </summary>
        public long? CustomerContragentId { get; set; }

        /// <summary>
        /// 9. Исполнитель
        /// </summary>
        public long? ExecutantContragentId { get; set; }

        /// <summary>
        /// 10. Гарантийный срок установлен
        /// </summary>
        public int IsGuaranteePeriod { get; set; }

        /// <summary>
        /// 11. Гарантийный срок (кол-во месяцев)
        /// </summary>
        public int? GuaranteePeriod { get; set; }

        /// <summary>
        /// 12. Наличие сметной документации
        /// </summary>
        public int? IsBudgetDocumentation { get; set; }

        /// <summary>
        /// 13. Проведение отбора предусмотрено законодательством
        /// </summary>
        public int? IsLawProvided { get; set; }

        /// <summary>
        /// 14. Адрес сайта с информацией об отборе
        /// </summary>
        public string InfoUrl { get; set; }

        /// <summary>
        /// 15. Статус договора
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// 16. Причина расторжения
        /// </summary>
        public string RevocationReason { get; set; }

        /// <summary>
        /// 17. Номер документа расторжения
        /// </summary>
        public string RevocationDocumentNumber { get; set; }

        /// <summary>
        /// 18. Дата документа расторжения
        /// </summary>
        public DateTime? RevocationDate { get; set; }

        #region DOGOVORKPRFILES
        /// <summary>
        /// 1. Уникальный идентификатор файла
        /// </summary>
        public FileInfo File { get; set; }

        /// <summary>
        /// 3. Тип файла
        /// </summary>
        public int FileType { get; set; }
        #endregion

        /// <summary>
        /// Объект капитального ремонта
        /// </summary>
        public long? ObjectCrId { get; set; }
    }
}