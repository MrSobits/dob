namespace Bars.Gkh.RegOperator.Controllers
{
	using System.Web.Mvc;
	using B4;
	using DomainService;
	using Entities;

	/// <summary>
	/// Контроллер для "Импортируемая оплата"
	/// </summary>
	public class ImportedPaymentController : B4.Alt.DataController<ImportedPayment>
    {
		/// <summary>
		/// Сопоставить ЛС
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
        public ActionResult ComparePersonalAccount(BaseParams baseParams)
        {
            var result = Resolve<IImportedPaymentService>().ComparePersonalAccount(baseParams);
			return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
		}
    }
}