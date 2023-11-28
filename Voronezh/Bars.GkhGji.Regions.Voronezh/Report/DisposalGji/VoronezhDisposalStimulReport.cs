namespace Bars.GkhGji.Regions.Voronezh.Report
{
    using System.Collections.Generic;
    using System.Linq;
    using B4.DataAccess;
    using Bars.B4.Modules.Reports;
    using Bars.B4.IoC;
    using B4.Utils;

    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Disposal;
    
    using Gkh.Report;
    using Gkh.Utils;
    using GkhGji.Entities;
    using GkhGji.Enums;
    using GkhGji.Report;
    using Stimulsoft.Report;
    using Stimulsoft.Report.Export;
    using Gkh.Authentification;
    
    /// <summary>
    /// Отчет для приказа
    /// </summary>
    public class VoronezhDisposalStimulReport : GjiBaseStimulReport
    {
        #region .ctor

        /// <summary>
        /// Конструктор
        /// </summary>
        public VoronezhDisposalStimulReport() : base(new ReportTemplateBinary(Properties.Resources.ChelyabinskDisposal))
        {
        }

        #endregion .ctor

        #region Properties

        /// <summary>
        /// Наименование отчета
        /// </summary>
        public override string Name => "Материалы правонарушения";

        /// <summary>
        /// Описание отчета
        /// </summary>
        public override string Description => "Материалы правонарушения";

        /// <summary>
        /// Код шаблона (файла)
        /// </summary>
        protected override string CodeTemplate { get; set; }

        /// <summary>
        /// Идентификатор отчета
        /// </summary>
        public override string Id => "VoronezhDisposal";

        /// <summary>
        /// Код формы, на которой находится кнопка печати
        /// </summary>
        public override string CodeForm => "Disposal";

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
        #endregion Properties

        private long DocumentId { get; set; }

        /// <summary>
        /// Установить пользовательские параметры
        /// </summary>
        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            this.DocumentId = userParamsValues.GetValue<object>("DocumentId").ToLong();
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
                    Code = "ChelyabinskDisposal",
                    Name = "ChelyabinskDisposal",
                    Description = "Материалы правонарушения",
                    Template = Properties.Resources.ChelyabinskDisposal
                }
            };
        }

        /// <summary>
        /// Подготовить параметры отчета
        /// </summary>
        public override void PrepareReport(ReportParams reportParams)
        {
            var disposalDomain = this.Container.ResolveDomain<ChelyabinskDisposal>();

            try
            {
                var disposal = disposalDomain.Get(this.DocumentId);

                if (disposal == null)
                    return;

                this.FillCommonFields(disposal);

                if (!disposal.KindCheck.Return(x => x.Name).IsEmpty())
                {
                    this.Report["ВидПроверкиРП"] = this.GetMorpher().Проанализировать(disposal.KindCheck.Name).Родительный;
                }

                this.Report["Id"] = this.DocumentId;
                this.Report["ДатаПриказа"] = disposal.DocumentDate.ToDateString("«dd» MMMM yyyy г.");
                this.Report["НомерПриказа"] = disposal.DocumentNumber;

                this.Report["СогласованиеСПрокуратурой"] = disposal.TypeAgreementProsecutor.GetEnumMeta().Display;

                this.Report["ВремяС"] = disposal.TimeVisitStart.ToTimeString("HH час. mm мин.");
                this.Report["ВремяПо"] = disposal.TimeVisitEnd.ToTimeString("HH час. mm мин.");

                this.Report["ПериодПроведенияПроверкиС"] = disposal.DateStart.ToDateString("«dd» MMMM yyyy г.");
                this.Report["ПериодПроведенияПроверкиПо"] = disposal.DateEnd.ToDateString("«dd» MMMM yyyy г.");

                this.Report["ДЛВынесшееПриказ"] = disposal.IssuedDisposal.Return(x => x.Fio);
                this.Report["ДЛВынесшееПриказДолжность"] = disposal.IssuedDisposal.Return(x => x.Position);

                this.Report["ОтветственныйЗаВыполнение"] = disposal.ResponsibleExecution.Return(x => x.Fio);
                this.Report["ОтветственныйЗаВыполнениеТелефон"] = disposal.ResponsibleExecution.Return(x => x.Phone);
                this.Report["ОтветственныйЗаВыполнениеДолжность"] = disposal.ResponsibleExecution.Return(x => x.Position);
                this.Report["ОтветственныйЗаВыполнениеЭлектроннаяПочта"] = disposal.ResponsibleExecution.Return(x => x.Email);

                this.Report["ТипКонтрагента"] = disposal.Return(x => x.Inspection).Return(x => x.TypeJurPerson.GetEnumMeta().Display);

                this.Report["ОснованиеОбследования"] = this.GetDocumentBase(disposal);
                this.Report.RegData("ФактыНарушений", "ФактыНарушений", this.GetFactViolations(disposal));

                this.FillDisposalSubentities(disposal);

                this.FillParentPrescription(disposal);

                this.FillInspection(disposal);

                this.GetUserInspector();
            }
            finally
            {
                this.Container.Release(disposalDomain);
            }
        }

        private void FillDisposalSubentities(Disposal disposal)
        {
            var expertDomain = this.Container.ResolveDomain<DisposalExpert>();
            var docInspectorDomain = this.Container.ResolveDomain<DocumentGjiInspector>();
            var disposalTypeSurveyDomain = this.Container.ResolveDomain<DisposalTypeSurvey>();
            var typesInspFoundationDomain = this.Container.ResolveDomain<TypeSurveyInspFoundationGji>();
            var typesGoalsDomain = this.Container.ResolveDomain<TypeSurveyGoalInspGji>();
            var disposalProvDocsDomain = this.Container.ResolveDomain<ChelyabinskDisposalProvidedDoc>();
            var verifSubjectDomain = this.Container.ResolveDomain<DisposalVerificationSubject>();

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

                this.Report.RegData("Эксперты", "Эксперты", experts);

                var inspectors = docInspectorDomain.GetAll()
                    .Where(x => x.DocumentGji.Id == disposal.Id)
                    .Select(x => new
                    {
                        ФИО = x.Inspector.FioGenitive,
                        Должность = x.Inspector.PositionGenitive
                    })
                    .ToList();

                this.Report.RegData("Инспекторы", "Инспекторы", inspectors);

                var typeIds = disposalTypeSurveyDomain.GetAll()
                    .Where(x => x.Disposal.Id == disposal.Id)
                    .Select(x => x.TypeSurvey.Id)
                    .ToArray();

                var foundations = typesInspFoundationDomain.GetAll()
                    .Where(x => typeIds.Contains(x.TypeSurvey.Id))
                    .Select(x => new { Наименование = x.NormativeDoc.Name })
                    .Distinct()
                    .ToList();

                this.Report.RegData("ПравовыеОснованияПроверки", "ПравовыеОснованияПроверки", foundations);

                var goals = typesGoalsDomain.GetAll()
                    .Where(x => typeIds.Contains(x.TypeSurvey.Id))
                    .Select(x => new { Наименование = x.SurveyPurpose.Name })
                    .ToList();

                this.Report.RegData("ЗадачиПроверки", "ЗадачиПроверки", goals);

                var provDocs = disposalProvDocsDomain.GetAll()
                    .Where(x => x.Disposal.Id == disposal.Id)
                    .OrderBy(x => x.Order)
                    .Select(x => new { Наименование = x.ProvidedDoc.Name })
                    .ToList();

                this.Report.RegData("ПредоставляемыеДокументы", "ПредоставляемыеДокументы", provDocs);

                var verifSubjs = verifSubjectDomain.GetAll()
                    .Where(x => x.Disposal.Id == disposal.Id)
                    .Select(x => new { Наименование = x.SurveySubject.Name })
                    .ToList();

                this.Report.RegData("ПредметыПроверки", "ПредметыПроверки", verifSubjs);
            }
            finally
            {
                this.Container.Release(expertDomain);
                this.Container.Release(docInspectorDomain);
                this.Container.Release(disposalTypeSurveyDomain);
                this.Container.Release(typesInspFoundationDomain);
                this.Container.Release(typesGoalsDomain);
                this.Container.Release(disposalProvDocsDomain);
            }
        }

        private void FillParentPrescription(Disposal disposal)
        {
            var prescription = this.GetParentDocument(disposal, TypeDocumentGji.Prescription);

            if (prescription != null)
            {
                this.Report["НомерПредписания"] = prescription.DocumentNumber;
                this.Report["ДатаПредписания"] = prescription.DocumentDate.ToDateString();
            }
        }

        private void FillInspection(DocumentGji document)
        {
            if (document.Inspection == null)
                return;

            var inspectionDomain = this.Container.ResolveDomain<InspectionGji>();
            var inspectionAppealsDomain = this.Container.ResolveDomain<InspectionAppealCits>();
            var baseJurPersonDomain = this.Container.ResolveDomain<BaseJurPerson>();
            var baseProsClaimDomain = this.Container.ResolveDomain<BaseProsClaim>();

            try
            {
                //т.к. у документа гжи lazy ссылка на проверку
                var inspection = inspectionDomain.Get(document.Inspection.Id);

                if (inspection == null)
                    return;

                this.FillInspectionSubentities(inspection);

                this.Report["ФИО"] = inspection.PhysicalPerson;
                this.Report["ОбъектПроверки"] = inspection.PersonInspection.GetEnumMeta().Display;

                if (inspection.Contragent != null)
                {
                    this.Report["ИНН"] = inspection.Contragent.Inn;
                    this.Report["ЮридическийАдрес"] = inspection.Contragent.FiasJuridicalAddress != null
                        ? inspection.Contragent.FiasJuridicalAddress.AddressName
                        : inspection.Contragent.AddressOutsideSubject;
                    this.Report["ЮридическоеЛицоРП"] = inspection.Contragent.NameGenitive;
                    this.Report["ТипПредпринимательства"] = inspection.Contragent.TypeEntrepreneurship.GetEnumMeta().Display;
                }

                this.Report["ТипЮридическогоЛица"] = inspection.TypeJurPerson.GetEnumMeta().Display;

                var baseJurPerson = baseJurPersonDomain.Get(inspection.Id);
                if (baseJurPerson != null)
                {
                    this.Report["ПланПроверокНомер"] = baseJurPerson.Plan.Return(x => x.NumberDisposal);
                    this.Report["ПланПроверокДата"] = baseJurPerson.Plan.Return(x => x.DateDisposal).ToDateString();
                }

                var baseProsClaim = baseProsClaimDomain.Get(inspection.Id);
                if (baseProsClaim != null)
                {
                    this.Report["ДокументНаименование"] = baseProsClaim.DocumentName;
                    this.Report["ДокументНомер"] = baseProsClaim.DocumentNumber;
                    this.Report["ДокументДата"] = baseProsClaim.DocumentDate.ToDateString();
                }

                var appeal = inspectionAppealsDomain.GetAll()
                    .Where(x => x.Inspection.Id == inspection.Id)
                    .Select(x => x.AppealCits)
                    .FirstOrDefault();

                if (appeal != null)
                {
                    this.Report["ТипКорреспондента"] = appeal.TypeCorrespondent.GetEnumMeta().Display;
                    this.Report["ДатаОбращения"] = appeal.DateFrom.ToDateString();
                    this.Report["НомерОбращения"] = appeal.NumberGji;
                }
            }
            finally
            {
                this.Container.Release(inspectionDomain);
                this.Container.Release(inspectionAppealsDomain);
                this.Container.Release(baseJurPersonDomain);
                this.Container.Release(baseProsClaimDomain);
            }
        }

        private void FillInspectionSubentities(InspectionGji inspection)
        {
            if (inspection == null) return;

            var inspectionRoDomain = this.Container.ResolveDomain<InspectionGjiRealityObject>();

            try
            {
                this.Report.RegData("ПроверяемыеДома",
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
                this.Container.Release(inspectionRoDomain);
            }
        }

        private List<ФактыНарушений> GetFactViolations(ChelyabinskDisposal disposal)
        {
            var serviceDisposalFactViolation = this.Container.ResolveDomain<DisposalFactViolation>();
            using (this.Container.Using(serviceDisposalFactViolation))
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

        private void GetUserInspector()
        {
            var userManager = this.Container.Resolve<IGkhUserManager>();

            try
            {
                var oper = userManager.GetActiveOperator();

                this.Report.RegData("Инспектор",
                         new
                         {
                             ДолжностьИнспектораОператора = oper?.Inspector.Position,
                             ФИОИнспектораОператора = oper?.Inspector.ShortFio,
                             ТелефонИнспектораОператора = oper?.Inspector.Phone,
                             EmailИнспектораОператора = oper?.Inspector.Email

                         });
            }
            finally
            {
                this.Container.Release(userManager);
            }
        }

    }
}