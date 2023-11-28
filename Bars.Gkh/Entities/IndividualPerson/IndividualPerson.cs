namespace Bars.Gkh.Entities
{
    using System;
   
    using Bars.GkhGji.Enums;
    using Bars.GkhGji;
    using Bars.Gkh.Enums;
    using Bars.B4.Modules.FIAS;

    /// <summary>
    /// Физическое лицо 
    /// </summary>
    public class IndividualPerson : BaseGkhEntity
    {
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
        public virtual DateTime?  DateBirth { get; set; }

        /// <summary>
        /// Номер паспорта физ лица 
        /// </summary>
        public virtual string PassportNumber { get; set; }


        /// <summary>
        /// Серия паспорта физ лица 
        /// </summary>
        public virtual string PassportSeries { get; set; }

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
        public virtual string DepartmentCode { get; set; }

        /// <summary>
        /// Дата выдачи  паспорта физ лицу   
        /// </summary>
        public virtual DateTime? DateIssue { get; set; }

        /// <summary>
        /// Семейное положение 
        /// </summary>
        public virtual FamilyStatus FamilyStatus { get; set; }

        /// <summary>
        /// Семейное положение 
        /// </summary>
        public virtual string DateBirthTxt { get; set; }

        /// <summary>
        /// Протокол - реквизиты - Тип адреса - Выбор из ФИАС
        /// </summary>
        public virtual FiasAddress FiasRegistrationAddress { get; set; }

        /// <summary>
        /// Протокол - реквизиты - Тип адреса - Выбор из ФИАС
        /// </summary>
        public virtual FiasAddress FiasFactAddress { get; set; }

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
        /// Контактный телефон физ лица
        /// </summary>
        public virtual string PhoneNumber { get; set; }
    }
}
