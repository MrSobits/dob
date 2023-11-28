﻿namespace Bars.Gkh.FormatDataExport.ExportableEntities
{
    using System.Collections.Generic;

    using Bars.Gkh.FormatDataExport.Enums;

    /// <summary>
    /// Группа экспортируемых сущностей
    /// </summary>
    public class ExportableEntityGroup : IExportableEntityGroup
    {
        /// <inheritdoc />
        public string Code { get; }

        /// <inheritdoc />
        public string Description { get; }

        /// <inheritdoc />
        public FormatDataExportProviderFlags AllowProviderFlags { get; }

        /// <inheritdoc />
        public IReadOnlyList<string> InheritedEntityCodeList { get; }

        public ExportableEntityGroup(string code, string description, IList<string> inheritedEtities, FormatDataExportProviderFlags allowProviderFlags)
        {
            this.Code = code;
            this.Description = description;
            this.InheritedEntityCodeList = new List<string>(inheritedEtities);
            this.AllowProviderFlags = allowProviderFlags;
        }
    }
}