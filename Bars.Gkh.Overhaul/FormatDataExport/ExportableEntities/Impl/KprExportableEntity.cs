namespace Bars.Gkh.Overhaul.FormatDataExport.ExportableEntities.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.Utils;
    using Bars.GkhCr.Entities;

    /// <summary>
    /// КПР
    /// </summary>
    public class KprExportableEntity : BaseExportableEntity<ProgramCr>
    {
        /// <inheritdoc />
        public override string Code => "KPR";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Uo |
            FormatDataExportProviderFlags.RegOpCr;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            var regionCode = this.Container.Resolve<IRegionCodeService>().GetRegionCode();

            return this.EntityRepository.GetAll()
                .Select(x => new ExportableRow(x.Id,
                    new List<string>
                    {
                        x.Id.ToStr(),
                        x.Name.Cut(500),
                        regionCode,
                        string.Empty,
                        this.GetDate(x.Period.DateStart),
                        this.GetDate(x.Period.DateEnd)
                    }))
                .ToList();
        }

        /// <inheritdoc />
        protected override IList<int> MandatoryFields { get; } = new List<int> { 0, 2 };

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный код",
                "Наименование",
                "Код региона реализации",
                "Код ОКТМО",
                "Месяц и год начала периода реализации",
                "Месяц и год окончания периода реализации"
            };
        }
    }
}