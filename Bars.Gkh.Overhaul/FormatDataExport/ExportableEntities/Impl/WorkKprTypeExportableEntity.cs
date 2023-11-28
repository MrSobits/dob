namespace Bars.Gkh.Overhaul.FormatDataExport.ExportableEntities.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Вид работ капитального ремонта
    /// </summary>
    public class WorkKprTypeExportableEntity : BaseExportableEntity<Work>
    {
        /// <inheritdoc />
        public override string Code => "WORKKPRTYPE";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Uo |
            FormatDataExportProviderFlags.RegOpCr;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.EntityRepository.GetAll()
                .Select(x => new ExportableRow(x.Id,
                    new List<string>
                    {
                        x.Id.ToStr(),
                        x.Name.Cut(100)
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
                "Наименование вида"
            };
        }
    }
}