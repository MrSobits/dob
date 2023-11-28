namespace Bars.GkhGji.Regions.Nso.Report
{
    using System.Collections.Generic;
    using System.Linq;
    using B4.DataAccess;
    using Bars.B4.Modules.Reports;
    using Bars.B4.IoC;
    using B4.Utils;
    using Entities;
    using Entities.Disposal;
    using Gkh.Report;
    using Gkh.Utils;
    using GkhGji.Entities;
    using GkhGji.Enums;
    using GkhGji.Report;
    using Stimulsoft.Report;
    using Stimulsoft.Report.Export;

	/// <summary>
	/// Отчет для приказа
	/// </summary>
	public class NsoDisposalStimulReport : GjiBaseStimulReport
    {
        #region .ctor

		/// <summary>
		/// Конструктор
		/// </summary>
        public NsoDisposalStimulReport() : base(new ReportTemplateBinary(Properties.Resources.NsoDisposal))
        {
        }

        #endregion .ctor

        #region Properties

        /// <summary>
        /// Наименование отчета
        /// </summary>
        public override string Name
        {
            get { return "Приказ"; }
        }

        /// <summary>
        /// Описание отчета
        /// </summary>
        public override string Description
        {
            get { return "Приказ"; }
        }

        /// <summary>
        /// Код шаблона (файла)
        /// </summary>
        protected override string CodeTemplate { get; set; }

        /// <summary>
        /// Идентификатор отчета
        /// </summary>
        public override string Id
        {
            get { return "NsoDisposal"; }
        }

        /// <summary>
        /// Код формы, на которой находится кнопка печати
        /// </summary>
        public override string CodeForm
        {
            get { return "Disposal"; }
        }

		/// <summary>
		/// Формат экспорта
		/// </summary>
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

		#endregion Properties

		private long DocumentId { get; set; }

        /// <summary>
        /// Установить пользовательские параметры
        /// </summary>
        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            DocumentId = userParamsValues.GetValue<object>("DocumentId").ToLong();
        }

        /// <summary>
        /// Получить список шаблонов
        /// </summary>
        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
            {
                new TemplateInfo
                {
                    Code = "NsoDisposal",
                    Name = "NsoDisposal",
                    Description = "Приказ",
                    Template = Properties.Resources.NsoDisposal
                }
            };
        }

        /// <summary>
        /// Подготовить параметры отчета
        /// </summary>
        public override void PrepareReport(ReportParams reportParams)
        {
            var disposalDomain = Container.ResolveDomain<NsoDisposal>();

            var disposal = disposalDomain.Get(DocumentId);

            if (disposal == null) return;

            FillCommonFields(disposal);

            if (!disposal.KindCheck.Return(x => x.Name).IsEmpty())
            {
                Report["ВидПроверкиРП"] = GetMorpher().Проанализировать(disposal.KindCheck.Name).Родительный;
            }

            Report["ДатаПриказа"] = disposal.DocumentDate.ToDateString("«dd» MMMM yyyy г.");
            Report["НомерПриказа"] = disposal.DocumentNumber;

            Report["СогласованиеСПрокуратурой"] = disposal.TypeAgreementProsecutor.GetEnumMeta().Display;

            Report["ВремяС"] = disposal.TimeVisitStart.ToTimeString("HH час. mm мин.");
            Report["ВремяПо"] = disposal.TimeVisitEnd.ToTimeString("HH час. mm мин.");

            Report["ПериодПроведенияПроверкиС"] = disposal.DateStart.ToDateString("«dd» MMMM yyyy г.");
            Report["ПериодПроведенияПроверкиПо"] = disposal.DateEnd.ToDateString("«dd» MMMM yyyy г.");

            Report["ДЛВынесшееПриказ"] = disposal.IssuedDisposal.Return(x => x.Fio);
            Report["ДЛВынесшееПриказДолжность"] = disposal.IssuedDisposal.Return(x => x.Position);

            Report["ОтветственныйЗаВыполнение"] = disposal.ResponsibleExecution.Return(x => x.Fio);
            Report["ОтветственныйЗаВыполнениеТелефон"] = disposal.ResponsibleExecution.Return(x => x.Phone);
            Report["ОтветственныйЗаВыполнениеДолжность"] = disposal.ResponsibleExecution.Return(x => x.Position);
            Report["ОтветственныйЗаВыполнениеЭлектроннаяПочта"] = disposal.ResponsibleExecution.Return(x => x.Email);

            Report["ТипКонтрагента"] = disposal.Return(x => x.Inspection).Return(x => x.TypeJurPerson.GetEnumMeta().Display);

            Report["ОснованиеОбследования"] = GetDocumentBase(disposal);
            Report.RegData("ФактыНарушений", "ФактыНарушений", GetFactViolations(disposal));

            FillDisposalSubentities(disposal);

            FillParentPrescription(disposal);

            FillInspection(disposal);
        }

        private void FillDisposalSubentities(Disposal disposal)
        {
            var expertDomain = Container.ResolveDomain<DisposalExpert>();
            var docInspectorDomain = Container.ResolveDomain<DocumentGjiInspector>();
            var disposalTypeSurveyDomain = Container.ResolveDomain<DisposalTypeSurvey>();
            var typesInspFoundationDomain = Container.ResolveDomain<TypeSurveyInspFoundationGji>();
            var typesGoalsDomain = Container.ResolveDomain<TypeSurveyGoalInspGji>();
            var disposalProvDocsDomain = Container.ResolveDomain<NsoDisposalProvidedDoc>();
            var verifSubjectDomain = Container.ResolveDomain<DisposalVerificationSubject>();

            try
            {
                var experts = expertDomain.GetAll()
                    .Where(x => x.Disposal.Id == disposal.Id)
                    .Select(x => new
                    {
                        ФИО = x.Expert.Name,
                        Должность = x.Expert.Code
                    })
                    .ToList();

                Report.RegData("Эксперты", "Эксперты", experts);

                var inspectors = docInspectorDomain.GetAll()
                    .Where(x => x.DocumentGji.Id == disposal.Id)
                    .Select(x => new
                    {
                        ФИО = x.Inspector.FioGenitive,
                        Должность = x.Inspector.PositionGenitive
                    })
                    .ToList();

                Report.RegData("Инспекторы", "Инспекторы", inspectors);

                var typeIds = disposalTypeSurveyDomain.GetAll()
                    .Where(x => x.Disposal.Id == disposal.Id)
                    .Select(x => x.TypeSurvey.Id)
                    .ToArray();

                var foundations = typesInspFoundationDomain.GetAll()
                    .Where(x => typeIds.Contains(x.TypeSurvey.Id))
                    .Select(x => new { Наименование = x.NormativeDoc.Name })
                    .Distinct()
                    .ToList();

                Report.RegData("ПравовыеОснованияПроверки", "ПравовыеОснованияПроверки", foundations);

                var goals = typesGoalsDomain.GetAll()
                    .Where(x => typeIds.Contains(x.TypeSurvey.Id))
                    .Select(x => new { Наименование = x.SurveyPurpose.Name })
                    .ToList();

                Report.RegData("ЗадачиПроверки", "ЗадачиПроверки", goals);

                var provDocs = disposalProvDocsDomain.GetAll()
                    .Where(x => x.Disposal.Id == disposal.Id)
					.OrderBy(x => x.Order)
                    .Select(x => new { Наименование = x.ProvidedDoc.Name })
                    .ToList();

                Report.RegData("ПредоставляемыеДокументы", "ПредоставляемыеДокументы", provDocs);

                var verifSubjs = verifSubjectDomain.GetAll()
                    .Where(x => x.Disposal.Id == disposal.Id)
                    .Select(x => new { Наименование = x.SurveySubject.Name })
                    .ToList();

                Report.RegData("ПредметыПроверки", "ПредметыПроверки", verifSubjs);
            }
            finally
            {
                Container.Release(expertDomain);
                Container.Release(docInspectorDomain);
                Container.Release(disposalTypeSurveyDomain);
                Container.Release(typesInspFoundationDomain);
                Container.Release(typesGoalsDomain);
                Container.Release(disposalProvDocsDomain);
            }
        }

        private void FillParentPrescription(Disposal disposal)
        {
            var prescription = GetParentDocument(disposal, TypeDocumentGji.Prescription);

            if (prescription != null)
            {
                Report["НомерПредписания"] = prescription.DocumentNumber;
                Report["ДатаПредписания"] = prescription.DocumentDate.ToDateString();
            }
        }

        private void FillInspection(DocumentGji document)
        {
            if (document.Inspection == null) return;

            var inspectionDomain = Container.ResolveDomain<InspectionGji>();
            var inspectionAppealsDomain = Container.ResolveDomain<InspectionAppealCits>();
            var baseJurPersonDomain = Container.ResolveDomain<BaseJurPerson>();
            var baseProsClaimDomain = Container.ResolveDomain<BaseProsClaim>();
            //т.к. у документа гжи lazy ссылка на проверку
            var inspection = inspectionDomain.Get(document.Inspection.Id);

            if (inspection == null) return;

            FillInspectionSubentities(inspection);

            Report["ФИО"] = inspection.PhysicalPerson;
            Report["ОбъектПроверки"] = inspection.PersonInspection.GetEnumMeta().Display;

            if (inspection.Contragent != null)
            {
                Report["ИНН"] = inspection.Contragent.Inn;
                Report["ЮридическийАдрес"] = inspection.Contragent.FiasJuridicalAddress != null ? inspection.Contragent.FiasJuridicalAddress.AddressName : inspection.Contragent.AddressOutsideSubject;
                Report["ЮридическоеЛицоРП"] = inspection.Contragent.NameGenitive;
                Report["ТипПредпринимательства"] = inspection.Contragent.TypeEntrepreneurship.GetEnumMeta().Display;
            }

            Report["ТипЮридическогоЛица"] = inspection.TypeJurPerson.GetEnumMeta().Display;

            var baseJurPerson = baseJurPersonDomain.Get(inspection.Id);
            if (baseJurPerson != null)
            {
                Report["ПланПроверокНомер"] = baseJurPerson.Plan.Return(x => x.NumberDisposal);
                Report["ПланПроверокДата"] = baseJurPerson.Plan.Return(x => x.DateDisposal).ToDateString();
            }

            var baseProsClaim = baseProsClaimDomain.Get(inspection.Id);
            if (baseProsClaim != null)
            {
                Report["ДокументНаименование"] = baseProsClaim.DocumentName;
                Report["ДокументНомер"] = baseProsClaim.DocumentNumber;
                Report["ДокументДата"] = baseProsClaim.DocumentDate.ToDateString();
            }

            var appeal = inspectionAppealsDomain.GetAll()
                .Where(x => x.Inspection.Id == inspection.Id)
                .Select(x => x.AppealCits)
                .FirstOrDefault();

            if (appeal != null)
            {
                Report["ТипКорреспондента"] = appeal.TypeCorrespondent.GetEnumMeta().Display;
                Report["ДатаОбращения"] = appeal.DateFrom.ToDateString();
                Report["НомерОбращения"] = appeal.NumberGji;
            }
        }

        private void FillInspectionSubentities(InspectionGji inspection)
        {
            if (inspection == null) return;

            var inspectionRoDomain = Container.ResolveDomain<InspectionGjiRealityObject>();

            try
            {
                Report.RegData("ПроверяемыеДома",
                    inspectionRoDomain.GetAll()
                        .Where(x => x.Inspection.Id == inspection.Id)
                        .Select(x => new
                        {
                            МунРайонИлиГорОкруг = x.RealityObject.Municipality.Name,
                            МунОбразование = x.RealityObject.MoSettlement.Name,
                            НаселенныйПункт = x.RealityObject.FiasAddress.PlaceName,
                            Улица = x.RealityObject.FiasAddress.StreetName,
                            НомерДома = x.RealityObject.FiasAddress.House,
                            НомерКвартиры = x.RoomNums
                        })
                        .ToArray());
            }
            finally
            {
                Container.Release(inspectionRoDomain);
            }
        }

        private List<ФактыНарушений> GetFactViolations(NsoDisposal disposal)
        {
            var serviceDisposalFactViolation = Container.ResolveDomain<DisposalFactViolation>();
            using(Container.Using(serviceDisposalFactViolation))
            {
                var factViols = serviceDisposalFactViolation.GetAll()
                    .Where(x => x.Disposal.Id == disposal.Id)
                    .Select(x => new ФактыНарушений { Наименование = x.TypeFactViolation.Name })
                    .ToList();

                return factViols;
            }
        }

        private class ФактыНарушений
        {
            public string Наименование { get; set; }
        }
    }
}