namespace Bars.Gkh.FormatDataExport.NetworkWorker
{
    using System.Threading;

    using Bars.B4;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Интерфейс для сетевого взаимодействия экспорта данных
    /// </summary>
    public interface IFormatDataTransferService
    {
        /// <summary>
        /// Задать токен аутентификации
        /// </summary>
        /// <param name="gkhOperator">Оператор</param>
        bool SetToken(Operator gkhOperator);

        /// <summary>
        /// Получить статус операции
        /// </summary>
        IDataResult GetStatus(long id);

        /// <summary>
        /// Загрузить файл
        /// </summary>
        IDataResult UploadFile(string filePath, CancellationToken cancellationToken);

        /// <summary>
        /// Получить файл
        /// </summary>
        IDataResult GetFile(long fileId);

        /// <summary>
        /// Запустить задачу импорта переданных данных
        /// </summary>
        IDataResult StartImport(long fileId, CancellationToken cancellationToken);
    }
}