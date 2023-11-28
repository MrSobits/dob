namespace Bars.Gkh.FormatDataExport.ProxyEntities
{
    using Bars.B4.DataModels;
    using Bars.B4.Modules.FileStorage;

    /// <summary>
    /// Файлы акта проверки
    /// </summary>
    public class ActCheckFilesProxy : IHaveId
    {
        /// <summary>
        /// 1. Уникальный идентификатор файла
        /// </summary>
        public long Id => this.File.Id;

        /// <summary>
        /// 1. Уникальный идентификатор файла
        /// </summary>
        public FileInfo File { get; set; }

        /// <summary>
        /// 2. Уникальный идентификатор акта проверки
        /// </summary>
        public long DocumentGjiId { get; set; }

        /// <summary>
        /// 3. Тип файла
        /// </summary>
        public int Type { get; set; }
    }
}