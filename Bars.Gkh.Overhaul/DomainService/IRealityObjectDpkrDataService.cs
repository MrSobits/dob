namespace Bars.Gkh.Overhaul.DomainService
{
    using System.Collections.Generic;
    using System.Linq;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Сервис дома в ДПКР
    /// </summary>
    public interface IRealityObjectDpkrDataService
    {
        /// <summary>
        /// Получить информацию о работе
        /// </summary>
        /// <param name="ro">Дом</param>
        /// <returns></returns>
        List<JobYears> GetWorkInfoByRealityObject(RealityObject ro);

        /// <summary>
        /// Получить информацию а дате и годе
        /// </summary>
        /// <param name="municipality">МО</param>
        /// <param name="year">Год</param>
        /// <returns></returns>
        IQueryable<RealityObjectDpkrInfo> GetDpkrDataAboveYear(Municipality municipality, int year);

        /// <summary>
        /// Получить объекты в программе
        /// </summary>
        /// <param name="municipality">Мун образование</param>
        /// <returns></returns>
        IQueryable<RealityObject> GetRobjectsInProgram(Municipality municipality = null);

        /// <summary>
        /// Получить опубликованные дома
        /// </summary>
        /// <param name="roId">Дом</param>
        /// <param name="minYear">Минимальный год</param>
        /// <returns></returns>
        Dictionary<long, int> GetPublishYearByCeo(long roId, int minYear);

        /// <summary>
        /// Получить скорректированные года конструктивных элементов
        /// </summary>
        /// <param name="roId">Дом</param>
        /// <returns></returns>
        Dictionary<long, int> GetAdjustedYears(long roId);
    }
}