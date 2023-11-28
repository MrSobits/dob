namespace Bars.GkhGji.Regions.Chelyabinsk
{
    using Bars.B4;

    /// <summary>
    /// Меню, навигация
    /// </summary>
    public class NavigationProvider : INavigationProvider
    {
        public string Key
        {
            get
            {
                return MainNavigationInfo.MenuName;
            }
        }

        public string Description
        {
            get
            {
                return MainNavigationInfo.MenuDescription;
            }
        }

        public void Init(MenuItem root)
        {

            root.Add("Административная комиссия")
                .Add("Реестр обращений")
                .Add("Результаты обмена данными с ЕАИС", "eaisintegration").AddRequiredPermission("GkhGji.AppealCitizens.View")
                .WithIcon("deliveryAgent");
            root.Add("Административная комиссия").Add("Риск-ориентированный подход").Add("Виды КНД", "kindknddict").WithIcon("reminderHead").AddRequiredPermission("GkhGji.RiskOrientedMethod.KindKNDDict.View");
            root.Add("Административная комиссия").Add("Риск-ориентированный подход").Add("Расчет категории риска", "romcategory").WithIcon("heatSeasonMassStateChange").AddRequiredPermission("GkhGji.RiskOrientedMethod.ROMCategory.View");
            root.Add("Административная комиссия").Add("Риск-ориентированный подход").Add("Массовый расчет категорий риска", "romcalctask").WithIcon("estimate").AddRequiredPermission("GkhGji.RiskOrientedMethod.ROMCategory.View");
            root.Add("Административная комиссия").Add("Риск-ориентированный подход").Add("Субъекты проверок ЛК", "licensecontrolobj").AddRequiredPermission("GkhGji.RiskOrientedMethod.ROMCategory.View");
            root.Add("Административная комиссия").Add("Риск-ориентированный подход").Add("Субъекты проверок ЖН", "housingspvobj").AddRequiredPermission("GkhGji.RiskOrientedMethod.ROMCategory.View");
            root.Add("Административная комиссия").Add("Риск-ориентированный подход").Add("Показатели эффективности КНД", "effectivekndindex").WithIcon("baseProsClaim").AddRequiredPermission("GkhGji.RiskOrientedMethod.EffectiveKNDIndex.View");

            root.Add("Административная комиссия").Add("Судебная практика").Add("Реестр судебной практики", "courtpractice").WithIcon("reminderHead").AddRequiredPermission("GkhGji.CourtPractice.CourtPracticeRegystry.View");
            root.Add("Справочники").Add("Общие").Add("Нарушители", "individualperson").AddRequiredPermission("Gkh.Dictionaries.IndividualPerson.View").WithIcon("inspector");
            root.Add("Справочники").Add("123").Add("Повестка на комиссию", "subpoena").AddRequiredPermission("GkhGji.Dict.Subpoena.View");
            //справочники
            //root.Add("Справочники").Add("Комиссии").Add("Коды регионов", "regioncode").AddRequiredPermission("GkhGji.Dict.RegionCode.View");
            //root.Add("Справочники").Add("Комиссии").Add("Коды документов физических лиц", "fldoctype").AddRequiredPermission("GkhGji.Dict.FLDocType.View");
            //root.Add("Справочники").Add("Комиссии").Add("Статусы плательщиков", "gisgmppayerstatus").AddRequiredPermission("GkhGji.SMEV.GISGMP.View");
            //root.Add("Справочники").Add("Комиссии").Add("Категория заявителя ЕГРН", "egrnapplicanttype").AddRequiredPermission("GkhGji.Dict.EGRNApplicantType.View");
            //root.Add("Справочники").Add("Комиссии").Add("Объект запроса ЕГРН", "egrnobjecttype").AddRequiredPermission("GkhGji.Dict.EGRNObjectType.View");
            //root.Add("Справочники").Add("Комиссии").Add("Документы ЕГРН", "egrndoctype").AddRequiredPermission("GkhGji.Dict.EGRNDocType.View");
            //root.Add("Справочники").Add("Комиссии").Add("Отделы прокуратуры", "prosecutoroffice").AddRequiredPermission("GkhGji.Dict.ProsecutorOffice.View");

            //root.Add("Справочники").Add("Комиссии").Add("Идентификаторы ССТУ", "sstutransferorg").AddRequiredPermission("GkhGji.Dict.SSTUTransferOrg.View");

            //СМЭВ   
            root.Add("Административная комиссия").Add("Межведомственное взаимодействие").Add("Запросы в МВД", "smevmvd").AddRequiredPermission("GkhGji.SMEV.SMEVMVD.View");
            root.Add("Административная комиссия").Add("Межведомственное взаимодействие").Add("Запросы в ЕГРЮЛ", "smevegrul").AddRequiredPermission("GkhGji.SMEV.SMEVEGRUL.View");
            root.Add("Административная комиссия").Add("Межведомственное взаимодействие").Add("Запросы в ЕГРИП", "smevegrip").AddRequiredPermission("GkhGji.SMEV.SMEVEGRIP.View");
            root.Add("Административная комиссия").Add("Межведомственное взаимодействие").Add("Запросы в ЕГРН", "smevegrn").AddRequiredPermission("GkhGji.SMEV.SMEVEGRN.View");
            root.Add("Административная комиссия").Add("Межведомственное взаимодействие").Add("Обмен данными с ГИС ГМП", "gisgmp").AddRequiredPermission("GkhGji.SMEV.GISGMP.View");
            root.Add("Административная комиссия").Add("Межведомственное взаимодействие").Add("Выгрузка проверок в ГИС ЕРП", "giserp").AddRequiredPermission("GkhGji.SMEV.GISERP.View");
            root.Add("Административная комиссия").Add("Межведомственное взаимодействие").Add("Реестр платежей", "payreg").AddRequiredPermission("GkhGji.SMEV.PAYREG.View");

            root.Add("Административная комиссия").Add("Основания проверок").Add("Плановые проверки ОМСУ", "baseomsu").AddRequiredPermission("GkhGji.Inspection.BaseOMSU.View").WithIcon("baseJurPerson");

            root.Add("Административная комиссия").Add("Реестр обращений").Add("Реестр предостережений", "B4.controller.Admonition").AddRequiredPermission("GkhGji.AppealCitizens.View").WithIcon("resolPros");
            root.Add("Административная комиссия").Add("Реестр обращений").Add("Реестр проверок в/а домов", "B4.controller.EmergencyHouse").AddRequiredPermission("GkhGji.AppealCitizens.View").WithIcon("resolPros");
            // Переоформление лицензии
            //var menuLicense = root.Add("Административная комиссия").Add("Лицензирование");
            //menuLicense.Add("Обращения за переоформлением лицензии", "licensereissuance").AddRequiredPermission("Gkh.ManOrgLicense.Request.View").WithIcon("menuManorgRequestLicense");
            root.Add("Административная комиссия").Add("Основания проверок").Add("Проверки по переоформлению лицензий", "baselicensereissuance").AddRequiredPermission("GkhGji.Inspection.BaseLicApplicants.View");

            // реестр рассылок
            root.Add("Административная комиссия").Add("Реестр обращений").Add("Реестр исходящих электронных писем", "emaillist").AddRequiredPermission("GkhGji.AppealCitizens.View").WithIcon("businessActivity");
            root.Add("Административная комиссия").Add("Реестр обращений").Add("Экспорт обращений в ССТУ", "sstuexporttask").AddRequiredPermission("GkhGji.DocumentsGji.View").WithIcon("appealCits");

            //root.Add("Администрирование").Add("Импорт/экспорт данных системы").Add("Экспорт обращений в ССТУ", "sstuexporttask").AddRequiredPermission("GkhGji.DocumentsGji.View").WithIcon("appealCits");
            
            root.Add("Административная комиссия").Add("Реестр обращений").Add("Реестр СОПР", "appealorder").AddRequiredPermission("GkhGji.SOPR.Appeal").WithIcon("appealCits");

            root.Add("Административная комиссия")
          .Add("Документы")
          .Add("Протоколы по ст.19.7 КоАП РФ", "protocol197")
          .AddRequiredPermission("GkhGji.DocumentsGji.Protocol197.View");
        }
    }
}