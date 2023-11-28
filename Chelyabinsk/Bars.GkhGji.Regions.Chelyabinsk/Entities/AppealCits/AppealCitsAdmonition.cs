namespace Bars.GkhGji.Regions.Chelyabinsk.Entities
{
    using System;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Обращениям граждан - Предостережение
    /// </summary>
    public class AppealCitsAdmonition : BaseGkhEntity
    {
        /// <summary>
        /// Обращение граждан
        /// </summary>
        public virtual AppealCits AppealCits { get; set; }

        /// <summary>
        /// Контрагент
        /// </summary>
        public virtual Contragent Contragent { get; set; }

        /// <summary>
        /// Документ
        /// </summary>
        public virtual string DocumentName { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string DocumentNumber { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Дата исполнения
        /// </summary>
        public virtual DateTime? PerfomanceDate { get; set; }

        /// <summary>
        /// Дата фактического исполнения
        /// </summary>
        public virtual DateTime? PerfomanceFactDate { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }

        /// <summary>
        /// Файл ответа
        /// </summary>
        public virtual FileInfo AnswerFile { get; set; }

        public virtual Inspector Inspector { get; set; }

        public virtual Inspector Executor { get; set; }
    }
}