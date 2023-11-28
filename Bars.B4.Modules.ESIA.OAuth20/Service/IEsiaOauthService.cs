namespace Bars.B4.Modules.ESIA.OAuth20.Service
{
    using System.Web;

    using Bars.B4.Modules.ESIA.OAuth20.Entities;

    /// <summary>
    /// Сервис авторизации в ЕСИА через OAuth
    /// </summary>
    public interface IEsiaOauthService
    {
        /// <summary>
        /// Обработать ответ от ЕСИА
        /// </summary>
        IDataResult<EsiaUserInfo> HandleEsiaCallback(HttpContext context, string state, string code);

        /// <summary>
        /// Выполнить действия логина
        /// </summary>
        IDataResult PerformLoginActions(EsiaUserInfo userInfo, HttpContext context);
    }
}
