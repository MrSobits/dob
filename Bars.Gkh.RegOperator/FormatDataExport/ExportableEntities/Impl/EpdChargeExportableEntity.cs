namespace Bars.Gkh.RegOperator.FormatDataExport.ExportableEntities.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.FormatDataExport.ProxyEntities;

    /// <summary>
    /// 2.11.16. Начисления в платежном документе (epdcharge.csv)
    /// </summary>
    public class EpdChargeExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "EPDCHARGE";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags => 
            FormatDataExportProviderFlags.Uo |
                FormatDataExportProviderFlags.Rso |
                FormatDataExportProviderFlags.Rc;

        /// <inheritdoc />
        protected override IList<int> MandatoryFields => new List<int> { 0, 1, 3, 4, 5 };

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            //todo дописать получение кода услуги, как только появится описание (и добавить обязательность)
            return this.ProxySelectorFactory.GetSelector<EpdProxy>()
                .ProxyListCache.Values
                .Select(x => new ExportableRow(x,
                    new List<string>
                    {
                        this.GetStrId(x), // 1. Уникальный код
                        x.SnapshotIdCharge.ToStr(), // 2. Платежный документ
                        x.ServiceCode, // 3. Код услуги
                        this.GetDecimal(x.TariffCharge), // 4. Тариф
                        this.GetDecimal(x.SaldoOutCharge), // 5. Итого к оплате за расчетный период
                        this.GetDecimal(x.ChargeCharge), // 6. Всего начислено за расчетный период ( без перерасчетов и льгот)
                        this.GetDecimal(x.ChangeCharge), // 7. Перерасчеты, корректировки (руб.)
                        this.GetDecimal(x.BenefitCharge), // 8. Льготы, субсидии, скидки (руб.)
                        x.ServiceType, // 9. Тип предоставления услуги
                        x.CurrentSum, // 10. Сумма платы с учётом рассрочки платежа - от платы за расчётный период
                        x.PreviousSum, // 11. Сумма платы с учётом рассрочки платежа - от платы за предыдущие расчётные периоды
                        x.PercentInRubles, // 12. Проценты за рассрочку, руб.
                        x.Percent, // 13. Проценты за рассрочку, %
                        x.SumWithPercent, // 14. Сумма к оплате с учётом рассрочки платежа и процентов за рассрочку
                        x.RecalcReason, // 15. Основания перерасчётов
                        this.GetDecimal(x.RecalcCharge), // 16. Сумма перерасчета , руб.
                        x.IndividualMeteringDeviceMeasure, // 17. Текущие показания приборов учёта коммунальных услуг - индивидуальное потребление
                        x.HouseMeteringDeviceMeasure, // 18. Текущие показания приборов учёта коммунальных услуг - общедомовые нужды
                        x.SumIndividualMeteringDevice, // 19. Суммарный объём коммунальных услуг в доме - индивидуальное потребление
                        x.SumHouseMeteringDevice, // 20. Суммарный объём коммунальных услуг в доме - общедомовые нужды
                        x.HouseMeteringDeviceStandard, // 21. Норматив потребления коммунальных услуг - общедомовые нужды
                        x.IndividualMeteringDeviceStandard, // 22. Норматив потребления коммунальных услуг - индивидуальное потребление
                        x.IndividualMeteringDeviceCharge, // 23. К оплате за индивидуальное потребление коммунальной услуги, руб.
                        x.HouseMeteringDeviceCharge, // 24. К оплате за общедомовое потребление коммунальной услуги, руб.
                        x.CalculationOrder, // 25. Порядок расчётов
                        x.VolumeMethod // 26. Способ определения объемов КУ
                    }))
                .ToList();
        }

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный код",
                "Платежный документ",
                "Код услуги",
                "Тариф",
                "Итого к оплате за расчетный период",
                "Всего начислено за расчетный период ( без перерасчетов и льгот)",
                "Перерасчеты, корректировки (руб.)",
                "Льготы, субсидии, скидки (руб.)",
                "Тип предоставления услуги",
                "Сумма платы с учётом рассрочки платежа - от платы за расчётный период",
                "Сумма платы с учётом рассрочки платежа - от платы за предыдущие расчётные периоды",
                "Проценты за рассрочку, руб.",
                "Проценты за рассрочку, %",
                "Сумма к оплате с учётом рассрочки платежа и процентов за рассрочку",
                "Основания перерасчётов",
                "Сумма перерасчета , руб.",
                "Текущие показания приборов учёта коммунальных услуг - индивидуальное потребление",
                "Текущие показания приборов учёта коммунальных услуг - общедомовые нужды",
                "Суммарный объём коммунальных услуг в доме - индивидуальное потребление",
                "Суммарный объём коммунальных услуг в доме - общедомовые нужды",
                "Норматив потребления коммунальных услуг - общедомовые нужды",
                "Норматив потребления коммунальных услуг - индивидуальное потребление",
                "К оплате за индивидуальное потребление коммунальной услуги, руб.",
                "К оплате за общедомовое потребление коммунальной услуги, руб.",
                "Порядок расчётов",
                "Способ определения объемов КУ"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return new List<string>
            {
                "EPD",
                "DICTUSLUGA"
            };
        }
    }
}
