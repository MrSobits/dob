namespace Bars.GkhGji.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using B4;

    using Bars.B4.Utils;
    using Bars.Gkh.Modules.ClaimWork.DomainService;
    using Bars.Gkh.Modules.RegOperator.DomainService;
    using Bars.GkhGji.DomainService;

    /// <summary>
    /// 
    /// </summary>
    public class ComissionMeetingReportController : BaseController
    {
        /// <summary>
        /// Получаю список сущностей для которых реализован IClaimWorkCodedReport + отчет по лс
        /// </summary>
        public ActionResult GetReportList(BaseParams baseParams)
        {
            var comissionMeetingReportProvider = this.Container.Resolve<IComissionMeetingReportService>();

            var reports = comissionMeetingReportProvider.GetReportList(baseParams);

            return new JsonListResult(reports);
        }
        public ActionResult GetResolutionReportList(BaseParams baseParams)
        {
            var comissionMeetingReportProvider = this.Container.Resolve<IComissionMeetingReportService>();

            var reports = comissionMeetingReportProvider.GetResolutionReportList(baseParams);

            return new JsonListResult(reports);
        }

        public ActionResult GetProtocol2025ReportList(BaseParams baseParams)
        {
            var comissionMeetingReportProvider = this.Container.Resolve<IComissionMeetingReportService>();

            var reports = comissionMeetingReportProvider.GetProtocol2025ReportList(baseParams);

            return new JsonListResult(reports);
        }

        public ActionResult GetResolutionDefinitionReportList(BaseParams baseParams)
        {
            var comissionMeetingReportProvider = this.Container.Resolve<IComissionMeetingReportService>();

            var reports = comissionMeetingReportProvider.GetResolutionDefinitionReportList(baseParams);

            return new JsonListResult(reports);
        }


        /// <summary>
        /// Создание и вывод одной печатной формы
        /// </summary>
        public ActionResult ReportPrint(BaseParams baseParams)
        {
            var reportId = baseParams.Params.GetAs<string>("reportId");


            object result= this.Container.Resolve<IComissionMeetingReportService>().GetReport(baseParams);          
            return new JsonNetResult(result);
        }

        /// <summary>
        /// Массовое создание печатных форм и сохранение на ftp
        /// </summary>
        public ActionResult MassReportPrint(BaseParams baseParams)
        {
            var reportId = baseParams.Params.GetAs<string>("reportId");
      

            var result =  this.Container.Resolve<IComissionMeetingReportService>().GetMassReport(baseParams);
            
            return new JsonNetResult(result);
        }

        /// <summary>
        /// Соформировать печатную форму для сведений о собственниках
        /// </summary>
        public ActionResult ReportLawsuitOnwerPrint(BaseParams baseParams)
        {
            var result = this.Container.Resolve<IComissionMeetingReportService>().GetLawsuitOnwerReport(baseParams);

            return new JsonNetResult(result);
        }
    }

    public class ReportInfo
    {
        public string Id;
        public string Name;
        public string Description;
    }
}