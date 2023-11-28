namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Да/Нет/Не задано
    /// </summary>
    public enum YesNoNotSetPartially
    {
        [Display("Не задано")]
        NotSet = 0,

        [Display("Ходатайство удоволетворено")]
        Yes = 10,

        [Display("Ходатайство не удоволетворено")]
        No = 20,

        [Display("Удоволетворено частично")]
        Partially = 30,


    }
}
