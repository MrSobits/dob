namespace Bars.Gkh.Gis.Reports.Reports
{
    using System.IO;
    using B4.DataAccess;
    using B4.Modules.Reports;
    using B4.Utils;
    using Properties;
    using Stimulsoft.Report;
    using System;
    using System.Collections.Generic;
    using System.Data;

    public class Report_SZ_Charge_MKD : StimulReportDynamicExcel
    {
        private DateTime _dateReport = DateTime.MinValue;

        public IList<PrintFormExportFormat> GetExportFormats()
        {
            return new[]
                   {
                       new PrintFormExportFormat { Id = (int)StiExportFormat.Excel,     Name = "Excel"         },
                       new PrintFormExportFormat { Id = (int)StiExportFormat.Excel2007, Name = "Excel 2007"    },
                       new PrintFormExportFormat { Id = (int)StiExportFormat.Pdf,       Name = "Adobe Acrobat" }
                   };
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            
            var sqlMain = string.Format(@"SELECT
	                rajon,
	                service,
	                measure,
	                cnt_dmx,
	                cnt_gil,
	                s_ob,
	                tariff,
	                c_calc,
	                sum_charge,
                    CASE WHEN service = 'Итого' THEN 1 ELSE 0 END as service_order
                FROM
	                report_charge,
	                sr_rajon
                WHERE
	                rajon_code = kod_raj
                  AND charge_date = '{0}'
                  AND vill = 'Итого'
                  AND nzp_serv IN (10,9,7,8,6,14,257,11,25,210,-1)
                ORDER BY rajon, service_order, service", _dateReport.ToString("yyyy-MM-01"));

            var sqlSumary = string.Format(@"SELECT
	                'Все районы',
	                service,
	                measure,
	                SUM(cnt_dmx),
	                SUM(cnt_gil),
	                SUM(s_ob),
	                SUM(tariff),
	                SUM(c_calc),
	                SUM(sum_charge),
                    CASE WHEN service = 'Итого' THEN 1 ELSE 0 END as service_order
                FROM
	                report_charge
                WHERE
	                charge_date = '{0}'
                  AND vill = 'Итого'
                  AND nzp_serv IN (10,9,7,8,6,14,257,11,25,210,-1)
                GROUP BY 2,3
                ORDER BY service_order, service", _dateReport.ToString("yyyy-MM-01"));

            var session = Container.Resolve<ISessionProvider>().GetCurrentSession();

            var mainList = session.CreateSQLQuery(sqlMain).List();
            var main = new DataTable { TableName = "main" };

            main.Columns.AddRange(new[]
            {
                new DataColumn("rajon", typeof(string)),
                new DataColumn("service", typeof(string)),
                new DataColumn("measure", typeof(string)),
                new DataColumn("cnt_dmx", typeof(int)),
                new DataColumn("cnt_gil", typeof(int)),
                new DataColumn("s_ob", typeof(decimal)),
                new DataColumn("tariff", typeof(decimal)),
                new DataColumn("c_calc", typeof(decimal)),
                new DataColumn("sum_charge", typeof(decimal))
            });
            foreach (object[] row in mainList)
            {
                main.Rows.Add(
                    row[0].To<string>(),
                    row[1].To<string>(),
                    row[2].To<string>(),
                    row[3].To<int>(),
                    row[4].To<int>(),
                    row[5].To<decimal>(),
                    row[6].To<decimal>(),
                    row[7].To<decimal>(),
                    row[8].To<decimal>());
            }

            var summaryList = session.CreateSQLQuery(sqlSumary).List();
            var summary = new DataTable { TableName = "summary" };

            summary.Columns.AddRange(new[]
            {
                new DataColumn("rajon", typeof(string)),
                new DataColumn("service", typeof(string)),
                new DataColumn("measure", typeof(string)),
                new DataColumn("cnt_dmx", typeof(int)),
                new DataColumn("cnt_gil", typeof(int)),
                new DataColumn("s_ob", typeof(decimal)),
                new DataColumn("tariff", typeof(decimal)),
                new DataColumn("c_calc", typeof(decimal)),
                new DataColumn("sum_charge", typeof(decimal))
            });
            foreach (object[] row in summaryList)
            {
                summary.Rows.Add(
                    row[0].To<string>(),
                    row[1].To<string>(),
                    row[2].To<string>(),
                    row[3].To<int>(),
                    row[4].To<int>(),
                    row[5].To<decimal>(),
                    row[6].To<decimal>(),
                    row[7].To<decimal>(),
                    row[8].To<decimal>());
            }

            Report.RegBusinessObject("Параметры Отчета", new { ReportDate = _dateReport });

            var dataset = new DataSet();
            dataset.Tables.Add(main);
            dataset.Tables.Add(summary);

            Report.Dictionary.DataSources.Clear();
            Report.RegData("data", dataset);
            Report.Dictionary.Synchronize();
        }

        public override string Name
        {
            get { return "Отчет по начислениям(МКД)"; }
        }

        public override string Desciption
        {
            get { return "Отчет по начислениям(МКД) из соц. защиты"; }
        }

        public override string GroupName
        {
            get { return "Отчет из соц. защиты"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.Report_SZ_Charge_MKD"; }
        }

        public override string RequiredPermission
        {
            get { return null; }
        }

        public override byte[] BynaryReportTemplate
        {
            get { return Resources.Report_SZ_Charge_MKD; }
        }

        public override Stream GetTemplate()
        {
            return new MemoryStream(Resources.Report_SZ_Charge_MKD);
        }

        public override void SetUserParams(B4.BaseParams baseParams)
        {
            _dateReport = baseParams.Params["reportDate"].ToDateTime();
        }
    }
}