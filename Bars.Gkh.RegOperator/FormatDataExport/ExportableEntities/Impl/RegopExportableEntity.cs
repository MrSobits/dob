namespace Bars.Gkh.RegOperator.FormatDataExport.ExportableEntities.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.Modules.RegOperator.Entities.RegOperator;

    /// <summary>
    /// Региональные операторы капитального ремонта
    /// </summary>
    public class RegopExportableEntity : BaseExportableEntity<RegOperator>
    {
        /// <inheritdoc />
        public override string Code => "REGOP";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.RegOpCr;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.GetFiltred(x => x.Contragent)
                .Select(x => new ExportableRow(x.Contragent,
                    new List<string>
                    {
                        this.GetStrId(x.Contragent),
                        string.Empty, // 2. Номер основания наделения полномочиями
                        string.Empty // 3. Дата основания наделения полномочиями
                    }))
                .ToList();
        }

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Контрагент",
                "Номер основания наделения полномочиями",
                "Дата основания наделения полномочиями"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return new List<string>
            {
                "CONTRAGENT"
            };
        }
    }
}