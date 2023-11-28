namespace Bars.GkhCr.Report
{
	using B4.Modules.Reports;
	using Bars.B4;
	using Bars.B4.Utils;
	using Bars.Gkh.Entities;
	using Bars.Gkh.Enums;
	using Bars.GkhCr.Entities;
	using Bars.GkhCr.Localizers;
	using Castle.Windsor;
	using System.Collections.Generic;
	using System.Linq;

    public class CountPhotoReport : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        private long programCrId;
        private List<long> municipalityListId = new List<long>();

        /// <summary>
        /// коды работ, по которым собирается отчет
        /// Для этого отчета важен порядок и коды, по которым собирается отчет
        /// </summary>
        private readonly List<string> allWorkCodes = new List<string>
            {
                "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", 
                "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "29"
            };


        public CountPhotoReport()
            : base(new ReportTemplateBinary(Properties.Resources.CountPhoto))
        {
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.CR.CountPhoto";
            }
        }

        /// <summary>
        /// Описание
        /// </summary>
        public override string Desciption
        {
            get { return "Отчет \"Количество фотографий\""; }
        }

        /// <summary>
        /// Группа
        /// </summary>
        public override string GroupName
        {
            get { return "Отчеты Кап.ремонт"; }
        }

        /// <summary>
        /// Представление с пользователскими параметрами
        /// </summary>
        public override string ParamsController
        {
            get { return "B4.controller.report.CountPhoto"; }
        }

        /// <summary>
        /// Наименование
        /// </summary>
        public override string Name
        {
            get { return "Отчет \"Количество фотографий\""; }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            programCrId = baseParams.Params["programCrId"].ToInt();

            municipalityListId.Clear();

            var municipalityStr = baseParams.Params.GetAs("municipalityIds", string.Empty);
            this.municipalityListId = !string.IsNullOrEmpty(municipalityStr) ? municipalityStr.Split(',').Select(id => id.ToLong()).ToList() : new List<long>();
        }

        public override void PrepareReport(ReportParams reportParams)
        {

            var programCr = this.Container.Resolve<IDomainService<ProgramCr>>()
                                  .GetAll().FirstOrDefault(x => x.Id == programCrId);

            if (programCr == null)
            {
                return;
            }

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");

            var queryObjectCr = Container.Resolve<IDomainService<ObjectCr>>()
                                         .GetAll()
                                         .WhereIf(municipalityListId.Count > 0, x => municipalityListId.Contains(x.RealityObject.Municipality.Id))
                                         .Where(x => x.ProgramCr.Id == programCrId);

            var queryObjectCrId = queryObjectCr.Select(x => x.Id);

            var listBuildContract = this.Container.Resolve<IDomainService<BuildContract>>()
                                                    .GetAll()
                                                    .Where(x => queryObjectCrId.Contains(x.ObjectCr.Id))
                                                    .Select(x => new { ObjectCrId = x.ObjectCr.Id, BuilderName = x.Builder.Contragent.Name })
                                                    .ToList();

            var listObjectCr = queryObjectCr.Select(x => new
                                                {
                                                    crObjectId = x.Id,
                                                    roId = x.RealityObject.Id,
                                                    x.GjiNum,
                                                    x.RealityObject.Address,
                                                    municipalityName = x.RealityObject.Municipality.Name,
                                                })
                                .ToList();

            var listTypeWorkCrObject = Container.Resolve<IDomainService<TypeWorkCr>>().GetAll()
                                                .Where(x => queryObjectCr.Any(y => y.Id == x.ObjectCr.Id))
                                                .Select(x => new
                                                    {
                                                        RoId = x.ObjectCr.RealityObject.Id,
                                                        WorkCode = x.Work.Code
                                                    });


            var queryObjectCrIds = queryObjectCr.Select(x => x.Id).Distinct();

            var dictProtocolCrInfo = Container.Resolve<IDomainService<ProtocolCr>>()
                                              .GetAll()
                                              .Where(x => queryObjectCrIds.Contains(x.ObjectCr.Id))
                                              .Select(x => new { roId = x.ObjectCr.RealityObject.Id, typeDocument = x.TypeDocumentCr })
                                              .AsEnumerable()
                                              .GroupBy(x => x.roId)
                                              .ToDictionary(
                                                 x => x.Key,
                                                 v => v.Select(y => y.typeDocument.Key).ToList());

            var groupedBycrObject = listObjectCr.GroupBy(x => x.crObjectId)
                .Select(x =>
                {
                    var listBuilders = listBuildContract.Where(y => y.ObjectCrId == x.Key)
                                         .Select(y => y.BuilderName)
                                         .Distinct()
                                         .ToList();

                    var builders = string.Empty;

                    if (listBuilders.Count > 0)
                    {
                        builders = listBuilders.Aggregate((curr, next) => string.Format("{0};{1}", curr, next));
                    }

                    return new
                    {
                        id = x.Select(y => y.roId).FirstOrDefault(),
                        municipalityName = x.Select(y => y.municipalityName).FirstOrDefault(),
                        gjiNum = x.Select(y => y.GjiNum).FirstOrDefault(),
                        address = x.Select(y => y.Address).FirstOrDefault(),
                        roId = x.Select(y => y.roId).FirstOrDefault(),
                        builders
                    };
                })
                .OrderBy(x => x.municipalityName);

            var relityObjIdsQuery = queryObjectCr.Select(x => x.RealityObject.Id);

            var realtyObjImages = this.Container.Resolve<IDomainService<RealityObjectImage>>().GetAll()
                                      .Where(x => programCr.Period.Id == x.Period.Id && relityObjIdsQuery.Contains(x.RealityObject.Id) && this.allWorkCodes.Contains(x.WorkCr.Code))
                                      .Select(x => new
                                      {
                                          realtyObjId = x.RealityObject.Id,
                                          WorkCRCode = x.WorkCr.Code,
                                          x.ImagesGroup
                                      })
                                      .ToList();

            var groupByCodeandImgGroup = realtyObjImages.GroupBy(x => new { x.realtyObjId, x.ImagesGroup, x.WorkCRCode })
               .Select(x => new
               {
                   x.Key.realtyObjId,
                   x.Key.WorkCRCode,
                   x.Key.ImagesGroup,
                   Count = x.Count()
               })
               .Where(y => y.WorkCRCode != null)
               .ToList();

            // Количество фотографий с группой - "Изображение жилого дома"
            var pictureHouseDict = this.Container.Resolve<IDomainService<RealityObjectImage>>().GetAll()
                                   .Where(x => relityObjIdsQuery.Contains(x.RealityObject.Id))
                                   .Where(x => x.ImagesGroup == ImagesGroup.PictureHouse)
                                   .GroupBy(x => x.RealityObject.Id)
                                   .Select(x => new
                                   {
                                       x.Key,
                                       pictCount = x.Count()
                                   })
                                   .ToDictionary(x => x.Key, x => x.pictCount);

            var homeWorkCodes = groupByCodeandImgGroup.GroupBy(x => x.realtyObjId).ToDictionary(x => x.Key, x => x.Select(y => y.WorkCRCode).Distinct().ToList());

            var imagegroups = new List<ImagesGroup>
                                  {
                                      ImagesGroup.AfterOverhaul,
                                      ImagesGroup.BeforeOverhaul,
                                      ImagesGroup.InOverhaul
                                  };

            var groupByCodeandImgGroup2 = groupedBycrObject.Select(x => x.id)
                                                           .Distinct()
                                                           .ToDictionary(
                                                               z => z,
                                                               z =>
                                                               allWorkCodes.ToDictionary(
                                                                   x => x,
                                                                   x => imagegroups.ToDictionary(y => y, y => (int?)null)
                                                                )
                                                            );

            // если у дома есть один из видов работ то вместо прочерка 0
            //foreach (var realtyObj in homeWorkCodes)
            //{
            //    foreach (var wc in realtyObj.Value)
            //    {
            //        foreach (var img in imagegroups)
            //        {
            //            if (groupByCodeandImgGroup2.ContainsKey(realtyObj.Key)
            //                && groupByCodeandImgGroup2[realtyObj.Key].ContainsKey(wc)
            //                && groupByCodeandImgGroup2[realtyObj.Key][wc].ContainsKey(img))
            //            {
            //                groupByCodeandImgGroup2[realtyObj.Key][wc][img] = 0;
            //            }
            //        }
            //    }
            //}

            groupByCodeandImgGroup.ForEach(
                x =>
                {
                    if (groupByCodeandImgGroup2.ContainsKey(x.realtyObjId)
                        && groupByCodeandImgGroup2[x.realtyObjId].ContainsKey(x.WorkCRCode)
                        && groupByCodeandImgGroup2[x.realtyObjId][x.WorkCRCode].ContainsKey(x.ImagesGroup))
                    {
                        groupByCodeandImgGroup2[x.realtyObjId][x.WorkCRCode][x.ImagesGroup] = x.Count;
                    }
                });

            var countTotalBeforeDict = allWorkCodes.ToDictionary(x => x, x => 0);
            var countTotalInDict = allWorkCodes.ToDictionary(x => x, x => 0);
            var countTotalAfterDict = allWorkCodes.ToDictionary(x => x, x => 0);
            var countTotalGenerallyDict = allWorkCodes.ToDictionary(x => x, x => 0);

            countTotalBeforeDict.Add("Total", 0);
            countTotalInDict.Add("Total", 0);
            countTotalAfterDict.Add("Total", 0);
            countTotalGenerallyDict.Add("Total", 0);

            var countBeforeDash = new Dictionary<string, int>();
            var countInDash = new Dictionary<string, int>();
            var countAfterDash = new Dictionary<string, int>();

            foreach (var crObject in groupedBycrObject)
            {
                section.ДобавитьСтроку();

                section["Municipality"] = crObject.municipalityName;
                section["Builder"] = crObject.builders;
                section["GJINumber"] = crObject.gjiNum;
                section["Address"] = crObject.address;

                int countTotalBefore = 0;
                int countTotalIn = 0;
                int countTotalAfter = 0;

                var crObjImg = groupByCodeandImgGroup2.ContainsKey(crObject.id)
                    ? groupByCodeandImgGroup2[crObject.id]
                    : new Dictionary<string, Dictionary<ImagesGroup, int?>>();

                var listTypeWorkCrObjectbyRoId = listTypeWorkCrObject.Where(x => x.RoId == crObject.id);
                foreach (var workCode in allWorkCodes)
                {
                    var hasWorkOnObject = listTypeWorkCrObjectbyRoId.Any(x => x.WorkCode == workCode);

                    var images = crObjImg.ContainsKey(workCode) ? crObjImg[workCode] : new Dictionary<ImagesGroup, int?>();

                    int? beforeOverhaul = images.ContainsKey(ImagesGroup.BeforeOverhaul) ? images[ImagesGroup.BeforeOverhaul] : null;
                    int? inOverhaul = images.ContainsKey(ImagesGroup.InOverhaul) ? images[ImagesGroup.InOverhaul] : null;
                    int? afterOverhaul = images.ContainsKey(ImagesGroup.AfterOverhaul) ? images[ImagesGroup.AfterOverhaul] : null;

                    
                    section[string.Format("CountBefore_{0}", workCode)] = beforeOverhaul == null
                        ? hasWorkOnObject 
                            ? "0"
                            : "-" 
                        : beforeOverhaul.ToString();
                    
                    countTotalBefore += beforeOverhaul ?? 0;
                    if (beforeOverhaul == null)
                    {
                        if (countBeforeDash.ContainsKey(workCode))
                        {
                            countBeforeDash[workCode] += 1;
                        }
                        else
                        {
                            countBeforeDash.Add(workCode, 1);
                        }
                    }

                    section[string.Format("CountIn_{0}", workCode)] = inOverhaul == null 
                        ? hasWorkOnObject 
                            ? "0"
                            : "-" 
                        : inOverhaul.ToString();

                    countTotalIn += inOverhaul ?? 0;
                    if (inOverhaul == null)
                    {
                        if (countInDash.ContainsKey(workCode))
                        {
                            countInDash[workCode] += 1;
                        }
                        else
                        {
                            countInDash.Add(workCode, 1);
                        }
                    }

                    section[string.Format("CountAfter_{0}", workCode)] = afterOverhaul == null
                        ? hasWorkOnObject
                            ? "0"
                            : "-"  
                        : afterOverhaul.ToString();

                    countTotalAfter += afterOverhaul ?? 0;
                    if (afterOverhaul == null)
                    {
                        if (countAfterDash.ContainsKey(workCode))
                        {
                            countAfterDash[workCode] += 1;
                        }
                        else
                        {
                            countAfterDash.Add(workCode, 1);
                        }
                    }

                    countTotalBeforeDict[workCode] += beforeOverhaul ?? 0;
                    countTotalInDict[workCode] += inOverhaul ?? 0;
                    countTotalAfterDict[workCode] += afterOverhaul ?? 0;
                }

                section["CountTotalBefore"] = countTotalBefore;
                section["CountTotalIn"] = countTotalIn;
                section["CountTotalAfter"] = countTotalAfter;
                var countPictHouse = pictureHouseDict.ContainsKey(crObject.id) ? pictureHouseDict[crObject.id] : 0;
                section["CountTotalGenerally"] = countPictHouse;

                var protocolCrInfoCr = dictProtocolCrInfo.ContainsKey(crObject.id) ? dictProtocolCrInfo[crObject.id] : null;
                section["CountGenerally"] = protocolCrInfoCr != null ? protocolCrInfoCr.Count(x => x == TypeDocumentCrLocalizer.ProtocolNeedCrKey) : 0;
                section["CountCompletion"] = protocolCrInfoCr != null ? protocolCrInfoCr.Count(x => x == TypeDocumentCrLocalizer.ProtocolCompleteCrKey) : 0;
                section["CountActCommissioning"] = protocolCrInfoCr != null ? protocolCrInfoCr.Count(x => x == TypeDocumentCrLocalizer.ActExpluatatinAfterCrKey) : 0;

                countTotalBeforeDict["Total"] += countTotalBefore;
                countTotalInDict["Total"] += countTotalIn;
                countTotalAfterDict["Total"] += countTotalAfter;
                countTotalGenerallyDict["Total"] += countPictHouse;
            }

            var countCrObj = groupByCodeandImgGroup2.Keys.Count;

            foreach (var workCode in allWorkCodes)
            {
                reportParams.SimpleReportParams[string.Format("TotalCountBefore_{0}", workCode)] = (countBeforeDash.ContainsKey(workCode) && countBeforeDash[workCode] == countCrObj) ? "-" : countTotalBeforeDict[workCode].ToStr();
                reportParams.SimpleReportParams[string.Format("TotalCountIn_{0}", workCode)] = (countInDash.ContainsKey(workCode) && countInDash[workCode] == countCrObj) ? "-" : countTotalInDict[workCode].ToStr();
                reportParams.SimpleReportParams[string.Format("TotalCountAfter_{0}", workCode)] = (countAfterDash.ContainsKey(workCode) && countAfterDash[workCode] == countCrObj) ? "-" : countTotalAfterDict[workCode].ToStr();
            }

            reportParams.SimpleReportParams["TotalBefore"] = countTotalBeforeDict["Total"];
            reportParams.SimpleReportParams["TotalIn"] = countTotalInDict["Total"];
            reportParams.SimpleReportParams["TotalAfter"] = countTotalAfterDict["Total"];
            reportParams.SimpleReportParams["TotalGenerally"] = countTotalGenerallyDict["Total"];

            var valuesDictProtocolCrInfo = dictProtocolCrInfo.Values.SelectMany(x => x);
            reportParams.SimpleReportParams["TotalCountGenerally"] = valuesDictProtocolCrInfo.Count(x => x == TypeDocumentCrLocalizer.ProtocolNeedCrKey);
            reportParams.SimpleReportParams["TotalCountCompletion"] = valuesDictProtocolCrInfo.Count(x => x == TypeDocumentCrLocalizer.ProtocolCompleteCrKey);
            reportParams.SimpleReportParams["TotalCountActCommissioning"] = valuesDictProtocolCrInfo.Count(x => x == TypeDocumentCrLocalizer.ActExpluatatinAfterCrKey);
        }

        public override string ReportGenerator
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }
    }
}