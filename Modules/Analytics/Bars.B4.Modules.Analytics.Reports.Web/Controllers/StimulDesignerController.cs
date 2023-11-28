namespace Bars.B4.Modules.Analytics.Reports.Web.Controllers
{
    using System.Web.Mvc;

    using Bars.B4.Application;
    using Bars.B4.Modules.Analytics.Reports.Web.DomainService;
    using Bars.B4.Utils.Annotations;
    
    using Stimulsoft.Report.Mvc;

    /// <summary>
    /// Controller Stimul Designer
    /// </summary>
    public class StimulDesignerController : BaseController
    {
        /// <summary>
        /// Создать шаблон
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public ActionResult CreateTemplate(BaseParams baseParams)
        {
            var type = baseParams.Params.GetAs("type", "");
            return this.GetOrCreateTemplate(baseParams, type, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult DesignerEvent()
        {
            return StiMvcDesignerFx.DesignerEventResult();
        }

        /// <summary>
        /// Получить локализацию
        /// </summary>
        /// <returns></returns>
        public ActionResult GetLocalization()
        {
            return
                StiMvcDesignerFx.GetLocalizationResult(ApplicationContext.Current.MapPath("~/net45/Content/Localization/ru.xml"));
        }

        /// <summary>
        /// Получить шаблон
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public ActionResult GetTemplate(BaseParams baseParams)
        {
            var type = baseParams.Params.GetAs("type", string.Empty);
            return this.GetOrCreateTemplate(baseParams, type, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Index(long id)
        {
            ArgumentChecker.NotLessThanZeroOrZero(id, "Не передан идентификатор отчета");
            return this.View();
        }

        /// <summary>
        /// Сохранить шаблон
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public ActionResult SaveTemplate(BaseParams baseParams)
        {
            var type = baseParams.Params.GetAs("type", string.Empty);
            var id = baseParams.Params.GetAs("id", 0L);
            ArgumentChecker.NotLessThanZeroOrZero(id, "Не передан идентификатор отчета");

            var stimulService = this.Container.Resolve<IStimulService>(type);
            try
            {
                stimulService.SaveTemplate(baseParams);
                return StiMvcDesignerFx.SaveReportResult();
            }
            catch
            {
                return StiMvcDesignerFx.SaveReportResult("Не удалось сохранить шаблон");
            }
            finally
            {
                this.Container.Release(stimulService);
            }
        }
        
        private ActionResult GetOrCreateTemplate(BaseParams baseParams, string type, bool isNew)
        {
            var id = baseParams.Params.GetAs("id", 0L);
            ArgumentChecker.NotLessThanZeroOrZero(id, "Не передан идентификатор отчета");
            
            var stimulService = this.Container.Resolve<IStimulService>(type);
            try
            {
                var sti = stimulService.GetOrCreateTemplate(baseParams, isNew);
                sti.Dictionary.Synchronize();

                return StiMvcDesignerFx.GetReportTemplateResult(sti);
            }
            finally
            {
                this.Container.Release(stimulService);
            }
        }
    }
}