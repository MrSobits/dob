namespace Bars.B4.Modules.Analytics.Reports.Generators
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    using Bars.B4.Application;
    using Bars.B4.Logging;
    using Bars.B4.Modules.Analytics.Data;
    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Modules.Analytics.Reports.Extensions;
    using Bars.B4.Utils;

    using Stimulsoft.Report;
    using Stimulsoft.Report.Dictionary;
    using Stimulsoft.Report.Export;

    using Ionic.Zip;

    /// <summary>
    /// Генератор отчётов
    /// </summary>
    public class StimulReportGenerator : IReportGenerator, IDisposable
    {
        public ILogManager LogManager { get; set; }

        /// <summary>
        /// Объект отчёта
        /// </summary>
        protected StiReport Report { get; set; }

        /// <summary>
        /// Сгенерировать отчёт
        /// </summary>
        /// <param name="dataSources">Источники данных</param>
        /// <param name="reportTemplate">Шаблон отчёта</param>
        /// <param name="baseParams">Параметры</param>
        /// <param name="printFormat">Формат печати</param>
        /// <param name="customArgs">Дополнительные параметры</param>
        public Stream Generate(IEnumerable<IDataSource> dataSources, Stream reportTemplate, BaseParams baseParams,
            ReportPrintFormat printFormat, IDictionary<string, object> customArgs)
        {
            this.Report = StiReport.GetReportFromAssembly(this.GetReportAssembly(reportTemplate));

            this.ConfigureReport(customArgs);

            this.RegData(dataSources, baseParams);
            this.RegVars(baseParams);

            if (!this.Report.IsRendered)
            {
                this.Report.Render(false);
            }

            var result = new MemoryStream();
            result.Seek(0, SeekOrigin.Begin);

            var exportSettings = customArgs.Get("ExportSettings") as StiExportSettings;
            if (exportSettings != null)
            {
                this.Report.ExportDocument(printFormat.ExportFormat(), result, exportSettings);
            }
            else
            {
                this.Report.ExportDocument(printFormat.ExportFormat(), result);
            }

            return this.PreparingStream(result, printFormat);
        }

        /// <summary>
        /// Регистрация данных
        /// </summary>
        /// <param name="dataSources">Источники данных</param>
        /// <param name="baseParams">Параметры</param>
        protected void RegData(IEnumerable<IDataSource> dataSources, BaseParams baseParams)
        {
            foreach (var dataSource in dataSources)
            {
                this.Report.RegData(
                    dataSource.Name,
                    dataSource.GetMetaData(),
                    dataSource.GetData(this.ExtractBindedParams(baseParams)));
            }
        }

        /// <summary>
        /// Извлечение параметров, необходимых источнику данных
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Параметры</returns>
        protected BaseParams ExtractBindedParams(BaseParams baseParams)
        {
            return baseParams;
        }

        private void ActualizeConnectionString(StiReport stiReport)
        {
            foreach (var database in stiReport.Dictionary.Databases)
            {
                var db = database as StiSqlDatabase;
                if (db != null)
                {
                    db.ConnectionString = ApplicationContext.Current.Configuration.ConnString;
                }
            }
        }

        private void RegVars(BaseParams baseParams)
        {
            var varItems = this.Report.Dictionary.ReturnSafe(x => x.Variables).ReturnSafe(x => x.Items);
            var Params = baseParams.ReturnSafe(x => x.Params);

            if (varItems != null && varItems.Any() && Params != null)
            {
                var varNames = varItems.Select(x => x.Name);

                foreach (var varName in varNames.Where(varName => this.Report.Dictionary.Variables.Contains(varName)))
                {
                    var value = Params.GetAs(varName, string.Empty);
                    var convertedValue = ConvertHelper.ConvertTo(value, this.Report.Dictionary.Variables[varName].Type);

                    this.Report[varName] = convertedValue;
                    this.Report.Dictionary.Variables[varName].Value = value;

                    if (this.Report.CompiledReport != null)
                    {
                        this.Report.CompiledReport[varName] = convertedValue;
                        this.Report.CompiledReport.Dictionary.Variables[varName].Value = value;
                    }
                }
            }
        }

        private Assembly GetReportAssembly(Stream reportTemplate)
        {
            string hash;

            if (!StimulReportAssemblyCache.Contains(reportTemplate, out hash))
            {
                StimulReportAssemblyCache.Add(reportTemplate);
            }

            return StimulReportAssemblyCache.Get(hash);
        }

        private void ConfigureReport(IDictionary<string, object> customArgs)
        {
            this.Report.CacheAllData = true;
            this.Report.ReportCacheMode = StiReportCacheMode.Off;

            if (!customArgs.Get("UseTemplateConnectionString").ToBool())
            {
                this.ActualizeConnectionString(this.Report);
            }
        }

        /// <summary>
        /// Освобождение ресурсов
        /// </summary>
        public virtual void Dispose()
        {
            this.Report?.Dispose();
        }

        /// <summary>
        /// Модифицировать выходной поток
        /// </summary>
        protected virtual Stream PreparingStream(Stream stream, ReportPrintFormat format)
        {
            if (format != ReportPrintFormat.zip)
            {
                return stream;
            }

            var archPath = this.Report["ArchPath"] as string;

            if (string.IsNullOrWhiteSpace(archPath))
            {
                throw new FormatException("Тип отчета *.ZIP, но в отчете не установлена переменная ArchPath");
            }

            if (!Directory.Exists(archPath))
            {
                this.LogManager.Error($"Директория архивации '{archPath}' не найдена");
                throw new DirectoryNotFoundException("Директория архивации не найдена");
            }

            stream.Dispose();

            var fileName = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            using (var zip = new ZipFile(Encoding.UTF8))
            {
                zip.AddDirectory(archPath);
                zip.Save(fileName);
            }

            Directory.Delete(archPath, true);

            return File.OpenRead(fileName);
        }
    }
}
