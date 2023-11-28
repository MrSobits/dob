namespace Bars.Gkh.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class LinqExtensions
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector)
        {
            var seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        /// <summary>
        /// Возвращает различающиеся элементы последовательности
        /// <para>
        /// Исключает null значения
        /// </para>
        /// </summary>
        public static IEnumerable<TSource> DistinctValues<TSource>(this IEnumerable<TSource> source)
        {
            var seenKeys = new HashSet<TSource>();
            foreach (TSource element in source.Where(x => x != null))
            {
                if (seenKeys.Add(element))
                {
                    yield return element;
                }
            }
        }
    }
}