namespace Bars.Gkh.Utils.Caching
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.SignalR;

    using Castle.Windsor;

    using Microsoft.AspNet.SignalR;

    using Newtonsoft.Json;

    /// <summary>
    /// Помощник при работе с закэшированным счётчиком запросов
    /// </summary>
    public static class CountCacheHelper
    {
        private const string CountKeyPrefix = "_Count";
        private static readonly Regex[] ignoredData =
        {
            new Regex(@"^(\S*)([&]+?page=\d+)(\S*)+?$",  RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Singleline),
            new Regex(@"^(\S*)([&]+?limit=\d+)(\S*)+?$", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Singleline),
            new Regex(@"^(\S*)([&]+?start=\d+)(\S*)+?$", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Singleline),
            new Regex(@"^(\S*)([&]+?sort=\S+)([&]+?\S*)+?$",  RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Singleline),
        };

        /// <summary>
        /// Метод получает количество записей текущего запроса и кэширует в сессии по FormData запроса.
        /// <para>Не зависит от пагинации или сортировки</para>
        /// </summary>
        /// <typeparam name="T">Тип запроса</typeparam>
        /// <param name="query">Запрос</param>
        /// <param name="cacheKey">Ключ кэша</param>
        /// <returns>Количество элементов, которые будут возвращены запросом</returns>
        public static int GetCountCached<T>(IQueryable<T> query, string cacheKey = null)
        {
            var key = cacheKey;
            var keyCount = cacheKey + CountCacheHelper.CountKeyPrefix;

            var cacheManager = SessionCacheManager.Instance;
            if (cacheManager.CurrentSession.IsNull())
            {
                return query.Count();
            }
            var requestForm = HttpContext.Current.Request.Form.ToStr();

            var currentBaseParams = requestForm;
            CountCacheHelper.ignoredData.ForEach(x => currentBaseParams = x.Replace(currentBaseParams, "$1$3"));

            var lastBaseParams = cacheManager.Get(key, string.Empty);

            var count = cacheManager.Get(keyCount, (int?)null);
            if (string.Compare(currentBaseParams, lastBaseParams, StringComparison.InvariantCulture) != 0 || !count.HasValue || count == 0)
            {
                count = query.Count();

                cacheManager.Add(key, currentBaseParams);
                cacheManager.Add(keyCount, count.Value);

                return count.Value;
            }

            return count.Value;
        }

        /// <summary>
        /// Сделать кэш невалидным для текущего префикса и разослать уведомления всем активным клиентам
        /// </summary>
        /// <param name="key">Префикс ключа</param>
        public static void InvalidateCache(string key)
        {
            CountCacheHelper.ClearCache(key);

            GlobalHost
                .ConnectionManager
                .GetHubContext<CountCacheHub>()
                .Clients.All.clearCache(JsonConvert.SerializeObject(new { key }));
        }

        /// <summary>
        /// Сделать кэш невалидным для текущего префикса
        /// </summary>
        /// <param name="key">Префикс ключа</param>
        public static void ClearCache(string key)
        {
            SessionCacheManager.Instance.Delete(x => x.ToLower().Contains(key.ToLower()));
        }

        /// <summary>
        /// Сделать кэш невалидным для указанного типа сущности
        /// </summary>
        /// <typeparam name="TEntity">Тип сущности</typeparam>
        /// <param name="container">Контейнер</param>
        public static void InvalidateCache<TEntity>(IWindsorContainer container) where TEntity : IEntity
        {
            container.UsingForResolvedAll<ICacheableViewModel<TEntity>>((cnt, services) =>
            {
                services.ForEach(x => x.InvalidateCache());
            });
        }
    }
}