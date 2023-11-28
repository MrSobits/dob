namespace Bars.Gkh.FormatDataExport.FormatProvider.CsvFormat
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Text;

    using Bars.Gkh.ConfigSections.Administration;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.Utils;

    using Ionic.Zip;

    /// <summary>
    /// Выгрузка данных в csv формат
    /// </summary>
    public class CsvFormatProvider : BaseFormatProvider
    {
        public override IList<string> ServiceEntityCodes { get; } = new List<string> { "_INFO" };

        private const string FileDirectoryName = "files";
        private CsvFileList csvFileList;
        private const int DefaultMaxArchiveSize = 1024 * 1024 * 200; // 200 MB
        private readonly Encoding defaultArchiveEncoding = Encoding.GetEncoding(866);
        private const float EntitiesPart = 50;
        private const float FilesPart = 90;
        protected float Progress = 0f;

        /// <inheritdoc />
        public override string FormatVersion => BaseExportableEntity.Version;

        /// <inheritdoc />
        protected override void ExportData(string pathToSave)
        {
            var entityDelta = CsvFormatProvider.EntitiesPart / this.ExportableEntities.Count;
            var config = this.Container.GetGkhConfig<AdministrationConfig>()
                .FormatDataExport
                .FormatDataExportGeneral;

            using (var zipArchiveFile = new ZipFile(pathToSave, this.defaultArchiveEncoding))
            {
                var maxArchiveSize = (config.MaxArchiveSize ?? 0) * 1024 * 1024;
                zipArchiveFile.MaxOutputSegmentSize = maxArchiveSize != 0 ? maxArchiveSize : CsvFormatProvider.DefaultMaxArchiveSize;

                this.csvFileList = new CsvFileList();

                var sw = new Stopwatch();
                foreach (var entity in this.ExportableEntities)
                {
                    this.CancellationToken.ThrowIfCancellationRequested();

                    this.LogManager.Debug($"Экспорт сущности '{entity.Code}'");
                    sw.Start();
                    try
                    {
                        var csvData = this.GetCsv(entity);

                        if (csvData.Data == null)
                        {
                            this.AddErrorToLog(entity.Code, "Нет данных для экспорта информации");
                            this.LogManager.Debug($"Нет данных для экспорта информации с кодом: '{entity.Code}'");
                            continue;
                        }

                        this.AddEntry(zipArchiveFile, csvData);
                    }
                    catch (Exception exception)
                    {
                        this.AddErrorToLog(entity.Code, exception.Message, exception);
                        this.LogManager.Error($"Ошибка экспорта информации с кодом: '{entity.Code}'", exception);
                    }
                    finally
                    {
                        sw.Stop();

                        this.AddErrorRecordsToLog(entity);

                        this.LogManager.Debug($"Экспорт сущности '{entity.Code}' завершен: {sw.Elapsed}");
                        sw.Reset();

                        this.Container.Release(entity);

                        this.Progress += entityDelta;
                        this.ProgressNotify(this.Progress);
                    }
                }

                this.ProgressNotify(CsvFormatProvider.EntitiesPart);

                this.CancellationToken.ThrowIfCancellationRequested();

                this.AddFilesToArchive(zipArchiveFile);

                this.AddCsvFileList(zipArchiveFile);

                this.CancellationToken.ThrowIfCancellationRequested();

                zipArchiveFile.Save();
            }
        }

        private CsvFileData GetCsv(IExportableEntity entity)
        {
            return CsvHelper.GetContent(entity.Code.ToLowerInvariant(), entity.GetHeader(), entity.GetData(this.DataSelectorParams));
        }

        private CsvFileData GetExpotrableFiles()
        {
            var exportableFiles = this.FileEntityCollection as IExportableEntity;
            if (exportableFiles != null)
            {
                return this.GetCsv(exportableFiles);
            }
            else
            {
                return CsvFileData.CreateEmpty();
            }
        }

        private void AddFilesToArchive(ZipFile zipArchiveFile)
        {
            var csvData = this.GetExpotrableFiles();

            if (!csvData.IsEmpty())
            {
                this.AddEntry(zipArchiveFile, csvData);
            }

            zipArchiveFile.AddDirectoryByName(CsvFormatProvider.FileDirectoryName);

            var filesDelta = (CsvFormatProvider.FilesPart - CsvFormatProvider.EntitiesPart)
                / this.FileEntityCollection.Count;

            foreach (var fileInfo in this.FileEntityCollection.GetFileStreams())
            {
                var fileName = Path.Combine(CsvFormatProvider.FileDirectoryName, fileInfo.Name);

                if (!zipArchiveFile.ContainsEntry(fileName))
                {
                    zipArchiveFile.AddEntry(fileName, fileInfo.FileStream);
                }
               
                this.Progress += filesDelta;
                this.ProgressNotify(this.Progress);
            }

            this.ProgressNotify(CsvFormatProvider.FilesPart);
        }

        private void AddCsvFileList(ZipFile zipArchiveFile)
        {
            var fileList = this.csvFileList.GetCsvFileData();
            zipArchiveFile.AddEntry(fileList.FullName, fileList.Data ?? new byte[0]);
        }

        private void AddEntry(ZipFile zipArchiveFile, CsvFileData csvData)
        {
            zipArchiveFile.AddEntry(csvData.FullName, csvData.Data ?? new byte[0]);
            this.csvFileList.Add(csvData);

            this.ExportedEntityCodeList.Add(csvData.FileName.ToUpper());
        }
    }
}