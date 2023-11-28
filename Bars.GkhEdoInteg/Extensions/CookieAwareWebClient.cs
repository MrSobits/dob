namespace Bars.GkhEdoInteg.Extensions
{
    using System;
    using System.Net;

    /// <summary>
    /// Реализация <see cref="WebClient"/> с куки сессией
    /// </summary>
    [System.ComponentModel.DesignerCategory("Code")]
    public class CookieAwareWebClient : WebClient
    {
        private readonly CookieContainer cookieContainer = new CookieContainer();

        /// <summary>
        /// Контейнер куки запроса
        /// </summary>
        public CookieContainer CookieContainer
        {
            get { return cookieContainer; }
        }

        /// <summary>
        /// Переопределение метода получение веб-запроса
        /// </summary>
        /// <param name="address">Uri-адрес</param>
        /// <returns>Веб-запрос</returns>
        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest request = base.GetWebRequest(address);
            HttpWebRequest webRequest = request as HttpWebRequest;
            if (webRequest != null)
            {
                webRequest.CookieContainer = cookieContainer;
            }
            return request;
        }
    }
}
