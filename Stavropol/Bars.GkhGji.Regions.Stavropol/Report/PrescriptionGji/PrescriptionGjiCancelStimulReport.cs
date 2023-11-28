namespace Bars.GkhGji.Regions.Stavropol.Report.PrescriptionGji
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Report;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Report;

    using Slepov.Russian.Morpher;

    using Stimulsoft.Report;

    public class PrescriptionGjiCancelStimulReport : GjiBaseStimulReport
    {
        private const string StavrapolSolution1 = "Stavrapol_Solution_1";

        private const string StavrapolSolution2 = "Stavrapol_Solution_2";

        /// <summary>
        /// Решение об отмене.
        /// </summary>
        private PrescriptionCancel prescriptionCancel;

        /// <summary>
        /// Предписание.
        /// </summary>
        private Prescription prescription;

        public PrescriptionGjiCancelStimulReport()
            : base(new ReportTemplateBinary(Properties.Resources.BlockGJI_Solution_1))
        {
        }

        public override string Id
        {
            get { return "PrescriptionCancel"; }
        }

        public override string CodeForm
        {
            get { return "PrescriptionCancel"; }
        }

        public override string Name
        {
            get { return "Решение"; }
        }

        public override string Description
        {
            get { return "Решение предписания"; }
        }

        public override StiExportFormat ExportFormat
        {
            get
            {
                return StiExportFormat.Word2007;
            }
        }

        /// <summary>
        /// Код шаблона (файла).
        /// </summary>
        protected override string CodeTemplate { get; set; }

        /// <summary>
        /// ИД решения об отмене.
        /// </summary>
        private long CancelId { get; set; }

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            this.CancelId = userParamsValues.GetValue<object>("CancelId").ToLong();

            var prescriptionCancelDomain = this.Container.ResolveDomain<PrescriptionCancel>();

            using (this.Container.Using(prescriptionCancelDomain))
            {
                this.prescriptionCancel = prescriptionCancelDomain.GetAll().FirstOrDefault(x => x.Id == this.CancelId);
            }

            if (this.prescriptionCancel == null)
            {
                throw new ReportProviderException("Не удалось получить решение об отмене");
            }

            this.prescription = this.prescriptionCancel.Prescription;
        }

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
                {
                    new TemplateInfo
                        {
                            Code = "BlockGJI_Solution_1",
                            Name = StavrapolSolution1,
                            Description = "Решение о продлении срока исполнения предписания",
                            Template = Properties.Resources.BlockGJI_Solution_1
                        },
                    new TemplateInfo
                        {
                            Code = "BlockGJI_Solution_2",
                            Name = StavrapolSolution2,
                            Description = "Решение об отказе в продлении срока исполнения предписания",
                            Template = Properties.Resources.BlockGJI_Solution_2
                        }
                };
        }

        /// <summary>
        /// Изменяем код шаблона в зависимости от типа решения и вызываем родительский метод.
        /// </summary>
        /// <returns>
        /// The <see cref="Stream"/>.
        /// </returns>
        public override Stream GetTemplate()
        {
            switch (this.prescriptionCancel.TypeCancel)
            {
                case TypePrescriptionCancel.Prolongation:
                    this.CodeTemplate = "BlockGJI_Solution_1";
                    break;
                case TypePrescriptionCancel.RefusExtend:
                    this.CodeTemplate = "BlockGJI_Solution_2";
                    break;
            }

            return base.GetTemplate();
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            this.CodeTemplate = "BlockGJI_Solution_1";
            var склонятель = new Склонятель("SonFhyB1DbaxkkAQ4tfrhQ==");

            // Заполняем общие поля
            this.FillCommonFields(this.prescription);

            var prescriptionViols = this.GetPrescriptonViolations(this.prescription);

            this.Report["ДатаРешения"] = this.prescriptionCancel.DocumentDate.HasValue
                                             ? this.prescriptionCancel.DocumentDate.Value.ToShortDateString()
                                             : string.Empty;

            var viol = prescriptionViols.FirstOrDefault();
            if (viol != null)
            {
                try
                {
                    this.Report["НаселенныйПункт"] = viol.InspectionViolation.RealityObject.FiasAddress.PlaceName;
                }
                catch (NullReferenceException ex)
                {
                    this.Report["НаселенныйПункт"] = string.Empty;
                }
            }

            this.Report["КодИнспектора"] = this.prescriptionCancel.IssuedCancel.Position;
            this.Report["ФИОИнспектора"] = this.prescriptionCancel.IssuedCancel.Fio;
            this.Report["Вотношении"] = this.prescription.Executant.Code;

            this.Report["ДатаХодатайства"] = this.prescriptionCancel.PetitionDate.HasValue
                                                 ? this.prescriptionCancel.PetitionDate.Value.ToShortDateString()
                                                 : string.Empty;
            this.Report["НомерХодатайства"] = this.prescriptionCancel.PetitionNumber;
            this.Report["ДатаПредписания"] = this.prescription.DocumentDate.HasValue
                                                 ? this.prescription.DocumentDate.Value.ToShortDateString()
                                                 : string.Empty;
            this.Report["НомерПредписания"] = this.prescription.DocumentNumber;

            if (!string.IsNullOrEmpty(this.prescription.PhysicalPerson))
            {
                var phisPersName = склонятель.Проанализировать(this.prescription.PhysicalPerson);
                this.Report["ФизЛицоРП"] = phisPersName.Родительный;
                this.Report["ФизЛицоИП"] = phisPersName.Именительный;
            }
            else
            {
                this.Report["ФизЛицоРП"] = string.Empty;
                this.Report["ФизЛицоИП"] = string.Empty;
            }

            this.Report["Реквизиты"] = this.prescription.PhysicalPersonInfo ?? string.Empty;
            this.Report["ПродлитьДо"] = this.prescriptionCancel.DateProlongation.HasValue
                                            ? this.prescriptionCancel.DateProlongation.Value.ToShortDateString()
                                            : string.Empty;
            this.Report["Установил"] = this.prescriptionCancel.DescriptionSet;
            this.Report["ФИОИнспектораСокр"] = this.prescriptionCancel.IssuedCancel.ShortFio;

            var firstInspectorPrescription = this.Container.Resolve<IDomainService<DocumentGjiInspector>>().GetAll()
                        .Where(x => x.DocumentGji.Id == this.prescription.Id)
                        .Select(x => x.Inspector)
                        .FirstOrDefault();

            if (firstInspectorPrescription != null)
            {
                this.Report["ФИОИнспектораПредписание"] = firstInspectorPrescription.Fio;
            }

            if (this.prescription.Contragent != null)
            {
                this.Report["КонтрагентСокр"] = this.prescription.Contragent.Name;

                var contactsContragent = this.Container.ResolveDomain<ContragentContact>().GetAll()
                    .FirstOrDefault(x => x.Contragent.Id == this.prescription.Contragent.Id);

                if (contactsContragent != null)
                {
                    if (contactsContragent.Position != null)
                    {
                        this.Report["ДолжностьКонтр"] = contactsContragent.Position.NameGenitive ?? string.Empty;
                    }

                    this.Report["АдресКонтрагента"] = string.Format(
                        "{0}, ИНН {1}",
                        this.prescription.Contragent.JuridicalAddress,
                        this.prescription.Contragent.Inn);

                    this.Report["КонтрагентФИО"] = string.Format(
                        "{0} {1} {2}",
                        contactsContragent.SurnameGenitive,
                        contactsContragent.NameGenitive,
                        contactsContragent.PatronymicGenitive);
                }
            }
        }

        /// <summary>
        /// Получает нарушения из предписания.
        /// </summary>
        /// <param name="presc">
        /// Предписание.
        /// </param>
        /// <returns>
        /// Нарушения
        /// </returns>
        private IEnumerable<PrescriptionViol> GetPrescriptonViolations(Prescription presc)
        {
            var prescriptionViolDomain = this.Container.ResolveDomain<PrescriptionViol>();

            using (this.Container.Using(prescriptionViolDomain))
            {
                return prescriptionViolDomain.GetAll().Where(x => x.Document.Id == presc.Id).ToList();
            }
        }
    }
}