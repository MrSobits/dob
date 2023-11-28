namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Да/Нет/Не задано
    /// </summary>
    public enum YesNoNotSet
    {
        [Display("Да")]
        Yes = 10,

        [Display("Нет")]
        No = 20,

        [Display("Не задано")]
        NotSet = 30,

        [Display("-")]
        NotNeed = 40
    }
}
