namespace Bars.GkhGji.Regions.BaseChelyabinsk.Report.Prescription
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Report;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.Dict;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Prescription;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Properties;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Disposal;
    using Bars.GkhGji.Report;

    using Stimulsoft.Report;
    using Stimulsoft.Report.Export;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Отчет "Предписание"
    /// </summary>
    public class PrescriptionGjiStimulReport : GjiBaseStimulReport
    {
        private long DocumentId { get; set; }

		/// <summary>
		/// Конструктор
		/// </summary>
        public PrescriptionGjiStimulReport()
            : base(new ReportTemplateBinary(Resources.ChelyabinskPrescription))
        {
        }
        
		/// <summary>
		/// Формат экспорта
		/// </summary>
        public override StiExportFormat ExportFormat => StiExportFormat.Word2007;

        /// <summary>
		/// Настройки экспорта
		/// </summary>
		public override StiExportSettings ExportSettings => new StiWord2007ExportSettings
		{
		    RemoveEmptySpaceAtBottom = false
		};

        /// <summary>
		/// Идентификатор отчета
		/// </summary>
		public override string Id => "ChelyabinskPrescriptionGji";

        /// <summary>
		/// Код формы
		/// </summary>
        public override string CodeForm => "Prescription";

        /// <summary>
		/// Наименование
		/// </summary>
        public override string Name => "Предписание";

        /// <summary>
		/// Описание
		/// </summary>
        public override string Description => "Предписание";

        /// <summary>
        /// Расширение
        /// </summary>
        public override string Extention => "mrt";

        /// <summary>
        /// Генератор отчетов
        /// </summary>
        public override string ReportGeneratorName => "StimulReportGenerator";

        /// <summary>
        /// Код шаблона
        /// </summary>
        protected override string CodeTemplate { get; set; }

        /// <summary>
        /// Установить пользовательские параметры
        /// </summary>
        /// <param name="userParamsValues">Значения пользовательских параметров</param>
        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            this.DocumentId = userParamsValues.GetValue<object>("DocumentId").ToLong();
        }

		/// <summary>
		/// Получить информацию о шаболне
		/// </summary>
		/// <returns></returns>
        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
                       {
                           new TemplateInfo
                               {
                                   Code = "ChelyabinskPrescriptionGji",
                                   Name = "Prescription",
                                   Description = "Предписание",
                                   Template = Resources.ChelyabinskPrescription
                               }
                       };
        }

		/// <summary>
		/// Подготовить отчет
		/// </summary>
		/// <param name="reportParams">Параметры отчета</param>
        public override void PrepareReport(ReportParams reportParams)
        {
            
            var baseJurPersonDomain = this.Container.ResolveDomain<BaseJurPerson>();
            var prescriptionDomain = this.Container.ResolveDomain<ChelyabinskPrescription>();
            var inspectionAppCitDomain = this.Container.ResolveDomain<InspectionAppealCits>();
            var disposalDomain = this.Container.ResolveDomain<Disposal>();
            var documentGjiInspectorDomain = this.Container.ResolveDomain<DocumentGjiInspector>();
            var inspectionRoDomain = this.Container.ResolveDomain<InspectionGjiRealityObject>();
            var prescrViolDomain = this.Container.ResolveDomain<PrescriptionViol>();
            var docInspectorDomain = this.Container.ResolveDomain<DocumentGjiInspector>();
            var childrenDomain = this.Container.ResolveDomain<DocumentGjiChildren>();
            var dispVerSubjectDomain = this.Container.ResolveDomain<DisposalVerificationSubject>();
            var prescActivDirectDomain = this.Container.ResolveDomain<PrescriptionActivityDirection>();
            var prescBaseDocDomain = this.Container.ResolveDomain<PrescriptionBaseDocument>();
            var violNormativeDocItemDomain = this.Container.ResolveDomain<ViolationNormativeDocItemGji>();
            var actCheckViolationDomain = this.Container.ResolveDomain<ActCheckViolation>();
            var zonalDomain = this.Container.ResolveDomain<ZonalInspectionInspector>();

            try
            {
                var prescription = prescriptionDomain.GetAll().FirstOrDefault(x => x.Id == this.DocumentId);

                if (prescription == null)
                    return;

                if (prescription.Stage == null)
                    return;

                if (prescription.Stage.Parent == null)
                    return;

                var disposal = disposalDomain.GetAll()
                    .FirstOrDefault(x => x.Stage.Id == prescription.Stage.Parent.Id);

                if (disposal == null)
                    return;
                
                this.FillCommonFields(prescription);

                this.Report["Id"] = this.DocumentId;
                this.Report["ДатаПредписания"] = prescription.DocumentDate.HasValue
                    ? prescription.DocumentDate.Value.ToString("D", new CultureInfo("ru-RU"))
                    : string.Empty;

                this.Report["НомерПредписания"] = prescription.DocumentNumber;

                this.Report["ФизическоеЛицо"] = prescription.PhysicalPerson;

                this.Report["РеквизитыФизлица"] = prescription.PhysicalPersonInfo;
                this.Report["МестоСоставления"] = prescription.DocumentPlace;
                this.Report["ВремяCоставления"] = prescription.DocumentTime.HasValue ? prescription.DocumentTime.Value.ToString("HH-mm") : "";
                this.Report["ТипИсполнителя"] = prescription.Executant != null ? prescription.Executant.Name : "";
                try
                {
                    switch (disposal.KindCheck.Code)
                    {
                  

                        case TypeCheck.NotPlannedExit:
                            this.Report["ВидПроверкиВходе"] = "проведена внеплановая выездная проверка, в ходе которой";
                            this.Report["ВидПроверки"] = "проведенной внеплановой выездной проверки";
                            break;

        
                        case TypeCheck.PlannedExit:
                            this.Report["ВидПроверкиВходе"] = "проведена плановая выездная проверка, в ходе которой";
                            this.Report["ВидПроверки"] = "проведенной плановой выездной проверки";
                            break;

                    }

                    switch (disposal.TypeDisposal)
                    {
                        case TypeDisposalGji.Base:
                            if (disposal.Inspection.TypeBase == TypeBase.CitizenStatement)
                            {
                                var appCit = inspectionAppCitDomain.GetAll().Where(x => x.Inspection == disposal.Inspection).Select(x => x.AppealCits).FirstOrDefault();
                                this.Report["ОснованиеПроверки"] = $"по обращению от {appCit.DateFrom.Value.ToString("dd.MM.yyyy")} №{appCit.NumberGji}";
                            }
                            else if (disposal.Inspection.TypeBase == TypeBase.PlanJuridicalPerson)
                            {
                                var вaseJurPerson = baseJurPersonDomain.GetAll().Where(x => x.Id == disposal.Inspection.Id).Select(x => x.Plan).FirstOrDefault();
                                this.Report["ОснованиеПроверки"] = $"в соответствии с планом мероприятий {вaseJurPerson.Name}";
                            }
                            break;


                    }
                }
                catch
                { }

                this.FillContragentInfo(prescription);

                if (prescription.Executant != null)
                {
                    var shortName = prescription.Contragent != null ? prescription.Contragent.ShortName : string.Empty;
                    var physicalPerson = prescription.PhysicalPerson;

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
                            this.Report["КомуВыдано"] = shortName;
                            break;
                        case "1":
                        case "3":
                        case "5":
                        case "11":
                        case "13":
                        case "16":
                        case "18":
                        case "19":
                            this.Report["КомуВыдано"] = shortName + ", " + physicalPerson;
                            break;
                        case "8":
                        case "9":
                        case "14":
                            this.Report["КомуВыдано"] = physicalPerson;
                            break;
                    }
                }

                var normativeDocDict = violNormativeDocItemDomain
                        .GetAll()
                        .Where(x => prescrViolDomain.GetAll()
                            .Any(y => y.Document.Id == prescription.Id && y.InspectionViolation.Violation.Id == x.ViolationGji.Id))
                        .Select(x => new
                        {
                            x.ViolationGji.Id,
                            x.NormativeDocItem.Number,
                            x.NormativeDocItem.NormativeDoc.Name
                        })
                        .AsEnumerable()
                        .GroupBy(x => x.Id)
                        .ToDictionary(x => x.Key, y => y.Select(x => x.Number).Union(y.Select(x => x.Name)).AggregateWithSeparator(", "));

                var violations = prescrViolDomain.GetAll()
                                      .Where(x => x.Document.Id == prescription.Id)
                                      .Select(x => new
                                      {
                                          ViolationId = x.InspectionViolation.Violation.Id,
                                          x.InspectionViolation.Violation.CodePin,
                                          x.Action,
                                          DatePlanRemoval = x.DatePlanRemoval.HasValue? x.DatePlanRemoval.Value: x.InspectionViolation.DatePlanRemoval.Value,
                                          Address = x.InspectionViolation.RealityObject != null ? x.InspectionViolation.RealityObject.Municipality.Name + ", " + x.InspectionViolation.RealityObject.Address : null,
                                      })
                                      // Сортировка должна быть такая же как в печатке Акт проверки предписания
                                      .OrderBy(x => x.DatePlanRemoval)
                                      .ThenBy(x => x.CodePin)
                                      .ThenBy(x => x.Action)
                                      .ToList();

                var dataViol = new List<PrescriptionViolRecord>();
                foreach (var viol in violations)
                {
                   dataViol.Add( new PrescriptionViolRecord
                            {
                                Адрес = viol.Address,
                                ПунктыНПА = normativeDocDict.Get(viol.ViolationId) ?? string.Empty,
								Мероприятие = viol.Action,
                                СрокМероприятия = viol.DatePlanRemoval != null
                                                    ? viol.DatePlanRemoval.ToShortDateString()
                                                    : string.Empty
                            });
                }

                this.Report.RegData("Нарушения", dataViol);

                var insp = docInspectorDomain.GetAll()
                    .Where(x => x.DocumentGji.Id == this.DocumentId)
                    .Select(x => new
                    {
                        x.Inspector.Fio,
                        x.Inspector.Position,
                        x.Inspector.ShortFio,
                        x.Inspector.FioAblative
                    })
                    .ToArray();

                if (insp.Length > 0)
                {
                    var firstInsp = insp.First();
                    this.Report["ДолжностьИнспектора"] = firstInsp.Position;

                    if (!string.IsNullOrEmpty(firstInsp.ShortFio))
                    {
                        this.Report["ИнспекторФамИО"] = firstInsp.ShortFio;
                    }
                }

                var actDoc = childrenDomain.GetAll()
                .Where(x => x.Children.Id == prescription.Id && x.Parent.TypeDocumentGji == TypeDocumentGji.ActCheck)
                .Select(x => x.Parent)
                .FirstOrDefault();

                if (actDoc != null)
                {
                    try
                    {
                        var docInspector = documentGjiInspectorDomain.GetAll()
                            .Where(x => x.DocumentGji.Id == actDoc.Id).Select(x=> x.Inspector).FirstOrDefault();
                        var zonal = zonalDomain.GetAll().Where(x => x.Inspector != null && x.Inspector == docInspector).FirstOrDefault();
                        Report["Инспектор_и_Должность"] = docInspector.PositionAblative + (zonal != null ? " " + zonal.ZonalInspection.NameGenetive : "") + " " + docInspector.FioAblative;
                        Report["ОтделЗЖИ"] = zonal != null ? zonal.ZonalInspection.Name : "";
                        Report["МаксДата"] = violations.Max(x => x.DatePlanRemoval).ToString("dd.MM.yyyy");
                        Report["ИнспекторФИО"] =docInspector.Fio;
                        Report["ИнспекторДолжностьИМесто"] = docInspector.Position + (zonal != null ? " " + zonal.ZonalInspection.NameGenetive : "");
                    }
                    catch
                    {

                    }
                    this.Report["НомерАктаПроверки"] = actDoc.DocumentNumber;
                    this.Report["ДатаАктаПроверки"] = actDoc.DocumentDate.HasValue ? actDoc.DocumentDate.Value.ToShortDateString() : string.Empty;


                    var actCheckNormativeDocs = violNormativeDocItemDomain
                        .GetAll()
                        .Where(x => actCheckViolationDomain.GetAll()
                            .Any(y => y.Document.Id == actDoc.Id && y.InspectionViolation.Violation.Id == x.ViolationGji.Id))
                        .Select(x => new
                        {
                            ПолноеНаименование = x.NormativeDocItem.NormativeDoc.FullName,
                            СокращенноеНаименование = x.NormativeDocItem.NormativeDoc.Name,
                        })
                        .Distinct()
                        .ToList();

                    this.Report.RegData("ПереченьНПА", actCheckNormativeDocs);
                }

                var realityObj = prescrViolDomain.GetAll()
                                      .Where(x => x.Document.Id == prescription.Id)
                                      .Where(x => x.InspectionViolation.RealityObject != null)
                                      .Select(x => x.InspectionViolation.RealityObject)
                                      .ToList()
                                      .Distinct(x => x.Id);

                // если дом один, то выводим адрес дома и номер дома, если домов нет или больше 1 - ничего не выводим
                if (realityObj.Any())
                {
                    var firstPrescriptionRo = realityObj.FirstOrDefault();

                    if (firstPrescriptionRo != null)
                    {
                        if (firstPrescriptionRo.FiasAddress != null)
                        {
                            var fias = firstPrescriptionRo.FiasAddress;

                            this.Report["НаселенныйПункт"] = fias.PlaceName;
                            this.Report["НомерДома"] = fias.House;

                            if (!string.IsNullOrEmpty(fias.Housing))
                            {
                                this.Report["НомерДома"] = fias.House + ", корп. " + fias.Housing;   
                            }

                            this.Report["Улица"] = fias.StreetName;
                        }
                    }
                }

                var surveySubjects = dispVerSubjectDomain.GetAll()
                    .Where(x => x.Disposal.Id == disposal.Id)
                    .Select(x => new { Наименование = x.SurveySubject.Name } )
                    .ToList();

                this.Report.RegData("ПредметыПроверки", surveySubjects);

                var activityDirections = prescActivDirectDomain.GetAll()
                    .Where(x => x.Prescription.Id == prescription.Id)
                    .Select(x => new { Наименование = x.ActivityDirection.Name })
                    .ToList();

                this.Report.RegData("НаправленияДеятельности", activityDirections);

                var prescBaseDocs = prescBaseDocDomain.GetAll()
                    .Where(x => x.Prescription.Id == prescription.Id)
                    .Select(x => new
                    {
                        KindBaseDocument = x.KindBaseDocument.Name,
                        x.NumDoc,
                        x.DateDoc
                    })
                    .ToList()
                    .Select(x => new
                    {
                        ДокументОснование = x.KindBaseDocument,
                        ДатаДокумента = x.DateDoc.HasValue ? x.DateDoc.Value.ToShortDateString() : string.Empty,
                        НомерДокумента  = x.NumDoc
                    })
                    .ToList();

                this.Report.RegData("ДокументыОснования", prescBaseDocs);
            }
            finally 
            {
                this.Container.Release(prescriptionDomain);
                this.Container.Release(disposalDomain);
                this.Container.Release(inspectionRoDomain);
                this.Container.Release(prescrViolDomain);
                this.Container.Release(docInspectorDomain);
                this.Container.Release(childrenDomain);
                this.Container.Release(dispVerSubjectDomain);
                this.Container.Release(prescActivDirectDomain);
                this.Container.Release(prescBaseDocDomain);
                this.Container.Release(violNormativeDocItemDomain);
                this.Container.Release(actCheckViolationDomain);
            }

        }

        private void FillContragentInfo(Prescription prescription)
        {
            var contragent = prescription.Contragent;

            if (contragent != null)
            {                                  
                this.Report["ИНН"] = contragent.Inn;
                this.Report["КПП"] = contragent.Kpp;

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

                    this.Report["АдресКонтрагента"] = newAddr.ToString();
                }
                else
                {
                    this.Report["АдресКонтрагента"] = contragent.JuridicalAddress;
                }
            }
        }


        protected class PrescriptionViolRecord
        {
            public string Адрес { get; set; }

            public string ПунктыНПА { get; set; }

            public string СрокМероприятия { get; set; }

            public string Мероприятие { get; set; }
        }
    }

}