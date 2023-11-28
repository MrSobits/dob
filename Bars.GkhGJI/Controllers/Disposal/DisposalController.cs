namespace Bars.GkhGji.Controllers
{

    using System.Collections;
    using System.Web.Mvc;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.Gkh.Domain;
    using Gkh.Entities;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using System;

    // Заглушка для того чтобы в существующих регионах там где от него наследовались не полетело 
    public class DisposalController: DisposalController<Disposal>
    {
        // Внимание !! Код override нужно писать не в этом классе, а в DisposalController<T>
    }

    // Класс переделан на для того чтобы в регионах можно было расширят ьсущность через subclass 
    // и при этом не писать дублирующий серверный код
    public class DisposalController<T> : B4.Alt.DataController<T>
        where T: Disposal
    {

        public IDisposalService DisposalService { get; set; }

        public ActionResult GetInfo(BaseParams baseParams)
        {
            return this.DisposalService.GetInfo(baseParams).ToJsonResult();
        }

        public ActionResult ListView(BaseParams baseParams)
        {
            return this.DisposalService.ListView(baseParams).ToJsonResult();
        }

        public ActionResult Export(BaseParams baseParams)
        {
            var export = this.Container.Resolve<IDataExportService>("DisposalDataExport");
            using (this.Container.Using(export))
            {
                return export?.ExportData(baseParams);
            }
        }

        public ActionResult GetAddressList(BaseParams baseParams)
        {
            var result = DisposalService.GetAddressList(baseParams);
            return new JsonListResult(result, result.Count);
        }

        public ActionResult GetFactAddressList(BaseParams baseParams)
        {
            var result = DisposalService.GetFactAddressList(baseParams);
            return new JsonListResult(result, result.Count);
        }

        public ActionResult GetNameList(BaseParams baseParams)
        {
            var result = DisposalService.GetNameList(baseParams);
            return new JsonListResult(result, result.Count);
        }

        public ActionResult ListNullInspection(BaseParams baseParams)
        {
            return this.DisposalService.ListNullInspection(baseParams).ToJsonResult();
        }

        public ActionResult ExportNullInspection(BaseParams baseParams)
        {
            var export = this.Container.Resolve<IDataExportService>("DisposalNullInspectionDataExport");
            using (this.Container.Using(export))
            {
                return export?.ExportData(baseParams);
            }
        }

        public ActionResult AddSurveySubjects(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IDisposalVerificationSubjectService>();
            using (this.Container.Using(service))
            {
                return service.AddSurveySubjects(baseParams).ToJsonResult();
            }
        }

        public ActionResult AddSurveyPurposes(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IDisposalSurveyPurposeService>();
            using (this.Container.Using(service))
            {
                return service.AddSurveyPurposes(baseParams).ToJsonResult();
            }
        }

        public ActionResult AddSurveyObjectives(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IDisposalSurveyObjectiveService>();
            using (this.Container.Using(service))
            {
                return service.AddSurveyObjectives(baseParams).ToJsonResult();
            }
        }

        public ActionResult AddInspFoundations(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IDisposalInsFoundationService>();
            using (this.Container.Using(service))
            {
                return service.AddInspFoundations(baseParams).ToJsonResult();
            }
        }

        /// <summary>
        /// Добавить "НПА проверки"
        /// </summary>
        public ActionResult AddInspFoundationChecks(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IDisposalInsFoundationCheckService>();
            using (this.Container.Using(service))
            {
                return service.AddInspFoundationChecks(baseParams).ToJsonResult();
            }
        }

        /// <summary>
        /// Добавить Требования НПА проверки
        /// </summary>
        public ActionResult AddNormDocItems(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IDisposalInsFoundationCheckService>();
            using (this.Container.Using(service))
            {
                return service.AddNormDocItems(baseParams).ToJsonResult();
            }
        }
    }
}