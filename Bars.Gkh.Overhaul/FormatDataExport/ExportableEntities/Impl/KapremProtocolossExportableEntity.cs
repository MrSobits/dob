namespace Bars.Gkh.Overhaul.FormatDataExport.ExportableEntities.Impl
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
    /// Протоколы общего собрания собственников, которыми принято решение о формирования фонда капитального ремонта
    /// </summary>
    public class KapremProtocolossExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "KAPREMPROTOCOLOSS";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Uo |
            FormatDataExportProviderFlags.RegOpCr;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.ProxySelectorFactory.GetSelector<KapremProtocolossProxy>()
                .ExtProxyListCache
                .Select(x => new ExportableRow(x.Id,
                    new List<string>
                    {
                        x.Id.ToStr(),
                        x.ContragentId.ToStr(),
                        x.RealityObjectId.ToStr(),
                        x.Status.ToStr(),
                        x.SolutionReason.ToStr(),
                        x.MethodFormFundCr.ToStr(),
                        x.ProtocolossId.ToStr(),
                        x.ProtocolNumber.Cut(50),
                        x.SolutionName.Cut(250),
                        x.DocumentType.Cut(250),
                        x.DocumentNumber.Cut(50),
                        this.GetDate(x.ProtocolDate),
                        this.GetDate(x.DateStart),
                        x.RegopSchetId.ToStr(),
                        x.KapremProtocolFilesId.ToStr()
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
                case 6:
                case 7:
                case 8:
                    // одно из полей
                    return row.Cells[6].IsEmpty() && row.Cells[7].IsEmpty() && row.Cells[8].IsEmpty();
                case 9:
                case 10:
                    // если документ решения = решение ОМС
                    if (row.Cells[6] == "2")
                    {
                        return row.Cells[cell.Key].IsEmpty();
                    }
                    break;
                case 11:
                case 12:
                    return row.Cells[cell.Key].IsEmpty();
                case 13:
                case 14:
                    // если выбран спец. счет
                    if (row.Cells[7] == "1")
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
                "Уникальный идентификатор протокола",
                "Организация ответственная за передачу информации",
                "Адрес дома ",
                "Статус ",
                "Основание принятия решения",
                "Способ формирования фонда",
                "Протокол ОСС",
                "Номер протокола",
                "Наименование документа решения",
                "Вид документа",
                "Номер документа",
                "Дата протокола",
                "Дата вступления в силу ",
                "Расчетный счет",
                "Справка об открытии спец счета"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return new List<string>
            {
                "CONTRAGENT",
                "DOM",
                "CONTRAGENTRSCHET",
            };
        }
    }
}