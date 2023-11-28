namespace Bars.GkhGji.Regions.BaseChelyabinsk.Report.Protocol
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Report;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Properties;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol197;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Properties;
    using Bars.GkhGji.Report;

    using Stimulsoft.Report;

    public class CourtPostRegistryReport : GjiBaseStimulReport, IComissionMeetingCodedReport
    {
        public CourtPostRegistryReport()
            : base(new ReportTemplateBinary(Resources.ChelyabinskProtocol))
        {
        }

        #region Properties

        public string ReportId => "CourtPostRegistryReport";
        public string DocId { get; set; }
        protected long comissId;
        public override string Id
        {
            get { return "CourtPostRegistryReport"; }
        }

        public override string CodeForm
        {
            get { return "CourtPostRegistryReport"; }
        }

        public override string Name
        {
            get { return "Почтовый реестр 20.25"; }
        }

        public override string Description
        {
            get { return "Почтовый реестр  20.25"; }
        }

        public string OutputFileName { get; set; } = "Почтовый реестр 20.25";

        public ComissionMeetingReportInfo ReportInfo { get; set; }

        public Stream ReportFileStream { get; set; }

        public override StiExportFormat ExportFormat
        {
            get { return StiExportFormat.Excel2007; }
        }

        protected override string CodeTemplate { get; set; }

        #endregion Properties

        protected string RecordIds;

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            this.RecordIds = userParamsValues.GetValue<object>("recIds").ToString();
        }

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
            {
                new TemplateInfo
                {
                    Code = "CourtPostRegistryReport",
                    Name = "CourtPostRegistryReport",
                    Description = "Почтовый реестр 20.25",
                    Template = Resources.CourtPostRegistryReport
                }
            };
        }

        public override Stream GetTemplate()
        {
            this.GetCodeTemplate();
            return base.GetTemplate();
        }

        private void GetCodeTemplate()
        {
            this.CodeTemplate = "CourtPostRegistryReport";
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            Report["recIds"] = this.RecordIds;
        }

        public void GenerateMassReport()
        {


            try
            {
                if (this.DocId.IsEmpty())
                {
                    throw new Exception("Не найден протокол");
                }


                StiExportFormat format = StiExportFormat.Excel2007;
                this.OutputFileName =
                    $"Почтовый реестр Суд.xlsx";

            }
            finally
            {

            }
        }
    }
}