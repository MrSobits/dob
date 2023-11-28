namespace Bars.Gkh.Gis.Reports.Reports
{
    using System.IO;
    using B4.DataAccess;
    using B4.Modules.Reports;
    using B4.Utils;
    using Properties;
    using Stimulsoft.Report.Components;
    using System;
    using System.Data;

    public class Report_SZ_Indicator : StimulReportDynamicExcel
    {
        private DateTime _dateReport = DateTime.MinValue;

        public override void PrepareReport(ReportParams reportParams)
        {
            Report.Pages[0].ExcelSheet = new StiExcelSheetExpression("Форма 1");
            Report.Pages[1].ExcelSheet = new StiExcelSheetExpression("Форма 1(п)");
            Report.Pages[2].ExcelSheet = new StiExcelSheetExpression("Форма 2");
            Report.Pages[3].ExcelSheet = new StiExcelSheetExpression("Форма 4");

            const string vill = "АКСАРИНСКОЕ";
            const string area = "Заинский";
            const int rajonCode = 20;
            const string note = @"Примечание: в отчет не включены домохозяйства с количеством проживающих равным 0.";
            const int nzpStandType = 2; // тип отчета

            var sqlInd1 = string.Format(@"WITH serv5 AS
                (
                SELECT
	              nzp_packet,
                  nzp_serv,
                  COALESCE(max(all_dmx),0) all_dmx,
                  COALESCE(sum(sum_fact),0) sum_fact,
                  COALESCE(sum(case when nzp_serv<>242 then c_calc else 0 end),0) c_calc,
                  COALESCE(sum(case when nzp_serv<>242 then cnt_gil_dmx else 0 end),0) cnt_gil_dmx,
                  COALESCE(sum(case when nzp_serv<>242 then cnt_dmx else 0 end),0) cnt_dmx
                FROM
	              public.report_indicator1
                WHERE
	              rajon_code = {0}
                  AND calc_date = '{1}'
                  {2}
                  AND nzp_serv IN (5, 242, 243)
                  AND nzp_packet <> 0
                  AND nzp_stand_type = {4}
                GROUP BY 1,2
                ORDER BY 1,2
                )
                SELECT DISTINCT
	                A.n_str,
                    A .nzp_serv,
	                A.nzp_packet,
	                A .f_1 as service,
	                A. packet as packet,
                  CASE
                    WHEN A.nzp_serv = 5 AND 20 IN (51,52,53)
                    THEN
                      case when coalesce(serv5.c_calc,0)>0 then (serv5.sum_fact/serv5.c_calc) else 0 end
                    ELSE
                      case when coalesce(ri1.c_calc,0)>0 then (ri1.sum_fact/ri1.c_calc) else 0 end
                  END as tarif,
                  CASE
                    WHEN A.nzp_serv = 5 AND 20 IN (51,52,53)
                    THEN
                      0
                    ELSE
                      case when coalesce(ri1.cnt_gil_dmx_sn_m,0)>0 then (ri1.c_sn_m/ri1.cnt_gil_dmx_sn_m) else 0 end
                  END as c_sn,
                  CASE
                    WHEN A.nzp_serv = 5 AND 20 IN (51,52,53)
                    THEN
                      case when coalesce(serv5.all_dmx,0)>0 then (cast(serv5.cnt_dmx as float)/serv5.all_dmx) else 0 end
                    ELSE
                      case when coalesce(ri1.all_dmx,0)>0 then (cast(ri1.cnt_dmx as float)/cast(ri1.all_dmx as float)) else 0 end
                  END as doly_dmx
                FROM
	                (select sif.nzp_serv, sif.f_1, MIN(sif.n_str) as n_str, p.nzp_packet, p.packet
                  from s_ind_fields sif, s_packet p
                  where nzp_serv > 0 and sif.nzp_rep = 1
                  group by 1,2,4) as A
                  LEFT JOIN report_indicator1 ri1 ON ri1.nzp_packet = A.nzp_packet AND ri1.nzp_serv = A.nzp_serv
                    AND ri1.rajon_code = {0}
                    AND ri1.calc_date = '{1}'
                    {3}
                    AND ri1.nzp_serv > 0
                    AND ri1.nzp_packet <> 0
                    AND nzp_stand_type = {4}
                  LEFT JOIN serv5 ON serv5.nzp_packet = A.nzp_packet AND serv5.nzp_serv = A.nzp_serv
                ORDER BY
	                n_str,
	                A .nzp_serv,
	                A.nzp_packet",
                rajonCode,
                _dateReport.ToString("yyyy-MM-01"),
                string.IsNullOrWhiteSpace(vill) ? "" : string.Format("AND vill = '{0}'", vill),
                string.IsNullOrWhiteSpace(vill) ? "" : string.Format("AND ri1.vill = '{0}'", vill),
                nzpStandType);

            const string sqlInd1PMain = @"select n_str, f_1, f_2, cnt_gil from public.s_ind_fields where nzp_rep=5 order by 1";
            var sqlInd1PDetail = string.Format(@"with form1 as
                (
                select calc_date, vill, nzp_packet, cnt_gil, cast(stand_cost as numeric(14,2)) as stand_cost,
                       sum(cnt_gil_dmx) cnt_gil_dmx,
                       sum(sum_fact) sum_fact,
                       sum(sum_fact_subs) sum_fact_subs
                from  report_indicator_dmx t
                where 
                t.rajon_code={0}
                {2}
                and t.calc_date = '{1}'
                and nzp_stand_type = {3}
                group by calc_date, vill, nzp_packet, cnt_gil, stand_cost
                )
                select
                  spg.cnt_gil,
                  spg.nzp_packet,
                  form1.stand_cost,
                  case when coalesce(form1.cnt_gil_dmx,0)>0 then form1.sum_fact_subs/form1.cnt_gil_dmx else 0 end fact_cost,
                  case when (coalesce(form1.cnt_gil_dmx,0)>0) and (coalesce(form1.stand_cost,0)>0) then ((form1.sum_fact_subs/form1.cnt_gil_dmx)/form1.stand_cost)*100 else 0 end ind_1_3
                from
                (select cnt_gil, cnt_gil_name, nzp_packet, packet from s_packet, s_cnt_gil) as spg
                left join form1 on spg.cnt_gil = form1.cnt_gil and spg.nzp_packet = form1.nzp_packet
                order by spg.cnt_gil, spg.nzp_packet",
                rajonCode,
                _dateReport.ToString("yyyy-MM-01"),
                string.IsNullOrWhiteSpace(vill) ? "" : string.Format("AND vill = '{0}'", vill),
                nzpStandType);

            const string sqlInd2Main = @"select n_str, f_1, f_2 from public.s_ind_fields where nzp_rep=2 order by 1";
            var sqlInd2Detail = string.Format(@"select
                            g.cnt_gil,
                            case when coalesce(f.cnt_gil_dmx,0)*f.s_stand<>0 then 100*((f.s_ob - (f.s_ob_max10 + f.s_ob_min10))/((1-2*0.1)*f.cnt_gil_dmx))/f.s_stand else -1 end ind_2_1,
                            case when coalesce(f.cnt_gil_dmx,0)>0 then (f.s_ob - (f.s_ob_max10 + f.s_ob_min10))/((1-2*0.1)*f.cnt_gil_dmx) else 0 end s_ob_gil,
                            f.s_ob,
                            f.s_ob_max10,
                            f.s_ob_min10,
                            0.1 * 100 doly_maxmin,
                            f.cnt_gil_dmx,
                            f.s_stand s_stand,
                            case when coalesce(f.cnt_gil_dmx,0)>0 then (cast(f.cnt_gil_vstand as numeric)/cast(f.cnt_gil_dmx as numeric)) * 100 else 0 end doly_gil_vstand,
                            f.cnt_gil_vstand,
                            case when coalesce(f.cnt_gil_dmx,0)>0 then (cast(f.cnt_gil_nstand as numeric)/cast(f.cnt_gil_dmx as numeric)) * 100 else 0 end doly_gil_nstand,
                            f.cnt_gil_nstand,
                            case when coalesce(f.cnt_gil_dmx,0)>0 then (cast(f.cnt_gil_rstand as numeric)/cast(f.cnt_gil_dmx as numeric)) * 100 else 0 end doly_gil_rstand,
                            f.cnt_gil_rstand
                     from s_cnt_gil g
                          left outer join report_indicator2 f
                            on g.cnt_gil=f.cnt_gil
                            and f.calc_date = '{1}'
                            {2}
                            and f.nzp_packet=-1
                            and f.rajon_code={0}
                            and nzp_stand_type = {3}
                     where (g.cnt_gil>=1 and g.cnt_gil<=5)
                     order by g.cnt_gil",
                rajonCode,
                _dateReport.ToString("yyyy-MM-01"),
                string.IsNullOrWhiteSpace(vill) ? "" : string.Format("AND vill = '{0}'", vill),
                nzpStandType);

            const string sqlInd4Main = @"select n_str, f_1, f_2 from public.s_ind_fields where nzp_rep=4 order by 1";
            var sqlInd4Detail = string.Format(@"select
                                   p.cnt_gil, p.nzp_packet,
                                   coalesce(f.stand_cost,0) stand_cost,
                                   coalesce(f.cnt_gil_smo, 0) cnt_gil_smo,
                                   coalesce(f.cnt_gil_smo_zaj, 0) cnt_gil_smo_zaj
                            from (select cnt_gil, cnt_gil_name, nzp_packet, packet from s_packet, s_cnt_gil) as p
                            left outer join report_indicator4 f
                                on f.cnt_gil=p.cnt_gil and f.nzp_packet=p.nzp_packet
                            and calc_date = '{1}'
                            {2}
                            and rajon_code={0}
                            and nzp_stand_type = {3}
                        where (p.cnt_gil>=1 and p.cnt_gil<=5)
                        order by p.nzp_packet, p.cnt_gil",
                rajonCode,
                _dateReport.ToString("yyyy-MM-01"),
                string.IsNullOrWhiteSpace(vill) ? "" : string.Format("AND vill = '{0}'", vill),
                nzpStandType);
            var sqlInd4PDetail = string.Format(@"select g.cnt_gil,
                                f.s_stand, f.mdd
                            from s_cnt_gil g
                            left outer join report_indicator4p f
                                on f.cnt_gil=g.cnt_gil
                                and f.calc_date = '{1}'
                                {2}
                                and f.rajon_code={0}
                     where (g.cnt_gil>=1 and g.cnt_gil<=5)
                     order by g.cnt_gil",
                rajonCode,
                _dateReport.ToString("yyyy-MM-01"),
                string.IsNullOrWhiteSpace(vill) ? "" : string.Format("AND vill = '{0}'", vill));

            var session = Container.Resolve<ISessionProvider>().GetCurrentSession();

            var ind1List = session.CreateSQLQuery(sqlInd1).List();
            var ind1 = new DataTable { TableName = "ind1" };

            ind1.Columns.AddRange(new[]
            {
                new DataColumn("n_str", typeof(int)),
                new DataColumn("nzp_serv", typeof(int)),
                new DataColumn("nzp_packet", typeof(int)),
                new DataColumn("service", typeof(string)),
                new DataColumn("packet", typeof(string)),
                new DataColumn("tarif", typeof(decimal)),
                new DataColumn("c_sn", typeof(decimal)),
                new DataColumn("doly_dmx", typeof(decimal))
            });
            foreach (object[] row in ind1List)
            {
                ind1.Rows.Add(
                    row[0].To<int>(),
                    row[1].To<int>(),
                    row[2].To<int>(),
                    row[3].To<string>(),
                    row[4].To<string>(),
                    row[5].To<decimal>(),
                    row[6].To<decimal>(),
                    row[7].To<decimal>()
                );
            }

            var ind1PMainList = session.CreateSQLQuery(sqlInd1PMain).List();
            var ind1P = new DataTable { TableName = "ind1P" };

            ind1P.Columns.AddRange(new[]
            {
                new DataColumn("n_str", typeof(int)),
                new DataColumn("f_1", typeof(string)),
                new DataColumn("f_2", typeof(string)),
                new DataColumn("cnt_gil", typeof(int)),
                new DataColumn("packet_1", typeof(string)),
                new DataColumn("packet_2", typeof(string)),
                new DataColumn("packet_3", typeof(string)),
                new DataColumn("packet_4", typeof(string))
            });
            foreach (object[] row in ind1PMainList)
            {
                ind1P.Rows.Add(
                    row[0].To<int>(),
                    row[1].To<string>(),
                    row[2].To<string>(),
                    row[3].To<int>()
                );
            }

            var ind1PDetailList = session.CreateSQLQuery(sqlInd1PDetail).List();
            var ind1PDetail = new DataTable { TableName = "ind1PDetail" };

            ind1PDetail.Columns.AddRange(new[]
            {
                new DataColumn("cnt_gil", typeof(int)),
                new DataColumn("nzp_packet", typeof(int)),
                new DataColumn("stand_cost", typeof(string)),
                new DataColumn("fact_cost", typeof(string)),
                new DataColumn("ind_1_3", typeof(string))
            });
            foreach (object[] row in ind1PDetailList)
            {
                ind1PDetail.Rows.Add(
                    row[0].To<int>(),
                    row[1].To<int>(),
                    row[2].To<string>(),
                    row[3].To<string>(),
                    row[4].To<string>()
                );
            }

            foreach (DataRow row in ind1PDetail.Rows)
            {
                var index = Convert.ToInt32(row["cnt_gil"]);
                ind1P.Rows[index]["packet_" + row["nzp_packet"]] = row["ind_1_3"];
                ind1P.Rows[index + 6]["packet_" + row["nzp_packet"]] = row["fact_cost"];
                ind1P.Rows[index + 12]["packet_" + row["nzp_packet"]] = row["stand_cost"];
            }

            var ind2MainList = session.CreateSQLQuery(sqlInd2Main).List();
            var ind2 = new DataTable { TableName = "ind2" };

            ind2.Columns.AddRange(new[]
            {
                new DataColumn("n_str", typeof(int)),
                new DataColumn("f_1", typeof(string)),
                new DataColumn("f_2", typeof(string)),
                new DataColumn("cnt_gil_1", typeof(string)),
                new DataColumn("cnt_gil_2", typeof(string)),
                new DataColumn("cnt_gil_3", typeof(string)),
                new DataColumn("cnt_gil_4", typeof(string)),
                new DataColumn("cnt_gil_5", typeof(string))
            });
            foreach (object[] row in ind2MainList)
            {
                ind2.Rows.Add(
                    row[0].To<int>(),
                    row[1].To<string>(),
                    row[2].To<string>()
                );
            }

            var ind2DetailList = session.CreateSQLQuery(sqlInd2Detail).List();
            var ind2Detail = new DataTable { TableName = "ind2Detail" };

            ind2Detail.Columns.AddRange(new[]
            {
                new DataColumn("cnt_gil", typeof(int)),
                new DataColumn("ind_2_1", typeof(string)),
                new DataColumn("s_ob_gil", typeof(string)),
                new DataColumn("s_ob", typeof(string)),
                new DataColumn("s_ob_max10", typeof(string)),
                new DataColumn("s_ob_min10", typeof(string)),
                new DataColumn("doly_maxmin", typeof(string)),
                new DataColumn("cnt_gil_dmx", typeof(string)),
                new DataColumn("s_stand", typeof(string)),
                new DataColumn("doly_gil_vstand", typeof(string)),
                new DataColumn("cnt_gil_vstand", typeof(string)),
                new DataColumn("doly_gil_nstand", typeof(string)),
                new DataColumn("cnt_gil_nstand", typeof(string)),
                new DataColumn("doly_gil_rstand", typeof(string)),
                new DataColumn("cnt_gil_rstand", typeof(string))
            });
            foreach (object[] row in ind2DetailList)
            {
                ind2Detail.Rows.Add(
                    row[0].To<string>(),
                    row[1].To<string>(),
                    row[2].To<string>(),
                    row[3].To<string>(),
                    row[4].To<string>(),
                    row[5].To<string>(),
                    row[6].To<string>(),
                    row[7].To<string>(),
                    row[8].To<string>(),
                    row[9].To<string>(),
                    row[10].To<string>(),
                    row[11].To<string>(),
                    row[12].To<string>(),
                    row[13].To<string>(),
                    row[14].To<string>()
                );
            }

            foreach (DataRow row in ind2Detail.Rows)
            {
                ind2.Rows[0]["cnt_gil_" + row["cnt_gil"]] = row["ind_2_1"];
                ind2.Rows[1]["cnt_gil_" + row["cnt_gil"]] = row["s_ob_gil"];
                ind2.Rows[2]["cnt_gil_" + row["cnt_gil"]] = row["s_ob"];
                ind2.Rows[3]["cnt_gil_" + row["cnt_gil"]] = row["s_ob_max10"];
                ind2.Rows[4]["cnt_gil_" + row["cnt_gil"]] = row["s_ob_min10"];
                ind2.Rows[5]["cnt_gil_" + row["cnt_gil"]] = row["doly_maxmin"];
                ind2.Rows[6]["cnt_gil_" + row["cnt_gil"]] = row["cnt_gil_dmx"];
                ind2.Rows[7]["cnt_gil_" + row["cnt_gil"]] = row["s_stand"];
                ind2.Rows[8]["cnt_gil_" + row["cnt_gil"]] = row["doly_gil_vstand"];
                ind2.Rows[9]["cnt_gil_" + row["cnt_gil"]] = row["cnt_gil_vstand"];
                ind2.Rows[10]["cnt_gil_" + row["cnt_gil"]] = row["doly_gil_nstand"];
                ind2.Rows[11]["cnt_gil_" + row["cnt_gil"]] = row["cnt_gil_nstand"];
                ind2.Rows[12]["cnt_gil_" + row["cnt_gil"]] = row["doly_gil_rstand"];
                ind2.Rows[13]["cnt_gil_" + row["cnt_gil"]] = row["cnt_gil_rstand"];
            }

            var ind4MainList = session.CreateSQLQuery(sqlInd4Main).List();
            var ind4 = new DataTable { TableName = "ind4" };

            ind4.Columns.AddRange(new[]
            {
                new DataColumn("n_str", typeof(int)),
                new DataColumn("f_1", typeof(string)),
                new DataColumn("f_2", typeof(string)),
                new DataColumn("cnt_gil_1", typeof(string)),
                new DataColumn("cnt_gil_2", typeof(string)),
                new DataColumn("cnt_gil_3", typeof(string)),
                new DataColumn("cnt_gil_4", typeof(string)),
                new DataColumn("cnt_gil_5", typeof(string))
            });
            foreach (object[] row in ind4MainList)
            {
                ind4.Rows.Add(
                    row[0].To<int>(),
                    row[1].To<string>(),
                    row[2].To<string>()
                );
            }

            var ind4PDetailList = session.CreateSQLQuery(sqlInd4PDetail).List();
            var ind4PDetail = new DataTable { TableName = "ind4PDetail" };

            ind4PDetail.Columns.AddRange(new[]
            {
                new DataColumn("cnt_gil", typeof(int)),
                new DataColumn("s_stand", typeof(int)),
                new DataColumn("mdd", typeof(int))
            });
            foreach (object[] row in ind4PDetailList)
            {
                ind4PDetail.Rows.Add(
                    row[0].To<int>(),
                    row[1].To<int>(),
                    row[2].To<int>()
                );
            }

            var ind4DetailList = session.CreateSQLQuery(sqlInd4Detail).List();
            var ind4Detail = new DataTable { TableName = "ind4Detail" };

            ind4Detail.Columns.AddRange(new[]
            {
                new DataColumn("cnt_gil", typeof(int)),
                new DataColumn("nzp_packet", typeof(int)),
                new DataColumn("stand_cost", typeof(int)),
                new DataColumn("cnt_gil_smo", typeof(int)),
                new DataColumn("cnt_gil_smo_zaj", typeof(int))
            });
            foreach (object[] row in ind4DetailList)
            {
                ind4Detail.Rows.Add(
                    row[0].To<int>(),
                    row[1].To<int>(),
                    row[2].To<int>(),
                    row[3].To<int>(),
                    row[4].To<int>()
                );
            }

            for (var i = 1; i <= 5; i++)
            {
                var index = "cnt_gil_" + i;
                ind4.Rows[0][index] = Convert.ToInt32(ind4PDetail.Rows[i - 1]["s_stand"]);
                ind4.Rows[1][index] = 100;
                ind4.Rows[2][index] = Convert.ToInt32(ind4PDetail.Rows[i - 1]["mdd"]);
                ind4.Rows[3][index] = 100;

                for (var j = 1; j <= 4; j++)
                {
                    ind4.Rows[4 + j][index] = Convert.ToDecimal(ind4Detail.Rows[(j - 1) * 5 + i - 1]["stand_cost"]).ToString("0.00");
                    ind4.Rows[9 + j][index] = Convert.ToInt32(ind4Detail.Rows[(j - 1) * 5 + i - 1]["cnt_gil_smo_zaj"]);
                }
            }

            Report.RegBusinessObject("ПараметрыОтчета", new { ReportDate = _dateReport, Area = area, Vill = string.IsNullOrWhiteSpace(vill) ? "Всего" : vill, Note = note });

            var dataset = new DataSet();
            dataset.Tables.Add(ind1);
            dataset.Tables.Add(ind1P);
            dataset.Tables.Add(ind2);
            dataset.Tables.Add(ind4);

            Report.Dictionary.DataSources.Clear();
            Report.RegData("data", dataset);
            Report.Dictionary.Synchronize();
        }

        public override string Name
        {
            get { return "Отчет по индикаторам"; }
        }

        public override string Desciption
        {
            get { return "Отчет по индикаторам из соц. защиты"; }
        }

        public override string GroupName
        {
            get { return "Отчет из соц. защиты"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.Report_SZ_Indicator"; }
        }

        public override string RequiredPermission
        {
            get { return null; }
        }

        public override byte[] BynaryReportTemplate
        {
            get { return Resources.Report_SZ_Indicator; }
        }

        public override Stream GetTemplate()
        {
            return new MemoryStream(Resources.Report_SZ_Indicator);
        }

        public override void SetUserParams(B4.BaseParams baseParams)
        {
            _dateReport = baseParams.Params["reportDate"].ToDateTime();
        }
    }
}