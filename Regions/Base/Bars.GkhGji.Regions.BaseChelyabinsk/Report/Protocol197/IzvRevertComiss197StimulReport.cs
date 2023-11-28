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

    public class IzvRevertComiss197StimulReport : GjiBaseStimulReport, IComissionMeetingCodedReport
    {
		public IzvRevertComiss197StimulReport()
            : base(new ReportTemplateBinary(Resources.ChelyabinskProtocol))
        {
        }

        #region Properties
        public string ReportId => "OprRevertComissProtocol197";

        public override string Id
        {
            get { return "OprRevertComissProtocol197"; }
        }

        public override string CodeForm
        {
			get { return "Protocol197"; }
        }

        public override string Name
        {
			get { return "Определение о возврате материалов дела"; }
        }

        public override string Description
        {
			get { return "Определение о возврате материалов дела об АП"; }
        }

        public string OutputFileName { get; set; } = "Определение о возврате материалов дела";

        public override StiExportFormat ExportFormat
        {
            get { return StiExportFormat.Word2007; }
        }

        protected override string CodeTemplate { get; set; }

        #endregion Properties

        protected long DocumentId;
        protected long comissId;
        public string DocId { get; set; }
        public ComissionMeetingReportInfo ReportInfo { get; set; }

        public Stream ReportFileStream { get; set; }

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            this.DocumentId = userParamsValues.GetValue<object>("DocumentId").ToLong();
            this.comissId = userParamsValues.GetValue<object>("comissionId").ToLong();

        }

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
            {
                new TemplateInfo
                {
                    Code = "OprRevertComissProtocol197",
                    Name = "OprRevertComissProtocol197",
                    Description = "Определение о возврате материалов дела",
                    Template = Resources.OPRED4_REP197
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
			this.CodeTemplate = "OprRevertComissProtocol197";
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var protocol = this.Container.ResolveDomain<Protocol197>().Get(this.DocumentId);
            if (protocol == null)
            {
                throw new ReportProviderException("Не удалось получить протокол");
            }
            Report["Id"] = protocol.Id;
            if (comissId > 0)
            {
                Report["comissId"] = comissId;
            }
            else
            {
                var cm = this.Container.ResolveDomain<ComissionMeeting>().GetAll()
                    .FirstOrDefault(x => x.ZonalInspection == protocol.ComissionMeeting.ZonalInspection && x.CommissionDate.Date == protocol.DateOfProceedings.Value.Date);
                Report["comissId"] = cm.Id;
            }
        }

        public void GenerateMassReport()
        {

            var DocumentGjiDomain = this.Container.Resolve<IDomainService<Entities.Protocol197.Protocol197>>();
            try
            {
                if (this.DocId.IsEmpty())
                {
                    throw new Exception("Не найден протокол");
                }
                this.DocumentId = Convert.ToInt64(this.DocId);

                var ownerInfo = DocumentGjiDomain.Get(DocumentId);

                StiExportFormat format = StiExportFormat.Word2007;
                this.OutputFileName =
                    $"Определение о возврате материалов по делу {ownerInfo.Inspection.InspectionNumber}  ({ownerInfo.DocumentNumber} - {ownerInfo.DocumentDate.Value.ToString("dd.MM.yyyy")}).docx";

            }
            finally
            {
                this.Container.Release(DocumentGjiDomain);
            }
        }
    }
}