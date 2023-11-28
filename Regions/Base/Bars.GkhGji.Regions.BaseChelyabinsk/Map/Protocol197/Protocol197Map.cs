namespace Bars.GkhGji.Regions.BaseChelyabinsk.Map.Protocol197
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol197;

    /// <summary>
    /// Маппинг для сущности "Протокол"
    /// </summary>
	public class Protocol197Map : JoinedSubClassMap<Protocol197>
    {
		public Protocol197Map() :
			base("Протокол по ст.19.7 КоАП РФ", "GJI_PROTOCOL197")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.PhysicalPerson, "Физическое лицо").Column("PHYSICAL_PERSON").Length(300);
            this.Property(x => x.PhysicalPersonInfo, "Реквизиты физ. лица").Column("PHYSICAL_PERSON_INFO").Length(500);
            this.Property(x => x.DateToCourt, "Дата передачи в суд").Column("DATE_TO_COURT");
            this.Property(x => x.ToCourt, "Документ передан в суд").Column("TO_COURT");
            this.Property(x => x.Description, "Примечание").Column("DESCRIPTION").Length(2000);
            this.Property(x => x.DateOfProceedings, "Дата рассмотрения дела").Column("DATE_OF_PROCEEDINGS");
            this.Property(x => x.HourOfProceedings, "Время рассмотрения дела(час)").Column("HOUR_OF_PROCEEDINGS");
            this.Property(x => x.MinuteOfProceedings, "Время рассмотрения дела(мин)").Column("MINUTE_OF_PROCEEDINGS");
    //        this.Property(x => x.PersonFollowConversion, "Лицо, выполнившее перепланировку/переустройство").Column("PERSON_FOLLOW_CONVERSION");
			this.Property(x => x.FormatDate, "FormatDate").Column("FORMAT_DATE");
			this.Property(x => x.FormatPlace, "FormatPlace").Column("FORMAT_PLACE").Length(500);
			this.Property(x => x.FormatHour, "FormatHour").Column("FORMAT_HOUR");
			this.Property(x => x.FormatMinute, "FormatMinute").Column("FORMAT_MINUTE");
            this.Property(x => x.PhysicalPersonPosition, "Должность").Column("PP_POSITION");
            this.Property(x => x.NotifNumber, "NotifNumber").Column("NOTIF_NUM").Length(100);
			this.Property(x => x.ProceedingsPlace, "ProceedingsPlace").Column("PROCEEDINGS_PLACE").Length(100);
			this.Property(x => x.Remarks, "Remarks").Column("REMARKS").Length(100);
			this.Property(x => x.PersonRegistrationAddress, "PersonRegistrationAddress").Column("PERSON_REG_ADDRESS").Length(250);
			this.Property(x => x.PersonFactAddress, "PersonFactAddress").Column("PERSON_FACT_ADDRESS").Length(250);
			this.Property(x => x.PersonJob, "PersonJob").Column("PERSON_JOB");
			this.Property(x => x.PersonPosition, "PersonPosition").Column("PERSON_POSITION");
			this.Property(x => x.PersonBirthDatePlace, "PersonBirthDatePlace").Column("PERSON_BIRTHDATE").Length(250);
			this.Property(x => x.PersonDoc, "PersonDoc").Column("PERSON_DOC");
			this.Property(x => x.PersonSalary, "PersonSalary").Column("PERSON_SALARY");
			this.Property(x => x.PersonRelationship, "PersonRelationship").Column("PERSON_RELAT");
			this.Property(x => x.TypePresence, "TypePresence").Column("TYPE_PRESENCE");
			this.Property(x => x.Representative, "Representative").Column("REPRESENTATIVE").Length(500);
			this.Property(x => x.ReasonTypeRequisites, "ReasonTypeRequisites").Column("REASON_TYPE_REQ").Length(1000);
			this.Property(x => x.NotifDeliveredThroughOffice, "NotifDeliveredThroughOffice").Column("DELIV_THROUGH_OFFICE");
			this.Property(x => x.ProceedingCopyNum, "ProceedingCopyNum").Column("PROCEEDING_COPY_NUM");
			this.Property(x => x.DateOfViolation, "DateOfViolation").Column("DATE_OF_VIOLATION");
			this.Property(x => x.HourOfViolation, "HourOfViolation").Column("HOUR_OF_VIOLATION");
			this.Property(x => x.MinuteOfViolation, "MinuteOfViolation").Column("MINUTE_OF_VIOLATION");
            this.Property(x => x.UIN, "УИН ГИС ГМП").Column("UIN");
            this.Reference(x => x.ResolveViolationClaim, "ResolveViolationClaim").Column("RESOLVE_VIOL_CLAIM_ID");
			this.Reference(x => x.Executant, "Тип исполнителя документа").Column("EXECUTANT_ID").Fetch();
            this.Reference(x => x.Contragent, "Контрагент").Column("CONTRAGENT_ID").Fetch();
			this.Reference(x => x.NormativeDoc, "NormativeDoc").Column("NORMATIVE_DOC_ID");
       //     this.Property(x => x.ControlType, "Вид контроля").Column("CONTROL_TYPE");
            Property(x => x.PhysicalPersonDocumentNumber, "Номер документа").Column("PHYSICALPERSON_DOC_NUM").Length(500);
            Property(x => x.PhysicalPersonIsNotRF, "Гражданство").Column("PHYSICALPERSON_NOT_CITIZENSHIP").DefaultValue(false);
            Property(x => x.PhysicalPersonDocumentSerial, "Серия документа").Column("PHYSICALPERSON_DOC_SERIAL").Length(500);
            Reference(x => x.PhysicalPersonDocType, "Тип документа ФЛ").Column("PHYSICALPERSON_DOCTYPE_ID").Fetch();
            Property(x => x.TypeAddress, "TypeAdress").Column("TYPE_ADDRESS");
            Property(x => x.PlaceOffense, "PlaceOffense").Column("PLACE_OFFENSE");
            this.Reference(x => x.FiasPlaceAddress, "Место выдачи документа").Column("FIAS_PLACE_ADDRESS").Fetch();
            Property(x => x.AddressPlace, "Адрес места совершения правонарушения").Column("ADDRESS_PLACE");
            this.Reference(x => x.JudSector, "Cудебный участок").Column("JUD_SECTOR").Fetch();

            this.Property(x => x.Fio, "ФИО физ лица ").Column("FIO");
            this.Property(x => x.PlaceResidence, "Место регистрации физ лица").Column("PLACE_RESIDENCE");
            this.Property(x => x.ActuallyResidence, "Место фактического жительства физ лица").Column("ACTUALLY_RESIDENCE");
            this.Property(x => x.BirthPlace, "Место рождения физ лица").Column("BIRTH_PLACE");
            this.Property(x => x.Job, "Место работы").Column("JOB");
            this.Property(x => x.DateBirth, "Дата рождения").Column("DATE_BIRTH");
            this.Property(x => x.PassportIssued, "Паспорт выдан").Column("PASSPORT_ISSUED");
            this.Property(x => x.DepartmentCode, "Код подразделения").Column("DEPARTMENT_CODE");
            this.Property(x => x.DateIssue, "Дата выдачи паспорта физ лицу ").Column("DATE_ISSUE");
            this.Property(x => x.INN, "ИНН").Column("INN");
            this.Property(x => x.FamilyStatus, "Семейное положение").Column("FAMILY_STATUS");
            this.Reference(x => x.IndividualPerson, "Связь с таблицей физ лица").Column("INDIVIDUAL_PERSON_ID").Fetch();
            this.Property(x => x.TypeViolator, "Тип нарушителя").Column("TYPE_VIOLATOR");
            this.Reference(x => x.FiasRegistrationAddress, "Место выдачи документа").Column("FIAS_REG_ADDRESS").Fetch();
            this.Reference(x => x.FiasFactAddress, "Место выдачи документа").Column("FIAS_FACT_ADDRESS").Fetch();
            this.Property(x => x.HasAnotherResolutions, "HasAnotherResolutions").Column("REPEATED");
            this.Property(x => x.DependentsNumber, "Количество иждевенцев").Column("DEPENDENTS_NUMBER");
            this.Reference(x => x.SocialStatus, "Социальный статус").Column("SOCIAL_STATE");
            this.Reference(x => x.Transport, "Transport").Column("TRANSPORT_ID");
            this.Property(x => x.PlaceResidenceOutState, "Место регистрации физ лица").Column("PLACE_RESIDENCE_OUTSTATE");
            this.Property(x => x.ActuallyResidenceOutState, "Место фактического жительства физ лица").Column("ACTUALLY_RESIDENCE_OUTSTATE");
            this.Property(x => x.IsPlaceResidenceOutState, "Место регистрации физ лица").Column("IS_PLACE_RESIDENCE_OUTSTATE");
            this.Property(x => x.IsActuallyResidenceOutState, "Место фактического жительства физ лица").Column("IS_ACTUALLY_RESIDENCE_OUTSTATE");
            this.Property(x => x.PhoneNumber, "Контактный телефон физ лица").Column("PHONE_NUMBER");
        }
    }
}