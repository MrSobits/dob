namespace Bars.B4.Modules.Analytics.Reports.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Logging;
    using Bars.B4.Modules.Analytics.Data;
    using Bars.B4.Modules.Analytics.Enums;
    using Bars.B4.Modules.Analytics.Reports.Entities;
    using Bars.B4.Modules.Analytics.Reports.Entities.History;
    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Modules.Analytics.Reports.Extensions;
    using Bars.B4.Modules.Analytics.Reports.Generators;
    using Bars.B4.Modules.Analytics.Reports.ReportHandlers;
    using Bars.B4.Modules.Analytics.Reports.Tasks;
    using Bars.B4.Modules.Analytics.Reports.Utils;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.B4.Utils;

    using Castle.Windsor;

    using FileInfo = Bars.B4.Modules.FileStorage.FileInfo;

    /// <summary>
    /// Сервис генерации отчётов
    /// </summary>
    public class ReportGeneratorService : IReportGeneratorService
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Генератор отчётов
        /// </summary>
        public IReportGenerator ReportGenerator { get; set; }

        /// <summary>
        /// Менеджер файлов
        /// </summary>
        public IFileManager FileManager { get; set; }

        /// <summary>
        /// Менеджер задач
        /// </summary>
        public ITaskManager TaskManager { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="StoredReport"/>
        /// </summary>
        public IDomainService<StoredReport> StoredReportDomain { get; set; }

        /// <summary>
        /// Поставщик информации о пользователе
        /// </summary>
        public IUserInfoProvider UserInfoProvider { get; set; }

        public IDomainService<ReportHistory> ReportHistoryDomain { get; set; }

        /// <summary>
        /// Менеджер логов
        /// </summary>
        public ILogManager LogManager { get; set; }

        /// <inheritdoc />
        public ReportResult Generate(BaseParams baseParams)
        {
            var reportId = baseParams.Params.GetAs<long>("reportId", ignoreCase: true);
            var format = baseParams.Params.GetAs("format", ignoreCase: true, defaultValue: ReportPrintFormat.xls);
            var report = this.StoredReportDomain.FirstOrDefault(x => x.Id == reportId);

            var file = this.GenerateReportInternal(baseParams, report, format);

            return new ReportResult
            {
                ReportStream = file,
                FileName = $"{report.Name}.{format.Extension()}"
            };
        }

        /// <inheritdoc />
        public IDataResult CreateTaskOrSaveOnServer(BaseParams baseParams)
        {
            var beforeResult = this.BeforeGenerate(baseParams);
            if (!beforeResult.Success)
            {
                return beforeResult;
            }

            var reportId = baseParams.Params.GetAs<long>("reportId", ignoreCase: true);
            var report = this.StoredReportDomain.FirstOrDefault(x => x.Id == reportId);
            var format = baseParams.Params.GetAs("format", ignoreCase: true, defaultValue: ReportPrintFormat.xls);

            if (report.GenerateOnCalcServer)
            {
                var taskResult = this.TaskManager.CreateTasks(new ReportGeneratorTask(report.Name), baseParams);

                // возвращаем результат постановки задачи
                return new BaseDataResult(new
                {
                    taskedReport = true,
                    taskId = taskResult.Data.ParentTaskId
                });
            }

            var result = this.SaveOnServer(baseParams, report, format);

            return new BaseDataResult(new
            {
                taskedReport = false,
                fileId = result.Data.Id
            });
        }

        /// <inheritdoc />
        public IDataResult<FileInfo> SaveOnServer(BaseParams baseParams, StoredReport report, ReportPrintFormat format)
        {
            IDataResult<FileInfo> reportResult = null;

            using (var file = this.GenerateReportInternal(baseParams, report, format))
            {
                var fileName = (file as FileStream)?.Name;

                using (var transaction = this.Container.Resolve<IDataTransaction>())
                {
                    try
                    {
                        var result = this.FileManager.SaveFile(file, this.GetFileName(report, format));

                        var paramsForSave = baseParams.Params.GetAs<Dictionary<string, object>>("paramsForSave", ignoreCase: true);

                        var history = new ReportHistory()
                        {
                            ReportType = ReportType.StoredReport,
                            ReportId = report.Id,
                            Date = DateTime.UtcNow,
                            Category = report.Category,
                            Name = report.DisplayName,
                            File = result,
                            User = this.UserInfoProvider?.GetActiveUser()
                        };

                        var reportParams = report.GetParams().ToDictionary(x => x.Name);

                        paramsForSave.ForEach(
                            x =>
                            {
                                var param = reportParams.Get(x.Key);
                                if (param != null)
                                {
                                    var dict = (DynamicDictionary) x.Value;

                                    history.ParameterValues.Add(
                                        x.Key,
                                        new ReportHistoryParam
                                        {
                                            DisplayName = param.Label,
                                            Value = this.GetValue(param, dict),
                                            DisplayValue = this.GetDisplayValue(param, dict)
                                        });
                                }
                            });
                        this.ReportHistoryDomain.Save(history);

                        transaction.Commit();

                        reportResult = new GenericDataResult<FileInfo>(result);
                        return reportResult;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                    finally
                    {
                        if (!string.IsNullOrEmpty(fileName) && format == ReportPrintFormat.zip && File.Exists(fileName))
                        {
                            File.Delete(fileName);
                        }

                        this.AfterGenerate(baseParams, reportResult);
                    }
                }
            }
        }

        private object GetValue(IParam param, DynamicDictionary paramValueObject)
        {
            return param.Multiselect && paramValueObject.GetAs<string>("Value") != "All"
                ? paramValueObject.GetAs<List<object>>("Value")
                : paramValueObject.Get("Value");
        }

        private string GetDisplayValue(IParam param, DynamicDictionary paramValueObject)
        {
            return param.ParamType == ParamType.Bool
                ? (bool)paramValueObject["Value"]
                    ? "Да"
                    : "Нет"
                : (string)paramValueObject["DisplayValue"];
        }

        private Stream GenerateReportInternal(BaseParams baseParams, StoredReport report, ReportPrintFormat format)
        {
            var guid = Guid.NewGuid();
            var sw = Stopwatch.StartNew();
            this.LogManager.Info($"Запуска отчета: {guid}; Название: {report.DisplayName}; Пользователи: {this.GetActiveUserName()}");

            var file = this.ReportGenerator.Generate(
                report.GetDataSources(),
                report.GetTemplate(),
                baseParams,
                format,
                new Dictionary<string, object>
                {
                    { "ExportSettings", report.GetExportSettings(format) },
                    { "UseTemplateConnectionString", report.UseTemplateConnectionString }
                });

            this.LogManager.Info($"Конец формирования отчета: {guid}; Название: {report.DisplayName}; Время формирования: {sw.Elapsed}");

            file.Seek(0, SeekOrigin.Begin);
            return file;
        }

        private string GetActiveUserName()
        {
            return this.UserInfoProvider?.GetActiveUser()?.Name;
        }

        private string GetFileName(StoredReport report, ReportPrintFormat format)
        {
            var name = string.Join("_", report.Name.Split(Path.GetInvalidFileNameChars()));
            var ext = format.Extension();

            return $"{name}.{ext}";
        }

        private IDataResult BeforeGenerate(BaseParams baseParams)
        {
            var codeReport = baseParams.Params.GetAs("codeReport", string.Empty, true);
            if (string.IsNullOrEmpty(codeReport))
            {
                return new BaseDataResult("Не указан код отчета");
            }
            var result = new BaseDataResult();
            this.Container.UsingForResolvedAll<IReportHandler>((ioc, handlers) =>
            {
                var handlerResult = new List<IDataResult>();
                handlers.Where(x => x.Code == codeReport).ForEach(x => handlerResult.Add(x.BeforePrint(baseParams)));

                result.Success = handlerResult.All(x => x.Success);
                result.Data = handlerResult;
            });

            return result;
        }

        private IDataResult AfterGenerate(BaseParams baseParams, IDataResult<FileInfo> reportResult)
        {
            var codeReport = baseParams.Params.GetAs("codeReport", string.Empty, true);
            if (string.IsNullOrEmpty(codeReport))
            {
                return new BaseDataResult("Не указан код отчета");
            }
            var result = new BaseDataResult();
            this.Container.UsingForResolvedAll<IReportHandler>((ioc, handlers) =>
            {
                var handlerResult = new List<IDataResult>();
                handlers.Where(x => x.Code == codeReport).ForEach(x => handlerResult.Add(x.AfterPrint(baseParams, reportResult)));

                result.Success = handlerResult.All(x => x.Success);
                result.Data = handlerResult;
            });

            return result;
        }
    }
}