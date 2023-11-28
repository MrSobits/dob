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
    using Bars.Gkh.Utils;

    /// <summary>
    /// Размещение размера платы за жилое помещение по договору управления
    /// </summary>
    public class DuChargeExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "DUCHARGE";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Uo |
            FormatDataExportProviderFlags.Oms;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.ProxySelectorFactory.GetSelector<DuProxy>()
                .ProxyListCache.Values
                .Select(x => new ExportableRow(x.Id,
                    new List<string>
                    {
                        x.Id.ToStr(),
                        x.ChargeStatus.ToStr(),
                        x.Id.ToStr(),
                        this.GetDate(x.StartDatePaymentPeriod),
                        this.GetDate(x.EndDatePaymentPeriod),
                        this.GetDecimal(x.PaymentAmount),
                        this.GetStrId(x.PaymentProtocolFile),
                        x.ServiceId.ToStr(),
                        this.GetDecimal(x.ServicePayment),
                        x.SetPaymentsFoundation.ToStr(),
                        x.CancellationReason.Cut(1000)
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
                case 5:
                case 9:
                    return row.Cells[cell.Key].IsEmpty();
                case 8:
                    return row.Cells[7].IsNotEmpty() && row.Cells[cell.Key].IsEmpty();
                case 10:
                    if (row.Cells[1] == "2")
                    {
                        return row.Cells[cell.Key].IsEmpty();
                    }
                    break;
            }
            return false;
        };

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный код",
                "Статус",
                "Договор управления",
                "Дата начала периода",
                "Дата окончания периода",
                "Цена за услуги, работы по управлению МКД",
                "Протокол, которым утверждён размер платы",
                "Работа, услуга организации",
                "Размер платы за услугу организации",
                "Тип размера платы",
                "Причина аннулирования"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return new List<string>
            {
                "DU",
                "DICTUSLUGA"
            };
        }
    }
}