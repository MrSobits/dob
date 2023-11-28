namespace Bars.GkhGji.DomainService
{
    using System.IO;

    using Bars.B4.Modules.Analytics.Reports;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.Gkh.Modules.ClaimWork.Enums;
    using Bars.Gkh.Report;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Расширение ICodedReport новыми полями
    /// </summary>
    public interface IComissionMeetingCodedReport : IGkhBaseReport
    {
        /// <summary>
        /// Идентификатор отчета
        /// </summary>
        string ReportId { get; }

        /// <summary>
        /// Идентификатор отчета
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Идентификатор отчета
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Идентификатор документа для печати
        /// </summary>
        string DocId { get; set; }

        /// <summary>
        /// Код формы, на которой находится кнопка печати
        /// </summary>
        string CodeForm { get; }

        /// <summary>
        /// Название выходного файла
        /// </summary>
        string OutputFileName { get; set; }


        /// <summary>
        /// Информация
        /// </summary>
        ComissionMeetingReportInfo ReportInfo { get; set; }


        Stream ReportFileStream { get; set; }

        /// <summary>
        /// Генерация документа для выгрузки
        /// </summary>
        void GenerateMassReport();
    }
}