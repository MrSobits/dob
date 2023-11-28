namespace Bars.Gkh.Gis.Reports.Reports
{
    using System.IO;
    using B4.DataAccess;
    using B4.Modules.Reports;
    using B4.Utils;
    using Properties;
    using System;
    using System.Data;

    public class Report_SZ_Collection : StimulReportDynamicExcel
    {
        private DateTime _dateReport = DateTime.MinValue;

        public override void PrepareReport(ReportParams reportParams)
        {
            var sqlMain = string.Format(@"SELECT
	                rajon,
	                cnt_dmx,
	                cnt_dmx_pay,
	                sum_insaldo - sum_money,
	                sum_charge,
                    sum_money,
	                sum_money_next
                FROM
	                report_collection,
	                sr_rajon
                WHERE
	                collect_date = '{0}'
                AND rajon_code = kod_raj
                AND uk = 'Итого'
                AND uk_podr = 'Итого'
                AND service = 'Итого'", _dateReport.ToString("yyyy-MM-01"));

            var session = Container.Resolve<ISessionProvider>().GetCurrentSession();
            var mainList = session.CreateSQLQuery(sqlMain).List();

            var main = new DataTable { TableName = "main" };

            main.Columns.AddRange(new[]
            {
                new DataColumn("rajon", typeof(string)),
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
                    row[1].To<int>(),
                    row[2].To<int>(),
                    row[3].To<decimal>(),
                    row[4].To<decimal>(),
                    row[5].To<decimal>(),
                    row[6].To<decimal>()
                );
            }

            Report.RegBusinessObject("ПараметрыОтчета", new { ReportDate = _dateReport });

            var dataset = new DataSet();
            dataset.Tables.Add(main);

            Report.Dictionary.DataSources.Clear();
            Report.RegData("data", dataset);
            Report.Dictionary.Synchronize();
        }

        public override string Name
        {
            get { return "Отчет по собираемости начислений"; }
        }

        public override string Desciption
        {
            get { return "Отчет по собираемости начислений из соц. защиты"; }
        }

        public override string GroupName
        {
            get { return "Отчет из соц. защиты"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.Report_SZ_Collection"; }
        }

        public override string RequiredPermission
        {
            get { return null; }
        }

        public override byte[] BynaryReportTemplate
        {
            get { return Resources.Report_SZ_Collection; }
        }

        public override Stream GetTemplate()
        {
            return new MemoryStream(Resources.Report_SZ_Collection);
        }


        public override void SetUserParams(B4.BaseParams baseParams)
        {
            _dateReport = baseParams.Params["reportDate"].ToDateTime();
        }
    }
}