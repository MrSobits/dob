namespace Bars.Gkh.RegOperator.FormatDataExport.ProxySelectors.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.FormatDataExport.ProxySelectors;
    using Bars.Gkh.RegOperator.DomainService;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;

    /// <summary>
    /// Селектор для Оплата
    /// </summary>
    public class OplataSelectorService : BaseProxySelectorService<OplataProxy>
    {
        /// <inheritdoc />
        protected override IDictionary<long, OplataProxy> GetCache()
        {
            var period = this.SelectParams.GetAs<ChargePeriod>("Period");

            var transferDomainService = this.Container.Resolve<ITransferDomainService>();
            var bankDocumentImportRepository = this.Container.ResolveRepository<BankDocumentImport>();
            var bankAccountStatementRepository = this.Container.ResolveRepository<BankAccountStatement>();
            
            var oplataPackSelectorService = this.ProxySelectorFactory.GetSelector<OplataPackProxy>();
            var epdSelectorService = this.ProxySelectorFactory.GetSelector<EpdProxy>();
            var indSelectorService = this.ProxySelectorFactory.GetSelector<IndProxy>();
            var domSelectorService = this.ProxySelectorFactory.GetSelector<DomProxy>();

            var epdAccountIds = epdSelectorService.ProxyListCache.Values.Select(x => x.AccountId).ToList();
            var indIds = indSelectorService.ProxyListCache.Keys;

            using (this.Container.Using(bankDocumentImportRepository, bankAccountStatementRepository))
            {
                return this.FilterService
                    .FilterByRealityObject(transferDomainService.GetAll<PersonalAccountPaymentTransfer>(), x => x.Owner.Room.RealityObject)
                    .Where(
                        x => bankDocumentImportRepository.GetAll().Any(y => y.TransferGuid == x.Operation.OriginatorGuid)
                            || bankAccountStatementRepository.GetAll().Any(y => y.TransferGuid == x.Operation.OriginatorGuid))
                    .Where(x => x.IsAffect)
                    .Where(x => x.ChargePeriod.Id == period.Id)
                    .Where(x => x.Reason.StartsWith("Оплата") || x.Reason.StartsWith("Отмена оплаты"))
                    .Select(
                        x => new
                        {
                            x.Id,
                            AccountId = x.Owner.Id,
                            OperationType = x.Reason.StartsWith("Оплата") ? 1 : 2,
                            x.PaymentDate,
                            x.OperationDate,
                            Amount = x.Reason.StartsWith("Оплата") ? x.Amount : -1 * x.Amount,
                            RoId = x.Owner.Room.RealityObject.Id,
                            OwnerId = x.Owner.AccountOwner.Id,
                            x.Operation.OriginatorGuid
                        })
                    .AsEnumerable()
                    .Where(x => epdAccountIds.Contains(x.AccountId))
                    .Where(x => indIds.Contains(x.OwnerId))
                    .Select(
                        x =>
                        {
                            var oplataPack = oplataPackSelectorService.ProxyListCache.Values.FirstOrDefault(y => y.TransferGuid == x.OriginatorGuid);

                            return new OplataProxy
                            {
                                Id = x.Id,
                                KvarId = x.AccountId,
                                OperationType = x.OperationType,
                                DocumentNumber = oplataPack?.Number,
                                PaymentDate = x.PaymentDate,
                                OperationDate = x.OperationDate,
                                Amount = x.Amount,
                                OplataPackId = this.GetId(oplataPack),
                                ContragentRschetId = domSelectorService.ProxyListCache.Get(x.RoId)?.CalcAccountId,
                                EpdId = this.GetId(epdSelectorService.ProxyListCache.Values.FirstOrDefault(y => y.AccountId == x.AccountId)),
                                IndId = this.GetId(indSelectorService.ProxyListCache.Get(x.OwnerId)),
                                PayerName = oplataPack?.PayerName,
                                Destination = oplataPack?.Destination
                            };
                        })
                    .ToDictionary(x => x.Id);
            }
        }
    }
}