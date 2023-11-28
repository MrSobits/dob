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
    using Bars.GkhCr.Entities;

    /// <summary>
    /// Работы договора на выполнение работ по капитальному ремонту
    /// </summary>
    public class WorkDogovExportableEntity : BaseExportableEntity<BuildContractTypeWork>
    {
        /// <inheritdoc />
        public override string Code => "WORKDOGOV";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Uo |
            FormatDataExportProviderFlags.RegOpCr;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.ProxySelectorFactory.GetSelector<WorkDogovProxy>()
                .ExtProxyListCache
                .Select(x => new ExportableRow(x.Id,
                    new List<string>
                    {
                        x.Id.ToStr(),
                        x.DogovorKprId.ToStr(),
                        x.KprId.ToStr(),
                        this.GetDate(x.StartDate),
                        this.GetDate(x.EndDate),
                        this.GetDecimal(x.ContractAmount),
                        this.GetDecimal(x.KprAmount),
                        this.GetDecimal(x.WorkVolume),
                        x.Okei,
                        x.AnotherUnit.Cut(50),
                        x.Description.Cut(500)
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
                    return row.Cells[cell.Key].IsEmpty();
                case 8:
                case 9:
                    return row.Cells[8].IsEmpty() && row.Cells[9].IsEmpty();
            }
            return false;
        };

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный код",
                "Код договора на выполнение работ по капитальному ремонту  ",
                "Код работы в КПР",
                "Дата начала выполнения работы",
                "Дата окончания выполнения работы",
                "Стоимость работы в договоре",
                "Стоимость работы в КПР",
                "Объём работы",
                "Код ОКЕИ",
                "Другая единица измерения",
                "Дополнительная информация"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return new List<string>
            {
                "DOGOVORKPR",
                "WORKKPR"
            };
        }
    }
}