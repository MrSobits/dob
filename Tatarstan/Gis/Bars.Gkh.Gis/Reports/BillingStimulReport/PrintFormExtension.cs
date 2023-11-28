namespace Bars.Gkh.Gis.Reports.BillingStimulReport
{
    using System.Collections.Generic;
    using B4.Modules.Reports;
    using B4.Modules.StimulReportGenerator;

    public static class PrintFormExtension
    {
        /// <summary>
        /// Метод-расширение для возможности получать список форматов из отчета
        /// </summary>
        /// <param name="printForm"></param>
        /// <returns></returns>
        public static IList<PrintFormExportFormat> GetExportFormats(this IPrintForm printForm)
        {
            return ((StimulReportGenerator)printForm).GetExportFormats();
        }
    }
}
