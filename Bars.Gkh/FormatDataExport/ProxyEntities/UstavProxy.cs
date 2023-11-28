namespace Bars.Gkh.FormatDataExport.ProxyEntities
{
    using System;

    using Bars.B4.DataModels;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Уставы
    /// </summary>
    public class UstavProxy : IHaveId
    {
        #region USTAV
        /// <summary>
        /// 1. Уникальный идентификатор
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 2. Уникальный идентификатор Управляющей организации/ТСЖ
        /// </summary>
        public long? ContragentId { get; set; }

        /// <summary>
        /// 3. Номер документа
        /// </summary>
        public string DocumentNumber { get; set; }

        /// <summary>
        /// 4. Дата регистрации ТСН/ТСЖ/кооператива (Организации Поставщика данных)
        /// </summary>
        public DateTime? DocumentDate { get; set; }

        /// <summary>
        /// 5. День месяца начала ввода показаний по ПУ
        /// </summary>
        public int? InputMeteringDeviceValuesBeginDay { get; set; }

        /// <summary>
        /// 6. День месяца окончания ввода показаний по ПУ
        /// </summary>
        public int? InputMeteringDeviceValuesEndDay { get; set; }

        /// <summary>
        /// 7. Последний день месяца ввода показаний по ПУ
        /// </summary>
        public int? IsInputMeteringDeviceValuesLastDay { get; set; }

        /// <summary>
        /// 8. День месяца выставления платежных документов
        /// </summary>
        public int? DrawingPaymentDocumentDay { get; set; }

        /// <summary>
        /// 9. Последний день месяца выставления платежных документов
        /// </summary>
        public int? IsDrawingPaymentDocumentLastDay { get; set; }

        /// <summary>
        /// 10. Месяц выставления платежных документов
        /// </summary>
        public int? ThisMonthPaymentDocDate { get; set; }

        /// <summary>
        /// 11. День месяца внесения платы за ЖКУ
        /// </summary>
        public int? PaymentServicePeriodDay { get; set; }

        /// <summary>
        /// 12. Последний день месяца внесения платы за ЖКУ
        /// </summary>
        public int? IsPaymentServicePeriodLastDay { get; set; }

        /// <summary>
        /// 13. Месяц внесения платы за ЖКУ
        /// </summary>
        public int? ThisMonthPaymentServiceDate { get; set; }

        /// <summary>
        /// 14. Единоличный исполнительный орган (Физическое лицо)
        /// </summary>
        public long? PhysicalContragentId { get; set; }

        /// <summary>
        /// 15. Единоличный исполнительный орган (Юридическое лицо)
        /// </summary>
        public long? LegalContragentId { get; set; }

        /// <summary>
        /// 16. Физическое лицо является собственником
        /// </summary>
        public int? IsPhysicalOwner { get; set; }

        /// <summary>
        /// 17. Состав органов управления/Правление
        /// </summary>
        public string Management { get; set; }

        /// <summary>
        /// 18. Статус устава
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 19. Причина аннулирования
        /// </summary>
        public string TerminateReason { get; set; }

        /// <summary>
        /// 20. Дата расторжения, прекращения действия устава
        /// </summary>
        public DateTime? TerminationDate { get; set; }

        /// <summary>
        /// 21. Причина прекращения действия устава (расторжения)
        /// </summary>
        public string ContractStopReason { get; set; }

        /// <summary>
        /// 22. Формировать заявки в реестр лицензий, если сведения о лицензии/управляемом объекте отсутствуют
        /// </summary>
        public int? IsFormingApplications { get; set; }

        /// <summary>
        /// 23. Управление многоквартирным домом осуществляется управляющей организацией по договору управления
        /// </summary>
        public int? IsManagementContract { get; set; }
        #endregion

        #region USTAVCHARGE
        /// <summary>
        /// USTAVCHARGE Тип договора управляющей организации
        /// </summary>
        public TypeContractManOrg TypeContract { get; set; }

        /// <summary>
        /// USTAVCHARGE 3. Информация о размере обязательных платежей
        /// </summary>
        public int PaymentInfo { get; set; }

        /// <summary>
        /// USTAVCHARGE 4. Дата начала периода
        /// </summary>
        public DateTime? StartDatePaymentPeriod { get; set; }

        /// <summary>
        /// USTAVCHARGE 5. Дата окончания периода
        /// </summary>
        public DateTime? EndDatePaymentPeriod { get; set; }

        /// <summary>
        /// USTAVCHARGE 6. Размер обязательных платежей членов ТСЖ
        /// </summary>
        public decimal? CompanyReqiredPaymentAmount { get; set; }

        /// <summary>
        /// USTAVCHARGE 7. Размер платы за содержание и ремонт помещений
        /// </summary>
        public decimal? ReqiredPaymentAmount { get; set; }

        /// <summary>
        /// USTAVCHARGE 8. Работа, услуга организации
        /// </summary>
        public long? ServiceId { get; set; }

        /// <summary>
        /// USTAVCHARGE 9. Размер платы за услугу организации
        /// </summary>
        public decimal? ServicePayment { get; set; }

        /// <summary>
        /// USTAVCHARGE Протокол собрания - Платежи/взносы собственников, не являющихся членами товарищества, кооператива
        /// </summary>
        public FileInfo PaymentProtocolFile { get; set; }

        /// <summary>
        /// USTAVCHARGE Идентификатор жилого дома
        /// </summary>
        public long RealityObjectId { get; set; }
        #endregion

        #region USTAVFILES
        /// <summary>
        /// USTAVFILES 1. Вложение устава Протокол ОСС (Тип 1)
        /// </summary>
        public FileInfo OssFile { get; set; }

        /// <summary>
        /// USTAVFILES 1. Вложение устава Документы устава (Тип 2)
        /// </summary>
        public FileInfo UstavFile { get; set; }
        #endregion

        #region USTAVOU
        /// <summary>
        /// USTAVOU 4. Дата начала предоставления услуг дому
        /// </summary>
        public DateTime? StartDate { get; set; }
        #endregion
    }
}