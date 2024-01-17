namespace Bars.GkhGji.Entities
{
    using System;
    using Bars.B4.Modules.FIAS;
    using Bars.Gkh.Enums;
    using Gkh.Entities;

    /// <summary>
    /// Постановление прокуратуры
    /// </summary>
    public class ResolPros : DocumentGji
    {
        /// <summary>
        /// Тип исполнителя документа
        /// </summary>
        public virtual ExecutantDocGji Executant { get; set; }

        /// <summary>
        /// Контрагент
        /// </summary>
        public virtual Contragent Contragent { get; set; }

        /// <summary>
        /// Физическое лицо
        /// </summary>
        public virtual string PhysicalPerson { get; set; }

        /// <summary>
        /// Должность физ. лица
        /// </summary>
        public virtual string PhysicalPersonPosition { get; set; }

        /// <summary>
        /// Реквизиты физ. лица
        /// </summary>
        public virtual string PhysicalPersonInfo { get; set; }

        /// <summary>
        /// Тип документа физ лица
        /// </summary>
        public virtual PhysicalPersonDocType PhysicalPersonDocType { get; set; }

        /// <summary>
        /// Номер документа физлица
        /// </summary>
        public virtual string PhysicalPersonDocumentNumber { get; set; }

        /// <summary>
        /// Серия документа физлица
        /// </summary>
        public virtual string PhysicalPersonDocumentSerial { get; set; }

        /// <summary>
        /// Не является гражданином РФ
        /// </summary>
        public virtual bool PhysicalPersonIsNotRF { get; set; }

        /// <summary>
		/// Адрес регистрации (место жительства, телефон)
		/// </summary>
		public virtual string PersonRegistrationAddress { get; set; }

        /// <summary>
        /// Фактический адрес
        /// </summary>
        public virtual string PersonFactAddress { get; set; }

        /// <summary>
        /// Место работы
        /// </summary>
        public virtual string PersonJob { get; set; }

        /// <summary>
        /// Должность
        /// </summary>
        public virtual string PersonPosition { get; set; }

        /// <summary>
        /// Дата, место рождения
        /// </summary>
        public virtual string PersonBirthDatePlace { get; set; }

        /// <summary>
        /// Место рождения физ лица 
        /// </summary>
        public virtual string BirthPlace { get; set; }

        /// <summary>
        /// Место работы 
        /// </summary>
        public virtual string Job { get; set; }

        /// <summary>
        /// Дата рождения физ лица 
        /// </summary>
        public virtual DateTime? DateBirth { get; set; }

        /// <summary>
        /// Паспорт выдан физ лицу 
        /// </summary>
        public virtual string PassportIssued { get; set; }

        /// <summary>
        /// Код подразделения выдавшего паспорт физ лицу    
        /// </summary>
        public virtual int? DepartmentCode { get; set; }

        /// <summary>
        /// Дата выдачи  паспорта физ лицу   
        /// </summary>
        public virtual DateTime? DateIssue { get; set; }

        /// <summary>
        /// Семейное положение 
        /// </summary>
        public virtual FamilyStatus FamilyStatus { get; set; }

        /// <summary>
		/// Социальный статус
		/// </summary>
		public virtual SocialStatus SocialStatus { get; set; }

        /// <summary>
        /// Социальный статус
        /// </summary>
        public virtual int? DependentsNumber { get; set; }

        /// <summary>
		/// Место регистрации физ лица  
		/// </summary>
		public virtual bool IsPlaceResidenceOutState { get; set; }

        /// <summary>
        /// Место фактического жительства физ лица 
        /// </summary>
        public virtual bool IsActuallyResidenceOutState { get; set; }

        /// <summary>
        /// Контактный телефон
        /// </summary>
        public virtual string PhoneNumber { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
		/// Адрес регистрации - Выбор из ФИАС
		/// </summary>
		public virtual FiasAddress FiasRegistrationAddress { get; set; }

        /// <summary>
        /// Фактический адрес - Выбор из ФИАС
        /// </summary>
        public virtual FiasAddress FiasFactAddress { get; set; }

        /// <summary>
        /// Муниципальное образование (Орган прокуратуры, вынесший постановление)
        /// </summary>
        public virtual Municipality Municipality { get; set; }

        /// <summary>
        /// Дата поступления в ГЖИ
        /// </summary>
        public virtual DateTime? DateSupply { get; set; }

        /// <summary>
        /// Не хранимое поле Акт проверки (подтягивается в методе Get)
        /// </summary>
        public virtual DocumentGji ActCheck { get; set; }

        /// <summary>
        /// Не хранимое поле InspectionId потомучто поле Inspection JSONIgnore ичтобы работат ьна клиенте нужен id инспекции
        /// </summary>
        public virtual long InspectionId { get; set; }

        /// <summary>
        /// Не хранимое поле, говорит о том можно или нет формировать Постановления из карточки Постановления прокуратуры
        /// в методе ResolProsGJIController/Get Идет логика получения правав можно или нет формировать постановление
        /// </summary>
        public virtual bool BlockResolution { get; set; }

        /// <summary>
        /// УИН
        /// </summary>
        public virtual string UIN { get; set; }

        /// <summary>
        /// Не хранимое поле Акт проверки (подтягивается в методе Get)
        /// </summary>
        public virtual ProsecutorOffice ProsecutorOffice { get; set; }
    }
}