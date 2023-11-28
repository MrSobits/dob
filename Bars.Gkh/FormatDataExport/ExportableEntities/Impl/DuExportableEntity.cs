namespace Bars.Gkh.FormatDataExport.ExportableEntities.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Договоры управления многоквартирным домом
    /// </summary>
    public class DuExportableEntity : BaseExportableEntity<ManOrgContractOwners>
    {
        /// <inheritdoc />
        public override string Code => "DU";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Uo |
            FormatDataExportProviderFlags.Oms |
            FormatDataExportProviderFlags.RegOpWaste;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.ProxySelectorFactory.GetSelector<DuProxy>()
                .ProxyListCache.Values
                .Select(x => new ExportableRow(x,
                    new List<string>
                    {
                        x.Id.ToStr(),
                        x.ContragentId.ToStr(),
                        x.DocumentNumber.Cut(255),
                        this.GetDate(x.DocumentDate),
                        this.GetDate(x.StartDate),
                        this.GetDate(x.PlannedEndDate),
                        x.ValidityMonths.ToStr(),
                        x.ValidityYear.ToStr(),
                        x.NoticeNumber.Cut(20),
                        x.NoticeLink.Cut(1000),
                        x.Owner.ToStr(),
                        x.ContragentOwnerId.ToStr(),
                        x.ContragentOwnerType.ToStr(),
                        x.ContractFoundation.ToStr(),
                        x.InputMeteringDeviceValuesBeginDay.ToStr(),
                        x.InputMeteringDeviceValuesEndDay.ToStr(),
                        x.IsInputMeteringDeviceValuesLastDay.ToStr(),
                        x.DrawingPaymentDocumentDay.ToStr(),
                        x.IsDrawingPaymentDocumentLastDay.ToStr(),
                        x.DrawingPaymentDocumentMonth.ToStr(),
                        x.Status.ToStr(),
                        this.GetDate(x.TerminationDate),
                        x.TerminationReason.ToStr(),
                        x.CancellationReason.Cut(1000),
                        x.IsFormingApplications.ToStr()
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
                case 13:
                case 20:
                    return row.Cells[cell.Key].IsEmpty();
                case 10:
                case 11:
                    return row.Cells[10].IsEmpty() && row.Cells[11].IsEmpty();
                case 12:
                    return !string.IsNullOrEmpty(row.Cells[11]) && row.Cells[cell.Key].IsEmpty();
                case 15:
                case 16:
                    return row.Cells[15].IsEmpty() && row.Cells[16].IsEmpty();
                case 17:
                case 18:
                    return row.Cells[17].IsEmpty() && row.Cells[18].IsEmpty();
                case 21:
                case 22:
                    if (row.Cells[20] == "4") // Расторгнут
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
                "Уникальный код ",
                "Уникальный идентификатор Управляющей организации",
                "Номер документа",
                "Дата заключения",
                "Дата вступления в силу",
                "Планируемая дата окончания",
                "Срок действия (Месяц)",
                "Срок действия (Год/лет)",
                "Номер извещения",
                "Ссылка на извещение на официальном сайте в сети «Интернет» для размещения информации о проведении торгов",
                "Собственник объекта жилищного фонда",
                "Контрагент второй стороны договора",
                "Тип второй стороны договора",
                "Основание заключения договора",
                "День месяца начала ввода показаний по ПУ ",
                "День месяца окончания ввода показаний по ПУ ",
                "Последний день месяца ввода показаний по ПУ",
                "День месяца выставления платежных документов ",
                "Последний день месяца выставления платежных документов",
                "Месяц выставления платежных документов",
                "Статус ДУ",
                "Дата расторжения, прекращения действия договора управления",
                "Причина расторжения договора",
                "Причина аннулирования",
                "Формировать заявки в реестр лицензий, если сведения о лицензии/управляемом объекте отсутствуют"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return new List<string>
            {
                "UO",
                "CONTRAGENT"
            };
        }
    }
}