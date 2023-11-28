namespace Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Views
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Enums;

    /*
     * Вьюха предназначена для реестра протоколов 19.7
     * с количественными и агрегированными показателями:
     * количество нарушений по которым создан протокол 19.7
     * фио инспекторов
     * строка идентификаторов жилых домов вида /1/2/4 для фильтрации в реестре документов
     */
	public class ViewProtocol197 : PersistentObject
    {
        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }

        /// <summary>
        /// Протокол 19.7
        /// </summary>
        public virtual long? Protocol197Id { get; set; }

        /// <summary>
        /// Количество нарушений
        /// </summary>
        public virtual int? CountViolation { get; set; }

        /// <summary>
        /// Инспекторы
        /// </summary>
        public virtual string InspectorNames { get; set; }

        /// <summary>
        /// строка идентификаторов жилых домов вида /1/2/4/ 
        /// </summary>
        public virtual string RealityObjectIds { get; set; }

        /// <summary>
        /// Наименования муниципальных образований жилых домов
        /// </summary>
        public virtual string MunicipalityNames { get; set; }

        /// <summary>
        /// Наименования муниципальных образований жилых домов
        /// </summary>
        public virtual string MoNames { get; set; }

        /// <summary>
        /// Наименования населенных пунктов жилых домов
        /// </summary>
        public virtual string PlaceNames { get; set; }

        /// <summary>
        /// Муниципальное образование первого жилого дома
        /// </summary>
        public virtual long? MunicipalityId { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime? DocumentDate { get; set; }

        /// <summary>
        /// номер документа
        /// </summary>
        public virtual string DocumentNumber { get; set; }

        /// <summary>
        /// Целая часть номер документа
        /// </summary>
        public virtual int? DocumentNum { get; set; }

        /// <summary>
        /// Контрагент МО Id 
        /// </summary>
        public virtual long? ContragentMuId { get; set; }

        /// <summary>
        /// Контрагент МО Name
        /// </summary>
        public virtual string ContragentMuName { get; set; }

        /// <summary>
        /// Контрагент (исполнитель)
        /// </summary>
        public virtual string ContragentName { get; set; }

        /// <summary>
        /// ФИО физлица
        /// </summary>
        public virtual string PhysicalPerson { get; set; }

        /// <summary>
        /// Тип исполнителя
        /// </summary>
        public virtual string TypeExecutant { get; set; }

        /// <summary>
        /// Идентификатор основания проверки
        /// </summary>
        public virtual long? InspectionId { get; set; }

        /// <summary>
        /// Тип основания проверки
        /// </summary>
        public virtual TypeBase TypeBase { get; set; }

        /// <summary>
        /// Тип документа ГЖИ
        /// </summary>
        public virtual TypeDocumentGji TypeDocumentGji { get; set; }

        /// <summary>
        /// Статьи закона
        /// </summary>
        public virtual string ArticleLaw { get; set; }

        /// <summary>
        /// Вид контроля
        /// </summary>
        public virtual ControlType ControlType { get; set; }

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
        /// Тип нарушителя
        /// </summary>
        public virtual TypeViolator TypeViolator { get; set; }

        /// <summary>
        /// ZonalInspection
        /// </summary>
        public virtual long ZonalInspectionId { get; set; }

        /// <summary>
        /// Сумма штрафа
        /// </summary>
        public virtual decimal? Penalty { get; set; }

        /// <summary>
        /// Дата нарушения
        /// </summary>
        public virtual DateTime? DateViolation { get; set; }

        /// <summary>
        /// Должность инспектора
        /// </summary>
        public virtual string InspectorPosition { get; set; }

        /// <summary>
        /// Создано постановление
        /// </summary>
        public virtual bool HasResolution { get; set; }

        /// <summary>
		/// Контактный телефон
		/// </summary>
		public virtual string PhoneNumber { get; set; }
    }
}