namespace Bars.GkhGji.Regions.Sahalin.Report
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Bars.B4.DataAccess;
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Report;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Nso.Entities;
    using Bars.GkhGji.Report;
    using Stimulsoft.Report;

    public class PrescriptionStimulReport : GjiBaseStimulReport
    {
        public PrescriptionStimulReport()
			: base(new ReportTemplateBinary(Properties.Resources.SahalinPrescription))
        {
        }

        #region Properties

        public override string Id
        {
            get { return "SahalinPrescriptionGji"; }
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

        public override StiExportFormat ExportFormat
        {
            get { return StiExportFormat.Word2007; }
        }

        protected override string CodeTemplate { get; set; }

        #endregion Properties

        protected long DocumentId;

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
                    Code = "SahalinPrescriptionGji",
                    Name = "Prescription",
                    Description = "Предписание",
                    Template = Properties.Resources.SahalinPrescription
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
			CodeTemplate = "SahalinPrescriptionGji";
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var prescriptionDomain = Container.ResolveDomain<Prescription>();
            var prescriptionViolDomain = Container.ResolveDomain<PrescriptionViol>();
            var docInspectorDomain = Container.ResolveDomain<DocumentGjiInspector>();

            try
            {
                var prescription = prescriptionDomain.FirstOrDefault(x => x.Id == DocumentId);

                if (prescription == null)
                {
                    return;
                }
                FillCommonFields(prescription);

                if (prescription.Contragent != null)
                {
                    var contragent = new
                    {
                        АдресФакт = prescription.Contragent.FactAddress,
                        Адрес = prescription.Contragent.JuridicalAddress,
                        Наименование = prescription.Contragent.Name,
                        НаименованиеСокр = prescription.Contragent.ShortName
                    };

                    Report.RegData("Контрагент", contragent);
                }

                var actCheck = GetParentDocument(prescription, TypeDocumentGji.ActCheck);

                if (actCheck != null)
                {
                    var actCheckProxy = new
                    {
                        Номер = actCheck.DocumentNumber,
                        Дата = actCheck.DocumentDate.HasValue
                            ? actCheck.DocumentDate.Value.ToShortDateString()
                            : string.Empty,
                    };

                    Report.RegData("АктПроверки", actCheckProxy);
                }

                var roInfos = prescriptionViolDomain.GetAll()
                    .Where(x => x.Document.Id == DocumentId)
                    .Select(x => new
                    {
                        Municipality =
                            x.InspectionViolation.RealityObject != null
                                ? x.InspectionViolation.RealityObject.Municipality.Name
                                : "",
                        PlaceName =
                            x.InspectionViolation.RealityObject != null
                                ? x.InspectionViolation.RealityObject.FiasAddress.PlaceName
                                : "",
                        RealityObject =
                            x.InspectionViolation.RealityObject != null
                                ? x.InspectionViolation.RealityObject.Address
                                : "",
                        Id = x.InspectionViolation.RealityObject != null ? x.InspectionViolation.RealityObject.Id : 0,
                    })
                    .OrderBy(x => x.Municipality)
                    .ThenBy(x => x.RealityObject)
                    .ToList()
                    .Select(x => new
                    {
                        Адрес = x.RealityObject,
                        МестоСоставления = x.PlaceName
                    }).ToList();

                Report.RegData("АдресДома", roInfos);

                var violInfos = prescriptionViolDomain.GetAll()
                    .Where(x => x.Document.Id == DocumentId)
                    .Select(x => new
                    {
                        x.InspectionViolation.Violation.CodePin,
                        x.Action,
                        x.InspectionViolation.DatePlanRemoval,
                        x.InspectionViolation.Violation.Name
                    })
                    .ToList()
                    .Select(x => new
                    {
                        КодПин = x.CodePin,
                        Мероприятие = x.Action,
                        ДатаВыполнения = x.DatePlanRemoval.HasValue
                            ? x.DatePlanRemoval.Value.ToShortDateString()
                            : string.Empty,
                        Наименование = x.Name
                    })
                    .ToList();

                Report.RegData("Нарушения", violInfos);

                var inspectors = docInspectorDomain.GetAll()
                    .Where(x => x.DocumentGji.Id == DocumentId)
                    .Select(x => new
                    {
                        ФИО = x.Inspector.Fio,
                        ФамилияИОСокр = x.Inspector.ShortFio,
                        Должность = x.Inspector.Position,
                        Описание = x.Inspector.Description
                    })
                    .ToList();

                Report.RegData("Инспекторы", inspectors);

                var prescriptionProxy = new
                {
                    Дата = prescription.DocumentDate.HasValue
                        ? prescription.DocumentDate.Value.ToShortDateString()
                        : string.Empty,
                    Номер = prescription.DocumentNumber
                };

                Report.RegData("Предписание", prescriptionProxy);

            }
            finally
            {
                Container.Release(prescriptionDomain);
                Container.Release(prescriptionViolDomain);
                Container.Release(docInspectorDomain);
            }
        }
    }
}
