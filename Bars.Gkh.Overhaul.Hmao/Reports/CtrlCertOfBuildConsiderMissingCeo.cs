namespace Bars.Gkh.Overhaul.Hmao.Reports
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

    using Castle.Windsor;
    using Gkh.Entities.CommonEstateObject;

    class CtrlCertOfBuildConsiderMissingCeo : BasePrintForm
    {
        public CtrlCertOfBuildConsiderMissingCeo()
            : base(new ReportTemplateBinary(Properties.Resources.CtrlCertOfBuildConsiderMissingCeo))
        {
        }

        #region параметры отчета
        private long[] municipalityIds;

        private bool typeManyApartments;
        private bool typeSocialBehavior;
        private bool typeIndividual;
        private bool typeBlockedBuilding;

        #endregion

        public IWindsorContainer Container { get; set; }
        
        public override string Name
        {
            get { return "Контроль паспортизации домов с учетом отсутствующих элементов"; }
        }

        public override string Desciption
        {
            get { return "Контроль паспортизации домов с учетом отсутствующих элементов"; }
        }

        public override string GroupName
        {
            get { return "Жилые дома"; }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.CtrlCertOfBuildConsiderMissingCeo";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Ovrhl.CtrlCertOfBuildConsiderMissingCeo";
            }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            var municipalityIdsList = baseParams.Params.GetAs("municipalityIds", string.Empty);
            municipalityIds = !string.IsNullOrEmpty(municipalityIdsList)
                                  ? municipalityIdsList.Split(',').Select(id => id.ToLong()).ToArray()
                                  : new long[0];

            typeManyApartments = baseParams.Params.GetAs("typeManyApartments", false);
            typeSocialBehavior = baseParams.Params.GetAs("typeSocialBehavior", false);
            typeBlockedBuilding = baseParams.Params.GetAs("typeBlockedBuilding", false);
            typeIndividual = baseParams.Params.GetAs("typeIndividual", false);
        }

        public override string ReportGenerator { get; set; }
        
        public override void PrepareReport(ReportParams reportParams)
        {
            var commonEstateObjectsQuery = CreateVerticalColums(reportParams);
            var ceoIds = commonEstateObjectsQuery.ToList();

            var realtyObjByMuDict = GetRealtyObjects();
            var realityObjectStructuralElement = this.GetData(commonEstateObjectsQuery);
            var realObjMissingCommonEstObj = this.GetMissingCeo(commonEstateObjectsQuery);

            var sectionMu = reportParams.ComplexReportParams.ДобавитьСекцию("sectionMu");
            var sectionRo = sectionMu.ДобавитьСекцию("sectionRo");
            var num = 1;
            var sumPercent= 0M;
            var moDomainService = Container.Resolve<IDomainService<Municipality>>();

            var manOrgByRo = Container.Resolve<IDomainService<ManOrgContractRealityObject>>().GetAll()
                .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.RealityObject.Municipality.Id))
                .Where(x => x.ManOrgContract.StartDate == null || x.ManOrgContract.StartDate <= DateTime.Now)
                .Where(x => x.ManOrgContract.EndDate == null || x.ManOrgContract.EndDate >= DateTime.Now.Date)
                .Where(x => x.ManOrgContract.ManagingOrganization != null)
                .Select(x => new
                {
                    moName = x.ManOrgContract.ManagingOrganization.Contragent.Name ?? string.Empty,
                    roId = x.RealityObject.Id
                })
                .ToList();

            foreach (var realtyObjectsByMu in realtyObjByMuDict.OrderBy(x => x.Key))
            {
                sectionMu.ДобавитьСтроку();
                sectionMu["MuName"] = realtyObjectsByMu.Key;
                var sumPercentByMu = 0M;

                foreach (var realtyObjectInfo in realtyObjectsByMu.Value.OrderBy(x => x.Address))
                {
                    sectionRo.ДобавитьСтроку();

                    var realtyObjectId = realtyObjectInfo.roId;

                    sectionRo["Num"] = num++;
                    sectionRo["MO"] = realtyObjectsByMu.Key;
                    sectionRo["AD"] = realtyObjectInfo.MoSettlement;
                    sectionRo["Address"] = realtyObjectInfo.Address;
                    sectionRo["ManOrg"] = manOrgByRo.Where(x => x.roId == realtyObjectId).Select(x => x.moName)
                        .Aggregate(string.Empty, (x, y) => x + (!string.IsNullOrEmpty(x) ? ", " + y : y));

                    var currentRoCeoIdsList = realityObjectStructuralElement.ContainsKey(realtyObjectInfo.roId) 
                        ? realityObjectStructuralElement[realtyObjectInfo.roId]
                        : new List<long>();

                    var currentRoMissingCeoIdsList = realObjMissingCommonEstObj.ContainsKey(realtyObjectInfo.roId)
                        ? realObjMissingCommonEstObj[realtyObjectInfo.roId].Where(x => !currentRoCeoIdsList.Contains(x)).ToList()
                        : new List<long>();

                    var tempCeoCount = ceoIds.Count - currentRoMissingCeoIdsList.Count();
                    var currentRoPercent = tempCeoCount != 0 ? currentRoCeoIdsList.Count / tempCeoCount.ToDecimal() : 0;
                    currentRoPercent = currentRoPercent > 1 ? 1 : currentRoPercent;

                    sumPercentByMu += currentRoPercent;
                    sectionRo["PercentOccup"] = currentRoPercent;

                    foreach (var ceoId in ceoIds)
                    {
                        var haveConElem = currentRoCeoIdsList.Contains(ceoId) ? "1" : currentRoMissingCeoIdsList.Contains(ceoId) ? "-" : "0";
                        sectionRo[string.Format("haveElement{0}", ceoId)] = haveConElem;
                    }
                }

                var currentMuPercent = sumPercentByMu / realtyObjectsByMu.Value.Count;
                sumPercent += currentMuPercent;
                sectionMu["AverageMun"] = currentMuPercent;
            }

            var cnt = realtyObjByMuDict.Select(y => y.Value).Count();
            reportParams.SimpleReportParams["AverageAllMun"] = ( sumPercent > 0 && sumPercent > 0 ? sumPercent / cnt : 0 );
        }

        public IQueryable<RealityObject> GetRoQueryable()
        {
            return Container.Resolve<IDomainService<RealityObject>>().GetAll()
                .Where(x => x.TypeHouse != TypeHouse.NotSet)
                .WhereIf(!typeManyApartments, x => x.TypeHouse != TypeHouse.ManyApartments)
                .WhereIf(!typeSocialBehavior, x => x.TypeHouse != TypeHouse.SocialBehavior)
                .WhereIf(!typeIndividual, x => x.TypeHouse != TypeHouse.Individual)
                .WhereIf(!typeBlockedBuilding, x => x.TypeHouse != TypeHouse.BlockedBuilding)
                .Where(x => x.ConditionHouse == ConditionHouse.Dilapidated || x.ConditionHouse == ConditionHouse.Serviceable)
                .WhereIf(municipalityIds.Length > 0,
                    x => municipalityIds.Contains(x.Municipality.Id) 
                        || municipalityIds.Contains(x.MoSettlement.Id));
        }

        public Dictionary<string, List<RealtyObjProxy>> GetRealtyObjects()
        {
            return this.GetRoQueryable()
                 .Select(x => new
                 {
                     x.Municipality.Name,
                     roId = x.Id,
                     x.Address,
                     MoSettlement = x.MoSettlement != null ? x.MoSettlement.Name : "",
                 })
                .AsEnumerable()
                .GroupBy(x => x.Name)
                .ToDictionary(
                x => x.Key,
                x => x.Select(y => new RealtyObjProxy { roId = y.roId, Address = y.Address, MoSettlement = y.MoSettlement }).ToList());
        }

        // получение групп конструктивных элементов по домам
        public Dictionary<long, List<long>> GetData(IQueryable<long> commonEstateObjectsQuery)
        {
            return this.Container.Resolve<IDomainService<RealityObjectStructuralElement>>().GetAll()
                .Where(x => GetRoQueryable().Any(y => y.Id == x.RealityObject.Id))
                .Where(x => commonEstateObjectsQuery.Contains(x.StructuralElement.Group.CommonEstateObject.Id))
                .Select(x => new
                {
                    roId = x.RealityObject.Id,
                    ceoId = x.StructuralElement.Group.CommonEstateObject.Id
                })
                .AsEnumerable()
                .GroupBy(x => x.roId)
                .ToDictionary(x => x.Key,
                    x => x.Select(y => y.ceoId).Distinct().ToList());
        }

        // получение отсутствующих объектов общего имущества по домам
        public Dictionary<long, List<long>> GetMissingCeo(IQueryable<long> commonEstateObjectsQuery)
        {
            return this.Container.Resolve<IDomainService<RealityObjectMissingCeo>>().GetAll()
                .Where(x => GetRoQueryable().Any(y => y.Id == x.RealityObject.Id))
                .Where(x => commonEstateObjectsQuery.Contains(x.MissingCommonEstateObject.Id))
                .Select(x => new
                {
                    roId = x.RealityObject.Id,
                    ceoId = x.MissingCommonEstateObject.Id
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
                Container.Resolve<IDomainService<CommonEstateObject>>().GetAll()
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
            }

            return commonEstateObjectsQuery.Select(x => x.Id);
        }
    }
}

