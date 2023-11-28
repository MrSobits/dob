namespace Bars.Gkh.Quartz
{
    using Bars.B4.IoC;

    using Castle.Windsor;

    using global::Quartz;

    /// <summary>
    /// Переопределение <see cref="Bars.B4.Modules.Quartz.WindsorJobListener"/>, чтобы не писало в лог инфо кучу сообщений
    /// </summary>
    public class GkhWindsorJobListener : IJobListener
    {
        private readonly IWindsorContainer container;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="container">Контейнер</param>
        public GkhWindsorJobListener(IWindsorContainer container)
        {
            this.container = container;
        }

        /// <inheritdoc />
        public void JobToBeExecuted(IJobExecutionContext context)
        {
            if (context.JobInstance != null)
            {
                this.container.BuildUp(context.JobInstance);
            }
        }

        /// <inheritdoc />
        public void JobExecutionVetoed(IJobExecutionContext context)
        {
        }

        /// <inheritdoc />
        public void JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException)
        {
        }

        /// <inheritdoc />
        public string Name => "WindsorJobListener";
    }
}