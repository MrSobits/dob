namespace Bars.Gkh.Extensions
{
    using FastMember;
    using Newtonsoft.Json;

    /// <summary>
    /// Утилитные методы для работы с любым объектом
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Получение значения свойства объекта
        /// </summary>
        /// <param name="source">Объект</param>
        /// <param name="propertyName">Имя свойства</param>
        /// <returns>Значение свойства. Если объект == null, то null</returns>
        public static object GetValue(this object source, string propertyName)
        {
            if (source == null) return null;

            var accessor = TypeAccessor.Create(source.GetType());
            return accessor[source, propertyName];
        }

        /// <summary>
        /// Получить json представление объекта
        /// </summary>
        public static string ToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// Метод проверяет, является ли объект <paramref name="value"/> равным значению по умолчанию для заданного типа
        /// </summary>
        public static bool IsDefault<T>(this T value)
        {
            return value.Equals(default(T));
        }

        /// <summary>
        /// Метод проверяет, является ли объект <paramref name="value"/> не равным значению по умолчанию для заданного типа
        /// </summary>
        public static bool IsNotDefault<T>(this T value)
        {
            return !value.IsDefault();
        }
    }
}