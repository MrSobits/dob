namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Форма проверки ЮЛ
    /// </summary>
    public enum TypeFormInspection
    {
        [Display("Выездная")]
        Exit = 10,

        [Display("Документарная")]
        Documentary = 20,

        [Display("Выездная и документарная")]
        ExitAndDocumentary = 30
    }
}