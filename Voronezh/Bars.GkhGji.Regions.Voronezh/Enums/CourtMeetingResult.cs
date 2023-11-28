namespace Bars.GkhGji.Regions.Voronezh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Результат рассмотрения
    /// </summary>
    public enum CourtMeetingResult
    {
        /// <summary>
        /// -
        /// </summary>
        [Display("-")]
        NotSet = 0,

        /// <summary>
        /// Оставлено в силе
        /// </summary>
        [Display("Оставлено в силе")]
        Denied = 10,

        /// <summary>
        /// Изменено судом
        /// </summary>
        [Display("Изменено судом")]
        PartiallySatisfied = 20,

        /// <summary>
        /// Отменено судом
        /// </summary>
        [Display("Отменено судом")]
        CompletelySatisfied = 30,

        /// <summary>
        /// Производство по делу прекращено
        /// </summary>
        [Display("Производство по делу прекращено")]
        Stoped = 40,

        /// <summary>
        /// Оставлено без рассмотрения
        /// </summary>
        [Display("Оставлено без рассмотрения")]
        LeftWithoutConsideration = 50,

        /// <summary>
        /// Отменено и направлено на новое рассмотрение
        /// </summary>
        [Display("Отменено и направлено на новое рассмотрение")]
        Returned = 60,

        /// <summary>
        /// Устное замечание
        /// </summary>
        [Display("Устное замечание")]
        VerbalRemark = 70,

        /// <summary>
        /// Устное замечание
        /// </summary>
        [Display("Малозначительность")]
        Insignificance = 80,

        /// <summary>
        /// Устное замечание
        /// </summary>
        [Display("Предупреждение")]
        Warning = 90,


    }
}