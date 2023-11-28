using NHibernate.Bytecode.Lightweight;

namespace Bars.GkhGji.Regions.Stavropol.Report
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
    using Bars.GkhGji.Report;
    using NHibernate.Linq;
    using Stimulsoft.Report;
	using Bars.GkhGji.DomainService;

    public class PrescriptionGjiStimulReport : GjiBaseStimulReport
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

        public PrescriptionGjiStimulReport()
            : base(new ReportTemplateBinary(Properties.Resources.BlockGJI_ExecutiveDocPrescription))
        {
            ExportFormat = StiExportFormat.Word2007;
            CodeTemplate = "BlockGJI_ExecutiveDocPrescription_2";
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
                                   Code = "BlockGJI_ExecutiveDocPrescription",
                                   Description = "Тип Исполнителя 6,7,14",
                                   Template = Properties.Resources.BlockGJI_ExecutiveDocPrescription
                               },
                           new TemplateInfo
                               {
                                   Name = "PrescriptionGJI",
                                   Code = "BlockGJI_ExecutiveDocPrescription_2",
                                   Description = "Тип Исполнителя 0,9,11,8,15,18,4",
                                   Template = Properties.Resources.BlockGJI_ExecutiveDocPrescription_2
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
            FillCommonFields(prescription);
            SetTemplateCode(prescription.Executant.Code);

            var baseDisposal = GetParentDocument(prescription, TypeDocumentGji.Disposal);

            if (baseDisposal != null)
            {
                var typeSurveys = Container.Resolve<IDomainService<DisposalTypeSurvey>>().GetAll()
                                           .Where(x => x.Disposal.Id == baseDisposal.Id)
                                           .Select(x => x.TypeSurvey)
                                           .ToList();

                Report["ПричинаОбследования"] =
                    typeSurveys.Aggregate("", (x, y) => x + (!string.IsNullOrEmpty(x) ? ", " + y.Name : y.Name));

                var issuedDisposal = Container.Resolve<IDomainService<Disposal>>().GetAll().Where(x => x.Id == baseDisposal.Id).Select(x => x.IssuedDisposal).FirstOrDefault();

                if (issuedDisposal != null)
                {
                    Report["РуководительФИО"] = string.Format(
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

                Report["НаселПунктОтдела"] = string.Join("; ", listLocality);
            }

            var actDoc = Container.Resolve<IDomainService<DocumentGjiChildren>>().GetAll()
                .Where(x => x.Children.Id == prescription.Id && x.Parent.TypeDocumentGji == TypeDocumentGji.ActCheck)
                .Select(x => x.Parent)
                .FirstOrDefault();

            if (actDoc != null)
            {
                var act = Container.Resolve<IDomainService<ActCheck>>().GetAll().FirstOrDefault(x => x.Id == actDoc.Id);

                Report["Кв"] = act.Flat;

                Report["ИнспектируемаяЧасть"] = Container.Resolve<IDomainService<ActCheckInspectedPart>>().GetAll()
                    .Where(x => x.ActCheck.Id == act.Id)
                    .Select(x => x.InspectedPart)
                    .Aggregate("", (x, y) => x + (!string.IsNullOrEmpty(x) ? ", " + y.Name : y.Name));

                Report["НомерАктаПроверки"] = actDoc.DocumentNumber;
                Report["ДатаАктаПроверки"] = actDoc.DocumentDate.HasValue ? actDoc.DocumentDate.Value.ToShortDateString() : string.Empty;

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
                    Report["ИнспекторФамИО"] = firstInspector.ShortFio;
                    Report["ИнспекторФамИОТП"] = string.Format("{0} {1}", firstInspector.FioAblative.Split(' ')[0], firstInspector.ShortFio.Substring(firstInspector.ShortFio.IndexOf(' ')));
                }

                Report["ДатаАкта"] = act.DocumentDate.ToDateTime().ToString("d MMMM yyyy");
                Report["НомерАкта"] = act.DocumentNumber;

                var firstWitness = ActCheckWitnessDomainService.GetAll().Where(x => x.ActCheck.Id == actDoc.Id)
                    .Select(x => new { x.Fio, x.Position })
                    .FirstOrDefault();
                Report["Вприсутствии"] = firstWitness != null
                                                                      ? string.Format("{0} - {1}", firstWitness.Fio, firstWitness.Position)
                                                                      : string.Empty;
            }

            var disposal = Container.Resolve<IDomainService<Disposal>>()
                .GetAll().FirstOrDefault(x => x.Stage.Id == prescription.Stage.Parent.Id);

            if (disposal != null)
            {
                Report["ДатаРаспоряжения"] = disposal.DocumentDate.ToDateTime().ToString("d MMMM yyyy");
                Report["НомерРаспоряжения"] = disposal.DocumentNumber;
            }

            Report["ДатаПредписания"] = prescription.DocumentDate.HasValue
                                                                     ? prescription.DocumentDate.Value.ToString("D", new CultureInfo("ru-RU"))
                                                                     : string.Empty;
            Report["НомерПредписания"] = prescription.DocumentNumber;

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
                    Report["Район"] = firstPrescriptionRo.Municipality != null
                                                                   ? firstPrescriptionRo.Municipality.Name
                                                                   : "";
                    Report["Стена"] = firstPrescriptionRo.WallMaterial != null ? firstPrescriptionRo.WallMaterial.Name : "";
                    Report["Кровля"] = firstPrescriptionRo.RoofingMaterial != null ? firstPrescriptionRo.RoofingMaterial.Name : "";
                    Report["Этажей"] = firstPrescriptionRo.MaximumFloors;
                    Report["Квартир"] = firstPrescriptionRo.NumberApartments;
                    Report["ОбщаяПлощадь"] = firstPrescriptionRo.AreaMkd;
                    Report["ГодЭксп"] = firstPrescriptionRo.DateCommissioning.HasValue
                                                                     ? firstPrescriptionRo.DateCommissioning.Value.ToShortDateString()
                                                                     : "";
                    Report["Подвал"] = firstPrescriptionRo.HavingBasement;

                    Report["ОбследПл"] = firstPrescriptionRo.AreaMkd.HasValue ? firstPrescriptionRo.AreaMkd.Value.RoundDecimal(2).ToStr() : "";

                    if (firstPrescriptionRo.FiasAddress != null)
                    {
                        var fias = firstPrescriptionRo.FiasAddress;

                        Report["АдресДома"] = fias.PlaceName + ", " + fias.StreetName;
                        Report["НаселенныйПункт"] = fias.PlaceName;
                        Report["НомерДома"] = fias.House;
                        Report["Корпус"] = fias.Housing;
                        Report["Секций"] = firstPrescriptionRo.NumberEntrances;
                        Report["Улица"] = fias.StreetName;
                    }

                    if (firstPrescriptionRo.MaximumFloors != null)
                    {
                        var floor = firstPrescriptionRo.MaximumFloors.ToLong();
                        if (floorsWords.ContainsKey(floor))
                        {
                            Report["ЭтажейПропис"] = floorsWords[floor];
                        }
                    }

                }
            }

            var contragent = prescription.Contragent;

            if (contragent != null)
            {
                Report["ИНН"] = contragent.Inn;
                Report["КПП"] = contragent.Kpp;
                Report["Контрагент"] = contragent.Name;
                Report["ТелефонКонтрагента"] = contragent.Phone;

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

                    Report["АдресКонтрагента"] = newAddr.ToString();
                }
                else
                {
                    Report["АдресКонтрагента"] = contragent.JuridicalAddress;
                }
            }

            if (prescription.Inspection.TypeBase == TypeBase.CitizenStatement)
            {
                Report["ПричинаОбследования"] = "По жалобе";
            }

            Report["ФизЛицо"] = prescription.PhysicalPerson;
			Report["АдресФизЛица"] = prescription.PhysicalPersonInfo;

			var prescriptionService = Container.Resolve<IPrescriptionService>();
			var prescriptionInfo = prescriptionService.GetInfo(prescription.Id);
			Report["ДокументОснов"] = GetDocumentBase(prescriptionInfo.Data);

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
                        TypeManagement = (TypeManagementManOrg?)x.ManOrgContract.ManagingOrganization.TypeManagement
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

            Report["СпособУправления"] = typeControl;

            #region Секция нарушений

            var violations = Container.Resolve<IDomainService<PrescriptionViol>>().GetAll()
                .Where(x => x.Document.Id == prescription.Id)
                //.Fetch(x => x.InspectionViolation)
                //.ThenFetch(x => x.Violation)
                .Select(x => new
                    {
                        ViolationId = x.InspectionViolation.Violation.Id,
                        ViolationCodePin = x.InspectionViolation.Violation.CodePin,
                        ViolationName = x.InspectionViolation.Violation.Name,
                        x.Action,
                        x.Description,
                        x.InspectionViolation.DatePlanRemoval,
                        x.InspectionViolation.Violation.PpRf170,
                        x.InspectionViolation.Violation.PpRf25,
                        x.InspectionViolation.Violation.PpRf307,
                        x.InspectionViolation.Violation.PpRf491,
                        x.InspectionViolation.Violation.OtherNormativeDocs,
                        x.InspectionViolation.RealityObject.Address,
                        x.InspectionViolation.RealityObject.TypeOwnership
                    })
                .AsEnumerable()
                .Distinct()
                .ToList();
            if (violations.Any())
            {
                Report.RegData("Нарушения", violations.Select(x => new
                {
                    КодПин = x.ViolationCodePin,
                    ТекстНарушения = x.ViolationName,
                    Мероприятие = x.Action,
                    СрокУстранения = DateToString(x.DatePlanRemoval)
                }));

                Report["АдресПравонарушения"] = violations.First().Address;
                Report["КатЖилФонд"] = violations.First().TypeOwnership.Name;
            }

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

                Report["ИнспекторыИКоды"] = inspectors.AggregateWithSeparator(x => x.Fio + " - " + x.Position, ", ");

                if (prescription.Inspection.TypeBase == TypeBase.CitizenStatement)
                {
                    var listBaseStatementAppealCits =
                        Container.Resolve<IDomainService<InspectionAppealCits>>().GetAll()
                            .Where(x => x.Inspection.Id == prescription.Inspection.Id)
                                 .Select(x => x.AppealCits.DocumentNumber)
                                 .ToList();

                    Report["НомерОбращения"] = listBaseStatementAppealCits.Aggregate(string.Empty,
                                                                                    (total, next) => total == string.Empty ? next : total + ", " + next);
                }

                if (disposal != null && disposal.Inspection != null)
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

                        Report["ДомаИАдреса"] = city + strtmp;
                    }
                }
            }

            var instFioPositions = DocumentGjiInspectorDomainService.GetAll()
                                                 .Where(x => x.DocumentGji.Id == prescription.Id)
                                                 .Select(x => new { x.Inspector.FioAblative, x.Inspector.PositionAblative })
                                                 .ToList();

            Report["ИнспекторДолжностьТП"] = string.Join(", ", instFioPositions.Select(x => string.Format("{0} - {1}", x.FioAblative, x.PositionAblative)).ToArray());

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

            Report["ИнспекторДолжность"] = insp.AggregateWithSeparator(x => x.Fio + " - " + x.Position, ", ");

            if (insp.Length > 0)
            {
                var firstInsp = insp.First();
                Report["ДолжностьИнспектора"] = firstInsp.Position;
            }
        }

		protected virtual void SetTemplateCode(string code)
        {
            CodeTemplate = "BlockGJI_ExecutiveDocPrescription_2";

            if (new[] { "6", "7", "14" }.Contains(code))
            {
                CodeTemplate = "BlockGJI_ExecutiveDocPrescription";
            }
        }

		private string GetDocumentBase(object obj)
		{
			return GetPropertyValue(obj, "baseName") as string;
		}

		private object GetPropertyValue(object obj, string propName)
		{
			object value = null;
			if (obj != null)
			{
				var prop = obj.GetType().GetProperties().FirstOrDefault(e => e.Name == propName);
				if (prop != null)
				{
					value = prop.GetValue(obj, null);
				}
			}

			return value;
		}
    }
}