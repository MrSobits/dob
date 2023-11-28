using Bars.B4.Utils;

namespace Bars.GkhGji.Regions.Voronezh.Enums
{
    /// <summary>
    /// Тип отчета
    /// </summary>
    public enum ASFKReportType
    {
        [Display("Итоговый и/или нулевой")]
        FinalOrNull = 0,

        [Display("Промежуточный")]
        Intermediate = 1
    }
}
