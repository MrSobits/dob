namespace Bars.Gkh.RegOperator.Imports.Ches.PreImport
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Text;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;
    using Bars.Gkh.ConfigSections.Administration;
    using Bars.Gkh.ConfigSections.RegOperator.Administration.BillingInfo;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Enums.Decisions;
    using Bars.Gkh.Extensions.Expressions;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    /// <summary>
    /// Реализация интерфейса сервиса фильтрации импорта
    /// </summary>
    public class BillingFilterService : IBillingFilterService
    {
        private IDictionary<string, DenyReason> DisallowAccounts { get; set; }
        private IDictionary<long, DenyReason> DisallowRealityObjects { get; set; }

        public IWindsorContainer Container { get; set; }

        /// <inheritdoc />
        public bool IsNotAllowAllRows { get; protected set; }

        /// <inheritdoc />
        public string ConfigDescription { get; protected set; }

        /// <inheritdoc />
        public bool CheckByAccountNumber(string accountNumber, out string errorMessage)
        {
            if (this.IsNotAllowAllRows)
            {
                errorMessage = "Данная строка не была загружена: ошибка настройки импорта сведений от биллинга";
                return false;
            }

            DenyReason reason;
            if (this.DisallowAccounts.TryGetValue(accountNumber, out reason))
            {
                errorMessage = this.GetErrorMessage(reason);
                return false;
            }

            errorMessage = string.Empty;
            return true;
        }

        /// <inheritdoc />
        public bool CheckByRealityObject(RealityObject realityObject, out string errorMessage)
        {
            if (this.IsNotAllowAllRows)
            {
                errorMessage = "Данная строка не была загружена: ошибка настройки импорта сведений от биллинга";
                return false;
            }

            DenyReason reason;
            if (this.DisallowRealityObjects.TryGetValue(realityObject.Id, out reason))
            {
                errorMessage = this.GetErrorMessage(reason);
                return false;
            }

            errorMessage = string.Empty;
            return true;
        }

        /// <inheritdoc />
        public void ValidateConfig()
        {
            if (this.IsNotAllowAllRows)
            {
                throw new Exception("Ошибка настройки импорта сведений от биллинга");
            }
        }

        /// <inheritdoc />
        public void Initialize()
        {
            var accountRepository = this.Container.ResolveRepository<BasePersonalAccount>();
            var config = this.Container.GetGkhConfig<AdministrationConfig>().Import.BillingInfo;

            this.IsNotAllowAllRows = !this.CheckConfig(config);
            this.ConfigDescription = this.SerializeConfig(config);

            using (this.Container.Using(accountRepository))
            {
                var query = this.FilterByConfig(
                    accountRepository.GetAll()
                        .WhereNotEmptyString(x => x.PersonalAccountNum),
                    config);

                var accounts = query.Select(x => new
                    {
                        x.PersonalAccountNum,
                        RoId = x.Room.RealityObject.Id,
                        x.Room.RealityObject.AccountFormationVariant,
                        x.Room.RealityObject.TypeHouse,
                        x.Room.RealityObject.ConditionHouse
                    })
                    .ToList();

                this.DisallowAccounts = accounts
                    .ToDictionary(x => x.PersonalAccountNum,
                        x => new DenyReason
                        {
                            AccountFormationVariant = x.AccountFormationVariant,
                            TypeHouse = x.TypeHouse,
                            ConditionHouse = x.ConditionHouse
                        });
                this.DisallowRealityObjects = accounts
                    .GroupBy(x => x.RoId, x => new DenyReason
                    {
                        AccountFormationVariant = x.AccountFormationVariant,
                        TypeHouse = x.TypeHouse,
                        ConditionHouse = x.ConditionHouse
                    })
                    .ToDictionary(x => x.Key, x => x.FirstOrDefault());

                accounts.Clear();
            }
        }

        private IQueryable<BasePersonalAccount> FilterByConfig(IQueryable<BasePersonalAccount> query, BillingInfoConfig config)
        {
            var fundFormFilter = this.GetFilterByFundForm(config).CompositeAnd();
            var realityObjectTypeFilter = this.GetFilterByRealityObjectType(config).CompositeAnd();
            var realityObjectStateFilter = this.GetFilterByRealityObjectState(config).CompositeAnd();

            var filter = new[] { fundFormFilter, realityObjectTypeFilter, realityObjectStateFilter }.CompositeOr();
            if (!filter.IsEmpty())
            {
                Expression<Func<BasePersonalAccount, BasePersonalAccount>> param = x => x;

                return query.Where(Expression.Lambda<Func<BasePersonalAccount, bool>>(filter, param.Parameters[0]));
            }
            else
            {
                return query;
            }
        }

        private IList<Expression> GetFilterByFundForm(BillingInfoConfig config)
        {
            var fundType = config.FundFormManagement;
            var filters = new List<Expression>();

            Expression<Func<BasePersonalAccount, bool>> filterExpression;
            if (fundType.IsRegOpAccount)
            {
                filterExpression = x => x.Room.RealityObject.AccountFormationVariant != CrFundFormationType.RegOpAccount;
                filters.Add(filterExpression.Body);
            }
            if (fundType.IsSpecialAccount)
            {
                filterExpression = x => x.Room.RealityObject.AccountFormationVariant != CrFundFormationType.SpecialAccount;
                filters.Add(filterExpression.Body);
            }
            if (fundType.IsSpecialRegOpAccount)
            {
                filterExpression = x => x.Room.RealityObject.AccountFormationVariant != CrFundFormationType.SpecialRegOpAccount;
                filters.Add(filterExpression.Body);
            }
            if (fundType.IsNotSet)
            {
                filterExpression = x => x.Room.RealityObject.AccountFormationVariant != CrFundFormationType.NotSelected;
                filters.Add(filterExpression.Body);
            }

            return filters;
        }

        private IList<Expression> GetFilterByRealityObjectType(BillingInfoConfig config)
        {
            var roType = config.RealityObjectType;
            var filters = new List<Expression>();

            Expression<Func<BasePersonalAccount, bool>> filterExpression;
            if (roType.IsManyApartments)
            {
                filterExpression = x => x.Room.RealityObject.TypeHouse != TypeHouse.ManyApartments;
                filters.Add(filterExpression.Body);
            }
            if (roType.IsBlockedBuilding)
            {
                filterExpression = x => x.Room.RealityObject.TypeHouse != TypeHouse.BlockedBuilding;
                filters.Add(filterExpression.Body);
            }
            if (roType.IsIndividual)
            {
                filterExpression = x => x.Room.RealityObject.TypeHouse != TypeHouse.Individual;
                filters.Add(filterExpression.Body);
            }
            if (roType.IsSocialBehavior)
            {
                filterExpression = x => x.Room.RealityObject.TypeHouse != TypeHouse.SocialBehavior;
                filters.Add(filterExpression.Body);
            }
            if (roType.IsNotSet)
            {
                filterExpression = x => x.Room.RealityObject.TypeHouse != TypeHouse.NotSet;
                filters.Add(filterExpression.Body);
            }

            return filters;
        }

        private IList<Expression> GetFilterByRealityObjectState(BillingInfoConfig config)
        {
            var roState = config.RealityObjectState;
            var filters = new List<Expression>();

            Expression<Func<BasePersonalAccount, bool>> filterExpression;
            if (roState.IsServiceable)
            {
                filterExpression = x => x.Room.RealityObject.ConditionHouse != ConditionHouse.Serviceable;
                filters.Add(filterExpression.Body);
            }
            if (roState.IsEmergency)
            {
                filterExpression = x => x.Room.RealityObject.ConditionHouse != ConditionHouse.Emergency;
                filters.Add(filterExpression.Body);
            }
            if (roState.IsDilapidated)
            {
                filterExpression = x => x.Room.RealityObject.ConditionHouse != ConditionHouse.Dilapidated;
                filters.Add(filterExpression.Body);
            }
            if (roState.IsRazed)
            {
                filterExpression = x => x.Room.RealityObject.ConditionHouse != ConditionHouse.Razed;
                filters.Add(filterExpression.Body);
            }
            if (roState.IsNotSet)
            {
                filterExpression = x => x.Room.RealityObject.ConditionHouse != ConditionHouse.NotSelected;
                filters.Add(filterExpression.Body);
            }

            return filters;
        }

        private bool CheckConfig(BillingInfoConfig config)
        {
            var fundType = config.FundFormManagement;
            var roType = config.RealityObjectType;
            var roState = config.RealityObjectState;

            return (fundType.IsNotSet || fundType.IsSpecialRegOpAccount || fundType.IsRegOpAccount || fundType.IsSpecialAccount) ||
                (roType.IsNotSet || roType.IsManyApartments || roType.IsBlockedBuilding || roType.IsIndividual || roType.IsSocialBehavior) ||
                (roState.IsNotSet || roState.IsServiceable || roState.IsEmergency || roState.IsDilapidated || roState.IsRazed);
        }

        private string GetErrorMessage(DenyReason reason)
        {
            var sb = new StringBuilder("Данная строка не была загружена: ");
            var reasonList = new List<string>(3);

            if (reason.AccountFormationVariant.HasValue)
            {
                reasonList.Add($"Способ формирования фонда = {reason.AccountFormationVariant.GetDisplayName()}");
            }
            if (reason.TypeHouse.HasValue)
            {
                reasonList.Add($"Тип дома = {reason.TypeHouse.GetDisplayName()}");
            }
            if (reason.ConditionHouse.HasValue)
            {
                reasonList.Add($"Состояние дома = {reason.ConditionHouse.GetDisplayName()}");
            }

            sb.Append(string.Join(", ", reasonList));

            return sb.ToString();
        }

        private string SerializeConfig(BillingInfoConfig config)
        {
            var sb = new StringBuilder();

            sb.AppendLine(config.GetDisplayName());
            this.SerializeProperty(sb, config.FundFormManagement);
            this.SerializeProperty(sb, config.RealityObjectState);
            this.SerializeProperty(sb, config.RealityObjectType);

            return sb.ToString();
        }

        private void SerializeProperty(StringBuilder sb, IGkhConfigSection config)
        {
            sb.AppendLine($"{config.GetDisplayName()}:");
            var configParams = new List<string>();
            foreach (var property in config.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                var name = property.GetCustomAttribute<GkhConfigPropertyAttribute>().DisplayName;
                var value = property.GetValue(config);
                configParams.Add($"{name}: {value}");
            }
            sb.AppendLine(configParams.AggregateWithSeparator(", "));
        }

        private class DenyReason
        {
            public CrFundFormationType? AccountFormationVariant { get; set; }
            public TypeHouse? TypeHouse { get; set; }
            public ConditionHouse? ConditionHouse { get; set; }
        }
    }
}