namespace Bars.GkhGji.FormatDataExport.ExportableEntities.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Документы проверки ГЖИ
    /// </summary>
    public class AuditFilesExportableEntity : BaseExportableEntity<Disposal>
    {
        /// <inheritdoc />
        public override string Code => "AUDITFILES";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Gji |
            FormatDataExportProviderFlags.Omjk;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            var exportData = this.ProxySelectorFactory.GetSelector<DisposalFilesProxy>()
                .ProxyListCache.Values
                .Where(x => x.File != null);

            return this.AddFilesToExport(exportData, x => x.File)
                .Select(x => new ExportableRow(
                    x.Id,
                    new List<string>
                    {
                        x.Id.ToStr(), // 1. Уникальный код
                        x.DocumentGjiId.ToStr(), // 2. Распоряжение
                        this.Yes // 3. Тип файла (всегда передаем 1)
                    }))
                .ToList();
        }

        /// <inheritdoc />
        protected override IList<int> MandatoryFields => this.GetAllFieldIds();

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный код",
                "Проверка",
                "Тип файла"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return new List<string>
            {
                "AUDIT",
                "DOM",
                "CONTRAGENT"
            };
        }
    }
}