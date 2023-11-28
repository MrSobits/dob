namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Вид КНД
    /// </summary>
    public enum ComissionDocumentDecision
    {
        /// <summary>
        /// Не задано
        /// </summary>
        [Display("Не задано")]
        NotSet = 0,

        /// <summary>
        /// Вынесено постановление
        /// </summary>
        [Display("Вынесено постановление")]
        Resolution = 10,

        /// <summary>
        /// Лицензионный контроль
        /// </summary>
        [Display("Рассмотрение на комиссии")]
        NewComisison = 20,

        /// <summary>
        /// Отложить рассмотрение
        /// </summary>
        [Display("Отложить рассмотрение")]
        Decline = 30,

        /// <summary>
        /// Отменить рассмотрение
        /// </summary>
        [Display("Вернуть составителю")]
        Stop = 40,

        /// <summary>
        /// Отменить рассмотрение
        /// </summary>
        [Display("Направить по подведомственности")]
        Transfer = 50,

    }
}