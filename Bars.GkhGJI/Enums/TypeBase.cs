namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип основания проверки
    /// </summary>
    public enum TypeBase
    {
        /// <summary>
        /// Инспекционная проверка
        /// </summary>
        [Display("Инспекционная проверка")]
        Inspection = 10,

        /// <summary>
        /// Обращение граждан
        /// </summary>
        [Display("Обращение граждан")]
        CitizenStatement = 20,

        /// <summary>
        /// Плановая проверка юр.лиц
        /// </summary>
        [Display("Плановая проверка юр. лиц")]
        PlanJuridicalPerson = 30,

        /// <summary>
        /// Плановая проверка юр.лиц
        /// </summary>
        [Display("Плановая проверка ОМСУ")]
        // ReSharper disable once InconsistentNaming
        PlanOMSU = 31,

        /// <summary>
        /// Поручение руководства
        /// </summary>
        [Display("Выявленное правонарушение")]
        DisposalHead = 40,

        /// <summary>
        /// Требование прокуратуры
        /// </summary>
        [Display("Требование прокуратуры")]
        ProsecutorsClaim = 50,

        /// <summary>
        /// Постановление прокуратуры
        /// </summary>
        [Display("Постановление прокуратуры")]
        ProsecutorsResolution = 60,

        /// <summary>
        /// Проверка деятельности ТСЖ
        /// </summary>
        [Display("Проверка деятельности ТСЖ")]
        ActivityTsj = 70,

        /// <summary>
        /// Подготовка к отопительному сезону
        /// </summary>
        [Display("Подготовка к отопительному сезону")]
        HeatingSeason = 80,

        /// <summary>
        /// Административное дело
        /// </summary>
        [Display("Административное дело")]
        AdministrativeCase = 90,

        /// <summary>
        /// Протокол МВД
        /// </summary>
        [Display("Протокол МВД")]
        ProtocolMvd = 100,

        /// <summary>
        /// Проверка по плану мероприятий
        /// </summary>
        [Display("Проверка по плану мероприятий")]
        PlanAction = 110,

        /// <summary>
        /// Протокол МЖК
        /// </summary>
        [Display("Протокол МЖК")]
        ProtocolMhc = 120,

        /// <summary>
        /// Проверка соискателей лицензии
        /// </summary>
        [Display("Проверка соискателей лицензии")]
        LicenseApplicants = 130,

        /// <summary>
        /// Проверка лицензиата для переоформления лицензии
        /// </summary>
        [Display("Проверка лицензиата")]
        LicenseReissuance = 135,

        /// <summary>
        /// Протокол по ст.19.7 КоАП РФ
        /// <para>Используется в НСО</para>
        /// </summary>
        [Display("Протокол на рассмотрение")]
        Protocol197 = 140,

        /// <summary>
        /// Без основания
        /// </summary>
        [Display("Без основания")]
        Default = 150,

        /// <summary>
        /// Протокол РСО
        /// </summary>
        [Display("Протокол РСО")]
        ProtocolRSO = 190
    }
}