namespace Bars.GkhGji.Report
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    using Bars.B4;
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Report;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Stimulsoft.Report;
    using Bars.GkhGji.Properties;
    using System.IO;
    using Bars.B4.Modules.FileStorage;
    using Bars.GkhGji.DomainService;
    using System;

    public class Protocol2025DebtReport : GkhBaseStimulReport, IComissionMeetingCodedReport
    {
        public Protocol2025DebtReport() : base(new ReportTemplateBinary(Properties.Resources.SPRAVKA_NEOPLAT))
        {
            ExportFormat = StiExportFormat.Word2007;
        }

        public string DocId { get; set; }
        public ComissionMeetingReportInfo ReportInfo { get; set; }
        public Stream ReportFileStream { get; set; }
        private long DocumentId { get; set; }
        public string ReportId => "Protocol2025Debt";
        public string OutputFileName { get; set; } = "Справка о наличии задолженности";

        private Protocol protocol;
        public Protocol Protocol
        {
            get { return protocol; }
            set { protocol = value; }
        }
        public override string Id
        {
            get { return "Protocol2025Debt"; }
        }
        public override string CodeForm
        {
            get { return "Protocol"; }
        }
        public override string Name
        {
            get { return "Справка о наличии задолженности"; }
        }
        public override string Description
        {
            get { return "Справка о наличии задолженности"; }
        }
        protected override string CodeTemplate { get; set; }
        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
                {
                    new TemplateInfo
                        {
                            Code = "SPRAVKA_NEOPLAT",
                            Name = "Protocol2025Debt",
                            Description = "Любой случай",
                            Template = Resources.SPRAVKA_NEOPLAT
                        }
                };
        }
        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            DocumentId = userParamsValues.GetValue<object>("DocumentId").ToLong();
        }

 
        public override void PrepareReport(ReportParams reportParams)
        {

            Protocol = Container.Resolve<IDomainService<Protocol>>().Load(DocumentId);


            try
            {
                if (Protocol == null)
                {
                    throw new ReportProviderException("Не удалось получить протокол");
                }
                Report["Id"] = Protocol.Id;
                CodeTemplate = "SPRAVKA_NEOPLAT";
            }
            finally
            {

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
                    $"Справка о наличии задолженности ({ownerInfo.DocumentNumber} - {ownerInfo.DocumentDate.Value.ToString("dd.MM.yyyy")}).docx";

            }
            finally
            {
                this.Container.Release(DocumentGjiDomain);
            }
        }
        public override Stream GetTemplate()
        {

            Protocol = Container.Resolve<IDomainService<Protocol>>().Load(DocumentId);

            if (Protocol == null)
            {
                throw new ReportProviderException("Не удалось получить протокол");
            }

            Report["Id"] = Protocol.Id;


            CodeTemplate = "SPRAVKA_NEOPLAT";



            var templateDomain = Container.Resolve<IDomainService<TemplateReplacement>>();
            var fileManager = Container.Resolve<IFileManager>();

            try
            {
                var code = CodeTemplate;

                var listTemplatesInfo = GetTemplateInfo();

                if (string.IsNullOrEmpty(code))
                {
                    if (listTemplatesInfo != null && listTemplatesInfo.Count > 0)
                    {
                        code = listTemplatesInfo.FirstOrDefault().Return(x => x.Code);
                    }
                }

                var templateReplace = templateDomain.GetAll().FirstOrDefault(x => x.Code == code);

                if (templateReplace != null)
                {

                    if (fileManager != null)
                    {
                        return fileManager.GetFile(templateReplace.File);
                    }
                }

                TemplateInfo template = null;
                if (listTemplatesInfo != null && listTemplatesInfo.Count > 0)
                {
                    template = listTemplatesInfo.FirstOrDefault(x => x.Code == code);
                }

                if (template != null && template.Template != null)
                {
                    return new MemoryStream(template.Template);
                }

                return null;
            }
            catch (FileNotFoundException)
            {
                throw new ReportProviderException("Не удалось получить шаблон замены");
            }
            finally
            {
                Container.Release(templateDomain);
                Container.Release(fileManager);
            }
        }
        protected virtual void FillRegionParams(ReportParams reportParams, Protocol protocol)
        {

        }
    }
}