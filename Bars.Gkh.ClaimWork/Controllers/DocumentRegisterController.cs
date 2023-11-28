namespace Bars.Gkh.ClaimWork.Controllers
{
    using System.Web.Mvc;
    using B4;
    using B4.IoC;
    using DomainService.DocumentRegister;

    /// <summary>
    /// Контроллер реестра документов ПИР
    /// </summary>
    public class DocumentRegisterController : BaseController
    {
        /// <summary>
        /// Список типов документов
        /// </summary>
        /// <returns></returns>
        public ActionResult ListTypeDocument()
        {
            var service = Resolve<IDocumentRegisterService>();

            using (Container.Using(service))
            {
                var result = service.ListTypeDocument();

                return new JsonListResult(result, result.Count);
            }
        }
    }
}