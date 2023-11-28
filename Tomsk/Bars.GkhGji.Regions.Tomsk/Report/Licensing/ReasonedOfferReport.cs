namespace Bars.GkhGji.Regions.Tomsk.Report.Licensing
{
	using System.Collections.Generic;
	using B4.DataAccess;
	using B4.Modules.Reports;
	using Gkh.Report;
	using Properties;
	using Stimulsoft.Report;

	public class ReasonedOfferReport : GkhBaseStimulReport
    {
        public ReasonedOfferReport() : base(new ReportTemplateBinary(Resources.ReasonedOffer))
        {
        }

        public override string Id
        {
            get { return "ReasonedOffer"; }
        }

        public override string CodeForm
        {
            get { return "ManOrgLicenseRequest"; }
        }

        public override string Name
        {
            get { return "Мотивированное предложение"; }
        }

        public override string Description
        {
            get { return "Мотивированное предложение"; }
        }

        protected override string CodeTemplate { get; set; }

		protected long RequestId;

		public override void SetUserParams(UserParamsValues userParamsValues)
        {
			RequestId = userParamsValues.GetValue<long>("RequestId");
		}

        /// <summary>Формат печатной формы</summary>
        public override StiExportFormat ExportFormat
        {
            get { return StiExportFormat.Word2007; }
        }

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
            {
                new TemplateInfo
                {
                    Code = "ReasonedOffer",
                    Description = "Мотивированное предложение",
                    Name = "ReasonedOffer",
                    Template = Resources.ReasonedOffer
				}
            };
        }

		public override void PrepareReport(ReportParams reportParams)
		{
			//отчет делается силами внедрения
			Report["ИдентификаторДокументаГЖИ"] = RequestId.ToString();
			Report["СтрокаПодключениякБД"] = Container.Resolve<IDbConfigProvider>().ConnectionString;
		}
	}
}