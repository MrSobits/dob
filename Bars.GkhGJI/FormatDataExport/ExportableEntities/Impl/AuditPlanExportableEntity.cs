namespace Bars.GkhGji.FormatDataExport.ExportableEntities.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.SurveyPlan;

    /// <summary>
    /// Планы проверок юр. лиц ГЖИ
    /// </summary>
    public class AuditPlanExportableEntity : BaseExportableEntity<PlanJurPersonGji>
    {
        /// <inheritdoc />
        public override string Code => "AUDITPLAN";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Gji |
            FormatDataExportProviderFlags.Omjk;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            var surveyPlanContragentRepository = this.Container.ResolveRepository<SurveyPlanContragent>();
            var inspectionGjiRepository = this.Container.ResolveRepository<BaseJurPerson>();
            var proxySelectorService = this.ProxySelectorFactory.GetSelector<GjiProxy>();

            using (this.Container.Using(surveyPlanContragentRepository, inspectionGjiRepository))
            {
                var inspectionId = proxySelectorService.ProxyListCache.Keys.Single();

                return this.EntityRepository.GetAll()
                    .Select(x => new
                    {
                        x.Id,
                        x.DateStart,
                        x.UriRegistrationNumber,
                        x.DateApproval
                    })
                    .AsEnumerable()
                    .Select(x => new ExportableRow(
                        x.Id,
                        new List<string>
                        {
                            x.Id.ToStr(), // 1. Уникальный код
                            this.Yes, // 2. Признак подписания плана проверок для публикации в ГИС ЖКХ. (Передавать 1)
                            inspectionId.ToStr(), // 3. Проверяющая организация
                            x.DateStart?.Year.ToStr(), // 4. Год плана проверок
                            this.GetDate(x.DateApproval ?? x.DateStart), // 5. Дата утверждения плана проверок
                            string.Empty, // 6. Дополнительная информация
                            this.Yes,     // 7. Статус плана (Передавать 1)
                            x.UriRegistrationNumber.HasValue ? this.No : this.Yes, // 8. Не должен быть зарегистрирован в едином реестре проверок (Передавать 2)
                            x.UriRegistrationNumber.ToStr() // 9. Регистрационный номер плана в едином реестре проверок
                        }))
                    .ToList();
            }
        }

        /// <inheritdoc />
        protected override Func<KeyValuePair<int, string>, ExportableRow, bool> EmptyFieldPredicate { get; } = (cell, row) =>
        {
            switch (cell.Key)
            {
                case 0:
                case 2:
                case 3:
                case 4:
                case 6:
                case 7:
                    return row.Cells[cell.Key].IsEmpty();
                case 8:
                    return row.Cells[7] == "2" && row.Cells[cell.Key].IsEmpty();
            }

            return false;
        };

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный код",
                "Признак подписания плана проверок для публикации в ГИС ЖКХ.",
                "Проверяющая организация",
                "Год плана проверок",
                "Дата утверждения плана проверок",
                "Дополнительная информация",
                "Статус плана",
                "Не должен быть зарегистрирован в едином реестре проверок",
                "Регистрационный номер плана в едином реестре проверок"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return new List<string>
            {
                "GJI"
            };
        }
    }
}