namespace Bars.B4.Modules.ESIA.Auth.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    using Bars.B4.Modules.ESIA.OAuth20.Entities;
    using Bars.B4.Modules.ESIA.OAuth20.Service;

    /// <summary>
    /// Контроллер ЕСИА OAuth авторизации
    /// </summary>
    public class OauthLoginController : BaseController
    {
        public IEsiaOauthService EsiaOauthService { get; set; }

        public ActionResult Index(EsiaUserInfo userInfo, IEnumerable<string> selectedOrganizationGuid)
        {
            //Если не передан ОГРН выбранной организации, то форма открывается в первый раз
            //Значит подготавливаем данные пользователя
            if (selectedOrganizationGuid == null)
            {
                this.PrepareUserInfo();
                return this.View();
            }

            if (selectedOrganizationGuid.Count() != 1)
            {
                this.ViewData["error"] = "Не удалось определить выбранную организацию.";
                return this.View();
            }
            //Если же передан ОГРН организации, то выполняем логин от её имени
            userInfo.SelectedOrganizationKey = selectedOrganizationGuid.First();
            return this.PerformLoginActions(userInfo);
        }

        /// <summary>
        /// Подготовить данные о пользователе
        /// </summary>
        private void PrepareUserInfo()
        {
            var parameters = this.Request.Params;

            var state = parameters["state"];
            var code = parameters["code"];

            //Обработка ответа от ЕСИА, получение информации по юзеру
            var result = this.EsiaOauthService.HandleEsiaCallback(System.Web.HttpContext.Current, state, code);

            this.ViewData["userInfo"] = result.Data;

            if (!result.Success)
            {
                this.ViewData["error"] = result.Message;

                //Если у юзера несколько организаций, собираем список для комбобокса
                if (result.Data?.OrganizationsList != null && result.Data.OrganizationsList.Count > 1)
                {
                    this.ViewData["organizations"] = result.Data
                        .OrganizationsList
                        .Select(
                            x => new SelectListItem
                            {
                                Text = x.FullName,
                                Value = x.FullName + "###" + x.Ogrn
                            });
                }
            }
            else
            {
                this.RedirectToMain();
            }
        }

        /// <summary>
        /// Выполнить действия логина
        /// </summary>
        private ActionResult PerformLoginActions(EsiaUserInfo userInfo)
        {
            var loginResult = this.EsiaOauthService.PerformLoginActions(userInfo, System.Web.HttpContext.Current);
            if (!loginResult.Success)
            {
                //Если произошла ошибка при логине, то возвращаем на форму
                this.PrepareUserInfo();
                this.ViewData["error"] = loginResult.Message;
                return this.View();
            }

            //Если все ок - редиректим на главную
            return this.RedirectToMain();
        }

        /// <summary>
        /// Редирект на главную страницу
        /// </summary>
        public ActionResult RedirectToMain()
        {
            var appUrl = HttpRuntime.AppDomainAppVirtualPath;

            if (appUrl != "/")
            {
                appUrl = "/" + appUrl;
            }

            var baseUrl = $"{this.Request.Url?.Scheme}://{this.Request.Url?.Authority}{appUrl}";

            this.Response.Redirect(baseUrl);

            return new ContentResult();
        }
    }
}