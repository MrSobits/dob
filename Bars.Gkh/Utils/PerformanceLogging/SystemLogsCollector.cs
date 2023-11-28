namespace Bars.Gkh.Utils.PerformanceLogging
{
    using Bars.B4.Logging;
    using Bars.B4.Utils;

    /// <summary>
    /// Коллектор сбора логов в файл .logs/info.log
    /// </summary>
    public class SystemLogsCollector : BaseLogsCollector
    {
        /// <summary>
        /// Менеджер логов
        /// </summary>
        public ILogManager LogManager { get; set; }

        /// <summary>
        /// Сохранение данных логов
        /// </summary>
        protected override void SaveLogs()
        {
            if (this.PerforanceLogItems.IsNotEmpty())
            {
                this.PerforanceLogItems.ForEach(x => this.LogManager.Info(
                    $"Key:{x.Key}|Time: {x.TimeSpan}{(x.Description != null ? "|Description: " + x.Description : string.Empty)}"));
            }
        }
    }
}