namespace Bars.B4.Modules.FIAS.AutoUpdater.Converter
{
    using System.Collections.Generic;

    /// <summary>
    /// Сервис конвертации данных ФИАС из *.dbf файлов
    /// </summary>
    internal interface IFiasDbConverter
    {
        /// <summary>
        /// Получить записи <see cref="Fias"/> из dbf файла
        /// </summary>
        /// <param name="dbfFilePath">Путь к *.dbf файлу</param>
        IEnumerable<AddressObjectsObject> GetFiasRecords(string dbfFilePath);

        /// <summary>
        /// Получить записи <see cref="FiasHouse"/> из dbf файла
        /// </summary>
        /// <param name="dbfFilePath">Путь к *.dbf файлу</param>
        IEnumerable<HousesHouse> GetFiasHouseRecords(string dbfFilePath);
    }
}