namespace Bars.Gkh.RegOperator.FormatDataExport.ProxySelectors.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.FormatDataExport.ProxySelectors;
    using Bars.Gkh.RegOperator.DataProviders.Meta;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.PayDoc;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Селектор для Платежный документ
    /// </summary>
    public class EpdSelectorService : BaseProxySelectorService<EpdProxy>
    {
        /// <inheritdoc />
        protected override IDictionary<long, EpdProxy> GetCache()
        {
            var paymentDocumentSnapshotRepository = this.Container.ResolveRepository<PaymentDocumentSnapshot>();

            using (this.Container.Using(paymentDocumentSnapshotRepository))
            {
                var period = this.SelectParams.GetAs<ChargePeriod>("Period");
                var kvarIds = this.ProxySelectorFactory.GetSelector<KvarProxy>().ProxyListCache.Keys;

                var contragentRschetDict = new Dictionary<string, long?>();
                var contragentRschetGroup = this.ProxySelectorFactory.GetSelector<ContragentRschetProxy>()
                    .ProxyListCache
                    .Values
                    .WhereNotEmptyString(x => x.SettlementAccount)
                    .Select(x => new
                    {
                        x.Id,
                        x.SettlementAccount
                    })
                    .GroupBy(x => x.SettlementAccount, x => (long?) x.Id);
                try
                {
                    contragentRschetDict = contragentRschetGroup.ToDictionary(x => x.Key, x => x.DistinctValues().SingleOrDefault());
                }
                catch (InvalidOperationException e)
                {
                    throw new Exception("Для одного расчетного счета привязано более одного контрагента", e);
                }

                return paymentDocumentSnapshotRepository.GetAll()
                    .Where(x => x.Period.Id == period.Id)
                    .Where(x => x.OwnerType == PersonalAccountOwnerType.Individual)
                    .WhereNotEmptyString(x => x.PaymentReceiverAccount)
                    .Select(
                        x => new
                        {
                            x.Id,
                            x.ObjectCreateDate,
                            AccountId = x.HolderId,
                            x.TotalCharge,
                            x.PaymentReceiverAccount,
                            Data = x.ConvertTo<InvoiceInfo>()
                        })
                    .AsEnumerable()
                    .Where(x => kvarIds.Contains(x.AccountId))
                    .Select(
                        x =>
                        {
                            var debt = x.Data?.ДолгНаНачало;
                            var debtNew = x.Data?.ДолгБазовыйНаНачало == null || x.Data?.ДолгТарифРешенияНаНачало == null
                                || x.Data?.ДолгПениНаНачало == null
                                ? 0
                                : (decimal) (x.Data.ДолгБазовыйНаНачало + x.Data.ДолгТарифРешенияНаНачало + x.Data.ДолгПениНаНачало);
                            var overpayments = x.Data?.ПереплатаНаНачало;

                            return new EpdProxy
                            {
                                Id = x.Id,
                                Date = x.ObjectCreateDate,
                                AccountId = x.AccountId,
                                PaymentReceiverAccountCode = contragentRschetDict.Get(x.PaymentReceiverAccount),
                                LivingArea = x.Data?.ОбщаяПлощадь,
                                Debt = debt ?? (debtNew > 0 ? debtNew : 0),
                                Overpayment = overpayments ?? (debtNew < 0 ? -debtNew : 0),
                                Tariff = x.Data.Тариф,
                                Correction = x.Data.КорректировкаБазовый + x.Data.КорректировкаТарифРешения + x.Data.КорректировкаПени,
                                Recalc = x.Data.ПерерасчетБазовый + x.Data.ПерерасчетТарифРешения + x.Data.ПерерасчетПени,
                                Charge = x.Data.НачисленоБазовый + x.Data.НачисленоТарифРешения + x.Data.НачисленоПени,
                                Benefit = x.Data.Льгота,
                                SaldoOut = x.Data.ДолгБазовыйНаКонец + x.Data.ДолгТарифРешенияНаКонец + x.Data.ДолгПениНаКонец
                            };
                        })
                    .ToDictionary(x => x.Id);
            }
        }
    }
}