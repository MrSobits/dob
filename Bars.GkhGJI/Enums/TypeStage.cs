namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Этап проверки ГЖИ
    /// </summary>
    public enum TypeStage
    {
        [Display("Распоряжение")]
        Disposal = 10,

        [Display("Распоряжение проверки предписания")]
        DisposalPrescription = 20,

        [Display("Акт проверки")]
        ActCheck = 30,

        [Display("Акт проверки (общий)")]
        ActCheckGeneral = 40,

        [Display("Акт обследования")]
        ActSurvey = 50,

        [Display("Предписание")]
        Prescription = 60,

        [Display("Протокол")]
        Protocol = 70,

        [Display("Постановление")]
        Resolution = 80,

        [Display("Акт проверки предписания")]
        ActRemoval = 90,

        [Display("Постановление прокуратуры")]
        ResolutionProsecutor = 100,

        [Display("Представление")]
        Presentation = 110,

        [Display("Акт визуального осмотра")] // Тип используется в Томске
        ActVisual = 120,

        [Display("Административное дело")] // Тип используется в Томске
        AdministrativeCase = 130,

        [Display("Протокол МВД")]
        ProtocolMvd = 140,

        [Display("Протокол МЖК")]
        ProtocolMhc = 150,

        [Display("Акт осмотра")]
        ActView = 160,

        [Display("Протокол по ст.19.7 КоАП РФ")] // Тип используется в НСО
        Protocol197 = 170,

        [Display("Постановление Роспотребнадзора")]
        ResolutionRospotrebnadzor = 180,


        [Display("Протокол РСО")]
        ProtocolRSO = 190,
    }
}