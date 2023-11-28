namespace Bars.Gkh.Gis.Reports.Reports
{
    using System.IO;
    using B4.Modules.Reports;
    using B4.Modules.StimulReportGenerator;
    using Stimulsoft.Report;

    /// <summary>
    /// Так как используется ReportPanel которая не поддерживает выбор формата, то необходимо задавать его через реализации
    ///     классов-наследников StimulReport
    /// </summary>
    public abstract class StimulReportDynamicExcel : StimulReportGenerator, IGeneratedPrintForm
    {
        public override void Generate(Stream result, ReportParams reportParams)
        {
            ExportFormat = StiExportFormat.Excel2007;
            base.Generate(result, reportParams);
        }
    }
}
