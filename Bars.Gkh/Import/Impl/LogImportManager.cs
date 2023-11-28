namespace Bars.Gkh.Import
{
    using System;
    using System.IO;
    using System.Text;

    using Bars.B4;
    using Bars.B4.IoC.Lifestyles.SessionLifestyle;
    using Bars.B4.Logging;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.Tasks.Common.Entities;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums.Import;

    using Castle.Windsor;

    using Ionic.Zip;
    using Ionic.Zlib;

    /// <summary>
    /// Сервис для работы с <see cref="LogImport"/>
    /// </summary>
    public class LogImportManager : ILogImportManager
    {
        private ZipFile logsZip;

        private ZipFile filesZip;

        /// <summary>
        /// Key импорта
        /// </summary>
        private string importKey;

        /// <summary>
        /// общее количество измененных строк
        /// </summary>
        private int сountChangedRows;

        /// <summary>
        /// общее количество созданных строк
        /// </summary>
        private int countImportedRows;

        private bool disposed;

        /// <summary>
        /// .ctor
        /// </summary>
        public LogImportManager()
        {
            this.logsZip = new ZipFile(Encoding.UTF8)
            {
                CompressionLevel = CompressionLevel.Level3,
                AlternateEncoding = Encoding.GetEncoding("cp866")
            };

            this.filesZip = new ZipFile(Encoding.UTF8)
            {
                CompressionLevel = CompressionLevel.Level3,
                AlternateEncoding = Encoding.GetEncoding("cp866")
            };
        }

        /// <summary>
        /// Деструктор
        /// </summary>
        ~LogImportManager()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// IoC
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// общее количество ошибок
        /// </summary>
        public int CountError { get; private set; }

        /// <summary>
        /// общее количество предупреждений
        /// </summary>
        public int CountWarning { get; private set; }

        /// <summary>
        /// Идентификатор файла
        /// </summary>
        public long LogFileId { get; private set; }

        /// <summary>
        /// Название файла
        /// </summary>
        public string FileNameWithoutExtention { get; set; }

        /// <summary>
        /// Количество импортируемых файлов
        /// </summary>
        public int CountImportedFile { get; set; }

        /// <summary>
        /// Дата загрузки
        /// </summary>
        public DateTime UploadDate { get; set; }

        /// <summary>
        /// Задача, которая разбирала импорт, на сервере вычислений
        /// </summary>
        public TaskEntry Task { get; set; }

        /// <summary>
        /// Добавляем импортируемый файл и его лог
        /// </summary>
        /// <param name="file">Импортируемый файл</param>
        /// <param name="fileName">Наименование файла с расширением</param>
        /// <param name="log"></param>
        public void Add(Stream file, string fileName, ILogImport log)
        {
            this.ThrowIfDisposed();

            if (fileName.Split('.').Length == 0)
            {
                throw new Exception("Не задано расширение файла");
            }

            this.CountImportedFile++;
            this.AddFile(file, fileName);
            this.AddLogInternal(log);
        }

        /// <summary>
        /// Добавить
        /// </summary>
        public void Add(FileData file, ILogImport log)
        {
            this.ThrowIfDisposed();

            using (var stream = new MemoryStream(file.Data))
            {
                this.Add(stream, $"{file.FileName}.{file.Extention}", log);
            }
        }

        /// <summary>
        /// Добавляем лог, использовать если не нужно добавлять сам файл импорта
        /// </summary>
        /// <param name="log"></param>
        public void AddLog(ILogImport log)
        {
            this.ThrowIfDisposed();

            this.CountImportedFile++;
            this.AddLogInternal(log);
        }

        /// <summary>
        /// Сохранить лог в систему
        /// </summary>
        public long Save()
        {
            this.ThrowIfDisposed();

            var oldScope = ExplicitSessionScope.EnterNewScope();

            var userManager = this.Container.Resolve<IGkhUserManager>();

            try
            {
                var activeOperator = userManager.GetActiveOperator();
                var login = userManager.GetActiveUser()?.Login ?? "anonymous";

                using (var logFile = new MemoryStream())
                {
                    this.logsZip.Save(logFile);

                    using (var file = new MemoryStream())
                    {
                        this.filesZip.Save(file);

                        var fileManager = this.Container.Resolve<IFileManager>();

                        // Сохраняем логи в файловом хранилище
                        var logFileInfo = fileManager.SaveFile(logFile, $"{this.FileNameWithoutExtention}.log.zip");

                        var fileInfo = fileManager.SaveFile(file, $"{this.FileNameWithoutExtention}.zip");

                        var logEntity = new LogImport
                        {
                            UploadDate = this.UploadDate == DateTime.MinValue ? DateTime.Now : this.UploadDate,
                            CountImportedFile = this.CountImportedFile,
                            CountImportedRows = this.countImportedRows,
                            CountChangedRows = this.сountChangedRows,
                            CountError = this.CountError,
                            CountWarning = this.CountWarning,
                            File = fileInfo,
                            LogFile = logFileInfo,
                            FileName = this.FileNameWithoutExtention,
                            ImportKey = this.importKey,
                            Operator = activeOperator,
                            Login = login,
                            Task = this.Task
                        };

                        var repLogImport = this.Container.Resolve<IDomainService<LogImport>>();
                        repLogImport.Save(logEntity);

                        this.LogFileId = logFileInfo.Id;
                    }
                }
            }
            catch (Exception exp)
            {
                this.Container.Resolve<ILogManager>().Error("Произошла ошибка при создании лога импорта".Localize(), exp);
            }
            finally
            {
                this.Container.Release(userManager);

                ExplicitSessionScope.LeaveScope(oldScope);
                this.Clear();
            }

            return this.LogFileId;
        }

        /// <summary>
        /// Получить информацию
        /// </summary>
        public string GetInfo()
        {
            var msg = string.Empty;

            if (this.CountImportedFile > 1)
            {
                msg = $"Количество загруженных файлов:{this.CountImportedFile};";
            }

            msg += $"Загружено:{this.countImportedRows}; Изменено: {this.сountChangedRows}; Предупреждений: {this.CountWarning}; Ошибок: {this.CountError}";

            return msg;
        }

        /// <summary>
        /// Вернуть статус импорта
        /// </summary>
        /// <returns></returns>
        public StatusImport GetStatus()
        {
            return this.CountError > 0
                ? StatusImport.CompletedWithError
                : (this.CountWarning > 0 ? StatusImport.CompletedWithWarning : StatusImport.CompletedWithoutError);
        }

        /// <summary>
        /// Освободить
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
        }

        /// <summary>
        /// Добавляем импортируемый файл
        /// </summary>
        private void AddFile(Stream file, string fileName)
        {
            file.Seek(0, SeekOrigin.Begin);

            var buffer = new byte[file.Length];

            file.Seek(0, SeekOrigin.Begin);
            file.Read(buffer, 0, buffer.Length);
            file.Seek(0, SeekOrigin.Begin);

            this.filesZip.AddEntry(fileName, buffer);
        }

        private void AddLogInternal(ILogImport log)
        {
            log.PlacingResults();

            this.сountChangedRows += log.CountChangedRows;
            this.CountError += log.CountError;
            this.CountWarning += log.CountWarning;
            this.countImportedRows += log.CountAddedRows;

            if (string.IsNullOrEmpty(log.ImportKey))
            {
                throw new Exception("Не задан key импорта");
            }

            this.importKey = log.ImportKey;

            var fileStream = log.GetFile();
            var buffer = new byte[fileStream.Length];

            fileStream.Seek(0, SeekOrigin.Begin);
            fileStream.Read(buffer, 0, buffer.Length);
            fileStream.Seek(0, SeekOrigin.Begin);

            this.logsZip.AddEntry(string.IsNullOrEmpty(log.FileName) ? this.FileNameWithoutExtention + ".log" : log.FileName, buffer);

            var errorRowsStream = log.GetErrorRows();
            if (errorRowsStream.IsNotNull())
            {
                errorRowsStream.Seek(0, SeekOrigin.Begin);
                this.logsZip.AddEntry(this.FileNameWithoutExtention + "_errors.csv", errorRowsStream);
            }
        }

        private void Clear()
        {
            if (this.logsZip != null)
            {
                this.logsZip.Dispose();
            }

            if (this.filesZip != null)
            {
                this.filesZip.Dispose();
            }

            this.logsZip = null;
            this.filesZip = null;
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // no managed resources to dispose
            }

            this.Clear();

            this.disposed = true;
        }

        private void ThrowIfDisposed()
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException("LogImportManager", "Log manager is disposed");
            }
        }
    }
}