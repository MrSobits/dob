namespace Bars.Gkh.FormatDataExport.ExportableEntities.ExportableFile
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Bars.B4.Config;
    using Bars.B4.Utils;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Экспортируемые файлы
    /// </summary>
    public class FilesExportableEntity : BaseFilesExportableEntity
    {
        private readonly string storagePath;

        public FilesExportableEntity(IConfigProvider configProvider)
        {
            this.storagePath = configProvider?.GetConfig()
                .ModulesConfig["Bars.B4.Modules.FileSystemStorage"]
                .GetAs("FileDirectory", string.Empty) ?? string.Empty;
        }

        /// <inheritdoc />
        public override IList<List<string>> GetData(DynamicDictionary baseParams)
        {
            var fileCollection = this.GetFiles();
            return fileCollection.Select(x => new List<string>
            {
                x.Id.ToStr(),
                x.FileName,
                x.FullName.Cut(100),
                x.Size.ToStr(),
                x.Description.Cut(500)
            })
            .ToList();
        }

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный идентификатор файла",
                "Уникальное наименование файла с расширением",
                "Наименование файла с расширением в системе отправителя",
                "Размер файла в байтах",
                "Описание файла"
            };
        }

        /// <inheritdoc />
        public override ICollection<ExportFileStream> GetFileStreams()
        {
            this.CleanFileStreams();
            this.Filestreams = this.GetFiles().AsParallel()
                .Select(x => new ExportFileStream(this.FileManager.GetFileStream(x, true)))
                .Where(x => x.FileStream != null)
                .ToList()
                .ToHashSet();

            return this.Filestreams;
        }

        private string GetFilePath(string fileName)
        {
            return Path.Combine(this.storagePath, fileName);
        }
    }
}