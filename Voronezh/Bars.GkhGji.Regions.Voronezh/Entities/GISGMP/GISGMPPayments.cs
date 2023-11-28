namespace Bars.GkhGji.Regions.Voronezh.Entities
{
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Enums;
    using System;

    public class GISGMPPayments : BaseEntity
    {
        /// <summary>
        /// Запрос к ВС ГИС ГМП
        /// </summary>
        public virtual GisGmp GisGmp { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo FileInfo { get; set; }

        /// <summary>
        /// ID платежа
        /// </summary>
        public virtual string PaymentId { get; set; }

        /// <summary>
        /// УИН
        /// </summary>
        public virtual string SupplierBillID { get; set; }

        /// <summary>
        /// Назначение оплаты
        /// </summary>
        public virtual string Purpose { get; set; }

        /// <summary>
        /// Сумма
        /// </summary>
        public virtual decimal Amount { get; set; }

        /// <summary>
        /// Дата оплаты
        /// </summary>
        public virtual DateTime? PaymentDate { get; set; }

        /// <summary>
        /// КБК
        /// </summary>
        public virtual string Kbk { get; set; }

        /// <summary>
        /// ОКТМО
        /// </summary>
        public virtual string OKTMO { get; set; }

        /// <summary>
        /// Сквитировано
        /// </summary>
        public virtual YesNoNotSet Reconcile { get; set; }
    }
}
