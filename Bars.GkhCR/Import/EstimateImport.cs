namespace Bars.GkhCr.Import
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums.Import;
    using Bars.Gkh.Import;
    using Bars.GkhCr.Entities;
    using Bars.GkhExcel;
    using Bars.B4.Logging;

    using Castle.Windsor;
    using Gkh.Import.Impl;

    /// <summary>
    /// Импорт смет
    /// </summary>
    public sealed class EstimateImport : GkhImportBase
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        private const string Numbers = "0123456789";
        private ILogImport logImport;
        private ILogImport[] _logImports;

        public IWindsorContainer Container { get; set; }

        public override string Key
        {
            get { return Id; }
        }

        public override string CodeImport
        {
            get { return "EstimateObjectCr"; }
        }

        public override string Name
        {
            get { return "Импорт смет"; }
        }

        public override string PossibleFileExtensions
        {
            get { return "xls"; }
        }

        public override string PermissionName
        {
            get { return "Import.Estimate.View"; }
        }

        /// <summary>
        /// Менеджер управляющий логами
        /// </summary>
        public ILogImportManager LogManager { get; set; }

        public override ImportResult Import(BaseParams baseParams)
        {
            var message = string.Empty;
            var fileData = baseParams.Files["FileImport"];
            var estimateCalculationId = baseParams.Params["EstimateCalculationId"].ToLong();

            var repEstimate = Container.Resolve<IDomainService<Estimate>>();
            var repEstimateCalculation = Container.Resolve<IDomainService<EstimateCalculation>>();
            var numberRow = 0;

            try
            {
                InitLog(fileData.FileName);
                var excel = Container.Resolve<IGkhExcelProvider>("ExcelEngineProvider");

                using (Container.Using(excel))
                {
                    if (excel == null)
                    {
                        throw new Exception("Не найдена реализация интерфейса IGkhExcelProvider");
                    }

                    var memoryStreamFile = new MemoryStream(fileData.Data);
                    excel.Open(memoryStreamFile);

                    var estimateCalculation = repEstimateCalculation.Get(estimateCalculationId);
                    if (estimateCalculation == null)
                    {
                        logImport.Error(Name, "Не найден сметный расчет по работе");

                        LogManager.Add(fileData, logImport);
                        LogManager.Save();
                        return new ImportResult(StatusImport.CompletedWithError,
                            "Не найден сметный расчет по работе", string.Empty, LogManager.LogFileId);
                    }

                    // Перед загрузкой удаляем показатели
                    var records = repEstimate.GetAll()
                        .Where(x => x.EstimateCalculation.Id == estimateCalculation.Id)
                        .Select(x => x.Id)
                        .ToArray();

                    var transaction = Container.Resolve<IDataTransaction>();
                    using (Container.Using(transaction))
                    {
                        try
                        {
                            foreach (var rec in records)
                            {
                                repEstimate.Delete(rec);
                            }

                            var start = false;

                            foreach (var row in excel.GetRows(0, 0))
                            {
                                numberRow++;
                                var firstCell = row[0];

                                // Считаем что достигли конца файла
                                if (start && firstCell.Value != null
                                    && firstCell.Value.ToLower().StartsWith("итого")) break;

                                // Ищем начало импортируемого блока данных
                                if (!start && row[0].Value == "1")
                                {
                                    start = true;
                                    continue;
                                }

                                // Пропускаем объединенные строки
                                if (row[0].IsMerged) continue;
                                if (!start) continue;

                                var number = firstCell.Value != null ? firstCell.Value.Trim() : null;

                                // пропускаем удаленные позиции
                                if (number != null && number.ToLower() == "уд")
                                    continue;

                                ImportRow(row, estimateCalculation, number, repEstimate);
                            }

                            logImport.IsImported = true;
                            transaction.Commit();
                        }
                        catch (Exception exc)
                        {
                            try
                            {
                                logImport.IsImported = false;
                                var logManager = Container.Resolve<ILogManager>();

                                using (Container.Using(logManager))
                                {
                                    logManager.Error("Импорт", exc);

                                    logImport.Error(string.Format("{0}. Строка {1}", Name, numberRow),
                                        "Произошла неизвестная ошибка. Обратитесь к администратору.");
                                    message = "Произошла неизвестная ошибка. Обратитесь к администратору";
                                    transaction.Rollback();
                                }
                            }
                            catch (Exception ex)
                            {
                                throw new Exception(ex.Message, exc);
                            }
                        }
                        finally
                        {
                            memoryStreamFile.Close();
                        }
                    }
                }

                if (logImport.IsImported && logImport.CountImportedRows == 0)
                {
                    logImport.IsImported = false;
                    logImport.Error(Name, "Не удалось обнаружить записи для импорта");
                    message = "Не удалось обнаружить записи для импорта";
                }

                LogManager.Add(fileData, logImport);
                message += LogManager.GetInfo();
                LogManager.Save();
            }
            finally
            {
                Container.Release(repEstimate);
                Container.Release(repEstimateCalculation);
            }

            var status = !logImport.IsImported ? StatusImport.CompletedWithError : (logImport.CountWarning > 0  ? StatusImport.CompletedWithWarning : StatusImport.CompletedWithoutError);

            using (Container.Using(LogManager, _logImports))
            {
                return new ImportResult(status, message, string.Empty, LogManager.LogFileId);
            }
        }

        public override bool Validate(BaseParams baseParams, out string message)
        {
            message = null;
            if (!baseParams.Files.ContainsKey("FileImport"))
            {
                message = "Не выбран файл для импорта";
                return false;
            }

            var bytes = baseParams.Files["FileImport"].Data;

            var extention = baseParams.Files["FileImport"].Extention;
    
            try
            {
                using (var memoryStreamFile = new MemoryStream(bytes))
                {
                    var fileExtentions = PossibleFileExtensions.Contains(",") ? PossibleFileExtensions.Split(',') : new[] { PossibleFileExtensions };
                    if (fileExtentions.All(x => x != extention))
                    {
                        message = string.Format("Необходимо выбрать файл с допустимым расширением: {0}", PossibleFileExtensions);
                        return false;
                    }

                    var excel = Container.Resolve<IGkhExcelProvider>("ExcelEngineProvider");

                    using (Container.Using(excel))
                    {
                        if (excel == null)
                        {
                            throw new Exception("Не найдена реализация интерфейса IGkhExcelProvider");
                        }

                        excel.Open(memoryStreamFile);

                        if (excel.IsEmpty(0, 0))
                        {
                            message = string.Format("Не удалось обнаружить записи в файле: {0}", PossibleFileExtensions);
                            return false;
                        }

	                    var row = excel.GetRows(0, 0).FirstOrDefault(e => e.Count() == 14 && e[0].Value == "№ пп");
                        if (row == null)
                        {
                            message = "Количество колонок в загружаемом файле меньше 14, либо не найдена строка заголовка";
                            return false;
                        }
                    }

                    return true;
                }
            }
            catch (Exception exp)
            {
                var logManager = Container.Resolve<ILogManager>();

                using (Container.Using(logManager))
                {
                    logManager.Error("Валидация файла импорта", exp);
                    message = "Произошла неизвестная ошибка при проверки формата файла";
                    return false;
                }
            }
        }

        private static string RemoveLineBreak(string value)
        {
            if (!string.IsNullOrEmpty(value) && value.Contains("\n"))
            {
                value = value.Remove(value.IndexOf("\n", StringComparison.Ordinal));
            }

            return string.IsNullOrEmpty(value) || (!string.IsNullOrEmpty(value) && value.Trim() == "-") ? "0" : value;
        }

        private void InitLog(string fileName)
        {
            LogManager = Container.Resolve<ILogImportManager>();
            if (LogManager == null)
            {
                throw new Exception("Не найдена реализация интерфейса ILogImportManager");
            }

            LogManager.FileNameWithoutExtention = fileName;
            LogManager.UploadDate = DateTime.Now;

            _logImports = Container.ResolveAll<ILogImport>();

            logImport = _logImports.FirstOrDefault(x => x.Key == MainLogImportInfo.Key);
            if (logImport == null)
            {
                throw new Exception("Не найдена реализация интерфейса ILogImport");
            }

            logImport.SetFileName(fileName);
            logImport.ImportKey = Key;
        }

        private void ImportRow(GkhExcelCell[] row, EstimateCalculation estimateCalculation, string number, IDomainService<Estimate> repEstimate)
        {
            var importUnitMeasure = RemoveLineBreak(row[3].Value.Trim());

            var num = number != null
                ? number.Length > 300
                    ? number.Substring(0, 300)
                    : number
                : null;

            var estimate = new Estimate
            {
                EstimateCalculation = estimateCalculation,
                Number = num,
                Name = row[2] != null ? row[2].Value : string.Empty,
                UnitMeasure = importUnitMeasure,
                OnUnitCount = row[4] != null ? RemoveLineBreak(row[4].Value).To<decimal?>() : 0m,
                TotalCount = row[5] != null ? RemoveLineBreak(row[5].Value).To<decimal?>() : 0m,
                OnUnitCost = row[6] != null ? RemoveLineBreak(row[6].Value).To<decimal?>() : 0m,
                TotalCost = row[7] != null ? RemoveLineBreak(row[7].Value).To<decimal?>() : 0m,
                BaseSalary = row[8] != null ? RemoveLineBreak(row[8].Value).To<decimal?>() : 0m,
                MachineOperatingCost = row[9] != null ? RemoveLineBreak(row[9].Value).To<decimal?>() : 0m,
                MechanicSalary = row[10] != null ? RemoveLineBreak(row[10].Value).To<decimal?>() : 0m,
                MaterialCost = row[11] != null ? RemoveLineBreak(row[11].Value).To<decimal?>() : 0m,
                BaseWork = row[12] != null ? RemoveLineBreak(row[12].Value).To<decimal?>() : 0m,
                MechanicWork = row[13] != null ? RemoveLineBreak(row[13].Value).To<decimal?>() : 0m
            };

            var reason = row[1] != null ? row[1].Value : string.Empty;
            if (!string.IsNullOrEmpty(reason) && Numbers.Contains(reason.Substring(0, 1)) && reason.Contains("."))
            {
                reason = reason.Substring(reason.IndexOf(".", 1, StringComparison.Ordinal) + 1).Trim();
            }

            estimate.Reason = reason;

            repEstimate.Save(estimate);
            logImport.CountAddedRows++;
        }
    }
}
