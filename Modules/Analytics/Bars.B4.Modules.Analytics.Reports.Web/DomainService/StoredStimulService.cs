namespace Bars.B4.Modules.Analytics.Reports.Web.DomainService
{
    using System.IO;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Analytics.Reports.Domain;
    using Bars.B4.Modules.Analytics.Reports.Entities;
    using Bars.B4.Utils.Annotations;

    using Castle.Windsor;

    using Stimulsoft.Report;
    using Stimulsoft.Report.Mvc;

    /// <summary>
    /// Сервис для генерации хранимых стимул отчетов
    /// </summary>
    public class StoredStimulService : IStimulService
    {
        /// <summary>
        /// Код сервиса
        /// </summary>
        public static string Code => "StoredReport";

        private readonly IWindsorContainer container;

        public StoredStimulService(IWindsorContainer container)
        {
            this.container = container;
        }

        /// <inheritdoc />
        public void SaveTemplate(BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs("id", 0L);
            var sti = StiMvcDesignerFx.GetReportObject();

            var reportDomain = this.container.ResolveDomain<StoredReport>();
            try
            {
                var report = reportDomain.GetAll().FirstOrDefault(x => x.Id == id);
                ArgumentChecker.NotNull(report, "Передан неверный идентификатор отчета", "id");

                report.SetTemplate(sti.SaveToByteArray());
                reportDomain.Update(report);
            }
            finally
            {
                this.container.Release(reportDomain);
            }
        }

        /// <inheritdoc />
        public StiReport GetOrCreateTemplate(BaseParams baseParams, bool isNew)
        {
            var sti = new StiReport();

            var reportDomain = this.container.ResolveDomain<StoredReport>();

            try
            {
                var id = baseParams.Params.GetAs("id", 0L);
                var addConn = baseParams.Params.GetAs("addConn", false);

                var report = reportDomain.GetAll().FirstOrDefault(x => x.Id == id);
                ArgumentChecker.NotNull(report, "Передан неверный идентификатор отчета", "id");

                if (!isNew && report.TemplateFile != null && report.TemplateFile.Length > 0)
                {
                    sti.Load(report.TemplateFile);
                }
                else
                {
                    var templateService = new EmptyTemplateService();
                    var tpl = templateService.GetTemplateWithMeta(report.GetDataSources(), addConn, report.GetParams());
                    tpl.Seek(0, SeekOrigin.Begin);
                    sti.Load(tpl);
                }
            }
            finally
            {
                this.container.Release(reportDomain);
            }

            return sti;
        }
    }
}