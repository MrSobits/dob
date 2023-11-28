namespace Bars.GkhCr.Report
{
    using System;
    using System.Linq;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;

    using Castle.Windsor;

    /// <summary>
    /// Реестр договоров подряда ГЖИ
    /// </summary>
    public class BuildContractsReestrReport : BasePrintForm
    {
        #region параметры

        // идентификатор программы КР
        private int programCrId;

        #endregion

        public BuildContractsReestrReport() : base(new ReportTemplateBinary(Properties.Resources.BuildContractsReestr))
        {
        }

        public IWindsorContainer Container { get; set; }

        public override string RequiredPermission
        {
            get { return "Reports.CR.BuildContractsReestr"; }
        }

        public override string Name
        {
            get { return "Реестр договоров подряда ГЖИ"; }
        }

        public override string Desciption
        {
            get { return "Реестр договоров подряда ГЖИ"; }
        }

        public override string GroupName
        {
            get { return "Капитальный ремонт"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.BuildContractsReestrReport"; }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            this.programCrId = baseParams.Params["programCrId"].ToInt();
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            // выбронная программа КР
            var program = this.Container.Resolve<IDomainService<ProgramCr>>().GetAll().Where(x => x.Id == this.programCrId)
                                        .Select(x => new { x.Period.DateEnd, x.Period.DateStart }).FirstOrDefault();
            if (program == null)
            {
                return;
            }

            reportParams.SimpleReportParams["year"] = program.DateEnd.Value.Year;

            var objectCrQuery = this.Container.Resolve<IDomainService<ObjectCr>>().GetAll()
                .Where(x => x.ProgramCr.Id == this.programCrId);

            var objectCrIds = objectCrQuery.Select(x => x.Id);
            var realtyObjIds = objectCrQuery.Select(x => x.RealityObject.Id);

            // объекты КР текущей программы КР
            var objectsCr = objectCrQuery
                .Select(x => new
                {
                    x.Id,
                    realtyObjId = x.RealityObject.Id,
                    x.RealityObject.Address,
                    muName = x.RealityObject.Municipality.Name,
                    x.GjiNum
                })
                .OrderBy(x => x.muName)
                .ToList();

            var buildContractQuery = this.Container.Resolve<IDomainService<BuildContract>>().GetAll().Where(x => objectCrIds.Contains(x.ObjectCr.Id));

            var contragentIds = buildContractQuery.Where(x => x.Builder.Contragent != null).Select(x => x.Builder.Contragent.Id);

            var buildContractsDict = buildContractQuery
                .Select(x => new
                {
                    crObjectId = x.ObjectCr.Id,
                    x.DateAcceptOnReg,
                    x.ProtocolNum,
                    x.ProtocolDateFrom,
                    x.TypeContractBuild,
                    contragentId = (long?)x.Builder.Contragent.Id,
                    contragentName = x.Builder.Contragent.Name,
                    builderId = (long?)x.Builder.Id,
                    x.Builder.Contragent.JuridicalAddress,
                    x.Builder.Contragent.Phone,
                    x.Builder.Contragent.Inn,
                    x.DocumentNum,
                    x.DocumentDateFrom,
                    x.Sum,
                    x.DateStartWork,
                    x.DateEndWork,
                    x.Inspector.Fio
                })
                .AsEnumerable()
                .GroupBy(x => x.crObjectId)
                .ToDictionary(
                    x => x.Key,
                    x => x.Select(y => new BuildContr
                    {
                        DateAcceptOnReg = y.DateAcceptOnReg,
                        ProtocolNum = y.ProtocolNum,
                        ProtocolDateFrom = y.ProtocolDateFrom,
                        TypeContractBuild = y.TypeContractBuild,
                        ContragentId = y.contragentId,
                        ContragentName = y.contragentName,
                        BuilderId = y.builderId,
                        JuridicalAddress = y.JuridicalAddress,
                        Phone = y.Phone,
                        Inn = y.Inn,
                        DocumentNum = y.DocumentNum,
                        DocumentDateFrom = y.DocumentDateFrom,
                        Sum = y.Sum,
                        DateStartWork = y.DateStartWork,
                        DateEndWork = y.DateEndWork,
                        Fio = y.Fio
                    }).ToList());

            // документы подрядчиков
            var builderDocuments = this.Container.Resolve<IDomainService<BuilderDocument>>().GetAll()
                .Where(x => contragentIds.Contains(x.Builder.Contragent.Id))
                .Select(x => new
                {
                    Id = (long?)x.Builder.Id ?? 0,
                    x.DocumentNum,
                    x.DocumentDate,
                    Name = x.Contragent.Name ?? string.Empty
                })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(
                    x => x.Key,
                    x => x.Select(y => new { y.DocumentNum, y.DocumentDate, y.Name }).ToList());

            // виды работ подрядчика
            var builderWorks = this.Container.Resolve<IDomainService<BuilderSroInfo>>().GetAll()
                .Where(x => contragentIds.Contains(x.Builder.Id))
                .Select(x => new { x.Builder.Id, x.Work.Name })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, x => x.Select(y => y.Name).ToList());

            // сметные расчеты по работам
            var estimateCalculation = this.Container.Resolve<IDomainService<EstimateCalculation>>().GetAll()
                .Where(x => x.TypeWorkCr.Work.TypeWork != TypeWork.Service)
                .Where(x => objectCrIds.Contains(x.ObjectCr.Id))
                .Select(x => new
                {
                    objectCrId = x.ObjectCr.Id,
                    typeWorkCrId = x.TypeWorkCr.Id
                })
                .AsEnumerable()
                .GroupBy(x => x.objectCrId)
                .ToDictionary(x => x.Key, x => x.Select(y => y.typeWorkCrId).ToList());

            // Виды работ ОКР
            var typeWorks = this.Container.Resolve<IDomainService<TypeWorkCr>>().GetAll()
                .Where(x => x.Work.TypeWork == TypeWork.Work)
                .Where(x => objectCrIds.Contains(x.ObjectCr.Id))
                .Select(x => new
                {
                    objectCrId = x.ObjectCr.Id,
                    typeWorkCrId = x.Id
                })
                .AsEnumerable()
                .GroupBy(x => x.objectCrId)
                .ToDictionary(x => x.Key, x => x.Select(y => y.typeWorkCrId).ToList());

            var programPeriodStart = program.DateStart;
            var programPeriodEnd = program.DateEnd;

            // руководители контрагентов
            var contragentContacts = this.Container.Resolve<IDomainService<ContragentContact>>().GetAll()
                .Where(x => x.Position.Code.Equals("1"))
                .Where(x => contragentIds.Contains(x.Contragent.Id))
                .WhereIf(programPeriodStart != DateTime.MinValue, x => x.DateEndWork == null || x.DateEndWork >= programPeriodStart)
                .WhereIf(programPeriodEnd.HasValue && programPeriodEnd.Value != DateTime.MinValue, x => x.DateStartWork == null || x.DateStartWork <= programPeriodEnd.Value)
                .Select(x => new
                {
                    x.Contragent.Id,
                    x.Name,
                    x.Surname,
                    x.Patronymic,
                    x.Contragent.Phone,
                    x.Contragent.Inn
                })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(
                    x => x.Key,
                    x => x.Select(y => new { y.Name, y.Surname, y.Patronymic, y.Phone, y.Inn }).ToList());

            // работы КР
            var typeWorksCrDict = this.Container.Resolve<IDomainService<TypeWorkCr>>().GetAll()
                .Where(x => x.Work != null)
                .Where(x => objectCrIds.Contains(x.ObjectCr.Id))
                .Select(x => new
                {
                    objectCrId = x.ObjectCr.Id,
                    x.Sum,
                    x.Work.TypeWork
                })
                .AsEnumerable()
                .GroupBy(x => x.objectCrId)
                .ToDictionary(x => x.Key, x => x.Select(y => new { y.Sum, y.TypeWork }).ToList());

            // Управляющие организации дома
            // !! В старой берется Ук с максимальным id тут по другому
            var managingOrgRealityObject = this.Container.Resolve<IDomainService<ManOrgContractRealityObject>>().GetAll()
                .Where(x => x.ManOrgContract.StartDate.HasValue && x.ManOrgContract.StartDate <= program.DateEnd)
                .Where(x => x.ManOrgContract.EndDate == null || x.ManOrgContract.EndDate >= program.DateEnd)
                .Where(x => realtyObjIds.Contains(x.RealityObject.Id))
                .Where(x => x.ManOrgContract.ManagingOrganization != null)
                .Select(x => new
                {
                    roId = x.RealityObject.Id,
                    moId = x.ManOrgContract.ManagingOrganization.Id,
                    contragentName = x.ManOrgContract.ManagingOrganization.Contragent.Name ?? string.Empty,
                    contragentAddress = x.ManOrgContract.ManagingOrganization.Contragent.JuridicalAddress ?? string.Empty,
                    contragentPhone = x.ManOrgContract.ManagingOrganization.Contragent.Phone ?? string.Empty,
                    x.ManOrgContract.ManagingOrganization.TypeManagement
                })
                .AsEnumerable()
                .GroupBy(x => x.roId)
                .ToDictionary(
                    x => x.Key,
                    x => x.Select(y => new
                    {
                        y.moId,
                        y.contragentName,
                        y.contragentAddress,
                        y.contragentPhone,
                        y.TypeManagement
                    }).ToList());
            
            var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");
            foreach (var objectCr in objectsCr)
            {
                if (!buildContractsDict.ContainsKey(objectCr.Id))
                {
                    continue;
                }

                var objContractsCount = buildContractsDict[objectCr.Id].Count;
                var objContractsSum = buildContractsDict[objectCr.Id].Sum(x => x.Sum);
                var objContracts = buildContractsDict[objectCr.Id].Count(x => x.ProtocolNum != null && !x.ProtocolNum.Equals(string.Empty));

                foreach (var buildContract in buildContractsDict[objectCr.Id])
                {
                    // проверка на наличие комплекта ПСД
                    bool havePsd = true;
                    var estimates = estimateCalculation.ContainsKey(objectCr.Id) ? estimateCalculation[objectCr.Id] : null;
                    var workTypes = typeWorks.ContainsKey(objectCr.Id) ? typeWorks[objectCr.Id] : null;
                    if (estimates == null || workTypes == null)
                    {
                        havePsd = false;
                    }
                    else
                    {
                        if (estimates.Count != workTypes.Count)
                        {
                            havePsd = false;
                        }
                        else
                        {
                            workTypes.Where(typeWorkCr => estimates.All(x => x != typeWorkCr)).ForEach(x => havePsd = false);
                        }
                    }

                    // количество документов подрядчика
                    var documentsCount = 0;
                    if (builderDocuments.ContainsKey(buildContract.BuilderId.ToInt()))
                    {
                        documentsCount = builderDocuments[buildContract.BuilderId.ToInt()].Count;
                    }

                    if (buildContract.TypeContractBuild == TypeContractBuild.NotDefined)
                    {
                        documentsCount = 0;
                    }

                    // счетчик цикла (кол-во строк)
                    var i = 0;
                    do
                    {
                        i++;
                        section.ДобавитьСтроку();
                        section["ReestrNumber"] = objectCr.GjiNum;
                        section["RegDate"] = (buildContract.DateAcceptOnReg != null && buildContract.DateAcceptOnReg.ToDateTime() != DateTime.MinValue)
                                                 ? buildContract.DateAcceptOnReg.ToDateTime().ToShortDateString()
                                                 : string.Empty;
                        if (i <= 1 && buildContractsDict[objectCr.Id].IndexOf(buildContract) == 0)
                        {
                            section["Municipality"] = objectCr.muName;
                            section["Address"] = objectCr.Address;
                            section["FinanceLimit"] = typeWorksCrDict[objectCr.Id].Sum(x => x.Sum);
                            section["IncludingSumSmr"] =
                                typeWorksCrDict[objectCr.Id].Where(x => x.TypeWork == TypeWork.Work).Sum(x => x.Sum);

                            if (managingOrgRealityObject.ContainsKey(objectCr.realtyObjId))
                            {
                                var currentMoList = managingOrgRealityObject[objectCr.realtyObjId];
                                if (currentMoList.Count == 1)
                                {
                                    section["CustomerName"] = currentMoList.First().contragentName;
                                    section["CustomerAddress"] = string.Format("{0}, {1}", currentMoList.First().contragentAddress, currentMoList.First().contragentPhone);
                                }
                                else if (currentMoList.Count > 1)
                                {
                                    var uk = currentMoList.FirstOrDefault(x => x.TypeManagement == TypeManagementManOrg.UK);
                                    if (uk != null)
                                    {
                                        section["CustomerName"] = uk.contragentName;
                                        section["CustomerAddress"] = string.Format("{0}, {1}", uk.contragentAddress, uk.contragentPhone);
                                    }
                                    else
                                    {
                                        section["CustomerName"] = currentMoList.First().contragentName ?? string.Empty;
                                        section["CustomerAddress"] = (currentMoList.First().contragentAddress
                                                                      ?? string.Empty) + ", "
                                                                     + (currentMoList.First().contragentPhone
                                                                        ?? string.Empty);
                                    }
                                }
                            }
                            else
                            {
                                section["CustomerName"] = string.Empty;
                                section["CustomerAddress"] = string.Empty;
                            }
                        }

                        var contragentContact = string.Empty;
                        if (buildContract.ContragentId.HasValue && contragentContacts.ContainsKey(buildContract.ContragentId.Value))
                        {
                            if (contragentContacts[buildContract.ContragentId.Value] != null)
                            {
                                var contact = contragentContacts[buildContract.ContragentId.Value].First();
                                contragentContact = string.Format("{0} {1} {2}", contact.Surname, contact.Name, contact.Patronymic);
                            }
                        }

                        // СМР
                        if (buildContract.TypeContractBuild == TypeContractBuild.Smr)
                        {
                            this.FillRepetitivePart("Smr", section, buildContract, contragentContact);
                        }

                        // приборы учета
                        if (buildContract.TypeContractBuild == TypeContractBuild.Device)
                        {
                            this.FillRepetitivePart("Devices", section, buildContract, contragentContact);
                        }

                        // лифтовое оборудование
                        if (buildContract.TypeContractBuild == TypeContractBuild.Lift)
                        {
                            this.FillRepetitivePart("Elevator", section, buildContract, contragentContact);
                        }

                        section["QualificationResultsCount"] = objContracts;
                        section["BuildContractsCount"] = objContractsCount;
                        section["ContractsTotalSum"] = objContractsSum;

                        if (documentsCount > 0)
                        {
                            var builderDocument = builderDocuments[buildContract.BuilderId.ToInt()][i - 1];

                            var works = builderWorks.ContainsKey(buildContract.BuilderId.ToInt()) ? string.Join(", ", builderWorks[buildContract.BuilderId.ToInt()]) : string.Empty;

                            // СМР
                            if (buildContract.TypeContractBuild == TypeContractBuild.Smr)
                            {
                                section["CertificateNumSmr"] = builderDocument.DocumentNum;
                                section["CertificateDateSmr"] = (builderDocument.DocumentDate.HasValue && builderDocument.DocumentDate.Value != DateTime.MinValue) ? builderDocument.DocumentDate.Value.ToShortDateString() : string.Empty;
                                section["DistributorCertSmr"] = builderDocument.Name;
                                section["WorksTypeSmr"] = works;
                            }

                            // приборы учета
                            if (buildContract.TypeContractBuild == TypeContractBuild.Device)
                            {
                                section["CertificateNumDevices"] = builderDocument.DocumentNum;
                                section["CertificateDateDevices"] = (builderDocument.DocumentDate.HasValue && builderDocument.DocumentDate.Value != DateTime.MinValue) ? builderDocument.DocumentDate.Value.ToShortDateString() : string.Empty;
                                section["DistributorCertDevices"] = builderDocument.Name;
                                section["WorksTypeDevices"] = works;
                            }

                            // лифтовое оборудование
                            if (buildContract.TypeContractBuild == TypeContractBuild.Lift)
                            {
                                section["CertificateNumElevator"] = builderDocument.DocumentNum;
                                section["CertificateDateElevator"] = (builderDocument.DocumentDate.HasValue && builderDocument.DocumentDate.Value != DateTime.MinValue) ? builderDocument.DocumentDate.Value.ToShortDateString() : string.Empty;
                                section["DistributorCertElevator"] = builderDocument.Name;
                                section["WorksTypeElevator"] = works;
                            }
                        }

                        if (havePsd)
                        {
                            section["PsdSetAvailability"] = "Имеется";
                        }
                        else
                        {
                            section["PsdSetAvailability"] = "Не имеется";
                        }

                        section["InspectorFio"] = buildContract.Fio;
                    }
                    while (i < documentsCount);
                }
            }
        }

        private void FillRepetitivePart(string type, Section section, BuildContr buildContract, string contragentContact)
        {
            var protocolDateFrom = (buildContract.ProtocolDateFrom.HasValue && buildContract.ProtocolDateFrom.Value != DateTime.MinValue) ? buildContract.ProtocolDateFrom.Value.ToShortDateString() : string.Empty;
            var juridicalAddress = (buildContract.JuridicalAddress ?? string.Empty)
                                              + (!string.IsNullOrEmpty(contragentContact) ? string.Format(", {0} ", contragentContact) : string.Empty)
                                              + (buildContract.Phone != null ? string.Format(", {0} ", buildContract.Phone) : string.Empty);

            section[string.Format("ProtocolNum{0}", type)] = buildContract.ProtocolNum;
            section[string.Format("ProtocolDate{0}", type)] = protocolDateFrom;
            section[string.Format("ContractorName{0}", type)] = buildContract.ContragentName;
            section[string.Format("ContractorDetails{0}", type)] = juridicalAddress;
            section[string.Format("Inn{0}", type)] = buildContract.Inn;

            section[string.Format("ContractNum{0}", type)] = buildContract.DocumentNum;
            section[string.Format("ContractDate{0}", type)] = buildContract.DocumentDateFrom.HasValue ? buildContract.DocumentDateFrom.Value.ToShortDateString() : string.Empty;
            section[string.Format("ContractSum{0}", type)] = buildContract.Sum;
            section[string.Format("WorkStartDate{0}", type)] = buildContract.DateStartWork.HasValue ? buildContract.DateStartWork.Value.ToShortDateString() : string.Empty;
            section[string.Format("WorkEndDate{0}", type)] = buildContract.DateEndWork.HasValue ? buildContract.DateEndWork.Value.ToShortDateString() : string.Empty;
        }

        private sealed class BuildContr
        {
            public DateTime? DateAcceptOnReg { get; set; }

            public string ProtocolNum { get; set; }

            public DateTime? ProtocolDateFrom { get; set; }

            public TypeContractBuild TypeContractBuild { get; set; }

            public long? ContragentId { get; set; }

            public string ContragentName { get; set; }

            public long? BuilderId { get; set; }

            public string JuridicalAddress { get; set; }

            public string Phone { get; set; }

            public string Inn { get; set; }

            public string DocumentNum { get; set; }

            public DateTime? DocumentDateFrom { get; set; }

            public decimal? Sum { get; set; }

            public DateTime? DateStartWork { get; set; }

            public DateTime? DateEndWork { get; set; }

            public string Fio { get; set; }
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