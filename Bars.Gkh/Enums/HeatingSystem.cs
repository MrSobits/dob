namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Система отопления
    /// </summary>
    public enum HeatingSystem
    {
        [Display("Не задано")]
        None = 0,

        [Display("Индивидуальное")]
        Individual = 10,

        [Display("Централизованное")]
        Centralized = 20
    }
}