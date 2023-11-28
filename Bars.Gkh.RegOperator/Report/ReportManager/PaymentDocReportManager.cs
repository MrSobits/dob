namespace Bars.Gkh.RegOperator.Report.ReportManager
{
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Analytics.Reports;
    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Modules.Analytics.Reports.Generators;
    using Bars.B4.Utils;
    using Bars.Gkh.RegOperator.CodedReports;
    using Bars.Gkh.RegOperator.CodedReports.PayDoc;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.PayDoc;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Bars.B4.Modules.Analytics.Reports.Extensions;
    using Bars.Gkh.Entities;

    using Stimulsoft.Report;

    /// <summary>
    /// Менеджер отчетов для платежных документов
    /// </summary>
    internal class PaymentDocReportManager : StimulReportGenerator
    {
        private readonly IRepository<PaymentDocumentTemplate> templateRepo;
        private Dictionary<string, Stream> tplCache;
        private Dictionary<string, Stream> TemplateCache
        {
            get { return this.tplCache ?? (this.tplCache = new Dictionary<string, Stream>()); }
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="templateRepo">Репозиторий для <see cref="PaymentDocumentTemplate"/></param>
        public PaymentDocReportManager(IRepository<PaymentDocumentTemplate> templateRepo)
        {
            this.templateRepo = templateRepo;
        }

        /// <summary>
        /// Создание копий шаблонов, если их не существует
        /// </summary>
        /// <param name="period">Период</param>
        public void CreateTemplateCopyIfNotExist(ChargePeriod period)
        {
            var reports = new ICodedReport[]
            {
                new BaseInvoiceReport(null),
                new InvoiceAndActReport(null),
                new InvoiceRegistryAndActReport(null, null)
            };

            foreach (var report in reports)
            {
                if (!this.templateRepo.GetAll().Any(x => x.Period.Id == period.Id && x.TemplateCode == report.Key))
                {
                    this.CreateTemplateCopy(report, period);
                }
            }
        }

        /// <summary>
        /// Генерация отчета
        /// </summary>
        /// <param name="report">Отчет</param>
        /// <param name="printFormat">Формат</param>
        /// <param name="period">Период</param>
        /// <returns></returns>
        public Stream GenerateReport(ICodedReport report, ReportPrintFormat printFormat, ChargePeriod period)
        {
            var template = this.GetTemplateForGenerating(report.Key, period);

            return this.Generate(report.GetDataSources(), template, null, printFormat,
                new Dictionary<string, object>
                {
                    {
                        "ExportSettings",
                        report.GetExportSettings(printFormat)
                    }
                });
        }

        /// <summary>
        /// Получение файла шаблона
        /// </summary>
        /// <param name="codedReport">Отчет</param>
        /// <param name="periodId">Id периода</param>
        /// <param name="original">Получить исходный шаблон</param>
        /// <returns></returns>
        public Stream ExtractTemplate(ICodedReport codedReport, long periodId, bool original = false)
        {
            var template = this.InternalExtractTemplate(codedReport, periodId, original);

            var stiReport = new StiReport();
            stiReport.Load(template);
            var templateDataSources = stiReport.Dictionary.DataSources.Items.ToList();

            if (!original)
            {
                foreach (var dataSource in codedReport.GetDataSources())
                {
                    templateDataSources.Where(x => x.Name.Equals(dataSource.Name))
                        .ForEach(stiReport.Dictionary.DataSources.Remove);

                    stiReport.AddDataSource(dataSource.Name, dataSource.GetMetaData());
                }
            }

            var result = new MemoryStream();
            result.Seek(0, SeekOrigin.Begin);
            stiReport.Save(result);
            return result;
        }

        private void CreateTemplateCopy(ICodedReport report, ChargePeriod period)
        {
            //берем актуальный шаблон самого последнего месяца с шаблоном
            var previousTemplate = this.templateRepo
                .GetAll()
                .Where(x => x.TemplateCode == report.Key)
                .OrderByDescending(x => x.Period.StartDate)
                .FirstOrDefault();

            byte[] tplBytes;

            if (previousTemplate == null)
            {
                var defaultTpl = report.GetTemplate();
                tplBytes = new byte[defaultTpl.Length];

                defaultTpl.Read(tplBytes, 0, tplBytes.Length);
            }
            else
            {
                tplBytes = previousTemplate.Template;
            }

            var tplCopy = new PaymentDocumentTemplate
            {
                Period = period,
                TemplateCode = report.Key,
                Template = tplBytes
            };
            this.templateRepo.Save(tplCopy);
        }

        private Stream GetTemplateForGenerating(string reportKey, ChargePeriod period)
        {
            var key = "{0}#{1}".FormatUsing(reportKey, period.Id);

            Stream template;
            if (!this.TemplateCache.TryGetValue(key, out template))
            {
                var tpl = this.templateRepo.GetAll()
                        .FirstOrDefault(x => x.TemplateCode == reportKey && x.Period.Id == period.Id);

                if (tpl == null)
                {
                    throw new InvalidOperationException(
                        "Не найден шаблон для платежного документа {0} за период {1}".FormatUsing(
                            reportKey,
                            period.Name));
                }

                template = new MemoryStream(tpl.Template);

                this.TemplateCache[key] = template;
            }

            template.Position = 0;

            return template;
        }

        private Stream InternalExtractTemplate(ICodedReport codedReport, long periodId, bool original = false)
        {
            if (original)
            {
                return codedReport.GetTemplate();
            }
            var customization = this.templateRepo.FirstOrDefault(x => x.TemplateCode == codedReport.Key && x.Period.Id == periodId);

            return customization != null ? new MemoryStream(customization.Template) : codedReport.GetTemplate();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            if (this.TemplateCache.IsNotEmpty())
            {
                this.TemplateCache.Values.ForEach(x => x.Dispose());
                this.TemplateCache.Clear();
            }
        }
    }
}