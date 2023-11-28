namespace Bars.Gkh.Gis.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using B4.DataAccess;
    using B4.Modules.Reports;
    using B4.Utils;

    using Reports;
    using Stimulsoft.Report;
    using Stimulsoft.Report.Dictionary;
    using Properties;

    public class Report_MKD : StimulReportDynamicExcel
    {
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
            //const string sqlMaster = @"SELECT DISTINCT
	           //                     ( CASE WHEN T .kod_raj = - 1 THEN 0 ELSE 1 END ) as kod_raj_sort, 
            //                        COALESCE ( r.rajon, 'Все районы' ) as rajon, 
            //                        ( CASE WHEN T .nzp_serv = - 1 THEN 1 ELSE 0 END ) as nzp_serv_sort, 
            //                        COALESCE (sv.service, 'Итого') as service,
            //                        COALESCE (M .measure, '') as measure,
            //                        t.kod_raj, t.nzp_serv, t.nzp_measure
            //                        FROM public.uk201401_nach_raj T
            //                        LEFT JOIN public.uk_rajon r ON T .kod_raj = r.kod_raj 
            //                        LEFT JOIN public.uk_services sv ON T .nzp_serv = sv.nzp_serv 
            //                        LEFT JOIN public.uk_measure M ON T .nzp_measure = M .nzp_measure 
            //                        WHERE T .is_mkd = 1 
            //                        ORDER BY 1,2,3,4;";

            //const string sqlDetail = @"SELECT
	           //                         T .kod_raj,
	           //                         T .nzp_serv,
            //                            T.nzp_measure,
            //                            T.dat_calc,
	           //                         SUM (cnt_dmx) as cnt_dmx,
	           //                         SUM (cnt_gil) as cnt_gil,
	           //                         SUM (sq) as sq,
	           //                         SUM (tarif) as tarif,
	           //                         SUM (c_calc) as c_calc,
	           //                         SUM (sum_charge) as sum_charge
            //                        FROM
	           //                         t_uk201401_nach_raj T
            //                        GROUP BY
	           //                         1,
	           //                         2,
            //                            3,
            //                            4
            //                        ORDER BY
	           //                         1,
	           //                         2,
            //                            3,
            //                            4";

            //var session =
            //    Container.Resolve<ISessionProvider>()
            //        .GetCurrentSession(
            //            Container.Resolve<IDbConfigProvider>(TypeAdditionalDb.SecondDb.GetEnumMeta().Display).DbName);
            //session.CreateSQLQuery("SELECT public.report_mkd (2013);").ExecuteUpdate();

            //var dataMaster = session.CreateSQLQuery(sqlMaster).List();
            //var master = new DataTable { TableName = "master" };
            //master.Columns.AddRange(new[]
            //{
            //    new DataColumn("kod_raj_sort", typeof(int)),
            //    new DataColumn("rajon", typeof(string)),
            //    new DataColumn("nzp_serv_sort", typeof(int)),
            //    new DataColumn("service", typeof(string)),
            //    new DataColumn("measure", typeof(string)),
            //    new DataColumn("kod_raj", typeof(int)),
            //    new DataColumn("nzp_serv", typeof(int)),
            //    new DataColumn("nzp_measure", typeof(int))
            //});
            //foreach (object[] row in dataMaster)
            //{
            //    master.Rows.Add(
            //        row[0].To<int>(),
            //        row[1].To<string>(),
            //        row[2].To<int>(),
            //        row[3].To<string>(),
            //        row[4].To<string>(),
            //        row[5].To<int>(),
            //        row[6].To<int>(),
            //        row[7].To<int>()
            //        );
            //}

            //var dataDetail = session.CreateSQLQuery(sqlDetail).List();
            //var detail = new DataTable { TableName = "detail" };
            //detail.Columns.AddRange(new[]
            //{
            //    new DataColumn("kod_raj", typeof(int)),
            //    new DataColumn("nzp_serv", typeof(int)),
            //    new DataColumn("nzp_measure", typeof(int)),
            //    new DataColumn("dat_calc", typeof(DateTime)),
            //    new DataColumn("cnt_dmx", typeof(decimal)),
            //    new DataColumn("cnt_gil", typeof(decimal)),
            //    new DataColumn("sq", typeof(decimal)),
            //    new DataColumn("tarif", typeof(decimal)),
            //    new DataColumn("c_calc", typeof(decimal)),
            //    new DataColumn("sum_charge", typeof(decimal))
            //});
            //foreach (object[] row in dataDetail)
            //{
            //    detail.Rows.Add(
            //        row[0].To<int>(),
            //        row[1].To<int>(),
            //        row[2].To<int>(),
            //        row[3].To<DateTime>(),
            //        row[4].To<decimal>(),
            //        row[5].To<decimal>(),
            //        row[6].To<decimal>(),
            //        row[7].To<decimal>(),
            //        row[8].To<decimal>(),
            //        row[9].To<decimal>()
            //    );
            //}

            //Report.Dictionary.DataSources.Clear();
            //Report.RegBusinessObject("ПараметрыОтчета", new { Date = new DateTime(2013, 1, 1), Service = "Водоснабжение" });
            //var data = new DataSet();
            //data.Tables.Add(master);
            //data.Tables.Add(detail);
            //Report.RegData("data", data);

            //Report.Dictionary.Synchronize();

            //var relation = new StiDataRelation("masterdetail", "masterdetail", "masterdetail", Report.Dictionary.DataSources["master"],
            //    Report.Dictionary.DataSources["detail"], new[] { "kod_raj", "nzp_serv", "nzp_measure" }, new[] { "kod_raj", "nzp_serv", "nzp_measure" });
            //Report.Dictionary.Relations.Add(relation);
            //Report.Dictionary.RegRelations();
        }

        public override string Name
        {
            get { return "Информация по ЦХД по начислению МКД"; }
        }

        public override string Desciption
        {
            get { return "Информация по ЦХД по начислению МКД"; }
        }

        public override string GroupName
        {
            get { return "Статистика по приборам учета"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.Report_MKD"; }
        }

        public override string RequiredPermission
        {
            get { return null; }
        }

        public override byte[] BynaryReportTemplate
        {
            get { return Resources.Report_MKD; }
        }

        public override Stream GetTemplate()
        {
            return new MemoryStream(Resources.Report_MKD);
        }
    }
}