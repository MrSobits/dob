namespace Bars.Gkh.FormatDataExport.ProxySelectors
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    using Bars.B4.DataModels;
    using Bars.B4.Utils;

    using Castle.Windsor;

    public class ProxySelectorFactory : IProxySelectorFactory
    {
        private bool disposed = false;
        private readonly object locker = new object();
        private readonly Dictionary<Type, object> proxySelectorCache = new Dictionary<Type, object>();
        private DynamicDictionary defaultSelectorParams = new DynamicDictionary();

        /// <inheritdoc />
        public IWindsorContainer Container { get; set; }

        /// <inheritdoc />
        public ConcurrentDictionary<Type, HashSet<long>> AdditionalProxyIds { get; } = new ConcurrentDictionary<Type, HashSet<long>>();

        /// <inheritdoc />
        public IProxySelectorService<T> GetSelector<T>() where T : class, IHaveId
        {
            lock (this.locker)
            {
                var proxyType = typeof(T);
                object selector;

                if (this.proxySelectorCache.TryGetValue(proxyType, out selector))
                {
                    return selector as IProxySelectorService<T>;
                }
                else
                {
                    var selectorService = this.Container.Resolve<IProxySelectorService<T>>();
                    if (selectorService == null)
                    {
                        throw new TypeLoadException($"Не удалось получить реализацию сервиса IProxySelectorService<{nameof(T)}>");
                    }
                    selectorService.SelectParams.ApplyIf(this.defaultSelectorParams);
                    this.proxySelectorCache.Add(proxyType, selectorService);
                    return selectorService;
                }
            }
        }

        /// <inheritdoc />
        public IProxySelectorService<T> GetSelector<T>(DynamicDictionary selectorParams, bool clear) where T : class, IHaveId
        {
            var selectorService = this.GetSelector<T>();
            selectorService.SelectParams.ApplyIf(selectorParams);
            if (clear)
            {
                selectorService.Clear();
            }

            return selectorService;
        }

        /// <inheritdoc />
        public void DisposeSelector<T>() where T : class, IHaveId
        {
            var proxyType = typeof(T);
            lock (this.locker)
            {
                if (this.proxySelectorCache.ContainsKey(proxyType))
                {
                    var selectorService = this.proxySelectorCache[proxyType];
                    this.proxySelectorCache.Remove(proxyType);
                    this.Container.Release(selectorService);
                }
            }
        }

        /// <inheritdoc />
        public void SetDefaultSelectorParams(DynamicDictionary selectorParams)
        {
            if (selectorParams != null)
            {
                this.defaultSelectorParams = selectorParams;
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (this.disposed)
            {
                return;
            }

            foreach (var proxySelectorService in this.proxySelectorCache)
            {
                this.Container.Release(proxySelectorService);
            }

            this.proxySelectorCache.Clear();

            this.disposed = true;
        }
    }
}