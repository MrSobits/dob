namespace Bars.GkhGji.Entities
{
    using System;
    using System.Collections.Generic;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Enums;

    /// <summary>
    /// Рапоряжение ГЖИ
    /// </summary>
    public class Disposal : DocumentGji
    {
        /// <summary>
        /// Тип нарушителя
        /// </summary>
        public virtual TypeViolator TypeViolator { get; set; }


        /// <summary>
        /// Связь с контрагентами 
        /// </summary>
        public virtual Contragent Contragent { get; set; }


        /// <summary>
        /// Связь с должностным лицом 
        /// </summary>
        public virtual ContragentContact ContragentContact { get; set; }


        /// <summary>
        /// Связь с физическим лицом 
        /// </summary>
        public virtual IndividualPerson IndividualPerson { get; set; }


        /// <summary>
        /// Тип распоряжения
        /// </summary>
        public virtual TypeDisposalGji TypeDisposal { get; set; }

        /// <summary>
        /// Дата начала обследования
        /// </summary>
        public virtual DateTime? DateStart { get; set; }

        /// <summary>
        /// Дата окончания обследования
        /// </summary>
        public virtual DateTime? DateEnd { get; set; }

        /// <summary>
        /// Согласование с прокуротурой
        /// </summary>
        public virtual TypeAgreementProsecutor TypeAgreementProsecutor { get; set; }

        /// <summary>
        /// Номер документа с результатом согласования
        /// </summary>
        public virtual string DocumentNumberWithResultAgreement { get; set; }

        /// <summary>
        /// Результат согласования
        /// </summary>
        public virtual TypeAgreementResult TypeAgreementResult { get; set; }

        /// <summary>
        /// Дата документа с результатом согласования
        /// </summary>
        public virtual DateTime? DocumentDateWithResultAgreement { get; set; }

        /// <summary>
        /// Должностное лицо (ДЛ) вынесшее распоряжение
        /// </summary>
        public virtual Inspector IssuedDisposal { get; set; }

        /// <summary>
        /// Ответственный за исполнение
        /// </summary>
        public virtual Inspector ResponsibleExecution { get; set; }

        /// <summary>
        /// Вид проверки
        /// </summary>
        public virtual KindCheckGji KindCheck { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Выезд на объект с
        /// </summary>
        public virtual DateTime? ObjectVisitStart { get; set; }

        /// <summary>
        /// Выезд на объект по
        /// </summary>
        public virtual DateTime? ObjectVisitEnd { get; set; }

        /// <summary>
        /// Выезд инспектора в командировку
        /// </summary>
        public virtual bool OutInspector { get; set; }

        /// <summary>
        /// Время начала визита (Время с)
        /// </summary>
        public virtual DateTime? TimeVisitStart { get; set; }

        /// <summary>
        /// Время окончания визита (Время по)
        /// </summary>
        public virtual DateTime? TimeVisitEnd { get; set; }

        /// <summary>
        /// Номер документа (Уведомление о проверке)
        /// </summary>
        public virtual string NcNum { get; set; }

        /// <summary>
        /// Дата документа (Уведомление о проверке)
        /// </summary>
        public virtual DateTime? NcDate { get; set; }

        /// <summary>
        /// Номер исходящего письма  (Уведомление о проверке)
        /// </summary>
        public virtual string NcNumLatter { get; set; }

        /// <summary>
        /// Дата исходящего пиьма  (Уведомление о проверке)
        /// </summary>
        public virtual DateTime? NcDateLatter { get; set; }

        /// <summary>
        /// Уведомление получено (Уведомление о проверке)
        /// </summary>
        public virtual YesNo NcObtained { get; set; }

        /// <summary>
        /// Уведомление отправлено (Уведомление о проверке)
        /// </summary>
        public virtual YesNo NcSent { get; set; }

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
        //ToDo ГЖИ следующие поля необходимо выпилить поля после перехода на правила


        /// <summary>
        /// Наименование транспортного средства  
        /// </summary>
        public virtual string NameTransport { get; set; }

        /// <summary>
        ///  Номер транспортного средства  
        /// </summary>
        public virtual string NamberTransport { get; set; }

        /// <summary>
        /// Регистрционный номер транспортного средства  
        /// </summary>
        public virtual int? RegistrationNamberTransport { get; set; }

        /// <summary>
        /// Серия номера транспортного средства 
        /// </summary>
        public virtual string SeriesTransport { get; set; }

        /// <summary>
        /// Регион регистрации номера транспортного средства 
        /// </summary>
        public virtual int? RegNamberTransport { get; set; }

        /// <summary>
        /// Семейное положение 
        /// </summary>
        public virtual FamilyStatus FamilyStatus { get; set; }


        /// <summary>
        /// Дата явки   
        /// </summary>
       public virtual DateTime? NoticeDate { get; set; }

        /// <summary>
        /// Время явки
        /// </summary>
       public virtual DateTime? NoticeTime { get; set; }

        /// <summary>
        /// Адрес комиссии
        /// </summary>
       public virtual string AddresСom { get; set; }

        /// <summary>
        /// Адрес оправления
        /// </summary>
        public virtual string AddresDepartures { get; set; }


        /// <summary>
        /// Почтовый индекс 
        /// </summary>
        public virtual string Postcode { get; set; }


        /// <summary>
        /// КУСП 
        /// </summary>
        public virtual long NumberKUSP { get; set; }

        /// <summary>
        /// Привлечение нарушителя
        /// </summary>
        public virtual bool ToAttracted { get; set; }

        /// <summary>
        ///     Муниципальное образование 
        /// </summary>
        public virtual Municipality Municipality { get; set; }

        #region Мусор
        /// <summary>
        /// Не хранимое поле. идентификатор Акта проверки общего. Для того чтобы несколько раз нельзя было делать Общий акт
        /// </summary>
        public virtual long? ActCheckGeneralId { get; set; }

        /// <summary>
        /// Список родительских документов (Используется при создании объекта)
        /// </summary>
        public virtual List<long> ParentDocumentsList { get; set; }

        /// <summary>
        /// Не хранимое
        /// </summary>
        public virtual TypeBase TypeBase { get; set; }

        /// <summary>
        /// Не хранимое
        /// </summary>
        public virtual long InspectionId { get; set; }

        /// <summary>
        /// Не хранимое
        /// </summary>
        public virtual bool HasChildrenActCheck { get; set; }

        /// <summary>
        /// Вид контроля(надзора)
        /// </summary>
        public virtual KindKNDGJI KindKNDGJI { get; set; }

        /// <summary>
        /// Номер в ЕРП
        /// </summary>
        public virtual string ERPID { get; set; }

        /// <summary>
        /// Номер решения о согласовании
        /// </summary>
        public virtual string ProcAprooveNum { get; set; }

        /// <summary>
        /// Дата решения о согласовании
        /// </summary>
        public virtual DateTime? ProcAprooveDate { get; set; }

        /// <summary>
        /// Файл решения о согласовании
        /// </summary>
        public virtual FileInfo ProcAprooveFile { get; set; }

        /// <summary>
        /// Файл решения о согласовании
        /// </summary>
        public virtual string FioProcAproove { get; set; }

        /// <summary>
        /// Файл решения о согласовании
        /// </summary>
        public virtual string PositionProcAproove { get; set; }
        #endregion
    }
}