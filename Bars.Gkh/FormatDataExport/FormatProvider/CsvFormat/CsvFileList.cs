namespace Bars.Gkh.FormatDataExport.FormatProvider.CsvFormat
{
    using System.Collections.Generic;

    /// <summary>
    /// Список экспортируемых файлов
    /// </summary>
    public class CsvFileList
    {
        private const string FileListName = "_filelist";

        private readonly IList<IList<string>> fileList = new List<IList<string>>();

        private readonly IList<string> header = new List<string>()
        {
            "Наименование файла",
            "Количество строк данных",
            "Контрольная строка"
        };

        /// <summary>
        /// Добавить информацию о файле
        /// </summary>
        public void Add(CsvFileData fileData)
        {
            this.fileList.Add(new List<string>
            {
                fileData.FullName,
                fileData.RowNumber.ToString(),
                fileData.HashSum
            });
        }

        /// <summary>
        /// Получить csv файл
        /// </summary>
        public CsvFileData GetCsvFileData()
        {
            return CsvHelper.GetContent(CsvFileList.FileListName, this.header, this.fileList);
        }
    }
}