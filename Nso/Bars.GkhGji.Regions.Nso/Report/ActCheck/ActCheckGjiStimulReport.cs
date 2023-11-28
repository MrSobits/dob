namespace Bars.GkhGji.Regions.Nso.Report
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    using B4.DataAccess;
    using B4.IoC;
    using B4.Modules.Reports;
    using B4.Modules.FIAS;
    using B4.Utils;
    using Entities.Disposal;
    using Gkh.Entities;
    using Gkh.Enums;
    using Gkh.Report;
    using Gkh.Utils;
    using GkhCalendar.DomainService;
    using GkhGji.Entities;
    using GkhGji.Entities.Dict;
    using GkhGji.Enums;
    using Entities;
    using Properties;
    using GkhGji.Report;
    using Stimulsoft.Report;
    using Stimulsoft.Report.Export;
	
	/// <summary>
	/// Отчет акта проверки
	/// </summary>
	public class ActCheckGjiStimulReport : GjiBaseStimulReport
    {
        #region Constructors and Destructors

		/// <summary>
		/// Конструктор
		/// </summary>
        public ActCheckGjiStimulReport()
            : base(new ReportTemplateBinary(Resources.ActCheck))
        {
        }

        #endregion

        #region Public Properties

		/// <summary>
		/// Код формы
		/// </summary>
        public override string CodeForm
        {
            get
            {
                return "ActCheck";
            }
        }

		/// <summary>
		/// Описание
		/// </summary>
        public override string Description
        {
            get
            {
                return "Акт проверки";
            }
        }

		/// <summary>
		/// Формат экспорта
		/// </summary>
        public override StiExportFormat ExportFormat
        {
            get
            {
                return StiExportFormat.Word2007;
            }
        }

		/// <summary>
		/// Настройки экспорта
		/// </summary>
		public override StiExportSettings ExportSettings
		{
			get
			{
				return new StiWord2007ExportSettings
				{
					RemoveEmptySpaceAtBottom = false
				};
			}
		}

		/// <summary>
		/// Идентификатор отчета
		/// </summary>
		public override string Id
        {
            get
            {
                return "ActCheck";
            }
        }

		/// <summary>
		/// Наименование
		/// </summary>
        public override string Name
        {
            get
            {
                return "Акт проверки";
            }
        }

        #endregion

        #region Properties

		/// <summary>
		/// Код шаблона
		/// </summary>
        protected override string CodeTemplate { get; set; }

        private long DocumentId { get; set; }

        #endregion

        #region Public Methods and Operators

		/// <summary>
		/// Получить информацию о шаблоне
		/// </summary>
		/// <returns></returns>
        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
            {
                new TemplateInfo
                {
                    Code = "NsoActSurvey",
                    Name = "TomskActSurvey",
                    Description = "Акт проверки НСО",
                    Template = Resources.ActCheck
                }
            };
        }

		/// <summary>
		/// Подготовить отчет
		/// </summary>
		/// <param name="reportParams">Параметры отчета</param>
        public override void PrepareReport(ReportParams reportParams)
        {
            var actCheckDomain = Container.ResolveDomain<NsoActCheck>();

            try
            {
                var act = actCheckDomain.Get(DocumentId);
                if (act == null)
                {
                    throw new ReportProviderException("Не удалось получить акт проверки");
                }
                FillCommonFields(act);

                Report["ДатаАкта"] = act.DocumentDate.ToDateString("d MMMM yyyy");
                Report["НомерАкта"] = act.DocumentNumber;
                Report["ПроверПлощадь"] = act.Area.HasValue ? act.Area.Value.ToStr() : null;
                Report["СКопиейПриказаОзнакомлен"] = act.AcquaintedWithDisposalCopy;
                Report["МестоСоставления"] = act.DocumentPlace;
                Report["ВремяСоставленияАкта"] = act.DocumentTime.ToTimeString();
                
                FillActSubentities(act);

                FillActCheckDates(act);

                FillActCheckViolations();

                FillInspection(act.Inspection);

                var disposal = FillParentDisposal(act);

                FillProvidedDoc(act, disposal);

                this.FillPrescription(act);
            }
            finally
            {
                Container.Release(actCheckDomain);
            }
        }

        private void FillActSubentities(NsoActCheck act)
        {
            if(act == null) return;

            var docInspectorDomain = Container.ResolveDomain<DocumentGjiInspector>();
            var zonalInspectorDomain = Container.ResolveDomain<ZonalInspectionInspector>();
            var actAnnexDomain = Container.ResolveDomain<ActCheckAnnex>();
            var actWitnessDomain = Container.ResolveDomain<ActCheckWitness>();
            var actLongTextDomain = Container.ResolveDomain<NsoDocumentLongText>();

            try
            {
                var longText = actLongTextDomain.GetAll().FirstOrDefault(x => x.DocumentGji.Id == act.Id);

                if (longText != null)
                {
                    Report["СведенияОЛицахДопустившихНарушения"] = longText.PersonViolationInfo.GetString();
                    Report["СведенияСвидетельствующие"] = longText.PersonViolationActionInfo.GetString();
                }

                var witness = actWitnessDomain.GetAll()
                    .Where(x => x.ActCheck.Id == DocumentId)
                    .Select(x => new { x.Fio, x.Position, x.IsFamiliar })
                    .ToArray();

                var allWitness = witness.AggregateWithSeparator(x => x.Position + " - " + x.Fio, ", ");
                var witnessIsFamiliar = witness.Where(x => x.IsFamiliar).AggregateWithSeparator(x => x.Fio, ", ");

                Report["ДЛприПроверке"] = allWitness;
                Report["Ознакомлен"] = witnessIsFamiliar;
                Report["ЧислоЛицПривлеченныеЛица"] = witness.Length > 0 ? "Лица, привлеченные" : "Лицо, привлеченное";

                var actCheckAnnex = actAnnexDomain.GetAll()
                    .Where(x => x.ActCheck.Id == DocumentId)
                    .AggregateWithSeparator(x => x.Name, ",");

                Report["ПрилагаемыеДокументы"] = actCheckAnnex;

                var inspectors = docInspectorDomain.GetAll()
                    .Where(x => x.DocumentGji.Id == DocumentId)
                    .Select(x => x.Inspector)
                    .ToArray();

                Report["ИнспекторыИКоды"] = inspectors
                    .Select(x => x.Fio + (x.Code.IsEmpty() ? null : " - " + x.Code))
                    .AggregateWithSeparator(", ");

                Report["ИнспекторыАкта"] = inspectors
                    .Select(x => x.Fio + (x.Position.IsEmpty() ? null : " - " + x.Position))
                    .AggregateWithSeparator(", ");

                var inspectorIds = inspectors.Select(x => x.Id).ToArray();

                if (inspectorIds.Any())
                {
                    var zonalInspection =
                        zonalInspectorDomain.GetAll()
                            .Where(x => inspectorIds.Contains(x.Inspector.Id))
                            .Where(x => x.ZonalInspection.Name != null)
                            .Select(x => x.ZonalInspection.Name)
                            .FirstOrDefault();

                    Report["Отдел"] = zonalInspection;
                }
            }
            finally
            {
                Container.Release(docInspectorDomain);
                Container.Release(zonalInspectorDomain);
                Container.Release(actAnnexDomain);
                Container.Release(actWitnessDomain);
                Container.Release(actLongTextDomain);
            }
        }

        private void FillActCheckDates(NsoActCheck act)
        {
#warning разобраться с этой фигней

            var actCheckPeriodDomain = Container.ResolveDomain<ActCheckPeriod>();
            var calendarService = Container.Resolve<IIndustrialCalendarService>();

            var actCheckPeriods = actCheckPeriodDomain.GetAll()
                .Where(x => x.ActCheck.Id == act.Id)
                .Select(x => new {x.DateCheck, x.DateStart, x.DateEnd})
                .OrderBy(x => x.DateCheck)
                .ThenBy(x => x.DateEnd)
                .ToArray();

            Report.RegData("ДатаИВремяПроведенияПроверки",
                actCheckPeriods
                    .Select(x => new
                    {
                        Дата = x.DateCheck.ToDateString(),
                        ВремяНачала = x.DateStart.ToTimeString(),
                        ВремяОкончания = x.DateEnd.ToTimeString(),
                        Продолжительность = x.DateStart.HasValue && x.DateEnd.HasValue
                            ? ((int) Math.Round((x.DateEnd.Value - x.DateStart.Value).TotalHours,
                                    MidpointRounding.AwayFromZero))
                                .ToString(CultureInfo.InvariantCulture)
                            : null
                    })
                    .ToArray());

            if (actCheckPeriods.Any())
            {
                var min = actCheckPeriods.OrderBy(x => x.DateStart).FirstOrDefault(x => x.DateStart.HasValue);
                var max = actCheckPeriods.OrderByDescending(x => x.DateEnd).FirstOrDefault(x => x.DateStart.HasValue);

                if (min != null && max != null)
                {
                    Report["ДатаВремяПроверки"] =
                        string.Format("с {0} {1} по {2} {3}",
                            min.DateStart.ToDateString("HH час. mm мин."),
                            min.DateCheck.ToDateString("«dd» MMMM yyyy г."),
                            max.DateEnd.ToDateString("HH час. mm мин."),
                            max.DateCheck.ToDateString("«dd» MMMM yyyy г."));
                }
            }

            if (actCheckPeriods.Length > 0)
            {
                var firstDay = actCheckPeriods.First();
                var lastDay = actCheckPeriods.Last();

                var days = calendarService
                    .GetWorkDays(firstDay.DateCheck.Value.AddDays(-1), lastDay.DateCheck.Value)
                    .Distinct()
                    .Select(x => x.DayDate)
                    .ToHashSet();

                var interval = new TimeSpan();

                if (days.Contains(firstDay.DateCheck.ToDateTime()))
                {
                    var date = firstDay.DateStart.ToDateTime();

                    interval = GetInteval(date, new DateTime(date.Year, date.Month, date.Day, 18, 0, 0, 0));
                }

                interval += new TimeSpan(0, (days.Count - 2) * 8, 0, 0);

                if (actCheckPeriods.Length > 1 && days.Contains(lastDay.DateCheck.ToDateTime()))
                {
                    var date = lastDay.DateEnd.ToDateTime();
                    interval += GetInteval(new DateTime(date.Year, date.Month, date.Day, 9, 0, 0, 0), date);
                }

                var hours = (int) interval.TotalHours;
                var workDays = hours / 8;
                hours = hours % 8;

                if (hours == 0)
                {
                    Report["ОбщаяПродолжительностьРабДни"] = "{0} дней".FormatUsing(workDays);
                }
                else
                {
                    Report["ОбщаяПродолжительностьРабДни"] = "{0} дней, {1} часов".FormatUsing(workDays, hours);
                }
            }

            var checkDatesList =
                actCheckPeriodDomain.GetAll()
                    .Where(x => x.ActCheck.Id == act.Id)
                    .Select(x => new {x.DateStart, x.DateEnd})
                    .ToList();

            if (checkDatesList.Count > 0)
            {
                var checkTime =
                    checkDatesList.Where(x => x.DateEnd.HasValue).Max(x => x.DateEnd.Value)
                    - checkDatesList.Where(x => x.DateStart.HasValue).Min(x => x.DateStart.Value);

                // полчаса и более округляем до часа
                var delta = Math.Round(checkTime.TotalHours - (int) checkTime.TotalHours, 1);
                var hours = delta >= 0.5 ? checkTime.Hours + 1 : checkTime.Hours;

                Report["ПродолжительностьПроверкиДЧ"] = checkTime.Days > 0
                    ? string.Format("{0} д. {1} ч.", checkTime.Days, hours)
                    : string.Format("{0} ч.", hours);
            }
        }

        private void FillActCheckViolations()
        {
            var actRoDomain = Container.ResolveDomain<ActCheckRealityObject>();
            var actViolStageDomain = Container.ResolveDomain<ActCheckViolation>();
            var normDocItemDomain = Container.ResolveDomain<ViolationNormativeDocItemGji>();
            var roLongTextDomain = Container.ResolveDomain<ActCheckRoLongDescription>();
            var violLongTextDomain = Container.ResolveDomain<ActCheckViolationLongText>();

            try
            {
                // Дома акта проверки
                var actRobjects = actRoDomain.GetAll()
                    .Where(x => x.ActCheck.Id == DocumentId)
                    .Select(x => new
                    {
                        x.RealityObject,
                        x.HaveViolation,
                        x.Description,
                        x.NotRevealedViolations,
                        x.OfficialsGuiltyActions,
                        x.PersonsWhoHaveViolated,
                        x.Id
                    })
                    .ToArray();

                /*Report["СведенияОЛицахДопустившихНарушения"] = actRobjects.AggregateWithSeparator(x => x.PersonsWhoHaveViolated, ", ");

                Report["СведенияСвидетельствующие"] = actRobjects.AggregateWithSeparator(x => x.OfficialsGuiltyActions, ", ");*/

                if (actRobjects.All(x => x.HaveViolation == YesNoNotSet.No))
                {
                    Report["НеВыявлено"] = actRobjects.AggregateWithSeparator(x => x.Description, "; ");
                    Report["НарушенияВыявлены"] = "Нет";
                }
                else
                {
                    Report["НарушенияВыявлены"] = "Да";
                }

                Report["НевыявленныеНарушения"] = actRobjects.AggregateWithSeparator(x => x.NotRevealedViolations, ", ");

                if (actRobjects.Length > 0)
                {
                    var firstRecord = actRobjects.First();

                    Report["Описание"] = GetViolationDescription(firstRecord.Id);

                    var firstRo = firstRecord.RealityObject;

                    if (firstRo != null)
                    {
                        Report["НомерДома"] = firstRo.FiasAddress.House;
                        Report["АдресДома"] = firstRo.FiasAddress.PlaceName + ", " + firstRo.FiasAddress.StreetName;
                        Report["НаселенныйПункт"] = firstRo.FiasAddress.PlaceName;

                        if (actRobjects.Length == 1)
                        {
                            Report["УлицаДом"] =
                                string.Format("{0}, {1}, д.{2}{3} ",
                                    firstRo.FiasAddress.PlaceName,
                                    firstRo.FiasAddress.StreetName,
                                    firstRo.FiasAddress.House,
                                    GetHousing(firstRo.FiasAddress.Housing));

                            Report["ДомАдрес"] = firstRo.FiasAddress.AddressName;
                        }
                        else
                        {
                            var realObjs = new StringBuilder();

                            foreach (var ro in actRobjects.Select(x => x.RealityObject))
                            {
                                if (realObjs.Length > 0)
                                    realObjs.Append("; ");

                                realObjs.AppendFormat("{0}, д.{1}{2}",
                                    ro.FiasAddress.StreetName,
                                    ro.FiasAddress.House,
                                    GetHousing(ro.FiasAddress.Housing));
                            }

                            Report["УлицаДом"] = string.Format("{0}.", realObjs);
                            Report["ДомаИАдреса"] = string.Format("{0}, {1}. ", firstRo.FiasAddress.PlaceName, realObjs);
                        }
                    }
                }

                
                var sum = actRobjects.Select(x => x.RealityObject.Return(z => z.AreaMkd)).Sum() ?? 0;
                Report["ОбщаяПлощадьСумма"] = sum.RoundDecimal(2).ToString(CultureInfo.InvariantCulture);

                // Нарушения
                var violData =
                    actViolStageDomain.GetAll()
                        .Where(x => x.Document.Id == DocumentId)
                        .Select(x => new
                        {
                            ViolId = x.Id,
                            ActObjectId = x.ActObject.Id,
                            x.InspectionViolation.Violation.Id,
                            x.InspectionViolation.Violation.CodePin,
                            x.InspectionViolation.Violation.Name,
                            x.InspectionViolation.Violation.PpRf25,
                            x.InspectionViolation.Violation.PpRf170,
                            x.InspectionViolation.Violation.PpRf307,
                            x.InspectionViolation.Violation.PpRf491,
                            x.InspectionViolation.Violation.OtherNormativeDocs,
                            x.InspectionViolation.RealityObject,
                            x.InspectionViolation.RealityObject.FiasAddress
                        })
                        .ToArray();

                Report["ТекстНарушения"] = violData.Select(x => x.Name).Distinct().AggregateWithSeparator(", ");

                Report["СсылкиНаПунктыНормативныхАктов"] = violData
                    .SelectMany(x => new[] {x.PpRf25, x.PpRf170, x.PpRf307, x.PpRf491, x.OtherNormativeDocs})
                    .AggregateWithSeparator(", ");

                var violIds = violData.Select(x => x.Id).Distinct().ToArray();

                var normDocs = normDocItemDomain.GetAll()
                    .Where(x => violIds.Contains(x.ViolationGji.Id))
                    .Select(x => new
                    {
                        Violation = x.ViolationGji.Name,
                        x.NormativeDocItem.NormativeDoc.Name,
                        x.NormativeDocItem.NormativeDoc.FullName,
                        x.NormativeDocItem.Number
                    })
                    .ToArray();

                Report["ПереченьНПД"] = normDocs
                    .Select(x => string.Format("{0} (далее - {1})", x.FullName, x.Name))
                    .Distinct()
                    .AggregateWithSeparator(", ");

                var actViolIds = violData.Select(z => z.ViolId).ToArray();

                var violDescriptions = violLongTextDomain.GetAll()
                    .Where(x => actViolIds.Contains(x.Violation.Id))
                    .GroupBy(z => z.Violation.Id)
                    .ToDictionary(z => z.Key, x => x.First().Description.GetString());
                


                var roIds = violData.Select(x => x.ActObjectId).Distinct().ToArray();

                var addChars = roLongTextDomain.GetAll()
                    .Where(x => roIds.Contains(x.ActCheckRo.Id))
                    .GroupBy(z => z.ActCheckRo.Id)
                    .ToDictionary(x => x.Key, z => z.First().AdditionalChars.GetString());

                //Если не указан дом, в этом случае проверяются не дома, а организация. Информация о домах не будет выводиться
                Report.RegData("ДомаПроверки", violData
                    .Distinct(x => x.ActObjectId)
                    .Where(o => o.RealityObject != null)
                    .Select(z =>
                    {
                        var ro = z.RealityObject;

                        return new
                        {
                            Адрес = ro.Address,
                            ХарактеристикиТП = GetRobjectChars(ro),
                            ДопХарактеристики = addChars.Get(z.ActObjectId)
                        };
                    }));
            }
            finally
            {
                Container.Release(actRoDomain);
                Container.Release(actViolStageDomain);
                Container.Release(normDocItemDomain);
                Container.Release(roLongTextDomain);
                Container.Release(violLongTextDomain);
            }
        }

        private NsoDisposal FillParentDisposal(NsoActCheck act)
        {
            var disposal = GetParentDocument(act, TypeDocumentGji.Disposal) as NsoDisposal;

            if(disposal == null) return null;

            var expertDomain = Container.ResolveDomain<DisposalExpert>();
            var typeSurveyDomain = Container.ResolveDomain<DisposalTypeSurvey>();
            var typeSurveyGoalDomain = Container.ResolveDomain<TypeSurveyGoalInspGji>();
            var subjDomain = Container.ResolveDomain<DisposalVerificationSubject>();

            try
            {
                Report["ДатаРаспоряжения"] = disposal.DocumentDate.ToDateString("d MMMM yyyy");
                Report["НомерРаспоряжения"] = disposal.DocumentNumber;

                Report["НомерРешенияПрокурора"] = disposal.ProsecutorDecNumber;
                Report["ДатаРешенияПрокурора"] = disposal.ProsecutorDecDate.ToDateString();

                Report["СогласованиеСПрокуратурой"] = disposal.TypeAgreementProsecutor.GetEnumMeta().Display;

                Report["ОснованиеОбследования"] = GetDocumentBase(disposal);

                if (!disposal.KindCheck.Return(x => x.Name).IsEmpty())
                {
                    Report["ВидПроверки"] = disposal.KindCheck.Name;
                    Report["ВидОбследования"] = disposal.KindCheck.Name;
                    Report["ВидОбследованияРП"] = GetMorpher().Проанализировать(disposal.KindCheck.Name).Родительный;
                }

                if (disposal.IssuedDisposal != null)
                {
                    var issued = disposal.IssuedDisposal;

                    if (!string.IsNullOrEmpty(issued.PositionGenitive))
                    {
                        Report["КодИнспектораРП"] = issued.PositionGenitive.ToLower();
                        Report["КодИнспектораТП"] = issued.PositionAblative.ToLower();
                    }

                    Report["РуководительРП"] = issued.FioGenitive;
                    Report["РуководительТП"] = issued.FioAblative;

                    Report["КодРуководителяФИО"] =
                        string.Format("{0} - {1}",
                            issued.Code,
                            Coalesce(issued.ShortFio, issued.Fio));
                }

                if (disposal.DateStart.HasValue && disposal.DateEnd.HasValue)
                {
                    var startDate = disposal.DateStart;

                    var countDays = 0;

                    while (startDate.Value.Date != disposal.DateEnd.Value.Date)
                    {
                        if (startDate.Value.DayOfWeek != DayOfWeek.Sunday && startDate.Value.DayOfWeek != DayOfWeek.Saturday)
                        {
                            countDays++;
                        }

                        startDate = startDate.Value.AddDays(1);
                    }

                    Report["ПродолжительностьПроверки"] = countDays.ToString(CultureInfo.InvariantCulture);
                }

                Report["Эксперты"] = expertDomain.GetAll()
                    .Where(x => x.Disposal.Id == disposal.Id)
                    .Select(x => x.Expert)
                    .AggregateWithSeparator(x => x.Name, ", ");

                Report["НачалоПериода"] = disposal.DateStart.ToDateString();
                Report["ОкончаниеПериода"] = disposal.DateEnd.ToDateString();

                var queryTypeSurveyIds = typeSurveyDomain.GetAll()
                    .Where(x => x.Disposal.Id == disposal.Id)
                    .Select(x => x.TypeSurvey.Id);

                var strGoalInspGji = typeSurveyGoalDomain.GetAll()
                    .Where(x => queryTypeSurveyIds.Contains(x.TypeSurvey.Id))
                    .AggregateWithSeparator(x => x.SurveyPurpose.Name, "; ");

                Report["Цель"] = strGoalInspGji;

                Report.RegData("ПредметыПроверки", subjDomain.GetAll()
                    .Where(x => x.Disposal.Id == disposal.Id)
                    .Select(x => new { Наименование = x.SurveySubject.Name })
                    .ToList());
            }
            finally
            {
                Container.Release(expertDomain);
                Container.Release(typeSurveyDomain);
                Container.Release(typeSurveyGoalDomain);
                Container.Release(subjDomain);
            }

            return disposal;
        }

        private void FillPrescription(NsoActCheck act)
        {
            var prescription = this.GetParentDocument(act, TypeDocumentGji.Prescription);

            if (prescription == null) return;

            var prescrViolDomain = Container.ResolveDomain<PrescriptionViol>();
            var actRemovalDomain = Container.ResolveDomain<ActRemovalViolation>();

            try
            {
                Report["НомерПредписания"] = prescription.DocumentNumber;
                Report["ДатаПредписания"] = prescription.DocumentDate.ToDateString();

                var viols =
                    prescrViolDomain.GetAll()
                                    .Where(x => x.Document.Id == prescription.Id)
                                    .Select(
                                        x =>
                                        new
                                            {
                                                x.Id,
                                                InspViolId = x.InspectionViolation.Id,
                                                ViolationId = x.InspectionViolation.Violation.Id,
                                                x.InspectionViolation.Violation.CodePin,
                                                x.Action,
                                                x.Description,
                                                x.InspectionViolation.DatePlanRemoval,
                                                x.InspectionViolation.DateFactRemoval,
                                                Municipality = x.InspectionViolation.RealityObject.Municipality.Name,
                                                RealityObject = x.InspectionViolation.RealityObject.Address,
                                            })
                                    .OrderBy(x => x.Id)
                                    .AsEnumerable()
                                    .Select(
                                        (x, n) =>
                                        new
                                            {
                                                Num = n + 1,
                                                x.InspViolId,
                                                x.ViolationId,
                                                x.CodePin,
                                                x.Action,
                                                x.Description,
                                                x.DateFactRemoval,
                                                x.DatePlanRemoval
                                            })
                                    .ToArray();

                Report["НомераНевыполненныхПунктовПредписания"] =
                    viols.Where(x => !x.DateFactRemoval.HasValue)
                         .Select(x => x.Num.ToString())
                         .AggregateWithSeparator(", ");

                var actRemoval = GetChildDocument(prescription, TypeDocumentGji.ActRemoval);

                if (actRemoval != null)
                {
                    var actViols = actRemovalDomain.GetAll()
                        .Where(x => x.Document.Id == actRemoval.Id)
                        .Select(x => new
                        {
                            x.CircumstancesDescription,
                            x.DateFactRemoval,
                            InspViolId = x.InspectionViolation.Id
                        })
                        .ToArray();

                    var result = viols
                        .Where(x => !x.DateFactRemoval.HasValue)
                        .Join(actViols, x => x.InspViolId, x => x.InspViolId, (x, y) => new
                        {
                            x.Num,
                            x.Action,
                            y.CircumstancesDescription,
                            y.DateFactRemoval
                        })
                        .Select(x => new
                        {
                            НомерПункта = x.Num,
                            Мероприятие = x.Action,
                            ОписаниеОбстоятельств = x.CircumstancesDescription,
                            ДатаФактИсполнения =
                                x.DateFactRemoval.HasValue
                                    ? x.DateFactRemoval.ToDateString()
                                    : "Не выполнено"
                        })
                        .ToArray();

                    Report.RegData("ВыполнениеПредписания", result);
                }
            }
            finally
            {
                Container.Release(prescrViolDomain);
                Container.Release(actRemovalDomain);
            }
        }

        private void FillInspection(InspectionGji inspection)
        {
            if(inspection == null) return;

            var contactDomain = Container.ResolveDomain<ContragentContact>();
            try
            {
                Report["ОбъектПроверки"] = inspection.PersonInspection.GetEnumMeta().Display;

                if (!inspection.PhysicalPerson.IsEmpty())
                {
                    Report["ФИОФизлицаРП"] = GetMorpher().Проанализировать(inspection.PhysicalPerson).Родительный;
                }

                var contragent = inspection.Contragent;

                if (contragent == null){ return; }
                else
                {
                    Report["УправОрг"] = contragent.Name;
                    Report["ИННУправОрг"] = contragent.Inn;

                    if (contragent.OrganizationForm != null)
                    {
                        Report["ТипЛица"] = contragent.OrganizationForm.Code != "91"
                            ? "юридического лица"
                            : "индивидуального предпринимателя";
                    }

                    Report["УправОргРП"] = contragent.NameGenitive.IsNotEmpty()
                        ? contragent.NameGenitive
                        : contragent.Name;
                    Report["СокрУправОрг"] = contragent.ShortName;

                    if (contragent.FiasJuridicalAddress != null)
                    {
                        Report["ЮрАдресКонтрагента"] = GetAddress(contragent.FiasJuridicalAddress);
                    }
                    else
                    {
                        Report["ЮрАдресКонтрагента"] = contragent.JuridicalAddress;
                    }

                    if (contragent.FiasFactAddress != null)
                    {
                        Report["АдресКонтрагентаФакт"] = contragent.FiasFactAddress != null
                            ? GetAddress(contragent.FiasFactAddress)
                            : string.Empty;
                    }
                    else
                    {
                        Report["АдресКонтрагентаФакт"] = contragent.FiasFactAddress.Return(x => x.AddressName);
                    }

                    var headContragent =
                        contactDomain.GetAll()
                            .Where(x => x.Contragent.Id == contragent.Id)
                            .Where(x => x.DateStartWork.HasValue)
                            .Where(x => x.DateStartWork.Value <= DateTime.Today)
                            .Where(x => !x.DateEndWork.HasValue || x.DateEndWork.Value >= DateTime.Today)
                            .FirstOrDefault(
                                x => x.Position != null && (x.Position.Code == "1" || x.Position.Code == "4"));

                    if (headContragent != null)
                    {
                        Report["РуководительЮЛДолжность"] =
                            string.Format("{0} {1} {2} {3}",
                                headContragent.Position.Return(x => x.Name),
                                headContragent.Surname,
                                headContragent.Name,
                                headContragent.Patronymic);

                        Report["РуководительЮЛ"] = string.Format("{0} {1} {2}", headContragent.Surname,
                            headContragent.Name, headContragent.Patronymic);
                    }
                }
            }
            finally
            {
                Container.Release(contactDomain);
            }
        }

        private void FillProvidedDoc(NsoActCheck act, Disposal disposal)
        {
            if (act == null || disposal == null) return;

            var actProvDocDomain = Container.ResolveDomain<ActCheckProvidedDoc>();
            var disposalProvDocDomain = Container.ResolveDomain<DisposalProvidedDoc>();

            try
            {
                var disposalProvDocs = disposalProvDocDomain.GetAll()
                    .Where(x => x.Disposal.Id == disposal.Id)
                    .Select(x => new DispProvidedDoc {Наименование = x.ProvidedDoc.Name})
                    .ToArray();

                var actProvDocs = actProvDocDomain.GetAll()
                    .Where(x => x.ActCheck.Id == act.Id)
                    .Select(x => new ActProvidedDoc {Наименование = x.ProvidedDoc.Name})
                    .ToArray();

                Report.RegData("ПредоставляемыеДокументы", disposalProvDocs);
                Report["ПредоставляемыеДокументыСтрокой"] =
                    disposalProvDocs.AggregateWithSeparator(x => x.Наименование, ", ");

                var listProvDocs = actProvDocs
                    .Where(x => disposalProvDocs.Any(y => y.Наименование == x.Наименование))
                    .ToList();

                Report.RegData("ПредоставленныеДокументы", listProvDocs);
                Report["ПредоставленныеДокументыСтрокой"] = 
                    listProvDocs.AggregateWithSeparator(x => x.Наименование, ", ");

                var otherProvDocs = actProvDocs
                    .Where(x => disposalProvDocs.All(y => y.Наименование != x.Наименование))
                    .ToList();

                Report.RegData("ДопПредоставленныеДокументы", otherProvDocs);
                Report["ДопПредоставленныеДокументыСтрокой"] =
                    otherProvDocs.AggregateWithSeparator(x => x.Наименование, ", ");
            }
            finally
            {
                Container.Release(actProvDocDomain);
                Container.Release(disposalProvDocDomain);
            }
        }

        private string GetViolationDescription(long actCheckRoId)
        {
            string result;

            var domainLongText = Container.ResolveDomain<ActCheckRoLongDescription>();
            using (Container.Using(domainLongText))
            {
                var actCheckRoLongDesc = domainLongText.GetAll().FirstOrDefault(x => x.ActCheckRo.Id == actCheckRoId);

                result = actCheckRoLongDesc.Return(z => z.Description).GetString();
            }

            return result;
        }

        private string GetHousing(string housing)
        {
            return !housing.IsEmpty()
                ? ", корп. " + housing
                : null;
        }

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            DocumentId = userParamsValues.GetValue<object>("DocumentId").ToLong();
        }

        #endregion

        #region Methods

        private string GetRobjectChars(RealityObject ro)
        {
            if (ro == null) return null;

            var result = new List<string>();

            if (ro.BuildYear.HasValue)
                result.Add(string.Format("Год постройки: {0}", ro.BuildYear.Value));

            if (ro.PhysicalWear.HasValue)
                result.Add(string.Format("% физического износа: {0}", ro.PhysicalWear.Value));

            if (ro.Floors.HasValue)
                result.Add(string.Format("Этажность: {0}", ro.Floors.Value));

            if (ro.WallMaterial != null)
                result.Add(string.Format("Материалы стен: {0}", ro.WallMaterial.Name));

            if (ro.RoofingMaterial != null)
                result.Add(string.Format("Тип кровли: {0}", ro.RoofingMaterial.Name));

            if (ro.NumberApartments.HasValue)
                result.Add(string.Format("Количество квартир: {0}", ro.NumberApartments.Value));

            if (ro.AreaLiving.HasValue)
                result.Add(string.Format("Площадь жилых помещений: {0}", ro.AreaLiving.Value));

            if (ro.AreaNotLivingFunctional.HasValue)
                result.Add(string.Format("Площадь нежилых помещений: {0}", ro.AreaNotLivingFunctional));

            result.Add(string.Format("Наличие лифтов: {0}", ro.NumberLifts.ToInt() > 0 ? "Да" : "Нет"));

            if (ro.NumberEntrances.HasValue)
                result.Add(string.Format("Количество подъездов: {0}", ro.NumberEntrances.Value));

            if (ro.HeatingSystem != 0)
                result.Add(string.Format("Степень благоустройства: Система отопления-{0}", ro.HeatingSystem.GetEnumMeta().Display));

            return result.AggregateWithSeparator("; ");
        }

        private DateTime ChangeTime(DateTime date, int hours, int minutes)
        {
            return new DateTime(date.Year, date.Month, date.Day, hours, minutes, date.Second, date.Millisecond, date.Kind);
        }

        private TimeSpan GetInteval(DateTime dateStart, DateTime dateEnd)
        {
            if (dateStart.Hour < 9)
            {
                dateStart = dateStart.AddHours(9 - dateStart.Hour);
            }

            if (dateEnd.Hour > 18)
            {
                dateEnd = dateEnd.AddHours(18 - dateEnd.Hour);
            }

            var inteval = dateEnd - dateStart;

            var startLunch = ChangeTime(dateStart.ToDateTime(), 12, 0);
            var endLunch = ChangeTime(dateStart.ToDateTime(), 13, 0);

            if (startLunch > dateStart)
            {
                if (endLunch < dateEnd)
                {
                    inteval -= endLunch - startLunch;
                }
                else if (dateEnd > startLunch)
                {
                    inteval -= dateEnd - startLunch;
                }
            }
            else
            {
                if (endLunch < dateEnd && endLunch > dateStart)
                {
                    inteval -= endLunch - dateStart;
                }
            }

            return inteval > TimeSpan.Zero ? inteval : TimeSpan.Zero;
        }

        private string GetTypeCheckAblative(KindCheckGji kindCheck)
        {
            var result = string.Empty;

            var dictTypeCheckAblative = new Dictionary<TypeCheck, string>
            {
                {TypeCheck.PlannedExit, "плановой выездной"},
                {TypeCheck.NotPlannedExit, "внеплановой выездной"},
                {TypeCheck.PlannedDocumentation, "плановой документарной"},
                {TypeCheck.NotPlannedDocumentation, "внеплановой документарной"},
                {TypeCheck.InspectionSurvey, "внеплановой выездной"},
                {TypeCheck.PlannedDocumentationExit, "плановой документарной и выездной"},
                {TypeCheck.VisualSurvey, "о внеплановой проверке технического состояния жилого помещения"},
                {TypeCheck.NotPlannedDocumentationExit, "внеплановой документарной и выездной"}
            };

            if (kindCheck != null)
            {
                result = dictTypeCheckAblative.Get(kindCheck.Code);
            }

            return result;
        }

        #endregion

        private class ActProvidedDoc
        {
            #region Public Properties

            public string Наименование { get; set; }

            #endregion
        }

        private class DispProvidedDoc
        {
            #region Public Properties

            public string Наименование { get; set; }

            #endregion
        }

        private string GetAddress(FiasAddress fiasAddress)
        {
            string address;
            if (fiasAddress.PlaceAddressName.Contains("г. Новосибирск"))
            {
                address = fiasAddress.PostCode + " г. Новосибирск" + fiasAddress.AddressName.Replace(fiasAddress.PlaceAddressName, "");
            }
            else
            {
                address = fiasAddress.PostCode + fiasAddress.AddressName;
            }
            return address;
        }
    }
}