namespace Bars.GkhGji.Regions.BaseChelyabinsk.Report.Protocol
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using B4.Modules.Reports;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Report;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Contracts;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Properties;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities;
    using Bars.GkhGji.Report;
    using Stimulsoft.Report;
    using Bars.GkhGji.DomainService;
    using System.IO;
    using System;
    using Bars.B4;

    public class ChelyabinskProtocolStimulReport : GjiBaseStimulReport, IComissionMeetingCodedReport
    {
        public ChelyabinskProtocolStimulReport()
            : base(new ReportTemplateBinary(Resources.BlockGJI_ExecutiveDocProtocol_1))
        {
            ExportFormat = StiExportFormat.Word2007;
        }
        public string DocId { get; set; }
        public ComissionMeetingReportInfo ReportInfo { get; set; }
        public Stream ReportFileStream { get; set; }
        private long DocumentId { get; set; }

        public string ReportId => "Protocol2025";

        public override string Id
        {
            get { return "Protocol"; }
        }
        public string OutputFileName { get; set; } = "Протокол 20.25";
        public override string CodeForm
        {
            get { return "Protocol"; }
        }

        public override string Name
        {
            get { return "Протокол"; }
        }

        public override string Description
        {
            get { return "Протокол"; }
        }

        protected override string CodeTemplate { get; set; }

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
                {
                    new TemplateInfo
                        {
                            Code = "BlockGJI_ExecutiveDocProtocol_1",
                            Name = "ProtocolGJI",
                            Description = "Любой случай",
                            Template = Resources.BlockGJI_ExecutiveDocProtocol_1
                        }
                };
        }

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            DocumentId = userParamsValues.GetValue<object>("DocumentId").ToLong();
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var protocolDomain = Container.ResolveDomain<Protocol>();
          

            try
            {
                var protocol = protocolDomain.Load(DocumentId);

                if (protocol == null)
                {
                    throw new ReportProviderException("Не удалось получить протокол");
                }
                Report["Id"] = protocol.Id;             


            }
            finally
            {
                Container.Release(protocolDomain);              
            }
        }

        public void GenerateMassReport()
        {

            var DocumentGjiDomain = this.Container.Resolve<IDomainService<Protocol>>();
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
                    $"Протокол 20.25 ({ownerInfo.DocumentNumber} - {ownerInfo.DocumentDate.Value.ToString("dd.MM.yyyy")}).docx";

            }
            finally
            {
                this.Container.Release(DocumentGjiDomain);
            }
        }
    }
}
