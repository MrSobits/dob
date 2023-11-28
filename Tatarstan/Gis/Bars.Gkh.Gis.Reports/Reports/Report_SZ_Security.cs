namespace Bars.Gkh.Gis.Reports.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using B4.DataAccess;
    using B4.Modules.Reports;
    using B4.Utils;
    using Properties;
    using Stimulsoft.Report;
    using Stimulsoft.Report.Dictionary;

    public class Report_SZ_Security : StimulReportDynamicExcel
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
           
            const string vill = "Г. КАЗАНЬ";
            const string sqlMaster = @"SELECT DISTINCT service
	            FROM report_security
	            WHERE
		            nzp_serv > 0
	            ORDER BY 1";
            const int rajonCode = 2;

            var sqlDetail = string.Format(@"WITH services AS (
	                SELECT DISTINCT
		                service,
		                nzp_packet,
		                cnt_gil
	                FROM
		                report_security
	                WHERE
		                nzp_packet <> 0
                        AND nzp_serv > 0
                ) SELECT
	                services.*, T .cnt_dmx,
	                T .cnt_dmx_gil,
	                T .square_house,
	                T .charge_sn,
	                T .charge_sn_fact
                FROM
	                services
                LEFT JOIN (
	                SELECT
		                service,
		                nzp_packet,
		                cnt_gil,
		                SUM(cnt_dmx) as cnt_dmx,
		                SUM(cnt_dmx_gil) as cnt_dmx_gil,
		                SUM(square_house) as square_house,
		                SUM(charge_sn) as charge_sn,
		                SUM(charge_sn_fact) as charge_sn_fact
	                FROM
		                report_security
	                WHERE
		                calc_date = '{0}'
                    {1}
	                AND rajon_code = {2}
	                AND nzp_packet <> 0
                    AND nzp_serv > 0
                  GROUP BY
                    1,2,3
                ) AS T ON T .service = services.service
                 AND T .nzp_packet = services.nzp_packet
                AND T .cnt_gil = services.cnt_gil
                ORDER BY
	                1,2,3,4,5,6",
                      _dateReport.ToString("yyyy-MM-01"),
                      string.IsNullOrWhiteSpace(vill) ? "" : string.Format("AND vill = '{0}'", vill), rajonCode);

            const string sqlPackets = @"SELECT DISTINCT nzp_packet
                FROM report_security
                WHERE nzp_packet <> 0
                ORDER BY 1";

            const string sqlCntGils = @"SELECT DISTINCT nzp_packet, cnt_gil
                FROM report_security
                WHERE nzp_packet <> 0
                ORDER BY 1,2";

            var sqlSummary = string.Format(@"
	                SELECT
		                service,
		                nzp_packet,
		                cnt_gil,
		                SUM(cnt_dmx) as cnt_dmx,
		                SUM(cnt_dmx_gil) as cnt_dmx_gil,
		                SUM(square_house) as square_house,
		                SUM(charge_sn) as charge_sn,
		                SUM(charge_sn_fact) as charge_sn_fact
	                FROM
		                report_security
	                WHERE
		                calc_date = '{0}'
	                AND rajon_code = {2}
	                AND nzp_packet <> 0
                    {1}
                    AND nzp_serv < 0
                    GROUP BY 1,2,3
                    ORDER BY 1,2,3",
                _dateReport.ToString("yyyy-MM-01"),
                string.IsNullOrWhiteSpace(vill) ? "" : string.Format("AND vill = '{0}'", vill), rajonCode);

            var session = Container.Resolve<ISessionProvider>().GetCurrentSession();

            var masterList = session.CreateSQLQuery(sqlMaster).List();
            var master = new DataTable { TableName = "master" };

            master.Columns.AddRange(new[]
            {
                new DataColumn("service", typeof(string))
            });
            foreach (var row in masterList)
            {
                master.Rows.Add(
                    row.To<string>()
                );
            }

            var detailList = session.CreateSQLQuery(sqlDetail).List();
            var detail = new DataTable { TableName = "detail" };

            detail.Columns.AddRange(new[]
            {
                new DataColumn("service", typeof(string)),
                new DataColumn("nzp_packet", typeof(int)),
                new DataColumn("cnt_gil", typeof(int)),
                new DataColumn("cnt_dmx", typeof(int)),
                new DataColumn("cnt_dmx_gil", typeof(int)),
                new DataColumn("square_house", typeof(decimal)),
                new DataColumn("charge_sn", typeof(decimal)),
                new DataColumn("charge_sn_fact", typeof(decimal))
            });
            foreach (object[] row in detailList)
            {
                detail.Rows.Add(
                    row[0].To<string>(),
                    row[1].To<int>(),
                    row[2].To<int>(),
                    row[3].To<int>(),
                    row[4].To<int>(),
                    row[5].To<decimal>(),
                    row[6].To<decimal>(),
                    row[7].To<decimal>()
                );
            }

            var packetsList = session.CreateSQLQuery(sqlPackets).List();
            var packets = new DataTable { TableName = "packets" };

            packets.Columns.AddRange(new[]
            {
                new DataColumn("nzp_packet", typeof(int))

            });
            foreach (var row in packetsList)
            {
                packets.Rows.Add(
                    row.To<int>()
                );
            }

            var cntgilsList = session.CreateSQLQuery(sqlCntGils).List();
            var cntgils = new DataTable { TableName = "cnt_gils" };

            cntgils.Columns.AddRange(new[]
            {
                new DataColumn("nzp_packet", typeof(int)),
                new DataColumn("cnt_gil", typeof(int))

            });
            foreach (object[] row in cntgilsList)
            {
                cntgils.Rows.Add(
                    row[0].To<int>(),
                    row[1].To<int>()
                );
            }

            var summaryList = session.CreateSQLQuery(sqlSummary).List();
            var summary = new DataTable { TableName = "summary" };

            summary.Columns.AddRange(new[]
            {
                new DataColumn("service", typeof(string)),
                new DataColumn("nzp_packet", typeof(int)),
                new DataColumn("cnt_gil", typeof(int)),
                new DataColumn("cnt_dmx", typeof(int)),
                new DataColumn("cnt_dmx_gil", typeof(int)),
                new DataColumn("square_house", typeof(decimal)),
                new DataColumn("charge_sn", typeof(decimal)),
                new DataColumn("charge_sn_fact", typeof(decimal))

            });
            foreach (object[] row in summaryList)
            {
                summary.Rows.Add(
                    row[0].To<string>(),
                    row[1].To<int>(),
                    row[2].To<int>(),
                    row[3].To<int>(),
                    row[4].To<int>(),
                    row[5].To<decimal>(),
                    row[6].To<decimal>(),
                    row[7].To<decimal>()
                );
            }

            Report.RegBusinessObject("ПараметрыОтчета", new { ReportDate = _dateReport, Area = "", Vill = string.IsNullOrWhiteSpace(vill) ? "Всего" : vill });

            var dataset = new DataSet();
            dataset.Tables.Add(master);
            dataset.Tables.Add(detail);
            dataset.Tables.Add(packets);
            dataset.Tables.Add(cntgils);
            dataset.Tables.Add(summary);

            Report.Dictionary.DataSources.Clear();
            Report.RegData("data", dataset);
            Report.Dictionary.Synchronize();

            var relation = new StiDataRelation("masterdetail", "masterdetail", "masterdetail", Report.Dictionary.DataSources["master"],
               Report.Dictionary.DataSources["detail"], new[] { "service" }, new[] { "service" });
            Report.Dictionary.Relations.Add(relation);
            Report.Dictionary.RegRelations();
        }

        public override string Name
        {
            get { return "Отчет по обеспеченности услугами"; }
        }

        public override string Desciption
        {
            get { return "Отчет по обеспеченности начислений из соц. защиты"; }
        }

        public override string GroupName
        {
            get { return "Отчет из соц. защиты"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.Report_SZ_Security"; }
        }

        public override string RequiredPermission
        {
            get { return null; }
        }

        public override byte[] BynaryReportTemplate
        {
            get { return Resources.Report_SZ_Security; }
        }

        public override Stream GetTemplate()
        {
            return new MemoryStream(Resources.Report_SZ_Security);
        }

        public override void SetUserParams(B4.BaseParams baseParams)
        {
            _dateReport = baseParams.Params["reportDate"].ToDateTime();
        }
    }
}