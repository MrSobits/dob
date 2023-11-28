namespace Bars.Gkh.FormatDataExport.ProxyEntities
{
    using System;

    using Bars.B4.DataModels;
    using Bars.B4.Modules.FileStorage;

    /// <summary>
    /// Протоколы общего собрания собственников, которыми принято решение о формирования фонда капитального ремонта
    /// </summary>
    public class KapremProtocolossProxy : IHaveId
    {
        /// <summary>
        /// 1. Уникальный идентификатор протокола
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 2. Организация ответственная за передачу информации
        /// </summary>
        public long? ContragentId { get; set; }

        /// <summary>
        /// 3. Адрес дома
        /// </summary>
        public long? RealityObjectId { get; set; }

        /// <summary>
        /// 4. Статус
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 5. Основание принятия решения
        /// </summary>
        public int? SolutionReason { get; set; }

        /// <summary>
        /// 6. Способ формирования фонда
        /// </summary>
        public int? MethodFormFundCr { get; set; }

        /// <summary>
        /// 7. Протокол ОСС
        /// </summary>
        public long? ProtocolossId { get; set; }

        /// <summary>
        /// 8. Номер протокола
        /// </summary>
        public string ProtocolNumber { get; set; }

        /// <summary>
        /// 9. Наименование документа решения
        /// </summary>
        public string SolutionName { get; set; }

        /// <summary>
        /// 10. Вид документа
        /// </summary>
        public string DocumentType { get; set; }

        /// <summary>
        /// 11. Номер документа
        /// </summary>
        public string DocumentNumber { get; set; }

        /// <summary>
        /// 12. Дата протокола
        /// </summary>
        public DateTime? ProtocolDate { get; set; }

        /// <summary>
        /// 13. Дата вступления в силу
        /// </summary>
        public DateTime? DateStart { get; set; }

        /// <summary>
        /// 14. Расчетный счет
        /// </summary>
        public long? RegopSchetId { get; set; }

        /// <summary>
        /// 15. Справка об открытии спец счета
        /// </summary>
        public long? KapremProtocolFilesId { get; set; }

        #region KAPREMPROTOCOLFILES
        /// <summary>
        /// 1. Файл протоколов ОСС на принятие фонда Кап. Ремонта
        /// </summary>
        public FileInfo File { get; set; }

        /// <summary>
        /// 3. Тип файла
        /// </summary>
        public int FileType { get; set; }
        #endregion
    }
}