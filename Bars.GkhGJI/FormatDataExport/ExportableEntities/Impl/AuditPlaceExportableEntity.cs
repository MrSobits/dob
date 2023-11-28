namespace Bars.GkhGji.FormatDataExport.ExportableEntities.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Места проведения проверки
    /// </summary>
    public class AuditPlaceExportableEntity : BaseExportableEntity<InspectionGjiRealityObject>
    {
        /// <inheritdoc />
        public override string Code => "AUDITPLACE";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Gji |
            FormatDataExportProviderFlags.Omjk;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.EntityRepository.GetAll()
                .Where(x => x.RealityObject != null)
                .Where(x => x.Inspection != null)
                .Select(x => new
                {
                    x.Id,
                    InspectionId = x.Inspection.Id,
                    RoId = x.RealityObject.Id
                })
                .AsEnumerable()
                .Select(x => new ExportableRow(x.Id,
                    new List<string>
                    {
                        x.InspectionId.ToStr(), // 1. Проверка
                        x.Id.ToStr(),   // 2. Порядковый номер
                        x.RoId.ToStr(), // 3. Дом
                        string.Empty    // 4. Дополнительная информация
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
                "Проверка",
                "Порядковый номер",
                "Дом",
                "Дополнительная информация"
           };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return new List<string>
            {
                "AUDIT",
                "DOM"
            };
        }
    }
}