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

    /// <summary>
    /// Объекты управления устава
    /// </summary>
    public class UstavOuExportableEntity : BaseExportableEntity<RealityObject>
    {
        /// <inheritdoc />
        public override string Code => "USTAVOU";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Uo;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.ProxySelectorFactory.GetSelector<UstavOuProxy>()
                .ProxyListCache.Values
                .Select(x => new ExportableRow(x.Id,
                    new List<string>
                    {
                        x.Id.ToStr(),
                        x.ContractId.ToStr(),
                        x.RealityObjectId.ToStr(),
                        this.GetDate(x.StartDate),
                        this.GetDate(x.EndDate),
                        x.IsContractReason.ToStr(),
                        this.GetStrId(x.AttachmentFile),
                        this.GetDate(x.ExceptionManagementDate)
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
                case 5:
                    return row.Cells[cell.Key].IsEmpty();
                case 6:
                    if (row.Cells[5] == "2")
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
                "Устав",
                "Дом",
                "Дата начала предоставления услуг дому",
                "Дата окончания предоставления услуг дому",
                "Основанием является договор управления",
                "Файл с дополнительным соглашением",
                "Дата исключения объекта управления из ДУ"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return new List<string>
            {
                "USTAV",
                "DOM"
            };
        }
    }
}