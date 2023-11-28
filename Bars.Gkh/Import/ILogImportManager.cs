namespace Bars.Gkh.Import
{
    using System;
    using System.IO;

    using Bars.B4;
    using Bars.Gkh.Enums.Import;
    using Bars.B4.Modules.Tasks.Common.Entities;

    public interface ILogImportManager : IDisposable
    {
        /// <summary>
        /// Общее количество ошибок
        /// </summary>
        int CountError { get; }

        /// <summary>
        /// Общее количество Предупреждений
        /// </summary>
        int CountWarning { get; }
        
        /// <summary>
        /// Дата загрузки
        /// </summary>
        DateTime UploadDate { get; set; }

        /// <summary>
        /// Идентификационные данные лога (заполнение происходит на методе Save)
        /// </summary>
        long LogFileId { get; }

        /// <summary>
        /// Наименование файла
        /// </summary>
        string FileNameWithoutExtention { get; set; }

        /// <summary>
        /// Количество файлов которые импортировались
        /// </summary>
        int CountImportedFile { get; set; }

        /// <summary>
        /// Задача, которая разбирала импорт, на сервере вычислений
        /// </summary>
        TaskEntry Task { get; set; }

        /// <summary>
        /// Добавить импортируемый файл и его лог
        /// </summary>
        /// <param name="file">Импортируемый файл</param>
        /// <param name="fileName">Наименование импортируемого файла</param>
        /// <param name="log">Лог импортируемого файла</param>
        void Add(Stream file, string fileName, ILogImport log);

        /// <summary>
        /// Добавить импортируемый файл и его лог
        /// </summary>
        /// <param name="file">Импортируемый файл</param>
        /// <param name="log">Лог импортируемого файла</param>
        void Add(FileData file, ILogImport log);

        /// <summary>
        ///  Добавить лог, использовать если не нужно добавлять сам файл импорта
        /// </summary>
        /// <param name="log">Лог</param>
        void AddLog(ILogImport log);

        /// <summary>
        /// Сохранить логи
        /// </summary>
        /// <returns>
        /// Идентификатор файла лога <see cref="int"/>.
        /// </returns>
        long Save();

        /// <summary>
        /// Вернуть текстовую информацию для пользователя
        /// </summary>
        /// <returns></returns>
        string GetInfo();

        /// <summary>
        /// Вернуть статус импорта
        /// </summary>
        /// <returns></returns>
        StatusImport GetStatus();
    }
}
