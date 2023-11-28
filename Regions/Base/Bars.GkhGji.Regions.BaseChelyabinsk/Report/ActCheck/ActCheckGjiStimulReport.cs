namespace Bars.GkhGji.Regions.BaseChelyabinsk.Report.ActCheck
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FIAS;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Report;
    using Bars.Gkh.Utils;
    using Bars.GkhCalendar.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.Dict;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Properties;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.ActCheck;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Disposal;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.DocumentGji;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Properties;
    using Bars.GkhGji.Report;


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

        ///// <summary>
        ///// Формат экспорта
        ///// </summary>
        //      public override StiExportFormat ExportFormat
        //      {
        //          get
        //          {
        //              return StiExportFormat.Odt;
        //          }
        //      }

        ///// <summary>
        ///// Настройки экспорта
        ///// </summary>
        //public override StiExportSettings ExportSettings
        //{
        //	get
        //	{
        //		return new StiOdtExportSettings
        //		{
        //			RemoveEmptySpaceAtBottom = false
        //		};
        //	}
        //}

        public override StiExportFormat ExportFormat
        {
            get { return StiExportFormat.Word2007; }
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
                    Code = "ChelyabinskActSurvey",
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
            var actCheckDomain = this.Container.ResolveDomain<ChelyabinskActCheck>();

            try
            {
                var act = actCheckDomain.Get(this.DocumentId);
                if (act == null)
                {
                    throw new ReportProviderException("Не удалось получить акт проверки");
                }
                this.FillCommonFields(act);

                this.Report["Id"] = this.DocumentId;
                this.Report["ДатаАкта"] = act.DocumentDate.ToDateString("d MMMM yyyy");
                this.Report["НомерАкта"] = act.DocumentNumber;
                this.Report["ПроверПлощадь"] = act.Area.HasValue ? act.Area.Value.ToStr() : null;
                this.Report["СКопиейПриказаОзнакомлен"] = act.AcquaintedWithDisposalCopy;
                this.Report["МестоСоставления"] = act.DocumentPlace;
                this.Report["ВремяСоставленияАкта"] = act.DocumentTime.ToTimeString();
                
                this.FillActSubentities(act);

                this.FillActCheckDates(act);

                this.FillActCheckViolations();

                this.FillInspection(act.Inspection);

                var disposal = this.FillParentDisposal(act);

                this.FillProvidedDoc(act, disposal);

                this.FillPrescription(act);
            }
            finally
            {
                this.Container.Release(actCheckDomain);
            }
        }

        private void FillActSubentities(ChelyabinskActCheck act)
        {
            if(act == null) return;

            var docInspectorDomain = this.Container.ResolveDomain<DocumentGjiInspector>();
            var zonalInspectorDomain = this.Container.ResolveDomain<ZonalInspectionInspector>();
            var actAnnexDomain = this.Container.ResolveDomain<ActCheckAnnex>();
            var actWitnessDomain = this.Container.ResolveDomain<ActCheckWitness>();
            var actLongTextDomain = this.Container.ResolveDomain<ChelyabinskDocumentLongText>();

            try
            {
                var longText = actLongTextDomain.GetAll().FirstOrDefault(x => x.DocumentGji.Id == act.Id);

                if (longText != null)
                {
                    this.Report["СведенияОЛицахДопустившихНарушения"] = longText.PersonViolationInfo.GetString();
                    this.Report["СведенияСвидетельствующие"] = longText.PersonViolationActionInfo.GetString();
                }

                var witness = actWitnessDomain.GetAll()
                    .Where(x => x.ActCheck.Id == this.DocumentId)
                    .Select(x => new { x.Fio, x.Position, x.IsFamiliar })
                    .ToArray();

                var allWitness = witness.AggregateWithSeparator(x => x.Position + " - " + x.Fio, ", ");
                var witnessIsFamiliar = witness.Where(x => x.IsFamiliar).AggregateWithSeparator(x => x.Fio, ", ");

                this.Report["ДЛприПроверке"] = allWitness;
                this.Report["Ознакомлен"] = witnessIsFamiliar;
                this.Report["ЧислоЛицПривлеченныеЛица"] = witness.Length > 0 ? "Лица, привлеченные" : "Лицо, привлеченное";

                var actCheckAnnex = actAnnexDomain.GetAll()
                    .Where(x => x.ActCheck.Id == this.DocumentId)
                    .AggregateWithSeparator(x => x.Name, ",");

                this.Report["ПрилагаемыеДокументы"] = actCheckAnnex;

                var inspectors = docInspectorDomain.GetAll()
                    .Where(x => x.DocumentGji.Id == this.DocumentId)
                    .Select(x => x.Inspector)
                    .ToArray();

                this.Report["ИнспекторыИКоды"] = inspectors
                    .Select(x => x.Fio + (x.Code.IsEmpty() ? null : " - " + x.Code))
                    .AggregateWithSeparator(", ");

                this.Report["ИнспекторыАкта"] = inspectors
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

                    this.Report["Отдел"] = zonalInspection;
                }
            }
            finally
            {
                this.Container.Release(docInspectorDomain);
                this.Container.Release(zonalInspectorDomain);
                this.Container.Release(actAnnexDomain);
                this.Container.Release(actWitnessDomain);
                this.Container.Release(actLongTextDomain);
            }
        }

        private void FillActCheckDates(ChelyabinskActCheck act)
        {
#warning разобраться с этой фигней

            var actCheckPeriodDomain = this.Container.ResolveDomain<ActCheckPeriod>();
            var calendarService = this.Container.Resolve<IIndustrialCalendarService>();

            var actCheckPeriods = actCheckPeriodDomain.GetAll()
                .Where(x => x.ActCheck.Id == act.Id)
                .Select(x => new {x.DateCheck, x.DateStart, x.DateEnd})
                .OrderBy(x => x.DateCheck)
                .ThenBy(x => x.DateEnd)
                .ToArray();

            this.Report.RegData("ДатаИВремяПроведенияПроверки",
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
                    this.Report["ДатаВремяПроверки"] =
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

                    interval = this.GetInteval(date, new DateTime(date.Year, date.Month, date.Day, 18, 0, 0, 0));
                }

                interval += new TimeSpan(0, (days.Count - 2) * 8, 0, 0);

                if (actCheckPeriods.Length > 1 && days.Contains(lastDay.DateCheck.ToDateTime()))
                {
                    var date = lastDay.DateEnd.ToDateTime();
                    interval += this.GetInteval(new DateTime(date.Year, date.Month, date.Day, 9, 0, 0, 0), date);
                }

                var hours = (int) interval.TotalHours;
                var workDays = hours / 8;
                hours = hours % 8;

                if (hours == 0)
                {
                    this.Report["ОбщаяПродолжительностьРабДни"] = "{0} дней".FormatUsing(workDays);
                }
                else
                {
                    this.Report["ОбщаяПродолжительностьРабДни"] = "{0} дней, {1} часов".FormatUsing(workDays, hours);
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

                this.Report["ПродолжительностьПроверкиДЧ"] = checkTime.Days > 0
                    ? string.Format("{0} д. {1} ч.", checkTime.Days, hours)
                    : string.Format("{0} ч.", hours);
            }
        }

        private void FillActCheckViolations()
        {
            var actRoDomain = this.Container.ResolveDomain<ActCheckRealityObject>();
            var actViolStageDomain = this.Container.ResolveDomain<ActCheckViolation>();
            var normDocItemDomain = this.Container.ResolveDomain<ViolationNormativeDocItemGji>();
            var roLongTextDomain = this.Container.ResolveDomain<ActCheckRoLongDescription>();
            var violLongTextDomain = this.Container.ResolveDomain<ActCheckViolationLongText>();

            try
            {
                // Дома акта проверки
                var actRobjects = actRoDomain.GetAll()
                    .Where(x => x.ActCheck.Id == this.DocumentId)
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
                    this.Report["НеВыявлено"] = actRobjects.AggregateWithSeparator(x => x.Description, "; ");
                    this.Report["НарушенияВыявлены"] = "Нет";
                }
                else
                {
                    this.Report["НарушенияВыявлены"] = "Да";
                }

                this.Report["НевыявленныеНарушения"] = actRobjects.AggregateWithSeparator(x => x.NotRevealedViolations, ", ");

                if (actRobjects.Length > 0)
                {
                    var firstRecord = actRobjects.First();

                    this.Report["Описание"] = this.GetViolationDescription(firstRecord.Id);

                    var firstRo = firstRecord.RealityObject;

                    if (firstRo != null)
                    {
                        this.Report["НомерДома"] = firstRo.FiasAddress.House;
                        this.Report["АдресДома"] = firstRo.FiasAddress.PlaceName + ", " + firstRo.FiasAddress.StreetName;
                        this.Report["НаселенныйПункт"] = firstRo.FiasAddress.PlaceName;

                        if (actRobjects.Length == 1)
                        {
                            this.Report["УлицаДом"] =
                                string.Format("{0}, {1}, д.{2}{3} ",
                                    firstRo.FiasAddress.PlaceName,
                                    firstRo.FiasAddress.StreetName,
                                    firstRo.FiasAddress.House,
                                    this.GetHousing(firstRo.FiasAddress.Housing));

                            this.Report["ДомАдрес"] = firstRo.FiasAddress.AddressName;
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
                                    this.GetHousing(ro.FiasAddress.Housing));
                            }

                            this.Report["УлицаДом"] = string.Format("{0}.", realObjs);
                            this.Report["ДомаИАдреса"] = string.Format("{0}, {1}. ", firstRo.FiasAddress.PlaceName, realObjs);
                        }
                    }
                }

                
                var sum = actRobjects.Select(x => x.RealityObject.Return(z => z.AreaMkd)).Sum() ?? 0;
                this.Report["ОбщаяПлощадьСумма"] = sum.RoundDecimal(2).ToString(CultureInfo.InvariantCulture);

                // Нарушения
                var violData =
                    actViolStageDomain.GetAll()
                        .Where(x => x.Document.Id == this.DocumentId)
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

                this.Report["ТекстНарушения"] = violData.Select(x => x.Name).Distinct().AggregateWithSeparator(", ");

                this.Report["СсылкиНаПунктыНормативныхАктов"] = violData
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

                this.Report["ПереченьНПД"] = normDocs
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
                this.Report.RegData("ДомаПроверки", violData
                    .Distinct(x => x.ActObjectId)
                    .Where(o => o.RealityObject != null)
                    .Select(z =>
                    {
                        var ro = z.RealityObject;

                        return new
                        {
                            Адрес = ro.Address,
                            ХарактеристикиТП = this.GetRobjectChars(ro),
                            ДопХарактеристики = addChars.Get(z.ActObjectId)
                        };
                    }));
            }
            finally
            {
                this.Container.Release(actRoDomain);
                this.Container.Release(actViolStageDomain);
                this.Container.Release(normDocItemDomain);
                this.Container.Release(roLongTextDomain);
                this.Container.Release(violLongTextDomain);
            }
        }

        private ChelyabinskDisposal FillParentDisposal(ChelyabinskActCheck act)
        {
            var disposal = this.GetParentDocument(act, TypeDocumentGji.Disposal) as ChelyabinskDisposal;

            if(disposal == null) return null;

            var expertDomain = this.Container.ResolveDomain<DisposalExpert>();
            var typeSurveyDomain = this.Container.ResolveDomain<DisposalTypeSurvey>();
            var typeSurveyGoalDomain = this.Container.ResolveDomain<TypeSurveyGoalInspGji>();
            var subjDomain = this.Container.ResolveDomain<DisposalVerificationSubject>();

            try
            {
                this.Report["ДатаРаспоряжения"] = disposal.DocumentDate.ToDateString("d MMMM yyyy");
                this.Report["НомерРаспоряжения"] = disposal.DocumentNumber;

                this.Report["НомерРешенияПрокурора"] = disposal.ProsecutorDecNumber;
                this.Report["ДатаРешенияПрокурора"] = disposal.ProsecutorDecDate.ToDateString();

                this.Report["СогласованиеСПрокуратурой"] = disposal.TypeAgreementProsecutor.GetEnumMeta().Display;

                this.Report["ОснованиеОбследования"] = this.GetDocumentBase(disposal);

                if (!disposal.KindCheck.Return(x => x.Name).IsEmpty())
                {
                    this.Report["ВидПроверки"] = disposal.KindCheck.Name;
                    this.Report["ВидОбследования"] = disposal.KindCheck.Name;
                    this.Report["ВидОбследованияРП"] = this.GetMorpher().Проанализировать(disposal.KindCheck.Name).Родительный;
                }

                if (disposal.IssuedDisposal != null)
                {
                    var issued = disposal.IssuedDisposal;

                    if (!string.IsNullOrEmpty(issued.PositionGenitive))
                    {
                        this.Report["КодИнспектораРП"] = issued.PositionGenitive.ToLower();
                        this.Report["КодИнспектораТП"] = issued.PositionAblative.ToLower();
                    }

                    this.Report["РуководительРП"] = issued.FioGenitive;
                    this.Report["РуководительТП"] = issued.FioAblative;

                    this.Report["КодРуководителяФИО"] =
                        string.Format("{0} - {1}",
                            issued.Code,
                            this.Coalesce(issued.ShortFio, issued.Fio));
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

                    this.Report["ПродолжительностьПроверки"] = countDays.ToString(CultureInfo.InvariantCulture);
                }

                this.Report["Эксперты"] = expertDomain.GetAll()
                    .Where(x => x.Disposal.Id == disposal.Id)
                    .Select(x => x.Expert)
                    .AggregateWithSeparator(x => x.Name, ", ");

                this.Report["НачалоПериода"] = disposal.DateStart.ToDateString();
                this.Report["ОкончаниеПериода"] = disposal.DateEnd.ToDateString();

                var queryTypeSurveyIds = typeSurveyDomain.GetAll()
                    .Where(x => x.Disposal.Id == disposal.Id)
                    .Select(x => x.TypeSurvey.Id);

                var strGoalInspGji = typeSurveyGoalDomain.GetAll()
                    .Where(x => queryTypeSurveyIds.Contains(x.TypeSurvey.Id))
                    .AggregateWithSeparator(x => x.SurveyPurpose.Name, "; ");

                this.Report["Цель"] = strGoalInspGji;

                this.Report.RegData("ПредметыПроверки", subjDomain.GetAll()
                    .Where(x => x.Disposal.Id == disposal.Id)
                    .Select(x => new { Наименование = x.SurveySubject.Name })
                    .ToList());
            }
            finally
            {
                this.Container.Release(expertDomain);
                this.Container.Release(typeSurveyDomain);
                this.Container.Release(typeSurveyGoalDomain);
                this.Container.Release(subjDomain);
            }

            return disposal;
        }

        private void FillPrescription(ChelyabinskActCheck act)
        {
            var prescription = this.GetParentDocument(act, TypeDocumentGji.Prescription);

            if (prescription == null) return;

            var prescrViolDomain = this.Container.ResolveDomain<PrescriptionViol>();
            var actRemovalDomain = this.Container.ResolveDomain<ActRemovalViolation>();

            try
            {
                this.Report["НомерПредписания"] = prescription.DocumentNumber;
                this.Report["ДатаПредписания"] = prescription.DocumentDate.ToDateString();

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

                this.Report["НомераНевыполненныхПунктовПредписания"] =
                    viols.Where(x => !x.DateFactRemoval.HasValue)
                         .Select(x => x.Num.ToString())
                         .AggregateWithSeparator(", ");

                var actRemoval = this.GetChildDocument(prescription, TypeDocumentGji.ActRemoval);

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

                    this.Report.RegData("ВыполнениеПредписания", result);
                }
            }
            finally
            {
                this.Container.Release(prescrViolDomain);
                this.Container.Release(actRemovalDomain);
            }
        }

        private void FillInspection(InspectionGji inspection)
        {
            if(inspection == null) return;

            var contactDomain = this.Container.ResolveDomain<ContragentContact>();
            var licRequestDomain = this.Container.ResolveDomain<BaseLicenseApplicants>();
            try
            {
                this.Report["ОбъектПроверки"] = inspection.PersonInspection.GetEnumMeta().Display;

                if (!inspection.PhysicalPerson.IsEmpty())
                {
                    this.Report["ФИОФизлицаРП"] = this.GetMorpher().Проанализировать(inspection.PhysicalPerson).Родительный;
                }
                Contragent contragent = null;
                if (inspection.TypeBase == TypeBase.LicenseApplicants)
                {
                    var licRequest = licRequestDomain.Get(inspection.Id);
                    contragent = licRequest.ManOrgLicenseRequest.Contragent;
                }
                else
                {
                    contragent = inspection.Contragent;
                }

                if (contragent == null){ return; }
                else
                {
                    this.Report["УправОрг"] = contragent.Name;
                    this.Report["ИННУправОрг"] = contragent.Inn;

                    if (contragent.OrganizationForm != null)
                    {
                        this.Report["ТипЛица"] = contragent.OrganizationForm.Code != "91"
                            ? "юридического лица"
                            : "индивидуального предпринимателя";
                    }

                    this.Report["УправОргРП"] = contragent.NameGenitive.IsNotEmpty()
                        ? contragent.NameGenitive
                        : contragent.Name;
                    this.Report["СокрУправОрг"] = contragent.ShortName;

                    if (contragent.FiasJuridicalAddress != null)
                    {
                        this.Report["ЮрАдресКонтрагента"] = this.GetAddress(contragent.FiasJuridicalAddress);
                    }
                    else
                    {
                        this.Report["ЮрАдресКонтрагента"] = contragent.JuridicalAddress;
                    }

                    if (contragent.FiasFactAddress != null)
                    {
                        this.Report["АдресКонтрагентаФакт"] = contragent.FiasFactAddress != null
                            ? this.GetAddress(contragent.FiasFactAddress)
                            : string.Empty;
                    }
                    else
                    {
                        this.Report["АдресКонтрагентаФакт"] = contragent.FiasFactAddress.Return(x => x.AddressName);
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
                        this.Report["РуководительЮЛДолжность"] =
                            string.Format("{0} {1} {2} {3}",
                                headContragent.Position.Return(x => x.Name),
                                headContragent.Surname,
                                headContragent.Name,
                                headContragent.Patronymic);

                        this.Report["РуководительЮЛ"] = string.Format("{0} {1} {2}", headContragent.Surname,
                            headContragent.Name, headContragent.Patronymic);
                    }
                }
            }
            finally
            {
                this.Container.Release(contactDomain);
            }
        }

        private void FillProvidedDoc(ChelyabinskActCheck act, Disposal disposal)
        {
            if (act == null || disposal == null) return;

            var actProvDocDomain = this.Container.ResolveDomain<ActCheckProvidedDoc>();
            var disposalProvDocDomain = this.Container.ResolveDomain<DisposalProvidedDoc>();

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

                this.Report.RegData("ПредоставляемыеДокументы", disposalProvDocs);
                this.Report["ПредоставляемыеДокументыСтрокой"] =
                    disposalProvDocs.AggregateWithSeparator(x => x.Наименование, ", ");

                var listProvDocs = actProvDocs
                    .Where(x => disposalProvDocs.Any(y => y.Наименование == x.Наименование))
                    .ToList();

                this.Report.RegData("ПредоставленныеДокументы", listProvDocs);
                this.Report["ПредоставленныеДокументыСтрокой"] = 
                    listProvDocs.AggregateWithSeparator(x => x.Наименование, ", ");

                var otherProvDocs = actProvDocs
                    .Where(x => disposalProvDocs.All(y => y.Наименование != x.Наименование))
                    .ToList();

                this.Report.RegData("ДопПредоставленныеДокументы", otherProvDocs);
                this.Report["ДопПредоставленныеДокументыСтрокой"] =
                    otherProvDocs.AggregateWithSeparator(x => x.Наименование, ", ");
            }
            finally
            {
                this.Container.Release(actProvDocDomain);
                this.Container.Release(disposalProvDocDomain);
            }
        }


        private string GetViolationDescription(long actCheckRoId)
        {
            string result;

            var domainLongText = this.Container.ResolveDomain<ActCheckRoLongDescription>();
            using (this.Container.Using(domainLongText))
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
            this.DocumentId = userParamsValues.GetValue<object>("DocumentId").ToLong();
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

            var startLunch = this.ChangeTime(dateStart.ToDateTime(), 12, 0);
            var endLunch = this.ChangeTime(dateStart.ToDateTime(), 13, 0);

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
                {TypeCheck.PlannedExit, "плановой"},
                {TypeCheck.NotPlannedExit, "внеплановой"},
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