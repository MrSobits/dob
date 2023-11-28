namespace Bars.Gkh.Gis.Reports.Reports
{
    using System.IO;
    using B4.DataAccess;
    using B4.Modules.Reports;
    using B4.Utils;
    using Properties;
    using System;
    using System.Data;

    public class Report_SZ_Charge : StimulReportDynamicExcel
    {
        private DateTime _dateReport = DateTime.MinValue;

        public override void PrepareReport(ReportParams reportParams)
        {
            var sqlMain = string.Format(@"SELECT
	                rajon,
	                vill,
	                service,
	                measure,
	                cnt_dmx,
	                cnt_gil,
	                s_ob,
	                tariff,
	                c_calc,
	                sum_charge,
                    CASE WHEN vill = 'Итого' THEN 1 ELSE 0 END as vill_order,
                    CASE WHEN service = 'Итого' THEN 1 ELSE 0 END as service_order
                FROM
	                report_charge,
	                sr_rajon
                WHERE
	                rajon_code = kod_raj
                  AND charge_date = '{0}'
                ORDER BY rajon, vill_order, vill, service_order, service", _dateReport.ToString("yyyy-MM-01"));

            var sqlSummary = string.Format(@"SELECT
	                'Все районы',
	                'Итого',
	                service,
	                measure,
	                sum(cnt_dmx),
	                sum(cnt_gil),
	                sum(s_ob),
	                sum(tariff),
	                sum(c_calc),
	                sum(sum_charge),
                    CASE WHEN service = 'Итого' THEN 1 ELSE 0 END as service_order
                FROM
	                report_charge
                WHERE
	                charge_date = '{0}'
                  AND vill = 'Итого'
                GROUP BY 3,4
                ORDER BY service_order, service", _dateReport.ToString("yyyy-MM-01"));

            var session = Container.Resolve<ISessionProvider>().GetCurrentSession();

            var mainList = session.CreateSQLQuery(sqlMain).List();
            var main = new DataTable { TableName = "main" };

            main.Columns.AddRange(new[]
            {
                new DataColumn("rajon", typeof(string)),
                new DataColumn("vill", typeof(string)),
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
                    row[3].To<string>(),
                    row[4].To<int>(),
                    row[5].To<int>(),
                    row[6].To<decimal>(),
                    row[7].To<decimal>(),
                    row[8].To<decimal>(),
                    row[9].To<decimal>());
            }

            var summaryList = session.CreateSQLQuery(sqlSummary).List();
            var summary = new DataTable { TableName = "summary" };

            summary.Columns.AddRange(new[]
            {
                new DataColumn("rajon", typeof(string)),
                new DataColumn("vill", typeof(string)),
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
                    row[3].To<string>(),
                    row[4].To<int>(),
                    row[5].To<int>(),
                    row[6].To<decimal>(),
                    row[7].To<decimal>(),
                    row[8].To<decimal>(),
                    row[9].To<decimal>());
            }

            Report.RegBusinessObject("ПараметрыОтчета", new { ReportDate = _dateReport });

            var dataset = new DataSet();
            dataset.Tables.Add(main);
            dataset.Tables.Add(summary);

            Report.Dictionary.DataSources.Clear();
            Report.RegData("data", dataset);
            Report.Dictionary.Synchronize();
        }

        public override string Name
        {
            get { return "Отчет по начислениям"; }
        }

        public override string Desciption
        {
            get { return "Отчет по начислениям из соц. защиты"; }
        }

        public override string GroupName
        {
            get { return "Отчет из соц. защиты"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.Report_SZ_Charge"; }
        }

        public override string RequiredPermission
        {
            get { return null; }
        }

        public override byte[] BynaryReportTemplate
        {
            get { return Resources.Report_SZ_Charge; }
        }

        public override Stream GetTemplate()
        {
            return new MemoryStream(Resources.Report_SZ_Charge);
        }

        public override void SetUserParams(B4.BaseParams baseParams)
        {
            _dateReport = baseParams.Params["reportDate"].ToDateTime();
        }
    }
}