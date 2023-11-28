namespace Bars.Gkh.Entities.Administration.FormatDataExport
{
    using System;
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Задача для экспорта данных по формату
    /// </summary>
    public class FormatDataExportTask : BaseEntity
    {
        /// <summary>
        /// Оператор
        /// </summary>
        public virtual Operator Operator { get; set; }

        /// <summary>
        /// Дата начала действия
        /// </summary>
        public virtual DateTime? StartDate { get; set; }

        /// <summary>
        /// Дата окончания действия
        /// </summary>
        public virtual DateTime? EndDate { get; set; }

        /// <summary>
        /// Периодичность запуска
        /// </summary>
        public virtual TaskPeriodType PeriodType { get; set; }

        /// <summary>
        /// Запустить сейчас
        /// <para>Не хранимое</para>
        /// </summary>
        public virtual bool StartNow { get; set; }

        /// <summary>
        /// Час запуска
        /// </summary>
        public virtual int StartTimeHour { get; set; }

        /// <summary>
        /// Минуты запуска
        /// </summary>
        public virtual int StartTimeMinutes { get; set; }

        /// <summary>
        /// Выгружаемые группы секций
        /// </summary>
        public virtual IList<string> EntityGroupCodeList { get; set; }

        /// <summary>
        /// Признак удаления задачи
        /// </summary>
        public virtual bool IsDelete { get; set; }

        /// <summary>
        /// Дни недели запуска
        /// </summary>
        public virtual IList<byte> StartDayOfWeekList { get; set; }

        /// <summary>
        /// Месяцы запуска
        /// </summary>
        public virtual IList<byte> StartMonthList { get; set; }

        /// <summary>
        /// Числа месяца запуска
        /// <para>
        /// 0 - последний день месяца
        /// </para>
        /// </summary>
        public virtual IList<byte> StartDaysList { get; set; }

        /// <summary>
        /// Параметры запуска задачи
        /// </summary>
        public virtual BaseParams BaseParams { get; set; }
    }
}