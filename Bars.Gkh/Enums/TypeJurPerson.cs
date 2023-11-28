namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    public enum TypeJurPerson
    {
        [Display("Управляющая организация")]
        ManagingOrganization = 10,

        [Display("Поставщик коммунальных услуг")]
        SupplyResourceOrg = 20,

        [Display("Орган местного самоуправления")]
        LocalGovernment = 30,

        [Display("Орган государственной власти")]
        PoliticAuthority = 40,

        [Display("Подрядчик")]
        Builder = 50,

        [Display("Поставщик жилищных услуг")]
        ServOrg = 60,

        [Display("Организация-арендатор")]
        RenterOrg = 70,

        [Display("Региональный оператор")]
        RegOp = 80,

        [Display("Обслуживающая компания")]
        ServiceCompany = 90,

        [Display("ТСЖ, ЖСК, специализированный кооператив")]
        Tsj = 100,

        [Display("Организация - собственник")]
        Owner = 110,

        [Display("Ресурсоснабжающая организация")]
        ResourceCompany = 120,

        [Display("Поставщик ресурсов")]
        PublicServiceOrg = 130
    }
}