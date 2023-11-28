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
    using Bars.Gkh.Report;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Properties;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol197;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Properties;
    using Bars.GkhGji.Report;

    using Stimulsoft.Report;

    public class PostRegystryReport : GjiBaseStimulReport, IComissionMeetingCodedReport
    {
		public PostRegystryReport()
            : base(new ReportTemplateBinary(Resources.ChelyabinskProtocol))
        {
        }

        #region Properties

        public string ReportId => "PostRegystry";
        public string DocId { get; set; }
        protected long comissId;
        public override string Id
        {
            get { return "PostRegystry"; }
        }

        public override string CodeForm
        {
			get { return "PostRegystry"; }
        }

        public override string Name
        {
			get { return "Реестр для почты"; }
        }

        public override string Description
        {
			get { return "Реестр для почты"; }
        }

        public string OutputFileName { get; set; } = "Реестр для почты";

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
            this.comissId = userParamsValues.GetValue<object>("comissionId").ToLong();

        }

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
            {
                new TemplateInfo
                {
                    Code = "PostRegystry",
                    Name = "PostRegystry",
                    Description = "Реестр для почты",
                    Template = Resources.PostRegistryReport
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
			this.CodeTemplate = "PostRegystry";
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            Report["recIds"] = this.RecordIds;
            Report["comissId"] = this.comissId;
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
                    $"Почтовый реестр.xlsx";

            }
            finally
            {
                
            }
        }
    }
}