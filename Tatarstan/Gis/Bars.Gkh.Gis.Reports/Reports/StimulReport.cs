namespace Bars.Gkh.Gis.Reports.Reports
{
    using System.IO;
    using B4;
    using B4.Modules.Reports;
    using B4.Utils;
    using Castle.Windsor;
    using Stimulsoft.Report;

    public abstract class StimulReport : IReportGeneratorFileName, IReportGeneratorMimeType, IGeneratedPrintForm
    {
        public IWindsorContainer Container { get; set; }

        /// <summary>Формат печатной формы</summary>
        public virtual StiExportFormat ExportFormat { get; set; }

        /// <summary>Компилируемый отчет</summary>
        protected StiReport Report { get; set; }

        /// <summary> IReportGenerator.Open </summary>
        public virtual void Open(Stream reportTemplate)
        {
            Report = new StiReport();

            Report.Load(reportTemplate);
        }

        public virtual void Generate(Stream result, ReportParams reportParams)
        {
            Report.Compile();
            
            if (!Report.IsRendered)
            {
                Report.Render();
            }

            result.Seek(0, SeekOrigin.Begin);

            Report.ExportDocument(ExportFormat, result);
        }

        public virtual MemoryStream GetGeneratedReport()
        {
            var reportParams = new ReportParams();

            Open(GetTemplate());
            PrepareReport(reportParams);

            var ms = new MemoryStream();
            Generate(ms, reportParams);
            ms.Seek(0, SeekOrigin.Begin);
            return ms;
        }

        public virtual string GetFileName()
        {
            string extention;
            switch (ExportFormat)
            {
                case StiExportFormat.Excel2007: extention = ".xlsx"; break;
                case StiExportFormat.Word2007: extention = ".docx"; break;
                case StiExportFormat.Pdf: extention = ".pdf"; break;
                case StiExportFormat.Ppt2007: extention = ".ppt"; break;
                case StiExportFormat.Odt: extention = ".odt"; break;
                case StiExportFormat.Text: extention = ".txt"; break;
                case StiExportFormat.ImagePng: extention = ".png"; break;
                case StiExportFormat.ImageSvg: extention = ".svg"; break;
                case StiExportFormat.ImageEmf: extention = ".emf"; break;
                case StiExportFormat.ImageJpeg: extention = ".jpg"; break;
                case StiExportFormat.Html: extention = ".html"; break;
                case StiExportFormat.Html5: extention = ".html"; break;
                case StiExportFormat.HtmlDiv: extention = ".html"; break;
                case StiExportFormat.HtmlSpan: extention = ".html"; break;
                case StiExportFormat.HtmlTable: extention = ".html"; break;
                case StiExportFormat.RtfWinWord: extention = ".rtf"; break;
                default: extention = ".bin"; break;
            }

            return "report" + extention;
        }

        /// <summary> MIME type </summary>
        public virtual string GetMimeType()
        {
            return MimeTypeHelper.GetMimeType(Path.GetExtension(GetFileName()));
        }

        public abstract Stream GetTemplate();

        public abstract void PrepareReport(ReportParams reportParams);

        public abstract string Name { get; }

        public virtual void SetUserParams(BaseParams baseParams)
        {
            
        }

        public abstract string Desciption { get; }

        public abstract string GroupName { get; }

        public abstract string ParamsController { get; }

        public abstract string RequiredPermission { get; }
    }
}
