namespace Bars.GkhGji.DomainService
{
    using System.Collections.Generic;

    using B4;
    using Bars.GkhGji.Controllers;

    /// <summary>
    /// Сервис для работы с претензиями неплательщиков
    /// </summary>
    public interface IComissionMeetingReportService
    {
        /// <summary>
        /// Получить список сущностей для которых реализован IClaimWorkCodedReport + отчет по лс
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        IList<ReportInfo> GetReportList(BaseParams baseParams);

        /// <summary>
        /// Получить список сущностей для которых реализован IClaimWorkCodedReport + отчет по лс
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        IList<ReportInfo> GetResolutionReportList(BaseParams baseParams);

        /// <summary>
        /// Получить список сущностей для которых реализован IClaimWorkCodedReport + отчет по лс
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        IList<ReportInfo> GetProtocol2025ReportList(BaseParams baseParams);

        /// <summary>
        /// Получить список сущностей для которых реализован IClaimWorkCodedReport + отчет по лс
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        IList<ReportInfo> GetResolutionDefinitionReportList(BaseParams baseParams);

        /// <summary>
        /// Создание и вывод одной печатной формы
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        IDataResult GetReport(BaseParams baseParams);

        /// <summary>
        /// Массовое создание печатных форм и сохранение на ftp
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        IDataResult GetMassReport(BaseParams baseParams);

        /// <summary>
        /// Создание и вывод одной печатной формы
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        IDataResult GetLawsuitOnwerReport(BaseParams baseParams);
    }
}