using Slepov.Russian.Morpher;

namespace Bars.GkhGji.Regions.Smolensk.Report.ProtocolGji
{
    using System.Collections.Generic;

    using B4.DataAccess;
    using B4.Modules.Reports;
    using B4.Utils;
    using Bars.B4.IoC;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Smolensk.Entities;

    using Gkh.Report;

    using GkhGji.Entities;
    using Stimulsoft.Report;

    using System.IO;

    /// <summary> Уведомление  на подписание акта и протокола по результатам проверки предписания </summary>
    public class ActRemovalSignNotificationReport : GjiBaseStimulReport
    {
        #region .ctor

        public ActRemovalSignNotificationReport()
            : base(new ReportTemplateBinary(Properties.Resources.ActRemovalSignNotification))
        {

        }

        #endregion .ctor

        #region Properties

        public override string Id
        {
            get { return "ActRemovalSignNotification"; }
        }

        public override string Name
        {
            get { return "Уведомление на подписание акта и протокола"; }
        }

        public override string Description
        {
            get { return "Уведомление  на подписание акта и протокола по результатам проверки предписания"; }
        }

        protected override string CodeTemplate { get; set; }

        public override string CodeForm
        {
            get { return "ActRemoval"; }
        }

        public override StiExportFormat ExportFormat
        {
            get { return StiExportFormat.Word2007; }
        }

        #endregion Properties

        protected long DocumentId;

        protected ActRemovalSmol ActRemoval;

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            DocumentId = userParamsValues.GetValue<object>("DocumentId").ToLong();

            var domain = Container.ResolveDomain<ActRemovalSmol>();

            using (Container.Using(domain))
            {
                ActRemoval = domain.FirstOrDefault(x => x.Id == DocumentId);
            }
        }

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
            {
                new TemplateInfo
                {
                    Code = "ActRemovalSignNotification",
                    Name = "ActRemovalSignNotification",
                    Description = "Уведомление  на подписание акта и протокола по результатам проверки предписания",
                    Template = Properties.Resources.ActRemovalSignNotification
                }
            };
        }

        private void GetCodeTemplate()
        {
            CodeTemplate = "ActRemovalSignNotification";
        }

        public override Stream GetTemplate()
        {
            this.GetCodeTemplate();
            return base.GetTemplate();
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            if (ActRemoval == null)
            {
                return;
            }

            FillCommonFields(ActRemoval);
            FillActRemovalData();
        }

        private void FillActRemovalData()
        {
			var parentDisposal = GetParentDocument(ActRemoval, TypeDocumentGji.Disposal) as Disposal;
            var parentPrescription = GetParentDocument(ActRemoval, TypeDocumentGji.Prescription) as Prescription;
			var parentActCheck = GetParentDocument(ActRemoval, TypeDocumentGji.ActCheck) as ActCheck;
	        
			Protocol parentProtocol = null;
	        if (parentActCheck != null)
	        {
		        parentProtocol = GetChildDocument(parentActCheck, TypeDocumentGji.Protocol) as Protocol;
	        }

			var склонятель = new Склонятель("SonFhyB1DbaxkkAQ4tfrhQ==");

	        Report["УведомлениеНомер"] = ActRemoval.Return(x => x.DocumentNumber, string.Empty);
            Report["УведомлениеДата"] = ActRemoval.DocumentDate.HasValue
				? ActRemoval.DocumentDate.Value.ToShortDateString() : string.Empty;

			Report["НомерАкта"] = ActRemoval.Return(x => x.DocumentNumber, string.Empty);
			Report["ДатаАкта"] = ActRemoval != null && ActRemoval.DocumentDate.HasValue
				? ActRemoval.DocumentDate.Value.ToShortDateString() : string.Empty;

            //if (parentProtocol != null)
            //{
                //Report["ЮрЛицо"] = parentProtocol.Return(x => x.Contragent).Return(x => x.Name, string.Empty);
                //Report["ЮрЛицоСокр"] = parentProtocol.Return(x => x.Contragent).Return(x => x.ShortName, string.Empty);
                //Report["АдресЮл"] = parentProtocol.Return(x => x.Contragent).Return(x => x.JuridicalAddress, string.Empty);
                //Report["ТелефонЮл"] = parentProtocol.Return(x => x.Contragent).Return(x => x.Phone, string.Empty);
            //}

	        if (parentPrescription != null)
	        {
				Report["НомерПредписания"] = parentPrescription.Return(x => x.DocumentNumber, string.Empty);
				Report["ДатаПредписания"] = parentPrescription.DocumentDate.HasValue
			        ? parentPrescription.DocumentDate.Value.ToShortDateString()
			        : string.Empty;
                Report["ТипИсполнителя"] = parentPrescription.Return(x => x.Executant).Return(x => x.Code, string.Empty);

                Report["ЮрЛицо"] = parentPrescription.Return(x => x.Contragent).Return(x => x.Name, string.Empty);
                Report["ЮрЛицоСокр"] = parentPrescription.Return(x => x.Contragent).Return(x => x.ShortName, string.Empty);
                Report["АдресЮл"] = parentPrescription.Return(x => x.Contragent).Return(x => x.JuridicalAddress, string.Empty);
                Report["ТелефонЮл"] = parentPrescription.Return(x => x.Contragent).Return(x => x.Phone, string.Empty);
	        }

	        if (parentDisposal != null)
	        {
				var kindCheck = parentDisposal.Return(x => x.KindCheck).Return(x => x.Name, string.Empty);

		        Report["ВидПроверки"] = kindCheck;
				Report["ВидПроверкиСкл"] = склонятель.Проанализировать(kindCheck).Дательный.ToLower();

		        Report["НомерПриказа"] = parentDisposal.Return(x => x.DocumentNumber, string.Empty);
		        Report["ДатаПриказа"] = parentDisposal.DocumentDate.HasValue
			        ? parentDisposal.DocumentDate.Value.ToShortDateString()
			        : string.Empty;
	        }
        }
    }
}
