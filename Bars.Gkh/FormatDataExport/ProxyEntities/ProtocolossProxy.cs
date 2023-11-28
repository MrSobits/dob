namespace Bars.Gkh.FormatDataExport.ProxyEntities
{
    using System;

    using Bars.B4.DataModels;
    using Bars.B4.Modules.FileStorage;

    /// <summary>
    /// Прокси Протокол общего собрания собственников
    /// </summary>
    [Obsolete("СА: Не выгружаем", true)]
    public class ProtocolossProxy : IHaveId
    {
        /// <summary>
        /// 1. Уникальный код
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 2. Уникальный идентификатор дома
        /// </summary>
        public long RealityObjectId { get; set; }

        /// <summary>
        /// 3. Владелец протокола
        /// </summary>
        public long? ContragentId { get; set; }

        /// <summary>
        /// 4. Дата составления протокола
        /// </summary>
        public DateTime? DocumentDate { get; set; }

        /// <summary>
        /// 5. Номер протокола
        /// </summary>
        public string DocumentNumber { get; set; }

        /// <summary>
        /// 6. Дата вступления в силу
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 7. Способ формирования фонда КР
        /// </summary>
        public int MethodFormFundCr { get; set; }

        /// <summary>
        /// 8. Форма проведения голосования
        /// </summary>
        public int? VotingForm { get; set; }

        /// <summary>
        /// 9. Дата окончания приема решений
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 10. Место приема решений
        /// </summary>
        public string DecisionPlace { get; set; }

        /// <summary>
        /// 11. Место проведения собрания
        /// </summary>
        public string MeetingPlace { get; set; }

        /// <summary>
        /// 12. Дата и время проведения собрания
        /// </summary>
        public DateTime? MeetingDateTime { get; set; }

        /// <summary>
        /// 13. Дата и время начала проведения голосования
        /// </summary>
        public DateTime? VoteStartDateTime { get; set; }

        /// <summary>
        /// 14. Дата и время окончания проведения голосования
        /// </summary>
        public DateTime? VoteEndDateTime { get; set; }

        /// <summary>
        /// 15. Порядок приема оформленных в письменной форме решений собственников
        /// </summary>
        public string ReceptionProcedure { get; set; }

        /// <summary>
        /// 16. Порядок ознакомления с информацией и материалами, которые будут представлены на данном собрании
        /// </summary>
        public string ReviewProcedure { get; set; }

        /// <summary>
        /// 17. Ежегодное собрание
        /// </summary>
        public int? IsAnnualMeeting { get; set; }

        /// <summary>
        /// 18. Правомочность собрания
        /// </summary>
        public int? IsCompetencyMeeting { get; set; }

        /// <summary>
        /// 19. Статус
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// 20. Основание изменения протокола
        /// </summary>
        public string ChangeReason => "Решение собственников";

        /// <summary>
        /// Вложение
        /// </summary>
        public FileInfo AttachmentFile { get; set; }
    }
}