namespace Bars.GkhGji.Entities
{
    using System;

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
        /// Физическое лицо
        /// </summary>
        public virtual string PhysicalPersonPosition { get; set; }

        /// <summary>
        /// Реквизиты физ. лица
        /// </summary>
        public virtual string PhysicalPersonInfo { get; set; }

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