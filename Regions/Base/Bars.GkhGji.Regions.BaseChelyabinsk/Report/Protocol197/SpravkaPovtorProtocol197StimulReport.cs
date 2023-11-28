namespace Bars.GkhGji.Regions.BaseChelyabinsk.Report.Protocol197
{
    using System;
    using System.Collections.Generic;
    using System.IO;
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

    public class SpravkaPovtorProtocol197StimulReport : GjiBaseStimulReport, IComissionMeetingCodedReport
    {
		public SpravkaPovtorProtocol197StimulReport()
            : base(new ReportTemplateBinary(Resources.ChelyabinskProtocol))
        {
        }

        #region Properties
        public string ReportId => "SpravkaPovtorProtocol197";

        public override string Id
        {
            get { return "SpravkaPovtorProtocol197"; }
        }

        public override string CodeForm
        {
			get { return "SpravkaPovtorProtocol197"; }
        }

        public override string Name
        {
			get { return "Справка об административных правонарушениях"; }
        }

        public override string Description
        {
			get { return "Справка об административных правонарушениях"; }
        }

        public string OutputFileName { get; set; } = "Справка об административных правонарушениях";

        public override StiExportFormat ExportFormat
        {
            get { return StiExportFormat.Word2007; }
        }

        protected override string CodeTemplate { get; set; }

        #endregion Properties

        protected long DocumentId;
        public string DocId { get; set; }
        public ComissionMeetingReportInfo ReportInfo { get; set; }

        public Stream ReportFileStream { get; set; }

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            this.DocumentId = userParamsValues.GetValue<object>("DocumentId").ToLong();

        }

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
            {
                new TemplateInfo
                {
                    Code = "SpravkaPovtorProtocol197",
                    Name = "SpravkaPovtorProtocol197",
                    Description = "Справка об административных правонарушениях",
                    Template = Resources.SPRAVKA_POVTOR_REP
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
			this.CodeTemplate = "SpravkaPovtorProtocol197";
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var protocol = this.Container.ResolveDomain<Protocol197>().Get(this.DocumentId);
            if (protocol == null)
            {
                throw new ReportProviderException("Не удалось получить протокол");
            }
            Report["Id"] = protocol.Id;
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
                this.OutputFileName = $"Справка об административных правонарушениях {ownerInfo.Fio}.docx";

            }
            finally
            {
                this.Container.Release(DocumentGjiDomain);
            }
        }
    }
}