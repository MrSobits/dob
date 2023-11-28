namespace Bars.B4
{
    using System;

    using Bars.B4.Application;
    using Bars.Gkh.B4Events;
    using Bars.Gkh.B4Events.Payloads;

    /// <summary>
    /// MVC приложение
    /// </summary>
    public class MvcApplication : B4MvcApplication
    {
        protected void Session_Start(object sender, EventArgs e)
        {
            this.Session["__dummy"] = "dummy";

            ApplicationContext.Current.Events.GetEvent<SessionStartEvent>().Publish(new SessionStartEventArgs(this.Session.SessionID));

            // если соединение защищённое, то и куки сессии должны быть защищены,
            // иначе приложение не запустится
            if (this.Response.Cookies["ASP.NET_SessionId"] != null && this.Request.IsSecureConnection)
            {
                this.Response.Cookies["ASP.NET_SessionId"].Secure = true;
            }
        }

        protected void Session_End(object sender, EventArgs e)
        {
            ApplicationContext.Current.Events.GetEvent<SessionEndEvent>().Publish(new SessionEndEventArgs(this.Session.SessionID));
        }
    }
}