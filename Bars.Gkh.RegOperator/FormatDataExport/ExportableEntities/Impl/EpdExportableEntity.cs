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
    /// Платежные документы (epd.csv)
    /// </summary>
    public class EpdExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "EPD";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Uo |
                FormatDataExportProviderFlags.Rso |
                FormatDataExportProviderFlags.RegOpCr |
                FormatDataExportProviderFlags.Rc;

        /// <inheritdoc />
        protected override IList<int> MandatoryFields => new List<int> {0, 1, 3, 4, 5, 10};

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.ProxySelectorFactory.GetSelector<EpdProxy>()
                .ProxyListCache.Values
                .Select(x => new ExportableRow(x,
                    new List<string>
                    {
                        this.GetStrId(x),
                        x.DocumentType, //2.  Тип документа
                        x.ChargeFlag, //3.  Начисления за капитальный ремонт
                        this.GetDate(x.Date),
                        x.AccountId.ToStr(), //5.  Лицевой счет
                        x.PaymentReceiverAccountCode.ToStr(), //6.  Уникальный код расчетного счета получателя платежа
                        x.ResidentNumber, //7.  Количество проживающих
                        this.GetDecimal(x.LivingArea), //8. Жилая площадь
                        x.HeatedArea, //9.  Отапливаемая площадь
                        x.TotalArea, //10. Общая площадь для ЛС
                        x.StateFlag, //11. Статус
                        this.GetDecimal(x.Debt), //12. Задолженность за предыдущие периоды, руб.
                        this.GetDecimal(x.Overpayment), //13. Аванс на начало расчетного периода, руб.
                        this.GetDecimal(x.TotalCharge) //14. Сумма к оплате с учетом рассрочки платежа и процентов за рассрочку,руб.
                    }))
                .ToList();
        }

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный код",
                "Тип документа",
                "Начисления за капитальный ремонт",
                "Дата",
                "Лицевой счет",
                "Уникальный код расчетного счета получателя платежа",
                "Количество проживающих",
                "Жилая площадь",
                "Отапливаемая площадь",
                "Общая площадь для ЛС",
                "Статус",
                "Задолженность за предыдущие периоды, руб.",
                "Аванс на начало расчетного периода, руб.",
                "Сумма к оплате с учетом рассрочки платежа и процентов за рассрочку,руб."
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return new List<string>
            {
                "CONTRAGENTRSCHET",
                "KVAR"
            };
        }
    }
}