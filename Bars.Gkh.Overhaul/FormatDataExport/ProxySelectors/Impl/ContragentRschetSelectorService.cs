namespace Bars.Gkh.Overhaul.FormatDataExport.ProxySelectors.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Enums;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.FormatDataExport.ProxySelectors;
    using Bars.Gkh.Overhaul.Entities;

    /// <summary>
    /// Сервис получения <see cref="ContragentRschetProxy"/>
    /// </summary>
    public class ContragentRschetSelectorService : BaseProxySelectorService<ContragentRschetProxy>
    {
        /// <inheritdoc />
        protected override IDictionary<long, ContragentRschetProxy> GetCache()
        {
            var contragentBankCreditOrgRepository = this.Container.ResolveRepository<ContragentBankCreditOrg>();

            using (this.Container.Using(contragentBankCreditOrgRepository))
            {
                return this.FilterService
                    .FilterByContragent(contragentBankCreditOrgRepository.GetAll(), x => x.Contragent)
                    .Where(x => x.Contragent.ContragentState == ContragentState.Active)
                    .Select(x => new ContragentRschetProxy
                    {
                        Id = x.ExportId,
                        SettlementAccount = x.SettlementAccount,
                        BankContragentId = this.GetId(x.CreditOrg),
                        ContragentId = this.GetId(x.Contragent),
                        CorrAccount = x.CorrAccount,
                        OpenDate = x.ObjectCreateDate,

                        ContragentBankCreditOrgId = x.Id
                    })
                    .ToDictionary(x => x.Id);
            }
        }
    }
}