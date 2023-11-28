namespace Bars.B4.Modules.Analytics.Reports.Web.DomainService
{
    using Stimulsoft.Report;

    /// <summary>
    /// Интерфейс для генерации стимул отчетов
    /// </summary>
    public interface IStimulService
    {
        /// <summary>
        /// Сохранение шаблона
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        void SaveTemplate(BaseParams baseParams);

        /// <summary>
        /// Получить или создать шаблон
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <param name="isNew"></param>
        /// <returns>Шаблон</returns>
        StiReport GetOrCreateTemplate(BaseParams baseParams, bool isNew);
    }
}
