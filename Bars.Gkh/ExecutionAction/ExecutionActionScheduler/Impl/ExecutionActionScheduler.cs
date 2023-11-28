namespace Bars.Gkh.ExecutionAction.ExecutionActionScheduler.Impl
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    using Bars.B4.Utils;
    using Bars.Gkh.Config;
    using Bars.Gkh.Quartz.Scheduler;

    using Castle.Windsor;

    using global::Quartz;
    using global::Quartz.Impl;
    using global::Quartz.Impl.Matchers;
    using global::Quartz.Simpl;

    /// <summary>
    /// Планировщик выполняемых действий. <see cref="DefaultThreadCount"/> = 1, <br/>
    /// <see cref="DefaultThreadPriority"/> = <see cref="System.Threading.ThreadPriority.Normal"/>
    /// </summary>
    public class ExecutionActionScheduler : BaseGkhQuartzScheduler, IExecutionActionScheduler
    {
        /// <summary>
        /// Код для регистрации
        /// </summary>
        public static string Code => typeof(ExecutionActionScheduler).GUID.ToString();

        /// <inheritdoc />
        public IList<IExecutionActionJobInfo> GetJobsInfo()
        {
            return this.Scheduler.GetJobGroupNames()
                .SelectMany(x => this.Scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupContains(x)))
                .Select(x => new ExecutionActionJobInfo
                {
                    TaskId = x.Name.ToLong(),
                    JobName = x.Name,
                    JobGroup = x.Group,
                    NextFireTime = this.Scheduler.GetTriggersOfJob(x)
                        .Select(t => t.GetNextFireTimeUtc()?.LocalDateTime)
                        .OrderByDescending(t => t)
                        .FirstOrDefault(),
                    PreviousFireTime = this.Scheduler.GetTriggersOfJob(x)
                        .Select(t => t.GetPreviousFireTimeUtc()?.LocalDateTime)
                        .OrderByDescending(t => t)
                        .FirstOrDefault()
                })
                .ToList<IExecutionActionJobInfo>();
        }

        /// <inheritdoc />
        string IExecutionActionScheduler.Code => ExecutionActionScheduler.Code;

        /// <summary>
        /// Имя ключа (<see cref="int"/>)
        /// </summary>
        public const string ThreadCountKeyName = "ExecutionActionThreadCount";

        /// <summary>
        /// Имя ключа (<see cref="ThreadPriority"/>)
        /// </summary>
        public const string ThreadPriorityKeyName = "ExecutionActionThreadPriority";

        /// <summary>
        /// Имя ключа (<see cref="bool"/>)
        /// </summary>
        public const string AutoStartMandatoryKeyName = "ExecutionActionAutoStartMandatory";

        private const int DefaultThreadCount = 1;
        private const ThreadPriority DefaultThreadPriority = ThreadPriority.BelowNormal;

        /// <summary>
        /// Автозапуск обязательных действий
        /// </summary>
        public const bool DefaultAutoStartMandatory = true;

        /// <summary>
        /// Планировщик выполнения действий
        /// </summary>
        public ExecutionActionScheduler(IWindsorContainer сontainer)
        {
            var configManager = сontainer.Resolve<IGkhParams>();

            var configParams = configManager.GetParams();

            var threadCount = configParams.GetAs<int>(
                ExecutionActionScheduler.ThreadCountKeyName,
                ExecutionActionScheduler.DefaultThreadCount);
            var threadPriority = configParams.GetAs<ThreadPriority>(
                ExecutionActionScheduler.ThreadPriorityKeyName,
                ExecutionActionScheduler.DefaultThreadPriority);

            сontainer.Release(configManager);

            DirectSchedulerFactory.Instance.CreateScheduler(
                this.GetType().Name,
                ExecutionActionScheduler.Code,
                new SimpleThreadPool(threadCount, threadPriority),
                new RAMJobStore());

            var jobListener = сontainer.Resolve<IJobListener>(ExecutionActionJobListener.Code);

            this.Scheduler = DirectSchedulerFactory.Instance.GetScheduler(this.GetType().Name);
            this.Scheduler.ListenerManager.AddJobListener(jobListener,GroupMatcher<JobKey>.AnyGroup());

            this.Scheduler.Start();
        }
    }
}