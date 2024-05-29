namespace Bars.GkhGji.Entities
{
    using System;
    using System.Collections.Generic;

    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.GkhGji.Enums;

    /// <summary>
    /// Постановление
    /// </summary>
    public class Resolution : DocumentGji
    {
        /// <summary>
        /// тип исполнителя документа
        /// </summary>
        public virtual ExecutantDocGji Executant { get; set; }

        /// <summary>
        /// Муниципальное образование
        /// </summary>
        public virtual Municipality Municipality { get; set; }

        /// <summary>
        /// Контрагент
        /// </summary>
        public virtual Contragent Contragent { get; set; }

        /// <summary>
        /// Вид санкции
        /// </summary>
        public virtual SanctionGji Sanction { get; set; }

        /// <summary>
        /// Должностное лицо
        /// </summary>
        public virtual Inspector Official { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        public virtual string Surname { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public virtual string FirstName { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        public virtual string Patronymic { get; set; }

        /// <summary>
        /// Должность
        /// </summary>
        public virtual string Position { get; set; }

        /// <summary>
        /// Физическое лицо
        /// </summary>
        public virtual string PhysicalPerson { get; set; }

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
        /// Дата вручения
        /// </summary>
        public virtual DateTime? DeliveryDate { get; set; }

        /// <summary>
        /// Дата вручения почтой
        /// </summary>
        public virtual DateTime? PostDeliveryDate { get; set; }


        /// <summary>
        /// Тип инициативного органа
        /// </summary>
        public virtual TypeInitiativeOrgGji TypeInitiativeOrg { get; set; }

        /// <summary>
        /// Номер участка
        /// </summary>
        public virtual string SectorNumber { get; set; }

        /// <summary>
        /// Сумма штрафов
        /// </summary>
        public virtual Decimal? PenaltyAmount { get; set; }

        /// <summary>
        /// Сумма штрафов по суду
        /// </summary>
        public virtual Decimal? PenaltyAmountByCourt { get; set; }

        /// <summary>
        /// Штраф оплачен
        /// </summary>
        public virtual YesNoNotSet Paided { get; set; }

        /// <summary>
        /// Штраф оплачен (подробно)
        /// </summary>
        public virtual ResolutionPaymentStatus PayStatus { get; set; }

        /// <summary>
        /// Дата передачи в ССП
        /// </summary>
        public virtual DateTime? DateTransferSsp { get; set; }

        /// <summary>
        /// Номер документа, передача в ССП
        /// </summary>
        public virtual string DocumentNumSsp { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Время составления документа
        /// </summary>
        public virtual DateTime? DocumentTime { get; set; }

        /// <summary>
        /// Выписка из ЕГРЮЛ
        /// </summary>
        public virtual DateTime? DateWriteOut { get; set; }

        /// <summary>
        /// Вступило в законную силу
        /// </summary>
        public virtual bool BecameLegal { get; set; }

        /// <summary>
        /// Код Гис - униклаьный идентификатор начисления 
        /// </summary>
        public virtual string GisUin { get; set; }

        /// <summary>
        /// МО получателя штрафа
        /// </summary>
        public virtual Municipality FineMunicipality { get; set; }

        //ToDo ГЖИ после перехода направила, выпилить поля которые нехрранимые 

        /// <summary>
        /// Список родительских документов (Используется при создании объекта)
        /// </summary>
        public virtual List<long> ParentDocumentsList { get; set; }

        /// <summary>
        /// Основание прекращения
        /// </summary>
        public virtual TypeTerminationBasement? TypeTerminationBasement { get; set; }

        /// <summary>
        /// Основание аннулирования
        /// </summary>
        //Используется в интеграции с ГИС ГМП (пока только татарстан)
        public virtual string AbandonReason { get; set; }

        /// <summary>
        /// Нарушитель явился на рассмотрение
        /// </summary>
        public virtual YesNoNotSet OffenderWas { get; set; }

        /// <summary>
        /// Номер постановления
        /// </summary>
        public virtual long? RulingNumber { get; set; }

        /// <summary>
        /// Фио в постановлении
        /// </summary>
        public virtual string RulinFio { get; set; }

        /// <summary>
        /// Дата постановления
        /// </summary>
        public virtual DateTime? RulingDate { get; set; }

        /// <summary>
        /// Почтовый идентификатор
        /// </summary>
        public virtual string PostGUID { get; set; }

        /// <summary>
        /// Отдел судебных приставов
        /// </summary>
        public virtual JurInstitution OSP { get; set; }

        /// <summary>
        /// Дата вручения исполнительного документа
        /// </summary>
        public virtual DateTime? DateOSPListArrive { get; set; }

        /// <summary>
        /// Дата исполнительного производства
        /// </summary>
        public virtual DateTime? DateExecuteSSP { get; set; }

        /// <summary>
        /// Дата окончания исполнительного производства
        /// </summary>
        public virtual DateTime? DateEndExecuteSSP { get; set; }

        /// <summary>
        /// Комментарий
        /// </summary>
        public virtual string Comment { get; set; }

        /// <summary>
        /// Номер исполнительного дела
        /// </summary>
        public virtual string ExecuteSSPNumber { get; set; }

        /// <summary>
        /// Решение ОСП
        /// </summary>
        public virtual OSPDecisionType OSPDecisionType { get; set; }

        //решения судебного участка

        /// <summary>
        /// Номер решения
        /// </summary>
        public virtual string DecisionNumber { get; set; }

        /// <summary>
        /// Дата решения
        /// </summary>
        public virtual DateTime? DecisionDate { get; set; }

        /// <summary>
        /// Дата вступления в законную силу
        /// </summary>
        public virtual DateTime? DecisionEntryDate { get; set; }

        /// <summary>
        /// Нарушение
        /// </summary>
        public virtual string Violation { get; set; }

        /// <summary>
        /// Судебный участок
        /// </summary>
        public virtual JurInstitution JudicalOffice { get; set; }

        /// <summary>
        /// Дата вступления в законную силу
        /// </summary>
        public virtual DateTime? InLawDate { get; set; }

        /// <summary>
        /// Оплатить до
        /// </summary>
        public virtual DateTime? DueDate { get; set; }

        /// <summary>
        /// Дата оплаты
        /// </summary>
        public virtual DateTime? PaymentDate { get; set; }

        /// <summary>
        /// Дата неоплаты
        /// </summary>
        public virtual DateTime? Protocol205Date { get; set; }

        /// <summary>
        /// Результат рассмотрения
        /// </summary>
        public virtual ConcederationResult ConcederationResult { get; set; } 

        /// <summary>
        /// Нарушитель
        /// </summary>
        public virtual IndividualPerson IndividualPerson { get; set; }

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
        /// Дата выдачи  паспорта физ лицу   
        /// </summary>
        public virtual DateTime? SendDate { get; set; }

        /// <summary>
        /// Направлено приставам
        /// </summary>
        public virtual bool SentToOSP { get; set; }

        /// <summary>
        /// Постановление отменено
        /// </summary>
        public virtual bool DischargedByCourt { get; set; }

        /// <summary>
        /// Направлено новое рассмотрение
        /// </summary>
        public virtual bool SentToNewConcederation { get; set; }

        /// <summary>
        /// Изменено судом
        /// </summary>
        public virtual bool ChangedByCourt { get; set; }
    }
}