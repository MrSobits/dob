namespace Bars.GkhGji.Regions.Voronezh.Permissions
{
    using B4;
    using Bars.B4.Application;
    using Bars.Gkh.TextValues;
    using Entities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol197;

    /// <summary>
    /// PermissionMap для GkhGjiPermissionMap
    /// </summary>
    public class GkhGjiVoronezhPermissionMap : PermissionMap
    {
        /// <summary>
        /// Интерфейс для описания текстовых значений пунктов меню
        /// </summary>
        public IMenuItemText MenuItemText { get; set; }

        /// <summary>
        /// Конструктор GkhGjiPermissionMap
        /// </summary>
        public GkhGjiVoronezhPermissionMap()
        {
            this.Namespace("GkhGji", "Модуль Комиссии");

            #region Риск-ориентированный подход
            //this.Namespace("GkhGji.RiskOrientedMethod", "Риск-ориентированный подход");

            //this.Namespace("GkhGji.RiskOrientedMethod.KindKNDDict", "Справочник типов КНД");
            //this.CRUDandViewPermissions("GkhGji.RiskOrientedMethod.KindKNDDict");


            //this.Namespace("GkhGji.RiskOrientedMethod.EffectiveKNDIndex", "Показатели эффективности КНД");
            //this.CRUDandViewPermissions("GkhGji.RiskOrientedMethod.EffectiveKNDIndex");

            //this.Namespace("GkhGji.RiskOrientedMethod.ROMCategory", "Расчет коэффициентов риска");
            //this.CRUDandViewPermissions("GkhGji.RiskOrientedMethod.ROMCategory");

            //this.Namespace("GkhGji.RiskOrientedMethod.ROMCategory.Field", "Поля");
            //this.Permission("GkhGji.RiskOrientedMethod.ROMCategory.Field.Vp_Edit", "Коэффициенты - Редактирование");
            //this.Permission("GkhGji.RiskOrientedMethod.ROMCategory.Field.Vp_View", "Коэффициенты - Просмотр");

            this.Namespace("GkhGji.CourtPractice", "Судебная и административная практика");

            this.Namespace("GkhGji.CourtPractice.CourtPracticeRegystry", "Реестр судебной и административной практики");
            this.CRUDandViewPermissions("GkhGji.CourtPractice.CourtPracticeRegystry");

            #endregion      

            #region Справочники
            this.Namespace("GkhGji.Dict", "Справочники");

            this.Namespace("GkhGji.Dict.RegionCode", "Коды регионов");
            this.CRUDandViewPermissions("GkhGji.Dict.RegionCode");

            this.Namespace("GkhGji.Dict.AppealExecutionType", "Тип исполнения обращения");
            this.CRUDandViewPermissions("GkhGji.Dict.AppealExecutionType");

            this.Namespace("GkhGji.Dict.ProsecutorOffice", "Органы прокуратуры");
            this.CRUDandViewPermissions("GkhGji.Dict.ProsecutorOffice");
            
            this.Namespace("GkhGji.Dict.SSTUTransferOrg", "Идентификаторы ССТУ");
            this.CRUDandViewPermissions("GkhGji.Dict.SSTUTransferOrg");         

            this.Namespace("GkhGji.Dict.FLDocType", "Документы физических лиц");
            this.CRUDandViewPermissions("GkhGji.Dict.FLDocType");

            this.Namespace("GkhGji.Dict.EGRNDocType", "Документы ЕГРН");
            this.CRUDandViewPermissions("GkhGji.Dict.EGRNDocType");

            this.Namespace("GkhGji.Dict.EGRNApplicantType", "Категория заявителя ЕГРН");
            this.CRUDandViewPermissions("GkhGji.Dict.EGRNApplicantType");

            this.Namespace("GkhGji.Dict.EGRNObjectType", "Тип объекта запроса ЕГРН");
            this.CRUDandViewPermissions("GkhGji.Dict.EGRNObjectType");

            #endregion

            #region АСФК
            this.Namespace("GkhGji.ASFK", "АСФК");
            this.CRUDandViewPermissions("GkhGji.ASFK");

            this.Namespace("Import.ASFK", "Импорт из Федереального казначейства");
            this.CRUDandViewPermissions("Import.ASFK");
            #endregion

            #region СМЭВ
            //"GkhGji.SMEV.SMEVMVD.View"
            this.Namespace("GkhGji.SMEV", "СМЭВ");

            this.Namespace("GkhGji.SMEV.SMEVSocialHire", "Запросы СГИО");
            this.CRUDandViewPermissions("GkhGji.SMEV.SMEVSocialHire");

            this.Namespace("GkhGji.SMEV.SMEVNDFL", "Запросы по НДФЛ");
            this.CRUDandViewPermissions("GkhGji.SMEV.SMEVNDFL");

            this.Namespace("GkhGji.SMEV.SMEVMVD", "Запросы в МВД");
            this.CRUDandViewPermissions("GkhGji.SMEV.SMEVMVD");

            this.Namespace("GkhGji.SMEV.SMEVPropertyType", "Запрос о принадлежности имущества ");
            this.CRUDandViewPermissions("GkhGji.SMEV.SMEVPropertyType");

            this.Namespace("GkhGji.SMEV.SMEVEGRUL", "Запросы в ЕГРЮЛ");
            this.CRUDandViewPermissions("GkhGji.SMEV.SMEVEGRUL");

            this.Namespace("GkhGji.SMEV.SMEVEGRUL", "Запросы в ЕГРИП"); //ЕГРИП
            this.CRUDandViewPermissions("GkhGji.SMEV.SMEVEGRIP");

            this.Namespace("GkhGji.SMEV.SMEVEGRUL", "Запросы в ЕГРИП");
            this.CRUDandViewPermissions("GkhGji.SMEV.SMEVEGRIP");

            this.Namespace("GkhGji.SMEV.SMEVEGRN", "Запросы в ЕГРН");
            this.CRUDandViewPermissions("GkhGji.SMEV.SMEVEGRN");

            this.Namespace("GkhGji.SMEV.SMEVFNSLicRequest", "Запросы в ФНС");
            this.CRUDandViewPermissions("GkhGji.SMEV.SMEVFNSLicRequest");
            
            this.Namespace("GkhGji.SMEV.GISGMP", "Обмен данными с ГИС ГМП");
            this.CRUDandViewPermissions("GkhGji.SMEV.GISGMP");

            this.Namespace("GkhGji.SMEV.GISERP", "Обмен данными с ГИС ЕРП");
            this.CRUDandViewPermissions("GkhGji.SMEV.GISERP");

            this.Namespace("GkhGji.SMEV.PAYREG", "Реестр платежей");
            this.CRUDandViewPermissions("GkhGji.SMEV.PAYREG");

            this.Namespace("GkhGji.SMEV.SMEVPremises", "Запросы по помещениям");
            this.CRUDandViewPermissions("GkhGji.SMEV.SMEVPremises");

            this.Namespace("GkhGji.SMEV.SMEVGASU", "Запросы ГАС Управление");
            this.CRUDandViewPermissions("GkhGji.SMEV.SMEVGASU");

            this.Namespace("GkhGji.SMEV.SMEVDISKVLIC", "Запросы по дисквалифицированным лицам");
            this.CRUDandViewPermissions("GkhGji.SMEV.SMEVDISKVLIC");

            this.Namespace("GkhGji.SMEV.SMEVExploitResolution", "Сведения о разрешении на ввод в эксплуатацию");
            this.CRUDandViewPermissions("GkhGji.SMEV.SMEVExploitResolution");

            this.Namespace("GkhGji.SMEV.SMEVValidPassport", "Запросы по действительным паспортам");
            this.CRUDandViewPermissions("GkhGji.SMEV.SMEVValidPassport");

            this.Namespace("GkhGji.SMEV.SMEVLivingPlace", "Запросы по местам проживания");
            this.CRUDandViewPermissions("GkhGji.SMEV.SMEVLivingPlace");

            this.Namespace("GkhGji.SMEV.SMEVStayingPlace", "Запросы по местам пребывания");
            this.CRUDandViewPermissions("GkhGji.SMEV.SMEVStayingPlace");

            #endregion

            #region
            this.Namespace("GkhGji.SOPR", "СОПР");
            this.Namespace("GkhGji.SOPR.Appeal", "Отписаные обращения");
            this.CRUDandViewPermissions("GkhGji.SOPR.Appeal");
            this.Permission("GkhGji.SOPR.Appeal.Vp_Edit", "Принято инспектором");
            #endregion

            #region Импорт из АМИРС
            this.Namespace("Administration.AMIRS", "Импорт из АМИРС");
            this.CRUDandViewPermissions("Administration.AMIRS");
            #endregion

            this.Namespace("GkhGji.FileRegister", "Архив файлов ГЖИ");
            this.CRUDandViewPermissions("GkhGji.FileRegister");



            this.Namespace<Protocol197>("GkhGji.DocumentsGji.Protocol197", "Протокол по ст.19.7 КоАП РФ");
            this.Permission("GkhGji.DocumentsGji.Protocol197.View", "Просмотр");

            //this.Namespace<AppealCits>("GkhGji.AppealCitizensState.AppealCitsAdmonition", "Предостережения");
            //this.Permission("GkhGji.AppealCitizensState.AppealCitsAdmonition.View", "Просмотр записей");
            //this.Permission("GkhGji.AppealCitizensState.AppealCitsAdmonition.Create", "Создание записей");
            //this.Permission("GkhGji.AppealCitizensState.AppealCitsAdmonition.Delete", "Удаление записей");
            //this.Namespace("GkhGji.AppealCitizensState.AppealCitsAdmonition.Field", "Поля");
            //this.Permission("GkhGji.AppealCitizensState.AppealCitsAdmonition.Field.All", "Доступность основных записей");
            //this.Permission("GkhGji.AppealCitizensState.AppealCitsAdmonition.Field.Answer", "Файл ответа");
            //this.Namespace("GkhGji.AppealCitizensState.AppealCitsAdmonition.Registry", "Реестры");
            //this.Namespace("GkhGji.AppealCitizensState.AppealCitsAdmonition.Registry.Violation", "Нарушения");
            //this.Permission("GkhGji.AppealCitizensState.AppealCitsAdmonition.Registry.Violation.Create", "Создание записей");
            //this.Permission("GkhGji.AppealCitizensState.AppealCitsAdmonition.Registry.Violation.Delete", "Удаление записей");

            //this.Namespace<AppealCits>("GkhGji.AppealCitizensState.AppealCitsPrescriptionFond", "Предписания ФКР");
            //this.Permission("GkhGji.AppealCitizensState.AppealCitsPrescriptionFond.View", "Просмотр записей");
            //this.Permission("GkhGji.AppealCitizensState.AppealCitsPrescriptionFond.Create", "Создание записей");
            //this.Permission("GkhGji.AppealCitizensState.AppealCitsPrescriptionFond.Delete", "Удаление записей");
            //this.Namespace("GkhGji.AppealCitizensState.AppealCitsPrescriptionFond.Field", "Поля");
            //this.Permission("GkhGji.AppealCitizensState.AppealCitsPrescriptionFond.Field.All", "Доступность основных записей");
            //this.Permission("GkhGji.AppealCitizensState.AppealCitsPrescriptionFond.Field.Answer", "Файл ответа");
            //this.Namespace("GkhGji.AppealCitizensState.AppealCitsPrescriptionFond.Registry", "Реестры");
            //this.Namespace("GkhGji.AppealCitizensState.AppealCitsPrescriptionFond.Registry.Violation", "Нарушения");
            //this.Permission("GkhGji.AppealCitizensState.AppealCitsPrescriptionFond.Registry.Violation.Create", "Создание записей");
            //this.Permission("GkhGji.AppealCitizensState.AppealCitsPrescriptionFond.Registry.Violation.Delete", "Удаление записей");
            //this.Namespace("GkhGji.AppealCitizensState.AppealCitsPrescriptionFond.Registry.ObjectCr", "Объекты КР");
            //this.Permission("GkhGji.AppealCitizensState.AppealCitsPrescriptionFond.Registry.ObjectCr.Create", "Создание записей");
            //this.Permission("GkhGji.AppealCitizensState.AppealCitsPrescriptionFond.Registry.ObjectCr.Delete", "Удаление записей");
        }
    }
}