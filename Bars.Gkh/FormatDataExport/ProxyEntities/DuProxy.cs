namespace Bars.Gkh.FormatDataExport.ProxyEntities
{
    using System;

    using Bars.B4.DataModels;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.FormatDataExport.ProxySelectors;

    /// <summary>
    /// Договор управления многоквартирным домом
    /// </summary>
    public class DuProxy : IHaveId
    {
        #region DU
        /// <summary>
        /// 1. Уникальный идентификатор
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 2. Уникальный идентификатор Управляющей организации
        /// </summary>
        public long? ContragentId { get; set; }

        //public Contragent Contragent { get; set; }

        /// <summary>
        /// 3. Номер документа
        /// </summary>
        public string DocumentNumber { get; set; }

        /// <summary>
        /// 4. Дата заключения
        /// </summary>
        public DateTime? DocumentDate { get; set; }

        /// <summary>
        /// 5. Дата вступления в силу
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 6. Планируемая дата окончания
        /// </summary>
        public DateTime? PlannedEndDate { get; set; }

        /// <summary>
        /// 7. Срок действия (Месяц)
        /// </summary>
        public int? ValidityMonths { get; set; }

        /// <summary>
        /// 8. Срок действия (Год/лет)
        /// </summary>
        public int? ValidityYear { get; set; }

        /// <summary>
        /// 9. Номер извещения
        /// </summary>
        public string NoticeNumber { get; set; }

        /// <summary>
        /// 10. Ссылка на извещение на официальном сайте в сети «Интернет» для размещения информации о проведении торгов
        /// </summary>
        public string NoticeLink { get; set; }

        /// <summary>
        /// 11. Собственник объекта жилищного фонда
        /// </summary>
        public int? Owner { get; set; }

        /// <summary>
        /// 12. Контрагент второй стороны договора
        /// </summary>
        [ProxyId(typeof(ContragentProxy))]
        public long? ContragentOwnerId { get; set; }

        //public Contragent ContragentOwner { get; set; }

        /// <summary>
        /// 13. Тип второй стороны договора
        /// </summary>
        public int? ContragentOwnerType { get; set; }

        /// <summary>
        /// 14. Основание заключения договора
        /// </summary>
        public int? ContractFoundation { get; set; }

        /// <summary>
        /// 15. День месяца начала ввода показаний по ПУ
        /// </summary>
        public int? InputMeteringDeviceValuesBeginDay { get; set; }

        /// <summary>
        /// 16. День месяца окончания ввода показаний по ПУ
        /// </summary>
        public int? InputMeteringDeviceValuesEndDay { get; set; }

        /// <summary>
        /// 17. Последний день месяца ввода показаний по ПУ
        /// </summary>
        public bool? IsInputMeteringDeviceValuesLastDay { get; set; }

        /// <summary>
        /// 18. День месяца выставления платежных документов
        /// </summary>
        public int? DrawingPaymentDocumentDay { get; set; }

        /// <summary>
        /// 19. Последний день месяца выставления платежных документов
        /// </summary>
        public bool? IsDrawingPaymentDocumentLastDay { get; set; }

        /// <summary>
        /// 20. Месяц выставления платежных документов
        /// </summary>
        public int? DrawingPaymentDocumentMonth { get; set; }

        /// <summary>
        /// 21. Статус ДУ
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 22. Дата расторжения, прекращения действия договора управления
        /// </summary>
        public DateTime? TerminationDate { get; set; }

        /// <summary>
        /// 23. Причина расторжения договора
        /// </summary>
        public int? TerminationReason { get; set; }

        /// <summary>
        /// 24. Причина аннулирования
        /// </summary>
        public string CancellationReason { get; set; }

        /// <summary>
        /// 25. Формировать заявки в реестр лицензий, если сведения о лицензии/управляемом объекте отсутствуют
        /// </summary>
        public int? IsFormingApplications { get; set; }
        #endregion

        #region DUCHARGE
        /// <summary>
        /// DUCHARGE 2. Статус
        /// </summary>
        public int? ChargeStatus { get; set; }

        /// <summary>
        /// DUCHARGE 4. Дата начала периода
        /// </summary>
        public DateTime? StartDatePaymentPeriod { get; set; }

        /// <summary>
        /// DUCHARGE 5. Дата окончания периода
        /// </summary>
        public DateTime? EndDatePaymentPeriod { get; set; }

        /// <summary>
        /// DUCHARGE 6. Цена за услуги, работы по управлению МКД
        /// </summary>
        public decimal? PaymentAmount { get; set; }

        /// <summary>
        /// DUCHARGE 7. Протокол, которым утверждён размер платы
        /// </summary>
        public FileInfo PaymentProtocolFile { get; set; }

        /// <summary>
        /// DUCHARGE 8. Работа, услуга организации
        /// </summary>
        public long? ServiceId { get; set; }

        /// <summary>
        /// DUCHARGE 9. Размер платы за услугу организации
        /// </summary>
        public decimal? ServicePayment { get; set; }

        /// <summary>
        /// DUCHARGE 10. Тип размера платы
        /// </summary>
        public int? SetPaymentsFoundation { get; set; }

        /// <summary>
        /// DUCHARGE Идентификатор жилого дома
        /// </summary>
        public long RealityObjectId { get; set; }

        #endregion

        #region DUFILES

        /// <summary>
        /// DUFILES 1. Договор управления и приложения к договору (тип 1)
        /// </summary>
        public FileInfo DuFile { get; set; }

        /// <summary>
        /// DUFILES 1. Протокол собрания собственников (тип 4)
        /// </summary>
        public FileInfo OssFile { get; set; }

        /// <summary>
        /// DUFILES 1. Реестр собственников, подписавших протокол (тип 8)
        /// </summary>
        public FileInfo OwnerFile { get; set; }

        ///// <summary>
        ///// Протокол собрания собственников
        ///// </summary>
        //public FileInfo ProtocolFileInfo { get; set; }
        #endregion
    }
}