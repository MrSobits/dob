namespace Bars.GkhGji.Entities
{
    using System;
    using System.Collections.Generic;
    using Bars.GkhGji.Enums;
    using Bars.Gkh.Entities;
    using Bars.B4.Modules.FIAS;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Протокол
    /// </summary>
    public class Protocol : DocumentGji
    {
        /// <summary>
        /// Протокол - реквизиты - Судебный участок
        /// </summary>
        public virtual JurInstitution JudSector { get; set; }
        /// <summary>
        /// Протокол - реквизиты - Тип адреса - Выбор из ФИАС
        /// </summary>
        public virtual FiasAddress FiasPlaceAddress { get; set; }
        /// <summary>
        /// Протокол - реквизиты - Тип адреса
        /// </summary>
        public virtual TypeAddress TypeAddress { get; set; }

        /// <summary>
        /// Протокол - реквизиты - Место совершения правонарушения
        /// </summary>
        public virtual PlaceOffense PlaceOffense { get; set; }
        /// <summary>
        /// Протокол - реквизиты - Адрес места совершения правонарущения
        /// </summary>
        public virtual string AddressPlace { get; set; }

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
        /// Физическое лицо - должность
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
        /// Дата передачи в суд
        /// </summary>
        public virtual DateTime? DateToCourt { get; set; }

        /// <summary>
        /// Документ передан в суд
        /// </summary>
        public virtual bool ToCourt { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Список нарушений (Используется при создании объекта Предписания)
        /// </summary>
        public virtual List<long> ViolationsList { get; set; }

        /// <summary>
        /// Список родительских документов (Используется при создании объекта)
        /// </summary>
        public virtual List<long> ParentDocumentsList { get; set; }

        /// <summary>
        /// Не хранимое поле. идентификатор Постановления. Для того чтобы несколько раз нельзя было созлавать постановление
        /// </summary>
        public virtual long? ResolutionId { get; set; }

        /// <summary>
        /// Дата рассмотрения дела
        /// </summary>
        public virtual DateTime? DateOfProceedings { get; set; }

        /// <summary>
        /// Время рассмотрения дела(час)
        /// </summary>
        public virtual int HourOfProceedings { get; set; }

        /// <summary>
        /// Время рассмотрения дела(мин)
        /// </summary>
        public virtual int MinuteOfProceedings { get; set; }

        /// <summary>
        /// Лицо, выполнившее перепланировку/переустройство
        /// </summary>
        public virtual string PersonFollowConversion { get; set; }

        /// <summary>
        /// Место составления
        /// </summary>
        public virtual string FormatPlace { get; set; }

        /// <summary>
        /// Часы времени составления
        /// </summary>
        public virtual int? FormatHour { get; set; }

        /// <summary>
        /// Минуты времени составления
        /// </summary>
        public virtual int? FormatMinute { get; set; }

        /// <summary>
        /// Доставлено через канцелярию
        /// </summary>
        public virtual bool NotifDeliveredThroughOffice { get; set; }

        /// <summary>
        ///  Дата  составления протокола 
        /// </summary>
        public virtual DateTime? FormatDate { get; set; }

        /// <summary>
        /// Номер уведомления о месте и времени составления протокола
        /// </summary>
        public virtual string NotifNumber { get; set; }

        /// <summary>
        /// Количество экземпляров
        /// </summary>
        public virtual int ProceedingCopyNum { get; set; }

        /// <summary>
        /// Место рассмотрения дела
        /// </summary>
        public virtual string ProceedingsPlace { get; set; }

        /// <summary>
        /// Замечания к протоколу со стороны нарушителя
        /// </summary>
        public virtual string Remarks { get; set; }

        /// <summary>
        /// УИН
        /// </summary>
        public virtual string UIN { get; set; }

        /// <summary>
        /// ФИО физ лица  
        /// </summary>
        public virtual string Fio { get; set; }

        /// <summary>
        /// Место регистрации физ лица  
        /// </summary>
        public virtual string PlaceResidence { get; set; }

        /// <summary>
        /// Место фактического жительства физ лица 
        /// </summary>
        public virtual string ActuallyResidence { get; set; }

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
        /// Номер паспорта физ лица 
        /// </summary>
        public virtual int? PassportNumber { get; set; }


        /// <summary>
        /// Серия паспорта физ лица 
        /// </summary>
        public virtual int? PassportSeries { get; set; }

        /// <summary>
        /// Паспорт выдан физ лицу 
        /// </summary>
        public virtual string PassportIssued { get; set; }
        /// <summary>
        /// ИНН 
        /// </summary>
        public virtual string INN { get; set; }

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
        /// Связь с физическим лицом 
        /// </summary>
        public virtual IndividualPerson IndividualPerson { get; set; }

        /// <summary>
        /// Дата правонарушения
        /// </summary>
        public virtual DateTime? DateViolation { get; set; }

        /// <summary>
		/// Место регистрации физ лица  
		/// </summary>
		public virtual string PlaceResidenceOutState { get; set; }

        /// <summary>
        /// Место фактического жительства физ лица 
        /// </summary>
        public virtual string ActuallyResidenceOutState { get; set; }

        /// <summary>
        /// Место регистрации физ лица  
        /// </summary>
        public virtual bool IsPlaceResidenceOutState { get; set; }

        /// <summary>
        /// Место фактического жительства физ лица 
        /// </summary>
        public virtual bool IsActuallyResidenceOutState { get; set; }

        /// <summary>
        ///  Наложенная сумма
        /// </summary>
        public virtual decimal? ChargeAmount { get; set; }

        /// <summary>
        /// Уплаченная сумма 
        /// </summary>
        public virtual decimal? PaidAmount { get; set; }

        /// <summary>
        /// Взыскан ли штраф 
        /// </summary>
        public virtual YesNoNotSet IsFineCharged { get; set; }

        /// <summary>
        /// Дата взыскания 
        /// </summary>
        public virtual DateTime? FineChargeDate { get; set; }

        /// <summary>
        /// Номер судебного решения 
        /// </summary>
        public virtual string CourtCaseNumber { get; set; }

        /// <summary>
        /// Дата судебного решения
        /// </summary>
        public virtual DateTime? CourtCaseDate { get; set; }

        /// <summary>
        /// Решение суда
        /// </summary>
        public virtual SanctionGji CourtSanction { get; set; }
    }
}