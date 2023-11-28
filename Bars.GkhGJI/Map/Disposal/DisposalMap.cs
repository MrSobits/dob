namespace Bars.GkhGji.Map
{
    using Bars.Gkh.Map;
    using Bars.GkhGji.Entities;

    /// <summary>Маппинг для "Рапоряжение ГЖИ"</summary>
    public class DisposalMap : GkhJoinedSubClassMap<Disposal>
    {

        public DisposalMap() : base("GJI_DISPOSAL")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.TypeDisposal, "Тип распоряжения").Column("TYPE_DISPOSAL").NotNull();
            this.Property(x => x.KindKNDGJI, "Вид контроля/надзора").Column("KIND_KND").NotNull();
            this.Property(x => x.TypeAgreementProsecutor, "Согласование с прокуратурой").Column("TYPE_AGRPROSECUTOR").NotNull();
            this.Property(x => x.DocumentNumberWithResultAgreement, "Номер документа с результатом согласования").Column("DOCUMENT_NUMBER_WITH_RESULT_AGREEMENT");
            this.Property(x => x.TypeAgreementResult, "Результат согласования").Column("TYPE_AGRRESULT").NotNull();
            this.Property(x => x.DocumentDateWithResultAgreement, "Дата документа с результатом согласования").Column("DOCUMENT_DATE_WITH_RESULT_AGREEMENT");
            this.Property(x => x.DateStart, "Дата начала обследования").Column("DATE_START");
            this.Property(x => x.DateEnd, "Дата окончания обследования").Column("DATE_END");
            this.Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(2000);
            this.Property(x => x.ObjectVisitStart, "Выезд на объект с").Column("OBJECT_VISIT_START");
            this.Property(x => x.ObjectVisitEnd, "Выезд на объект по").Column("OBJECT_VISIT_END");
            this.Property(x => x.OutInspector, "Выезд инспектора в командировку").Column("OUT_INSPECTOR").NotNull();
            this.Reference(x => x.KindCheck, "Вид проверки").Column("KIND_CHECK_ID").Fetch();
            this.Reference(x => x.ProcAprooveFile, "Вид проверки").Column("APROOVE_FILE_ID").Fetch();
            this.Reference(x => x.IssuedDisposal, "Должностное лицо (ДЛ) вынесшее распоряжение").Column("ISSUED_DISPOSAL_ID").Fetch();
            this.Reference(x => x.ResponsibleExecution, "Ответственный за исполнение").Column("RESPONSIBLE_EXECUT_ID").Fetch();

            this.Reference(x => x.Contragent, "Связь с таблицей контрагентов").Column("CONTRAGENT_ID").Fetch();
            this.Reference(x => x.ContragentContact, "Связь с таблицей контакты контрагентов").Column("CONTRAGENT_CONTACT_ID").Fetch();
            this.Reference(x => x.IndividualPerson, "Связь с таблицей физ лица").Column("INDIVIDUAL_PERSON_ID").Fetch();
            this.Reference(x => x.Municipality, "Связь с таблицей муниципальных образований").Column("MUNICIPALITY_ID").Fetch();

            this.Property(x => x.TimeVisitStart, "Время начала обследования").Column("TIME_VISIT_SART");
            this.Property(x => x.TimeVisitEnd, "Время окончания обследования").Column("TIME_VISIT_END");
            this.Property(x => x.NcNum, "Номер документа").Column("NC_NUM");
            this.Property(x => x.NcDate, "Дата документа").Column("NC_DATE");
            this.Property(x => x.ProcAprooveDate, "Дата согласования").Column("APROOVE_DATE");
            this.Property(x => x.ProcAprooveNum, "Номер согласования").Column("APROOVE_NUMBER");
            this.Property(x => x.NcNumLatter, "Номер исходящего письма").Column("NC_NUM_LETTER");
            this.Property(x => x.NcDateLatter, "Дата исходящего пиьма").Column("NC_DATE_LETTER");
            this.Property(x => x.NcObtained, "Уведомление получено").Column("NC_OBTAINED");
            this.Property(x => x.NcSent, "Уведомление отправлено").Column("NC_SENT");
            this.Property(x => x.ERPID, "Ид в версии").Column("ERPID");
            this.Property(x => x.FioProcAproove, "ФИО специалиста прокуратуры").Column("FIO_DOC_APPROVE");
            this.Property(x => x.PositionProcAproove, "должность специалиста прокуратуры").Column("POSITION_DOC_APPROVE");
            this.Property(x => x.TypeViolator, "Тип нарушителя").Column("TYPE_VIOLATOR");
            this.Property(x => x.Fio, "ФИО физ лица ").Column("FIO");
            this.Property(x => x.PlaceResidence, "Место регистрации физ лица").Column("PLACE_RESIDENCE");
            this.Property(x => x.ActuallyResidence, "Место фактического жительства физ лица").Column("ACTUALLY_RESIDENCE");
            this.Property(x => x.BirthPlace, "Место рождения физ лица").Column("BIRTH_PLACE");
            this.Property(x => x.Job, "Место работы").Column("JOB");
            this.Property(x => x.DateBirth, "Дата рождения").Column("DATE_BIRTH");
            this.Property(x => x.PassportNumber, "Номер паспорта").Column("PASSPORT_NUMBER");
            this.Property(x => x.PassportSeries, "Серия паспорта").Column("PASSPORT_SERIES");
            this.Property(x => x.PassportIssued, "Паспорт выдан").Column("PASSPORT_ISSUED");
            this.Property(x => x.DepartmentCode, "Код подразделения").Column("DEPARTMENT_CODE");
            this.Property(x => x.DateIssue, "Дата выдачи паспорта физ лицу ").Column("DATE_ISSUE");
            this.Property(x => x.INN, "ИНН").Column("INN");
            this.Property(x => x.FamilyStatus, "Семейное положение").Column("FAMILY_STATUS");

            this.Property(x => x.NameTransport, "Наименование транспорта").Column("NAME_TRANSPORT");
            this.Property(x => x.NamberTransport, "Номер транспорта").Column("NAMBER_TRANSPORT");
            this.Property(x => x.RegistrationNamberTransport, "Регистрационный номер транспорта").Column("REGISTRATION_NAMBER_TRANSPORT");
            this.Property(x => x.SeriesTransport, "Серия номера транспорта").Column("SERIES_TRANSPORT");
            this.Property(x => x.RegNamberTransport, "Регион регистрации номера транспорта").Column("REG_NAMBER_TRANSPORT");

            this.Property(x => x.NoticeDate, "Дата явки").Column("NOTICE_DATE");
            this.Property(x => x.NoticeTime, "Время явки").Column("NOTICE_TIME");
            this.Property(x => x.AddresСom, "Адрес комиссии").Column("ADDRES_COM");
            this.Property(x => x.AddresDepartures, "Адрес оправления").Column("ADDRES_DEPARTURES");
            this.Property(x => x.Postcode, "Почтовый индекс").Column("POSTCODE");
            this.Property(x => x.NumberKUSP, "КУСП").Column("NUMBER_KUSP");
            this.Property(x => x.ToAttracted, "Привлечение нарушителя").Column("TO_ATTRACTED");
        }
    }
}
