namespace Bars.GkhGji.Regions.Voronezh
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

            // реестр рассылок
            root.Add("Административная комиссия").Add("Реестр обращений").Add("Реестр исходящих электронных писем", "emaillist").AddRequiredPermission("GkhGji.AppealCitizens.View").WithIcon("businessActivity");
            root.Add("Администрирование").Add("Импорты").Add("Импорт из АМИРС", "amirsimport").AddRequiredPermission("Administration.AMIRS.View");

            //АСФК
            root.Add("Административная комиссия").Add("Межведомственное взаимодействие").Add("АС Федерального казначейства", "asfk").AddRequiredPermission("GkhGji.ASFK.View");
            root.Add("Административная комиссия").Add("Межведомственное взаимодействие").Add("Импорт из Федерального казначейства", "asfkimport").AddRequiredPermission("Import.ASFK.View");

            //СМЭВ
            root.Add("Административная комиссия").Add("Межведомственное взаимодействие").Add("Сведения о доходах (2-НДФЛ)", "ndfl").AddRequiredPermission("GkhGji.SMEV.SMEVNDFL.View");

            root.Add("Административная комиссия").Add("Межведомственное взаимодействие").Add("Сведения из ЕГРЮЛ", "smevegrul").AddRequiredPermission("GkhGji.SMEV.SMEVEGRUL.View");
            root.Add("Административная комиссия").Add("Межведомственное взаимодействие").Add("Сведения из ЕГРИП", "smevegrip").AddRequiredPermission("GkhGji.SMEV.SMEVEGRIP.View"); //егрип
            root.Add("Административная комиссия").Add("Межведомственное взаимодействие").Add("Тестирование СМЭВ", "smevdo").AddRequiredPermission("GkhGji.SMEV.SMEVEGRIP.View"); //егрип
            root.Add("Административная комиссия").Add("Межведомственное взаимодействие").Add("Сведения о наличии (отсутствии) судимости", "smevmvd").AddRequiredPermission("GkhGji.SMEV.SMEVMVD.View");
      
            root.Add("Административная комиссия").Add("Межведомственное взаимодействие").Add("Обмен информацией с ГИС ГМП", "gisgmp").AddRequiredPermission("GkhGji.SMEV.GISGMP.View");
          
            root.Add("Административная комиссия").Add("Межведомственное взаимодействие").Add("Реестр платежей", "payreg").AddRequiredPermission("GkhGji.SMEV.PAYREG.View");
          //  root.Add("Административная комиссия").Add("Межведомственное взаимодействие").Add("Запросы в ГосИмущество", "smevpropertytype").AddRequiredPermission("GkhGji.SMEV.SMEVPropertyType.View");
            root.Add("Административная комиссия").Add("Межведомственное взаимодействие").Add("Информация из Реестра дисквалифицированных лиц", "diskvlic").AddRequiredPermission("GkhGji.SMEV.SMEVDISKVLIC.View");
            root.Add("Административная комиссия").Add("Межведомственное взаимодействие").Add("Предоставление СНИЛС", "smevsnils").AddRequiredPermission("GkhGji.SMEV.SMEVDISKVLIC.View");
            root.Add("Административная комиссия").Add("Межведомственное взаимодействие").Add("Сведения из ФГИС ЕГРН", "smevegrn").AddRequiredPermission("GkhGji.SMEV.SMEVEGRN.View");

            //root.Add("Справочники").Add("Комиссии").Add("Коды регионов", "regioncode").AddRequiredPermission("GkhGji.Dict.RegionCode.View");
            root.Add("Справочники").Add("Комиссии").Add("Отделы прокуратуры", "prosecutoroffice").AddRequiredPermission("GkhGji.Dict.ProsecutorOffice.View");
            //root.Add("Справочники").Add("Комиссии").Add("Тип исполнения обращения", "appealexecutiontype").AddRequiredPermission("GkhGji.Dict.AppealExecutionType.View");
            //root.Add("Справочники").Add("Комиссии").Add("Идентификаторы ССТУ", "sstutransferorg").AddRequiredPermission("GkhGji.Dict.SSTUTransferOrg.View");
            root.Add("Справочники").Add("Комиссии").Add("Коды документов физических лиц", "fldoctype").AddRequiredPermission("GkhGji.Dict.FLDocType.View");
            root.Add("Справочники").Add("Общие").Add("Нарушители", "individualperson").AddRequiredPermission("Gkh.Dictionaries.IndividualPerson.View").WithIcon("inspector");
            //root.Add("Справочники").Add("Комиссии").Add("Статусы плательщиков", "gisgmppayerstatus").AddRequiredPermission("GkhGji.SMEV.GISGMP.View"); 
            //root.Add("Справочники").Add("Комиссии").Add("Категория заявителя ЕГРН", "egrnapplicanttype").AddRequiredPermission("GkhGji.Dict.EGRNApplicantType.View");
            root.Add("Справочники").Add("Комиссии").Add("Объект запроса ЕГРН", "egrnobjecttype").AddRequiredPermission("GkhGji.Dict.EGRNObjectType.View");
            root.Add("Справочники").Add("Комиссии").Add("Документы ЕГРН", "egrndoctype").AddRequiredPermission("GkhGji.Dict.EGRNDocType.View");
           
            root.Add("Административная комиссия").Add("Судебная практика").Add("Реестр судебной практики", "courtpractice").WithIcon("reminderHead").AddRequiredPermission("GkhGji.CourtPractice.CourtPracticeRegystry.View");
          
            root.Add("Административная комиссия")
          .Add("Документы")
          .Add("Архив документов Комиссии", "fileregister")
          .AddRequiredPermission("GkhGji.FileRegister.View");
        }
    }
}
