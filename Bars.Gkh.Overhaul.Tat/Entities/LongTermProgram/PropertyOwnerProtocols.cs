namespace Bars.Gkh.Overhaul.Tat.Entities
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Overhaul.Tat.Enum;
    using Gkh.Entities;

    /// <summary>
    /// Протоколы собственников помещений МКД
    /// </summary>
    public class PropertyOwnerProtocols : BaseEntity
    {
        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// тип протокола
        /// </summary>
        public virtual PropertyOwnerProtocolType TypeProtocol { get; set; }

        /// <summary>
        /// Дата
        /// </summary>
        public virtual DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Номер
        /// </summary>
        public virtual string DocumentNumber { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description  { get; set; }

        /// <summary>
        /// Файл (документ)
        /// </summary>
        public virtual FileInfo DocumentFile { get; set; }

        /// <summary>
        /// Количество голосов (кв.м.)
        /// </summary>
        public virtual decimal? NumberOfVotes { get; set; }

        /// <summary>
        /// Общее количество голосов (кв.м.)
        /// </summary>
        public virtual decimal? TotalNumberOfVotes { get; set; }

        /// <summary>
        /// Доля принявших участие (%)
        /// </summary>
        public virtual decimal? PercentOfParticipating { get; set; }

        /// <summary>
        /// Сумма займа
        /// </summary>
        public virtual decimal? LoanAmount { get; set; }

        /// <summary>
        /// Заемщик
        /// </summary>
        public virtual Contragent Borrower { get; set; }

        /// <summary>
        /// Кредитор
        /// </summary>
        public virtual Contragent Lender { get; set; }

    }
}