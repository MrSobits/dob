using Bars.B4.Utils;

namespace Bars.GkhGji.Regions.Voronezh.Enums
{
    /// <summary>
    /// Уровень бюджета
    /// </summary>
    public enum ASFKBudgetLevel
    {
        [Display("Федеральный")]
        Federal = 1,

        [Display("Субъекта РФ")]
        SubjectRF = 2,

        [Display("Местный")]
        Local = 3,

        [Display("ГВФ РФ")]
        GVFRF = 4,

        [Display("ТГВФ")]
        TGVF = 5
    }
}
