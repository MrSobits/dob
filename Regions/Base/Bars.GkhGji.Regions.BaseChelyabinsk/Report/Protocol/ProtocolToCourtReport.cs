namespace Bars.GkhGji.Regions.BaseChelyabinsk.Report.Protocol197
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Report;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Properties;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol197;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Properties;
    using Bars.GkhGji.Report;

    using Stimulsoft.Report;
    using Stimulsoft.Report.Export;

    public class ProtocolToCourtReport : GjiBaseStimulReport, IComissionMeetingCodedReport
    {
		public ProtocolToCourtReport()
            : base(new ReportTemplateBinary(Resources.PROT2025TOCOURTReport))
        {
        }

        #region Properties

        ///// <summary>
        ///// Настройки экспорта
        ///// </summary>
        public override StiExportSettings ExportSettings
        {
            get
            {
                return new StiWord2007ExportSettings
                {
                    RemoveEmptySpaceAtBottom = false
                };
            }
        }

        public string ReportId => "ProtocolToCourt";
        public string DocId { get; set; }
        protected long comissId;
        public override string Id
        {
            get { return "ProtocolToCourt"; }
        }

        public override string CodeForm
        {
            get { return "Protocol"; }
        }

        public override string Name
        {
			get { return "Сопровод в суд"; }
        }

        public override string Description
        {
			get { return "Сопровод в суд"; }
        }

        public string OutputFileName { get; set; } = "Сопровод в суд";

        public ComissionMeetingReportInfo ReportInfo { get; set; }

        public Stream ReportFileStream { get; set; }

        public override StiExportFormat ExportFormat
        {
            get { return StiExportFormat.Word2007; }
        }

        protected override string CodeTemplate { get; set; }

        #endregion Properties

        protected string RecordIds;

        protected string CurrUser;

        protected string CurrUserPhone;

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            this.RecordIds = userParamsValues.GetValue<object>("recIds").ToString();
            this.CurrUser = userParamsValues.GetValue<object>("currUser").ToString();
            this.CurrUserPhone = userParamsValues.GetValue<object>("currUserPhone").ToString();
        }

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
            {
                new TemplateInfo
                {
                    Code = "ProtocolToCourt",
                    Name = "ProtocolToCourt",
                    Description = "Сопровод в суд",
                    Template = Resources.PROT2025TOCOURTReport
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
			this.CodeTemplate = "ProtocolToCourt";
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            Report["recIds"] = this.RecordIds;
            Report["currUser"] = this.CurrUser;
            Report["currUserPhone"] = this.CurrUserPhone;
        }

        public void GenerateMassReport()
        {          

           
            try
            {
                
                StiExportFormat format = StiExportFormat.Word2007;
                this.OutputFileName =
                    $"Сопровод в суд.docx";

            }
            finally
            {
                
            }
        }
    }
}