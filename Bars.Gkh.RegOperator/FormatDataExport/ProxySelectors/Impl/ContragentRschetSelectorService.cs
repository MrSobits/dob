namespace Bars.Gkh.RegOperator.FormatDataExport.ProxySelectors.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Enums;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.FormatDataExport.ProxySelectors;
    using Bars.Gkh.Modules.RegOperator.Entities.RegOperator;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Сервис получения <see cref="ContragentRschetProxy"/>
    /// </summary>
    public class ContragentRschetSelectorService : BaseProxySelectorService<ContragentRschetProxy>
    {
        /// <inheritdoc />
        protected override IDictionary<long, ContragentRschetProxy> GetCache()
        {
            var contragentBankCreditOrgRepository = this.Container.ResolveRepository<ContragentBankCreditOrg>();
            var regopCalcAccountRepository = this.Container.ResolveRepository<RegopCalcAccount>();
            var specialCalcAccountRepository = this.Container.ResolveRepository<SpecialCalcAccount>();

            var regOpRepository = this.Container.ResolveRepository<RegOperator>();

            using (this.Container.Using(contragentBankCreditOrgRepository,
                regopCalcAccountRepository,
                specialCalcAccountRepository,
                regOpRepository))
            {
                var regopContragentId = regOpRepository.GetAll()
                    .Select(x => this.GetId(x.Contragent))
                    .FirstOrDefault() ?? -1;

                var regopCalcAccounts = this.FilterService
                    .FilterByContragent(specialCalcAccountRepository.GetAll(), x => x.AccountOwner)
                    .Where(x => x.IsActive)
                    .Select(x => new ContragentRschetProxy
                    {
                        Id = x.ExportId,
                        SettlementAccount = x.AccountNumber,
                        BankContragentId = this.GetId(x.CreditOrg),
                        ContragentId = this.GetId(x.AccountOwner),
                        CorrAccount = string.Empty,
                        OpenDate = x.DateOpen.IsValid() ? x.DateOpen : x.ObjectCreateDate,
                        CloseDate = x.DateClose,

                        IsFilial = false,
                        CalcAccountId = x.Id,
                        IsRegopAccount = this.GetId(x.AccountOwner) == regopContragentId,
                        RegopAccountType = this.GetId(x.AccountOwner) == regopContragentId ? 1 : default(int?)
                    })
                    .ToList();

                var specialCalcAccounts = this.FilterService
                    .FilterByContragent(specialCalcAccountRepository.GetAll(), x => x.AccountOwner)
                    .Where(x => x.IsActive)
                    .Select(x => new ContragentRschetProxy
                    {
                        Id = x.ExportId,
                        SettlementAccount = x.AccountNumber,
                        BankContragentId = this.GetId(x.CreditOrg),
                        ContragentId = this.GetId(x.AccountOwner),
                        CorrAccount = string.Empty,
                        OpenDate = x.DateOpen.IsValid() ? x.DateOpen : x.ObjectCreateDate,
                        CloseDate = x.DateClose,

                        IsFilial = false,
                        CalcAccountId = x.Id,
                        IsRegopAccount = this.GetId(x.AccountOwner) == regopContragentId,
                        RegopAccountType = this.GetId(x.AccountOwner) == regopContragentId ? 2 : default(int?)
                    })
                    .ToList();

                return this.FilterService
                    .FilterByContragent(contragentBankCreditOrgRepository.GetAll(), x => x.Contragent)
                    .Where(x => x.Contragent.ContragentState == ContragentState.Active)
                    .Select(x => new
                    {
                        x.Id,
                        x.ExportId,
                        x.SettlementAccount,
                        CreditOrgId = this.GetId(x.CreditOrg),
                        ContragentId = this.GetId(x.Contragent),
                        x.CorrAccount,
                        x.ObjectCreateDate,
                        IsFilial = (bool?) x.CreditOrg.IsFilial
                    })
                    .AsEnumerable()
                    .Select(x => new ContragentRschetProxy
                    {
                        Id = x.ExportId,
                        SettlementAccount = x.SettlementAccount,
                        BankContragentId = x.CreditOrgId,
                        ContragentId = x.ContragentId,
                        CorrAccount = x.CorrAccount,
                        OpenDate = x.ObjectCreateDate,

                        IsFilial = x.IsFilial ?? false,
                        ContragentBankCreditOrgId = x.Id,

                        IsRegopAccount = x.ContragentId.HasValue && x.ContragentId == regopContragentId,
                        RegopAccountType = x.ContragentId.HasValue && x.ContragentId == regopContragentId
                            ? 1 : default(int?)
                    })
                    .Union(regopCalcAccounts)
                    .Union(specialCalcAccounts)
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, x => x.OrderBy(y => y.OpenDate).FirstOrDefault());
            }
        }
    }
}