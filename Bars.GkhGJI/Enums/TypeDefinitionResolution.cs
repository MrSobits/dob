﻿namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    // Для данного енума сделан IResolutionDefinitionService который возвращает для ставраполи все значения
    // а для других регионов только 10 и 20. Если в какомто регионе требуется добавить значения
    // то необходимо в регионе заменить реализацию Поулчения Типов данного енума чтобы на клиенте показался тип

    /// <summary>
    /// Тип определения постановление
    /// </summary>
    public enum TypeDefinitionResolution
    {
        [Display("Об отсрочке (рассрочке) исполнения постановления")]
        Deferment = 10,

        [Display("Об исправлении допущенной ошибки в постановлении")]
        CorrectionError = 20,

        [Display("Определение о возврате материалов")]
        Return = 25,

        // Для Ставрополя
        [Display("О продлении срока рассмотрения дела об административном правонарушении")]
        ProlongationReview = 30,

        // Для Ставрополя
        [Display("О передаче постановления о возбуждении дела об административном правонарушении и других материалов дела на рассмотрение по подведомственности")]
        TransferRegulation = 40,

        // Для Ставрополя
        [Display("О назначении места и времени рассмотрения жалобы на постановление по делу об административном правонарушении")]
        AppointmentPlaceTime = 50,

        // Для Ставрополя
        [Display("Об отложении рассмотрения жалобы на постановление по делу об административном правонарушении")]
        SuspenseReviewAppeal = 60,

        // Для САХИ
        [Display("О рассрочке исполнения постановления")]
        Installment = 70,


        // Для Админ комиссий 
        [Display("в порядке подготовки к рассмотрению дела об административном правонарушении")]
        AdminCommission = 80,

        [Display("об отказе в удовлетворении ходатайства")]
        AdminCommissionRefusalPetitions = 90,

        [Display("об отложении рассмотрения дела")]
        AdminCommissionDepositWork = 100,

        [Display("о назначении административного наказания")]
        PostanRep = 110,

        [Display("о прекращении производства")]
        PostanUrRep = 120,

        [Display("об отложении рассмотрения дела")]
        Postan1Rep = 130,

        [Display("о прекращении исполнения постановления")]
        Postan1UrRep = 140,




    }
}