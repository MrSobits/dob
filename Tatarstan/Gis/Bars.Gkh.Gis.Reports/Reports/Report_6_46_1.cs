namespace Bars.Gkh.Gis.Reports.Reports
{
    using System.Collections;
    using System.IO;
    using B4.DataAccess;
    using B4.Modules.Reports;
    using B4.Utils;

    using Castle.Windsor;
    using Properties;
    using System;
    using System.Data;

    public class Report_6_46_1 : StimulReportDynamicExcel
    {
        public IWindsorContainer Container { get; set; }

        private DateTime _startDateReport = DateTime.MinValue;
        private DateTime _endDateReport = DateTime.MinValue;

        public Stream GetTemplate()
        {
            return new MemoryStream(Resources.Report_6_46_1);
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var sql = String.Format("select raj as point, " +
                                    "service, " +
                                    "sum(a.cntdom_all) as cntdom_all, " +
                                    "sum(a.cntdom_nodpu)  as cntdom_nodpu, " +
                                    "sum(a.cntdom_nodpu_odn)    as cntdom_nodpu_odn, " +
                                    "sum(a.cntdom_dpu)    as cntdom_dpu, " +
                                    "sum(a.cntdom_dpu_odn)  as cntdom_dpu_odn, " +
                                    "sum(a.dky) as dky " +
                                    "from public.resp_pu a  " +
                                    "where a.nzp_serv = 25 " +
                                    "and a.year_ >= {0} " +
                                    "and a.month_ >= {1} " +
                                    "and a.year_ <= {2} " +
                                    "and a.month_ <= {3} " +
                                    "group by 1,2 order by 2",
                                    _startDateReport.Year,
                                    _startDateReport.Month,
                                    _endDateReport.Year,
                                    _endDateReport.Month); // доступные услуги 6,8,9,25

            //var session =
            //    Container.Resolve<ISessionProvider>()
            //        .GetCurrentSession(
            //            Container.Resolve<IDbConfigProvider>(TypeAdditionalDb.SecondDb.GetEnumMeta().Display).DbName);
            //IList dataList;
            //using (var transaction = session.BeginTransaction())
            //{
            //    var date = _startDateReport;
            //    var endDate = _endDateReport.AddMonths(1);
            //    do
            //    {
            //        session.CreateSQLQuery("SELECT public.report_6_46_1(:pref, :nzp_raj, :month, :year);")
            //            // todo здесь надо сделать выбор региона
            //            .SetString("pref", "avia36")
            //            .SetInt32("nzp_raj", 45)
            //            .SetInt32("month", _startDateReport.Month)
            //            .SetInt32("year", _startDateReport.Year)
            //            .ExecuteUpdate();
            //        date = date.AddMonths(1);
            //    } while (date.Month != endDate.Month || date.Year != endDate.Year);
            //    dataList = session.CreateSQLQuery(sql).List();
            //    transaction.Commit();
            //}

            //var data = new DataTable { TableName = "main" };

            //data.Columns.AddRange(new[]
            //{
            //    new DataColumn("point", typeof(string)),
            //    new DataColumn("service", typeof(string)),
            //    new DataColumn("cntdom_all", typeof(int)),
            //    new DataColumn("cntdom_nodpu", typeof(int)),
            //    new DataColumn("cntdom_nodpu_odn", typeof(int)),
            //    new DataColumn("cntdom_dpu", typeof(int)),
            //    new DataColumn("cntdom_dpu_odn", typeof(int)),
            //    new DataColumn("dky", typeof(int))
            //});
            //foreach (object[] row in dataList)
            //{
            //    data.Rows.Add(
            //        row[0].To<string>(),
            //        row[1].To<string>(),
            //        row[2].To<int>(),
            //        row[3].To<int>(),
            //        row[4].To<int>(),
            //        row[5].To<int>(),
            //        row[6].To<int>(),
            //        row[7].To<int>());
            //}

            //Report.RegBusinessObject("ПараметрыОтчета", new { StartDate = _startDateReport, FinishDate = _endDateReport });

            //var dataset = new DataSet();
            //dataset.Tables.Add(data);

            //Report.Dictionary.DataSources.Clear();
            //Report.RegData("data", data);
            //Report.Dictionary.Synchronize();
        }

        public override string Name
        {
            get { return "Статистика по приборам учета за период"; }
        }

        public override string Desciption
        {
            get { return "Статистика по приборам учета за период"; }
        }

        public override string GroupName
        {
            get { return "Статистика по приборам учета"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.Report_6_46_1"; }
        }

        public override string RequiredPermission
        {
            get { return null; }
        }

        public override byte[] BynaryReportTemplate
        {
            get { return Resources.Report_6_46_1; }
        }

        public override void SetUserParams(B4.BaseParams baseParams)
        {
            _startDateReport = baseParams.Params["startReportDate"].ToDateTime();
            _endDateReport = baseParams.Params["endReportDate"].ToDateTime();
        }
    }
}
