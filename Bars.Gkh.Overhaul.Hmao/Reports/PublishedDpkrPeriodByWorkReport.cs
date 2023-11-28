﻿namespace Bars.Gkh.Overhaul.Hmao.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using B4.Modules.Reports;

    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Config;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Bars.Gkh.Utils;

    using Castle.Windsor;
    using Overhaul.Entities;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Overhaul.Hmao.ConfigSections;

    public class PublishedDpkrPeriodByWorkReport : BasePrintForm
    {
        #region Dependency injection members

        public IDomainService<PublishedProgramRecord> PublishedProgRecDomain { get; set; }

        public IDomainService<VersionRecordStage1> Stage1VersionDomain { get; set; }

        public IRepository<Municipality> MunicipalityDomain { get; set; }

        public IDomainService<RealityObject> RealityObjectDomain { get; set; }

        #endregion

        public PublishedDpkrPeriodByWorkReport()
            : base(new ReportTemplateBinary(Properties.Resources.PublishProgramPeriodByWork))
        {

        }

        public IWindsorContainer Container { get; set; }

        public override string Name
        {
            get
            {
                return "Опубликованная программа КР (по видам работ с группировкой по годам)";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Опубликованная программа КР (по видам работ с группировкой по годам)";
            }
        }

        public override string GroupName
        {
            get
            {
                return "Долгосрочная программа";
            }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.PublishedDpkrPeriodByWork";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.GkhOverhaul.PublishedDpkrPeriodByWorkReport";
            }
        }

        private long[] municipalityIds;

        private int groupPeriod;

        public override void SetUserParams(BaseParams baseParams)
        {
            var strMunicpalIds = baseParams.Params.GetAs("municipalityIds", string.Empty);

            groupPeriod = baseParams.Params.GetAs("groupPeriod", 5);

            municipalityIds = !string.IsNullOrEmpty(strMunicpalIds)
                ? strMunicpalIds.Split(',').Select(x => x.ToLong()).ToArray()
                : new long[0];
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var config = Container.GetGkhConfig<OverhaulHmaoConfig>();
            var minYear = config.ProgrammPeriodStart;
            var maxYear = config.ProgrammPeriodEnd;

            if (minYear == 0 || maxYear == 0 || minYear > maxYear)
            {
                throw new ReportParamsException("Проверьте правильность параметров расчета дпкр");
            }

            var periodByYearDict = this.GetPeriodByYearDict(minYear, maxYear);

            var muNameById = MunicipalityDomain.GetAll()
                .WhereIf(municipalityIds.Any(), x => municipalityIds.Contains(x.Id))
                .ToDictionary(x => x.Id, y => y.Name);

            var roInfoById = RealityObjectDomain.GetAll()
                .WhereIf(municipalityIds.Any(),
                    x => municipalityIds.Contains(x.Municipality.Id)
                         || municipalityIds.Contains(x.MoSettlement.Id))
                .Select(x => new
                                 {
                                     x.Id,
                                     x.Address,
                                     x.DateCommissioning
                                 })
                .ToDictionary(x => x.Id);

            var publishRecQuery =
                PublishedProgRecDomain.GetAll()
                    .Where(x => x.PublishedProgram.ProgramVersion.IsMain)
                    .Where(x => minYear <= x.PublishedYear && maxYear >= x.PublishedYear)
                    .WhereIf(municipalityIds.Any(),
                        x => municipalityIds.Contains(x.PublishedProgram.ProgramVersion.Municipality.Id));

            var worksDict = Container.Resolve<IDomainService<StructuralElementWork>>().GetAll()
                    .Select(x => new
                    {
                        structId = x.StructuralElement.Id,
                        WorkId = x.Job.Work.Id
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.structId)
                    .ToDictionary(x => x.Key, y => y.First().WorkId);

            var periodByStage2Dict = publishRecQuery
                .Where(x => x.Stage2 != null)
                .Select(x => new {x.Stage2.Id, x.PublishedYear})
                .OrderBy(x => x.PublishedYear)
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, y =>
                    { 
                        var year = y.Select(x => x.PublishedYear).FirstOrDefault();
                        return periodByYearDict.ContainsKey(year) ? periodByYearDict[year] : string.Empty;
                    });


            var appParams = Container.Resolve<IGkhParams>().GetParams();

            var moLevel = appParams.ContainsKey("MoLevel") && !string.IsNullOrEmpty(appParams["MoLevel"].To<string>())
                ? appParams["MoLevel"].To<MoLevel>()
                : MoLevel.MunicipalUnion;

            var data =
                Stage1VersionDomain.GetAll()
                    .Where(x => publishRecQuery.Any(y => y.Stage2.Id == x.Stage2Version.Id))
                    .OrderBy(x => x.RealityObject.Address)
                    .Select(x => new
                    {
                        RoId = x.RealityObject.Id,
                        MuId = moLevel == MoLevel.MunicipalUnion ?  x.RealityObject.Municipality.Id :  x.RealityObject.MoSettlement.Id,
                        StructElId = x.StructuralElement.StructuralElement.Id,
                        Stage2Id = x.Stage2Version.Id
                    })
                    .AsEnumerable()
                    .Select(x => new
                    {
                        x.MuId,
                        x.RoId,
                        WorkId = worksDict.ContainsKey(x.StructElId) ? worksDict[x.StructElId] : 0,
                        Period = periodByStage2Dict.ContainsKey(x.Stage2Id) ? periodByStage2Dict[x.Stage2Id] : string.Empty,
                    });

            var workIds = data.Select(x => x.WorkId).Distinct().ToArray();

            var dataByMu =
                data
                    .GroupBy(x => x.MuId)
                    .ToDictionary(x => x.Key, y => y.GroupBy(x => x.RoId)
                        .ToDictionary(x => x.Key, z => z.GroupBy(x => x.WorkId).Select(x => new
                                                                                            {
                                                                                                x.Key,
                                                                                                Periods = x.Select(k => k.Period).Distinct().OrderBy(k => k).AggregateWithSeparator("; ")
                                                                                            })));


            CreateVerticalColums(reportParams, workIds);

            reportParams.SimpleReportParams["ReportDate"] = DateTime.Now.Date.ToShortDateString();
            var sectMunicipality = reportParams.ComplexReportParams.ДобавитьСекцию("sectionMunicipality");

            var number = 1;
            foreach (var muData in dataByMu)
            {
                sectMunicipality.ДобавитьСтроку();
                sectMunicipality["Municipality"] = muNameById.ContainsKey(muData.Key)
                                                       ? muNameById[muData.Key]
                                                       : string.Empty;

                var sectRealObj = sectMunicipality.ДобавитьСекцию("sectionRealObj");

                foreach (var roData in muData.Value)
                {
                    sectRealObj.ДобавитьСтроку();
                    sectRealObj["Number"] = number++;
                    sectRealObj["Address"] = roInfoById.ContainsKey(roData.Key)
                                                           ? roInfoById[roData.Key].Address
                                                           : string.Empty;
                    sectRealObj["DateCommissioning"] = roInfoById.ContainsKey(roData.Key) && roInfoById[roData.Key].DateCommissioning.HasValue
                                                           ? roInfoById[roData.Key].DateCommissioning.Value.ToShortDateString()
                                                           : string.Empty;

                    foreach (var periodsByWorkId in roData.Value)
                    {
                        sectRealObj[string.Format("Work{0}", periodsByWorkId.Key)] = periodsByWorkId.Periods;
                    }
                }
            }
        }

        // заполнение вертикальной секции
        private void CreateVerticalColums(ReportParams reportParams, IEnumerable<long> workIds)
        {
            var verticalSection = reportParams.ComplexReportParams.ДобавитьСекцию("vertsection");

            var works =
                Container.Resolve<IDomainService<Work>>()
                         .GetAll()
                         .Where(x => workIds.Contains(x.Id))
                         .Select(x => new { x.Id, x.Name })
                         .ToDictionary(x => x.Id, y => y.Name);

            foreach (var work in works)
            {
                verticalSection.ДобавитьСтроку();
                verticalSection["PeriodYears"] = string.Format("$Work{0}$", work.Key);
                verticalSection["TypeWork"] = work.Value;
            }
        }

        private Dictionary<int, string> GetPeriodByYearDict(int minYear, int maxYear)
        {
            var result = new Dictionary<int, string>();

            var currentYear = minYear;

            while (currentYear <= maxYear)
            {
                var tmpEndYear = currentYear + groupPeriod - 1;
                var endYearPeriod =
                    tmpEndYear > maxYear
                        ? maxYear
                        : tmpEndYear;

                var period = endYearPeriod - currentYear > 1
                        ? string.Format("{0}-{1}", currentYear, endYearPeriod)
                        : currentYear.ToString("0000");

                for (int i = currentYear; i < endYearPeriod+1; i++ )
                {
                    result.Add(i, period);
                }

                currentYear = endYearPeriod + 1;
            }

            return result;
        }
    }
}