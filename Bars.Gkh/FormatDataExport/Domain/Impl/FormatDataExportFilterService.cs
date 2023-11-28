namespace Bars.Gkh.FormatDataExport.Domain.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Linq.Expressions;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.Modules.Gkh1468.Entities;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    public class FormatDataExportFilterService : IFormatDataExportFilterService
    {
        public IWindsorContainer Container { get; set; }

        private int BulkSize { get; set; }
        private bool IsFilteredByRealityObject { get; set; }
        private IList<long> OperatorInspectorIdList { get; set; }
        private IList<long> OperatorContragentIdList { get; set; }
        private IList<long> OperatorMunicipalityIdList { get; set; }

        /// <inheritdoc />
        public ReadOnlyCollection<long> ContragentIds { get; private set; } = new ReadOnlyCollection<long>(new List<long>());

        /// <inheritdoc />
        public ReadOnlyCollection<long> RealityObjectIds { get; private set; } = new ReadOnlyCollection<long>(new List<long>());

        /// <inheritdoc />
        public void Init(FormatDataExportProviderType provider, DynamicDictionary filterParams, int bulkSize)
        {
            this.BulkSize = bulkSize;
            this.OperatorInspectorIdList = filterParams.GetAs("OperatorInspectorList", new List<long>());
            this.OperatorContragentIdList = filterParams.GetAs("OperatorContragentList", new List<long>());
            this.OperatorMunicipalityIdList = filterParams.GetAs("OperatorMunicipalityList", new List<long>());

            switch (provider)
            {
                case FormatDataExportProviderType.Uo:
                    this.InitUo();
                    return;

                case FormatDataExportProviderType.Oms:
                    this.InitOms();
                    return;

                case FormatDataExportProviderType.Rc:
                    this.InitRc();
                    return;

                case FormatDataExportProviderType.Rso:
                    this.InitRso();
                    return;

                case FormatDataExportProviderType.Gji:
                    break;
                case FormatDataExportProviderType.Ogv:
                    break;
                case FormatDataExportProviderType.RegOpCr:
                    this.InitRegop();
                    return;
                case FormatDataExportProviderType.Omjk:
                    break;
                case FormatDataExportProviderType.OgvEnergo:
                    break;
                case FormatDataExportProviderType.RegOpWaste:
                    break;
            }

            this.ContragentIds = new ReadOnlyCollection<long>(this.OperatorContragentIdList);
        }

        /// <inheritdoc />
        public IQueryable<Contragent> FilterByContragent(IQueryable<Contragent> query)
        {
            return this.WhereContains(query, x => x, this.ContragentIds);
        }

        /// <inheritdoc />
        public IQueryable<TEntity> FilterByContragent<TEntity>(IQueryable<TEntity> query, Expression<Func<TEntity, Contragent>> contragentSelector)
        {
            return this.WhereContains(query, contragentSelector, this.ContragentIds);
        }

        /// <inheritdoc />
        public IEnumerable<TEntity> FilterByContragent<TEntity>(IEnumerable<TEntity> data, Expression<Func<TEntity, Contragent>> contragentSelector)
        {
            return this.FilterByContragent(data.AsQueryable(), contragentSelector);
        }

        /// <inheritdoc />
        public IQueryable<RealityObject> FilterByRealityObject(IQueryable<RealityObject> query)
        {
            if (this.IsFilteredByRealityObject)
            {
                return this.WhereContains(query, x => x, this.RealityObjectIds);
            }

            return query;
        }

        /// <inheritdoc />
        public IQueryable<TEntity> FilterByRealityObject<TEntity>(IQueryable<TEntity> query, Expression<Func<TEntity, RealityObject>> realityObjectSelector)
        {
            if (this.IsFilteredByRealityObject)
            {
                return this.WhereContains(query, realityObjectSelector, this.RealityObjectIds);
            }

            return query;
        }

        /// <inheritdoc />
        public IEnumerable<TEntity> FilterByRealityObject<TEntity>(IEnumerable<TEntity> data, Expression<Func<TEntity, RealityObject>> realityObjectSelector)
        {
            return this.FilterByRealityObject(data.AsQueryable(), realityObjectSelector);
        }

        private IQueryable<TEntity> WhereContains<TEntity, TProperty>(
            IQueryable<TEntity> query,
            Expression<Func<TEntity, TProperty>> propertySelector,
            ReadOnlyCollection<long> collection)
        {
            var offset = 0;
            var count = collection.Count;
            var entity = propertySelector.Parameters[0];
            var id = typeof(Contragent).GetProperty("Id");
            var idSelector = Expression.Property(propertySelector.Body, id);

            var expressionParts = new List<Expression>();

            while (offset < count)
            {
                var partSize = count > this.BulkSize
                    ? this.BulkSize
                    : count - offset;

                var partData = collection.Skip(offset).Take(partSize).ToList();

                expressionParts.Add(Expression.Call(
                    Expression.Constant(partData),
                    partData.GetType().GetMethod("Contains"),
                    idSelector));

                offset += partSize;
            }

            if (expressionParts.Count > 0)
            {
                var wholeExpression = expressionParts.First();
                foreach (var expression in expressionParts.Skip(1))
                {
                    wholeExpression = Expression.MakeBinary(ExpressionType.OrElse, wholeExpression, expression);
                }

                var predicate = Expression.Lambda<Func<TEntity, bool>>(wholeExpression, entity);

                query = query.Where(predicate);
            }
            else
            {
                query = query.Where(x => false);
            }

            return query;
        }

        private void InitUo()
        {
            var manOrgRepository = this.Container.ResolveRepository<ManagingOrganization>();
            var manOrgRoRepository = this.Container.ResolveRepository<ManOrgContractRealityObject>();

            using (this.Container.Using(manOrgRepository, manOrgRoRepository))
            {
                var query = manOrgRepository.GetAll();
                if (this.OperatorMunicipalityIdList.IsNotEmpty())
                {
                    query = query.WhereContainsBulked(x => x.Contragent.Municipality.Id, this.OperatorMunicipalityIdList, 5000);
                }
                if (this.OperatorContragentIdList.IsNotEmpty())
                {
                    query = query.WhereContainsBulked(x => x.Contragent.Id, this.OperatorContragentIdList, 5000);
                }

                var contragentIds = query
                    .Select(x => x.Contragent.Id)
                    .ToList();

                this.ContragentIds = new ReadOnlyCollection<long>(contragentIds);

                this.CheckContragents("Не удалось определить контрагентов УО");

                var realityObjectIds = manOrgRoRepository.GetAll()
                    .WhereNotNull(x => x.RealityObject)
                    .WhereNotNull(x => x.ManOrgContract.ManagingOrganization.Contragent)
                    .Where(x => x.RealityObject.FiasAddress.HouseGuid.HasValue)
                    .Where(x => this.ContragentIds.Contains(x.ManOrgContract.ManagingOrganization.Contragent.Id))
                    .Select(x => x.RealityObject.Id)
                    .ToArray();

                this.RealityObjectIds = new ReadOnlyCollection<long>(realityObjectIds);
                this.IsFilteredByRealityObject = true;
            }
        }

        private void InitOms()
        {
            var omsRepository = this.Container.ResolveRepository<LocalGovernment>();
            var omsMoRepository = this.Container.ResolveRepository<LocalGovernmentMunicipality>();
            var realityObjectRepository = this.Container.ResolveRepository<RealityObject>();

            using (this.Container.Using(omsRepository, omsMoRepository, realityObjectRepository))
            {
                var contragentIds = new List<long>();

                if (this.OperatorContragentIdList.IsNotEmpty())
                {
                    contragentIds = omsRepository.GetAll()
                        .WhereContainsBulked(x => x.Contragent.Id, this.OperatorContragentIdList, 5000)
                        .Select(x => x.Contragent.Id)
                        .ToList();

                } else if (this.OperatorMunicipalityIdList.IsNotEmpty())
                {
                    contragentIds = omsMoRepository.GetAll()
                        .WhereNotNull(x => x.LocalGovernment)
                        .WhereContainsBulked(x => x.Municipality.Id, this.OperatorMunicipalityIdList, 5000)
                        .Select(x => x.LocalGovernment.Contragent.Id)
                        .ToList();
                }

                var uniqueContragentIds = contragentIds.DistinctValues().ToList();

                var municipalityIds = omsMoRepository.GetAll()
                    .WhereNotNull(x => x.LocalGovernment)
                    .WhereNotNull(x => x.Municipality)
                    .WhereContainsBulked(x => x.LocalGovernment.Contragent.Id, uniqueContragentIds)
                    .Select(x => x.Municipality.Id)
                    .AsEnumerable()
                    .DistinctValues()
                    .ToList();

                this.ContragentIds = new ReadOnlyCollection<long>(uniqueContragentIds);

                this.CheckContragents("Не удалось определить контрагентов ОМС");

                var realityObjectIds = realityObjectRepository.GetAll()
                    .WhereNotNull(x => x.Municipality)
                    .Where(x => x.FiasAddress.HouseGuid.HasValue)
                    .WhereContainsBulked(x => x.Municipality.Id, municipalityIds)
                    .Select(x => x.Id)
                    .ToArray();

                this.RealityObjectIds = new ReadOnlyCollection<long>(realityObjectIds);
                this.IsFilteredByRealityObject = true;
            }
        }

        private void InitRso()
        {
            var publicServiceOrgRepository = this.Container.ResolveRepository<PublicServiceOrg>();
            var publicServiceOrgContractRealObjRepository = this.Container.ResolveRepository<PublicServiceOrgContractRealObj>();

            using (this.Container.Using(publicServiceOrgRepository, publicServiceOrgContractRealObjRepository))
            {
                var query = publicServiceOrgRepository.GetAll();
                if (this.OperatorMunicipalityIdList.IsNotEmpty())
                {
                    query = query.WhereContainsBulked(x => x.Contragent.Municipality.Id, this.OperatorMunicipalityIdList, 5000);
                }
                if (this.OperatorContragentIdList.IsNotEmpty())
                {
                    query = query.WhereContainsBulked(x => x.Contragent.Id, this.OperatorContragentIdList, 5000);
                }

                var contragentIds = query
                    .Select(x => x.Contragent.Id)
                    .ToList();

                this.ContragentIds = new ReadOnlyCollection<long>(contragentIds);

                this.CheckContragents("Не удалось определить контрагентов РСО");

                var realityObjectIds = publicServiceOrgContractRealObjRepository.GetAll()
                    .WhereNotNull(x => x.RealityObject)
                    .WhereNotNull(x => x.RsoContract.PublicServiceOrg.Contragent)
                    .Where(x => this.ContragentIds.Contains(x.RsoContract.PublicServiceOrg.Contragent.Id))
                    .Where(x => x.RealityObject.FiasAddress.HouseGuid.HasValue)
                    .Select(x => x.RealityObject.Id)
                    .ToArray();

                this.RealityObjectIds = new ReadOnlyCollection<long>(realityObjectIds);
                this.IsFilteredByRealityObject = true;
            }
        }

        private void InitRc()
        {
            var cashPaymentCenterRepository = this.Container.ResolveRepository<CashPaymentCenter>();
            var cashPaymentCenterRealObjRepository = this.Container.ResolveRepository<CashPaymentCenterRealObj>();

            using (this.Container.Using(cashPaymentCenterRepository, cashPaymentCenterRealObjRepository))
            {
                var query = cashPaymentCenterRepository.GetAll();
                if (this.OperatorMunicipalityIdList.IsNotEmpty())
                {
                    query = query.WhereContainsBulked(x => x.Contragent.Municipality.Id, this.OperatorMunicipalityIdList, 5000);
                }
                if (this.OperatorContragentIdList.IsNotEmpty())
                {
                    query = query.WhereContainsBulked(x => x.Contragent.Id, this.OperatorContragentIdList, 5000);
                }

                var contragentIds = query
                    .Select(x => x.Contragent.Id)
                    .ToList();

                this.ContragentIds = new ReadOnlyCollection<long>(contragentIds);

                this.CheckContragents("Не удалось определить контрагентов РКЦ");

                var realityObjectIds = cashPaymentCenterRealObjRepository.GetAll()
                    .WhereNotNull(x => x.RealityObject)
                    .WhereNotNull(x => x.CashPaymentCenter.Contragent)
                    .Where(x => this.ContragentIds.Contains(x.CashPaymentCenter.Contragent.Id))
                    .Where(x => x.RealityObject.FiasAddress.HouseGuid.HasValue)
                    .Select(x => x.RealityObject.Id)
                    .ToArray();

                this.RealityObjectIds = new ReadOnlyCollection<long>(realityObjectIds);
                this.IsFilteredByRealityObject = true;
            }
        }

        private void InitRegop()
        {
            this.ContragentIds = new ReadOnlyCollection<long>(this.OperatorContragentIdList);

            var realityObjectRepository = this.Container.ResolveRepository<RealityObject>();

            using (this.Container.Using(realityObjectRepository))
            {
                var realityObjectIds = realityObjectRepository.GetAll()
                    .Where(x => x.FiasAddress.HouseGuid.HasValue)
                    .Where(
                        x => x.AccountFormationVariant == CrFundFormationType.RegOpAccount
                            || x.AccountFormationVariant == CrFundFormationType.SpecialRegOpAccount)
                    .Select(x => x.Id)
                    .ToArray();

                this.RealityObjectIds = new ReadOnlyCollection<long>(realityObjectIds);

                this.IsFilteredByRealityObject = true;
            }
        }

        private void CheckContragents(string errorMessage)
        {
            if (this.ContragentIds.IsEmpty())
            {
                throw new Exception(errorMessage);
            }
        }
    }
}