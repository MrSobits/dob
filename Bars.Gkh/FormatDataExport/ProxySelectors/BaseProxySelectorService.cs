namespace Bars.Gkh.FormatDataExport.ProxySelectors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Threading;

    using Bars.B4.DataModels;
    using Bars.B4.Logging;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.FormatDataExport.Domain;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.FormatDataExport.FormatProvider;
    using Bars.Gkh.Utils;

    using Castle.Windsor;
    
    /// <summary>
    /// Базовый сервис получения прокси-объекта
    /// </summary>
    /// <typeparam name="T">Прокси-сущность</typeparam>
    public abstract class BaseProxySelectorService<T> : IProxySelectorService<T>
        where T : IHaveId
    {
        private IDictionary<long, T> proxyListCache = null;

        protected long[] additionalIds => this.ProxySelectorFactory
            .AdditionalProxyIds
            .GetOrAdd(typeof(T), new HashSet<long>())
            .ToArray();

        private readonly object locker = new object();

        /// <inheritdoc />
        public DynamicDictionary SelectParams { get; } = new DynamicDictionary();

        /// <inheritdoc />
        public IDictionary<long, T> ProxyListCache
        {
            get
            {
                if (this.proxyListCache != null)
                {
                    return this.proxyListCache;
                }
                lock (this.locker)
                {
                    return this.proxyListCache ?? this.GetProxyList();
                }
            }
        }

        /// <inheritdoc />
        public ICollection<T> ExtProxyListCache => this.ProxyListCache.Values
            .Union(this.GetAdditionalCache(), this.Comparer).ToList();

        /// <inheritdoc />
        public IDictionary<long, T> GetProxyList()
        {
            this.LogManager.Debug($"Инициализация кэша для {typeof(T).Name}");
            if (this.ProxySelectorFactory == null)
            {
                this.ProxySelectorFactory = this.SelectParams.GetAs<IProxySelectorFactory>("ProxySelectorFactory");
            }

            if (this.FilterService == null)
            {
                this.FilterService = this.SelectParams.GetAs<IFormatDataExportFilterService>("FormatDataExportFilterService");
            }

            Interlocked.Exchange(ref this.proxyListCache, this.GetCache());

            this.AddAdditionalIds();
            return this.proxyListCache;
        }
        /// <inheritdoc />
        public void Clear()
        {
            this.proxyListCache?.Clear();
            this.proxyListCache = null;
        }

        private void AddAdditionalIds()
        {
            var type = typeof(T);
            type.GetProperties()
                .Where(x => x.CustomAttributes.Any(z => z.AttributeType == typeof(ProxyIdAttribute)))
                .ForEach(x =>
                {
                    var attribute = (ProxyIdAttribute) Attribute.GetCustomAttribute(x, typeof(ProxyIdAttribute));
                    var proxyTypeIds = this.ProxySelectorFactory.AdditionalProxyIds
                        .GetOrAdd(attribute.ProxyType, new HashSet<long>());

                    this.LogManager.Debug($"Инициализация дополнительных данных для {attribute.ProxyType.Name} инициатор {type.Name}");

                    foreach (var proxy in this.proxyListCache.Values)
                    {
                        var id = x.GetValue(proxy) as long?;
                        if (id.HasValue)
                        {
                            proxyTypeIds.Add(id.Value);
                        }
                    }
                });
        }

        /// <summary>
        /// Получить кэш объекта
        /// </summary>
        protected abstract IDictionary<long, T> GetCache();

        /// <summary>
        /// Получить дополнительные прокси-сущности
        /// </summary>
        protected virtual ICollection<T> GetAdditionalCache()
        {
            return new T[0];
        }

        /// <summary>
        /// Компаратор сущностей по идентификатору
        /// </summary>
        protected readonly IEqualityComparer<T> Comparer = EntityEqComparer.ById<T>();

        /// <summary>
        /// IoC
        /// </summary>
        public IWindsorContainer Container { get; set; }

        public ILogManager LogManager { get; set; }

        /// <summary>
        /// Конвертер данных приведения к формату экспорта
        /// </summary>
        public IExportFormatConverter Converter { get; set; }

        /// <inheritdoc />
        public IProxySelectorFactory ProxySelectorFactory { get; private set; }

        /// <inheritdoc />
        public IFormatDataExportFilterService FilterService { get; private set; }

        /// <inheritdoc />
        public void Dispose()
        {
            this.Clear();
        }

        protected Operator Operator { get; set; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected long? GetId(IHaveId entity)
        {
            // ReSharper disable once MergeConditionalExpression
            return entity != null ? entity.Id : (long?) null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected long? GetId(IHaveExportId entity)
        {
            // ReSharper disable once MergeConditionalExpression
            return entity != null ? entity.ExportId : (long?) null;
        }

        protected Func<IHaveId, long> Id = entity => entity is IHaveExportId
            ? ((IHaveExportId)entity).ExportId
            : entity.Id;
    }
}