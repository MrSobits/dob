namespace Bars.Gkh.FormatDataExport.ExportableEntities.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;

    /// <summary>
    /// Подъезды
    /// </summary>
    public class EntranceExportableEntity : BaseExportableEntity<Entrance>
    {
        /// <inheritdoc />
        public override string Code => "ENTRANCE";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Uo |
            FormatDataExportProviderFlags.Rso |
            FormatDataExportProviderFlags.RegOpCr |
            FormatDataExportProviderFlags.Oms |
            FormatDataExportProviderFlags.Rc;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.GetFiltred(x => x.RealityObject)
                .Select(x => new
                {
                    x.Id,
                    RoId = (long?) x.RealityObject.Id,
                    x.Number,
                    x.RealityObject.MaximumFloors,
                    x.RealityObject.BuildYear
                })
                .AsEnumerable()
                .Select(x => new ExportableRow(x.Id,
                    new List<string>
                    {
                        x.Id.ToStr(),
                        x.RoId.ToStr(),
                        x.Number.ToStr(),
                        this.GetNotZeroValue(x.MaximumFloors),
                        this.GetFirstDateYear(x.BuildYear),
                        string.Empty // 6. Дата прекращения существования объекта
                    }))
                .ToList();
        }

        /// <inheritdoc />
        protected override IList<int> MandatoryFields => new List<int> { 0, 1, 2 };

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный код подъезда в системе отправителя",
                "Уникальный идентификатор дома",
                "Номер подъезда",
                "Этажность",
                "Дата постройки",
                "Дата прекращения существования объекта"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return new List<string>
            {
                "DOM"
            };
        }
    }
}