﻿namespace Bars.Gkh.RegOperator.Report
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Bars.B4.Modules.Reports;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.RegOperator.Properties;
    using Bars.Gkh.StimulReport;

    using Stimulsoft.Report;

    public class BankDocumentImportCheckReport : StimulReport, IBaseReport
    {
        private readonly string docNum;

        private readonly DateTime? docDate;

        private readonly decimal? docSum;

        private readonly List<Result> results;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="docNum">Номер реестра</param>
        /// <param name="docDate">Дата реестра</param>
        /// <param name="docSum">Сумма реестра</param>
        /// <param name="results">Результаты проверки</param>
        public BankDocumentImportCheckReport(string docNum, DateTime? docDate, decimal? docSum, List<Result> results)
        {
            this.docNum = docNum;
            this.docDate = docDate;
            this.docSum = docSum;
            this.results = results;
        }

        /// <summary>Формат печатной формы</summary>
        public override StiExportFormat ExportFormat => StiExportFormat.Excel2007;

        /// <summary>Получить шаблон</summary>
        public Stream GetTemplate()
        {
            return new MemoryStream(Resources.BankDocumentImportCheckReport);
        }

        /// <summary>Выполнить сборку отчета</summary>
        /// <param name="reportParams">Параметры отчета</param>
        public void PrepareReport(ReportParams reportParams)
        {
            this.Report["НомерРеестра"] = this.docNum;
            this.Report["ДатаРеестра"] = this.docDate;
            this.Report["СуммаРеестра"] = this.docSum;
            this.Report["Учтено"] = this.results.SafeSum(x => x.Учтено);
            this.Report["Подтверждено"] = this.results.SafeSum(x => x.Подтверждено);
            this.Report["Расхождение"] = this.results.SafeSum(x => x.Расхождение);

            this.Report.RegData("РасхожденияПоЛС", this.results.Where(x => x.Учтено != x.Подтверждено).ToArray());
        }

        /// <summary>Имя отчета</summary>
        public string Name => "Отчет проверки реестра оплат";

        public class Result
        {
            public string ЛС { get; set; }

            public decimal Подтверждено { get; set; }

            public decimal Учтено { get; set; }

            public decimal Расхождение => Math.Abs(this.Подтверждено - this.Учтено);
        }
    }
}