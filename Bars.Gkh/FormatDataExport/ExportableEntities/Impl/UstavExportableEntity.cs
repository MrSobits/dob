namespace Bars.Gkh.FormatDataExport.ExportableEntities.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Уставы
    /// </summary>
    public class UstavExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "USTAV";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Uo;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.ProxySelectorFactory.GetSelector<UstavProxy>()
                .ProxyListCache.Values
                .Select(x => new ExportableRow(x,
                new List<string>
                {
                    x.Id.ToStr(),
                    x.ContragentId.ToStr(),
                    x.DocumentNumber.Cut(255),
                    this.GetDate(x.DocumentDate),
                    x.InputMeteringDeviceValuesBeginDay.ToStr(),
                    x.InputMeteringDeviceValuesEndDay.ToStr(),
                    x.IsInputMeteringDeviceValuesLastDay.ToStr(),
                    x.DrawingPaymentDocumentDay.ToStr(),
                    x.IsDrawingPaymentDocumentLastDay.ToStr(),
                    x.ThisMonthPaymentDocDate.ToStr(),
                    x.PaymentServicePeriodDay.ToStr(),
                    x.IsPaymentServicePeriodLastDay.ToStr(),
                    x.ThisMonthPaymentServiceDate.ToStr(),
                    x.PhysicalContragentId.ToStr(),
                    x.LegalContragentId.ToStr(),
                    x.IsPhysicalOwner.ToStr(),
                    x.Management.Cut(1000),
                    x.Status.ToStr(),
                    x.TerminateReason.Cut(1000),
                    this.GetDate(x.TerminationDate),
                    x.ContractStopReason.Cut(255),
                    x.IsFormingApplications.ToStr(),
                    x.IsManagementContract.ToStr()
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
                case 16:
                case 17:
                    return row.Cells[cell.Key].IsEmpty();
                case 13:
                case 14:
                    return row.Cells[13].IsEmpty() && row.Cells[14].IsEmpty();
            }
            return false;
        };

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный код ",
                "Уникальный идентификатор Управляющей организации/ТСЖ",
                "Номер документа",
                "Дата регистрации ТСН/ТСЖ/кооператива (Организации Поставщика данных)",
                "День месяца начала ввода показаний по ПУ ",
                "День месяца окончания ввода показаний по ПУ ",
                "Последний день месяца ввода показаний по ПУ",
                "День месяца выставления платежных документов ",
                "Последний день месяца выставления платежных документов",
                "Месяц выставления платежных документов",
                "День месяца внесения платы за ЖКУ ",
                "Последний день месяца внесения платы за ЖКУ",
                "Месяц внесения платы за ЖКУ",
                "Единоличный исполнительный орган (Физическое лицо)",
                "Единоличный исполнительный орган (Юридическое лицо)",
                "Физическое лицо является собственником",
                "Состав органов управления/Правление",
                "Статус устава",
                "Причина аннулирования",
                "Дата расторжения, прекращения действия устава",
                "Причина прекращения действия устава (расторжения)",
                "Формировать заявки в реестр лицензий, если сведения о лицензии/управляемом объекте отсутствуют",
                "Управление многоквартирным домом осуществляется управляющей организацией по договору управления"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return new List<string>
            {
                "UO"
            };
        }
    }
}
