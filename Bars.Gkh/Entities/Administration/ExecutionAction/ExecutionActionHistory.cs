namespace Bars.Gkh.Entities.Administration.ExecutionAction
{
    using System;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Выполненные действия
    /// </summary>
    // TODO Удалить в 1.39
    [Obsolete("Удалить в 1.39")]
    public class ExecutionActionHistory : BaseEntity
    {
        /// <summary>
        /// Код действия
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Идентификатор задачи
        /// </summary>
        public virtual Guid JobId { get; set; }

        /// <summary>
        /// Дата создания задачи
        /// </summary>
        public virtual DateTime CreateDate { get; set; }

        /// <summary>
        /// Дата запуска задачи
        /// </summary>
        public virtual DateTime StartDate { get; set; }

        /// <summary>
        /// Дата завершения задачи
        /// </summary>
        public virtual DateTime EndDate { get; set; }

        /// <summary>
        /// Сообщение об ошибке
        /// </summary>
        public virtual IDataResult DataResult { get; set; }

        /// <summary>
        /// Статус выполнения
        /// </summary>
        public virtual ExecutionActionStatus Status { get; set; }
    }
}