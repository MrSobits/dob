namespace Bars.Gkh.Gis.Reports.Reports
{
    using System.IO;
    using B4.DataAccess;
    using B4.Modules.Reports;
    using B4.Utils;
    using Properties;
    using System;
    using System.Data;

    public class Report_SZ_Collection_Service : StimulReportDynamicExcel
    {
        private DateTime _dateReport = DateTime.MinValue;
        private int _typeReport = 0;

        public override void PrepareReport(ReportParams reportParams)
        {
            var mode = _typeReport == 10 ? 1 : 2; // 1 - все, 2 - без газа
            var sqlMain = string.Format(@"SELECT
	                rajon,
                    service,
	                cnt_dmx,
	                cnt_dmx_pay,
	                sum_insaldo - sum_money,
	                sum_charge,
                    sum_money,
	                sum_money_next,
                    0 as service_order
                FROM
	                report_collection,
	                sr_rajon
                WHERE
	                collect_date = '{0}'
                AND rajon_code = kod_raj
                AND uk = 'Итого'
                AND uk_podr = 'Итого'
                AND nzp_serv NOT IN ({1}28,29,30,31,32,100,200,201,202,203,204,207,208,250,215,12,13,230,27,-1)
                UNION ALL
                SELECT
                    rajon,
                    service,
                    {2},
                    {3},
	                SUM(sum_insaldo - sum_money),
	                SUM(sum_charge),
                    SUM(sum_money),
	                SUM(sum_money_next),
                    1 as service_order
                FROM
	                report_collection,
	                sr_rajon
                WHERE
	                collect_date = '{0}'
                AND rajon_code = kod_raj
                AND uk = 'Итого'
                AND uk_podr = 'Итого'
                AND nzp_serv = -1
                GROUP BY 1,2,3,4
                ORDER BY 1,service_order,2",
                _dateReport.ToString("yyyy-MM-01"),
                mode == 1 ? "" : "25,10,209,210,",
                mode == 1 ? "cnt_dmx_subs" : "cnt_dmx_subs_gas",
                mode == 1 ? "cnt_dmx_pay_subs" : "cnt_dmx_pay_subs_gas");

            var sqlSummary = string.Format(@"SELECT
	                'Все районы',
                    service,
                    SUM(cnt_dmx) as cnt_dmx,
                    SUM(cnt_dmx_pay) as cnt_dmx_pay,
	                SUM(sum_insaldo - sum_money) as sum_insaldo,
	                SUM(sum_charge) as sum_charge,
                    SUM(sum_money) as sum_money,
	                SUM(sum_money_next) as sum_money_next,
                  0 as rajon_order
                FROM
	                report_collection
                WHERE
	                collect_date = '{0}'
                AND uk = 'Итого'
                AND uk_podr = 'Итого'
                AND nzp_serv NOT IN ({1}28,29,30,31,32,100,200,201,202,203,204,207,208,250,215,12,13,230,27,-1)
                GROUP BY 2
                UNION ALL
                SELECT
                    'Все районы',
                    service,
                    SUM({2}),
                    SUM({3}),
	                SUM(sum_insaldo - sum_money),
	                SUM(sum_charge),
                    SUM(sum_money),
	                SUM(sum_money_next),
                  1 as rajon_order
                FROM
	                report_collection
                WHERE
	                collect_date = '{0}'
                    AND uk = 'Итого'
                    AND uk_podr = 'Итого'
                    AND nzp_serv = -1 
                GROUP BY 1,2
                ORDER BY rajon_order,2",
                _dateReport.ToString("yyyy-MM-01"),
                mode == 1 ? "" : "25,10,209,210,",
                mode == 1 ? "cnt_dmx_subs" : "cnt_dmx_subs_gas",
                mode == 1 ? "cnt_dmx_pay_subs" : "cnt_dmx_pay_subs_gas");

            var session = Container.Resolve<ISessionProvider>().GetCurrentSession();

            var mainList = session.CreateSQLQuery(sqlMain).List();
            var main = new DataTable { TableName = "main" };

            main.Columns.AddRange(new[]
            {
                new DataColumn("rajon", typeof(string)),
                new DataColumn("service", typeof(string)),
                new DataColumn("cnt_dmx", typeof(int)),
                new DataColumn("cnt_dmx_pay", typeof(int)),
                new DataColumn("sum_insaldo", typeof(decimal)),
                new DataColumn("sum_charge", typeof(decimal)),
                new DataColumn("sum_money", typeof(decimal)),
                new DataColumn("sum_money_next", typeof(decimal))
            });
            foreach (object[] row in mainList)
            {
                main.Rows.Add(
                    row[0].To<string>(),
                    row[1].To<string>(),
                    row[2].To<int>(),
                    row[3].To<int>(),
                    row[4].To<decimal>(),
                    row[5].To<decimal>(),
                    row[6].To<decimal>(),
                    row[7].To<decimal>()
                );
            }

            var summaryList = session.CreateSQLQuery(sqlSummary).List();
            var summary = new DataTable { TableName = "summary" };

            summary.Columns.AddRange(new[]
            {
                new DataColumn("rajon", typeof(string)),
                new DataColumn("service", typeof(string)),
                new DataColumn("cnt_dmx", typeof(int)),
                new DataColumn("cnt_dmx_pay", typeof(int)),
                new DataColumn("sum_insaldo", typeof(decimal)),
                new DataColumn("sum_charge", typeof(decimal)),
                new DataColumn("sum_money", typeof(decimal)),
                new DataColumn("sum_money_next", typeof(decimal))
            });
            foreach (object[] row in summaryList)
            {
                summary.Rows.Add(
                    row[0].To<string>(),
                    row[1].To<string>(),
                    row[2].To<int>(),
                    row[3].To<int>(),
                    row[4].To<decimal>(),
                    row[5].To<decimal>(),
                    row[6].To<decimal>(),
                    row[7].To<decimal>()
                );
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
            get { return "Отчет по собираемости начислений субсидируемых услуг "; }
        }

        public override string Desciption
        {
            get { return "Отчет по собираемости начислений субсидируемых услуг из соц. защиты"; }
        }

        public override string GroupName
        {
            get { return "Отчет из соц. защиты"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.Report_SZ_Collection_Service"; }
        }

        public override string RequiredPermission
        {
            get { return null; }
        }

        public override byte[] BynaryReportTemplate
        {
            get { return Resources.Report_SZ_Collection_Service; }
        }

        public override Stream GetTemplate()
        {
            return new MemoryStream(Resources.Report_SZ_Collection_Service);
        }

        public override void SetUserParams(B4.BaseParams baseParams)
        {
            _dateReport = baseParams.Params["reportDate"].ToDateTime();
            _typeReport = baseParams.Params["reportType"].ToInt();
        }
    }
}