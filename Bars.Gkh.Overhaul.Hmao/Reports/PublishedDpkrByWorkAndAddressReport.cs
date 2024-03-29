﻿namespace Bars.Gkh.Overhaul.Hmao.Reports
{
    using System.Linq;

    using Bars.B4;
    using B4.Modules.Reports;

    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Config;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Bars.Gkh.Utils;

    using Castle.Windsor;
    using Overhaul.Entities;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Overhaul.Hmao.ConfigSections;

    public class PublishedDpkrByWorkAndAddressReport : BasePrintForm
    {
        #region Dependency injection members

        public IDomainService<PublishedProgramRecord> PublishedProgRecDomain { get; set; }

        public IDomainService<VersionRecordStage1> Stage1VersionDomain { get; set; }

        public IRepository<Municipality> MunicipalityDomain { get; set; }

        public IDomainService<RealityObject> RealityObjectDomain { get; set; }

        #endregion

        public PublishedDpkrByWorkAndAddressReport()
            : base(new ReportTemplateBinary(Properties.Resources.PublishProgramByWorkAndAddress))
        {

        }

        public IWindsorContainer Container { get; set; }

        public override string Name
        {
            get
            {
                return "Отчет по опубликованию (в разрезе видов работ и уровней адреса)";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Отчет по опубликованию (в разрезе видов работ и уровней адреса)";
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
                return "B4.controller.report.PublishedDpkrByWork";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.GkhOverhaul.PublishedDpkrByWorkReport";
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

            CreateVerticalColums(reportParams, minYear, maxYear);

            var muNameById = MunicipalityDomain.GetAll()
                .WhereIf(municipalityIds.Any(), x => municipalityIds.Contains(x.Id))
                .Select(x => new
                {
                     x.Id,
                     x.Name,
                     ParentName = x.ParentMo.Name
                })
                .AsEnumerable()
                .ToDictionary(x => x.Id, y => string.Format("{0} {1}", y.Name, y.ParentName));

            var roAddressById = RealityObjectDomain.GetAll()
                .WhereIf(municipalityIds.Any(),
                    x => municipalityIds.Contains(x.Municipality.Id)
                         || municipalityIds.Contains(x.MoSettlement.Id))
                .ToDictionary(x => x.Id, y => y.Address);

            var publishRecQuery =
                PublishedProgRecDomain.GetAll()
                    .Where(x => x.PublishedProgram.ProgramVersion.IsMain)
                    .WhereIf(municipalityIds.Any(),
                        x => municipalityIds.Contains(x.PublishedProgram.ProgramVersion.Municipality.Id));

            var worksDict = Container.Resolve<IDomainService<StructuralElementWork>>().GetAll()
                    .Select(x => new
                    {
                        structId = x.StructuralElement.Id,
                        WorkName = x.Job.Work.Name
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.structId)
                    .ToDictionary(x => x.Key, y => y.First().WorkName);

            var yearByStage2Dict = publishRecQuery
                .Where(x => x.Stage2 != null)
                .Select(x => new {x.Stage2.Id, x.PublishedYear})
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, y => y.Select(x => x.PublishedYear).FirstOrDefault());


            var appParams = Container.Resolve<IGkhParams>().GetParams();

            var moLevel = appParams.ContainsKey("MoLevel") && !string.IsNullOrEmpty(appParams["MoLevel"].To<string>())
                ? appParams["MoLevel"].To<MoLevel>()
                : MoLevel.MunicipalUnion;

            var query =
                Stage1VersionDomain.GetAll()
                    .Where(x => publishRecQuery.Any(y => y.Stage2.Id == x.Stage2Version.Id))
                    .WhereIf(moLevel == MoLevel.Settlement, x => x.RealityObject.MoSettlement != null)
                    .Select(x => new
                    {
                        RoId = x.RealityObject.Id,
                        x.RealityObject.Address,
                        MuId = moLevel == MoLevel.MunicipalUnion ? x.RealityObject.Municipality.Id : x.RealityObject.MoSettlement.Id,
                        StructElId = x.StructuralElement.StructuralElement.Id,
                        Stage2Id = x.Stage2Version.Id,
                        MuName = x.RealityObject.Municipality.Name,
                        ParentMuName = x.RealityObject.Municipality.ParentMo.Name,
                        PlaceName = x.RealityObject.FiasAddress.PlaceName ?? "",
                        StreetName = x.RealityObject.FiasAddress.StreetName ?? "",
                        x.RealityObject.FiasAddress.House
                    })
                    .AsEnumerable();

            var data =
                query
                    .OrderBy(x => x.MuName)
                    .ThenBy(x => x.ParentMuName)
                    .ThenBy(x => x.PlaceName.Substring(x.PlaceName.IndexOf(". ") + 1))
                    .ThenBy(x => x.StreetName.Substring(x.StreetName.IndexOf(". ") + 1))
                    .ThenBy(x => x.House)
                    .Select(x => new
                    {
                        x.MuId,
                        x.RoId,
                        WorkName = worksDict.ContainsKey(x.StructElId) ? worksDict[x.StructElId] : string.Empty,
                        Year = yearByStage2Dict.ContainsKey(x.Stage2Id) ? yearByStage2Dict[x.Stage2Id] : 0,
                    })
                    .GroupBy(x => x.MuId)
                    .ToDictionary(x => x.Key, y => y.GroupBy(x => x.RoId)
                        .ToDictionary(x => x.Key, z => z.Select(x => new
                        {
                            x.Year,
                            x.WorkName
                        })));

            var sectMunicipality = reportParams.ComplexReportParams.ДобавитьСекцию("sectionMunicipality");

            foreach (var muData in data)
            {
                var number = 1;
                sectMunicipality.ДобавитьСтроку();
                sectMunicipality["Municipality"] = muNameById.ContainsKey(muData.Key)
                                                       ? muNameById[muData.Key]
                                                       : string.Empty;

                var sectRealObj = sectMunicipality.ДобавитьСекцию("sectionRealObj");

                foreach (var roData in muData.Value)
                {
                    sectRealObj.ДобавитьСтроку();
                    sectRealObj["Address"] = roAddressById.ContainsKey(roData.Key)
                                                           ? roAddressById[roData.Key]
                                                           : string.Empty;
                    sectRealObj["Number"] = number;
                    var currentYear = minYear;

                    while (currentYear <= maxYear)
                    {
                        var tmpEndYear = currentYear + groupPeriod - 1;
                        int endYearPeriod =
                            tmpEndYear > maxYear
                                ? maxYear
                                : tmpEndYear;

                        var works =
                            roData.Value.Where(x => x.Year >= currentYear && x.Year <= endYearPeriod)
                                  .Select(x => x.WorkName)
                                  .Distinct()
                                  .ToList();

                        sectRealObj[string.Format("TypeWork{0}", currentYear)] = works.Any() ? works.AggregateWithSeparator(", ") : " _____ \n";

                        currentYear = endYearPeriod + 1;
                    }

                    number++;
                }
            }
        }

        // заполнение вертикальной секции
        public void CreateVerticalColums(ReportParams reportParams, int minYear, int maxYear)
        {
            var verticalSection = reportParams.ComplexReportParams.ДобавитьСекцию("vertsection");

            var currentYear = minYear;
            var columnNum = 3;
            while (currentYear <= maxYear)
            {
                var tmpEndYear = currentYear + groupPeriod - 1;
                var endYearPeriod =
                    tmpEndYear > maxYear
                        ? maxYear
                        : tmpEndYear;

                verticalSection.ДобавитьСтроку();
                verticalSection["PeriodYears"] =
                    endYearPeriod - currentYear > 1
                        ? string.Format("{0}-{1}", currentYear, endYearPeriod)
                        : currentYear.ToString("0000");
                verticalSection["TypeWork"] = string.Format("$TypeWork{0}$", currentYear);
                verticalSection["ColumnNum"] = columnNum++;

                currentYear = endYearPeriod + 1;
            }
        }
    }
}