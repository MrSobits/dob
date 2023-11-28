namespace Bars.Gkh.FormatDataExport.Scheduler.Impl
{
    using System;
    using System.Linq;
    using System.Threading;

    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.IoC.Lifestyles.SessionLifestyle;
    using Bars.B4.Logging;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.B4.Utils;
    using Bars.Gkh.ConfigSections.Administration;
    using Bars.Gkh.Entities.Administration.FormatDataExport;
    using Bars.Gkh.Enums;
    using Bars.Gkh.FormatDataExport.Tasks;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    using global::Quartz;

    /// <summary>
    /// Экземпляр задачи
    /// </summary>
    public class FormatDataExportJobInstance : IInterruptableJob
    {
        private IWindsorContainer container;
        private IUserIdentity userIdentity;
        private FormatDataExportTask exportTask;

        /// <summary>
        /// Имя задачи
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Название группы
        /// </summary>
        public string GroupName { get; private set; }

        /// <summary>
        /// Ключ задачи
        /// </summary>
        public JobKey JobKey { get; private set; }

        /// <summary>
        /// Ключ триггера
        /// </summary>
        public TriggerKey TriggerKey { get; private set; }

        /// <summary>
        /// Токен отмены
        /// </summary>
        public CancellationToken CancellationToken { get; set; }

        /// <summary>
        /// Конструктор по умолчанию для создания объекта планировщиком
        /// </summary>
        [Obsolete("Use FormatDataExportJobInstance(FormatDataExportTask task)")]
        public FormatDataExportJobInstance()
        {
        }

        /// <param name="task">Описатель задачи</param>
        public FormatDataExportJobInstance(FormatDataExportTask task)
        {
            this.exportTask = task;
            this.container = ApplicationContext.Current.Container;

            var identity = this.container.Resolve<IUserIdentity>();
            this.userIdentity = identity.IsAuthenticated
                ? identity
                : new UserIdentity(task.Operator.User.Id,
                    task.Operator.User.Name,
                    Guid.NewGuid().ToString("N"),
                    DynamicDictionary.Create(),
                    "FormatDataExportJobInstance"
                );

            this.FillInfo();
        }

        private void Init(IJobExecutionContext context)
        {
            this.container = (IWindsorContainer)context.JobDetail.JobDataMap.Get(nameof(this.container));
            this.userIdentity = (IUserIdentity)context.JobDetail.JobDataMap.Get(nameof(this.userIdentity));
            this.exportTask = (FormatDataExportTask)context.JobDetail.JobDataMap.Get(nameof(this.exportTask));
            this.CancellationToken = (CancellationToken)context.JobDetail.JobDataMap.Get(nameof(this.CancellationToken));

            this.FillInfo();
        }

        private void FillInfo()
        {
            this.Name = this.exportTask.EntityGroupCodeList.IsEmpty()
                ? "ALL"
                : this.exportTask.EntityGroupCodeList.AggregateWithSeparator(";");
            this.GroupName = this.exportTask.Operator.User.Login;

            this.JobKey = new JobKey(this.Name, this.GroupName);
            this.TriggerKey = new TriggerKey(this.Name, this.GroupName);
        }

        private JobDataMap GetParams()
        {
            return new JobDataMap
            {
                { nameof(this.container), this.container },
                { nameof(this.userIdentity), this.userIdentity },
                { nameof(this.exportTask), this.exportTask },
                { nameof(this.CancellationToken), this.CancellationToken }
            };
        }

        /// <inheritdoc />
        public void Execute(IJobExecutionContext context)
        {
            this.Init(context);

            var logManager = this.container.Resolve<ILogManager>();
            try
            {
                logManager.Debug($"Запуск задачи '{this.JobKey}'");
                this.InternalExecute();
            }
            catch (OperationCanceledException)
            {
                logManager.Debug($"Задача '{this.JobKey}' прервана пользователем");
            }
            finally
            {
                logManager.Debug($"Завершение задачи '{this.JobKey}'");

                GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true);
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
        }

        private void InternalExecute()
        {
            var config = this.container.GetGkhConfig<AdministrationConfig>()
                .FormatDataExport
                .FormatDataExportGeneral;

            ExplicitSessionScope.CallInNewScope(this.userIdentity, () =>
            {
                if (config.StartInExecutor)
                {
                    this.RunInExecutor();
                }
                else
                {
                    this.Run();
                }
            });
        }

        private void Run()
        {
            var job = this.container.Resolve<FormatDataExportJob>();
            try
            {
                job.ExportTask = this.exportTask;
                job.CancellationToken = this.CancellationToken;

                job.Execute();
            }
            catch(Exception e)
            {
                this.container.Resolve<ILogManager>().Error("Экспорт данных системы в РИС ЖКХ", e);
            }
            finally
            {
                this.container.Release(job);
                job = null;
            }
        }

        private void RunInExecutor()
        {
            var taskManager = this.container.Resolve<ITaskManager>();

            var baseParams = new BaseParams { Params = { ["FormatDataExportTaskId"] = this.exportTask.Id } };
            taskManager.CreateTasks(new FormatDataExportTaskProvider(), baseParams);
        }

        /// <summary>
        /// Получить сущность задачи
        /// </summary>
        public IJobDetail GetJob()
        {
            return JobBuilder.Create(this.GetType())
                .WithIdentity(this.JobKey)
                .UsingJobData(this.GetParams())
                .Build();
        }

        /// <summary>
        /// Получить триггер запуска задачи
        /// </summary>
        public ITrigger GetTrigger()
        {
            var triggerBuilder = TriggerBuilder.Create()
                .WithIdentity(this.TriggerKey);

            if (this.exportTask.PeriodType == TaskPeriodType.NoPeriodicity)
            {
                if (this.exportTask.StartNow)
                {
                    triggerBuilder = triggerBuilder.StartNow();
                }
                else
                {
                    var startDate = DateTime.Today
                        .AddHours(this.exportTask.StartTimeHour)
                        .AddMinutes(this.exportTask.StartTimeMinutes);
                    if (startDate <= DateTime.Now)
                    {
                        throw new Exception($"Указанное время запуска уже прошло: {startDate}");
                    }

                    triggerBuilder = triggerBuilder.StartAt(startDate.ToUniversalTime());
                }
            }
            else
            {
                var cronSchedule = CronScheduleBuilder.CronSchedule(this.GetCronExpression());
                triggerBuilder = triggerBuilder.WithSchedule(cronSchedule);
                if (this.exportTask.StartDate.HasValue)
                {
                    triggerBuilder = triggerBuilder.StartAt(this.exportTask.StartDate.Value.ToUniversalTime());
                }
                if (this.exportTask.EndDate.HasValue)
                {
                    triggerBuilder = triggerBuilder.EndAt(this.exportTask.EndDate.Value.ToUniversalTime());
                }
            }

            return triggerBuilder.Build();
        }

        private string GetCronExpression()
        {
            /*
            ┌───────────── секунды (0 - 59)
            | ┌───────────── минуты (0 - 59)
            | │ ┌────────────── часы (0 - 23)
            | │ │ ┌─────────────── день месяца (1 - 31)
            | │ │ │ ┌──────────────── месяц (1 - 12)
            | │ │ │ │ ┌───────────────── день недели (0 - 6) (0 - Вс, 6 - Сб; 7 - Вс)
            | │ │ │ │ │
            * * * * * *
             */
            var min = this.exportTask.StartTimeMinutes.ToString();
            var hour = this.exportTask.StartTimeHour.ToString();
            var dayOfMonth = "*";
            var month = "*";
            var dayOfWeek = "?";
            if (this.exportTask.PeriodType == TaskPeriodType.Monthly)
            {
                dayOfMonth = this.GetDayOfMonth();
                month = this.GetMonth();
            }
            if (this.exportTask.PeriodType == TaskPeriodType.Weekly)
            {
                dayOfMonth = "?";
                dayOfWeek = this.GetDayOfWeek();
            }

            return $"0 {min} {hour} {dayOfMonth} {month} {dayOfWeek}";
        }

        private string GetDayOfMonth()
        {
            if (this.exportTask.StartDayOfWeekList.IsNotEmpty())
            {
                return "?";
            }

            if (this.exportTask.StartDaysList.IsEmpty())
            {
                return "*";
            }

            var dayList = this.exportTask.StartDaysList.OrderBy(x => x).ToList();

            if (dayList[0] == 0)
            {
                return "L";
            }

            return dayList.AggregateWithSeparator(x => x.ToString(), ",");
        }

        private string GetMonth()
        {
            if (this.exportTask.StartMonthList.IsEmpty())
            {
                return "*";
            }

            return this.exportTask.StartMonthList.AggregateWithSeparator(x => x.ToString(), ",");
        }

        private string GetDayOfWeek()
        {
            if (this.exportTask.StartDayOfWeekList.IsEmpty() || this.exportTask.StartDaysList.IsNotEmpty())
            {
                return "?";
            }

            return this.exportTask.StartDayOfWeekList.AggregateWithSeparator(x => (x < 7 ? (x + 1) : 1).ToString(), ",");
        }

        /// <inheritdoc />
        public void Interrupt()
        {
            var config = this.container.GetGkhConfig<AdministrationConfig>()
                .FormatDataExport
                .FormatDataExportGeneral;

            ExplicitSessionScope.CallInNewScope(this.userIdentity, () =>
            {
                if (config.StartInExecutor)
                {
                    throw new NotImplementedException();
                }
            });
        }
    }
}