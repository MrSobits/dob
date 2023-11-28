namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Семейное положение
    /// </summary>
    public enum FamilyStatus
    {
        /// <summary>
        /// Не состоит в браке
        /// </summary>
        [Display("Не задано")]
        Default = 0,
        /// <summary>
        /// В браке
        /// </summary>
        [Display("В браке")]
        Married = 1,

        /// <summary>
        /// Не состоит в браке
        /// </summary>
        [Display("Не состоит в браке")]
        NoMarried = 2,



    }
}