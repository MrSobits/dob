namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип кровли
    /// </summary>
    public enum TypeRoof
    {
        [Display("Не задано")]
        None = 0,

        [Display("Плоская")]
        Plane = 10,

        [Display("Скатная")]
        Pitched = 20
    }
}
