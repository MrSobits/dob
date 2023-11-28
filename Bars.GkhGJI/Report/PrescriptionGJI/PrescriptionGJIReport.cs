namespace Bars.GkhGji.Report
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    using Bars.B4;
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Report;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class PrescriptionGjiReport : GjiBaseReport
    {
        private long DocumentId { get; set; }

        public IDomainService<DocumentGjiInspector> DocumentGjiInspectorDomainService { get; set; }

        public IDomainService<ActCheckWitness> ActCheckWitnessDomainService { get; set; }

        private Dictionary<long, string> floorsWords = new Dictionary<long, string>()
                                                          {
            {1, "одноэтажный"},
            {2, "двухэтажный"},
            {3, "трехэтажный"},
            {4, "четырехэтажный"},
            {5, "пятиэтажный"},
            {6, "шестиэтажный"},
            {7, "семиэтажный"},
            {8, "восьмиэтажный"},
            {9, "девятиэтажный"},
            {10, "десятиэтажный"},
            {11, "одиннадцатиэтажный"},
            {12, "двенадцатиэтажный"},
            {13, "тринадцатиэтажный"},
            {14, "четырнадцатиэтажный"},
            {15, "пятнадцатиэтажный"},
            {16, "шестнадцатиэтажный"},
            {17, "семнадцатиэтажный"},
            {18, "восемнадцатиэтажный"},
            {19, "девятнадцатиэтажный"},
            {20, "двадцатиэтажный"},
            {21, "двадцатиодноэтажный"},
            {22, "двадцатидвухэтажный"},
            {23, "двадцатитрехэтажный"},
            {24, "двадцатичетырехэтажный"},
            {25, "двадцатипятиэтажный}"}
                                                          };

        protected override string CodeTemplate { get; set; }

        public PrescriptionGjiReport() 
            : base(new ReportTemplateBinary(Properties.Resources.BlockGJI_ExecutiveDocPrescription))
        {
        }

        public override string Id
        {
            get { return "Prescription"; }
        }

        public override string CodeForm
        {
            get { return "Prescription"; }
        }

        public override string Name
        {
            get { return "Предписание"; }
        }

        public override string Description
        {
            get { return "Предписание"; }
        }

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            DocumentId = userParamsValues.GetValue<object>("DocumentId").ToLong();
        }

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
                       {
                           new TemplateInfo
                               {
                                   Name = "PrescriptionGJI",
                                   Code = "BlockGJI_ExecutiveDocPrescription_1",
                                   Description = "Тип обследования с кодом 22",
                                   Template = Properties.Resources.BlockGJI_ExecutiveDocPrescription_1
                               },
                           new TemplateInfo
                               {
                                   Name = "PrescriptionGJI",
                                   Code = "BlockGJI_ExecutiveDocPrescription",
                                   Description = "Любой другой случай",
                                   Template = Properties.Resources.BlockGJI_ExecutiveDocPrescription
                               }
                       };
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var prescription = Container.Resolve<IDomainService<Prescription>>().Load(DocumentId);

            if (prescription == null)
            {
                throw new ReportProviderException("Не удалось получить предписание");
            }

            // Заполняем общие поля
            FillCommonFields(reportParams, prescription);

            CodeTemplate = "BlockGJI_ExecutiveDocPrescription";

            var baseDisposal = GetParentDocument(prescription, TypeDocumentGji.Disposal);

            if (baseDisposal != null)
            {
                var typeSurveys = Container.Resolve<IDomainService<DisposalTypeSurvey>>().GetAll()
                                           .Where(x => x.Disposal.Id == baseDisposal.Id)
                                           .Select(x => x.TypeSurvey)
                                           .ToList();

                reportParams.SimpleReportParams["ПричинаОбследования"] = 
                    typeSurveys.Aggregate("", (x, y) => x + (!string.IsNullOrEmpty(x) ? ", " + y.Name : y.Name));

                GetTemplateCode(typeSurveys);

                var issuedDisposal = Container.Resolve<IDomainService<Disposal>>().GetAll().Where(x => x.Id == baseDisposal.Id).Select(x => x.IssuedDisposal).FirstOrDefault();

                if (issuedDisposal != null)
                {
                    reportParams.SimpleReportParams["РуководительФИО"] = string.Format(
                        "{0} - {1}",
                        issuedDisposal.Position,
                        string.IsNullOrEmpty(issuedDisposal.ShortFio) ? issuedDisposal.Fio : issuedDisposal.ShortFio);
                }

                var queryInspectorId = DocumentGjiInspectorDomainService.GetAll()
                                                    .Where(x => x.DocumentGji.Id == baseDisposal.Id)
                                                    .Select(x => x.Inspector.Id);

                var listLocality = Container.Resolve<IDomainService<ZonalInspectionInspector>>().GetAll()
                                    .Where(x => queryInspectorId.Contains(x.Inspector.Id))
                                    .Select(x => x.ZonalInspection.Locality)
                                    .Distinct()
                                    .ToList();

                reportParams.SimpleReportParams["НаселПунктОтдела"] = string.Join("; ", listLocality);
            }

            var actDoc = Container.Resolve<IDomainService<DocumentGjiChildren>>().GetAll()
                .Where(x => x.Children.Id == prescription.Id && x.Parent.TypeDocumentGji == TypeDocumentGji.ActCheck)
                .Select(x => x.Parent)
                .FirstOrDefault();

            if (actDoc != null)
            {
                var act = Container.Resolve<IDomainService<ActCheck>>().GetAll().FirstOrDefault(x => x.Id == actDoc.Id);

                reportParams.SimpleReportParams["Кв"] = act.Flat;

                reportParams.SimpleReportParams["ИнспектируемаяЧасть"] = Container.Resolve<IDomainService<ActCheckInspectedPart>>().GetAll()
                    .Where(x => x.ActCheck.Id == act.Id)
                    .Select(x => x.InspectedPart)
                    .Aggregate("", (x, y) => x + (!string.IsNullOrEmpty(x) ? ", " + y.Name : y.Name));

                reportParams.SimpleReportParams["НомерАктаПроверки"] = actDoc.DocumentNumber;
                reportParams.SimpleReportParams["ДатаАктаПроверки"] = actDoc.DocumentDate.HasValue ? actDoc.DocumentDate.Value.ToShortDateString() : string.Empty;

                var firstInspector = DocumentGjiInspectorDomainService.GetAll()
                                          .Where(x => x.DocumentGji.Id == DocumentId)
                                          .Select(x => new
                                          {
                                              x.Inspector.ShortFio,
                                              x.Inspector.FioAblative
                                          })
                                          .FirstOrDefault();

                if (firstInspector != null && !string.IsNullOrEmpty(firstInspector.ShortFio))
                {
                    reportParams.SimpleReportParams["ИнспекторФамИО"] = firstInspector.ShortFio;
                    reportParams.SimpleReportParams["ИнспекторФамИОТП"] = string.Format("{0} {1}", firstInspector.FioAblative.Split(' ')[0], firstInspector.ShortFio.Substring(firstInspector.ShortFio.IndexOf(' ')));
                }

                reportParams.SimpleReportParams["ДатаАкта"] = act.DocumentDate.ToDateTime().ToString("d MMMM yyyy");
                reportParams.SimpleReportParams["НомерАкта"] = act.DocumentNumber;

                var firstWitness = ActCheckWitnessDomainService.GetAll().Where(x => x.ActCheck.Id == actDoc.Id)
                    .Select(x => new { x.Fio, x.Position })
                    .FirstOrDefault();
                reportParams.SimpleReportParams["Вприсутствии"] = firstWitness != null
                                                                      ? string.Format("{0} - {1}", firstWitness.Fio, firstWitness.Position)
                                                                      : string.Empty;
            }

            var disposal = Container.Resolve<IDomainService<Disposal>>()
                                     .GetAll()
                                     .Where(x => x.Stage.Id == prescription.Stage.Parent.Id)
                                     .FirstOrDefault();

            if (disposal != null)
            {
                reportParams.SimpleReportParams["ДатаРаспоряжения"] = disposal.DocumentDate.ToDateTime().ToString("d MMMM yyyy");
                reportParams.SimpleReportParams["НомерРаспоряжения"] = disposal.DocumentNumber;
            }

            reportParams.SimpleReportParams["ДатаПредписания"] = prescription.DocumentDate.HasValue
                                                                     ? prescription.DocumentDate.Value.ToString("D", new CultureInfo("ru-RU"))
                                                                     : string.Empty;
            reportParams.SimpleReportParams["НомерПредписания"] = prescription.DocumentNumber;

            var realityObj = Container.Resolve<IDomainService<PrescriptionViol>>().GetAll()
                                      .Where(x => x.Document.Id == prescription.Id)
                                      .Select(x => x.InspectionViolation.RealityObject)
                                      .ToList()
                                      .Distinct(x => x.Id);

            // если дом один, то выводим адрес дома и номер дома, если домов нет или больше 1 - ничего не выводим
            if (realityObj.Count() == 1)
            {
                var firstPrescriptionRo = realityObj.FirstOrDefault();

                if (firstPrescriptionRo != null)
                {
                    reportParams.SimpleReportParams["Район"] = firstPrescriptionRo.Municipality != null
                                                                   ? firstPrescriptionRo.Municipality.Name
                                                                   : "";
                    reportParams.SimpleReportParams["Стена"] = firstPrescriptionRo.WallMaterial != null ? firstPrescriptionRo.WallMaterial.Name : "";
                    reportParams.SimpleReportParams["Кровля"] = firstPrescriptionRo.RoofingMaterial != null ? firstPrescriptionRo.RoofingMaterial.Name : "";
                    reportParams.SimpleReportParams["Этажей"] = firstPrescriptionRo.MaximumFloors;
                    reportParams.SimpleReportParams["Квартир"] = firstPrescriptionRo.NumberApartments;
                    reportParams.SimpleReportParams["ОбщаяПлощадь"] = firstPrescriptionRo.AreaMkd;
                    reportParams.SimpleReportParams["ГодЭксп"] = firstPrescriptionRo.DateCommissioning.HasValue
                                                                     ? firstPrescriptionRo.DateCommissioning.Value.ToShortDateString()
                                                                     : "";
                    reportParams.SimpleReportParams["Подвал"] = firstPrescriptionRo.HavingBasement;

                    reportParams.SimpleReportParams["ОбследПл"] = firstPrescriptionRo.AreaMkd.HasValue ? firstPrescriptionRo.AreaMkd.Value.RoundDecimal(2).ToStr() : "";

                    if (firstPrescriptionRo.FiasAddress != null)
                    {
                        var fias = firstPrescriptionRo.FiasAddress;

                        reportParams.SimpleReportParams["АдресДома"] = fias.PlaceName + ", " + fias.StreetName;
                        reportParams.SimpleReportParams["НаселенныйПункт"] = fias.PlaceName;
                        reportParams.SimpleReportParams["НомерДома"] = fias.House;
                        reportParams.SimpleReportParams["Корпус"] = fias.Housing;
                        reportParams.SimpleReportParams["Секций"] = firstPrescriptionRo.NumberEntrances;
                        reportParams.SimpleReportParams["Улица"] = fias.StreetName;
                    }

                    if (firstPrescriptionRo.MaximumFloors != null)
                    {
                        var floor = firstPrescriptionRo.MaximumFloors.ToLong();
                        if (floorsWords.ContainsKey(floor))
                        {
                            reportParams.SimpleReportParams["ЭтажейПропис"] = floorsWords[floor];
                        }
                    }

                }
            }

            var contragent = prescription.Contragent;

            if (contragent != null)
            {
                reportParams.SimpleReportParams["ИНН"] = contragent.Inn;
                reportParams.SimpleReportParams["КПП"] = contragent.Kpp;

                if (contragent.FiasJuridicalAddress != null)
                {
                    var subStr = contragent.FiasJuridicalAddress.AddressName.Split(',');

                    var newAddr = new StringBuilder();

                    foreach (var rec in subStr)
                    {
                        if (newAddr.Length > 0)
                        {
                            newAddr.Append(',');
                        }

                        if (rec.Contains("р-н."))
                        {
                            var mu = rec.Replace("р-н.", "") + " район";
                            newAddr.Append(mu);
                            continue;
                        }

                        newAddr.Append(rec);
                    }

                    reportParams.SimpleReportParams["АдресКонтрагента"] = newAddr;
                }
                else
                {
                    reportParams.SimpleReportParams["АдресКонтрагента"] = contragent.JuridicalAddress;
                }
            }

            if (prescription.Inspection.TypeBase == TypeBase.CitizenStatement)
            {
                reportParams.SimpleReportParams["ПричинаОбследования"] = "По жалобе";
            }

            if (prescription.Executant != null)
            {
                switch (prescription.Executant.Code)
                {
                    case "0":
                    case "2":
                    case "4":
                    case "10":
                    case "12":
                    case "6":
                    case "7":
                    case "15":
                    case "21": //ИП
                        reportParams.SimpleReportParams["КомуВыдано"] = prescription.Contragent != null
                                                                            ? prescription.Contragent.Name
                                                                            : "";
                        break;
                    case "1":
                    case "3":
                    case "5":
                    case "11":
                    case "13":
                    case "16":
                    case "18":
                    case "19":
                        {
                            var result = prescription.Contragent != null ? prescription.Contragent.Name : "";

                            if (!string.IsNullOrEmpty(result) && !string.IsNullOrEmpty(prescription.PhysicalPerson))
                                result += ", ";

                            result += prescription.PhysicalPerson;

                            reportParams.SimpleReportParams["КомуВыдано"] = result;
                        }

                        break;
                    case "8":
                    case "9":
                    case "14":
                        reportParams.SimpleReportParams["КомуВыдано"] = prescription.PhysicalPerson;
                        break;
                }

                switch (prescription.Executant.Code)
                {
                    case "0":
                    case "2":
                    case "4":
                    case "10":
                    case "12":
                    case "6":
                    case "7":
                    case "15":
                    case "21": //ИП
                        reportParams.SimpleReportParams["КомуВыдано1"] = prescription.Contragent != null
                                                                  ? prescription.Contragent.Name
                                                                  : string.Empty;
                        break;
                    case "8":
                    case "9":
                    case "14":
                        reportParams.SimpleReportParams["КомуВыдано1"] = prescription.PhysicalPerson;
                        break;
                }

                switch (prescription.Executant.Code)
                {
                    case "1":
                    case "3":
                    case "5":
                    case "11":
                    case "13":
                    case "16":
                    case "18":
                    case "19":
                        var contName = prescription.Contragent != null ? prescription.Contragent.Name : string.Empty;
                        reportParams.SimpleReportParams["КомуВыданоДЛ"] = prescription.PhysicalPerson + " - " + contName;
                        break;
                    case "6":
                    case "7":
                    case "14":
                        reportParams.SimpleReportParams["КомуВыданоДЛ"] = prescription.PhysicalPerson;
                        break;
                }
            }

            reportParams.SimpleReportParams["Реквизиты"] = prescription.PhysicalPersonInfo;

            string typeControl = null;
            var firstRo = realityObj.FirstOrDefault();

            if (firstRo != null)
            {
                var date = prescription.DocumentDate ?? DateTime.Now.Date;

                var typeCurrentContract = Container.Resolve<IDomainService<ManOrgContractRealityObject>>().GetAll()
                    .Where(x => x.RealityObject.Id == firstRo.Id)
                    .Where(x => x.ManOrgContract.StartDate <= date)
                    .Where(x => !x.ManOrgContract.EndDate.HasValue || x.ManOrgContract.EndDate >= date)
                    .OrderByDescending(x => x.ManOrgContract.EndDate)
                    .Select(x => new
                    {
                        x.ManOrgContract.TypeContractManOrgRealObj,
                        TypeManagement = (TypeManagementManOrg?) x.ManOrgContract.ManagingOrganization.TypeManagement
                    })
                    .FirstOrDefault();

                if (typeCurrentContract != null)
                {
                    if (typeCurrentContract.TypeManagement.HasValue)
                    {
                        switch (typeCurrentContract.TypeManagement.Value)
                        {
                            case TypeManagementManOrg.TSJ:
                                typeControl = "2";
                                break;
                            case TypeManagementManOrg.UK:
                                typeControl = "3";
                                break;
                        }
                    }
                    else
                    {
                        switch (typeCurrentContract.TypeContractManOrgRealObj)
                        {
                            case TypeContractManOrg.DirectManag:
                                typeControl = "1";
                                break;
                            case TypeContractManOrg.JskTsj:
                                typeControl = "2";
                                break;
                            //Ук
                            case TypeContractManOrg.ManagingOrgJskTsj:
                            case TypeContractManOrg.ManagingOrgOwners:
                                typeControl = "3";
                                break;
                        }
                    }
                }
            }

            reportParams.SimpleReportParams["СпособУправления"] = typeControl;

            #region Секция нарушений

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("СекцияНарушений");

            var violations = Container.Resolve<IDomainService<PrescriptionViol>>().GetAll()
                .Where(x => x.Document.Id == prescription.Id)
                .Select(x => new
                    {
                        ViolationId = x.InspectionViolation.Violation.Id,
                        ViolationCodePin = x.InspectionViolation.Violation.CodePin,
                        ViolationName = x.InspectionViolation.Violation.Name,
                        x.Action,
                        x.Description,
                        DatePlanRemoval = x.DatePlanRemoval ?? x.InspectionViolation.DatePlanRemoval,
                        x.InspectionViolation.Violation.PpRf170,
                        x.InspectionViolation.Violation.PpRf25,
                        x.InspectionViolation.Violation.PpRf307,
                        x.InspectionViolation.Violation.PpRf491,
                        x.InspectionViolation.Violation.OtherNormativeDocs
                    })
                .AsEnumerable()
                .Distinct()
                .ToList();
            var strCodePin = new StringBuilder();

            var i = 0;
            var addedList = new List<long>();
            foreach (var viol in violations.Where(viol => !addedList.Contains(viol.ViolationId)))
            {
                addedList.Add(viol.ViolationId);

                section.ДобавитьСтроку();
                section["Номер1"] = ++i;
                section["Пункт"] = viol.ViolationCodePin;
                section["ТекстНарушения"] = viol.ViolationName;
                section["Мероприятие"] = viol.Action;
                section["Мероприятия"] = viol.Action;
                section["Подробнее"] = viol.Description;
                var strDateRemoval = viol.DatePlanRemoval.HasValue
                                                ? viol.DatePlanRemoval.Value.ToShortDateString()
                                                : string.Empty;

                section["СрокУстранения"] = strDateRemoval;
                section["СрокиИсполнения"] = strDateRemoval;
                section["СрокИсполненияПредписания"] = strDateRemoval;

                section["ПП_РФ_170"] = viol.PpRf170;
                section["ПП_РФ_25"] = viol.PpRf25;
                section["ПП_РФ_307"] = viol.PpRf307;
                section["ПП_РФ_491"] = viol.PpRf491;
                section["Прочие_норм_док"] = viol.OtherNormativeDocs;

                if (strCodePin.Length > 0)
                {
                    strCodePin.Append(", ");
                }

                 strCodePin.Append(viol.ViolationCodePin.Replace("ПиН ", string.Empty));
            }

            reportParams.SimpleReportParams["ПиН"] = strCodePin;

            #endregion

            if (CodeTemplate == "BlockGJI_ExecutiveDocPrescription")
            {
                var inspectors = DocumentGjiInspectorDomainService.GetAll()
                                                   .Where(x => x.DocumentGji.Id == DocumentId)
                                                   .Select(x => new
                                                        {
                                                            x.Inspector.Fio,
                                                            x.Inspector.Position,
                                                            x.Inspector.ShortFio,
                                                            x.Inspector.FioAblative
                                                        })
                                                   .ToArray();

                reportParams.SimpleReportParams["ИнспекторыИКоды"] = inspectors.AggregateWithSeparator(x => x.Fio + " - " + x.Position, ", ");

                if (prescription.Inspection.TypeBase == TypeBase.CitizenStatement)
                {
                    var listBaseStatementAppealCits =
                        Container.Resolve<IDomainService<InspectionAppealCits>>().GetAll()
                            .Where(x => x.Inspection.Id == prescription.Inspection.Id)
                                 .Select(x => x.AppealCits.DocumentNumber)
                                 .ToList();

                    reportParams.SimpleReportParams["НомерОбращения"] = listBaseStatementAppealCits.Aggregate(string.Empty, 
                                                                                    (total, next) => total == string.Empty ? next : total + ", " + next);
                }

                if (disposal!=null && disposal.Inspection != null)
                {
                    var listRealityObjs = Container.Resolve<IDomainService<InspectionGjiRealityObject>>().GetAll()
                             .Where(x => x.Inspection.Id == disposal.Inspection.Id)
                             .Select(x => new
                             {
                                 city = x.RealityObject.FiasAddress.PlaceName,
                                 street = x.RealityObject.FiasAddress.StreetName,
                                 house = x.RealityObject.FiasAddress.House
                             })
                             .ToList();

                    if (listRealityObjs.Count > 0)
                    {
                        var city = listRealityObjs.FirstOrDefault().city;
                        var strtmp = listRealityObjs.Aggregate(
                            string.Empty,
                            (total, next) => string.Format("{0} {1} {2}", total == string.Empty ? "" : total + ";", next.street, next.house));

                        reportParams.SimpleReportParams["ДомаИАдреса"] = city + strtmp;
                    }
                }                
            }

            var instFioPositions = DocumentGjiInspectorDomainService.GetAll()
                                                 .Where(x => x.DocumentGji.Id == prescription.Id)
                                                 .Select(x => new { x.Inspector.FioAblative, x.Inspector.PositionAblative })
                                                 .ToList();

            reportParams.SimpleReportParams["ИнспекторДолжностьТП"] = string.Join(", ", instFioPositions.Select(x => string.Format("{0} - {1}", x.FioAblative, x.PositionAblative)).ToArray());

            var insp = DocumentGjiInspectorDomainService.GetAll()
                    .Where(x => x.DocumentGji.Id == DocumentId)
                    .Select(x => new
                    {
                        x.Inspector.Fio,
                        x.Inspector.Position,
                        x.Inspector.ShortFio,
                        x.Inspector.FioAblative
                    })
                    .ToArray();

            reportParams.SimpleReportParams["ИнспекторДолжность"] = insp.AggregateWithSeparator(x => x.Fio + " - " + x.Position, ", ");

            if (insp.Length > 0)
            {
                var firstInsp = insp.First();
                reportParams.SimpleReportParams["ДолжностьИнспектора"] = firstInsp.Position;
            }

            this.FillRegionParams(reportParams, prescription);
        }


        protected virtual void GetTemplateCode(List<TypeSurveyGji> typeSurveys)
        {
            CodeTemplate = "BlockGJI_ExecutiveDocPrescription";

            if (typeSurveys.Any(x => x.Code == "22"))
            {
                CodeTemplate = "BlockGJI_ExecutiveDocPrescription_1";
            }
        }
    }
}