namespace Bars.B4.Modules.FIAS.AutoUpdater.ArchiveReader.Impl
{
    using Bars.B4.Config;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Logging;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Castle.Core;
    using Castle.Windsor;
    using SharpCompress.Archives;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading;

    /// <summary>
    /// Сервис работы с архивом ФИАС
    /// </summary>
    internal class FiasArchiveReader : IFiasArchiveReader, IInitializable
    {
        public IWindsorContainer Container { get; set; }
        public ILogManager LogManager { get; set; }

        private string regionCodeFromConfig;

        /// <inheritdoc />
        public string FiasEntityName => "addrob";

        /// <inheritdoc />
        public string FiasHouseEntityName => "house";

        /// <inheritdoc />
        public void Initialize()
        {
            var configProvider = this.Container.Resolve<IConfigProvider>();

            using (this.Container.Using(configProvider))
            {
                var config = configProvider.GetConfig().GetModuleConfig("Bars.B4.Modules.FIAS.AutoUpdater");

                var regionCode = config.GetAs("RegionCode", default(string), true);

                var code = 0;
                if (int.TryParse(regionCode, out code))
                {
                    this.regionCodeFromConfig = regionCode;
                }
                else
                {
                    this.LogManager.Warning($"Указан некорректный код региона: '{regionCode}'");
                }
            }
        }

        /// <summary>
        /// Распаковка архива ФИАС в словарь EntityName-содержимое файла
        /// </summary>
        /// <param name="archPath">Путь к архиву</param>
        /// <param name="unpackDir">Папка для распакованных файлов</param>
        public IDataResult<Dictionary<string, string>> UnpackFiles(string archPath, CancellationToken ct, IProgressIndicator indicator = null, string unpackDir = null)
        {
            try
            {
                var result = new Dictionary<string, string>();
                var regionCode = GetRegionCode();
                unpackDir = unpackDir ?? Path.GetDirectoryName(archPath);

                using (var arch = ArchiveFactory.Open(archPath))
                {
                    int entriescount = arch.Entries.Count();
                    uint i = 0;

                    foreach (var archiveEntry in arch.Entries)
                    {
                        i++;
                        //addrob
                        if (archiveEntry.Key.ToLower() == $"{FiasEntityName}{regionCode}.dbf")
                        {
                            indicator?.Report(null, (uint)(i * 100 / entriescount), $"Распаковка файла {archiveEntry.Key}");
                            result.Add(FiasEntityName, UnpackFile(archiveEntry, unpackDir, ct));
                        }
                        //house
                        else if (archiveEntry.Key.ToLower() == $"{FiasHouseEntityName}{regionCode}.dbf")
                        {
                            indicator?.Report(null, (uint)(i * 100 / entriescount), $"Распаковка файла {archiveEntry.Key}");
                            result.Add(FiasHouseEntityName, UnpackFile(archiveEntry, unpackDir, ct));
                        }
                        if (result.Count == 2)
                        {
                            break;
                        }
                    }
                }

                if (result.Count < 2)
                {
                    throw new FileNotFoundException("Не удалось распаковать все необходимые файлы");
                }

                return new GenericDataResult<Dictionary<string, string>>(result);
            }
            catch (Exception e)
            {
                return new GenericDataResult<Dictionary<string, string>>
                {
                    Success = false,
                    Message = e.Message
                };
            }
        }

        /// <summary>
        /// Распаковать файл
        /// </summary>
        internal virtual string UnpackFile(IArchiveEntry archiveEntry, string unpackDir, CancellationToken ct)
        {
            var filePath = Path.Combine(unpackDir, archiveEntry.Key);

            try
            {
                using (var archStream = archiveEntry.OpenEntryStream())
                using (var fs = new FileStream(filePath, FileMode.OpenOrCreate))
                {
                    fs.Seek(0, SeekOrigin.Begin);

                    archStream.CopyTo(fs);

                    fs.Flush();
                }

                LogManager.Debug($"Распаковка файла '{filePath}' завершена");               

            }
            catch
            {               
                if(File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                archiveEntry.WriteToDirectory(unpackDir);
                LogManager.Debug($"Распаковка файла '{filePath}' завершена");
            }
            return filePath;

        }

        internal virtual string GetRegionCode()
        {
            if (!string.IsNullOrWhiteSpace(this.regionCodeFromConfig))
            {
                return this.regionCodeFromConfig;
            }

            var fiasRepository = this.Container.Resolve<IRepository<Fias>>();
            try
            {
                return fiasRepository.GetAll().Select(x => x.CodeRegion).FirstOrDefault(x => x != null && x != string.Empty);
            }
            finally
            {
                this.Container.Release(fiasRepository);
            }
        }

    }
}