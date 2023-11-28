namespace Bars.Gkh.FormatDataExport.FormatProvider
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.ConfigSections.Administration;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.FormatDataExport.Domain;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    public class ExportFormatProviderBuilder : IExportFormatProviderBuilder
    {
        public IWindsorContainer Container { get; set; }
        public IDomainService<Contragent> ContragentDomainService { get; set; }
        public IDomainService<Operator> OperatorDomainService { get; set; }
        public IExportableEntityResolver ExportableEntityResolver { get; set; }
        public IFormatDataExportRoleService FormatDataExportRoleService { get; set; }
        public IEnumerable<IExportableEntityGroup> ExportableEntityGroups { get; set; }
        public IGkhUserManager GkhUserManager { get; set; }
        public IDomainService<OperatorContragent> OperatorContragentDomain { get; set; }

        private BaseParams baseParams = new BaseParams();
        private CancellationToken cancellationToken = CancellationToken.None;
        private IList<string> entityGroupCodeList;

        /// <inheritdoc />
        public IExportFormatProviderBuilder SetCancellationToken(CancellationToken token)
        {
            this.cancellationToken = token;

            return this;
        }

        /// <inheritdoc />
        public IExportFormatProviderBuilder SetParams(BaseParams builderParams)
        {
            this.baseParams = builderParams;

            return this;
        }

        /// <inheritdoc />
        public IExportFormatProviderBuilder SetEntytyGroupCodeList(IList<string> entityGroupCodes)
        {
            this.entityGroupCodeList = entityGroupCodes;

            return this;
        }

        /// <inheritdoc />
        public IExportFormatProvider Build<T>() where T : BaseFormatProvider, new()
        {
            var provider = this.Container.Resolve<T>();

            provider.CancellationToken = this.cancellationToken;
            provider.DataSelectorParams.Apply(this.baseParams.Params);
            this.ApplyConfigParams(provider.DataSelectorParams);

            var operatorId = provider.DataSelectorParams.GetAsId("OperatorId");

            provider.Operator = this.OperatorDomainService.Get(operatorId);

            provider.Contragent = provider.Operator.Contragent;

            if (provider.Operator.Role == null)
            {
                provider.Operator.Role = this.GkhUserManager.GetActiveOperatorRoles().FirstOrDefault();
            }

            var providerType = this.FormatDataExportRoleService.GetProviderType(provider.Operator.Role);
            var entityCodes = this.entityGroupCodeList.IsEmpty()
                ? provider.DataSelectorParams.GetAs("EntityGroupCodes", new List<string>())
                : this.entityGroupCodeList;

            var entities = entityCodes.IsEmpty()
                ? this.ExportableEntityResolver.GetEntityList(providerType)
                : this.ExportableEntityResolver.GetInheritedEntityList(entityCodes, providerType);

            var sortedEntities = this.SortEtities(entities);

            foreach (var code in provider.ServiceEntityCodes)
            {
                sortedEntities.Add(this.ExportableEntityResolver.GetEntity(code, providerType));
            }

            provider.InitExportableEntities(sortedEntities.AsReadOnly());

            return provider;
        }

        private void ApplyConfigParams(DynamicDictionary dataSelectorParams)
        {
            var config = this.Container.GetGkhConfig<AdministrationConfig>()
                .FormatDataExport;

            var paramsConfig = config.FormatDataExportParams;

            dataSelectorParams["GjiContragentId"] = paramsConfig.GjiContragent.Id;
            dataSelectorParams["LeaderPositionId"] = paramsConfig.LeaderPosition.Id;
            dataSelectorParams["AccountantPositionId"] = paramsConfig.AccountantPosition.Id;
            dataSelectorParams["TsjChairmanPosition"] = paramsConfig.TsjChairmanPosition.Id;
            dataSelectorParams["TsjMemberPositionId"] = paramsConfig.TsjMemberPosition.Id;
            dataSelectorParams["TimeZone"] = paramsConfig.TimeZone;
        }

        private List<IExportableEntity> SortEtities(IEnumerable<IExportableEntity> entities)
        {
            var i = 10;
            var entityDict = entities.ToDictionary(x => x.Code, x => new StructOrder(i++, x));

            foreach (var entity in entities)
            {
                var cycleChecker = new List<string>();
                this.CalcWeight(entity, entityDict, cycleChecker);
            }

            return entityDict.OrderBy(x => x.Value.Weight)
                .Select(x => x.Value.ExportableEntity)
                .ToList();
        }

        private void CalcWeight(IExportableEntity entity, Dictionary<string, StructOrder> entityDict, List<string> cycleChecker)
        {
            if (cycleChecker.FirstOrDefault() == entity.Code)
            {
                throw new OverflowException($"Обнаружена циклическая зависимость секций: {cycleChecker.AggregateWithSeparator(", ")}");
            }
            var weight = entityDict.Get(entity.Code).Weight;
            cycleChecker.Add(entity.Code);
            entity.GetInheritedEntityCodeList()
                .Where(entityDict.ContainsKey)
                .ForEach(x =>
                {
                    var structOrder = entityDict[x];
                    structOrder.Weight += weight;
                    this.CalcWeight(entityDict[x].ExportableEntity, entityDict, cycleChecker);
                });
        }

        private class StructOrder
        {
            public int Weight { get; set; }

            public IExportableEntity ExportableEntity { get; }

            public StructOrder(int w, IExportableEntity entity)
            {
                this.Weight = w;
                this.ExportableEntity = entity;
            }
        }
    }
}