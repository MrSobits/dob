namespace Bars.GkhGji.FormatDataExport.ExportableEntities.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Дома в предписании
    /// </summary>
    public class PreceptHouseExportableEntity : BaseExportableEntity<Prescription>
    {
        /// <summary>
        /// Проверяемые дома
        /// </summary>
        public IDomainService<InspectionGjiRealityObject> InspectionGjiRealityObjectDomain { get; set; }

        /// <inheritdoc />
        public override string Code => "PRECEPTHOUSE";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Gji |
            FormatDataExportProviderFlags.Omjk;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            var query = this.EntityRepository.GetAll()
                .WhereNotNull(x => x.Inspection)
                .Select(x => new
                {
                        x.Id,
                        InspectionId = x.Inspection.Id,
                        x.TypeDocumentGji
                });

            var inspectionIdsQuery = query.Select(x => x.InspectionId);

            var roIdsDict = this.InspectionGjiRealityObjectDomain.GetAll()
                .Where(x => inspectionIdsQuery.Any(y => y == x.Inspection.Id))
                .Select(x => new
                    {
                        InspectionId = x.Inspection.Id,
                        RoId = (long?) x.RealityObject.Id
                    })
                .AsEnumerable()
                .GroupBy(x => x.InspectionId, x => x.RoId)
                .ToDictionary(x => x.Key, x => x.FirstOrDefault(y => y.HasValue));

            return query.AsEnumerable()
                .Select(x =>
                {
                    var roIdStr = roIdsDict.Get(x.InspectionId).ToStr();

                    return new ExportableRow(
                            x.Id,
                            new List<string>
                            {
                                x.Id.ToStr(), // 1. Уникальный код
                                roIdStr // 2. Дом
                            }
                        );
                })
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
                "Дом"
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