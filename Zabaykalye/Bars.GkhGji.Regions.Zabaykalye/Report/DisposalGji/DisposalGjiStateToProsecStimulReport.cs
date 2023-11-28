namespace Bars.GkhGji.Regions.Zabaykalye.Report
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Report;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Zabaykalye.Properties;
    using GkhGji.Report;
    using Stimulsoft.Report;

    public class DisposalGjiStateToProsecStimulReport : GjiBaseStimulReport
    {
        public DisposalGjiStateToProsecStimulReport()
            : base(new ReportTemplateBinary(Resources.InstructionStateToProsec))
        {
        }
        private long DocumentId { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var disposalService = Container.ResolveDomain<Disposal>();
            var baseJurPersonDomain = Container.ResolveDomain<BaseJurPerson>();
            var zonalInspDomain = Container.ResolveDomain<ZonalInspection>();

            using (Container.Using(disposalService, baseJurPersonDomain, zonalInspDomain))
            {
                var disposal = disposalService.Load(DocumentId);

                if (disposal != null)
                {
                    FillCommonFields(disposal);
                    Report["Иснспектор"] = disposal.ReturnSafe(x => x.ResponsibleExecution.Fio);
                    Report["ДолжностьИнспектора"] = disposal.ReturnSafe(x => x.ResponsibleExecution.Position);


                    var baseJurPerson = baseJurPersonDomain.Get(disposal.Inspection.Id);

                    if (baseJurPerson != null)
                    {
                        Report["Контрагент"] = baseJurPerson.Contragent.ReturnSafe(x => x.ShortName);
                        Report["АдресЮР"] = baseJurPerson.Contragent.ReturnSafe(x => x.JuridicalAddress);
                        Report["ОснованиеПроверки"] = baseJurPerson.TypeBaseJuralPerson.GetEnumMeta().Display;
                        Report["ДатаНачалоПроверки"] = baseJurPerson.DateStart.ToDateTime().ToString("«dd» MMMM yyyy", CultureInfo.CurrentCulture); ;
                    }

                    Report["ВремяПроведения"] = string.Empty;
                }

                var zonalInsp = zonalInspDomain.GetAll().FirstOrDefault();

                if (zonalInsp != null)
                {
                    Report["ГЖИРп"] = zonalInsp.Return(x => x.NameGenetive);
                    Report["ГЖИАдрес"] = zonalInsp.Return(x => x.Address);
                }
            }

        }

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            DocumentId = userParamsValues.GetValue<object>("DocumentId").ToLong();
        }

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
                            Code = "BlockGJI_InstructionStateToProsec",
                            Name = "InstructionStateToProsec",
                            Description = "Заявление в прокуратуру",
                            Template = Resources.InstructionStateToProsec
                        }
                };
        }

        public override string Name
        {
            get { return "Заявление в прокуратуру"; }
        }

        public override string Description
        {
            get { return "Заявление в прокуратуру"; }
        }

        protected override string CodeTemplate { get; set; }

        public override string Id
        {
            get { return "DisposalStateToProsec"; }
        }

        public override string CodeForm
        {
            get { return "Disposal"; }
        }
    }
}