namespace Bars.Gkh.ExecutionAction.ExecutionActionScheduler
{
    using Bars.B4.Application;
    using Bars.B4.Events;
    using Bars.B4.IoC;
    using Bars.B4.IoC.Lifestyles.SessionLifestyle;

    using Castle.Windsor;

    /// <summary>
    /// Постановка в очередь задач с флагом обязательного выполнения
    /// </summary>
    public class InitExecutionActionScheduler : EventHandlerBase<AppStartEventArgs>
    {
        private IWindsorContainer Container => ApplicationContext.Current.Container;

        /// <summary>
        /// Хендлер события
        /// </summary>
        /// <param name="args">
        /// Аргументы
        /// </param>
        public override void OnEvent(AppStartEventArgs args)
        {
            ExplicitSessionScope.CallInNewScope(() =>
            {
                this.Container.UsingForResolved<IExecutionActionService>((ioc, service) => service.RestoreJobs());
            });
        }
    }
}