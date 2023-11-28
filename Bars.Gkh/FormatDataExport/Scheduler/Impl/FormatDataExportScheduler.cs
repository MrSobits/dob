namespace Bars.Gkh.FormatDataExport.Scheduler.Impl
{
    using System.Collections.Concurrent;
    using System.Text;
    using System.Threading;

    using Bars.B4;
    using Bars.B4.Logging;

    using Castle.Windsor;

    using global::Quartz;
    using global::Quartz.Impl;
    using global::Quartz.Impl.Matchers;
    using global::Quartz.Simpl;

    /// <summary>
    /// Планировщик экспорта данных по формату.
    /// </summary>
    public class FormatDataExportScheduler : IFormatDataExportScheduler
    {
        /// <summary>
        /// Код для регистрации
        /// </summary>
        public static string Code => nameof(FormatDataExportScheduler);

        /// <inheritdoc />
        string IFormatDataExportScheduler.Code => FormatDataExportScheduler.Code;

        public IWindsorContainer Container { get; set; }
        public ILogManager LogManager { get; set; }
        private readonly IScheduler scheduler;
        private readonly ConcurrentDictionary<JobKey, CancellationTokenSource> cancellationTokenSources;

        private const int DefaultThreadCount = 4;
        private const ThreadPriority DefaultThreadPriority = ThreadPriority.Normal;

        public FormatDataExportScheduler()
        {
            DirectSchedulerFactory.Instance.CreateScheduler(
                FormatDataExportScheduler.Code,
                this.GetType().FullName,
                new SimpleThreadPool(FormatDataExportScheduler.DefaultThreadCount, FormatDataExportScheduler.DefaultThreadPriority),
                new RAMJobStore());

            this.scheduler = DirectSchedulerFactory.Instance.GetScheduler(FormatDataExportScheduler.Code);
            this.cancellationTokenSources = new ConcurrentDictionary<JobKey, CancellationTokenSource>();
            this.scheduler.Start();
        }

        /// <inheritdoc />
        public IDataResult ScheduleJob(FormatDataExportJobInstance job)
        {
            var cancellationTokenSource = new CancellationTokenSource();
            job.CancellationToken = cancellationTokenSource.Token;

            var scheduleTime = this.scheduler.ScheduleJob(job.GetJob(), job.GetTrigger());

            this.cancellationTokenSources.GetOrAdd(job.JobKey, cancellationTokenSource);

            this.LogManager.Debug($"Задача '{job.JobKey}' поставлена в очередь. " +
                $"Время следующего запуска: {scheduleTime.LocalDateTime}");

            return new BaseDataResult(scheduleTime);
        }

        /// <inheritdoc />
        public IDataResult UnScheduleJob(FormatDataExportJobInstance job)
        {
            var result = new BaseDataResult(job);

            this.Cancel(job.JobKey);

            if (this.scheduler.CheckExists(job.TriggerKey))
            {
                result.Success &= this.scheduler.UnscheduleJob(job.TriggerKey);
            }
            if (this.scheduler.CheckExists(job.JobKey))
            {
                result.Success &= this.scheduler.Interrupt(job.JobKey);
            }
            return result;
        }

        private void Cancel(JobKey jobKey)
        {
            CancellationTokenSource cancellationTokenSource;
            this.cancellationTokenSources.TryRemove(jobKey, out cancellationTokenSource);
            cancellationTokenSource?.Cancel();
        }

        /// <inheritdoc />
        public string GetAllJobInfo()
        {
            var jobGroups = this.scheduler.GetJobGroupNames();
            var sb = new StringBuilder();
            sb.AppendLine($"[{this.GetType().FullName}]");
            sb.AppendLine("Очередь задач:");
            foreach (string group in jobGroups)
            {
                var groupMatcher = GroupMatcher<JobKey>.GroupContains(group);
                var jobKeys = this.scheduler.GetJobKeys(groupMatcher);
                foreach (var jobKey in jobKeys)
                {
                    var detail = this.scheduler.GetJobDetail(jobKey);
                    var triggers = this.scheduler.GetTriggersOfJob(jobKey);
                    foreach (var trigger in triggers)
                    {
                        sb.AppendFormat($"\tJob group: {group}\r\n");
                        sb.AppendFormat($"\tJob name: {jobKey.Name}\r\n");
                        sb.AppendFormat($"\tJob description: {detail.Description}\r\n");
                        sb.AppendFormat($"\tTrigger name: {trigger.Key.Name}\r\n");
                        sb.AppendFormat($"\tTrigger group: {trigger.Key.Group}\r\n");
                        sb.AppendFormat($"\tTriggerType name: {trigger.GetType().Name}\r\n");
                        sb.AppendFormat($"\tTrigger state: {this.scheduler.GetTriggerState(trigger.Key)}\r\n");
                        var nextFireTime = trigger.GetNextFireTimeUtc();
                        if (nextFireTime.HasValue)
                        {
                            sb.AppendFormat($"\tNext fire time: {nextFireTime.Value.LocalDateTime}\r\n");
                        }

                        var previousFireTime = trigger.GetPreviousFireTimeUtc();
                        if (previousFireTime.HasValue)
                        {
                            sb.AppendFormat($"\tPrevious fire time: {previousFireTime.Value.LocalDateTime}\r\n");
                        }
                    }
                }
            }
            return sb.ToString();
        }
    }
}