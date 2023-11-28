using Bars.B4.Utils;

namespace Bars.GkhGji.Regions.Voronezh.Enums
{
    /// <summary>
    /// Тип КБК
    /// </summary>
    public enum ASFKKBKType
    {
        [Display("Доходы")]
        Income = 20,

        [Display("ИВФДБ (в документации не было пояснения)")]
        IVFDB = 31,

        [Display("ИВнФДБ (в документации не было пояснения)")]
        IVnFDB = 32
    }
}
