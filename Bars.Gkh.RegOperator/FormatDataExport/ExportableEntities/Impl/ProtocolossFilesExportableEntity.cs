namespace Bars.Gkh.RegOperator.FormatDataExport.ExportableEntities.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.FormatDataExport.ProxyEntities;

    /// <summary>
    /// Файлы протокола общего собрания собственников
    /// </summary>
    [Obsolete("СА: Не выгружаем", true)]
    public class ProtocolossFilesExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "PROTOCOLOSSFILES";

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            var entities = this.ProxySelectorFactory.GetSelector<ProtocolossProxy>()
                .ProxyListCache.Values
                .Where(x => x.AttachmentFile != null);

            return this.AddFilesToExport(entities, x => x.AttachmentFile)
                .Select(x => new ExportableRow(x.Id,
                    new List<string>
                    {
                        x.AttachmentFile.Id.ToStr(),
                        x.Id.ToStr(),
                        this.No // Протокол очного голосования
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
                "Уникальный идентификатор файла",
                "Уникальный идентификатор протокола голосования",
                "Тип"
            };
        }

        /// <inheritdoc />
        public override IList<string> GetInheritedEntityCodeList()
        {
            return new List<string>
            {
                //"PROTOCOLOSS"
            };
        }
    }
}