namespace Bars.GkhGji.Report
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Bars.B4;
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Report;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.DomainService;
    using System;
    using Stimulsoft.Report;
    using System.IO;

    public class ResolutionGjiReport : GjiBaseStimulReport, IComissionMeetingCodedReport
    {
        protected long DocumentId { get; set; }

        protected override string CodeTemplate { get; set; }
        public string DocId { get; set; }
        public Stream ReportFileStream { get; set; }
        public ComissionMeetingReportInfo ReportInfo { get; set; }
        public string ReportId => "Resolution";
        public string OutputFileName { get; set; } = "Постановление";

        public ResolutionGjiReport() : base(new ReportTemplateBinary(Properties.Resources.BlockGJI_Resolution))
        {
        }

        public override string Id
        {
            get { return "Resolution"; }
        }

        public override string CodeForm
        {
            get { return "Resolution"; }
        }

        public override string Name
        {
            get { return "Постановление"; }
        }

        public override string Description
        {
            get { return "Постановление"; }
        }

        /// <summary>
        /// Расширение
        /// </summary>
        public override string Extention => "mrt";

        /// <summary>
        /// Генератор отчетов
        /// </summary>
        public override string ReportGeneratorName => "StimulReportGenerator";

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
                            Name = "ResolutionGJI",
                            Code = "BlockGJI_Resolution",
                            Description = "Вид санкции с кодом 0,1,5",
                            Template = Properties.Resources.BlockGJI_Resolution
                        },
                    new TemplateInfo
                        {
                            Name = "ResolutionGJI",
                            Code = "BlockGJI_Resolution_1",
                            Description = "Вид санкции с кодом 2",
                            Template = Properties.Resources.BlockGJI_Resolution_1
                        },
                    new TemplateInfo
                        {
                            Name = "ResolutionGJI",
                            Code = "BlockGJI_Resolution_2",
                            Description = "Вид санкции с кодом 3",
                            Template = Properties.Resources.BlockGJI_Resolution_2
                        },
                    new TemplateInfo
                        {
                            Name = "ResolutionGJI",
                            Code = "BlockGJI_Resolution_3",
                            Description = "Вид санкции с кодом 4",
                            Template = Properties.Resources.BlockGJI_Resolution_3
                        },
                    new TemplateInfo
                        {
                            Name = "ResolutionGJI",
                            Code = "BlockGJI_Resolution_4",
                            Description = "Вид санкции с кодом 1 и тип исполнителя с кодом 1,5,6,7,10,12,13,14,16,19",
                            Template = Properties.Resources.BlockGJI_Resolution
                        }
                };
        }

        protected virtual void SelectCodeTemlate(string sanctionCode, string executantCode)
        {
            switch (sanctionCode)
            {
                case "0":
                case "5":
                    CodeTemplate = "BlockGJI_Resolution";
                    break;
                case "2":
                    CodeTemplate = "BlockGJI_Resolution_1";
                    break;
                case "3":
                    CodeTemplate = "BlockGJI_Resolution_2";
                    break;
                case "4":
                    CodeTemplate = "BlockGJI_Resolution_3";
                    break;
                case "1":
                    if (new[] { "1", "5", "6", "7", "10", "12", "13", "14", "16", "19" }.Contains(executantCode))
                    {
                        CodeTemplate = "BlockGJI_Resolution_4";
                    }
                    else
                    {
                        CodeTemplate = "BlockGJI_Resolution";
                    }
                    break;
            }
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var resolution = Container.Resolve<IDomainService<Resolution>>().Load(DocumentId);

            if (resolution == null)
            {
                throw new ReportProviderException("Не удалось получить постановление");
            }

            if (resolution.Sanction == null)
            {
                throw new ReportProviderException("Не указана санкция");
            }
            Report["Id"] = resolution.Id;
            SelectCodeTemlate(resolution.Sanction.Code, resolution.Executant != null ? resolution.Executant.Code : string.Empty);
        }

        public void GenerateMassReport()
        {

            var DocumentGjiDomain = this.Container.Resolve<IDomainService<Resolution>>();
            try
            {
                if (this.DocId.IsEmpty())
                {
                    throw new Exception("Не найден протокол");
                }
                this.DocumentId = Convert.ToInt64(this.DocId);

                var ownerInfo = DocumentGjiDomain.Get(DocumentId);

                StiExportFormat format = StiExportFormat.Word2007;
                this.OutputFileName =
                    $"Постановление {ownerInfo.Inspection.InspectionNumber}  ({ownerInfo.DocumentNumber} - {ownerInfo.DocumentDate.Value.ToString("dd.MM.yyyy")}).docx";

            }
            finally
            {
                this.Container.Release(DocumentGjiDomain);
            }
        }
    }
}