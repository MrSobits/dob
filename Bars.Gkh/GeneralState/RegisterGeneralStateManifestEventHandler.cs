namespace Bars.Gkh.GeneralState
{
    using Bars.B4.Application;
    using Bars.B4.Events;
    using Bars.B4.IoC.Lifestyles.SessionLifestyle;

    using Castle.Windsor;

    /// <summary>
    /// Событие для регистрации манифестов обобщенных состояний
    /// </summary>
    public class RegisterGeneralStateManifestEventHandler : EventHandlerBase<AppStartEventArgs>
    {
        private IWindsorContainer container => ApplicationContext.Current.Container;

        /// <summary>Метод, вызываемый при возникновении события.</summary>
        /// <param name="args">Аргумент события.</param>
        public override void OnEvent(AppStartEventArgs args)
        {
            ExplicitSessionScope.CallInNewScope(() => this.container.Resolve<IGeneralStateProvider>().Init());
        }
    }
}