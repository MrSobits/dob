namespace Bars.GkhGji.Entities
{
    using System;

    using B4.Modules.States;
    using Gkh.Entities;
    using Enums;

    using Newtonsoft.Json;

    /// <summary>
    /// Базовый документ ГЖИ
    /// </summary>
    public class DocumentGji : BaseGkhEntity, IStatefulEntity
    {
        /// <summary>
        /// Мероприятие комиссии
        /// </summary>
        [JsonIgnore]
        public virtual InspectionGji Inspection { get; set; }

        /// <summary>
        /// Этап проверки
        /// </summary>
        public virtual InspectionGjiStage Stage { get; set; }

        /// <summary>
        /// Тип документа ГЖИ
        /// </summary>
        public virtual TypeDocumentGji TypeDocumentGji { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string DocumentNumber { get; set; }

        /// <summary>
        /// Номер документа (Целая часть)
        /// </summary>
        public virtual int? DocumentNum { get; set; }

        /// <summary>
        /// Дополнительный номер документа (порядковый номер если документов одного типа несколько)
        /// </summary>
        public virtual int? DocumentSubNum { get; set; }

        /// <summary>
        /// Буквенный подномер
        /// </summary>
        public virtual string LiteralNum { get; set; }

        /// <summary>
        /// Год документа
        /// </summary>
        public virtual int? DocumentYear { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }

        /// <summary>
        /// Нехранимое поле Дата строкой, для удобства работы на клиенте
        /// </summary>
        public virtual string DocumentDateStr { get; set; }

        /// <summary>
        /// ГИС ЖКХ GUID
        /// </summary>
        public virtual string GisGkhGuid { get; set; }

        /// <summary>
        /// ГИС ЖКХ Transport GUID
        /// </summary>
        public virtual string GisGkhTransportGuid { get; set; }

        /// <summary>
        /// заседание
        /// </summary>
        public virtual ComissionMeeting ComissionMeeting { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string CaseNumber { get; set; }

        /// <summary>
        /// заседание
        /// </summary>
        public virtual ZonalInspection ZonalInspection { get; set; }
    }
}