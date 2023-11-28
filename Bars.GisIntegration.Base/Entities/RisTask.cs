namespace Bars.GisIntegration.Base.Entities
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.GisIntegration.Base.Enums;

    /// <summary>
    /// Задача
    /// </summary>
    public class RisTask : BaseEntity, IUserEntity
    {
        /// <summary>
        /// Описание задачи
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Время начала выполнения
        /// </summary>
        public virtual DateTime StartTime { get; set; }

        /// <summary>
        /// Время окончания выполнения
        /// </summary>
        public virtual DateTime EndTime { get; set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public virtual string UserName { get; set; }

        /// <summary>
        /// Имя класса - экспортера, инициировавшего задачу
        /// </summary>
        public virtual string ClassName { get; set; }

        /// <summary>
        /// Статус задачи
        /// </summary>
        public virtual TaskState TaskState { get; set; }

        /// <summary>
        /// Сообщение
        /// </summary>
        public virtual string Message { get; set; }
    }
}
