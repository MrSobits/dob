namespace Bars.Gkh
{
    using System.Web.Mvc;
    using System.Web.Security;

    using Bars.B4;

    public class SessionTimeoutActionHandler : IActionExecuteHandler
    {
        
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var authCookie = filterContext.HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName];

            if (authCookie == null)
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest() && filterContext.HttpContext.Request.AppRelativeCurrentExecutionFilePath != "~/action/setup/login")
                {
                    throw new AuthorizationFailureException("AjaxAuthError");
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext actionExecutedContext)
        {
        }

        public void OnResultExecuting(ResultExecutingContext resultExecutingContext)
        {
        }

        public void OnResultExecuted(ResultExecutedContext resultExecutedContext)
        {
        }
    }
}