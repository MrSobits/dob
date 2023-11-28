namespace Bars.GkhCr.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Enums;
    using Bars.GkhCr.Entities;

    using Castle.Windsor;

    /// <summary>
    /// Отчет МО о ходе работ (по домам)
    /// </summary>
    public class WorksProgressReport : BasePrintForm
    {
        // идентификатор программы КР
        private long programCrId;
        private long[] municipalityIds;
        private long[] finSourceIds;
        private DateTime reportDate = DateTime.MinValue;

        public WorksProgressReport()
            : base(new ReportTemplateBinary(Properties.Resources.WorksProgress))
        {
        }

        public IWindsorContainer Container { get; set; }

        public override string RequiredPermission
        {
            get { return "Reports.CR.WorksProgress"; }
        }

        public override string Desciption
        {
            get { return "Отчет МО о ходе работ (по домам)"; }
        }

        public override string GroupName
        {
            get { return "Отчеты ГЖИ"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.WorksProgress"; }
        }

        public override string Name
        {
            get { return "Отчет МО о ходе работ (по домам)"; }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            programCrId = baseParams.Params["programCrId"].ToInt();

            var dateReportPar = baseParams.Params["reportDate"].ToDateTime();

            var dateReport = dateReportPar == DateTime.Today.Date ? DateTime.Today.AddDays(1) : dateReportPar;

            reportDate = dateReport != DateTime.MinValue ? dateReport : DateTime.Now;

            var municipalityIdsList = baseParams.Params.ContainsKey("municipalityIds")
                                  ? baseParams.Params["municipalityIds"].ToString()
                                  : string.Empty;

            this.municipalityIds = !string.IsNullOrEmpty(municipalityIdsList) ? municipalityIdsList.Split(',').Select(id => id.ToLong()).ToArray() : new long[0];

            var finSourceIdsList = baseParams.Params.ContainsKey("finSourceIds")
                      ? baseParams.Params["finSourceIds"].ToString()
                      : string.Empty;

            this.finSourceIds = !string.IsNullOrEmpty(finSourceIdsList) ? finSourceIdsList.Split(',').Select(id => id.ToLong()).ToArray() : new long[0];
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var programCr = this.Container.Resolve<IDomainService<ProgramCr>>().Get(this.programCrId);
            var programCrName = programCr.Name;
            var programCrYear = programCr.Period.DateEnd.HasValue
                                    ? programCr.Period.DateEnd.Value.Year
                                    : this.reportDate.Date.Year;

            var workCodes = new List<string>();
            for (int i = 1; i <= 23; i++)
            {
                workCodes.Add(i.ToStr());
            }
            workCodes.Add("999");

            var works = Container.Resolve<IDomainService<Work>>()
                         .GetAll()
                         .Where(x => x.TypeWork == TypeWork.Work)
                         .Where(x => workCodes.Contains(x.Code))
                         .Select(x => new { x.Id, x.Name, UnitMeasureName = x.UnitMeasure.Name, x.Code })
                         .ToList();

            var queryTypeWorkCr = Container.Resolve<IDomainService<TypeWorkCr>>()
                            .GetAll()
                            .Where(x => x.ObjectCr.ProgramCr.Id == programCrId && x.Work.TypeWork == TypeWork.Work)
                            .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.ObjectCr.RealityObject.Municipality.Id))
                            .WhereIf(finSourceIds.Length > 0, x => finSourceIds.Contains(x.FinanceSource.Id))
                            .Where(x => workCodes.Contains(x.Work.Code));

            var queryTypeWorkCrIds = queryTypeWorkCr.Select(x => x.Id);

            var typeWorks = queryTypeWorkCr.Select(x => new
                                                        {
                                                            TypeWorkId = x.Id,
                                                            Municipality = x.ObjectCr.RealityObject.Municipality.Name,
                                                            x.ObjectCr.GjiNum,
                                                            x.ObjectCr.RealityObject.Address,
                                                            RealObjId = x.ObjectCr.RealityObject.Id,
                                                            WorkId = x.Work.Id,
                                                            FinanceSourceId = x.FinanceSource.Id,
                                                            FinanceSourceName = x.FinanceSource.Name,
                                                            x.VolumeOfCompletion,
                                                            x.PercentOfCompletion,
                                                            x.CostSum,
                                                            x.Sum
                                                        })
                                        .AsEnumerable()                                        
                                        .GroupBy(x => new { x.RealObjId, x.FinanceSourceId, x.GjiNum, x.Address, x.FinanceSourceName, x.Municipality })
                                        .OrderBy(x => x.Key.Municipality)
                                        .ThenBy(x => x.Key.Address)
                                        .ThenBy(x => x.Key.FinanceSourceName)
                                        .ToDictionary(x => x.Key, y => y.GroupBy(x => new { x.TypeWorkId, x.WorkId })
                                                                        .ToDictionary(x => x.Key, z => new
                                                                                                    {
                                                                                                        PercentOfCompletion = z.Sum(v => v.PercentOfCompletion),
                                                                                                        CostSum = z.Sum(v => v.CostSum),
                                                                                                        VolumeOfCompletion = z.Sum(v => v.VolumeOfCompletion),
                                                                                                        Sum = z.Sum(v => v.Sum)
                                                                                                    }));

            var archiveSmr = Container.Resolve<IDomainService<ArchiveSmr>>()
                         .GetAll()
                         .Where(x => queryTypeWorkCrIds.Contains(x.TypeWorkCr.Id))
                         .Where(x => x.DateChangeRec <= reportDate)
                         .Select(x => new
                                     {
                                         archId = x.Id,
                                         TypeWorkCrId = x.TypeWorkCr.Id,
                                         FinanceSourceId = x.TypeWorkCr.FinanceSource.Id,
                                         x.DateChangeRec,
                                         WorkId = x.TypeWorkCr.Work.Id,
                                         x.VolumeOfCompletion,
                                         x.PercentOfCompletion,
                                         x.TypeWorkCr.Sum,
                                         x.CostSum
                                     })
                         .AsEnumerable()
                         .GroupBy(x => x.TypeWorkCrId)
                         .ToDictionary(x => x.Key, y => y.OrderByDescending(z => z.DateChangeRec)
                                                         .ThenByDescending(z => z.archId)
                                                         .First());

            var vertColumn = reportParams.ComplexReportParams.ДобавитьСекцию("ВертСекция");

            var columnNumber = 5;
            foreach (var work in works)
            {
                vertColumn.ДобавитьСтроку();
                vertColumn["Работа"] = work.Name;
                if (work.Code == "18")
                {
                    vertColumn["Работа"] = "Прочие ремонтно-строительные работы";
                }

                vertColumn["ЕдИзм"] = work.UnitMeasureName;
                vertColumn["Колонка1"] = columnNumber;
                vertColumn["Колонка2"] = columnNumber + 1;
                vertColumn["Колонка3"] = columnNumber + 2;
                vertColumn["Объем"] = string.Format("$Объем{0}$", work.Id);
                vertColumn["Процент"] = string.Format("$Процент{0}$", work.Id);
                vertColumn["Сумма"] = string.Format("$Сумма{0}$", work.Id);
                vertColumn["ОбъемИтог"] = string.Format("$ОбъемИтог{0}$", work.Id);
                vertColumn["СуммаИтог"] = string.Format("$СуммаИтог{0}$", work.Id);
                columnNumber += 3;
            }

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("ГоризСекция");

            var volumeTotal = new Dictionary<long, decimal>();
            var sumTotal = new Dictionary<long, decimal>();

            var sumOfFactSum = 0m;
            var sumOfCostSum = 0m;

            foreach (var typeWork in typeWorks)
            {
                section.ДобавитьСтроку();
                section["Номер"] = typeWork.Key.GjiNum;
                section["Район"] = typeWork.Key.Municipality;
                section["Адрес"] = typeWork.Key.Address;
                section["ИсточникФинансирования"] = typeWork.Key.FinanceSourceName;

                foreach (var work in typeWork.Value)
                {
                    var tmpVolumeOfCompletion = work.Value.VolumeOfCompletion;
                    var tmpPercentOfCompletion = work.Value.PercentOfCompletion;
                    var tmpCostSum = work.Value.CostSum;

                    if (archiveSmr.ContainsKey(work.Key.TypeWorkId))
                    {
                        var tmpItemArchiveSmr = archiveSmr[work.Key.TypeWorkId];
                        tmpVolumeOfCompletion = tmpItemArchiveSmr.VolumeOfCompletion;
                        tmpPercentOfCompletion = tmpItemArchiveSmr.PercentOfCompletion;
                        tmpCostSum = tmpItemArchiveSmr.CostSum;
                    }

                    section["Объем" + work.Key.WorkId] = tmpVolumeOfCompletion.ToDecimal().RoundDecimal(2);
                    section["Процент" + work.Key.WorkId] = tmpPercentOfCompletion.ToDecimal().RoundDecimal(2) + "%";
                    section["Сумма" + work.Key.WorkId] = tmpCostSum.ToDecimal().RoundDecimal(2);

                    if (volumeTotal.ContainsKey(work.Key.WorkId))
                    {
                        volumeTotal[work.Key.WorkId] += tmpVolumeOfCompletion.ToDecimal().RoundDecimal(2);
                    }
                    else
                    {
                        volumeTotal.Add(work.Key.WorkId, tmpVolumeOfCompletion.ToDecimal().RoundDecimal(2));
                    }

                    if (sumTotal.ContainsKey(work.Key.WorkId))
                    {
                        sumTotal[work.Key.WorkId] += tmpCostSum.ToDecimal().RoundDecimal(2);
                    }
                    else
                    {
                        sumTotal.Add(work.Key.WorkId, tmpCostSum.ToDecimal().RoundDecimal(2));
                    }
                }

                var costSum = typeWork.Value.Sum(x => x.Value.CostSum).ToDecimal().RoundDecimal(2);
                var sum = typeWork.Value.Sum(x => x.Value.Sum).ToDecimal().RoundDecimal(2);
                var percent = sum != 0 ? costSum / sum * 100 : 0.00m;

                sumOfFactSum += sum;
                sumOfCostSum += costSum;

                section["СуммаПоФакту"] = costSum;
                section["СуммаПоСмете"] = sum;
                section["ПроцентВыполнения"] = percent.ToDecimal().RoundDecimal(2) + "%";
            }

            reportParams.SimpleReportParams["ДатаОтчета"] = reportDate.ToShortDateString();
            reportParams.SimpleReportParams["Программа"] = programCrName;
            reportParams.SimpleReportParams["Колонка4"] = columnNumber;
            reportParams.SimpleReportParams["Колонка5"] = columnNumber + 1;
            reportParams.SimpleReportParams["Колонка6"] = columnNumber + 2;

            foreach (var work in works)
            {
                reportParams.SimpleReportParams["ОбъемИтог" + work.Id] = volumeTotal.ContainsKey(work.Id) ? volumeTotal[work.Id] : 0m;
                reportParams.SimpleReportParams["СуммаИтог" + work.Id] = sumTotal.ContainsKey(work.Id) ? sumTotal[work.Id] : 0m;
            }

            var percentOfSum = sumOfFactSum != 0 ? sumOfCostSum / sumOfFactSum * 100 : 0.00m;

            reportParams.SimpleReportParams["СуммаПоФактуИтог"] = sumOfCostSum;
            reportParams.SimpleReportParams["СуммаПоСметеИтог"] = sumOfFactSum;
            reportParams.SimpleReportParams["ПроцентВыполненияИтог"] = percentOfSum.RoundDecimal(2) + "%";
        }

        public override string ReportGenerator
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}