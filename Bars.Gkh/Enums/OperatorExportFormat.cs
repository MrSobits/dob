namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Формат выгрузки отчетов для оператора
    /// </summary>
    public enum OperatorExportFormat
    {
        [Display("docx")]
        docx = 0,

        [Display("odt")]
        odt = 4
    }
}
