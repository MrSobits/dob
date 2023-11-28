namespace Bars.Gkh.Gis.Reports.Reports
{
    using System.IO;
    using B4.DataAccess;
    using B4.Modules.Reports;
    using B4.Utils;
    using Properties;
    using Stimulsoft.Report.Dictionary;
    using System;
    using System.Data;

    public class Report_5_37_2 : StimulReportDynamicExcel
    {
        private DateTime _startDateReport = DateTime.MinValue;
        private DateTime _endDateReport = DateTime.MinValue;

        public override void PrepareReport(ReportParams reportParams)
        {
            const string sqlMaster = @"select raj, a.nzp_raj, sum(count_ls) as count_ls,
                sum(sum_oplat) as sum_oplat
                from t1 a, public.subs_rajon_gor b
                where a.nzp_raj=b.nzp_raj
                group by 1,2
                order by 1";

            const string sqlDetail = @"select nzp_raj, (case when (paysource < 0) or (paysource > 7) then 99
                                when  paysource = 0  then 1 else paysource end)
                                      as sort_plat,
                                      (case
                                           when paysource in (0,1) then 'Касса по счету'
                                           when paysource = 2 then 'Касса по номеру лицевого счета'
                                           when paysource = 3 then 'Банкомат по счету'
                                           when paysource = 4 then 'Банкомат по номеру лицевого счета'
                                           when paysource = 5 then 'Интернет по счету'
                                           when paysource = 6 then 'Интернет по номеру лицевого счета'
                                           when paysource = 7 then 'Оплата поставщиков'
                                     else 'Неопределено' end) as type_plat,
                   sum(count_ls) as count_ls,
                   sum(sum_oplat) as sum_oplat
                   from t1 group by 1,2,3 order by 1,2";

            const string sqlSummary = @"select (case when (paysource < 0) or (paysource > 7) then 99
                                when  paysource = 0  then 1 else paysource end)
                                      as sort_plat,
                                      (case
                                           when paysource in (0,1) then 'Касса по счету'
                                           when paysource = 2 then 'Касса по номеру лицевого счета'
                                           when paysource = 3 then 'Банкомат по счету'
                                           when paysource = 4 then 'Банкомат по номеру лицевого счета'
                                           when paysource = 5 then 'Интернет по счету'
                                           when paysource = 6 then 'Интернет по номеру лицевого счета'
                                           when paysource = 7 then 'Оплата поставщиков'
                                     else 'Неопределено' end) as type_plat,
                   sum(count_ls) as count_ls,
                   sum(sum_oplat) as sum_oplat
                   from t1 group by 1,2 order by 1";

//            var session = Container.Resolve<ISessionProvider>().GetCurrentSession(Container.Resolve<IDbConfigProvider>("GisDb").DbName);
//            session.CreateSQLQuery("SELECT public.report_5_37(month, year, month, year);").ExecuteUpdate();
            var session = Container.Resolve<ISessionProvider>().OpenStatelessSession();
            using (var transaction = session.BeginTransaction())
            {

                session.CreateSQLQuery("SELECT public.report_5_37(:startmonth, :startyear, :endmonth, :endyear);")
                    .SetInt32("startmonth", _startDateReport.Month)
                    .SetInt32("startyear", _startDateReport.Year)
                    .SetInt32("endmonth", _endDateReport.Month)
                    .SetInt32("endyear", _endDateReport.Year)
                    .ExecuteUpdate();

                var dataMaster = session.CreateSQLQuery(sqlMaster).List();
                var master = new DataTable { TableName = "master" };
                master.Columns.AddRange(new[]
                {
                    new DataColumn("raj", typeof(string)),
                    new DataColumn("nzp_raj", typeof(int)),
                    new DataColumn("count_ls", typeof(long)),
                    new DataColumn("sum_oplat", typeof(decimal))
                });
                foreach (object[] row in dataMaster)
                {
                    master.Rows.Add(
                        row[0].To<string>(),
                        row[1].To<int>(),
                        row[2].To<long>(),
                        row[3].To<decimal>()
                        );
                }

                var dataDetail = session.CreateSQLQuery(sqlDetail).List();
                var detail = new DataTable { TableName = "detail" };
                detail.Columns.AddRange(new[]
                {
                    new DataColumn("nzp_raj", typeof(int)),
                    new DataColumn("sort_plat", typeof(int)),
                    new DataColumn("type_plat", typeof(string)),
                    new DataColumn("count_ls", typeof(long)),
                    new DataColumn("sum_oplat", typeof(decimal))
                });
                foreach (object[] row in dataDetail)
                {
                    detail.Rows.Add(
                        row[0].To<int>(),
                        row[1].To<int>(),
                        row[2].To<string>(),
                        row[3].To<long>(),
                        row[4].To<decimal>()
                    );
                }

                var dataSummary = session.CreateSQLQuery(sqlSummary).List();
                var summary = new DataTable { TableName = "summary" };
                summary.Columns.AddRange(new[]
                {
                    new DataColumn("sort_plat", typeof(int)),
                    new DataColumn("type_plat", typeof(string)),
                    new DataColumn("count_ls", typeof(long)),
                    new DataColumn("sum_oplat", typeof(decimal))
                });
                foreach (object[] row in dataSummary)
                {
                    summary.Rows.Add(
                        row[0].To<int>(),
                        row[1].To<string>(),
                        row[2].To<long>(),
                        row[3].To<decimal>()
                    );
                }

                Report.Dictionary.DataSources.Clear();
                Report.RegBusinessObject("ПараметрыОтчета", new { StartDate = _startDateReport, FinishDate = _endDateReport });
                var data = new DataSet();
                data.Tables.Add(master);
                data.Tables.Add(detail);
                data.Tables.Add(summary);
                Report.RegData("data", data);

                Report.Dictionary.Synchronize();

                var relationDetail = new StiDataRelation("masterdetail", "masterdetail", "masterdetail", Report.Dictionary.DataSources["master"],
                    Report.Dictionary.DataSources["detail"], new[] { "nzp_raj" }, new[] { "nzp_raj" });
                Report.Dictionary.Relations.Add(relationDetail);
                Report.Dictionary.RegRelations();
            transaction.Commit();
            }
        }

        public override string Name
        {
            get { return "Статистика по платежам за период (по районам)"; }
        }

        public override string Desciption
        {
            get { return "Статистика по платежам за период (по районам)"; }
        }

        public override string GroupName
        {
            get { return "Статистика по платежам"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.Report_5_37_2"; }
        }

        public override string RequiredPermission
        {
            get { return null; }
        }

        public override byte[] BynaryReportTemplate
        {
            get { return Resources.Report_5_37_2; }
        }

        public override Stream GetTemplate()
        {
            return new MemoryStream(Resources.Report_5_37_2);
        }

        public override void SetUserParams(B4.BaseParams baseParams)
        {
            _startDateReport = baseParams.Params["startReportDate"].ToDateTime();
            _endDateReport = baseParams.Params["endReportDate"].ToDateTime();
        }
    }
}