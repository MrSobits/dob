namespace Bars.Gkh.FormatDataExport.ProxyEntities
{
    using System;

    using Bars.B4.DataModels;
    using Bars.B4.Modules.FileStorage;

    /// <summary>
    /// Объекты управления договора управления
    /// </summary>
    public class UstavOuProxy : IHaveId
    {
        /// <summary>
        /// 1. Уникальный код
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 2. Договор управления
        /// </summary>
        public long? ContractId { get; set; }

        /// <summary>
        /// 3. Дом
        /// </summary>
        public long? RealityObjectId { get; set; }

        /// <summary>
        /// 4. Дата начала предоставления услуг дому
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 5. Дата окончания предоставления услуг дому
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 6. Основанием является договор управления
        /// </summary>
        public int IsContractReason { get; set; }

        /// <summary>
        /// 7. Файл с дополнительным соглашением
        /// </summary>
        public FileInfo AttachmentFile { get; set; }

        /// <summary>
        /// 8. Дата исключения объекта управления из ДУ
        /// </summary>
        public DateTime? ExceptionManagementDate { get; set; }
    }
}