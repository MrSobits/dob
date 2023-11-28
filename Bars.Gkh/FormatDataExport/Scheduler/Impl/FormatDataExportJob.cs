namespace Bars.Gkh.FormatDataExport.Scheduler.Impl
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;

    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.Config;
    using Bars.B4.Logging;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.ConfigSections.Administration;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities.Administration.FormatDataExport;
    using Bars.Gkh.Enums;
    using Bars.Gkh.FormatDataExport.FormatProvider;
    using Bars.Gkh.FormatDataExport.FormatProvider.CsvFormat;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    /// <summary>
    /// Задача запускаемая при срабатывании триггера
    /// </summary>
    public class FormatDataExportJob
    {
        private string FtpPath => ApplicationContext.Current.Configuration.AppSettings.GetAs<string>("FtpPath");
        private FormatDataExportResult ExportResult { get; set; }
        private FormatDataExportRemoteResult ExportRemoteResult { get; set; }

        public FormatDataExportTask ExportTask { private get; set; }
        public CancellationToken CancellationToken { private get; set; }
        public IProgressIndicator ProgressIndicator { private get; set; }

        public IWindsorContainer Container { get; set; }
        public IExportFormatProviderBuilder ExportProviderBuilder { get; set; }
        public IGkhUserManager GkhUserManager { get; set; }
        public IConfigProvider ConfigProvider { get; set; }
        public ILogManager LogManager { get; set; }
        public IDomainService<FormatDataExportTask> FormatDataExportTaskDomain { get; set; }
        public IDomainService<FormatDataExportResult> FormatDataExportResultDomain { get; set; }
        public IDomainService<FormatDataExportRemoteResult> FormatDataExportRemoteResultDomain { get; set; }

        /// <inheritdoc />
        public void Execute()
        {
            var activeOperator = this.GkhUserManager.GetActiveOperator();
            var activeOperatorLogin = activeOperator?.User.Login;

            var baseParams = new BaseParams();
            baseParams.Params.Apply(new Dictionary<string, object>
            {
                { "OperatorId", activeOperator?.Id },
            });

            var exportProviderBuilder = this.ExportProviderBuilder
                .SetParams(baseParams)
                .SetCancellationToken(this.CancellationToken)
                .SetEntytyGroupCodeList(this.ExportTask.EntityGroupCodeList);

            var exportProvider = this.CheckRemoteAddress()
                ? exportProviderBuilder.Build<NetCsvFormatProvider>()
                : exportProviderBuilder.Build<CsvFormatProvider>();

            this.ExportResult = new FormatDataExportResult
            {
                StartDate = DateTime.Now,
                Task = this.ExportTask,
                Progress = 0,
                Status = FormatDataExportStatus.Running,
                EntityCodeList = exportProvider.EntityCodeList
            };
            this.UpdateResult();

            this.ExportRemoteResult = new FormatDataExportRemoteResult
            {
                TaskResult = this.ExportResult
            };
            this.UpdateRemoteResult();

            try
            {
                if (this.ExportTask.IsDelete)
                {
                    throw new OperationCanceledException("Задача удалена");
                }

                exportProvider.OnProgressChanged += this.ExportProviderOnProgressChanged;
                exportProvider.OnAfterExport += this.ExportProviderOnAfterExport;

                var versionFolder = $"format_{exportProvider.FormatVersion}";
                var fileName = $"{DateTime.Now:yyyyMMdd_HHmmss}.zip";

                var ftpDirectory = this.GetFtpDirectory(versionFolder, activeOperatorLogin);
                if (!Directory.Exists(ftpDirectory))
                {
                    Directory.CreateDirectory(ftpDirectory);
                }

                var pathToSave = Path.Combine(ftpDirectory, fileName);

                exportProvider.Export(pathToSave);

                this.ExportResult.Progress = 100;
                this.ExportResult.Status = string.IsNullOrEmpty(exportProvider.SummaryErrors)
                    ? FormatDataExportStatus.Successfull
                    : FormatDataExportStatus.Error;
            }
            catch (OperationCanceledException)
            {
                this.LogManager.Debug($"Экспорт данных по формату {exportProvider.FormatVersion} прерван пользователем");
                this.ExportResult.Status = FormatDataExportStatus.Aborted;
                this.ExportResult.Progress = 0;
            }
            catch (Exception e)
            {
                this.LogManager.Error($"Экспорт данных по формату {exportProvider.FormatVersion}", e);
                this.ExportResult.Status = FormatDataExportStatus.RuntimeError;
                this.ExportResult.Progress = 0;
            }
            finally
            {
                this.ExportResult.LogOperation = exportProvider.LogOperation;
                this.ExportResult.EndDate = DateTime.Now;
                this.UpdateResult();
            }
        }

        private void ExportProviderOnAfterExport(object sender, IList<string> exportedEntityCodeList)
        {
            this.ExportResult.EntityCodeList = exportedEntityCodeList;
            this.UpdateResult();
        }

        private bool CheckRemoteAddress()
        {
            return !string.IsNullOrWhiteSpace(this.Container.GetGkhConfig<AdministrationConfig>()
                .FormatDataExport
                .FormatDataExportGeneral
                .TransferServiceAddress);
        }

        private void ExportProviderOnProgressChanged(object sender, float progress)
        {
            this.ProgressIndicator?.Report(null, (uint)Math.Round(progress), string.Empty);
            this.ExportResult.Progress = progress;
            this.UpdateResult();
        }

        private void UpdateResult()
        {
            this.FormatDataExportResultDomain.SaveOrUpdate(this.ExportResult);
        }

        private void UpdateRemoteResult()
        {
            this.FormatDataExportRemoteResultDomain.SaveOrUpdate(this.ExportRemoteResult);
        }

        private string GetFtpDirectory(string formatFolder, string userName)
        {
            return Path.Combine(this.FtpPath, formatFolder, userName);
        }
    }
}