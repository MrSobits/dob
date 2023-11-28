namespace Bars.Gkh.DomainService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using Bars.Gkh.Entities;

    /// <summary>
    /// Сервис для работы УО МКД
    /// </summary>
    public interface IRealityObjectManOrgService
    {
        /// <summary>
        /// Получить словарь актуальных управляющих организаций по домам
        /// </summary>
        IDictionary<long, ManagingOrganization> GetActualManagingOrganizations();

        /// <summary>
        /// Получить актуальные управляющие организации по домам
        /// </summary>
        /// <param name="predicateExpression">Условие фильтрации</param>
        IQueryable<ManagingOrganization> GetActualManagingOrganizationsQuery(Expression<Func<ManOrgContractRealityObject, bool>> predicateExpression);
    }
}