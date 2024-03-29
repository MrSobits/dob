﻿namespace Bars.GisIntegration.Base.Map.Bills
{
    using Bars.GisIntegration.Base.Entities.Bills;

    /// <summary>
    /// Маппинг сущности Bars.Gkh.Ris.Entities.HcsBills.RisAdditionalServiceChargeInfo
    /// </summary>
    public class RisAdditionalServiceChargeInfoMap: BaseRisChargeInfoMap<RisAdditionalServiceChargeInfo>
    {
        /// <summary>
        /// Конструктор маппинга сущности Bars.Gkh.Ris.Entities.HcsBills.RisAdditionalServiceChargeInfo
        /// </summary>
        public RisAdditionalServiceChargeInfoMap()
            : base("Bars.Gkh.Ris.Entities.Bills.RisAdditionalServiceChargeInfo", "RIS_ADDITIONAL_SERVICE_CHARGE_INFO"
                )
        {
        }

        /// <summary>
        /// Инициализация маппинга
        /// </summary>
        protected override void Map()
        {
            this.Reference(x => x.PaymentDocument, "PaymentDocument").Column("PAYMENT_DOC_ID");
            this.Property(x => x.MoneyRecalculation, "MoneyRecalculation").Column("MONEY_RECALCULATION");
            this.Property(x => x.MoneyDiscount, "MoneyDiscount").Column("MONEY_DISCOUNT");
            this.Property(x => x.TotalPayable, "TotalPayable").Column("TOTAL_PAYABLE");
            this.Property(x => x.AccountingPeriodTotal, "AccountingPeriodTotal").Column("ACCOUNTING_PERIOD_TOTAL");
            this.Property(x => x.CalcExplanation, "CalcExplanation").Column("CALC_EXPLANATION");
            this.Property(x => x.OrgPpaguid, "OrgPpaguid").Column("ORG_PPAGUID");
        }
    }
}
