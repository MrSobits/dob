﻿namespace Bars.Gkh.RegOperator.CodedReports.PayDoc
{
    using Bars.B4.Application;
    using Bars.B4.Modules.Analytics.Data;
    using Bars.B4.Modules.Analytics.Reports;
    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.Gkh.ConfigSections.RegOperator;
    using Bars.Gkh.Enums;
    using Bars.Gkh.RegOperator.DataProviders.Meta;
    using Bars.Gkh.RegOperator.DataProviders.PayDoc;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.PayDoc;
    using Bars.Gkh.RegOperator.Properties;
    using Bars.Gkh.Utils;
    using Stimulsoft.Report.Export;
    using System.Collections.Generic;

    internal class BaseInvoiceReport : BaseCodedReport
    {
        private readonly IEnumerable<PaymentDocumentSnapshot> accounts;

        public BaseInvoiceReport(IEnumerable<PaymentDocumentSnapshot> accounts)
        {
            this.accounts = accounts;
        }

        protected override byte[] Template
        {
            get
            {
                return Resources.PaymentDocumentReceipt;
            }
        }

        public override string Name
        {
            get
            {
                return "СЧЁТ-ИЗВЕЩЕНИЕ на оплату взноса на капитальный ремонт";
            }
        }

        public override string Description { get; set; }

        public override IEnumerable<IDataSource> GetDataSources()
        {
            return new[]
            {
                new CodedDataSource("Записи", 
                    new BaseInvoiceDataProvider<InvoiceInfo>(ApplicationContext.Current.Container, this.accounts))
            };
        }

        public override StiExportSettings GetExportSettings(ReportPrintFormat format)
        {
            if (format == ReportPrintFormat.pdf)
            {
                var config = ApplicationContext.Current.Container.GetGkhConfig<RegOperatorConfig>().PaymentDocumentConfigContainer.PaymentDocumentConfigCommon.QualityOptions;
                return new StiPdfExportSettings
                {
                    ImageQuality = config.ImageQuality,
                    ImageResolution = config.Dpi,
                    EmbeddedFonts = config.EmbeddedFonts == YesNo.Yes,
                    ImageCompressionMethod = config.ImageCompressionMethod,
                    Compressed = config.Compressed == YesNo.Yes,
                    StandardPdfFonts = config.UseStandardPdfFonts == YesNo.Yes
                };
            }

            return base.GetExportSettings(format);
        }
    }
}