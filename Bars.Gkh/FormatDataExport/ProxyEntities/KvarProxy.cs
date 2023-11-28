namespace Bars.Gkh.FormatDataExport.ProxyEntities
{
    using System;

    using Bars.B4.DataModels;

    /// <summary>
    /// Прокси для лицевого счета
    /// </summary>
    public class KvarProxy : IHaveId
    {
        /// <summary>
        /// 1. Уникальный код лицевого счета в системе отправителя
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 2. Уникальный номер лицевого счета в системе отправителя
        /// </summary>
        public string PersonalAccountNum { get; set; }

        /// <summary>
        /// 8. Дата открытия ЛС
        /// </summary>
        public DateTime OpenDate { get; set; }

        /// <summary>
        /// 10. Дата закрытия ЛС
        /// </summary>
        public DateTime? CloseDate { get; set; }

        /// <summary>
        /// 11. Причина закрытия ЛС
        /// </summary>
        public string CloseReasonType { get; set; }

        /// <summary>
        /// 12. Основание закрытия ЛС
        /// </summary>
        public string CloseReason { get; set; }

        /// <summary>
        /// 17. Общая площадь (площадь применяемая для расчета большинства площадных услуг)
        /// </summary>
        public decimal? Area { get; set; }

        /// <summary>
        /// 31. Код контрагента, с которым у потребителя ЖКУ заключен договор на оказание услуг (принципал)
        /// </summary>
        public long? PrincipalContragentId { get; set; }

        /// <summary>
        /// 32. Тип лицевого счета
        /// </summary>
        public string PersonalAccountType { get; set; }

        /// <summary>
        /// Владелец-физ лицо
        /// </summary>
        public IndProxy IndividualOwner { get; set; }

        /// <summary>
        /// 34. Плательщик – Организация
        /// </summary>
        public long? ContragentId { get; set; }

        /// <summary>
        /// Помещение
        /// </summary>
        public PremisesProxy Premises { get; set; }

        /// <summary>
        /// KVARACCCOM 2. Уникальный идентификатор помещения
        /// </summary>
        public long? PremisesId => this.Premises?.Id;

        /// <summary>
        /// KVARACCCOM 3. Уникальный идентификатор дома
        /// </summary>
        public long? RealityObjectId => this.Premises?.RealityObjectId;

        /// <summary>
        /// KVARACCCOM 4. Уникальный идентификатор комнаты
        /// </summary>
        public long? RoomId { get; set; }

        /// <summary>
        /// KVARACCCOM 5. Доля внесения платы, размер доли в %
        /// </summary>
        public decimal? Share { get; set; }
    }
}
