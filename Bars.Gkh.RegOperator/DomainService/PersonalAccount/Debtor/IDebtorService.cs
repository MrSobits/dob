namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount
{
    using System.Collections;
    using B4;
    
    /// <summary>
    /// Сервис должников
    /// </summary>
    public interface IDebtorService
    {
        /// <summary>
        /// Создать должников
        /// </summary>
        /// <param name="baseParams">baseParams</param>
        IDataResult Create(BaseParams baseParams);

        /// <summary>
        /// Очистить реестр
        /// </summary>
        /// <param name="baseParams">baseParams</param>
        IDataResult Clear(BaseParams baseParams);

        /// <summary>
        /// Получить список должников
        /// </summary>
        /// <param name="baseParams">baseParams</param>
        /// <param name="paging">paging</param>
        /// <param name="totalCount">totalCount</param>
        IList GetList(BaseParams baseParams, bool paging, out int totalCount);

        /// <summary>
        /// Создание ПИР
        /// </summary>
        /// <param name="baseParams">baseParams</param>
        IDataResult CreateClaimWorks(BaseParams baseParams);

        /// <summary>
        /// Обновить судебные учереждения
        /// </summary>
        /// <param name="baseParams"> baseParams </param>
        /// <returns> IDataResult </returns>
        IDataResult UpdateJurInstitution(BaseParams baseParams);
    }
}