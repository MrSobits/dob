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
    /// Оплаты ЖКУ
    /// </summary>
    public class OplataExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "OPLATA";

        /// <inheritdoc />
        protected override IList<int> MandatoryFields { get; } = new List<int> {0, 1, 2, 3, 4, 6, 9, 10, 14};

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags => FormatDataExportProviderFlags.Uo
            | FormatDataExportProviderFlags.Rso
            | FormatDataExportProviderFlags.RegOpCr
            | FormatDataExportProviderFlags.Rc;

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный код оплаты",
                "Уникальный код лицевого счета в системе отправителя",
                "Тип операции",
                "Номер платежного документа(распоряжения)",
                "Дата оплаты",
                "Дата учета",
                "Сумма оплаты",
                "Источник оплаты",
                "Месяц, за который произведена оплата",
                "Уникальный код пачки оплат",
                "Расчетный счет получателя",
                "Платежный документ",
                "Уникальный идентификатор плательщика",
                "Наименование плательщика",
                "Назначение платежа",
                "Произвольный комментарий"
            };
        }

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.ProxySelectorFactory.GetSelector<OplataProxy>()
                .ProxyListCache.Values
                .Select(x => new ExportableRow(x,
                    new List<string>
                    {
                        this.GetStrId(x), // 1. Уникальный код оплаты
                        x.KvarId.ToStr(), // 2. Уникальный код лицевого счета в системе отправителя
                        x.OperationType.ToStr(), // 3. Тип операции
                        x.DocumentNumber, // 4. Номер платежного документа(распоряжения)
                        this.GetDate(x.PaymentDate), // 5. Дата оплаты
                        this.GetDate(x.OperationDate), // 6. Дата учета
                        this.GetDecimal(x.Amount), // 7. Сумма оплаты
                        string.Empty, // 8. Источник оплаты
                        string.Empty, // 9. Месяц, за который произведена оплата
                        x.OplataPackId.ToStr(), // 10. Уникальный код пачки оплат
                        x.ContragentRschetId.ToStr(), // 11. Расчетный счет получателя
                        x.EpdId.ToStr(), // 12. Платежный документ
                        x.IndId.ToStr(), // 13. Уникальный идентификатор плательщика
                        x.PayerName.ToStr(), // 14. Наименование плательщика
                        x.Destination.ToStr(), // 15. Назначение платежа
                        string.Empty // 16. Произвольный комментарий
                    }))
                .ToList();
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return new List<string>
            {
                "KVAR",
                "CONTRAGENTRSCHET",
                "EPD",
                "IND",
                "OPLATAPACK"
            };
        }
    }
}