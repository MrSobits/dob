namespace Bars.B4.Modules.FIAS.AutoUpdater.DownloadService.Impl
{
    using Bars.B4.Config;
    using Bars.B4.IoC;
    using Bars.B4.Logging;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Castle.Core;
    using Castle.Windsor;
    using System;
    using System.IO;
    using System.Net;
    using System.ServiceModel;
    using System.Threading;

    /// <summary>
    /// Сервис загрузки обновлений ФИАС
    /// </summary>
    internal class FiasDownloadService : IFiasDownloadService, IInitializable
    {
        #region Fields

        private DownloadServiceClient client;        

        private bool isDisposed = false;

        private string workDirectory;

        #endregion

        #region Properties

        public IWindsorContainer Container { get; set; }

        public ILogManager LogManager { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Вызывается кастлом при создании
        /// </summary>
        public void Initialize()
        {
            var configProvider = this.Container.Resolve<IConfigProvider>();

            using (this.Container.Using(configProvider))
            {
                var config = configProvider.GetConfig().GetModuleConfig("Bars.B4.Modules.FIAS.AutoUpdater");
                var endpointAddress = config.GetAs("EndpointAddress", default(string), true);

                if (endpointAddress == null)
                {
                    throw new ArgumentNullException($"Не указан адрес сервера обновления ФИАС в \"b4.user.config\"");
                }

                this.workDirectory = config.GetAs("WorkDirectory", default(string), true);
                if (string.IsNullOrWhiteSpace(this.workDirectory))
                {
                    this.workDirectory = Path.GetTempPath();
                }
                var binding = new BasicHttpsBinding();
                binding.MaxReceivedMessageSize = Int32.MaxValue;
                binding.MaxBufferPoolSize = Int32.MaxValue;
                binding.ReaderQuotas = new System.Xml.XmlDictionaryReaderQuotas
                {
                    MaxDepth = 32,
                    MaxArrayLength = Int32.MaxValue
                };
                this.client = new DownloadServiceClient(binding, new EndpointAddress(endpointAddress));
              //  this.client = new DownloadServiceClient();
            }
        }

        /// <summary>
        /// Загрузить последнее обновление ФИАС
        /// </summary>
        public IDataResult GetLastDeltaUpdate(BaseParams baseParams, CancellationToken ct, IProgressIndicator indicator = null)
        {
            DownloadFileInfo fileInfo = new DownloadFileInfo();
            var isForceDownload = baseParams.Params.GetAs("forcedownload", false, true);
            var filesInfo = client.GetAllDownloadFileInfo();
            foreach (var file in filesInfo)
            {
                if (!string.IsNullOrEmpty(file.FiasDeltaDbfUrl))
                {
                    fileInfo = file;
                    break;
                }
            }
            var fiasDbfUrl = new Uri(fileInfo.FiasDeltaDbfUrl);
            return GetUpdateFile(fiasDbfUrl, fileInfo, isForceDownload, ct, indicator);
        }

        /// <summary>
        /// Загрузить последнюю полную версию ФИАС
        /// </summary>
        public IDataResult GetLastFullUpdate(BaseParams baseParams, CancellationToken ct, IProgressIndicator indicator = null)
        {
            DownloadFileInfo fileInfo = new DownloadFileInfo();
            var isForceDownload = baseParams.Params.GetAs("forcedownload", false, true);
            var filesInfo = client.GetAllDownloadFileInfo();
            foreach (var file in filesInfo)
            {
                if (!string.IsNullOrEmpty(file.FiasCompleteDbfUrl))
                {
                    fileInfo = file;
                    break;
                }
            }
            var fiasDbfUrl = new Uri(fileInfo.FiasCompleteDbfUrl);

            return GetUpdateFile(fiasDbfUrl, fileInfo, isForceDownload, ct, indicator);
        }

        #endregion

        private IDataResult GetUpdateFile(Uri fiasDbfUrl, DownloadFileInfo fileInfo, bool isForceDownload, CancellationToken ct, IProgressIndicator indicator = null)
        {
            try
            {
                var filePath = GetFilePath(fiasDbfUrl, (int)fileInfo.VersionId);

                if (CheckLockFile(filePath))
                    return BaseDataResult.Error($"Обнаружен lock файл для файла {filePath}. Загрузка этого файла уже идет в другой сессии. Если это результат прекращения работы сервиса во время загрузки, удалите lock файл вручную");

                if (!File.Exists(filePath) || isForceDownload)
                    DownloadFile(fiasDbfUrl, filePath, ct, indicator);

                if (!CheckFile(filePath))
                    return BaseDataResult.Error("Ошибка при загрузке файла");               

                return new BaseDataResult(filePath) { Message = fileInfo.TextVersion };
            }
            catch (Exception e)
            {
                return BaseDataResult.Error("GetUpdateFile: "+e.Message);
            }
        }

        /// <summary>
        /// Скачивание файла
        /// </summary>
        private void DownloadFile(Uri fiasDbfUrl, string filePath, CancellationToken ct, IProgressIndicator indicator = null)
        {
            var lockFilePath = GetLockFile(filePath);
            var lockFile = File.Create(lockFilePath, 1, FileOptions.DeleteOnClose);
            try
            {
                using (var webClient = new WebClient())
                {
                    if(indicator!= null)
                        webClient.DownloadProgressChanged += (sender, args) => { indicator.Report(null, (uint)args.ProgressPercentage, $"Скачивание файла {fiasDbfUrl.AbsolutePath}"); };

                    using (var downloadTask = webClient.DownloadFileTaskAsync(fiasDbfUrl, filePath))
                    {
                        try
                        {
                            downloadTask.Wait(ct);
                        }
                        catch (OperationCanceledException)
                        {
                            LogManager.Debug("Отмена загрузки файла");
                        }
                        finally
                        {
                            if (ct.IsCancellationRequested)
                            {
                                webClient?.CancelAsync();
                            }
                        }
                    }
                }
            }
            finally
            {
                lockFile.Close();
            }
        }

        private string GetFilePath(Uri downloadUrl, int version)
        {
            var directoryPath = string.IsNullOrWhiteSpace(this.workDirectory) ? Path.GetTempPath() : this.workDirectory;
            var fileName = Path.GetFileNameWithoutExtension(downloadUrl.AbsolutePath);
            var fileExtension = Path.GetExtension(downloadUrl.AbsolutePath);
            var filePath = Path.Combine(directoryPath, $"{fileName}_{version}{fileExtension}");

            return filePath;
        }

        protected virtual string GetLockFile(string filePath)
        {
            return $"{filePath}.lock";
        }

        protected virtual bool CheckLockFile(string filePath)
        {
            var lockFile = this.GetLockFile(filePath);
            if (File.Exists(lockFile))
            {
                this.LogManager.Warning($"Обнаружен LOCK файл '{lockFile}'");
                return true;
            }

            return false;
        }

        protected virtual bool CheckFile(string filePath)
        {
            return (File.Exists(filePath) && new FileInfo(filePath).Length > 0);
        }
    }
}