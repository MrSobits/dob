namespace Bars.Gkh.SignalR
{
    using System.IO;
    using System.Text;

    using Bars.B4;

    using Microsoft.AspNet.SignalR;
    using Microsoft.AspNet.SignalR.Hubs;

    /// <summary>
    /// Динамический ресурс, предоставляющий функционал по генерации
    /// SignalR-прокси
    /// </summary>
    public class SignalrHubsResource : ContentResource
    {
        private readonly string serviceUrl;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="serviceUrl">Базовый адрес сервиса SignalR</param>
        public SignalrHubsResource(string serviceUrl)
        {
            this.serviceUrl = serviceUrl.Trim('/');
        }

        /// <summary>
        /// Конструктор. В качестве базового адреса используется /signalr/signalr
        /// </summary>
        public SignalrHubsResource()
            : this("signalr/signalr")
        {
        }

        /// <summary>
        ///     Возвращает для ресурса поток, доступный только для чтения.
        /// </summary>
        /// <returns />
        public override Stream GetStream()
        {
            var proxyGenerator = GlobalHost.DependencyResolver.Resolve<IJavaScriptProxyGenerator>();
            return new MemoryStream(Encoding.UTF8.GetBytes(proxyGenerator.GenerateProxy(this.serviceUrl)));
        }
    }
}