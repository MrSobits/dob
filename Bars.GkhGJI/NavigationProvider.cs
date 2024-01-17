// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NavigationProvider.cs" company="">
//   
// </copyright>
// <summary>
//   Меню, навигация
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Bars.GkhGji
{
    using B4;

    using Bars.Gkh.TextValues;

    /// <summary>
    /// Меню, навигация
    /// </summary>
    public class NavigationProvider : INavigationProvider
    {
        public IMenuItemText MenuItemText { get; set; }

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
            root.Add("Административная комиссия").Add("Основания проверок").Add("Проверки по требованию прокуратуры", "baseprosclaim").AddRequiredPermission("GkhGji.Inspection.BaseProsClaim.View").WithIcon("baseProsClaim");
            root.Add("Административная комиссия").Add("Основания проверок").Add(MenuItemText.GetText("Проверки по обращениям граждан"), "basestatement").AddRequiredPermission("GkhGji.Inspection.BaseStatement.View").WithIcon("baseStatement");
            root.Add("Административная комиссия").Add("Основания проверок").Add(MenuItemText.GetText("Инспекционные проверки"), "baseinscheck").AddRequiredPermission("GkhGji.Inspection.BaseInsCheck.View").WithIcon("baseInsCheck");
            root.Add("Административная комиссия").Add("Основания проверок").Add("Проверки по факту выявления правонарушения", "basedisphead").AddRequiredPermission("GkhGji.Inspection.BaseDispHead.View").WithIcon("baseDispHead");
            root.Add("Административная комиссия").Add("Основания проверок").Add("Проверки без основания", "basedefault").AddRequiredPermission("GkhGji.Inspection.BaseDefault.View");

            // реестр обращений пока невозможно перевести на роуты, пожтому пока вернул черещ Котроллер
            root.Add("Административная комиссия").Add("Реестр обращений").Add("Реестр обращений", "B4.controller.AppealCits").AddRequiredPermission("GkhGji.AppealCitizens.View").WithIcon("appealCits");

            root.Add("Административная комиссия").Add("Документы").Add("Постановления прокуратуры", "resolpros").AddRequiredPermission("GkhGji.DocumentsGji.ResolPros.View").WithIcon("resolPros");
            //root.Add("Административная комиссия").Add("Документы").Add("Протоколы Комиссии", "protocolmvd").AddRequiredPermission("GkhGji.DocumentsGji.ProtocolMvd.View");
            //root.Add("Административная комиссия").Add("Документы").Add("Протоколы МЖК", "protocolmhc").AddRequiredPermission("GkhGji.DocumentsGji.ProtocolMhc.View");
            //root.Add("Административная комиссия").Add("Документы").Add("Протоколы РСО", "protocolrso").AddRequiredPermission("GkhGji.DocumentsGji.ProtocolRSO.View");
            root.Add("Административная комиссия").Add("Документы").Add("Реестр документов Комиссии", "B4.controller.documentsgjiregister.Navigation").AddRequiredPermission("GkhGji.DocumentsGji.View").WithIcon("documentGji");
            root.Add("Административная комиссия").Add("Документы").Add("Заседания комиссии", "comissionmeeting").AddRequiredPermission("GkhGji.Comissions.ComissionMeeting.View").WithIcon("qualification");
            //root.Add("Административная комиссия").Add("Документы").Add("Запросы по лицензированию УК", "edslicrequest").AddRequiredPermission("Gkh.ManOrgLicense.EDSLicRequest.View");
            //root.Add("Административная комиссия").Add("Деятельность ТСЖ и ЖСК").Add("Деятельность ТСЖ и ЖСК", "activitytsj").AddRequiredPermission("GkhGji.ActivityTsj.View").WithIcon("activityTsj");
            //root.Add("Административная комиссия").Add("Подготовка к отопительному сезону").Add("Подготовка к работе в зимних условиях", "workwintercondition").AddRequiredPermission("GkhGji.WorkWinterCondition.View");
            //root.Add("Административная комиссия").Add("Подготовка к отопительному сезону").Add("Информация о подаче тепла", "heatinputinformation").AddRequiredPermission("GkhGji.HeatInputInformation.View");
            //root.Add("Административная комиссия").Add("Подготовка к отопительному сезону").Add("Документы по подготовке к отопительному сезону", "heatseason").AddRequiredPermission("GkhGji.HeatSeason.View");
            //root.Add("Административная комиссия").Add("Подготовка к отопительному сезону").Add("Котельные", "boilerrooms").AddRequiredPermission("GkhGji.HeatSeason.BoilerRooms.View").WithIcon("heatSeason");
            //root.Add("Административная комиссия").Add("Подготовка к отопительному сезону").Add("Массовая смена статусов документов", "heatseasdocmasschangestate").AddRequiredPermission("GkhGji.HeatSeasonDocMassChangeState.View").WithIcon("heatSeasonMassStateChange");
            //root.Add("Административная комиссия").Add("Подготовка к отопительному сезону").Add("Сведения о наличии и расходе топлива ЖКХ", "fuelinfoperiod").AddRequiredPermission("GkhGji.HeatSeason.FuelInfoPeriod.View");

            var gjiMenu = root.Add("Административная комиссия");
            //gjiMenu.Add("Реестр уведомлений").Add("Реестр уведомлений о начале предпринимательской деятельности", "businessactivity").AddRequiredPermission("GkhGji.BusinessActivityViewCreate.View").WithIcon("businessActivity");
            //gjiMenu.ReOrder("Реестр уведомлений");
            
            root.Add("Административная комиссия").Add("Настройки Комиссии").Add("Настройка правил проставления видов проверок", "kindcheckrulereplace").AddRequiredPermission("GkhGji.Settings.KindCheckRuleReplace.View");
            root.Add("Административная комиссия").Add("Настройки Комиссии").Add("Настройка правил проставления номеров документов", "docnumvalidationrule").AddRequiredPermission("GkhGji.Settings.DocNumValidationRule.View");
            root.Add("Административная комиссия").Add("Настройки Комиссии").Add("Настройка параметров", "paramsgji").AddRequiredPermission("GkhGji.Settings.Params.View");

            root.Add("Административная комиссия").Add("Управление задачами").Add("Панель руководителя Комиссии", "reminderHead").AddRequiredPermission("GkhGji.ManagementTask.ReminderInspector.View").WithIcon("reminderHead");
            root.Add("Административная комиссия").Add("Управление задачами").Add("Доска задач Члена Комиссии", "reminderInspector").AddRequiredPermission("GkhGji.ManagementTask.ReminderHead.View").WithIcon("reminderInspector");

            root.Add("Административная комиссия")
                .Add("Формирование плана проверок")
                .Add("Реестр планов", "surveyplan")
                .AddRequiredPermission("GkhGji.SurveyPlan.View");

            //root.Add("Справочники").Add("Комиссии").Add("Планы проверок юридических лиц", "planjurpersongji").AddRequiredPermission("GkhGji.Dict.PlanJurPerson.View");
            //root.Add("Справочники").Add("Комиссии").Add("Планы инспекционных проверок", "planinscheckgji").AddRequiredPermission("GkhGji.Dict.PlanInsCheck.View");
            //root.Add("Справочники").Add("Комиссии").Add("Планы мероприятий", "planactiongji").AddRequiredPermission("GkhGji.Dict.PlanActionGji.View");
            root.Add("Справочники").Add("Комиссии").Add("Нарушения", "violationgji").AddRequiredPermission("GkhGji.Dict.Violation.View");
            root.Add("Справочники").Add("Комиссии").Add("Группы нарушений", "violationfeaturegji").AddRequiredPermission("GkhGji.Dict.ViolationGroup.View");
            root.Add("Справочники").Add("Комиссии").Add("Характеристики нарушений", "featureviolgji").AddRequiredPermission("GkhGji.Dict.FeatureViol.View");
            //root.Add("Справочники").Add("Комиссии").Add("Инспектируемые части", "inspectedpartgji").AddRequiredPermission("GkhGji.Dict.InspectedPart.View");
            root.Add("Справочники").Add("Общие").Add("Типы нарушения", "articlelawgji").AddRequiredPermission("GkhGji.Dict.ArticleLaw.View");
            root.Add("Справочники").Add("Общие").Add("Составители", "executantdocgji").AddRequiredPermission("GkhGji.Dict.ExecutantDoc.View");
            root.Add("Справочники").Add("Комиссии").Add("Виды санкций", "sanctiongji").AddRequiredPermission("GkhGji.Dict.Sanction.View");
            //  root.Add("Справочники").Add("Общие").Add("Нарушители", "individualperson").AddRequiredPermission("Gkh.Dictionaries.IndividualPerson.View");
            root.Add("Справочники").Add("Общие").Add("Повестка на комиссию", "subpoena").AddRequiredPermission("GkhGji.Dict.Subpoena.View");

            root.Add("Справочники").Add("Комиссии").Add("Предоставляемые документы", "provideddocgji").AddRequiredPermission("GkhGji.Dict.ProvidedDoc.View");
            root.Add("Справочники").Add("Комиссии").Add("Типы обследования", "typesurveygji").AddRequiredPermission("GkhGji.Dict.TypeSurvey.View");
            root.Add("Справочники").Add("Комиссии").Add("Виды проверок", "kindcheckgji").AddRequiredPermission("GkhGji.Dict.KindCheck.View");
            root.Add("Справочники").Add("Комиссии").Add("Эксперты", "expertgji").AddRequiredPermission("GkhGji.Dict.Expert.View");
            root.Add("Справочники").Add("Комиссии").Add("Виды санкций", "sanctiongji").AddRequiredPermission("GkhGji.Dict.Sanction.View");
            root.Add("Справочники").Add("Комиссии").Add("Виды суда", "typecourtgji").AddRequiredPermission("GkhGji.Dict.TypeCourt.View");
            root.Add("Справочники").Add("Комиссии").Add("Решения суда", "courtverdictgji").AddRequiredPermission("GkhGji.Dict.CourtVerdict.View");
            root.Add("Справочники").Add("Комиссии").Add("Инстанции", "instancegji").AddRequiredPermission("GkhGji.Dict.Instance.View");
            //root.Add("Справочники").Add("Общие").Add("Признак составителя", "competentorggji").AddRequiredPermission("GkhGji.Dict.CompetentOrg.View");
            root.Add("Справочники").Add("Комиссии").Add("Содержание ответа", "answercontentgji").AddRequiredPermission("GkhGji.Dict.AnswerContent.View");
            root.Add("Справочники").Add("Комиссии").Add("Тематики обращений", "statsubjectgji").AddRequiredPermission("GkhGji.Dict.StatSubject.View");
            root.Add("Справочники").Add("Комиссии").Add("Подтематики", "statsubsubjectgji").AddRequiredPermission("GkhGji.Dict.StatSubsubject.View");
            root.Add("Справочники").Add("Комиссии").Add("Признак волокиты", "redtapeflaggji").AddRequiredPermission("GkhGji.Dict.RedtapeFlag.View");
            root.Add("Справочники").Add("Комиссии").Add("Мероприятия по устранению нарушений", "actionsremovviolgji").AddRequiredPermission("GkhGji.Dict.ActionsRemovViol.View");
            root.Add("Справочники").Add("Комиссии").Add("Мероприятия по контролю", "controlactivity").AddRequiredPermission("GkhGji.Dict.ActionsRemovViol.View");
            //         root.Add("Справочники").Add("Комиссии").Add("Органы, принимающие решение по предписанию", "decisionmakeingauthoritygji").AddRequiredPermission("GkhGji.Dict.DecisionMakingAuthorityGji.View");
            //         root.Add("Справочники").Add("Комиссии").Add("Цели проверки", "surveypurpose").AddRequiredPermission("GkhGji.Dict.SurveyPurpose.View");
            //         root.Add("Справочники").Add("Комиссии").Add("Задачи проверки", "surveyobjective").AddRequiredPermission("GkhGji.Dict.SurveyObjective.View");
            //         root.Add("Справочники").Add("Комиссии").Add("Типы запросов лицензии МКД", "mkdlictyperequest").AddRequiredPermission("GkhGji.Dict.MKDLicTypeRequest.View");
            //         root.Add("Справочники").Add("Комиссии").Add("Цели проведения проверки", "auditpurposegji").AddRequiredPermission("GkhGji.Dict.AuditPurposeGji.View");
            root.Add("Справочники").Add("Комиссии").Add("Направления деятельности субъектов проверки", "activitydirection").AddRequiredPermission("GkhGji.Dict.ActivityDirection.View");
            root.Add("Справочники").Add("Комиссии").Add("Коды документов", "documentcode").AddRequiredPermission("GkhGji.Dict.DocumentCode.View");
            root.Add("Справочники").Add("Комиссии").Add("Виды оснований деятельности субъектов проверки", "kindbasedocument").AddRequiredPermission("GkhGji.Dict.KindBaseDocument.View");
            root.Add("Справочники").Add("Комиссии").Add("Предметы проверки", "surveysubject").AddRequiredPermission("GkhGji.Dict.SurveySubject.View");
            root.Add("Справочники").Add("Комиссии").Add("Предмет спора", "typefactviolation").AddRequiredPermission("GkhGji.Dict.TypeFactViolation.View");
            //root.Add("Справочники").Add("Комиссии").Add("Перечень требований к субъектам проверки", "surveysubjectrequirement").AddRequiredPermission("GkhGji.Dict.SurveySubjectRequirement.View");
            //root.Add("Справочники").Add("Комиссии").Add("Наименования требований по устранению нарушений", "resolveviolationclaim").AddRequiredPermission("GkhGji.Dict.ResolveViolationClaim.View");
            //root.Add("Справочники").Add("Комиссии").Add("Причины уведомлений", "notificationcauses").AddRequiredPermission("GkhGji.Dict.NotificationCause.View");
            //root.Add("Справочники").Add("Комиссии").Add("Способы управления МКД", "mkdmanagementmethods").AddRequiredPermission("GkhGji.Dict.MkdManagementMethod.View");
            root.Add("Справочники").Add("Комиссии").Add("Органы МВД", "organmvd").AddRequiredPermission("GkhGji.Dict.OrganMvd.View");
            //         root.Add("Справочники").Add("Комиссии").Add("Основание проверки", "inspectionbasetype").AddRequiredPermission("GkhGji.Dict.InspectionBaseType.View");
            //         root.Add("Справочники").Add("Комиссии").Add("Категории риска УК", "riskcategory").AddRequiredPermission("GkhGji.Dict.RiskCategory.View");
            root.Add("Справочники").Add("Комиссии").Add("Рабочий календарь", "prodcalendar").AddRequiredPermission("GkhGji.Dict.RiskCategory.View");
            root.Add("Справочники").Add("Общие").Add("Коды типов документов", "physicalpersondoctype").AddRequiredPermission("GkhGji.Dict.PhysicalPersonDocType.View");
            root.Add("Справочники").Add("Общие").Add("Решения", "concederationresult").AddRequiredPermission("GkhGji.Dict.ConcederationResult.View");
            root.Add("Справочники").Add("Общие").Add("Социальный статус", "socialstatus").AddRequiredPermission("GkhGji.AppealCitizens.AppealCitsInfo.View");
            root.Add("Справочники").Add("Комиссии").Add("Отделы судебных приставов", "OSP").AddRequiredPermission("GkhGji.RiskCategory.View");

            root.Add("Справочники").Add("Обращения").Add("Виды обращений", "kindstatementgji").AddRequiredPermission("GkhGji.Dict.KindStatement.View");
            root.Add("Справочники").Add("Обращения").Add("Источники поступлений", "revenuesourcegji").AddRequiredPermission("GkhGji.Dict.RevenueSource.View");
            root.Add("Справочники").Add("Обращения").Add("Формы поступлений", "revenueformgji").AddRequiredPermission("GkhGji.Dict.RevenueForm.View");
            root.Add("Справочники").Add("Обращения").Add("Резолюции", "resolvegji").AddRequiredPermission("GkhGji.Dict.Resolve.View");
            //root.Add("Справочники").Add("Отопительный сезон").Add("Периоды", "heatseasonperiodgji").AddRequiredPermission("GkhGji.Dict.HeatSeasonPeriod.View");
            root.Add("Справочники").Add("Общие").Add("Состояние протокола", "kindprotocoltsj").AddRequiredPermission("GkhGji.Dict.KindProtocolTsj.View");
            //root.Add("Справочники").Add("Деятельность ТСЖ").Add("Статьи устава", "articletsj").AddRequiredPermission("GkhGji.Dict.ArticleTsj.View");
            //root.Add("Справочники").Add("Жилищно-коммунальное хозяйство").Add("Виды работ (уведомления)", "kindworknotifgji").AddRequiredPermission("GkhGji.Dict.KindWorkNotif.View");

            //root.Add("Администрирование").Add("Импорты").Add("Импорт обращений в жилищную инспекцию", "importappeal").AddRequiredPermission("Import.Tarif.View");
            root.Add("Администрирование").Add("Логи").Add("Книга регистрации обращений", "appealcitsinfo").AddRequiredPermission("GkhGji.AppealCitizens.AppealCitsInfo.View");

            //Владелец спецсчета
            root.Add("Участники процесса").Add("Роли контрагента").Add("Владельцы спецсчетов", "specaccownergrid").AddRequiredPermission("Gkh.Orgs.SpecAccOwner.View").WithIcon("manOrg");

            //заявки на внесение изменений в реестр лицензий
            //var menuLicense = root.Add("Административная комиссия").Add("Лицензирование");
            //menuLicense.Add("Внесение изменений в реестр лицензий", "mkdlicrequest").AddRequiredPermission("Gkh.ManOrgLicense.MKDLicRequest.View").WithIcon("objCrMassChangeStatus");
        }
    }
}