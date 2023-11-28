namespace Bars.Gkh.FormatDataExport.FormatProvider.CsvFormat
{
    using System;
    using System.IO;

    using Bars.Gkh.FormatDataExport.NetworkWorker;
    using Bars.Gkh.FormatDataExport.NetworkWorker.Responses;

    /// <summary>
    /// Экспорт данных в csv с последующей передачей в РИС
    /// </summary>
    public class NetCsvFormatProvider : CsvFormatProvider
    {
        public IFormatDataTransferService FormatDataTransferService { get; set; }

        /// <inheritdoc />
        protected override void ExportData(string pathToSave)
        {
            base.ExportData(pathToSave);

            if (File.Exists(pathToSave))
            {
                this.SendData(pathToSave);
            }
            else
            {
                throw new FileNotFoundException("Не удалось получить файл для экспорта");
            }
        }

        private void SendData(string dataPath)
        {
            this.FormatDataTransferService.SetToken(this.Operator);

            var uploadResult = this.FormatDataTransferService.UploadFile(dataPath, this.CancellationToken);

            if (uploadResult.Success && uploadResult.Data is UploadSuccess)
            {
                var remoteFile = uploadResult.Data as UploadSuccess;

                var importStatus = this.FormatDataTransferService.StartImport(remoteFile.Id, this.CancellationToken);
                if(!importStatus.Success)
                {
                    this.AddErrorToLog("StartImport", importStatus.Message, importStatus.Data as Exception);
                }
            }
            else if (!uploadResult.Success)
            {
                this.AddErrorToLog("UploadFile", uploadResult.Message, uploadResult.Data as Exception);
            }
        }
    }
}