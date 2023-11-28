namespace Bars.Gkh.Decisions.Nso.Entities
{
    using B4.Modules.FileStorage;
    using B4.Modules.States;
    using Bars.Gkh.Decisions.Nso.Domain;
    using Bars.Gkh.Enums;
    using Gkh.Entities;
    using System;

    using Bars.Gkh.FormatDataExport.ExportableEntities;

    /// <summary>
    /// Протокол решений собственников
    /// </summary>
    public class RealityObjectDecisionProtocol : BaseImportableEntity, IStatefulEntity, IDecisionProtocol, IHaveExportId
    {
        /// <inheritdoc />
        public virtual long ExportId { get; set; }

        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Файл протокола
        /// </summary>
        public virtual FileInfo File { get; set; }

        /// <summary>
        /// Наименование документа
        /// </summary>
        public virtual string DocumentName { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string DocumentNum { get; set; }

        /// <summary>
        /// Дата вступления в силу
        /// </summary>
        public virtual DateTime DateStart { get; set; }

        /// <summary>
        /// Дата протокола
        /// </summary>
        public virtual DateTime ProtocolDate { get; set; }

        public virtual string Description { get; set; }

        public virtual decimal VotesTotalCount { get; set; }

        public virtual decimal VotesParticipatedCount { get; set; }

        public virtual decimal ParticipatedShare { get; set; }

        public virtual YesNo HasQuorum { get; set; }

        public virtual decimal PositiveVotesCount { get; set; }

        public virtual decimal DecidedShare { get; set; }

        /// <summary>
        /// Уполномоченное лицо
        /// </summary>
        public virtual string AuthorizedPerson { get; set; }

        /// <summary>
        /// Телефон уполномоченного лица
        /// </summary>
        public virtual string PhoneAuthorizedPerson { get; set; }

        [Obsolete("Не хранимое")]
        public virtual string ManOrgName { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }

        /// <summary>
        /// Номер входящего письма
        /// </summary>
        public virtual string LetterNumber { get; set; }

        /// <summary>
        /// Дата входящего письма
        /// </summary>
        public virtual DateTime? LetterDate { get; set; }

        /// <summary>
        /// ГИС ЖКХ GUID
        /// </summary>
        public virtual string GisGkhGuid { get; set; }

        /// <summary>
        /// ГИС ЖКХ Transport GUID
        /// </summary>
        public virtual string GisGkhTransportGuid { get; set; }

        /// <summary>
        /// ГИС ЖКХ GUID вложения
        /// </summary>
        public virtual string GisGkhAttachmentGuid { get; set; }
    }
}