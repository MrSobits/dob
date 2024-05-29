namespace Bars.GkhGji.Regions.Voronezh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Причина отмены
    /// </summary>
    public enum CourtDischargeReason
    {
        /// <summary>
        /// Не задано
        /// </summary>
        [Display("Не задано")]
        NotSet = 0,

        /// <summary>
        /// Малозначительность, отсутствие вины административного правонарушения
        /// </summary>
        [Display("Малозначительность, отсутствие вины в административном правонарушении")]
        InsignificantOrNoGuilt = 10,

        /// <summary>
        /// Отсутствие состава, события административного правонарушения
        /// </summary>
        [Display("Отсутствие состава, события административного правонарушения")]
        NoOffenseOrEvent = 20,

        /// <summary>
        /// Нарушение процессуальных требований
        /// </summary>
        [Display("Нарушение процессуальных требований")]
        ProcessualViolation = 30,

        /// <summary>
        /// Истечение сроков давности привлечения к административной ответственности
        /// </summary>
        [Display("Истечение сроков давности привлечения к административной ответственности")]
        Expired = 40,

        /// <summary>
        /// Иные
        /// </summary>
        [Display("Иные")]
        Other = 50
    }
}