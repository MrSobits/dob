namespace Bars.B4.Modules.ESIA.OAuth20.Handlers
{
    using System.Web;

    /// <summary>
    /// Обработчик логаута
    /// </summary>
    public class OauthLogoutHandler : IHttpHandler
    {
        /// <summary>
        ///     Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler" /> interface.
        /// </summary>
        /// <param name="context">
        ///     An <see cref="T:System.Web.HttpContext" /> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests.
        /// </param>
        public void ProcessRequest(HttpContext context)
        {
            var request = HttpContext.Current.Request;
            var appUrl = HttpRuntime.AppDomainAppVirtualPath;

            if (appUrl != "/")
                appUrl = "/" + appUrl;

            var baseUrl = $"{request.Url.Scheme}://{request.Url.Authority}{appUrl}";

            context.Response.Redirect(baseUrl);
        }

        /// <summary>
        ///     Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler" /> instance.
        /// </summary>
        /// <returns>true if the <see cref="T:System.Web.IHttpHandler" /> instance is reusable; otherwise, false.
        /// </returns>
        public bool IsReusable => true;
    }
}
