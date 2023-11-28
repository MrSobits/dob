namespace Bars.Gkh
{
    using B4;
    using System.Linq;
    using TextValues;

    /// <summary>
    ///     Меню, навигация
    /// </summary>
    public class NavigationProvider : INavigationProvider
    {
        /// <summary>
        /// Текстовое обозначение пунктов меню
        /// </summary>
        public IMenuItemText MenuItemText { get; set; }

        /// <summary>
        /// Ключ
        /// </summary>
        public string Key
        {
            get
            {
                return MainNavigationInfo.MenuName;
            }
        }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description
        {
            get
            {
                return MainNavigationInfo.MenuDescription;
            }
        }
        
        /// <summary>
        /// Инициализация меню
        /// </summary>
        /// <param name="root"></param>
        public void Init(MenuItem root)
        {
            var adminRoot = root.Add("Администрирование").WithIcon("gkh-admin");
            adminRoot.Add("Информация о системе").Add("Версия", "system_version").AddRequiredPermission("Administration.Version.View");

            adminRoot.Add("Настройка пользователей").Add("Операторы", "operatoradministration").AddRequiredPermission("Administration.Operator.View").WithIcon("operator");
            adminRoot.Add("Настройка пользователей").Add("Профиль", "profilesettingadministration").AddRequiredPermission("Administration.Profile.View");

            adminRoot.Add("Настройки приложения").Add("Настройки приложения", "gkhparams").AddRequiredPermission("Administration.GkhParams.View");
            adminRoot.Add("Настройки приложения").Add("Единые настройки приложения", "gkhconfig").AddRequiredPermission("Administration.GkhParams.View");
            adminRoot.Add("Настройки приложения").Add("Реестр блокировок таблиц", "tablelock").AddRequiredPermission("Administration.TableLock.View");

            adminRoot.Add("Задачи").Add("Список задач", "gkhtaskscontroller").AddRequiredPermission("Administration.GkhTasksController.View");
            adminRoot.Add("Шаблоны").Add("Шаблоны документов", "templatereplacement").AddRequiredPermission("Administration.TemplateReplacement.View").WithIcon("templReplAdmin");
            adminRoot.Add("Логи").Add("Логи загрузок", "importlog").AddRequiredPermission("Administration.ImportLog.View");
            adminRoot.Add("Логи").Add("Логи операций", "operationlog").AddRequiredPermission("Administration.OperationLog.View");
            adminRoot.Add("Новости и инструкции").Add("Документация", "instructiongroupsmanager").AddRequiredPermission("Administration.InstructionGroups.View").WithIcon("instruction");
            adminRoot.Add("Выполнение действий").Add("Выполнение действий", "executionAction").AddRequiredPermission("Administration.ExecutionAction.View");

            root.Add("Отчеты").WithIcon("gkh-report");
            root.Add("Жилищный фонд");
            root.Add("Участники процесса").WithIcon("gkh-contragent");
            //root.Add("Капитальный ремонт").WithIcon("gkh-cap-repair");
            root.Add("Аналитика.Управление").WithIcon("gkh-cap-repair");
            //root.Add("Региональный фонд").WithIcon("gkh-reg-fund");
            //root.Add("Претензионная работа");
            //root.Add(this.MenuItemText.GetText("Правонарушения")).Add(this.MenuItemText.GetText("Материалы о правонарушениях"))
            //    .AddRequiredPermission("Gkh.Dictionaries.Suggestion.CitizenSuggestionViewCreate.View");

            root.Add("Административная комиссия").WithIcon("gkh-gji");
            //root.Add("Раскрытие информации").WithIcon("gkh-disinfo");
            var dictsRoot = root.Add("Справочники").WithIcon("gkh-dict");

            //var menuLicense = root.Add("Административная комиссия").Add("Лицензирование");
            //menuLicense.Add("Реестр должностных лиц", "person").AddRequiredPermission("Gkh.Person.View").WithIcon("menuPerson");
            //menuLicense.Add("Реестр претендентов на сдачу квалификационного экзамена", "requesttoexamregister").AddRequiredPermission("Gkh.RequestToExamRegister.View").WithIcon("menuRequestToExamRegister");
            //menuLicense.Add("Реестр квалификационных аттестатов", "qualifysertifiateregistry").AddRequiredPermission("Gkh.RequestToExamRegister.View");
            //menuLicense.Add("Обращения за выдачей лицензии", "manorgrequestlicense").AddRequiredPermission("Gkh.ManOrgLicense.Request.View").WithIcon("menuManorgRequestLicense");
            //menuLicense.Add("Реестр лицензий", "manorglicenseregister").AddRequiredPermission("Gkh.ManOrgLicense.License.View").WithIcon("menuManorgLicenseRegister");

            //var menucscalc = root.Add("Административная комиссия").Add("Электронный инспектор");
            //menucscalc.Add("Тарифы и нормативы", "tarifnormative").AddRequiredPermission("Gkh.CSCalculation.CSFormula.View").WithIcon("billing");
            //menucscalc.Add("Коэффициенты", "mocoefficient").AddRequiredPermission("Gkh.CSCalculation.CSFormula.View").WithIcon("billing");
            //menucscalc.Add("Формулы расчета", "cscalculationformula").AddRequiredPermission("Gkh.CSCalculation.CSFormula.View").WithIcon("stateTransfer");
            //menucscalc.Add("Реестр расчетов платы за ЖКУ", "cscalculation").AddRequiredPermission("Gkh.CSCalculation.Calculate.View").WithIcon("menuRequestToExamRegister");
           

            var commonDictsRoot = dictsRoot.Add("Общие");
            commonDictsRoot.Add("Муниципальные образования", "municipality").AddRequiredPermission("Gkh.Dictionaries.Municipality.View").WithIcon("municipality");
            commonDictsRoot.Add("Тип участника комиссии", "position").AddRequiredPermission("Gkh.Dictionaries.Position.View").WithIcon("position");
            commonDictsRoot.Add("Организационно-правовые формы", "orgform").AddRequiredPermission("Gkh.Dictionaries.OrganizationForm.View").WithIcon("orgForm");
            commonDictsRoot.Add("Единицы измерения", "unitmeasure").AddRequiredPermission("Gkh.Dictionaries.UnitMeasure.View").WithIcon("unitMeasure");
            commonDictsRoot.Add("Нормативные документы", "normativedoc").AddRequiredPermission("Gkh.Dictionaries.NormativeDoc.View");
            //commonDictsRoot.Add("Типы домов", "realestatetype").AddRequiredPermission("Gkh.Dictionaries.RealEstateType.View");

            commonDictsRoot.Add("Дерево муниципальных образований", "municipalityTree").AddRequiredPermission("Gkh.Dictionaries.MunicipalityTree.View").WithIcon("municipality");
            //commonDictsRoot.Add("Универсальные справочники", "multipurposeglossary").AddRequiredPermission("Gkh.Dictionaries.Multipurpose.View");
            //commonDictsRoot.Add("Документы подрядных организаций", "builderdocumenttype").AddRequiredPermission("Gkh.Dictionaries.BuilderDocumentType.View");
            //commonDictsRoot.Add("ЦТП", "centralheatingstation").AddRequiredPermission("Gkh.Dictionaries.CentralHeatingStation.View");

            //var gkhDictsRoot = dictsRoot.Add("Жилищно-коммунальное хозяйство");
            //gkhDictsRoot.Add("Группы капитальности", "capitalgroup").AddRequiredPermission("Gkh.Dictionaries.CapitalGroup.View");
            //gkhDictsRoot.Add("Типы технического мониторинга", "monitoringtypedict").AddRequiredPermission("Gkh.Dictionaries.MonitoringTypeDict.View");
            //gkhDictsRoot.Add("Конструктивные элементы", "constructiveelement").AddRequiredPermission("Gkh.Dictionaries.ConstructiveElement.View");
            //root.Add("Справочники")
            //    .Add("Жилищно-коммунальное хозяйство")
            //    .Add("Группы конструктивных элементов", "constructiveelementgroup")
            //    .AddRequiredPermission("Gkh.Dictionaries.ConstructiveElementGroup.View");

            //gkhDictsRoot.Add("Виды видеонаблюдения", "videooverwatchtype").AddRequiredPermission("Gkh.Dictionaries.VideoOverwatchType.View").WithIcon("reminderHead");

            //gkhDictsRoot.Add("Работы и услуги организации", "organizationwork").AddRequiredPermission("Gkh.Dictionaries.OrganizationWork.View");
            //gkhDictsRoot.Add("Работы по содержанию и ремонту МКД", "contentrepairmkdwork").AddRequiredPermission("Gkh.Dictionaries.ContentRepairMkdWork.View");

            //gkhDictsRoot.Add("Приборы учета", "meterdevice").AddRequiredPermission("Gkh.Dictionaries.MeteringDevice.View");
            //gkhDictsRoot.Add("Материалы кровли", "roofingmaterial").AddRequiredPermission("Gkh.Dictionaries.RoofingMaterial.View");
            //gkhDictsRoot.Add("Материалы стен", "wallmaterial").AddRequiredPermission("Gkh.Dictionaries.WallMaterial.View");
            //gkhDictsRoot.Add("Формы собственности", "typeownership").AddRequiredPermission("Gkh.Dictionaries.TypeOwnership.View");
            //gkhDictsRoot
            //    .Add("Виды работ текущего ремонта", "currentworkkindrepair")
            //    .AddRequiredPermission("Gkh.Dictionaries.WorkKindCurrentRepair.View");
            //gkhDictsRoot.Add("Типы проектов", "typeproject").AddRequiredPermission("Gkh.Dictionaries.TypeProject.View");
            //gkhDictsRoot.Add("Типы обслуживания", "typeservice").AddRequiredPermission("Gkh.Dictionaries.TypeService.View");
            //gkhDictsRoot.Add("Особые признаки строения", "buildingfeature").AddRequiredPermission("Gkh.Dictionaries.BuildingFeature.View");
            //gkhDictsRoot.Add("Типы категорий МКД", "typecategorycs").AddRequiredPermission("Gkh.Dictionaries.TypeCategoryCS.View");
            //gkhDictsRoot.Add("Категории МКД", "categorycsmkd").AddRequiredPermission("Gkh.CSCalculation.CSFormula.View");
            //gkhDictsRoot.Add("Причина расторжения договора", "stopreason").AddRequiredPermission("Gkh.Dictionaries.StopReason.View");

            //gkhDictsRoot
            //    .Add("Услуги по договорам управления", "managementcontractservice")
            //    .AddRequiredPermission("Gkh.Dictionaries.ManagementContractService.View");
            //var programDictsRoot = dictsRoot.Add("Программы");
            //programDictsRoot.Add("Периоды программ", "period").AddRequiredPermission("Gkh.Dictionaries.Period.View");
            //programDictsRoot.Add("Варианты дальнейшего использования", "furtheruse").AddRequiredPermission("Gkh.Dictionaries.FurtherUse.View");
            //programDictsRoot.Add("Основания нецелесообразности", "reasoninexpedient").AddRequiredPermission("Gkh.Dictionaries.ReasonInexpedient.View");
            //programDictsRoot.Add("Источники по программам переселения", "resettlementprogsource").AddRequiredPermission("Gkh.Dictionaries.ResettlementProgramSource.View");

            //var overhaulDictsRoot = dictsRoot.Add("Капитальный ремонт");
            //overhaulDictsRoot.Add("Специальности", "speciality").AddRequiredPermission("Gkh.Dictionaries.Specialty.View");
            //overhaulDictsRoot.Add("Учебные заведения", "institutions").AddRequiredPermission("Gkh.Dictionaries.Institutions.View");
            //overhaulDictsRoot.Add("Виды оснащения", "kindequipment").AddRequiredPermission("Gkh.Dictionaries.KindEquipment.View");
            //overhaulDictsRoot.Add("Виды работ", "work").AddRequiredPermission("Gkh.Dictionaries.Work.View").WithIcon("work");
            root.Add("Справочники").Add("Общие").Add(this.MenuItemText.GetText("Комиссии"), "zonalinspection").AddRequiredPermission("Gkh.Dictionaries.ZonalInspection.View");
            root.Add("Справочники").Add("Общие").Add("Тип платежного документа", "licenseprovideddoc").AddRequiredPermission("Gkh.Dictionaries.LicenseProvidedDoc.View");
            //root.Add("Справочники").Add("Комиссии").Add("Причины переоформления лицензии", "licenseregistrationreason").AddRequiredPermission("Gkh.Dictionaries.LicenseRegistrationReason.View");

            root.Add("Справочники").Add("Общие").Add("Члены комиссии", "inspector").AddRequiredPermission("Gkh.Dictionaries.Inspector.View").WithIcon("inspector");
           
            root.Add("Справочники").Add("Комиссии").Add("Нарушители", "individualperson").AddRequiredPermission("Gkh.Dictionaries.IndividualPerson.View");

            //root.Add("Справочники").Add("Комиссии").Add("Вопросы квалификационного экзамена", "qtestquestionsdict").AddRequiredPermission("Gkh.Dictionaries.QualifyTestQuestions");
            //root.Add("Справочники").Add("Комиссии").Add("Настройки квалификационного экзамена", "qtestsettingsdict").AddRequiredPermission("Gkh.Dictionaries.QualifyTestQuestions");

            //root.Add("Справочники").Add("Раскрытие информации");

            //root.Add("Справочники").Add("Капитальный ремонт").Add("Средняя стоимость квадратного метра", "livingsquarecost").AddRequiredPermission("Gkh.Dictionaries.LivingSquareCost");

            root.Add(this.MenuItemText.GetText("Правонарушения")).Add(this.MenuItemText.GetText("Материалы о правонарушениях")).Add(this.MenuItemText.GetText("Информация о правонарушениях"), "citizensuggestion").AddRequiredPermission("Gkh.Dictionaries.Suggestion.CitizenSuggestion.View");
            root.Add(this.MenuItemText.GetText("Правонарушения")).Add(this.MenuItemText.GetText("Материалы о правонарушениях")).Add("Вид материала", "rubric").AddRequiredPermission("Gkh.Dictionaries.Suggestion.Rubric.View");
            root.Add(this.MenuItemText.GetText("Правонарушения")).Add(this.MenuItemText.GetText("Материалы о правонарушениях")).Add("Места правонарушений", "problemplace").AddRequiredPermission("Gkh.Dictionaries.ProblemPlace.View");
            root.Add(this.MenuItemText.GetText("Правонарушения")).Add(this.MenuItemText.GetText("Материалы о правонарушениях")).Add("Дополнительные атрибуты", "categoryposts").AddRequiredPermission("Gkh.Dictionaries.CategoryPosts.View");

            //root.Add("Справочники").Add("Отопительный сезон");
            //root.Add("Справочники").Add("Деятельность ТСЖ");
            //root.Add("Справочники").Add("Страхование").Add("Виды деятельности страховой организации", "belayorgkindactivity").AddRequiredPermission("Gkh.Dictionaries.BelayOrgKindActivity.View");
            //root.Add("Справочники").Add("Страхование").Add("Виды рисков", "kindrisk").AddRequiredPermission("Gkh.Dictionaries.KindRisk.View");

            //root.Add("Справочники").Add("Комиссии").Add("Статусы протоколов МКД", "protocolmkdstate").AddRequiredPermission("Gkh.Dictionaries.ProtocolMKDState.View");
            //root.Add("Справочники").Add("Комиссии").Add("Источники протоколов МКД", "protocolmkdsource").AddRequiredPermission("Gkh.Dictionaries.ProtocolMKDSource.View");
            //root.Add("Справочники").Add("Комиссии").Add("Инициаторы протоколов МКД", "protocolmkdiniciator").AddRequiredPermission("Gkh.Dictionaries.ProtocolMKDIniciator.View");

            root.Add("Жилищный фонд").Add("Объекты жилищного фонда").Add("Реестр жилых домов", "realityobject").AddRequiredPermission("Gkh.RealityObject.View").WithIcon("realObj");
            //root.Add("Жилищный фонд").Add("Объекты жилищного фонда").Add("Мониторинг жилищного фонда", "housingfundmonitoring").AddRequiredPermission("Gkh.HousingFundMonitoringPeriod.View").WithIcon("houseMonitoring");
            root.Add("Жилищный фонд").Add("Аварийность").Add("Реестр аварийных домов", "emergencyobject").AddRequiredPermission("Gkh.EmergencyObject.View").WithIcon("emergencyObj");
            //root.Add("Жилищный фонд").Add("Аварийность").Add("Массовая смена статусов обьектов строительства", "constructionobjectmasschangestate").AddRequiredPermission("Gkh.ConstructionObjectMassStateChange.View");
            //root.Add("Жилищный фонд")
            //    .Add("Аварийность")
            //    .Add("Программы переселения", "resettlementprogram")
            //    .AddRequiredPermission("Gkh.Dictionaries.ResettlementProgram.View")
            //    .WithIcon("resettlementProg");

            root.Add("Участники процесса").Add("Контрагенты").Add("Контрагенты", "contragent").AddRequiredPermission("Gkh.Orgs.Contragent.View").WithIcon("contragent");
            root.Add("Участники процесса").Add("Роли контрагента").Add("Управляющие организации", "managingorganization").AddRequiredPermission("Gkh.Orgs.Managing.View").WithIcon("manOrg");
            root.Add("Участники процесса").Add("Роли контрагента").Add("Поставщики коммунальных услуг", "supplyresorg").AddRequiredPermission("Gkh.Orgs.SupplyResource.View").WithIcon("supplyResOrg");
            root.Add("Участники процесса").Add("Роли контрагента").Add("Поставщики жилищных услуг", "serviceorganization").AddRequiredPermission("Gkh.Orgs.Serv.View").WithIcon("serviceOrg");
            root.Add("Участники процесса").Add("Роли контрагента").Add("Подрядные организации", "builder").AddRequiredPermission("Gkh.Orgs.Builder.View").WithIcon("builder");
            root.Add("Участники процесса").Add("Роли контрагента").Add("Страховые организации", "belayorg").AddRequiredPermission("Gkh.Orgs.Belay.View");
            root.Add("Участники процесса").Add("Роли контрагента").Add("Органы местного самоуправления", "localgovernment").AddRequiredPermission("Gkh.Orgs.LocalGov.View").WithIcon("localGovernment");
            root.Add("Участники процесса").Add("Роли контрагента").Add("Органы государственной власти", "politicauthority").AddRequiredPermission("Gkh.Orgs.PoliticAuth.View").WithIcon("politicAuth");
            root.Add("Участники процесса").Add("Роли контрагента").Add("Платежные агенты", "paymentagent").AddRequiredPermission("Gkh.Orgs.PaymentAgent.View").WithIcon("contragent");
            root.Add("Участники процесса").Add("Роли контрагента").Add("Технические заказчики", "technicalcustomers").AddRequiredPermission("Gkh.Orgs.TechnicalCustomer.View").WithIcon("work");
            root.Add("Участники процесса").Add("Роли контрагента").Add("Жилищные инспекции", "housinginspection").AddRequiredPermission("Gkh.Orgs.HousingInspection.View").WithIcon("menuPerson");
            
            //root.Add("Жилищный фонд").Add("Страхование").Add("Страхование деятельности УО", "belaymanorgactivity").AddRequiredPermission("Gkh.BelayManOrgActivity.View").WithIcon("belayMoAct");

            // пункты из платформы, для добавления иконок
            root.Add("Администрирование").Add("Настройка пользователей").Add("Роли", "role").AddRequiredPermission("B4.Security.Role").WithIcon("role");
            root.Add("Администрирование")
                .Add("Настройка пользователей")
                .Add("Настройка локальных администраторов", "localadminrolesettings")
                .AddRequiredPermission("B4.Security.LocalAdminRoleSettings")
                .WithIcon("rolePermission");

            root.Add("Администрирование")
                .Add("Настройка прав доступа")
                .Add("Настройка ограничений", "rolepermission")
                .AddRequiredPermission("B4.Security.AccessRights")
                .WithIcon("rolePermission");
            root.Add("Администрирование").Add("Настройка прав доступа").Add("Обязательность полей", "fieldrequirement").AddRequiredPermission("B4.Security.FieldRequirement");
            root.Add("Администрирование").Add("Настройки статусов").Add("Статусы", "B4.controller.State").AddRequiredPermission("B4.States.State.View").WithIcon("state");
            root.Add("Администрирование")
                .Add("Настройки статусов")
                .Add("Переходы статусов", "B4.controller.StateTransfer")
                .AddRequiredPermission("B4.States.StateTransfer.View")
                .WithIcon("stateTransfer");
            root.Add("Администрирование")
                .Add("Настройки статусов")
                .Add("Правила перехода статусов", "B4.controller.StateTransferRule")
                .AddRequiredPermission("B4.States.StateTransferRule.View")
                .WithIcon("stateTransferRule");
            root.Add("Администрирование")
                .Add("Настройки статусов")
                .Add("Настройка ограничений по статусам", "statepermission")
                .AddRequiredPermission("B4.States.StatePermission.View")
                .WithIcon("statePermission");
            root.Add("Администрирование").Add("Новости и инструкции").Add("Новости", "B4.controller.Administrator.News").AddRequiredPermission("News.Administering").WithIcon("news");
            root.Add("Администрирование").Add("Аудит").Add("Последние действия", "B4.controller.LastUserAction").AddRequiredPermission("B4.Audit.LastActions").WithIcon("lastUserAction");
            root.Add("Администрирование").Add("Аудит").Add("Аудит действий", "B4.controller.UserAction").AddRequiredPermission("B4.Audit.Audit").WithIcon("userAction");
            root.Add("Администрирование").Add("Аудит").Add("Статистика действий", "B4.controller.UserActionStatistic").AddRequiredPermission("B4.Audit.AuditStat").WithIcon("userActionStat");
            root.Add("Администрирование").Add("Аудит").Add("Уникальные пользователи", "B4.controller.UniqueUsers").AddRequiredPermission("B4.Audit.UniqueUsers").WithIcon("uniqueUsers");          
            //root.Add("Администрирование").Add("ОКТМО").Add("Привязка населенного пункта", "fiasoktmo").AddRequiredPermission("Administration.Oktmo.fiasoktmo.View");
            //root.Add("Администрирование").Add("ОКТМО").Add("Данные из ОКТМО", "oktmodataimport").AddRequiredPermission("Import.Oktmo");
            //root.Add("Администрирование").Add("ОКТМО").Add("Импорт ОКТМО для населенных пунктов", "municipalityfiasoktmoimport").AddRequiredPermission("Import.MunicipalityFiasOktmo");
            var reportRoot = root.Add("Отчеты").Add("Отчеты");

            var reportItem = reportRoot.Items.FirstOrDefault(x => x.Caption == "Отчеты");
            if (reportItem != null)
            {
                reportRoot.Items.Remove(reportItem);
            }
            reportRoot.Add("Категории печатных форм", "B4.controller.PrintFormCategory").WithIcon("printFormCategory");
            reportRoot.Add("Справочник печатных форм", "B4.controller.PrintForm").WithIcon("printForm");
            
            /*root.Add("Администрирование")
                .Add("Импорты")
                .Add("Импорт лицевых счетов (Сведения из ЖКУ)", "gkuimport")
                .AddRequiredPermission("Import.Gku.View");
            root.Add("Администрирование")
                .Add("Импорты")
                .Add("Импорт данных из Биллинга", "billingimport")
                .AddRequiredPermission("Import.Billing.View");*/
            //root.Add("Администрирование")
            //    .Add("Импорт жилых домов")
            //    .Add("Импорт жилых домов из Реформы ЖКХ", "roimport")
            //    .AddRequiredPermission("Import.RoImport.View");
            //root.Add("Администрирование")
            //    .Add("Импорт жилых домов")
            //    .Add("Импорт жилых домов из фонда", "roimportfromfund")
            //    .AddRequiredPermission("Import.RoImportFromFund.View");

            root.Add("Администрирование")
                .Add("Импорты")
                .Add("Импорт операторов", "importoperator")
                .AddRequiredPermission("Import.Operator.View");
            //root.Add("Администрирование")
            //    .Add("Импорты")
            //    .Add("Импорт лицензий", "managingorganizationimport")
            //    .AddRequiredPermission("Import.ManagingOrganization.View");
            //root.Add("Администрирование")
            //    .Add("Импорты")
            //    .Add("Импорт организаций (с созданием договоров с домами)", "importorganization")
            //    .AddRequiredPermission("Import.OrganizationImport");
            //root.Add("Администрирование")
            //    .Add("Импорты")
            //    .Add("Импорт лифтов (техпаспорт)", "tpelevatorsimport")
            //    .AddRequiredPermission("Import.TpElevatorsImport");

            adminRoot.Get("Новости и инструкции").Caption = "Новости и документация";

            this.RemoveEmptySettingsItem(ref root);
            this.RemoveMessengerItem(ref root);
        }

        /// <summary>
        /// Удалить пустой пункт меню "Настройки"
        /// </summary>
        /// <param name="root"></param>
        private void RemoveEmptySettingsItem(ref MenuItem root)
        {
            var settingsItem = root.Items.FirstOrDefault(item => item.Caption == "Настройки");

            // Меню "Настройки" пустое (в нём есть один пункт "Профиль", который также пуст)
            if (settingsItem != null &&
                (settingsItem.Items.Count == 0 ||
                (settingsItem.Items.Count == 1 && settingsItem.Items[0].Caption == "Профиль" && settingsItem.Items[0].Items.Count == 0)))
            {
                root.Items.Remove(settingsItem);
            }
        }
        /// <summary>
        /// Удалить пункт меню "Сообщения"
        /// </summary>
        /// <param name="root"></param>
        private void RemoveMessengerItem(ref MenuItem root)
        {
            var administrationItem = root.Items.FirstOrDefault(item => item.Caption == "Администрирование");

            if (administrationItem != null)
            {
                var messengerItem = administrationItem.Items.FirstOrDefault(item => item.Caption == "Сообщения");

                if (messengerItem != null)
                {
                    administrationItem.Items.Remove(messengerItem);
                }
            }
        }
    }
}