namespace Bars.Gkh.Gis.Reports
{
    using System;
    using System.Collections.Generic;
    using B4;
    using B4.Modules.Reports;

    using Bars.B4.Utils;

    using BillingStimulReport;
    using Entities.RealEstate.GisRealEstateType;
    using Gkh.Entities;
    using Properties;
    using Stimulsoft.Report;

    public class ReportIndicator : BillingReport
    {
        private long _type;
        private List<long> _municipalities;
        private List<long> _indicators;
        private DateTime _dateFrom;
        private DateTime _dateTo;

        public override IList<PrintFormExportFormat> GetExportFormats()
        {
            return new[]
                   {
                       new PrintFormExportFormat { Id = (int)StiExportFormat.Excel,     Name = "Excel"         },
                       new PrintFormExportFormat { Id = (int)StiExportFormat.Excel2007, Name = "Excel 2007"    }//,
                       //new PrintFormExportFormat { Id = (int)StiExportFormat.Pdf,       Name = "Adobe Acrobat" }
                   };
        }

        
        public override void PrepareReport(ReportParams reportParams)
        {
            throw new NotImplementedException("Метод устарел, не работает в данной архитектуре!");
            /*
            // параметры для фильтрации по типу дома
            var commonParams =
                Container.Resolve<IDomainService<GisRealEstateTypeCommonParam>>()
                    .GetAll()
                    .Where(x => x.RealEstateType.Id == _type);
            // индикаторы с ограничениями
            IQueryable<GisRealEstateTypeIndicator> indicators;
            if (_indicators == null)
            {
                indicators = Container.Resolve<IDomainService<GisRealEstateTypeIndicator>>()
                    .GetAll()
                    .Where(x => x.RealEstateType.Id == _type);
            }
            else
            {
                indicators = Container.Resolve<IDomainService<GisRealEstateTypeIndicator>>()
                    .GetAll()
                    .Where(x => x.RealEstateType.Id == _type && _indicators.Contains(x.Id));
            }
            // дома в МЖФ
            var realityObjects =
                Container.Resolve<IDomainService<RealityObject>>()
                    .GetAll()
                    .Where(x => _municipalities.Contains(x.Municipality.Id));

            var filteredRealityObject = new List<long>();

            realityObjects.ForEach(x =>
            {
                var add = false;
                foreach (var param in commonParams)
                {
                    var value = x.GetType().GetProperty(param.CommonParamCode).GetValue(x, null);
                    if (value == null) continue;
                    var type = value.GetType();
                    if (!string.IsNullOrEmpty(param.PrecisionValue))
                    {
                        switch (Type.GetTypeCode(type))
                        {
                            case TypeCode.Byte:
                            case TypeCode.SByte:
                            case TypeCode.UInt16:
                            case TypeCode.UInt32:
                            case TypeCode.UInt64:
                            case TypeCode.Int16:
                            case TypeCode.Int32:
                            case TypeCode.Int64:
                            case TypeCode.Decimal:
                            case TypeCode.Double:
                            case TypeCode.Single:
                                // натуральное число
                                var round = Math.Round(Convert.ToDecimal(value), 25);
                                add = round == Math.Round(Convert.ToDecimal(param.PrecisionValue), 25);
                                break;
                                // дата
                            case TypeCode.DateTime:
                                var date = Convert.ToDateTime(value);
                                add = date ==
                                      DateTime.ParseExact(param.PrecisionValue, "MM/dd/yyyy",
                                          CultureInfo.InvariantCulture);
                                break;
                        }
                    }
                    else
                    {
                        switch (Type.GetTypeCode(type))
                        {
                            case TypeCode.Byte:
                            case TypeCode.SByte:
                            case TypeCode.UInt16:
                            case TypeCode.UInt32:
                            case TypeCode.UInt64:
                            case TypeCode.Int16:
                            case TypeCode.Int32:
                            case TypeCode.Int64:
                            case TypeCode.Decimal:
                            case TypeCode.Double:
                            case TypeCode.Single:
                                // натуральное число
                                var round = Math.Round(Convert.ToDecimal(value), 25);
                                add = round >= Math.Round(Convert.ToDecimal(param.Min), 25) &&
                                      round <= Math.Round(Convert.ToDecimal(param.Max), 25);
                                break;
                                // дата
                            case TypeCode.DateTime:
                                var date = Convert.ToDateTime(value);
                                add = date >= DateTime.ParseExact(param.Min, "MM/dd/yyyy", CultureInfo.InvariantCulture) &&
                                      date <= DateTime.ParseExact(param.Max, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                                break;
                        }
                    }
                }
                if (add) filteredRealityObject.Add(x.Id);
            });

            var billingHouses = Container.Resolve<IDomainService<AddressMatch>>()
                .GetAll()
                .Where(
                    x =>
                        filteredRealityObject.Contains(x.RealityObject.Id) &&
                        x.TypeAddressMatched == TypeAddressMatched.MatchedFound);

            if (!billingHouses.Any() || !indicators.Any() || _dateTo < _dateFrom) return;

            var dataBankManager = Container.Resolve<IDataBankManager>();
            var sql = string.Format(@"SELECT
	                t1.nzp_dom as HouseId,
	                t1.name_prm as Name,
	                CASE
                WHEN resy.name_y IS NULL THEN
	                t1.val_prm
                ELSE
	                resy.name_y
                END AS ValPrm,
                 t1.dat_s as DateBegin,
                 t1.dat_po as DateEnd,
                 t1.type_prm as Type
                FROM
	                (
		                SELECT
			                prm2.nzp_key AS ID,
			                prm2.nzp AS nzp_dom,
                      prmname.type_prm,
			                prmname.name_prm,
			                prm2.val_prm,
			                prm2.dat_s,
			                prm2.dat_po,
			                prmtab.name_tab,
			                prmname.nzp_res
		                FROM
			                $prefix_data.prm_2 prm2,
			                $prefix_kernel.prm_name prmname,
			                $prefix_kernel.prm_table prmtab
		                WHERE
			                prm2.nzp_prm = prmname.nzp_prm
		                AND prmname.prm_num = prmtab.prm_num
		                AND prm2.is_actual <> 100
                        AND prm2.nzp IN ({0})
                        AND dat_s < '{1}'
                        AND dat_po > '{2}'
		                AND prmname.name_prm IN ({3})
	                ) t1
                LEFT JOIN $prefix_kernel.res_y resy ON t1.nzp_res = resy.nzp_res
                AND t1.val_prm = resy.nzp_y :: TEXT",
                string.Join(",", billingHouses.Select(x => x.BillingAddressId)),
                _dateTo.ToString("yyyy-MM-dd"),
                _dateFrom.ToString("yyyy-MM-dd"),
                string.Join(",", indicators.Select(x => "'" + x.RealEstateIndicator.Name + "'")));

            var billingParams = new List<HouseParam>();

            // цикл по префиксам всех локальных банков
            dataBankManager.DataBanks.Where(x => !x.IsCentral).Select(x => x.Prefix).Distinct().ForEach(x =>
            {
                var res = Container.Resolve<IBillingRepository<HouseParam>>()
                    .Bind(x + DataBankPostfixContainer.DataPostfix)
                    .Query(sql.Replace("$prefix", x)).Select(y => new HouseParam
                    {
                        DateBegin = y.DateBegin,
                        DateEnd = y.DateEnd,
                        HouseId = y.HouseId,
                        Name = y.Name,
                        ValPrm = y.ValPrm,
                        Type = y.Type
                    });
                billingParams.AddRange(res);
            });

            var reportData = new List<ReportObject>();
            foreach (var billingHouse in billingHouses)
                foreach (var indicator in indicators)
                {
                    var date = new DateTime(_dateFrom.Year, _dateFrom.Month, 1);
                    var newRange = new List<ReportObject>();
                    while (date <= _dateTo)
                    {
                        var bilPrm =
                            billingParams.FirstOrDefault(
                                x =>
                                    date >= x.DateBegin && date < x.DateEnd &&
                                    x.Name == indicator.RealEstateIndicator.Name &&
                                    x.HouseId == billingHouse.BillingAddressId);
                        if (bilPrm == null)
                        {
                            // не найдено значение для индикатора
                            newRange.Add(new ReportObject
                            {
                                House = billingHouse.RealityObject,
                                Indicator = indicator,
                                IsNormal = true,
                                Value = "",
                                Date = date
                            });
                            date = date.AddMonths(1);
                            continue;
                        }

                        TypeCode type;
                        switch (bilPrm.Type)
                        {
                            case "bool":
                                type = TypeCode.Boolean;
                                break;
                            case "sprav":
                                type = TypeCode.String;
                                break;
                            case "date":
                                type = TypeCode.DateTime;
                                break;
                            case "int":
                                type = TypeCode.Int64;
                                break;
                            case "float":
                                type = TypeCode.Decimal;
                                break;
                            default:
                                type = TypeCode.Empty;
                                break;
                        }

                        if (type == TypeCode.Empty)
                        {
                            // неизвестное значение
                            newRange.Add(new ReportObject
                            {
                                House = billingHouse.RealityObject,
                                Indicator = indicator,
                                IsNormal = true,
                                Value = "",
                                Date = date
                            });
                        }
                        else if (type == TypeCode.Boolean || type == TypeCode.String)
                        {
                            if (indicator.PrecisionValue.IsEmpty())
                            {
                                // на булевское значение только индикатор с точным значением
                                newRange.Add(new ReportObject
                                {
                                    House = billingHouse.RealityObject,
                                    Indicator = indicator,
                                    IsNormal = true,
                                    Value = "",
                                    Date = date
                                });
                            }
                            else
                            {
                                newRange.Add(new ReportObject
                                {
                                    House = billingHouse.RealityObject,
                                    Indicator = indicator,
                                    IsNormal = type == TypeCode.Boolean
                                        ? (indicator.PrecisionValue == bilPrm.ValPrm)
                                        : (Convert.ToDateTime(indicator.PrecisionValue) ==
                                           Convert.ToDateTime(bilPrm.ValPrm)),
                                    Value = Convert.ToString(bilPrm.ValPrm),
                                    Date = date
                                });
                            }
                        }
                        else
                        {
                            if (indicator.PrecisionValue.IsEmpty())
                            {
                                bool normal;
                                switch (type)
                                {
                                    case TypeCode.DateTime:
                                        normal = Convert.ToDateTime(indicator.Min) <= Convert.ToDateTime(bilPrm.ValPrm) &&
                                                 Convert.ToDateTime(indicator.Max) >= Convert.ToDateTime(bilPrm.ValPrm);
                                        break;
                                    case TypeCode.Int64:
                                        normal = Convert.ToInt64(indicator.Min) <= Convert.ToInt64(bilPrm.ValPrm) &&
                                                 Convert.ToInt64(indicator.Max) >= Convert.ToInt64(bilPrm.ValPrm);
                                        break;
                                    case TypeCode.Decimal:
                                        var val = Convert.ToDecimal(Regex.Replace(bilPrm.ValPrm, "[.,]", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator));
                                        var min = Convert.ToDecimal(Regex.Replace(indicator.Min, "[.,]", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator));
                                        var max = Convert.ToDecimal(Regex.Replace(indicator.Max, "[.,]", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator));
                                        normal = min <= val && max >= val;
                                        break;
                                    default:
                                        normal = true;
                                        break;
                                }
                                newRange.Add(new ReportObject
                                {
                                    House = billingHouse.RealityObject,
                                    Indicator = indicator,
                                    IsNormal = normal,
                                    Value = Convert.ToString(bilPrm.ValPrm),
                                    Date = date
                                });
                            }
                            else
                            {
                                newRange.Add(new ReportObject
                                {
                                    House = billingHouse.RealityObject,
                                    Indicator = indicator,
                                    IsNormal =
                                        Convert.ChangeType(indicator.PrecisionValue, type) ==
                                        Convert.ChangeType(bilPrm.ValPrm, type),
                                    Value = Convert.ToString(bilPrm.ValPrm),
                                    Date = date
                                });
                            }
                        }
                        date = date.AddMonths(1);
                    }

                    // если есть несовпадение хотя бы в одном месяце, то добавляем в отчет
                    if (newRange.Any(x => !x.IsNormal))
                    {
                        reportData.AddRange(newRange);
                    }
                }

            var mainTable = ToDataTable(reportData
                .Where(x => x.Date == reportData.Min(y => y.Date))
                .Select(x => new
                {
                    MunicipalityId = x.House.Municipality.Id,
                    MunicipalityName = x.House.Municipality.Name,
                    HouseId = x.House.Id,
                    x.House.Address,
                    x.House.ManOrgs,
                    IndicatorId = x.Indicator.Id,
                    IndicatorName = x.Indicator.RealEstateIndicator.Name,
                    IndicatorValue = x.Indicator.PrecisionValue.IsEmpty()
                        ? string.Format("от {0} до {1}", x.Indicator.Min, x.Indicator.Max)
                        : x.Indicator.PrecisionValue
                })
                .ToList());
            mainTable.TableName = "Main";

            var detailTable = ToDataTable(reportData.Select(x => new
            {
                HouseId = x.House.Id,
                IndicatorId = x.Indicator.Id,
                x.Date,
                x.Value,
                x.IsNormal
            }).ToList());
            detailTable.TableName = "Detail";

            var monthTable = ToDataTable(reportData.Select(x => x.Date).Distinct().OrderBy(x => x).ToList());
            monthTable.TableName = "Months";

            var housesType = Container.Resolve<IDomainService<GisRealEstateType>>().Get(_type).Name;

            Report.RegBusinessObject("ПараметрыОтчета", new { housesType });

            var dataset = new DataSet();
            dataset.Tables.Add(mainTable);
            dataset.Tables.Add(detailTable);
            dataset.Tables.Add(monthTable);

            Report.Dictionary.DataSources.Clear();
            Report.RegData("data", dataset);
            Report.Dictionary.Synchronize();

            var relationDetail = new StiDataRelation("MainDetail", "MainDetail", "MainDetail", Report.Dictionary.DataSources["Main"],
                    Report.Dictionary.DataSources["Detail"], new[] { "HouseId", "IndicatorId" }, new[] { "HouseId", "IndicatorId" });
            Report.Dictionary.Relations.Add(relationDetail);
            Report.Dictionary.RegRelations();
            */
        }

        public override string Name
        {
            get { return "Отчет по индикаторам"; }
        }

        public override string Desciption
        {
            get { return "Отчет по индикаторам"; }
        }

        public override string GroupName
        {
            get { return "Отчет по индикаторам"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.Report_SZ_Indicator"; }
        }

        public override string RequiredPermission
        {
            get { return null; }
        }

        protected override byte[] BynaryReportTemplate
        {
            get { return Resources.ReportIndicator; }
        }
        public override void SetUserParams(BaseParams baseParams)
        {
            var reportParams = baseParams.Params.GetAs<DynamicDictionary>("ReportParams");
            _type = reportParams.GetAs<long>("type");
            _municipalities = reportParams.GetAs<List<long>>("municipalities");
            _indicators = reportParams.GetAs<List<long>>("indicators");
            _dateFrom = reportParams.GetAs<DateTime>("dateFrom");
            _dateTo = reportParams.GetAs<DateTime>("dateTo");
        }

        private class ReportObject
        {
            public bool IsNormal { get; set; }
            public string Value { get; set; }
            public RealityObject House { get; set; }

            public DateTime Date { get; set; }
            public GisRealEstateTypeIndicator Indicator { get; set; }
        }
    }
}