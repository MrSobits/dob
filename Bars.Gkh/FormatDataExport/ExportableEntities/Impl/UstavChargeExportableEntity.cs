namespace Bars.Gkh.FormatDataExport.ExportableEntities.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.FormatDataExport.ProxyEntities;

    /// <summary>
    /// Размещение сведений о размера платы за жилое помещение по уставу
    /// </summary>
    public class UstavChargeExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "USTAVCHARGE";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Uo;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.ProxySelectorFactory.GetSelector<UstavProxy>()
                .ProxyListCache.Values
                .Select(x => new ExportableRow(x.Id,
                    new List<string>
                    {
                        x.Id.ToStr(),
                        x.Id.ToStr(),
                        x.PaymentInfo.ToStr(),
                        this.GetDate(x.StartDatePaymentPeriod),
                        this.GetDate(x.EndDatePaymentPeriod),
                        this.GetDecimal(x.CompanyReqiredPaymentAmount),
                        this.GetDecimal(x.ReqiredPaymentAmount),
                        x.ServiceId.ToStr(),
                        this.GetDecimal(x.ServicePayment),
                    }))
                .ToList();
        }

        /// <inheritdoc />
        protected override Func<KeyValuePair<int, string>, ExportableRow, bool> EmptyFieldPredicate { get; } = (cell, row) =>
        {
            switch (cell.Key)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                    return row.Cells[cell.Key].IsEmpty();
                case 8:
                    return row.Cells[7].IsNotEmpty() && row.Cells[cell.Key].IsEmpty();
            }
            return false;
        };

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный код",
                "Устав",
                "Информация о размере обязательных платежей",
                "Дата начала периода",
                "Дата окончания периода",
                "Размер обязательных платежей членов ТСЖ",
                "Размер платы за содержание и ремонт помещений",
                "Работа, услуга организации",
                "Размер платы за услугу организации"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return new List<string>
            {
                "USTAV"
            };
        }
    }
}