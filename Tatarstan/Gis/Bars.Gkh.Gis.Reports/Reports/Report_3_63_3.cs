namespace Bars.Gkh.Gis.Reports.Reports
{
    using System;
    using System.Data;
    using System.IO;
    using B4.DataAccess;
    using B4.Modules.Reports;
    using B4.Utils;
    using Properties;
    using Stimulsoft.Report.Dictionary;

    public class Report_3_63_3 : StimulReportDynamicExcel
    {
        //Пользовательский параметр ДатаОтчета
        private DateTime _dateReport = DateTime.MinValue;

        public override Stream GetTemplate()
        {
            return new MemoryStream(Resources.Report_3_63_3);
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            const string sqlMaster = "select raj, a.nzp_raj " +
                                     "from t1 a, public.subs_rajon_gor b " +
                                     "where a.nzp_raj=b.nzp_raj " +
                                     "group by 1,2 order by 1";

            const string sqlDetail = @"select nzp_raj, 0 as kod_, sum(count_dom) as count_dom,
                sum(count_ls) as count_ls, sum(sum_tarif) as sum_tarif,
                sum(sum_odn) as sum_odn,
                sum(rvaldlt) as rvaldlt,
                sum(c_calc) as c_calc,
                sum(nodn) as nodn
                from t1
                group by 1,2

                union all
                select distinct nzp_raj, 9 as kod_, sum(count_dom) as count_dom,
                sum(count_ls) as count_ls, sum(sum_tarif) as sum_tarif,
                sum(sum_odn) as sum_odn,
                sum(rvaldlt) as rvaldlt,
                sum(c_calc) as c_calc,
                sum(nodn) as nodn
                from t1
                where coalesce(is_dpu,0) = 1
                group by 1

                union all
                select nzp_raj, max(1) as kod_, sum(count_dom) as count_dom,
                sum(count_ls) as count_ls, sum(sum_tarif) as sum_tarif,
                sum(sum_odn) as sum_odn,
                sum(rvaldlt) as rvaldlt,
                sum(c_calc) as c_calc,
                sum(nodn) as nodn
                from t1
                where coalesce(is_dpu,0) <> 1 
                group by 1
                union all
                select nzp_raj, kod_, sum(count_dom) as count_dom, sum(count_ls) as count_ls, sum(sum_tarif) as sum_tarif,
                sum(sum_odn) as sum_odn,
                sum(rvaldlt) as rvaldlt,
                sum(c_calc) as c_calc,
                sum(nodn) as nodn
                from t1
                group by 1,2
                order by 1,2";

            const string sqlSummary = @"select 0 as kod_, sum(count_dom) as count_dom,
                sum(count_ls) as count_ls, sum(sum_tarif) as sum_tarif,
                sum(sum_odn) as sum_odn,
                sum(rvaldlt) as rvaldlt,
                sum(c_calc) as c_calc,
                sum(nodn) as nodn
                from t1
                group by 1

                union all
                select distinct 9 as kod_, sum(count_dom) as count_dom,
                sum(count_ls) as count_ls, sum(sum_tarif) as sum_tarif,
                sum(sum_odn) as sum_odn,
                sum(rvaldlt) as rvaldlt,
                sum(c_calc) as c_calc,
                sum(nodn) as nodn
                from t1
                where coalesce(is_dpu,0) = 1

                union all
                select  max(1) as kod_, sum(count_dom) as count_dom,
                sum(count_ls) as count_ls, sum(sum_tarif) as sum_tarif,
                sum(sum_odn) as sum_odn,
                sum(rvaldlt) as rvaldlt,
                sum(c_calc) as c_calc,
                sum(nodn) as nodn
                from t1
                where coalesce(is_dpu,0) <> 1 
                union all
                select  kod_, sum(count_dom) as count_dom, sum(count_ls) as count_ls, sum(sum_tarif) as sum_tarif,
                sum(sum_odn) as sum_odn,
                sum(rvaldlt) as rvaldlt,
                sum(c_calc) as c_calc,
                sum(nodn) as nodn
                from t1
                group by 1
                order by 1";

            var session = Container.Resolve<ISessionProvider>().OpenStatelessSession();
            using (var transaction = session.BeginTransaction())
            {
                session.CreateSQLQuery("SELECT * FROM public.report_3_63_3(:month, :year);")
                    .SetInt32("month", _dateReport.Month)
                    .SetInt32("year", _dateReport.Year)
                    .ExecuteUpdate();

                var dataMaster = session.CreateSQLQuery(sqlMaster).List();

                var master = new DataTable {TableName = "master"};
                master.Columns.AddRange(new[]
                {
                    new DataColumn("raj", typeof (string)),
                    new DataColumn("nzp_raj", typeof (int))
                });

                foreach (object[] row in dataMaster)
                {
                    master.Rows.Add(row[0].To<string>(), row[1].To<int>());
                }

                var dataDetail = session.CreateSQLQuery(sqlDetail).List();
                var detail = new DataTable {TableName = "detail"};
                detail.Columns.AddRange(new[]
                {
                    new DataColumn("nzp_raj", typeof (int)),
                    new DataColumn("kod_", typeof (int)),
                    new DataColumn("count_dom", typeof (int)),
                    new DataColumn("count_ls", typeof (int)),
                    new DataColumn("sum_tarif", typeof (decimal)),
                    new DataColumn("sum_odn", typeof (decimal)),
                    new DataColumn("rvaldlt", typeof (decimal)),
                    new DataColumn("c_calc", typeof (decimal)),
                    new DataColumn("nodn", typeof (decimal))
                });

                foreach (object[] row in dataDetail)
                {
                    detail.Rows.Add(
                        row[0].To<int>(),
                        row[1].To<int>(),
                        row[2].To<int>(),
                        row[3].To<int>(),
                        row[4].To<decimal>(),
                        row[5].To<decimal>(),
                        row[6].To<decimal>(),
                        row[7].To<decimal>(),
                        row[8].To<decimal>()
                        );
                }

                var dataSummary = session.CreateSQLQuery(sqlSummary).List();
                var summary = new DataTable {TableName = "summary"};
                summary.Columns.AddRange(new[]
                {
                    new DataColumn("kod_", typeof (int)),
                    new DataColumn("count_dom", typeof (int)),
                    new DataColumn("count_ls", typeof (int)),
                    new DataColumn("sum_tarif", typeof (decimal)),
                    new DataColumn("sum_odn", typeof (decimal)),
                    new DataColumn("rvaldlt", typeof (decimal)),
                    new DataColumn("c_calc", typeof (decimal)),
                    new DataColumn("nodn", typeof (decimal))
                });
                foreach (object[] row in dataSummary)
                {
                    summary.Rows.Add(
                        row[0].To<int>(),
                        row[1].To<int>(),
                        row[2].To<int>(),
                        row[3].To<decimal>(),
                        row[4].To<decimal>(),
                        row[5].To<decimal>(),
                        row[6].To<decimal>(),
                        row[7].To<decimal>()
                        );
                }

                Report.Dictionary.DataSources.Clear();
                Report.RegBusinessObject("ПараметрыОтчета", new {Date = _dateReport, Service = "Водоснабжение"});
                var data = new DataSet();
                data.Tables.Add(master);
                data.Tables.Add(detail);
                data.Tables.Add(summary);
                Report.RegData("data", data);

                Report.Dictionary.Synchronize();

                var relation = new StiDataRelation("masterdetail", "masterdetail", "masterdetail",
                    Report.Dictionary.DataSources["master"],
                    Report.Dictionary.DataSources["detail"], new[] {"nzp_raj"}, new[] {"nzp_raj"});
                Report.Dictionary.Relations.Add(relation);
                Report.Dictionary.RegRelations();
            }
        }

        public override string Name
        {
            get { return "Статистика по ОДН за период"; }
        }

        public override string Desciption
        {
            get { return "Статистика по ОДН за период"; }
        }

        public override string GroupName
        {
            get { return "Статистика по приборам учета"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.Report_3_63_3"; }
        }

        public override string RequiredPermission
        {
            get { return null; }
        }

        public override byte[] BynaryReportTemplate
        {
            get { return Resources.Report_3_63_3; }
        }

        public override void SetUserParams(B4.BaseParams baseParams)
        {
            _dateReport = baseParams.Params["reportDate"].ToDateTime();
        }
    }
}
