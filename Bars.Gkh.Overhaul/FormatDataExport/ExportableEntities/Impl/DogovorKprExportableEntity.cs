namespace Bars.Gkh.Overhaul.FormatDataExport.ExportableEntities.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Договоры на выполнение работ по капитальному ремонту
    /// </summary>
    public class DogovorKprExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "DOGOVORKPR";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Uo |
            FormatDataExportProviderFlags.RegOpCr;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.ProxySelectorFactory.GetSelector<DogovorKprProxy>()
                .ExtProxyListCache
                .Select(x => new ExportableRow(x.Id,
                    new List<string>
                    {
                        x.Id.ToStr(),
                        x.KprId.ToStr(),
                        x.DocumentNumber.Cut(500),
                        this.GetDate(x.DocumentDate),
                        this.GetDate(x.StartDate),
                        this.GetDate(x.EndDate),
                        this.GetDecimal(x.Sum),
                        x.CustomerContragentId.ToStr(),
                        x.ExecutantContragentId.ToStr(),
                        x.IsGuaranteePeriod.ToStr(),
                        x.GuaranteePeriod.ToStr(),
                        x.IsBudgetDocumentation.ToStr(),
                        x.IsLawProvided.ToStr(),
                        x.InfoUrl.Cut(100),
                        x.Status.ToStr(),
                        x.RevocationReason,
                        x.RevocationDocumentNumber.Cut(500),
                        this.GetDate(x.RevocationDate)
                    }))
                .ToList();
        }

        /// <inheritdoc />
        protected override IList<int> MandatoryFields { get; } = new List<int> { 0, 1, 2, 3, 4, 5, 6, 9 };

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный код",
                "Код КПР",
                "Номер договора",
                "Дата договора",
                "Дата начала выполнения",
                "Дата окончания выполнения работ",
                "Сумма договора",
                "Заказчик",
                "Исполнитель",
                "Гарантийный срок установлен",
                "Гарантийный срок (кол-во месяцев)",
                "Наличие сметной документации",
                "Проведение отбора предусмотрено законодательством",
                "Адрес сайта с информацией об отборе",
                "Статус договора",
                "Причина расторжения",
                "Номер документа расторжения",
                "Дата документа расторжения"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return new List<string>
            {
                "KPR",
                "CONTRAGENT"
            };
        }
    }
}