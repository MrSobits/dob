﻿namespace Bars.Gkh.Overhaul.Hmao.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using B4.Modules.Reports;

    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Overhaul.Hmao.Properties;

    using Castle.Windsor;
    using Gkh.Entities.CommonEstateObject;

    class ControlCertificationOfBuild : BasePrintForm
    {
        public ControlCertificationOfBuild()
            : base(new ReportTemplateBinary(Resources.ControlCertificationOfBuild))
        {
        }

        private long[] municipalityIds;
        private int[] houseTypes;
        public IWindsorContainer Container { get; set; }
        public override string Name
        {
            get { return "Контроль паспортизации домов"; }
        }

        public override string Desciption
        {
            get { return "Контроль паспортизации домов"; }
        }

        public override string GroupName
        {
            get { return "Жилые дома"; }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.ControlCertificationOfBuild";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Ovrhl.ControlCertificationOfBuild";
            }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            var municipalityIdsList = baseParams.Params.GetAs("municipalityIds", string.Empty);
            municipalityIds = !string.IsNullOrEmpty(municipalityIdsList)
                                  ? municipalityIdsList.Split(',').Select(id => id.ToLong()).ToArray()
                                  : new long[0];

            var houseTypesList = baseParams.Params.GetAs("houseTypes", string.Empty);
            houseTypes = !string.IsNullOrEmpty(houseTypesList)
                                  ? houseTypesList.Split(',').Select(id => id.ToInt()).ToArray()
                                  : new int[0];
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var commonEstateObjectsQuery = CreateVerticalColums(reportParams);
            var ceoIds = commonEstateObjectsQuery.ToList();

            var manOrgByRoIdDict = Container.Resolve<IDomainService<ManOrgContractRealityObject>>().GetAll()
                .WhereIf(municipalityIds.Length > 0, 
                    x => municipalityIds.Contains(x.RealityObject.Municipality.Id)
                        || municipalityIds.Contains(x.RealityObject.MoSettlement.Id))

                .WhereIf(houseTypes.Length > 0, x => houseTypes.Contains((int)x.RealityObject.TypeHouse))
                .Where(x => x.ManOrgContract.StartDate == null || x.ManOrgContract.StartDate <= DateTime.Now)
                .Where(x => x.ManOrgContract.EndDate == null || x.ManOrgContract.EndDate >= DateTime.Now.Date)
                .Where(x => x.ManOrgContract.ManagingOrganization != null)
                .Where(x => x.ManOrgContract.TypeContractManOrgRealObj != TypeContractManOrg.DirectManag)
                .Select(x => new
                {
                    moName = x.ManOrgContract.ManagingOrganization.Contragent.Name ?? string.Empty,
                    roId = x.RealityObject.Id
                })
                .AsEnumerable()
                .GroupBy(x => x.roId)
                .ToDictionary(x => x.Key, x => x.Select(y => y.moName).ToList());

            var realtyObjByMuDict = Container.Resolve<IDomainService<RealityObject>>().GetAll()
                .WhereIf(municipalityIds.Length > 0, 
                    x => municipalityIds.Contains(x.Municipality.Id)
                        || municipalityIds.Contains(x.MoSettlement.Id))
                .WhereIf(houseTypes.Length > 0, x => houseTypes.Contains((int)x.TypeHouse))
                .Select(x => new
                {
                    x.Municipality.Name,
                    roId = x.Id,
                    x.Address
                })
                .AsEnumerable()
                .SelectMany(x =>
                {
                    if (manOrgByRoIdDict.ContainsKey(x.roId))
                    {
                        var manOrgs = manOrgByRoIdDict[x.roId];

                        return manOrgs
                            .Select(y => new RealtyObjProxy
                            {
                                MuName = x.Name,
                                roId = x.roId,
                                Address = x.Address,
                                MoName = y
                            })
                            .ToList();
                    }
                    return new List<RealtyObjProxy>
                    {
                        new RealtyObjProxy
                        {
                            MuName = x.Name,
                            roId = x.roId,
                            Address = x.Address,
                            MoName = string.Empty
                        }
                    };
                })
                .GroupBy(x => x.MuName)
                .ToDictionary(x => x.Key, x => x.ToList());


            var realityObjectStructuralElement = this.GetData(commonEstateObjectsQuery);

            var realityObjectStructuralElementKeys = realityObjectStructuralElement.Select(x => x.Key).ToList();

            var structuralElementInfo = Container.Resolve<IDomainService<RealityObjectStructuralElement>>().GetAll()
                .Where(x => realityObjectStructuralElementKeys.Contains(x.RealityObject.Id))
                .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.RealityObject.Municipality.Id))
                .WhereIf(houseTypes.Length > 0, x => houseTypes.Contains((int)x.RealityObject.TypeHouse))
                .Where(x => commonEstateObjectsQuery.Contains(x.StructuralElement.Group.CommonEstateObject.Id))
                .Where(x => x.RealityObject.BuildYear != null)
                .Select(x => new
                {
                    RealObjId = x.RealityObject.Id,
                    x.LastOverhaulYear,
                    x.Volume,
                    CommonEstateObjectId = x.StructuralElement.Group.CommonEstateObject.Id
                })
                .AsEnumerable()
                .GroupBy(x => new
                {
                    x.RealObjId,
                    x.CommonEstateObjectId
                })
                .ToDictionary(x => x.Key, x => x.ToList());

            var missingStructuralElementInfo = Container.Resolve<IDomainService<RealityObjectMissingCeo>>().GetAll()
                .Where(x => realityObjectStructuralElementKeys.Contains(x.RealityObject.Id))
                .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.RealityObject.Municipality.Id))
                .WhereIf(houseTypes.Length > 0, x => houseTypes.Contains((int)x.RealityObject.TypeHouse))
                .Where(x => x.RealityObject.BuildYear != null)
                .Select(x => new
                {
                    RealObjId = x.RealityObject.Id,
                    CommonEstateObjectId = x.MissingCommonEstateObject.Id
                })
                .AsEnumerable()
                .GroupBy(x => x.RealObjId)
                .ToDictionary(x => x.Key, x => x.ToList());

            var sectionMu = reportParams.ComplexReportParams.ДобавитьСекцию("sectionMu");
            var sectionManOrg = sectionMu.ДобавитьСекцию("sectionManOrg");
            var sectionRo = sectionManOrg.ДобавитьСекцию("sectionRo");
            var num = 1;
            var sumPercent = 0M;

            foreach (var realtyObjectsByMu in realtyObjByMuDict.OrderBy(x => x.Key))
            {
                sectionMu.ДобавитьСтроку();
                sectionMu["MuName"] = realtyObjectsByMu.Key;
                var sumPercentByMu = 0M;
                var ooiCountDict = ceoIds.ToDictionary(x => x, x => 0);

                foreach (var manOrgGrouped in realtyObjectsByMu.Value.GroupBy(x => x.MoName).OrderBy(x => x.Key))
                {
                    sectionManOrg.ДобавитьСтроку();

                    var sumPercentByManOrg = 0M;
                    var ooiCountDictOrg = ceoIds.ToDictionary(x => x, x => 0);
                    foreach (var realtyObjectInfo in manOrgGrouped.OrderBy(x => x.Address))
                    {
                        sectionRo.ДобавитьСтроку();
                        sectionRo["Num"] = num++;
                        sectionRo["MO"] = realtyObjectsByMu.Key;
                        sectionRo["ManOrg"] = realtyObjectInfo.MoName;
                        sectionRo["Address"] = realtyObjectInfo.Address;

                        //для расчета процентов
                        var countCeo = 0;
                        var missingCeoCount = 0;

                        //если у констр характер в доме, в группе есть хотябы одна запись где не заполнены: Объем или 
                        //год установки, всему Констр элемету ставим 0
                        foreach (var ceoId in ceoIds)
                        {
                            var roId = realtyObjectInfo.roId;

                            //выбираем по текущему дому, группу элементов
                            var structuralElementListInfo = structuralElementInfo.Where(x => x.Key.RealObjId == roId)
                                .Where(x => x.Key.CommonEstateObjectId == ceoId);

                            sectionRo[string.Format("haveElement{0}", ceoId)] = 0;

                            foreach (var structuralElementByCeo in structuralElementListInfo.Select(x => x.Value))
                            {
                                //общие количество элементов в группе
                                var structuralElementCount = structuralElementByCeo.Count;

                                //элементы с заполнненым объемом и годом
                                var filledElement = 0;

                                foreach (var structuralElement in structuralElementByCeo)
                                {
                                    if (structuralElement.Volume != 0 && structuralElement.LastOverhaulYear != 0)
                                    {
                                        filledElement += 1;
                                    }
                                }
                                //если заполненых равное колво общего числа ставим 1
                                if (filledElement >= structuralElementCount)
                                {
                                    sectionRo[string.Format("haveElement{0}", ceoId)] = 1;

                                    ooiCountDict[ceoId] += 1;
                                    ooiCountDictOrg[ceoId] += 1;
                                    countCeo += 1;
                                }
                            }

                            // Отсутствие конструктивных элементов
                            var missingStructuralElementListInfo = missingStructuralElementInfo.Where(x => x.Key == roId);
                            foreach (var missingStructuralElementList in missingStructuralElementListInfo.Select(x => x.Value))
                            {
                                foreach (var missingStructuralElemen in missingStructuralElementList)
                                {
                                    if (missingStructuralElemen.CommonEstateObjectId == ceoId)
                                    {
                                        sectionRo[string.Format("haveElement{0}", ceoId)] = "-";
                                        ++missingCeoCount;
                                    }
                                }
                            }
                        }

                        var currentRoPercent = Math.Min(1m, ceoIds.Count != 0 ? (countCeo + missingCeoCount) / ceoIds.Count.ToDecimal() : 0);
                        sumPercentByMu += currentRoPercent;
                        sumPercentByManOrg += currentRoPercent;
                        sectionRo["PercentOccup"] = currentRoPercent;
                    }

                    ooiCountDictOrg.ForEach(x => sectionManOrg[string.Format("OrgCount{0}", x.Key)] = x.Value);
                    sectionManOrg["ManOrg"] = manOrgGrouped.Key;

                    sectionManOrg["AverageManOrg"] = sumPercentByManOrg / manOrgGrouped.Count();
                }

                var currentMuPercent = sumPercentByMu / realtyObjectsByMu.Value.Count;
                sumPercent += currentMuPercent;
                sectionMu["AverageMun"] = currentMuPercent;

                ooiCountDict.ForEach(x => sectionMu[string.Format("OOICount{0}", x.Key)] = x.Value);
            }
            
            var count = realtyObjByMuDict.Select(y => y.Value).Count();
            reportParams.SimpleReportParams["AverageAllMun"] = sumPercent / (count != 0 ? count : 1);
        }

        // получение групп конструктивных элементов по домам
        public Dictionary<long, List<long>> GetData(IQueryable<long> commonEstateObjectsQuery)
        {
            return this.Container.Resolve<IDomainService<RealityObjectStructuralElement>>().GetAll()
                .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.RealityObject.Municipality.Id))
                .WhereIf(houseTypes.Length > 0, x => houseTypes.Contains((int)x.RealityObject.TypeHouse))
                .Where(x => commonEstateObjectsQuery.Contains(x.StructuralElement.Group.CommonEstateObject.Id))
                .Where(x => x.RealityObject.BuildYear != null)
                .Select(x => new
                {
                    roId = x.RealityObject.Id,
                    groupId = x.StructuralElement.Group.Id,
                    ceoId = x.StructuralElement.Group.CommonEstateObject.Id
                })
                .AsEnumerable()
                .GroupBy(x => x.roId)
                .ToDictionary(
                x => x.Key,
                x => x.Select(y => y.ceoId).Distinct().ToList());
        }

        // заполнение вертикальной секции
        public IQueryable<long> CreateVerticalColums(ReportParams reportParams)
        {
            var commonEstateObjectsQuery =
                Container.Resolve<IDomainService<CommonEstateObject>>()
                    .GetAll()
                    .Where(x => x.IncludedInSubjectProgramm);

            var commonEstateObjects =
                commonEstateObjectsQuery
                .Select(x => new { x.Id, x.Name })
                .OrderBy(x => x.Name)
                .ToList();

            var verticalSection = reportParams.ComplexReportParams.ДобавитьСекцию("GroupCost");

            foreach (var commonEstateObject in commonEstateObjects)
            {
                verticalSection.ДобавитьСтроку();
                verticalSection["GroupName"] = commonEstateObject.Name;
                verticalSection["haveElement"] = string.Format("$haveElement{0}$", commonEstateObject.Id);
                verticalSection["OOICount"] = string.Format("$OOICount{0}$", commonEstateObject.Id);
                verticalSection["OrgCount"] = string.Format("$OrgCount{0}$", commonEstateObject.Id);
            }

            return commonEstateObjectsQuery.Select(x => x.Id);
        }
    }

    internal sealed class RealtyObjProxy
    {
        public long roId;

        public string Address;

        public string MuName;

        public string MoName;

        public string MoSettlement;

    }
}

