namespace Bars.Gkh.FormatDataExport.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Linq.Expressions;

    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.FormatDataExport.Enums;

    /// <summary>
    /// Сервис фильтрации данных по контрагентам и жилым домам
    /// </summary>
    public interface IFormatDataExportFilterService
    {
        /// <summary>
        /// Коллекция идентификаторов контрагентов
        /// </summary>
        ReadOnlyCollection<long> ContragentIds { get; }

        /// <summary>
        /// Коллекция идентификаторов жилых домов
        /// </summary>
        ReadOnlyCollection<long> RealityObjectIds { get; }

        /// <summary>
        /// Инициализировать сервис идентификаторами контрагентов
        /// </summary>
        /// <param name="provider">Поставщик информации</param>
        /// <param name="filterParams">Параметры фильтрации</param>
        /// <param name="bulkSize">Количество идентификаторов в запросе</param>
        void Init(FormatDataExportProviderType provider, DynamicDictionary filterParams, int bulkSize = 5000);

        /// <summary>
        /// Фильтрация запроса по выбранным в настройках контрагентам
        /// </summary>
        IQueryable<Contragent> FilterByContragent(IQueryable<Contragent> query);

        /// <summary>
        /// Фильтрация запроса по выбранным в настройках контрагентам
        /// </summary>
        IQueryable<TEntity> FilterByContragent<TEntity>(IQueryable<TEntity> query, Expression<Func<TEntity, Contragent>> contragentSelector);

        /// <summary>
        /// Фильтрация запроса по выбранным в настройках контрагентам
        /// </summary>
        IEnumerable<TEntity> FilterByContragent<TEntity>(IEnumerable<TEntity> data, Expression<Func<TEntity, Contragent>> contragentSelector);

        /// <summary>
        /// Фильтрация запроса по жилым домам, обслуживаемых выбранными в настройках контрагентами
        /// </summary>
        IQueryable<RealityObject> FilterByRealityObject(IQueryable<RealityObject> query);

        /// <summary>
        /// Фильтрация запроса по жилым домам, обслуживаемых выбранными в настройках контрагентами
        /// </summary>
        IQueryable<TEntity> FilterByRealityObject<TEntity>(IQueryable<TEntity> query, Expression<Func<TEntity, RealityObject>> realityObjectSelector);

        /// <summary>
        /// Фильтрация запроса по жилым домам, обслуживаемых выбранными в настройках контрагентами
        /// </summary>
        IEnumerable<TEntity> FilterByRealityObject<TEntity>(IEnumerable<TEntity> data, Expression<Func<TEntity, RealityObject>> realityObjectSelector);
    }
}