namespace Bars.Gkh.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Web;

    using Bars.B4.Utils;
    using Bars.B4.Utils.Annotations;

    /// <summary>
    /// Менеджер кэша в сессии
    /// </summary>
    public class SessionCacheManager
    {
        private static readonly Lazy<SessionCacheManager> instance = new Lazy<SessionCacheManager>(() => new SessionCacheManager());

        private SessionCacheManager()
        {
        }

        /// <summary>
        /// Текущая сессия
        /// </summary>
        public HttpSessionStateBase CurrentSession => HttpContext.Current?.Session != null ? new HttpSessionStateWrapper(HttpContext.Current.Session) : null;

        /// <summary>
        /// Экземпляр <see cref="SessionCacheManager"/>
        /// </summary>
        public static SessionCacheManager Instance => SessionCacheManager.instance.Value;

        /// <summary>
        /// Добавить элемент в кэш
        /// </summary>
        /// <typeparam name="TValue">Тип элемента</typeparam>
        /// <param name="key">Ключ</param>
        /// <param name="value">Значение</param>
        /// <returns>Объект</returns>
        public TValue Add<TValue>(string key, TValue value)
        {
            return this.Add(key, (object)value).To<TValue>();
        }

        /// <summary>
        /// Добавить элемент в кэш
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <param name="value">Значение</param>
        /// <returns>Объект</returns>
        public object Add(string key, object value)
        {
            ArgumentChecker.NotNull(key, nameof(key));
            ArgumentChecker.NotNull(value, nameof(value));

            var session = this.CurrentSession;

            if (session.IsNotNull())
            {
                session[key] = value;
            }
            
            return value;
        }

        /// <summary>
        /// Получить элемент из кэша
        /// </summary>
        /// <typeparam name="TValue">Тип элемента</typeparam>
        /// <param name="key">Ключ</param>
        /// <param name="defaultValue">Значение по умолчанию</param>
        /// <returns>Объект</returns>
        public TValue Get<TValue>(string key, TValue defaultValue = default(TValue))
        {
            return this.Get(key).To<TValue>(defaultValue);
        }

        /// <summary>
        /// Получить элемент из кэша
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <param name="defaultValue">Значение по умолчанию</param>
        /// <returns>Объект</returns>
        public object Get(string key, object defaultValue = null)
        {
            ArgumentChecker.NotNull(key, nameof(key));

            var session = this.CurrentSession;
            return session?[key] ?? defaultValue;
        }

        /// <summary>
        /// Получить элементы из кэша, ключи которых удовлетворяют <paramref name="predicate"/>
        /// </summary>
        /// <param name="predicate">Предикат-условие</param>
        /// <returns>Объекты</returns>
        public IEnumerable<TObject> GetAll<TObject>(Func<string, bool> predicate)
        {
            ArgumentChecker.NotNull(predicate, nameof(predicate));

            var session = this.CurrentSession;

            if (session.IsNull())
            {
                yield break;
            }

            var enumerator = session.Keys.GetEnumerator();

            var keys = new HashSet<string>();
            while (enumerator.MoveNext())
            {
                var current = enumerator.Current.ToString();

                if (predicate(current))
                {
                    keys.Add(current);
                }
            }

            foreach (var key in keys)
            {
                yield return session[key].To<TObject>();
            }
        }

        /// <summary>
        /// Получить элементы из кэша, ключи которых удовлетворяют <paramref name="predicate"/>
        /// </summary>
        /// <param name="predicate">Предикат-условие</param>
        /// <returns>Объекты</returns>
        public IEnumerable<object> GetAll(Func<string, bool> predicate)
        {
            ArgumentChecker.NotNull(predicate, nameof(predicate));

            var session = this.CurrentSession;

            if (session.IsNull())
            {
                yield break;
            }

            var enumerator = session.Keys.GetEnumerator();

            var keys = new HashSet<string>();
            while (enumerator.MoveNext())
            {
                var current = enumerator.Current.ToString();

                if (predicate(current))
                {
                    keys.Add(current);
                }
            }

            foreach (var key in keys)
            {
                yield return session[key];
            }
        }

        /// <summary>
        /// Удалить элемент из кэша
        /// </summary>
        /// <param name="key">Ключ</param>
        public void Delete(string key)
        {
            ArgumentChecker.NotNull(key, nameof(key));

            var session = this.CurrentSession;
            session?.Remove(key);
        }

        /// <summary>
        /// Удалить элементы из кэша
        /// </summary>
        /// <param name="predicate">Условие для ключа</param>
        public void Delete(Func<string, bool> predicate)
        {
            ArgumentChecker.NotNull(predicate, nameof(predicate));

            var session = this.CurrentSession;

            if (session.IsNull())
            {
                return;
            }

            var enumerator = session.Keys.GetEnumerator();

            var keys = new HashSet<string>();
            while (enumerator.MoveNext())
            {
                var current = enumerator.Current.ToString();

                if (predicate(current))
                {
                    keys.Add(current);
                }
            }

            foreach (var key in keys)
            {
                session.Remove(key);
            }
        }
    }
}