using GisGkhLibrary.Entities;
using GisGkhLibrary.Entities.Dictionaries;
using GisGkhLibrary.Enums;
using GisGkhLibrary.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GisGkhLibrary.Managers
{
    /// <summary>
    /// Человекоюзабельная обертка для методов словарей
    /// </summary>
    public static class DictionaryManager
    {
        /// <summary>
        /// Получить список всех словарей
        /// </summary>
        /// <param name="version">Версия из <Раздел 6. Перечень версий> справочники ГИС ЖКХ</param>
        /// <returns>Список <see cref = DictionaryInfo></returns>
        public static IEnumerable<DictionaryInfo> GetAllDictionaries(string version)
        {
            var list = NsiServiceCommon.exportNsiList(version);

            return list.Select(x => new DictionaryInfo
            {
                RegistryNumber = int.Parse(x.RegistryNumber),
                Name = x.Name,
                Modified = x.Modified
            });
        }

        /// <summary>
        /// Получить словарь
        /// </summary>
        public static IEnumerable<T> GetDictionary<T>(DateTime? ModifiedAfter = null) where T: DictionaryBase
        {
            var dictionaryName = typeof(T).Name;
            var type = Enum.Parse(typeof(DictionaryType), dictionaryName);

            //TODO: загрузка постраничных словарей
            var item = NsiServiceCommon.ExportNsiItem((int)type, ModifiedAfter);
            return item.Select(x => (T)Activator.CreateInstance(typeof(T), x));
        }        
    }
}
