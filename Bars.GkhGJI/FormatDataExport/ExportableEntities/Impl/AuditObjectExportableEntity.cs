﻿namespace Bars.GkhGji.FormatDataExport.ExportableEntities.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Предметы проверки
    /// </summary>
    public class AuditObjectExportableEntity : BaseExportableEntity<InspectionGji>
    {
        /// <inheritdoc />
        public override string Code => "AUDITOBJECT";

        /// <inheritdoc />
        public override FormatDataExportProviderFlags AllowProviderFlags =>
            FormatDataExportProviderFlags.Gji |
            FormatDataExportProviderFlags.Omjk;

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            return this.EntityRepository.GetAll()
                .Select(x => new ExportableRow(x.Id,
                    new List<string>
                    {
                        x.Id.ToStr(), // 1. Проверка
                        this.No       // 2. Предмет проверки (передаем с кодом 2)
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
                "Проверка",
                "Предмет проверки"
           };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return new List<string>
            {
                "AUDIT"
            };
        }
    }
}