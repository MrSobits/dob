namespace Bars.GkhGji.Regions.BaseChelyabinsk.Report
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

    public class OSSPPostRegistryComissionReport : GjiBaseStimulReport, IComissionMeetingCodedReport
    {
        public OSSPPostRegistryComissionReport()
            : base(new ReportTemplateBinary(Resources.ChelyabinskProtocol))
        {
        }

        #region Properties

        public string ReportId => "OSSPPostRegistryComissionReport";
        public string DocId { get; set; }
        protected long comissId;
        public override string Id
        {
            get { return "OSSPPostRegistryComissionReport"; }
        }

        public override string CodeForm
        {
            get { return "OSSPPostRegistryComissionReport"; }
        }

        public override string Name
        {
            get { return "Почтовый реестр - ССП (Комиссия)"; }
        }

        public override string Description
        {
            get { return "Почтовый реестр - ССП (Комиссия)"; }
        }

        public string OutputFileName { get; set; } = "Почтовый реестр ССП";

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
                    Code = "OSSPPostRegistryComissionReport",
                    Name = "OSSPPostRegistryComissionReport",
                    Description = "Почтовый реестр - ССП (Комиссия)",
                    Template = Resources.OSSPPostRegistryComissionReport
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
            this.CodeTemplate = "OSSPPostRegistryComissionReport";
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
                    throw new Exception("Не найдено постановление");
                }


                StiExportFormat format = StiExportFormat.Excel2007;
                this.OutputFileName =
                    $"Почтовый реестр ССП.xlsx";

            }
            finally
            {

            }
        }
    }
}