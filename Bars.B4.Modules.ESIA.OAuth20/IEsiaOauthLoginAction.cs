namespace Bars.B4.Modules.ESIA.OAuth20
{
    using System.Web;

    using Bars.B4.Modules.ESIA.OAuth20.Entities;
    using Bars.B4.Utils;

    /// <summary>
    /// Интерфейс действия, отрабатываемого при авторизации через ЕСИА
    /// </summary>
    public interface IEsiaOauthLoginAction
    {
        /// <summary>
        /// Выполнение логина
        /// </summary>
        IDataResult PerformLogin(EsiaUserInfo userInfo, HttpContext context);
    }
}
