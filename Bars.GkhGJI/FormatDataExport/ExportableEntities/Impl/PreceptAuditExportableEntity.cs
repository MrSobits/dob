namespace Bars.GkhGji.FormatDataExport.ExportableEntities.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Предписание проверки ГЖИ
    /// </summary>
    public class PreceptAuditExportableEntity : BaseExportableEntity<Prescription>
    {  
        /// <summary>
        /// Нарушения
        /// </summary>
        public IRepository<PrescriptionViol> PrescriptionViolRepository { get; set; }

        /// <inheritdoc />
        public override string Code => "PRECEPTAUDIT";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Gji |
            FormatDataExportProviderFlags.Omjk;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            var violationDict = this.PrescriptionViolRepository.GetAll()
                .Where(x => x.DateFactRemoval != null)
                .Select(x => new
                {
                    x.Document.Id,
                    x.DateFactRemoval,
                    x.DatePlanRemoval
                })
                .AsEnumerable()
                .GroupBy(x => x.Id, x => x.DateFactRemoval)
                .ToDictionary(x => x.Key, x => x.OrderBy(y => y).First());

            var query = this.EntityRepository.GetAll()
                .WhereNotNull(x => x.Inspection)
                .Select(x => new
                {
                    x.Id,
                    InspectionId = x.Inspection.Id,
                    x.DocumentNumber,
                    x.DocumentDate
                });

            return query.AsEnumerable()
                .Select(x =>
                {
                    var dateFactRemoval = this.GetDate(violationDict.Get(x.Id));
                    var datePlanRemoval = this.GetDate(violationDict.Get(x.Id));

                    var stateCode = dateFactRemoval.IsNotEmpty() ? "1" : "2";
                    
                    return new ExportableRow(x.Id,
                        new List<string>
                        {
                            x.Id.ToStr(), // 1. Уникальный код
                            x.InspectionId.ToStr(), // 2. Проверка
                            this.Yes, // 3. Статус предписания
                            x.DocumentNumber.Cut(255),    // 4. Номер документа
                            this.GetDate(x.DocumentDate), // 5. Дата документа
                            datePlanRemoval, // 6. Срок исполнения требований
                            string.Empty,    // 7. Краткая информация
                            stateCode,       // 8. Статус исполнения предписания
                            dateFactRemoval, // 9. Фактическая дата исполнения
                            string.Empty, // 10. Причина отмены документа
                            string.Empty, // 11. Дата отмены документа
                            string.Empty, // 12. Организация, принявшая решение об отмене
                            string.Empty, // 13. Номер решения об отмене
                            string.Empty  // 14. Дополнительная информация
                        }
                    );
                })
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
                case 7:
                case 9:
                case 10:
                case 11:
                    return row.Cells[cell.Key].IsEmpty();
            }
            return false;
        };

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный код",
                "Проверка",
                "Статус предписания",
                "Номер документа",
                "Дата документа",
                "Срок исполнения требований",
                "Краткая информация",
                "Статус исполнения предписания",
                "Фактическая дата исполнения",
                "Причина отмены документа",
                "Дата отмены документа",
                "Организация, принявшая решение об отмене",
                "Номер решения об отмене",
                "Дополнительная информация"
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