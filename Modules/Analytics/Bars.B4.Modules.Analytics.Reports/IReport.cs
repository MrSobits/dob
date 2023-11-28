namespace Bars.B4.Modules.Analytics.Reports
{
    using System.Collections.Generic;
    using System.IO;
    using Bars.B4.Modules.Analytics.Data;
    using Bars.B4.Modules.Analytics.Reports.Enums;

    using Stimulsoft.Report.Export;

    /// <summary>
    /// Интерфейс для отчета
    /// </summary>
    public interface IReport
    {
        /// <summary>
        /// Получить шаблон
        /// </summary>
        /// <returns></returns>
        Stream GetTemplate();

        /// <summary>
        /// Получить данные
        /// </summary>
        /// <returns></returns>
        IEnumerable<IDataSource> GetDataSources();

        /// <summary>
        /// Получить параметры
        /// </summary>
        /// <returns></returns>
        IEnumerable<IParam> GetParams();

        /// <summary>
        /// Метод, позволяющий указать предпочитаемые параметры экспорта.
        /// Если null - используются параметры по умолчанию
        /// </summary>
        /// <param name="format">Формат экспорта</param>
        /// <returns>Параметры экспорта</returns>
        StiExportSettings GetExportSettings(ReportPrintFormat format);
    }
}