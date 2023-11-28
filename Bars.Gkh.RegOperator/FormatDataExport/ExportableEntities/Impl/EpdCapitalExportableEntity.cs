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
    /// Начисления по капитальному ремонту
    /// </summary>
    public class EpdCapitalExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "EPDCAPITAL";

        /// <inheritdoc />
        protected override IList<int> MandatoryFields { get; } = new List<int> {0, 1, 3, 4, 5, 6, 7};

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags => FormatDataExportProviderFlags.Uo
            | FormatDataExportProviderFlags.RegOpCr
            | FormatDataExportProviderFlags.Rc;

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный код",
                "Платежный документ",
                "Дата",
                "Размер взноса на кв. метр (руб.)",
                "Всего начислено за расчётный период (руб.)",
                "Перерасчеты, корректировки (руб.)",
                "Льготы, субсидии, скидки (руб.)",
                "Итого к оплате за расчётный период (руб.)"
            };
        }

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.ProxySelectorFactory.GetSelector<EpdProxy>()
                .ProxyListCache.Values
                .Select(x => new ExportableRow(x,
                    new List<string>
                    {
                        this.GetStrId(x), //1.Уникальный код
                        x.SnapshotIdCapital.ToStr(), // 2.Платежный документ
                        this.GetDate(x.DateCapital), // 3.Дата
                        this.GetDecimal(x.TariffCapital), // 4.Размер взноса на кв.метр(руб.)
                        this.GetDecimal(x.ChargeCapital), // 5.Всего начислено за расчётный период(руб.)
                        this.GetDecimal(x.ChangeCapital), // 6.Перерасчеты, корректировки(руб.)
                        this.GetDecimal(x.BenefitCapital), // 7.Льготы, субсидии, скидки(руб.)
                        this.GetDecimal(x.SaldoOutCapital) // 8.Итого к оплате за расчётный период(руб.)
                    }))
                .ToList();
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return new List<string>
            {
                "EPD"
            };
        }
    }
}