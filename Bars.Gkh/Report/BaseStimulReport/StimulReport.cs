namespace Bars.Gkh.StimulReport
{
    using System.Collections.Generic;
    using System.IO;
    using B4.Application;
    using B4.Modules.Reports;
    using Bars.B4.Utils;

    using Stimulsoft.Report;
    using Stimulsoft.Report.Dictionary;
    using Stimulsoft.Report.Export;

	/// <summary>
    /// Базовый отчет Stimulsoft
    /// Пример использования:
    /// 
    /// // Создаем отчет и передаем ему параметры отчета
    /// var report = new Report { Container = Container };
    ///
    /// // Устанавливаем формат печати
    /// report.SetExportFormat(new PrintFormExportFormat { Id = (int)StiExportFormat.Pdf });
    /// 
    /// // открываем шаблон
    /// var template = report.GetTemplate();
    /// report.Open(template);
    /// 
    /// // Запускаем формирование и получениие результата в виде MemoryStream
    /// var result = new MemoryStream();
    /// report.PrepareReport(new ReportParams());
    /// report.Generate(result, new ReportParams());
    /// result.Seek(0, SeekOrigin.Begin);
    /// 
    /// // Готовый отчет
    /// return result;
    /// </summary>
    public class StimulReport : IReportGenerator, IReportGeneratorFileName, IReportGeneratorMimeType, IExportablePrintForm
    {
		/// <summary>
		/// Конструктор
		/// </summary>
        public StimulReport()
        {
            Report = new StiReport();
        }

        /// <summary>Формат печатной формы</summary>
        public virtual StiExportFormat ExportFormat { get; set; }

		/// <summary>Настройки экспорта (для каждого формата свой производный тип)</summary>
		public virtual StiExportSettings ExportSettings { get; set; }

		/// <summary>Компилируемый отчет</summary>
		protected StiReport Report { get; set; }

        /// <summary> IReportGenerator.Open </summary>
        public virtual void Open(Stream reportTemplate)
        {
            Report.Load(reportTemplate);
            ActualizeConnectionString(Report);
            Report.Compile();           
        }

		/// <summary>
		/// Сгенерировать отчет
		/// </summary>
		/// <param name="result">Результат</param>
		/// <param name="reportParams">Параметры отчета</param>
        public virtual void Generate(Stream result, ReportParams reportParams)
        {
            if (!Report.IsRendered)
            {
                Report.Render();
            }

            result.Seek(0, SeekOrigin.Begin);

            Report.ExportDocument(ExportFormat, result, ExportSettings);
        }

		/// <summary>
		/// Получить сгенерированный отчет
		/// </summary>
		/// <returns></returns>
        public virtual MemoryStream GetGeneratedReport()
        {
            var reportParams = new ReportParams();
            var printForm = this as IBaseReport;
            if (printForm != null)
            {
                // Если кто-то захочет поменять порядок вызовов метода Open и PrepareREport
                // то такое делать ненадо
                // поскольку для Sti,ula сначала нужно всебя затянуть шаблон, а только потом заполнять словарь Report["параметр"]
                Open(printForm.GetTemplate());
                printForm.PrepareReport(reportParams);
            }

            var ms = new MemoryStream();
            Generate(ms, reportParams);
            ms.Seek(0, SeekOrigin.Begin);
            return ms;
        }

		/// <summary>
		/// Получить имя файла
		/// </summary>
		/// <returns></returns>
        public virtual string GetFileName()
        {
            var extention = string.Empty;
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
        /// <summary> IExportablePrintForm.GetExportFormats() </summary>
        public virtual IList<PrintFormExportFormat> GetExportFormats()
        {
            return new[]
            {
                new PrintFormExportFormat { Id = (int)StiExportFormat.Excel2007, Name = "MS Excel 2007"       },
                new PrintFormExportFormat { Id = (int)StiExportFormat.Word2007,  Name = "MS Word 2007"        },
                new PrintFormExportFormat { Id = (int)StiExportFormat.Pdf,       Name = "Adobe Acrobat"       },
                new PrintFormExportFormat { Id = (int)StiExportFormat.Ppt2007,   Name = "MS Power Point"      },
                new PrintFormExportFormat { Id = (int)StiExportFormat.Odt,       Name = "OpenDocument Writer" },
                new PrintFormExportFormat { Id = (int)StiExportFormat.Text,      Name = "Текст (TXT)"         },
                new PrintFormExportFormat { Id = (int)StiExportFormat.ImagePng,  Name = "Изображение (PNG)"   },
                new PrintFormExportFormat { Id = (int)StiExportFormat.ImageSvg,  Name = "Изображение (SVG)"   },
                new PrintFormExportFormat { Id = (int)StiExportFormat.Html,      Name = "Веб страница (Html)" }
            };
        }
        /// <summary> IExportablePrintForm.SetExportFormat() </summary>
        public virtual void SetExportFormat(PrintFormExportFormat format)
        {
            ExportFormat = (StiExportFormat)format.Id;
        }

        private void ActualizeConnectionString(StiReport stiReport)
        {
            foreach (var database in stiReport.Dictionary.Databases)
            {
                var db = database as StiSqlDatabase;
                if (db != null)
                {
                    db.ConnectionString = ApplicationContext.Current.Configuration.ConnString;
                }
            }
        }
    }
}