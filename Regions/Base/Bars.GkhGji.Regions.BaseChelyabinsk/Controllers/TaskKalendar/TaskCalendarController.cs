namespace Bars.GkhGji.Regions.BaseChelyabinsk.Controllers
{
	using System;
	using System.Web.Mvc;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.GkhCalendar.DomainService;
    using Bars.GkhGji.Regions.BaseChelyabinsk.DomainService;
    using Entities;

	/// <summary>
	/// Контроллер для День производственного календаря
	/// </summary>
    public class TaskCalendarController : B4.Alt.DataController<TaskCalendar>
    {
		/// <summary>
		/// Получить список дней
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Список дней</returns>
        public ActionResult GetDaysList(BaseParams baseParams)
        {
            var result = this.Resolve<ITaskCalendarService>().GetDays(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

		/// <summary>
		/// Получить дату после заданного количества рабочих дней
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Дата</returns>
		public ActionResult GetDateAfterWorkDays(BaseParams baseParams)
		{
			var date = baseParams.Params.GetAs<DateTime>("date");
			var workDaysCount = baseParams.Params.GetAs<uint>("workDaysCount");

			var result = this.Resolve<IIndustrialCalendarService>().GetDateAfterWorkDays(date, workDaysCount);
			return new JsonNetResult(result);
		}

		public ActionResult GetListLicRequest(BaseParams baseParams)
		{
			var resolutionService = Container.Resolve<ITaskCalendarService>();
			try
			{
				return resolutionService.GetListDisposal(baseParams).ToJsonResult();
			}
			catch
			{
				return null;
			}
			finally
			{

			}
		}
		public ActionResult GetListProtocolsGji(BaseParams baseParams)
		{
			var resolutionService = Container.Resolve<ITaskCalendarService>();
			try
			{
				return resolutionService.GetListProtocolsGji(baseParams).ToJsonResult();
			}
			catch
			{
				return null;
			}
			finally
			{
			}
		}

		public ActionResult GetListProtocolsInCommission(BaseParams baseParams)
		{
			var resolutionService = Container.Resolve<ITaskCalendarService>();
			try
			{
				return resolutionService.GetListProtocolsInCommission(baseParams).ToJsonResult();
			}
			catch
			{
				return null;
			}
			finally
			{
			}
		}

		public ActionResult GetListResolutionsInCommission(BaseParams baseParams)
		{
			var resolutionService = Container.Resolve<ITaskCalendarService>();
			try
			{
				return resolutionService.GetListResolutionsInCommission(baseParams).ToJsonResult();
			}
			catch
			{
				return null;
			}
			finally
			{
			}
		}

		public ActionResult GetListResolutionDefinitionsInCommission(BaseParams baseParams)
		{
			var resolutionService = Container.Resolve<ITaskCalendarService>();
			try
			{
				return resolutionService.GetListResolutionDefinitionsInCommission(baseParams).ToJsonResult();
			}
			catch
			{
				return null;
			}
			finally
			{
			}
		}

		public ActionResult GetListListComissionsInDocument(BaseParams baseParams)
		{
			var resolutionService = Container.Resolve<ITaskCalendarService>();
			try
			{
				return resolutionService.GetListListComissionsInDocument(baseParams).ToJsonResult();
			}
			catch
			{
				return null;
			}
			finally
			{
			}
		}
		public ActionResult GetListCourtPractice(BaseParams baseParams)
		{
			var resolutionService = Container.Resolve<ITaskCalendarService>();
			try
			{
				return resolutionService.GetListCourtPractice(baseParams).ToJsonResult();
			}
			finally
			{

			}
		}
	}
}