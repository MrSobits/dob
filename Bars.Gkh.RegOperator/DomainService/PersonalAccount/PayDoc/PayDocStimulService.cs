﻿namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Analytics.Reports.Web.DomainService;
    using Bars.B4.Utils.Annotations;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.PayDoc;

    using Castle.Windsor;

    using Stimulsoft.Report;
    using Stimulsoft.Report.Mvc;

    /// <summary>
    /// Сервис для генерации стимул отчетов для платежных документов
    /// </summary>
    public class PayDocStimulService : IStimulService
    {
        /// <summary>
        /// Код сервиса
        /// </summary>
        public static string Code => "PaymentDocumentTemplate";

        private readonly IWindsorContainer container;

        public PayDocStimulService(IWindsorContainer container)
        {
            this.container = container;
        }

        /// <inheritdoc />
        public void SaveTemplate(BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs("id", 0L);
            var sti = StiMvcDesignerFx.GetReportObject();

            var reportDomain = this.container.ResolveDomain<PaymentDocumentTemplate>();
            try
            {
                var report = reportDomain.GetAll().FirstOrDefault(x => x.Id == id);
                ArgumentChecker.NotNull(report, "Передан неверный идентификатор отчета", "id");

                report.Template = sti.SaveToByteArray();

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

            var reportDomain = this.container.ResolveDomain<PaymentDocumentTemplate>();
            try
            {
                var id = baseParams.Params.GetAs("id", 0L);

                var report = reportDomain.GetAll().FirstOrDefault(x => x.Id == id);
                ArgumentChecker.NotNull(report, "Передан неверный идентификатор отчета", "id");

                if (!isNew && report.Template != null && report.Template.Length > 0)
                {
                    sti.Load(report.Template);
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