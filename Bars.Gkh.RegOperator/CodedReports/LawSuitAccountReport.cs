﻿namespace Bars.Gkh.RegOperator.CodedReports
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;

    using Bars.B4.Application;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Analytics.Data;
    using Bars.B4.Modules.Analytics.Reports;
    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Modules.Analytics.Reports.Generators;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Modules.ClaimWork;
    using Bars.Gkh.Modules.ClaimWork.DomainService;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.Gkh.Modules.ClaimWork.Enums;
    using Bars.Gkh.RegOperator.DataProviders;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Properties;

    using Castle.Windsor;

    using Newtonsoft.Json;

    using Stimulsoft.Report.Export;

    /// <summary>
    /// Реализация BaseCodedReport, IClaimWorkCodedReport для искового заявления
    /// </summary>
    public class LawSuitAccountReport : ExportFormatHelper, IClaimWorkCodedReport
    {
        /// <summary>
        /// Container
        /// </summary>
        [JsonIgnore]
        public IWindsorContainer Container { get; set; }

        protected override byte[] Template => Resources.LawSuitOwnerInfoReport;

        public string DocumentId { get; set; }

        public string Id => "LawSuitAccountReport";

        public override string Name => "Исковое заявление (лицевой счет)";

        public override string Description
        {
            get { return this.Name; }
            set { }
        }

        public string CodeForm => "LawSuitAccount";

        public string OutputFileName { get; set; } = "Исковое заявление";

        public long[] OnwerInfoIds { get; set; }

        public ClaimWorkDocumentType DocumentType => ClaimWorkDocumentType.Lawsuit;

        public ClaimWorkReportInfo ReportInfo { get; set; }

        public Stream ReportFileStream { get; set; }

        public long[] OwnerInfoIds { get; set; }

        public override IEnumerable<IDataSource> GetDataSources()
        {
            return new Collection<IDataSource>
            {
                new CodedDataSource("Список лицевых счетов", new LawSuitOwnerDataProvider(ApplicationContext.Current.Container)
                {
                    OwnerInfoIds = this.ReportInfo?.OnwerInfoIds ?? new long[0]
                })
            };
        }

        public override StiExportSettings GetExportSettings(ReportPrintFormat format)
        {
            if (format == ReportPrintFormat.docx)
            {
                return new StiWord2007ExportSettings
                {
                    RemoveEmptySpaceAtBottom = false
                };
            }

            return null;
        }

        public void Generate()
        {
            var generator = this.Container.Resolve<ICodedReportManager>();
            var petitionDomain = this.Container.ResolveDomain<Petition>();
            var accountDetailDomain = this.Container.ResolveDomain<ClaimWorkAccountDetail>();
            try
            {
                if (this.DocumentId.IsEmpty())
                {
                    throw new Exception("Не найдено исковое заявление");
                }

                var lawSuit = petitionDomain.Get(this.DocumentId.ToLong());

                if (lawSuit == null)
                {
                    throw new Exception("Не найдено исковое заявление");
                }

                var accountDetail = accountDetailDomain.Get(this.ReportInfo.OnwerInfoIds.FirstOrDefault());

                OperatorExportFormat format = GetExtension();
                this.OutputFileName =
                    $"Исковое заявление ({accountDetail.PersonalAccount.PersonalAccountNum} - {DateTime.Now.Date.ToShortDateString()}).{format}"
                        .Replace("\"", "");

                this.ReportFileStream = generator.GenerateReport(this, null, (ReportPrintFormat)format);
            }
            finally
            {
                this.Container.Release(generator);
                this.Container.Release(petitionDomain);
                this.Container.Release(accountDetailDomain);
            }
        }
    }
}