namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип приложения
    /// </summary>
    
    public enum TypeAnnex
    {
        /// <summary>
        /// Не задано
        /// </summary>
        [Display("Не задано")]
        NotSet = 0,

        /// <summary>
        /// Распоряжение
        /// </summary>
        [Display("Материалы правонарушения")]
        Disposal = 10,

        /// <summary>
        /// Уведомление о проверке
        /// </summary>
        [Display("Уведомление о проверке")]
        DisposalNotice = 20,

        /// <summary>
        /// Акт проверки
        /// </summary>
        [Display("Акт")]
        ActCheck = 30,

        /// <summary>
        /// Уведомление-вызов
        /// </summary>
        [Display("Уведомление-вызов")]
        PrescriptionNotice = 50,

        /// <summary>
        /// Протокол
        /// </summary>
        [Display("Протокол")]
        Protocol = 60,

        /// <summary>
        /// Постановление
        /// </summary>
        [Display("Постановление")]
        Resolution = 70,

        /// <summary>
        /// Мотивированный запрос
        /// </summary>
        [Display("Мотивированный запрос")]
        MotivRequest = 80,

        /// <summary>
        /// уведомления о составлении протокола
        /// </summary>
        [Display("Уведомления о составлении протокола")]
        ProtocolNotification = 90,

        /// <summary>
        /// Определение
        /// </summary>
        [Display("Определение")]
        ActDefinition = 100,

        /// <summary>
        /// Определение
        /// </summary>
        [Display("Ходатайство")]
        Hodat = 110

    }
}
