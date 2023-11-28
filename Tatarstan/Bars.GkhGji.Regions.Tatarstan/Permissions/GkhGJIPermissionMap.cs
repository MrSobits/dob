namespace Bars.GkhGji.Regions.Tatarstan.Permissions
{
    using B4;

    public class GkhGjiPermissionMap : PermissionMap
    {
        public GkhGjiPermissionMap()
        {
            this.Permission("GkhGji.DocumentsGji.ActRemoval.Field.SumAmountWorkRemoval_Edit", "Сумма работ по устранению нарушений");

            this.Namespace("GkhGji.GisCharge", "Интеграция с ГИС ГМП");

            this.Permission("GkhGji.GisCharge.View", "Просмотр - Отправка начисленных штрафов");
            this.Permission("GkhGji.GisCharge.ParamsView", "Просмотр - Настройка параметров");

            this.Namespace("GkhGji.Dict.InspectionBaseType", "Основание проверки");
            this.CRUDandViewPermissions("GkhGji.Dict.InspectionBaseType");

            this.Permission("GkhGji.DocumentsGji.ActCheck.Field.AcquaintState_View", "Статус ознакомления с результатами проверки - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ActCheck.Field.AcquaintState_Edit", "Статус ознакомления с результатами проверки - Редактирование");
            this.Permission("GkhGji.DocumentsGji.ActCheck.Field.AcquaintedDate_View", "Дата ознакомления - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ActCheck.Field.AcquaintedDate_Edit", "Дата ознакомления - Редактирование");
            this.Permission("GkhGji.DocumentsGji.ActCheck.Field.RefusedToAcquaintPerson_View", "ФИО должностного лица, отказавшегося от ознакомления с актом проверки - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ActCheck.Field.RefusedToAcquaintPerson_Edit", "ФИО должностного лица, отказавшегося от ознакомления с актом проверки - Редактирование");
            this.Permission("GkhGji.DocumentsGji.ActCheck.Field.AcquaintedPerson_View", "ФИО должностного лица, ознакомившегося с актом проверки - Просмотр");
            this.Permission("GkhGji.DocumentsGji.ActCheck.Field.AcquaintedPerson_Edit", "ФИО должностного лица, ознакомившегося с актом проверки - Редактирование");

            this.Permission("GkhGji.DocumentsGji.Protocol.Field.FormatPlace_View", "Место и время составления протокола - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Protocol.Field.FormatPlace_Edit", "Место и время составления протокола - Редактирование");
            this.Permission("GkhGji.DocumentsGji.Protocol.Field.NotifDeliveredThroughOffice_View", "Вручено через канцелярию - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Protocol.Field.NotifDeliveredThroughOffice_Edit", "Вручено через канцелярию - Редактирование");
            this.Permission("GkhGji.DocumentsGji.Protocol.Field.FormatDate_View", "Дата вручения (регистрации) уведомления - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Protocol.Field.FormatDate_Edit", "Дата вручения (регистрации) уведомления - Редактирование");
            this.Permission("GkhGji.DocumentsGji.Protocol.Field.NotifNumber_View", "Номер регистрации - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Protocol.Field.NotifNumber_Edit", "Номер регистрации - Редактирование");
            this.Permission("GkhGji.DocumentsGji.Protocol.Field.DateOfProceedings_View", "Дата и время расмотрения дела - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Protocol.Field.DateOfProceedings_Edit", "Дата и время расмотрения дела - Редактирование");
            this.Permission("GkhGji.DocumentsGji.Protocol.Field.ProceedingCopyNum_View", "Количество экземпляров - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Protocol.Field.ProceedingCopyNum_Edit", "Количество экземпляров - Редактирование");
            this.Permission("GkhGji.DocumentsGji.Protocol.Field.ProceedingsPlace_View", "Место рассмотрения дела - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Protocol.Field.ProceedingsPlace_Edit", "Место рассмотрения дела - Редактирование");
            this.Permission("GkhGji.DocumentsGji.Protocol.Field.Remarks_View", "Замечания со стороны нарушителя - Просмотр");
            this.Permission("GkhGji.DocumentsGji.Protocol.Field.Remarks_Edit", "Замечания со стороны нарушителя - Редактирование");
            this.Permission("GkhGji.Inspection.BaseJurPerson.Field.CountHours_Edit", "Срок проверки (количество Часов)");

            this.Namespace("GkhGji.DocumentsGji.Disposal.Register.SubjectVerificationGrid", "Предметы проверки");
            this.Permission("GkhGji.DocumentsGji.Disposal.Register.SubjectVerificationGrid.View", "Просмотр");
            this.Permission("GkhGji.DocumentsGji.Disposal.Register.SubjectVerificationGrid.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.Disposal.Register.SubjectVerificationGrid.Delete", "Удаление записей");

            this.Namespace("GkhGji.DocumentsGji.Disposal.Register.SurveyPurposeGrid", "Цели проверки");
            this.Permission("GkhGji.DocumentsGji.Disposal.Register.SurveyPurposeGrid.View", "Просмотр");
            this.Permission("GkhGji.DocumentsGji.Disposal.Register.SurveyPurposeGrid.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.Disposal.Register.SurveyPurposeGrid.Delete", "Удаление записей");

            this.Namespace("GkhGji.DocumentsGji.Disposal.Register.SurveyObjectiveGrid", "Задачи проверки");
            this.Permission("GkhGji.DocumentsGji.Disposal.Register.SurveyObjectiveGrid.View", "Просмотр");
            this.Permission("GkhGji.DocumentsGji.Disposal.Register.SurveyObjectiveGrid.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.Disposal.Register.SurveyObjectiveGrid.Delete", "Удаление записей");

            this.Namespace("GkhGji.DocumentsGji.Disposal.Register.InspFoundationCheckGrid", "НПА проверки");
            this.Permission("GkhGji.DocumentsGji.Disposal.Register.InspFoundationCheckGrid.View", "Просмотр");
            this.Permission("GkhGji.DocumentsGji.Disposal.Register.InspFoundationCheckGrid.Create", "Создание записей");
            this.Permission("GkhGji.DocumentsGji.Disposal.Register.InspFoundationCheckGrid.Delete", "Удаление записей");

            this.Permission("GkhGji.Inspection.BaseStatement.Field.RegistrationNumber_View", "Учетный номер проверки в едином реестре - Просмотр");
            this.Permission("GkhGji.Inspection.BaseStatement.Field.RegistrationNumber_Edit", "Учетный номер проверки в едином реестре - Редактирование");
            this.Permission("GkhGji.Inspection.BaseStatement.Field.RegistrationNumberDate_View", "Дата присвоения учетного номера - Просмотр");
            this.Permission("GkhGji.Inspection.BaseStatement.Field.RegistrationNumberDate_Edit", "Дата присвоения учетного номера - Редактирование");
            this.Permission("GkhGji.Inspection.BaseStatement.Field.CheckDayCount_View", "Срок проверки (количество дней) - Просмотр");
            this.Permission("GkhGji.Inspection.BaseStatement.Field.CheckDayCount_Edit", "Срок проверки (количество дней) - Редактирование");
            this.Permission("GkhGji.Inspection.BaseStatement.Field.CheckDate_View", "Дата проверки - Просмотр");
            this.Permission("GkhGji.Inspection.BaseStatement.Field.CheckDate_Edit", "Дата проверки - Редактирование");

            this.Namespace("GkhGji.Inspection.BaseStatement.Register.Contragent", "Органы совместной проверки");
            this.Permission("GkhGji.Inspection.BaseStatement.Register.Contragent.Create", "Создание записей");
            this.Permission("GkhGji.Inspection.BaseStatement.Register.Contragent.Delete", "Удаление записей");

            this.Permission("GkhGji.Inspection.BaseProsClaim.Field.RegistrationNumber_View", "Учетный номер проверки в едином реестре - Просмотр");
            this.Permission("GkhGji.Inspection.BaseProsClaim.Field.RegistrationNumber_Edit", "Учетный номер проверки в едином реестре - Редактирование");
            this.Permission("GkhGji.Inspection.BaseProsClaim.Field.RegistrationNumberDate_View", "Дата присвоения учетного номера - Просмотр");
            this.Permission("GkhGji.Inspection.BaseProsClaim.Field.RegistrationNumberDate_Edit", "Дата присвоения учетного номера - Редактирование");
            this.Permission("GkhGji.Inspection.BaseProsClaim.Field.CheckDayCount_View", "Срок проверки (количество дней) - Просмотр");
            this.Permission("GkhGji.Inspection.BaseProsClaim.Field.CheckDayCount_Edit", "Срок проверки (количество дней) - Редактирование");
            this.Permission("GkhGji.Inspection.BaseProsClaim.Field.CheckDate_View", "Дата проверки - Просмотр");
            this.Permission("GkhGji.Inspection.BaseProsClaim.Field.CheckDate_Edit", "Дата проверки - Редактирование");

            this.Namespace("GkhGji.Inspection.BaseProsClaim.Register.Contragent", "Органы совместной проверки");
            this.Permission("GkhGji.Inspection.BaseProsClaim.Register.Contragent.Create", "Создание записей");
            this.Permission("GkhGji.Inspection.BaseProsClaim.Register.Contragent.Delete", "Удаление записей");

            this.Permission("GkhGji.Inspection.BaseDispHead.Field.RegistrationNumber_View", "Учетный номер проверки в едином реестре - Просмотр");
            this.Permission("GkhGji.Inspection.BaseDispHead.Field.RegistrationNumber_Edit", "Учетный номер проверки в едином реестре - Редактирование");
            this.Permission("GkhGji.Inspection.BaseDispHead.Field.RegistrationNumberDate_View", "Дата присвоения учетного номера - Просмотр");
            this.Permission("GkhGji.Inspection.BaseDispHead.Field.RegistrationNumberDate_Edit", "Дата присвоения учетного номера - Редактирование");
            this.Permission("GkhGji.Inspection.BaseDispHead.Field.CheckDayCount_View", "Срок проверки (количество дней) - Просмотр");
            this.Permission("GkhGji.Inspection.BaseDispHead.Field.CheckDayCount_Edit", "Срок проверки (количество дней) - Редактирование");
            this.Permission("GkhGji.Inspection.BaseDispHead.Field.CheckDate_View", "Дата проверки - Просмотр");
            this.Permission("GkhGji.Inspection.BaseDispHead.Field.CheckDate_Edit", "Дата проверки - Редактирование");

            this.Namespace("GkhGji.Inspection.BaseDispHead.Register.Contragent", "Органы совместной проверки");
            this.Permission("GkhGji.Inspection.BaseDispHead.Register.Contragent.Create", "Создание записей");
            this.Permission("GkhGji.Inspection.BaseDispHead.Register.Contragent.Delete", "Удаление записей");
        }
    }
}