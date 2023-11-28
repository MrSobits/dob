namespace Bars.B4.Modules.ESIA.Auth
{
    using Bars.B4.IoC;
    using Bars.B4.Modules.ESIA.Auth.Actions;
    using Bars.B4.Modules.ESIA.Auth.Controllers;
    using Bars.B4.Modules.ESIA.Auth.Interceptors;
    using Bars.B4.Modules.ESIA.OAuth20;
    using Bars.B4.Windsor;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Класс модуля профиля B4 пользователя для RMS.
    /// </summary>
    [Bars.B4.Utils.Display("Модуль включающий авторизацию ESIA в B4 приложении")]
    [Bars.B4.Utils.Description("")]
    [Bars.B4.Utils.CustomValue("Version", 1)]
    [Bars.B4.Utils.CustomValue("Uid", "eab36a7d-aef5-46f0-8f31-711e9c869504")]
    public class Module : AssemblyDefinedModule
    {
        //protected override void SetPredecessors()
        //{
        //    SetPredecessor<Bars.B4.Modules.ExtJs.Module>();
        //    SetPredecessor<Bars.B4.Modules.ESIA.Module>();

        //    base.SetPredecessors();
        //}

        /// <summary>
        /// Метод конфигурации модуля.
        /// </summary>
        public override void Install()
        {
            this.Container.RegisterTransient<IResourceManifest, ResourceManifest>("ESIA.Auth resources");

            this.Container.ReplaceController<ESIALoginController>("Login");
            this.Container.RegisterController<EsiaOperatorController>();
            this.Container.RegisterController<OauthLoginController>();

            this.Container.RegisterTransient<dk.nita.saml20.Actions.IAction, ESIAuth>();
            this.Container.RegisterTransient<IEsiaOauthLoginAction, EsiaOauthLogin>();

            this.Container.RegisterDomainInterceptor<Operator, OperatorEsiaInterceptor>();
        }
    }
}