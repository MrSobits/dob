using Bars.Gkh.Utils;

namespace Bars.GkhGji.Regions.Stavropol.Report.DisposalGji
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Report;
    using Bars.GkhGji.Contracts;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Report;
    using Stimulsoft.Report;

    public class DisposalGjiStimulReport : GjiBaseStimulReport
    {
        public DisposalGjiStimulReport()
			: base(new ReportTemplateBinary(Properties.Resources.BlockGJI_Inspection_Stavropol))
        {
            ExportFormat = StiExportFormat.Word2007;
        }

        public override string Id
        {
            get { return "Disposal"; }
        }

        public override string CodeForm
        {
            get { return "Disposal"; }
        }

        public override string Name
        {
            get { return Container.Resolve<IDisposalText>().SubjectiveCase; }
        }

        public override string Description
        {
            get { return Container.Resolve<IDisposalText>().SubjectiveCase; }
        }

        public override string ReportGeneratorName
        {
            get { return "DocIoGenerator"; }
        }

        protected override string CodeTemplate { get; set; }

        private long DocumentId { get; set; }

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
                {
                    new TemplateInfo
                        {
                            Code = "BlockGJI_Inspection_Stavropol",
                            Name = "DisposalGJI",
                            Description = "Распоряжение исп. предп, проведение (вне)плановой проверки",
                            Template = Properties.Resources.BlockGJI_Inspection_Stavropol
                        }
                };
        }

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            DocumentId = userParamsValues.GetValue<object>("DocumentId").ToLong();
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            CodeTemplate = "BlockGJI_Inspection_Stavropol";

            var disposal = Container.Resolve<IDomainService<Disposal>>().Get(DocumentId);

            if (disposal == null)
            {
                var dispText = Container.Resolve<IDisposalText>();
                throw new ReportProviderException(string.Format("Не удалось получить {0}", dispText.SubjectiveCase.ToLower()));
            }

            var typeSurveys = Container.Resolve<IDomainService<DisposalTypeSurvey>>().GetAll()
                                       .Where(x => x.Disposal.Id == disposal.Id)
                                       .Select(x => x.TypeSurvey)
                                       .ToArray();

            if (disposal.KindCheck != null)
            {
                var hasExperts = Container.Resolve<IDomainService<DisposalExpert>>().GetAll().Any(x => x.Disposal.Id == disposal.Id);

                if (hasExperts)
                {
                    if (disposal.KindCheck.Code == TypeCheck.NotPlannedExit
                        || disposal.KindCheck.Code == TypeCheck.PlannedDocumentation
                        || disposal.KindCheck.Code == TypeCheck.NotPlannedDocumentation)
                    {
                        CodeTemplate += "-1";
                    }
                }

                var kindCheck = string.Empty;

                switch (disposal.KindCheck.Code)
                {
                    case TypeCheck.PlannedExit:
                        kindCheck = "плановой выездной";
                        break;
                    case TypeCheck.NotPlannedExit:
                    case TypeCheck.InspectionSurvey:
                        kindCheck = "внеплановой выездной";
                        break;
                    case TypeCheck.PlannedDocumentation:
                        kindCheck = "плановой документарной";
                        break;
                    case TypeCheck.NotPlannedDocumentation:
                        kindCheck = "внеплановой документарной";
                        break;
                    case TypeCheck.PlannedDocumentationExit:
                        kindCheck = "плановой документарной и выездной";
                        break;
                    case TypeCheck.VisualSurvey:
                        kindCheck = "о внеплановой проверке технического состояния жилого помещения";
                        break;
                    case TypeCheck.NotPlannedDocumentationExit:
                        kindCheck = "внеплановой документарной и выездной";
                        break;
                }

                Report["ВидПроверки"] = kindCheck;
                Report["ВидОбследования"] = disposal.KindCheck.Name.ToLower();
            }

            var cultureInfo = new CultureInfo("ru-RU");
            var dateFormat = "«dd» MMMM yyyy";

            // заполняем общие поля
            FillCommonFields(disposal);

            if (disposal.Inspection.Contragent != null)
            {
                Report["УправОрг"] = disposal.Inspection.Contragent.Name;
                Report["ОГРН"] = disposal.Inspection.Contragent.Ogrn;
                Report["УправОргРП"] = disposal.Inspection.Contragent.NameGenitive;
                Report["УправОргСокр"] = disposal.Inspection.Contragent.ShortName;
				Report["ИННУправОрг"] = disposal.Inspection.Contragent.Inn;

                if (disposal.Inspection.Contragent.FiasJuridicalAddress != null)
                {
                    var subStr = disposal.Inspection.Contragent.FiasJuridicalAddress.AddressName.Split(',');

                    var newAddr = new StringBuilder();

                    foreach (var rec in subStr)
                    {
                        if (newAddr.Length > 0)
                        {
                            newAddr.Append(',');
                        }

                        if (rec.Contains("р-н."))
                        {
                            var mu = rec.Replace("р-н.", string.Empty) + " район";
                            newAddr.Append(mu);
                            continue;
                        }

                        newAddr.Append(rec);
                    }

                    Report["АдресКонтрагента"] = newAddr.ToString();

                    var fiasAddr = disposal.Inspection.Contragent.FiasJuridicalAddress;

                    var addrExceptMu = new StringBuilder();

                    var addSeparator = false;

                    if (!string.IsNullOrEmpty(fiasAddr.PlaceName))
                    {
                        addrExceptMu.Append(fiasAddr.PlaceName);
                        addSeparator = true;
                    }

                    if (!string.IsNullOrEmpty(fiasAddr.StreetName))
                    {
                        addrExceptMu.Append(addSeparator ? ", " + fiasAddr.StreetName : fiasAddr.StreetName);
                    }

                    if (!string.IsNullOrEmpty(fiasAddr.House))
                    {
                        addrExceptMu.Append(addSeparator ? ", д. " + fiasAddr.House : "д. " + fiasAddr.House);
                    }

                    if (!string.IsNullOrEmpty(fiasAddr.Housing))
                    {
                        addrExceptMu.Append(addSeparator ? ", корп. " + fiasAddr.Housing : "корп. " + fiasAddr.Housing);
                    }

                    if (!string.IsNullOrEmpty(fiasAddr.Building))
                    {
                        addrExceptMu.Append(addSeparator ? ", секц. " + fiasAddr.Building : "секц. " + fiasAddr.Building);
                    }

                    if (!string.IsNullOrEmpty(fiasAddr.Flat))
                    {
                        addrExceptMu.Append(addSeparator ? ", кв." + fiasAddr.Flat : "кв." + fiasAddr.Flat);
                    }

                    Report["АдресОтносительноМО"] = addrExceptMu.ToString();
                }
                else
                {
                    Report["АдресКонтрагента"] = disposal.Inspection.Contragent.AddressOutsideSubject;
                }

				Report["АдресКонтрагентаФакт"] = disposal.Inspection.Contragent.FactAddress;
            }

            Report["ДатаРаспоряжения"] = disposal.DocumentDate.HasValue
                            ? disposal.DocumentDate.Value.ToString("D", cultureInfo)
                            : string.Empty;

            Report["НомерРаспоряжения"] = disposal.DocumentNumber;

            Report["НачалоПериодаВыезд"] = disposal.ObjectVisitStart.HasValue
                                                                        ? disposal.ObjectVisitStart.Value.ToString(dateFormat, cultureInfo)
                                                                        : string.Empty;

            Report["ОкончаниеПериодаВыезд"] = disposal.ObjectVisitEnd.HasValue
                                                                        ? disposal.ObjectVisitEnd.Value.ToString(dateFormat, cultureInfo)
                                                                        : string.Empty;

            Report["НачалоПериода"] = disposal.DateStart.HasValue
                                                                        ? disposal.DateStart.Value.ToString(dateFormat, cultureInfo)
                                                                        : string.Empty;

            Report["ОкончаниеПериода"] = disposal.DateEnd.HasValue
                                                                        ? disposal.DateEnd.Value.ToString(dateFormat, cultureInfo)
                                                                        : string.Empty;


            var attachDomain = Container.ResolveDomain<DisposalProvidedDoc>();
            using (Container.Using(attachDomain))
            {
                var docs = attachDomain.GetAll().Where(x => x.Disposal.Id == disposal.Id).Select(x => x.ProvidedDoc.Name).ToList();
                var docsStr = new StringBuilder();
                foreach (var doc in docs)
                {
                    docsStr.Append("-").Append(doc).Append("\\");
                }

                Report["ПредоставляемыеДокументы"] = docsStr.ToString();
            }


            var inspectors = Container.Resolve<IDomainService<DocumentGjiInspector>>().GetAll()
                                      .Where(x => x.DocumentGji.Id == disposal.Id)
                                      .Select(x => x.Inspector)
                                      .ToList();

            if (inspectors.Any())
            {
                var inspectorsAndCodes = inspectors
                    .Aggregate(string.Empty, (x, y) => x + (string.IsNullOrEmpty(x)
                                                      ? string.Format("{0} - {1}", y.Fio, y.Position)
                                                      : string.Format(", {0} - {1}", y.Fio, y.Position)));

                var inspectorsAndCodesDative = inspectors
                    .Aggregate(string.Empty, (x, y) => x + (string.IsNullOrEmpty(x)
                                                      ? string.Format("{0} - {1}", y.FioDative, y.PositionDative)
                                                      : string.Format(", {0} - {1}", y.FioDative, y.PositionDative)));

                var inspectorsAndCodesAccusative = inspectors
                    .Aggregate(string.Empty, (x, y) => x + (string.IsNullOrEmpty(x)
                                                      ? string.Format("{0} - {1}", y.FioAccusative, y.PositionAccusative)
                                                      : string.Format(", {0} - {1}", y.FioAccusative, y.PositionAccusative)));

                Report["ИнспекторыИКоды"] = inspectorsAndCodes;
                Report["ИнспекторыИКодыДатП"] = inspectorsAndCodesDative;
                Report["ИнспекторыИКодыВинП"] = inspectorsAndCodesAccusative;

                var firstInspector = inspectors.FirstOrDefault();
                if (firstInspector != null)
                {
                    Report["ТелефонИнспектора"] = firstInspector.Phone;
                    var operatorDomain = Container.ResolveDomain<Operator>();
                    using (Container.Using(operatorDomain))
                    {
                        var inspId = firstInspector.Id;
	                    var op = operatorDomain.GetAll().FirstOrDefault(x => x.Inspector.Id == inspId && x.User.Email != null && x.User.Email != "");
						Report["EmailИнспектора"] = op != null ? op.User.Email : string.Empty;
                    }
                }

                // формируем следующую строку: дефис пробел должность в рп фио в рп
                var strBuilder = new StringBuilder();

                var currRow = new StringBuilder();

                foreach (var insp in inspectors)
                {
                    currRow.Append("- ");

                    if (!string.IsNullOrEmpty(insp.PositionGenitive))
                    {
                        currRow.Append(insp.PositionGenitive);
                    }

                    if (!string.IsNullOrEmpty(insp.FioGenitive))
                    {
                        currRow.Append(" ");
                        currRow.Append(insp.FioGenitive);
                    }

                    // чтобы не выводить строки типа "- " проверяем длину
                    if (currRow.ToString().Length > 2)
                    {
                        if (strBuilder.Length > 0)
                        {
                            strBuilder.Append(";\n");
                        }

                        strBuilder.Append(currRow);
                    }

                    currRow.Clear();
                }

                if (strBuilder.Length > 0)
                {
                    strBuilder.Append(".");
                }

                Report["ДолжностьФИОРП"] = strBuilder.ToString();
            }

            if (disposal.IssuedDisposal != null)
            {
                Report["КодРуководителя"] = disposal.IssuedDisposal.Position;

                Report["КодРуководителяФИО(сИнициалами)"] = string.Format(
                    "{0} {1}",
                    disposal.IssuedDisposal.Position,
                    string.IsNullOrEmpty(disposal.IssuedDisposal.ShortFio) ? disposal.IssuedDisposal.Fio : disposal.IssuedDisposal.ShortFio);

                Report["КодНачальника(ВинП)"] = disposal.IssuedDisposal.PositionAccusative;
                Report["Начальник(ВинП)"] = disposal.IssuedDisposal.FioAccusative;

                Report["РуководительДолжность"] = disposal.IssuedDisposal.Position;
                Report["РуководительФИОСокр"] = disposal.IssuedDisposal.ShortFio;
            }

            var realityObjs = Container.Resolve<IDomainService<InspectionGjiRealityObject>>().GetAll()
                                       .Where(x => x.Inspection.Id == disposal.Inspection.Id)
                                       .Select(x => x.RealityObject)
                                       .ToList();

            if (realityObjs.Count == 1)
            {
                var firstObject = realityObjs.FirstOrDefault();
				if (firstObject.FiasAddress != null)
                {
                    Report["ДомАдрес"] = firstObject.FiasAddress.PlaceName + ", " + firstObject.FiasAddress.StreetName;
                    Report["НомерДома"] = firstObject.FiasAddress.House;
                    Report["АдресДома"] = string.Format(
                        "{0}, {1}, д.{2}",
                        firstObject.FiasAddress.PlaceName,
                        firstObject.FiasAddress.StreetName,
                        firstObject.FiasAddress.House);
                    Report["ДомАдрес1"] = firstObject.FiasAddress.StreetName + ", " + firstObject.FiasAddress.PlaceName;
                }

				var zonalInspection = Container.Resolve<IDomainService<ZonalInspectionMunicipality>>().GetAll()
					.Where(x => x.Municipality.Id == firstObject.Municipality.Id)
					.Select(x => x.ZonalInspection)
					.FirstOrDefault();

	            if (zonalInspection != null)
	            {
					Report["ЗональноеНаименование1ГосЯзык1"] = zonalInspection.ZoneName;
	            }
            }

            var realObjs = new StringBuilder();
            if (realityObjs.Count > 0)
            {
                realObjs.AppendFormat("{0}, ", realityObjs.FirstOrDefault().FiasAddress.PlaceName);
                foreach (var realityObject in realityObjs)
                {
                    if (realObjs.Length > 0)
                        realObjs.Append("; ");

                    realObjs.AppendFormat("{0}, д.{1}", realityObject.FiasAddress.StreetName, realityObject.FiasAddress.House);
                }

                Report["ДомаИАдреса"] = realObjs.ToString();
            }

            if (disposal.TypeDisposal == TypeDisposalGji.DocumentGji)
            {
                var servDocChildren = Container.Resolve<IDomainService<DocumentGjiChildren>>();

                var firstPrescription = servDocChildren
                        .GetAll()
                        .Where(x => x.Children.Id == disposal.Id && x.Parent.TypeDocumentGji == TypeDocumentGji.Prescription)
                        .Select(x => x.Parent)
                        .FirstOrDefault();

                if (firstPrescription != null)
                {
                    Report["НомерПредписания"] = string.Format("№{0}", firstPrescription.DocumentNumber);
                    Report["ДатаПредписания"] = firstPrescription.DocumentDate.HasValue
                                                                             ? firstPrescription.DocumentDate.Value.ToShortDateString()
                                                                             : string.Empty;
                }

                var prescriptions = servDocChildren.GetAll()
                             .Where(x => x.Children.Id == disposal.Id && x.Parent.TypeDocumentGji == TypeDocumentGji.Prescription)
                             .Select(x => new { x.Parent.DocumentDate, x.Parent.DocumentNumber, x.Parent.DocumentSubNum })
                             .OrderBy(x => x.DocumentDate)
                             .AsEnumerable()
                             .GroupBy(x => x.DocumentDate)
                             .ToDictionary(
                                x => x.Key,
                                z => z.Select(x => new { x.DocumentNumber, x.DocumentSubNum })
                                    .Aggregate(string.Empty, (x, y) =>
                                {
                                    if (!string.IsNullOrEmpty(x))
                                        x += ", ";

                                    if (!string.IsNullOrEmpty(y.DocumentNumber))
                                    {
                                        x += y.DocumentNumber;
                                        if (y.DocumentSubNum.HasValue)
                                            x += "/" + y.DocumentSubNum.Value;
                                    }

                                    return x;
                                }));

                Report["Предписания"] = prescriptions.Aggregate(
                    string.Empty,
                    (x, y) =>
                    {
                        if (!string.IsNullOrEmpty(x))
                        {
                            x += string.Format(", {0} от {1}", y.Value, y.Key.HasValue ? y.Key.Value.ToShortDateString() : string.Empty);
                        }
                        else
                        {
                            x += string.Format("{0}{1} от {2}", prescriptions.Count > 1 ? "предписаний №№" : "предписания №", y.Value, y.Key.HasValue ? y.Key.Value.ToShortDateString() : string.Empty);
                        }

                        return x;
                    });


                var prescriptionRealObjs = Container.Resolve<IDomainService<PrescriptionViol>>().GetAll()
                        .Where(x => x.Document.Id == firstPrescription.Id && x.InspectionViolation.RealityObject != null)
                        .Select(x => x.InspectionViolation.RealityObject)
                        .Distinct()
                        .ToList();

                if (prescriptionRealObjs.Count > 0)
                {
                    var prescripRealObjs = new StringBuilder();
                    var prescripRealObjNums = new StringBuilder();

                    foreach (var prescriptionRealObj in prescriptionRealObjs)
                    {
                        if (prescripRealObjNums.Length > 0)
                            prescripRealObjNums.Append("; ");

                        if (prescripRealObjs.Length > 0)
                            prescripRealObjs.Append("; ");

                        prescripRealObjs.AppendFormat(prescriptionRealObj.Address);
                        prescripRealObjNums.Append(prescriptionRealObj.FiasAddress.House);
                    }

                    Report["ДомАдресПВП"] = prescripRealObjs.ToString();
                    Report["НомерДомаПВП"] = prescripRealObjNums.ToString();
                }
            }

	        var prescription = GetChildDocument(disposal, TypeDocumentGji.Prescription);
	        if (prescription != null)
	        {
		        var prescriptionViolations = Container.Resolve<IDomainService<PrescriptionViol>>().GetAll()
			        .Where(x => x.Document.Id == prescription.Id).ToList();

		        Report["ПредметПроверки"] = prescriptionViolations
			        .Where(x => x.InspectionViolation != null && x.InspectionViolation.Violation != null)
			        .Select(x => x.InspectionViolation.Violation.Name)
			        .AggregateWithSeparator(", ");
	        }

	        if (disposal.ResponsibleExecution != null)
            {
                Report["Ответственный"] = disposal.ResponsibleExecution.Fio;
                Report["ДолжностьОтветственныйРп"] = disposal.ResponsibleExecution.PositionGenitive;

                // именно в винительном падеже
                Report["ФИООтветственныйСокрРп"] = disposal.ResponsibleExecution.FioAccusative;
            }

            Report["ФИООбр"] = "ФИО";

            Report["АдресОбр"] = "Адрес";

            // если основание проверки - требование прокуратуры
            if (disposal.Inspection.TypeBase == TypeBase.ProsecutorsClaim)
            {
                var prosClaim = Container.Resolve<IDomainService<BaseProsClaim>>().Load(disposal.Inspection.Id);

                Report["№Требования"] = prosClaim.DocumentNumber;
                Report["Дата"] = prosClaim.ProsClaimDateCheck.ToDateTime().ToShortDateString();
                Report["ДЛТребование"] = prosClaim.IssuedClaim;

                Report["Требование"] = string.Format("{0} от {1}",
                                                                                prosClaim.DocumentNumber,
                                                                                prosClaim.DocumentDate.HasValue
                                                                                    ? prosClaim.DocumentDate.Value.ToShortDateString()
                                                                                    : string.Empty);
            }

            // если основание проверки - поручение руководства
            if (disposal.Inspection.TypeBase == TypeBase.DisposalHead)
            {
                Report["ПравовоеОснованиеПроверки"] =
                    "на основании ст.20 Жилищного кодекса РФ, приказа начальника ГЖИ РТ № $НомерПриказа$ от $ДатаПриказа$";

                var dispHead = Container.Resolve<IDomainService<BaseDispHead>>().Load(disposal.Inspection.Id);

                Report["НомерПриказа"] = dispHead.InspectionNum;
                Report["ДатаПриказа"] = dispHead.DispHeadDate.HasValue
                                                                     ? dispHead.DispHeadDate.Value.ToString("D", cultureInfo)
                                                                     : string.Empty;
            }

            if (disposal.Inspection.TypeBase == TypeBase.CitizenStatement)
            {
                var citState = Container.Resolve<IDomainService<BaseStatement>>().Load(disposal.Inspection.Id);

                switch (citState.PersonInspection)
                {
                    case PersonInspection.Organization:
                        Report["Лицо"] = "юридического лица";
                        break;
                    case PersonInspection.PhysPerson:
                        Report["Лицо"] = "физического лица";
                        break;
                }

                var appeals =
                    Container.Resolve<IDomainService<InspectionAppealCits>>().GetAll()
                        .Where(x => x.Inspection.Id == disposal.Inspection.Id)
                        .Select(x => new
                            {
                                x.Id,
                                x.AppealCits.Correspondent,
                                x.AppealCits.CorrespondentAddress,
                                x.AppealCits.NumberGji,
                                x.AppealCits.DateFrom,
                                x.AppealCits.TypeCorrespondent
                            })
                        .ToList();

                if (appeals.Count > 0)
                {
                    var firstCorrespondent = appeals.Select(x => x.TypeCorrespondent).FirstOrDefault();

                    switch (firstCorrespondent)
                    {
                        case TypeCorrespondent.CitizenHe:
                            Report["СклонениеГражданин"] = "гражданина";
                            Report["СклонениеОбращение"] = "обратившегося";
                            Report["СклонениеПроживание"] = "проживающего";
                            break;
                        case TypeCorrespondent.CitizenShe:
                            Report["СклонениеГражданин"] = "гражданки";
                            Report["СклонениеОбращение"] = "обратившейся";
                            Report["СклонениеПроживание"] = "проживающей";
                            break;
                        case TypeCorrespondent.CitizenThey:
                            Report["СклонениеГражданин"] = "граждан";
                            Report["СклонениеОбращение"] = "обратившихся";
                            Report["СклонениеПроживание"] = "проживающих";
                            break;
                    }

                    var fioCorr = new StringBuilder();
                    var addrCorr = new StringBuilder();
                    var appealsNumDate = new StringBuilder();
                    var appealNumGji = new StringBuilder();

                    foreach (var appeal in appeals)
                    {
                        if (!string.IsNullOrEmpty(appeal.Correspondent))
                        {
                            if (fioCorr.Length > 0)
                                fioCorr.Append(", ");

                            fioCorr.Append(appeal.Correspondent);
                        }

                        if (!string.IsNullOrEmpty(appeal.CorrespondentAddress))
                        {
                            if (addrCorr.Length > 0)
                                addrCorr.Append(", ");

                            addrCorr.Append(appeal.CorrespondentAddress);
                        }

                        if (!string.IsNullOrEmpty(appeal.NumberGji))
                        {
                            if (appealsNumDate.Length > 0)
                                appealsNumDate.Append(", ");

                            appealsNumDate.AppendFormat("{0} от {1}",
                                                             appeal.NumberGji,
                                                             appeal.DateFrom.HasValue
                                                                 ? appeal.DateFrom.Value.ToShortDateString()
                                                                 : string.Empty);

                            if (appealNumGji.Length > 0)
                                appealNumGji.Append(", ");

                            appealNumGji.Append(appeal.NumberGji);
                        }
                    }

                    Report["ФИООбр"] = fioCorr.ToString();
                    Report["АдресОбр"] = addrCorr.ToString();
                    Report["Обращения"] = appealsNumDate.ToString();
                    Report["НомерГЖИ"] = appealNumGji.ToString();

                    var appealIds = appeals.Select(x => x.Id).ToList();

                    var sources = Container.Resolve<IDomainService<AppealCitsSource>>().GetAll()
                        .Where(x => appealIds.Contains(x.AppealCits.Id))
                        .Select(x => x.RevenueSource.Name)
                        .Distinct()
                        .ToList();

                    var appealSources = new StringBuilder();
                    foreach (var source in sources.Where(x => !string.IsNullOrEmpty(x)))
                    {
                        if (appealSources.Length > 0)
                            appealSources.Append(", ");

                        appealSources.Append(source);
                    }

                    Report["ИсточникОбращения"] = appealSources.ToString();
                }
            }

            Report["Эксперты"] =
                Container.Resolve<IDomainService<DisposalExpert>>().GetAll()
                         .Where(x => x.Disposal.Id == disposal.Id)
                         .Select(x => x.Expert)
                         .Aggregate(string.Empty, (x, y) => x + (string.IsNullOrEmpty(x) ? y.Name : ", " + y.Name));

            var typeSurveyIds = typeSurveys.Select(x => x.Id).ToArray();

            if (typeSurveyIds.Any())
            {
                Report["Цель"] =
                    Container.Resolve<IDomainService<TypeSurveyGoalInspGji>>().GetAll()
                             .Where(x => typeSurveyIds.Contains(x.TypeSurvey.Id))
                             .Select(x => x.SurveyPurpose)
                             .Aggregate(string.Empty, (x, y) => x + (string.IsNullOrEmpty(x) ? y.Name : ", " + y.Name));

                Report["Задача"] =
                    Container.Resolve<IDomainService<TypeSurveyTaskInspGji>>().GetAll()
                             .Where(x => typeSurveyIds.Contains(x.TypeSurvey.Id))
                             .Select(x => x.SurveyObjective)
                             .Aggregate(string.Empty, (x, y) => x + (string.IsNullOrEmpty(x) ? y.Name : ", " + y.Name));

                var inspFounds =
                    Container.Resolve<IDomainService<TypeSurveyInspFoundationGji>>().GetAll()
                        .Where(x => typeSurveyIds.Contains(x.TypeSurvey.Id))
                        .Select(x => x.NormativeDoc.Name)
                        .Distinct()
                        .AsEnumerable();

                var inspFoundation = new StringBuilder();

                foreach (var foundation in inspFounds.Where(x => !string.IsNullOrEmpty(x)))
                {
                    if (inspFoundation.Length > 0)
                        inspFoundation.Append(";\n- ");

                    inspFoundation.Append(foundation);
                }

                if (inspFoundation.Length > 0)
                    inspFoundation.Append(".");

                Report["ПравовоеОснование"] = inspFoundation.ToString();
            }
        }
    }
}