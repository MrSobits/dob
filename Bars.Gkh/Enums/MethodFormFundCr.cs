namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Способ формирования фонда КР
    /// </summary>
    public enum MethodFormFundCr
    {
        [Display("На счете регионального оператора")]
        RegOperAccount = 10,

        [Display("На специальном счете")]
        SpecialAccount = 20,

        [Display("Не задано")]
        NotSet = 0
    }
}
