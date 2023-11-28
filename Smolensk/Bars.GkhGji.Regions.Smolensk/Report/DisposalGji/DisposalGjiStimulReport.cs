namespace Bars.GkhGji.Regions.Smolensk.Report.DisposalGji
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Bars.B4;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    using System.Collections.Generic;
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.GkhGji.Contracts;
    using Bars.Gkh.Report;
    using Bars.GkhGji.Regions.Smolensk.Entities;
    using Bars.GkhGji.Regions.Smolensk.Entities.Disposal;
    using Bars.GkhGji.Regions.Smolensk.Enums;

    using Stimulsoft.Report;

    class DisposalGjiStimulReport : GjiBaseStimulReport
    {
        public DisposalGjiStimulReport()
            : base(new ReportTemplateBinary(Properties.Resources.Disposal))
        {
        }

        private long DocumentId { get; set; }
        private DisposalSmol disposal { get; set; }

        public override string Id
        {
            get { return "Disposal"; }
        }

        public override string CodeForm
        {
            get { return "Disposal"; }
        }

        public override StiExportFormat ExportFormat
        {
            get { return StiExportFormat.Word2007; }
        }
        public override string Name
        {
            get { return Container.Resolve<IDisposalText>().SubjectiveCase; }
        }

        public override string Description
        {
            get { return Container.Resolve<IDisposalText>().SubjectiveCase; }
        }

        protected override string CodeTemplate { get; set; }

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            DocumentId = userParamsValues.GetValue<object>("DocumentId").ToLong();

            var disposalDomain = Container.Resolve<IDomainService<DisposalSmol>>();
            try
            {
                disposal = disposalDomain.GetAll().FirstOrDefault(x => x.Id == DocumentId);
            }
            finally 
            {
                Container.Release(disposalDomain);
            }

        }

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
                {
                    new TemplateInfo
                        {
                            Code = "SmolenskDisposal",
                            Name = "DisposalGJI",
                            Description = "Приказ на проверку юр. лиц",
                            Template = Properties.Resources.Disposal
                        },
                    new TemplateInfo
                        {
                            Code = "SmolenskDisposal_fiz",
                            Name = "SmolenskDisposal_fiz",
                            Description = "Приказ на проверку физ. лиц и дл. лиц",
                            Template = Properties.Resources.Disposal
                        }
                };
        }

        public override Stream GetTemplate()
        {
            this.GetCodeTemplate();
            return base.GetTemplate();
        }

        private void GetCodeTemplate()
        {
            // По умолчанию печатаем как для Физ лиц
            CodeTemplate = "SmolenskDisposal_fiz";

            if (disposal.Inspection.PersonInspection == PersonInspection.Organization)
            {
                // Если тип проверки для Организаций то меняем шаблон
                CodeTemplate = "SmolenskDisposal";
            }
        }

        public override void PrepareReport(ReportParams reportParams)
        {

            var dispText = Container.Resolve<IDisposalText>();
            var dispTypeSurveyDoamin = Container.Resolve<IDomainService<DisposalTypeSurvey>>();
            var dispInspectorsDomain = Container.Resolve<IDomainService<DocumentGjiInspector>>();
            var operatorDomain = Container.Resolve<IDomainService<Operator>>();
            var inspRoDomain = Container.Resolve<IDomainService<InspectionGjiRealityObject>>();
            var disExpertDomain = Container.Resolve<IDomainService<DisposalExpert>>();
            var typeSurveyGoalDomain = Container.Resolve<IDomainService<TypeSurveyGoalInspGji>>();
            var typeSurveyTaskDomain = Container.Resolve<IDomainService<TypeSurveyTaskInspGji>>();
            var typeSurveyInspFoundDomain = Container.Resolve<IDomainService<TypeSurveyInspFoundationGji>>();
            var dispProvDocDomain = Container.Resolve<IDomainService<DisposalProvidedDoc>>();

            try
            {
                if (disposal == null)
                {
                    throw new ReportProviderException(string.Format("Не удалось получить {0}", dispText.SubjectiveCase.ToLower()));
                }
                FillCommonFields(disposal);

                var typeSurveys = dispTypeSurveyDoamin.GetAll()
                                           .Where(x => x.Disposal.Id == disposal.Id)
                                           .Select(x => x.TypeSurvey)
                                           .ToArray();

                if (disposal.KindCheck != null)
                {
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
                var dateFormat = "«dd» MMMM yyyy г.";

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

                    }
                    else
                    {
                        Report["АдресКонтрагента"] = disposal.Inspection.Contragent.AddressOutsideSubject;
                    }

                    Report["АдресКонтрагентаФакт"] = disposal.Inspection.Contragent.FactAddress;
                }

                Report["ДатаРаспоряжения"] = disposal.DocumentDate.HasValue
                                ? disposal.DocumentDate.Value.ToString(dateFormat)
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

                Report["Цель"] = disposal.VerificationPurpose;    

                var inspectors = dispInspectorsDomain.GetAll()
                                          .Where(x => x.DocumentGji.Id == disposal.Id)
                                          .Select(x => x.Inspector)
                                          .ToList();

                if (inspectors.Any())
                {
                    var inspectorsAndCodes = inspectors.Any() ? inspectors
                        .Aggregate(string.Empty, (x, y) => x + (string.IsNullOrEmpty(x)
                                                          ? string.Format("{0} - {1}", y.Fio, y.Position)
                                                          : string.Format(", {0} - {1}", y.Fio, y.Position)))
                                                          : string.Empty;

                    Report["ИнспекторыИКоды"] = inspectorsAndCodes;

                    //var firstInspector = inspectors.FirstOrDefault();
                    //if (firstInspector != null)
                    //{
                    //    Report["ТелефонИнспектора"] = firstInspector.Phone;
                    //}

                    // формируем следующую строку: дефис пробел должность в рп фио в рп
                    var strBuilder = new StringBuilder();

                    var currRow = new StringBuilder();

                    foreach (var insp in inspectors)
                    {
                        // чтобы не выводить строки типа "- " проверяем длину
                        if (currRow.ToString().Length > 2)
                        {
                            if (strBuilder.Length > 0)
                            {
                                strBuilder.Append(", ");
                            }
                        }

                        if (!string.IsNullOrEmpty(insp.FioAccusative))
                        {
                            currRow.Append(insp.FioAccusative);
                        }

                        if (!string.IsNullOrEmpty(insp.PositionAccusative))
                        {
                            currRow.Append(" ");
                            currRow.Append(insp.PositionAccusative);
                        }

                        strBuilder.Append(currRow);

                        currRow.Clear();
                    }

                    if (strBuilder.Length > 0)
                    {
                        strBuilder.Append(".");
                    }

                    Report["ДолжностьФИОРП"] = strBuilder.ToString();

                    var listInspectorId = inspectors.Select(x => x.Id).ToList();

                    var listEmails = operatorDomain.GetAll()
                            .Where(x => listInspectorId.Contains(x.Inspector.Id))
                            .ToList()
                            .Select(x => x.Return(z => z.User).Return(y => y.Email));

                    Report["EmailИнспектора"] = string.Join("; ", listEmails);
                }

                if (disposal.IssuedDisposal != null)
                {
                    Report["КодРуководителя"] = disposal.IssuedDisposal.Position;

                    Report["РуководительДолжность"] = disposal.IssuedDisposal.Position;
                    Report["РуководительФИОСокр"] = disposal.IssuedDisposal.ShortFio;
                }

                var realityObjs = inspRoDomain.GetAll()
                                           .Where(x => x.Inspection.Id == disposal.Inspection.Id)
                                           .Select(x => x.RealityObject)
                                           .ToList();

                if (realityObjs.Count == 1)
                {
                    var firstObject = realityObjs.FirstOrDefault();
                    if (firstObject.FiasAddress != null)
                    {
                        Report["ДомАдрес"] = firstObject.FiasAddress.PlaceName + ", "
                                             + firstObject.FiasAddress.StreetName;
                        Report["НомерДома"] = firstObject.FiasAddress.House;
                        Report["АдресДома"] = string.Format(
                            "{0}, {1}, д.{2}",
                            firstObject.FiasAddress.PlaceName,
                            firstObject.FiasAddress.StreetName,
                            firstObject.FiasAddress.House);
                    }
                }

                var realObjs = new StringBuilder();
                if (realityObjs.Count > 0)
                {
                    foreach (var realityObject in realityObjs)
                    {
                        if (realObjs.Length > 0)
                            realObjs.Append("; ");

                        var housing = string.Empty;
                        if (!string.IsNullOrEmpty(realityObject.FiasAddress.Housing))
                        {
                            housing = string.Format(", корп. {0}", realityObject.FiasAddress.Housing);
                        }

                        realObjs.AppendFormat("{0}, д.{1}{2}", realityObject.FiasAddress.StreetName, realityObject.FiasAddress.House, housing);
                    }

                    Report["ДомаИАдреса"] = string.Format("{0}, ", realityObjs.FirstOrDefault().FiasAddress.PlaceName) + realObjs.ToString();

                }

                if (disposal.ResponsibleExecution != null)
                {
                    Report["Ответственный"] = disposal.ResponsibleExecution.Fio;
                    Report["ДолжностьОтветственныйРП"] = disposal.ResponsibleExecution.PositionGenitive;

                    // именно в винительном падеже
                    Report["ФИООтветственныйСокрРП"] = disposal.ResponsibleExecution.FioAccusative;

                    Report["ДолжностьИнспектора"] = disposal.ResponsibleExecution.Position;

                    Report["ТелефонИнспектора"] = disposal.ResponsibleExecution.Phone;
                }

                var dataExperts = disExpertDomain.GetAll()
                             .Where(x => x.Disposal.Id == disposal.Id)
                             .Select(x => x.Expert)
                             .ToList();

                if (dataExperts.Any())
                {
                    Report["Эксперты"] = dataExperts.Aggregate(string.Empty, (x, y) => x + (string.IsNullOrEmpty(x) ? y.Name : ", " + y.Name));
                }
                             
                var typeSurveyIds = typeSurveys.Select(x => x.Id).ToArray();

                if (typeSurveyIds.Any())
                {
                    var tasks =
                        typeSurveyTaskDomain.GetAll().Where(x => typeSurveyIds.Contains(x.TypeSurvey.Id)).ToList();

                    if (tasks.Any())
                    {
                        Report["Задача"] = tasks.Aggregate(string.Empty, (x, y) => x + (string.IsNullOrEmpty(x) ? y.SurveyObjective.Name : ", " + y.SurveyObjective.Name));    
                    }
                    

                    var inspFounds = typeSurveyInspFoundDomain.GetAll()
                            .Where(x => typeSurveyIds.Contains(x.TypeSurvey.Id))
                            .Select(x => new { x.NormativeDoc.Name, x.Code })
                            .OrderBy(x => x.Code)
                            .ToArray()
                            .Select(x => x.Name)
                            .Distinct()
                            .ToList();

                    Report.RegData("СекцияПравовоеОснование", inspFounds.Where(x => !string.IsNullOrEmpty(x)).Select(x => new { ПравовоеОснование = x }));
                }



                var queryTypeSurveyIds = dispTypeSurveyDoamin.GetAll()
                                                    .Where(x => x.Disposal.Id == disposal.Id)
                                                    .Select(x => x.TypeSurvey.Id);

                var strGoalInspGji = typeSurveyGoalDomain
                             .GetAll()
                             .Where(x => queryTypeSurveyIds.Contains(x.TypeSurvey.Id))
                             .Select(x => x.SurveyPurpose.Name)
                             .ToList()
                             .Aggregate(string.Empty, (t, n) => t + n + ";");

                if (strGoalInspGji.Length > 0)
                {
                    Report["ЦельОбследования"] = strGoalInspGji.Substring(0, strGoalInspGji.Length - 1);
                }
                else
                {
                    Report["ЦельОбследования"] = string.Empty;
                }


                var number = 1;
                var listDisposalProvidedDoc = dispProvDocDomain
                             .GetAll()
                             .Where(x => x.Disposal.Id == disposal.Id)
                             .Select(x => x.ProvidedDoc.Name)
                             .AsEnumerable()
                             .Select(x =>
                             {
                                 return "{0}) {1}".FormatUsing(number++, x);
                             })
                             .ToList();

                Report.RegData("СекцияПредоставляемыеДокументы", listDisposalProvidedDoc.Select(x => new { ПредоставляемыеДокументы = x }));
                
                FillRegionParams(reportParams, disposal);

            }
            finally
            {
                Container.Release(dispText);
                Container.Release(dispTypeSurveyDoamin);
                Container.Release(dispInspectorsDomain);
                Container.Release(operatorDomain);
                Container.Release(inspRoDomain);
                Container.Release(disExpertDomain);
                Container.Release(typeSurveyGoalDomain);
                Container.Release(typeSurveyTaskDomain);
                Container.Release(typeSurveyInspFoundDomain);
                Container.Release(dispProvDocDomain);
            }
        }

        protected void FillRegionParams(ReportParams reportParams, DocumentGji doc)
        {
            //var displosalProvidedDocDateDomain = Container.Resolve<IDomainService<DisposalProvidedDocDate>>();
            var disposalProvidedDocDomain = Container.Resolve<IDomainService<DisposalProvidedDoc>>();
            var disposalExpertDomain = Container.Resolve<IDomainService<DisposalExpert>>();
            var disposalTypeSurveyDomain = Container.Resolve<IDomainService<DisposalTypeSurvey>>();
            var disposalSurveySubjectDomain = Container.Resolve<IDomainService<DisposalSurveySubject>>();
            var typeSurveyInspFoundationGjiDomain = Container.Resolve<IDomainService<TypeSurveyInspFoundationGji>>();
            var inspectionGjiDomain = Container.Resolve<IDomainService<InspectionGji>>();
            var inspectionAppealCitsDomain = Container.Resolve<IDomainService<InspectionAppealCits>>();
            var baseJurPersonDomain = Container.Resolve<IDomainService<BaseJurPerson>>();
            var appealCitsSourceDomain = Container.Resolve<IDomainService<AppealCitsSource>>();
            var typeSurveyGjiIssueDomain = Container.Resolve<IDomainService<TypeSurveyGoalInspGji>>();
            var servDocChildren = Container.Resolve<IDomainService<DocumentGjiChildren>>();
            var dispControlMeasureDomain = Container.Resolve<IDomainService<DisposalControlMeasures>>();

            try
            {
                var disposal = (Disposal)doc;

                //var str = string.Empty;
                //var dateProvideDocuments =
                //    displosalProvidedDocDateDomain.GetAll().FirstOrDefault(x => x.Disposal.Id == disposal.Id);

                //if (disposal.KindCheck.Code == TypeCheck.NotPlannedDocumentation)
                //{
                //    str =
                //        string.Format(
                //            "В срок не позднее 10 (десяти) рабочих дней со дня получения настоящего распоряжения {0} предоставить"
                //                        + "в Департамент копии следующих документов, заверенные надлежащим образом:",
                //                        Report["СокрУправОрг"]);
                //}
                //else if (disposal.KindCheck.Code == TypeCheck.NotPlannedExit)
                //{
                //    str =
                //        string.Format(
                //            "В срок до {0} предоставить в Департамент следующие документы, заверенные"
                //                        + " надлежащим образом:",
                //                        dateProvideDocuments != null
                //                            ? dateProvideDocuments.DateTimeDocument.Value.ToShortDateString()
                //                            : string.Empty);
                //}

                var listDisposalProvidedDoc =
                    disposalProvidedDocDomain.GetAll()
                        .Where(x => x.Disposal.Id == disposal.Id)
                        .Select(x => new { x.ProvidedDoc.Name, x.Description })
                        .ToList();

                //var strList = listDisposalProvidedDoc.Select(x => str).ToList();

                //Report.RegData("СекцияРаздел11", strList.Select(x => new
                //{
                //    ТекстПередДокументами = x
                //}).ToList());

                //var section11 = reportParams.ComplexReportParams.ДобавитьСекцию("СекцияРаздел11");


                Report.RegData("СекцияДокументы", listDisposalProvidedDoc.Select(x =>
                    new
                    {
                        ПредоставляемыйДокумент = !string.IsNullOrEmpty(x.Description) ? x.Description : x.Name
                    }));

                //if (listDisposalProvidedDoc.Any())
                //{
                //    //section11.ДобавитьСтроку();
                //    //section11["ТекстПередДокументами"] = str;

                //    //var sectionDocuments = section11.ДобавитьСекцию("СекцияДокументы");

                //    //foreach (var item in listDisposalProvidedDoc)
                //    //{
                //    //    sectionDocuments.ДобавитьСтроку();
                //    //    sectionDocuments["ПредоставляемыйДокумент"] = !string.IsNullOrEmpty(item.Description) ? item.Description : item.Name;
                //    //}
                //}

                // const string durationInspections = "2 рабочих дней";

                //Report["ПродолжительностьПроверки"] = durationInspections;

                //var strControlActivities = string.Empty;

                //if (disposal.KindCheck.Code == TypeCheck.NotPlannedExit)
                //{
                //    strControlActivities =
                //        string.Format(
                //            "провести визуальный осмотр дома по адресу: " + "{0} (с {1} по {2} - {3}.)",
                //                                         Report["ДомаИАдреса"],
                //                                         Report["НачалоПериода"],
                //                                         Report["ОкончаниеПериода"],
                //                                         durationInspections);
                //}
                //else if (disposal.KindCheck.Code == TypeCheck.NotPlannedDocumentation)
                //{

                //    strControlActivities =
                //        string.Format(
                //            "запросить и рассмотреть документы, необходимые для "
                //            + "принятия объективного решения в рамках проводимой проверки " + "(с {0} по {1} - {2}.)",
                //                                         Report["НачалоПериода"],
                //                                         Report["ОкончаниеПериода"],
                //                                         durationInspections);
                //}



                Report.RegData("СекцияМероприятияПоКонтролю", dispControlMeasureDomain.GetAll().Where(x => x.Disposal.Id == disposal.Id).Select(x => new { МероприятияПоКонтролю = x.ControlMeasuresName}));

                var listExperts = disposalExpertDomain.GetAll()
                        .Where(x => x.Disposal.Id == disposal.Id)
                        .Select(x => new
                        {
                            Эксперты = x.Expert.Name
                        })
                        .ToList();

                if (listExperts.Count == 0)
                {
                    listExperts.Add(new
                    {
                        Эксперты = "Не привлекаются"
                    });
                }

                Report.RegData("СекцияЭксперты", listExperts);

                var serviceTypeSurvey = disposalTypeSurveyDomain.GetAll().Where(x => x.Disposal.Id == disposal.Id).ToList();

                if (serviceTypeSurvey.Any())
                {
                    Report["ТипОбследования"] = serviceTypeSurvey.Select(x => x.TypeSurvey.Name)
                             .Aggregate(string.Empty, (x, y) => x + (string.IsNullOrEmpty(x) ? y : ", " + y));    
                }

                var typeSurveyCodes = serviceTypeSurvey.Select(x => x.TypeSurvey.Code).ToList();

                var strRuleCode12 = "Правила предоставления коммунальных услуг собственникам и пользователям "
                                    + "помещений в многоквартирных домах и жилых домов, утвержденных постановлением Правительства РФ от 06.05.2011 № 354.";

                var strRuleCode13 = "Правила и нормы технической эксплуатации жилищного фонда, "
                                    + "утвержденные постановлением Госстроя РФ от 27.09.2003 № 170; "
                                    + "Правила содержания общего имущества в многоквартирном доме, утвержденные постановлением Правительства РФ от 13.08.2006 № 491.";

                var strRuleCode14 =
                    "Правила предоставления коммунальных услуг собственникам и пользователям помещений в многоквартирных домах и жилых домов, "
                                    + "утвержденных постановлением Правительства РФ от 06.05.2011 № 354 и "
                                    + "Правила и нормы технической эксплуатации жилищного фонда, утвержденные постановлением Госстроя РФ от 27.09.2003 № 170";

                if (typeSurveyCodes.Count == 1)
                {
                    var firstCode = typeSurveyCodes.First();

                    switch (firstCode)
                    {
                        case "12":
                            Report["Правило"] = strRuleCode12;
                            break;
                        case "13":
                            Report["Правило"] = strRuleCode13;
                            break;
                        case "14":
                            Report["Правило"] = strRuleCode14;
                            break;
                    }
                }
                else if (typeSurveyCodes.Count > 1)
                {
                    Report["Правило"] = strRuleCode14;
                }

                var listDispSurveySubject =
                    disposalSurveySubjectDomain.GetAll()
                        .Where(x => x.Disposal.Id == disposal.Id)
                        .Select(x => x.SurveySubject.Name)
                        .ToList();


                Report["ПредметПроверки"] = listDispSurveySubject.AggregateWithSeparator(", ");

                var resultSectionDispSurveySubject = listDispSurveySubject.Select(x => new
                {
                    ПредметПроверки = x,
                });

                Report.RegData("СекцияПредметПроверки", resultSectionDispSurveySubject);

                var kindCheck = string.Empty;

                switch (disposal.KindCheck.Code)
                {
                    case TypeCheck.PlannedExit:
                        kindCheck = "плановой выездной";
                        break;
                    case TypeCheck.NotPlannedExit:
                        kindCheck = "внеплановой выездной";
                        break;
                    case TypeCheck.PlannedDocumentation:
                        kindCheck = "плановой документарной";
                        break;
                    case TypeCheck.NotPlannedDocumentation:
                        kindCheck = "внеплановой документарной";
                        break;
                    case TypeCheck.InspectionSurvey:
                        kindCheck = "инспекционной";
                        break;
                }

                Report["ВидПроверки"] = kindCheck;

                var inspectionId = disposal.Inspection.Id;

                var inspectionGjiData =
                    inspectionGjiDomain.GetAll()
                    .Where(x => x.Id == inspectionId)
                                       .Select(x => new { x.TypeBase, x.InspectionNumber, x.ObjectCreateDate, x.PersonInspection })
                    .FirstOrDefault();

                if (inspectionGjiData != null)
                {
                    Report["ТипОснованияПроверки"] = inspectionGjiData.TypeBase.GetEnumMeta().Display;

                    if (inspectionGjiData.PersonInspection == PersonInspection.PhysPerson
                        || inspectionGjiData.PersonInspection == PersonInspection.Official)
                    {
                        switch (inspectionGjiData.TypeBase)
                        {
                            case TypeBase.CitizenStatement:
                                Report["ТипОснованияПроверки"] = "проверкой по обращению и заявлению";
                                break;
                            case TypeBase.ProsecutorsClaim:
                                Report["ТипОснованияПроверки"] = "проверкой по требованию прокуратуры";
                                break;
                            case TypeBase.DisposalHead:
                                Report["ТипОснованияПроверки"] = "проверкой по поручению руководителей";
                                break;
                        }
                    }
                    
                    Report["НомерОснованияПроверки"] = inspectionGjiData.InspectionNumber;
                    Report["ДатаОснованияПроверки"] = inspectionGjiData.ObjectCreateDate.ToString("dd.MM.yyyy г.");
                    
                }

                var appealCitsData =
                    inspectionAppealCitsDomain.GetAll()
                    .Where(x => x.Inspection.Id == inspectionId)
                                              .Select(
                                                  x =>
                                                  new
                                                  {
                                                      x.AppealCits.Id,
                                                      x.AppealCits.NumberGji,
                                                      x.AppealCits.DateFrom,
                                                      x.AppealCits.Correspondent,
                                                      x.AppealCits.TypeCorrespondent
                                                  })
                    .FirstOrDefault();

                if (appealCitsData != null)
                {
                    Report["НомерОбращения"] = appealCitsData.NumberGji;
                    Report["ДатаОбращения"] = appealCitsData.DateFrom.HasValue
                            ? appealCitsData.DateFrom.Value.ToString("dd.MM.yyyy г.")
                            : string.Empty;

                    Report["Корреспондент"] = appealCitsData.Correspondent;

                    Report["ТипКорреспондента"] = appealCitsData.TypeCorrespondent.GetEnumMeta().Display;
                }

                var jurPersonPlan =
                    baseJurPersonDomain.GetAll().Where(x => x.Id == inspectionId).Select(x => x.Plan.Name).FirstOrDefault();

                //if (jurPersonPlan != null)
                //{
                //    int planYear = Convert.ToInt32(Regex.Replace(jurPersonPlan, @"[^\d]+", ""));
                //    Report["ПлановыйГод"] = planYear.ToString();
                //}

                var nameGenitive = appealCitsData != null
                    ? appealCitsSourceDomain.GetAll()
                        .Where(x => x.AppealCits.Id == appealCitsData.Id)
                                                               .Select(
                                                                   x =>
                                                                   new
                                                                   {
                                                                       x.RevenueSource.Name,
                                                                       x.RevenueSource.NameGenitive
                                                                   })
                        .FirstOrDefault()
                    : null;

                Report["ИсточникОбращенияРП"] = nameGenitive.ReturnSafe(x => x.NameGenitive);

                var typeSurveyId =
                    disposalTypeSurveyDomain.GetAll()
                    .Where(x => x.Disposal.Id == disposal.Id)
                    .Select(x => x.TypeSurvey.Id)
                    .FirstOrDefault();

                var typeSurveyGjiIssueName =
                    typeSurveyGjiIssueDomain.GetAll()
                    .Where(x => x.TypeSurvey.Id == typeSurveyId)
                    .Select(x => x.SurveyPurpose.Name)
                    .FirstOrDefault();

                Report["ПоВопросу"] = typeSurveyGjiIssueName;

                var strBasesurveyDisp = string.Empty;

                if (disposal.TypeDisposal == TypeDisposalGji.DocumentGji)
                {
                    var firstPrescription = servDocChildren
                            .GetAll()
                            .Where(x => x.Children.Id == disposal.Id && x.Parent.TypeDocumentGji == TypeDocumentGji.Prescription)
                            .Select(x => x.Parent)
                            .FirstOrDefault();

                    if (firstPrescription != null)
                    {
                        if (inspectionGjiData != null)
                        {
                            if (inspectionGjiData.PersonInspection == PersonInspection.PhysPerson
                                || inspectionGjiData.PersonInspection == PersonInspection.Official)
                            {
                                Report["ТипОснованияПроверки"] = string.Format("проверкой предписания {0} от {1}", 
                                    firstPrescription.DocumentNumber, 
                                    firstPrescription.DocumentDate.HasValue ? firstPrescription.DocumentDate.Value.ToShortDateString(): string.Empty);
                            }

                        }
                    }
            
                }
                else if (disposal.Inspection.TypeBase == TypeBase.CitizenStatement
                         && nameGenitive.ReturnSafe(x => x.Name.ToLower().Trim()) == "заявитель")
                {
                    strBasesurveyDisp =
                        string.Format(
                            "обращение граждан (вх. № {0} от {1})"
                                                      + " по вопросу {2} в доме, расположенному по адресу: {3}.",
                                                      Report["НомерОбращения"],
                                                      Report["ДатаОбращения"],
                                                      Report["ПоВопросу"],
                                                      Report["ДомаИАдреса"]);
                }
                else if (disposal.Inspection.TypeBase == TypeBase.CitizenStatement && nameGenitive != null
                         && nameGenitive.ReturnSafe(x => x.Name.ToLower().Trim()) != "заявитель")
                {
                    strBasesurveyDisp =
                        string.Format(
                            "обращение граждан, поступившее в Департамент из {0} (вх. № {1} от {2}) по вопросу {3}"
                                                      + " в доме, расположенному по адресу: {4}.",
                                                      Report["ИсточникОбращения(РП)"],
                                                      Report["НомерОбращения"],
                                                      Report["ДатаОбращения"],
                                                      Report["ПоВопросу"],
                                                      Report["ДомаИАдреса"]);
                }
                else if (disposal.Inspection.TypeBase == TypeBase.CitizenStatement && nameGenitive == null)
                {
                    strBasesurveyDisp =
                        string.Format(
                            "обращение граждан, поступившее в Департамент (вх. № {0} от {1}) по вопросу {2}"
                                                      + " в доме, расположенному по адресу: {3}.",
                                                      Report["НомерОбращения"],
                                                      Report["ДатаОбращения"],
                                                      Report["ПоВопросу"],
                                                      Report["ДомаИАдреса"]);
                }
                else if (disposal.Inspection.TypeBase == TypeBase.DisposalHead)
                {
                    strBasesurveyDisp =
                        string.Format(
                            "поручение руководителя (вх. № {0} от {1}) по вопросу {2} в доме, расположенному по адресу: {3}",
                                                      Report["НомерОснованияПроверки"],
                                                      Report["ДатаОснованияПроверки"],
                                                      Report["ПоВопросу"],
                                                      Report["ДомаИАдреса"]);
                }
                else if (disposal.Inspection.TypeBase == TypeBase.ProsecutorsClaim)
                {
                    strBasesurveyDisp =
                        string.Format(
                            "требование {0} по вопросу {1} в доме, расположенному по адресу: {2}",
                                                      Report["ИсточникОбращения(РП)"],
                                                      Report["ПоВопросу"],
                                                      Report["ДомаИАдреса"]);
                }
                else if (disposal.Inspection.TypeBase == TypeBase.PlanJuridicalPerson)
                {
                    strBasesurveyDisp = string.Format(
                        "план проведения плановых проверок на {0} год", Report["ПлановыйГод"]);
                }

                Report["ОснованиеОбследованияПриказа"] = strBasesurveyDisp;
            }
            finally
            {
                //Container.Release(displosalProvidedDocDateDomain);
                Container.Release(disposalProvidedDocDomain);
                Container.Release(disposalExpertDomain);
                Container.Release(disposalTypeSurveyDomain);
                Container.Release(disposalSurveySubjectDomain);
                Container.Release(typeSurveyInspFoundationGjiDomain);
                Container.Release(inspectionGjiDomain);
                Container.Release(inspectionAppealCitsDomain);
                Container.Release(baseJurPersonDomain);
                Container.Release(appealCitsSourceDomain);
                Container.Release(typeSurveyGjiIssueDomain);
                Container.Release(servDocChildren);
                Container.Release(dispControlMeasureDomain);
            }
        }


    }
}
