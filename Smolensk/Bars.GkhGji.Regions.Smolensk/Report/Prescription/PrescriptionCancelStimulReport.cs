namespace Bars.GkhGji.Regions.Smolensk.Report.Prescription
{
    using System.Collections.Generic;
    using System.Linq;

    using B4.DataAccess;
    using B4.Modules.Reports;
    using B4.Utils;
    using Bars.B4.IoC;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;

    using Entities;
    using Gkh.Report;
    using Stimulsoft.Report;

    public class PrescriptionCancelStimulReport : GkhBaseStimulReport
    {
        #region .ctor

        public PrescriptionCancelStimulReport()
            : base(new ReportTemplateBinary(Properties.Resources.PrescriptionCancel))
        {
        }

        #endregion .ctor

        #region Properties

        public override string Name
        {
            get { return "Решение о продлении срока"; }
        }

        public override StiExportFormat ExportFormat
        {
            get { return StiExportFormat.Word2007; }
        }

        public override string Description
        {
            get { return "Решение"; }
        }

        protected override string CodeTemplate { get; set; }

        public override string Id
        {
            get { return "SmolPrescriptionCancel"; }
        }

        public override string CodeForm
        {
            get { return "PrescriptionCancel"; }
        }

        #endregion

        #region Fields

        protected long CancelId;

        #endregion Fields

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            CancelId = userParamsValues.GetValue<object>("CancelId").ToLong();
        }

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
            {
                new TemplateInfo
                {
                    Code = "PrescriptionCancel",
                    Name = "PrescriptionCancel",
                    Description = "Решение предписания",
                    Template = Properties.Resources.PrescriptionCancel
                }
            };
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            PrescriptionCancelSmol cancel;
            Inspector inspectorPrescription;

            var cancelDomain = Container.ResolveDomain<PrescriptionCancelSmol>();
            var docInspectorDomain = Container.ResolveDomain<DocumentGjiInspector>();
            using (Container.Using(cancelDomain, docInspectorDomain))
            {
                cancel = cancelDomain.Get(CancelId);

                if (cancel == null)
                {
                    return;
                }

                inspectorPrescription = docInspectorDomain.GetAll().Where(x => x.DocumentGji.Id == cancel.Prescription.Id).Select(x => x.Inspector).FirstOrDefault();
            }

            if (inspectorPrescription != null)
            {
                Report["ДолжностьИнспектораПредписания"] = inspectorPrescription.Position;
                Report["ФИОИнспектораПредписания"] = inspectorPrescription.Fio;
            }

            Report["ДатаРешения"] =
                cancel.DocumentDate.HasValue
                    ? cancel.DocumentDate.Value.ToShortDateString()
                    : null;

            Report["КодИнспектора"] = cancel.IssuedCancel.Return(x => x.Position);
            Report["ФИОИнспектора"] = cancel.IssuedCancel.Return(x => x.Fio);
            
            Report["НомерХодатайства"] = cancel.SmolPetitionNum;
            Report["ДатаХодатайства"] =
                cancel.SmolPetitionDate.HasValue
                    ? cancel.SmolPetitionDate.Value.ToShortDateString()
                    : null;

            if (cancel.Prescription != null)
            {
                Report["Контрагент"] = cancel.Prescription.Contragent.Return(x => x.Name);
                Report["НомерПредписания"] = cancel.Prescription.DocumentNumber;
                Report["ДатаПредписания"] =
                    cancel.Prescription.DocumentDate.HasValue
                        ? cancel.Prescription.DocumentDate.Value.ToShortDateString()
                        : null;
            }

            Report["Установлено"] = cancel.SmolDescriptionSet;
            Report["РезультатРешения"] = cancel.SmolCancelResult;
        }
    }
}