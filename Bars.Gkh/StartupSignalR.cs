using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Bars.Gkh.StartupSignalR))]

namespace Bars.Gkh
{
    /// <summary>
    /// Инициализация SignalR
    /// <para>For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888</para>
    /// </summary>
    public class StartupSignalR
    {
        /// <summary>
        /// Мапинг роутов SignalR
        /// <para>See https://www.asp.net/signalr/overview/releases/upgrading-signalr-1x-projects-to-20 </para>
        /// </summary>
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
