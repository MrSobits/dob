using System;
using System.Collections.Generic;

namespace GisGkhLibrary.Entities.HouseMgmt.Account
{
    /// <summary>
    /// Лицевой счет
    /// </summary>
    public class Account
    {
        /// <summary>
        /// Guid лицевого счета в ГИС ЖКХ
        /// </summary>
        public Guid AccountGUID { get; set; }

        /// <summary>
        /// Номер ЛС
        /// </summary>
        public string AccountNumber { get; set; }

        /// <summary>
        /// Тип ЛС
        /// </summary>
        public Enums.HouseMgmt.AccountType AccountType { get; set; }

        /// <summary>
        /// Дата создания ЛС в ГИС ЖКХ (игнорируется при импорте, оставлено для совместимости)
        /// </summary>
        public DateTime? CreationDate { get; set; }

        /// <summary>
        /// Количество проживающих
        /// </summary>
        public UInt32 LivingPersonsNumber { get; set; }

        /// <summary>
        /// Общая площадь для ЛС
        /// </summary>
        public decimal? TotalSquare { get; set; }

        /// <summary>
        /// Отапливаемая площадь
        /// </summary>
        public decimal? HeatedArea { get; set; }

        /// <summary>
        /// Информация о закрытии ЛС. null, если не закрыт
        /// </summary>
        public CloseInfo CloseInfo { get; set; }

        /// <summary>
        /// Помещения
        /// </summary>
        public IEnumerable<Accommodation> Accommodations { get; set; }

        /// <summary>
        /// Сведения о платильщике
        /// </summary>
        public PayerInfo PayerInfo { get; set; }

        /// <summary>
        /// Контракты
        /// </summary>
        public IEnumerable<SupplyResourceContract> Contracts  { get; set; }
}
}
