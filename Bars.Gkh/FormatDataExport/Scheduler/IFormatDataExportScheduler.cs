namespace Bars.Gkh.FormatDataExport.Scheduler
{
    using Bars.B4;
    using Bars.Gkh.FormatDataExport.Scheduler.Impl;

    public interface IFormatDataExportScheduler
    {
        /// <summary>
        /// Код планировщика
        /// </summary>
        string Code { get; }

        /// <summary>
        /// Поставить задачу в очередь
        /// </summary>
        IDataResult ScheduleJob(FormatDataExportJobInstance job);

        /// <summary>
        /// Удалить задачу из очереди
        /// </summary>
        IDataResult UnScheduleJob(FormatDataExportJobInstance job);

        /// <summary>
        /// Получить информацию об очереди планировщика
        /// </summary>
        string GetAllJobInfo();
    }
}