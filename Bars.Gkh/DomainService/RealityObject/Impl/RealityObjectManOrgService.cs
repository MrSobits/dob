﻿namespace Bars.Gkh.DomainService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Utils;

    using NHibernate.Linq;

    /// <summary>
    /// Сервис для работы УО МКД
    /// </summary>
    public class RealityObjectManOrgService : IRealityObjectManOrgService
    {
        /// <summary>
        /// МКД - УО
        /// </summary>
        public IRepository<ManOrgContractRealityObject> ManOrgContractRealityObjectRepository { get; set; }

        /// <inheritdoc />
        public IDictionary<long, ManagingOrganization> GetActualManagingOrganizations()
        {
            return this.GetQuery()
                .Fetch(x => x.ManOrgContract)
                .ThenFetch(x => x.ManagingOrganization)
                .ThenFetch(x => x.Contragent)
                .Select(x => new
                {
                    x.RealityObject.Id,
                    x.ManOrgContract.StartDate,
                    x.ManOrgContract.ManagingOrganization,
                    x.ManOrgContract.ManagingOrganization.Contragent
                })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, x => x.OrderByDescending(y => y.StartDate).FirstOrDefault()?.ManagingOrganization);
        }

        /// <inheritdoc />
        public IQueryable<ManagingOrganization> GetActualManagingOrganizationsQuery(Expression<Func<ManOrgContractRealityObject, bool>> predicateExpression)
        {
            return this.GetQuery()
                .Where(predicateExpression)
                .Select(x => x.ManOrgContract.ManagingOrganization);
        }

        private IQueryable<ManOrgContractRealityObject> GetQuery()
        {
            var nowDate = DateTime.Today;

            return this.ManOrgContractRealityObjectRepository.GetAll()
                .WhereNotNull(x => x.RealityObject)
                .WhereNotNull(x => x.ManOrgContract.ManagingOrganization)
                .Where(x => x.RealityObject.TypeHouse != TypeHouse.BlockedBuilding
                    && x.RealityObject.TypeHouse != TypeHouse.Individual)
                .Where(x => x.RealityObject.ConditionHouse == ConditionHouse.Serviceable
                    || x.RealityObject.ConditionHouse == ConditionHouse.Emergency
                    || x.RealityObject.ConditionHouse == ConditionHouse.Dilapidated
                    && !x.RealityObject.ResidentsEvicted)
                .Where(x => x.ManOrgContract.StartDate <= nowDate && nowDate <= (x.ManOrgContract.EndDate ?? DateTime.MaxValue));
        }
    }
}