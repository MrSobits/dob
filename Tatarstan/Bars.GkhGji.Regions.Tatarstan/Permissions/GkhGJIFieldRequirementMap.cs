namespace Bars.GkhGji.Regions.Tatarstan.Permissions
{
    using Bars.Gkh.DomainService;

    /// <summary>
    /// Обязательность полей
    /// </summary>
    public class GkhGjiFieldRequirementMap : FieldRequirementMap
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public GkhGjiFieldRequirementMap()
        {
            this.Requirement("GkhGji.Inspection.BaseJurPerson.Field.CountHours", "Срок проверки (количество Часов)");
            this.Requirement("GkhGji.Inspection.BaseJurPerson.Field.InspectionBaseType", "Основание проверки");

            this.Namespace("GkhGji.DocumentReestrGji.Disposal", "Распоряжение");
            this.Namespace("GkhGji.DocumentReestrGji.Disposal.Field", "Поля");
            this.Requirement("GkhGji.DocumentReestrGji.Disposal.Field.TimeVisitStart", "Время с");
            this.Requirement("GkhGji.DocumentReestrGji.Disposal.Field.TimeVisitEnd", "Время по");
            this.Requirement("GkhGji.DocumentReestrGji.Disposal.Field.NcDate", "Дата");
            this.Requirement("GkhGji.DocumentReestrGji.Disposal.Field.NcNum", "Номер документа");
            this.Requirement("GkhGji.DocumentReestrGji.Disposal.Field.NcDateLatter", "Дата исходящего письма");
            this.Requirement("GkhGji.DocumentReestrGji.Disposal.Field.NcNumLatter", "Номер исходящего письма");
            this.Requirement("GkhGji.DocumentReestrGji.Disposal.Field.NcSent", "Уведомление передано");
            this.Requirement("GkhGji.DocumentReestrGji.Disposal.Field.NcObtained", "Уведомление получено");

            this.Requirement("GkhGji.DocumentReestrGji.ActCheck.Field.DocumentPlace", "Место составления");
            this.Requirement("GkhGji.DocumentReestrGji.ActCheck.Field.DocumentTime", "Время составления акта");
            this.Requirement("GkhGji.DocumentReestrGji.ActCheck.Field.AcquaintState", "Статус ознакомления с результатами проверки");
            this.Requirement("GkhGji.DocumentReestrGji.ActCheck.Field.RefusedToAcquaintPerson", "ФИО должностного лица, отказавшегося от ознакомления с актом проверки");
            this.Requirement("GkhGji.DocumentReestrGji.ActCheck.Field.AcquaintedPerson", "ФИО должностного лица, ознакомившегося с актом проверки");
            this.Requirement("GkhGji.DocumentReestrGji.ActCheck.Field.AcquaintedDate", "Дата ознакомления");

            this.Namespace("GkhGji.DocumentReestrGji.Protocol", "Протокол");
            this.Namespace("GkhGji.DocumentReestrGji.Protocol.Field", "Поля");
            this.Requirement("GkhGji.DocumentReestrGji.Protocol.Field.FormatPlace", "Место и время составления протокола");
            this.Requirement("GkhGji.DocumentReestrGji.Protocol.Field.FormatDate", "Дата вручения (регистрации) уведомления");
            this.Requirement("GkhGji.DocumentReestrGji.Protocol.Field.NotifNumber", "Номер регистрации");
            this.Requirement("GkhGji.DocumentReestrGji.Protocol.Field.DateOfProceedings", "Дата и время расмотрения дела");
            this.Requirement("GkhGji.DocumentReestrGji.Protocol.Field.ProceedingCopyNum", "Количество экземпляров");
            this.Requirement("GkhGji.DocumentReestrGji.Protocol.Field.ProceedingsPlace", "Место рассмотрения дела");
            this.Requirement("GkhGji.DocumentReestrGji.Protocol.Field.Remarks", "Замечания со стороны нарушителя");

            this.Namespace("GkhGji.Inspection.BaseStatement", "Обращение граждан");
            this.Namespace("GkhGji.Inspection.BaseStatement.MainInfo", "Основная информация");
            this.Namespace("GkhGji.Inspection.BaseStatement.MainInfo.Field", "Поля");
            this.Requirement("GkhGji.Inspection.BaseStatement.MainInfo.Field.RegistrationNumber", "Учетный номер проверки в едином реестре проверок");
            this.Requirement("GkhGji.Inspection.BaseStatement.MainInfo.Field.RegistrationNumberDate", "Дата присвоения учетного номера");
            this.Requirement("GkhGji.Inspection.BaseStatement.MainInfo.Field.CheckDayCount", "Срок проверки (количество дней)");
            this.Requirement("GkhGji.Inspection.BaseStatement.MainInfo.Field.CheckDate", "Дата проверки");

            this.Namespace("GkhGji.Inspection.BaseProsClaim", "Требование прокуратуры");
            this.Namespace("GkhGji.Inspection.BaseProsClaim.MainInfo", "Основная информация");
            this.Namespace("GkhGji.Inspection.BaseProsClaim.MainInfo.Field", "Поля");
            this.Requirement("GkhGji.Inspection.BaseProsClaim.MainInfo.Field.RegistrationNumber", "Учетный номер проверки в едином реестре проверок");
            this.Requirement("GkhGji.Inspection.BaseProsClaim.MainInfo.Field.RegistrationNumberDate", "Дата присвоения учетного номера");
            this.Requirement("GkhGji.Inspection.BaseProsClaim.MainInfo.Field.CheckDayCount", "Срок проверки (количество дней)");
            this.Requirement("GkhGji.Inspection.BaseProsClaim.MainInfo.Field.CheckDate", "Дата проверки");

            this.Requirement("GkhGji.Inspection.BaseDispHead.MainInfo.Field.RegistrationNumber", "Учетный номер проверки в едином реестре проверок");
            this.Requirement("GkhGji.Inspection.BaseDispHead.MainInfo.Field.RegistrationNumberDate", "Дата присвоения учетного номера");
            this.Requirement("GkhGji.Inspection.BaseDispHead.MainInfo.Field.CheckDayCount", "Срок проверки (количество дней)");
            this.Requirement("GkhGji.Inspection.BaseDispHead.MainInfo.Field.CheckDate", "Дата проверки");
        }
    }
}
