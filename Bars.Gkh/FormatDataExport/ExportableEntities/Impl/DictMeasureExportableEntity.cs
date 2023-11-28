namespace Bars.Gkh.FormatDataExport.ExportableEntities.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Справочник единиц измерения
    /// </summary>
    public class DictMeasureExportableEntity : BaseExportableEntity<UnitMeasure>
    {
        /// <inheritdoc />
        public override string Code => "DICTMEASURE";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Uo |
            FormatDataExportProviderFlags.Rso |
            FormatDataExportProviderFlags.Rc;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.EntityRepository.GetAll()
                .Select(x => new ExportableRow(x,
                    new List<string>
                    {
                        this.GetStrId(x),
                        x.Name.Cut(60),
                        x.ShortName.Cut(40),
                        string.Empty,
                        string.Empty
                    }))
                .ToList();
        }

        /// <inheritdoc />
        protected override IList<int> MandatoryFields { get; } = new List<int> { 0, 1, 2 };

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный код единицы измерения в системе отправителя",
                "Наименование",
                "Сокращенное обозначение",
                "Код по ОКЕИ",
                "Код базовой единицы измерения"
            };
        }
    }
}