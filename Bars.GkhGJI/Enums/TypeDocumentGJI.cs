namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип документа ГЖИ
    /// </summary>
    public enum TypeDocumentGji
    {
        /// <summary>
        /// Распоряжение
        /// </summary>
        [Display("Дело об административных правонарушениях")]
        Disposal = 10,

        /// <summary>
        /// Акт проверки
        /// </summary>
        [Display("Акт проверки")]
        ActCheck = 20,

        /// <summary>
        /// Акт устранения нарушений
        /// </summary>
        [Display("Акт устранения нарушений")]
        ActRemoval = 30,

        /// <summary>
        /// Акт обследования
        /// </summary>
        [Display("Акт обследования")]
        ActSurvey = 40,

        /// <summary>
        /// Предписание
        /// </summary>
        [Display("Предписание")]
        Prescription = 50,

        /// <summary>
        /// Протокол 20.25
        /// </summary>
        [Display("Протокол 20.25")]
        Protocol = 60,

        /// <summary>
        /// Постановление
        /// </summary>
        [Display("Постановление")]
        Resolution = 70,

        /// <summary>
        /// Постановление прокуратуры
        /// </summary>
        [Display("Постановление прокуратуры")]
        ResolutionProsecutor = 80,

        /// <summary>
        /// Представление
        /// </summary>
        [Display("Представление")]
        Presentation = 90,

        /// <summary>
        /// Акт визуального обследования
        /// <para>Используется в Томске</para>
        /// </summary>
        [Display("Акт визуального обследования")]
        ActVisual = 100,

        /// <summary>
        /// Административное дело
        /// <para>Используется в Томске</para>
        /// </summary>
        [Display("Административное дело")]
        AdministrativeCase = 110,

        /// <summary>
        /// Протокол МВД
        /// </summary>
        [Display("Протокол МВД")]
        ProtocolMvd = 120,

        /// <summary>
        /// Протокол МЖК
        /// </summary>
        [Display("Протокол МЖК")]
        ProtocolMhc = 130,

        /// <summary>
        /// Протокол по ст.19.7 КоАП РФ
        /// <para>Используется в НСО</para>
        /// </summary>
        [Display("Протокол на рассмотрение")]
        Protocol197 = 140,

        /// <summary>
        /// Постановление Роспотребнадзора
        /// </summary>
        [Display("Постановление Роспотребнадзора")]
        ResolutionRospotrebnadzor = 150,
        /// <summary>
        /// Протокол РСО
        /// </summary>
        [Display("Протокол РСО")]
        ProtocolRSO = 190,
    }
}