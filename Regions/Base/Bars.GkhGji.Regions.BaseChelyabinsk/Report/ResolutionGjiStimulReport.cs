namespace Bars.GkhGji.Regions.BaseChelyabinsk.Report
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using B4.Modules.Reports;
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Report;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Properties;
    using Bars.GkhGji.Report;
    using Slepov.Russian.Morpher;
    using Stimulsoft.Report;
    using Bars.GkhGji.DomainService;
    using System;

    public class ResolutionGjiStimulReport : GkhBaseStimulReport, IComissionMeetingCodedReport
    {
      
          
        public ResolutionGjiStimulReport()
            : base(new ReportTemplateBinary(Resources.POSTAN1_REP))
        {
            ExportFormat = StiExportFormat.Word2007;
        }
        protected long DocumentId { get; set; }

        protected long DisposalId { get; set; }

        private Resolution definition;
        public Resolution Definition
        {
            get { return definition; }
            set { definition = value; }
        }
        public override string Id
        {
            get { return "Resolution"; }
        }
        public string DocId { get; set; }
        public Stream ReportFileStream { get; set; }
        public ComissionMeetingReportInfo ReportInfo { get; set; }
        public string ReportId => "Resolution";
        public string OutputFileName { get; set; } = "Постановление";


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

        protected override string CodeTemplate { get; set; }

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
                            Code = "POSTAN1_REP",
                            Description = "постановление о назначении административного наказания",
                            Template = Properties.Resources.POSTAN1_REP
                    },
                    new TemplateInfo
                    {
                            Name = "ResolutionGJI",
                            Code = "POSTAN2_REP",
                            Description = "о прекращениии производства",
                            Template = Properties.Resources.POSTAN2_REP
                    },
                    new TemplateInfo
                    {
                            Name = "ResolutionGJI",
                            Code = "POSTAN3_REP",
                            Description = "о прекращениии исполнения постановления",
                            Template = Properties.Resources.POSTAN3_REP
                    },
                    new TemplateInfo
                    {
                            Name = "ResolutionGJI",
                            Code = "POSTAN1_UR_REP",
                            Description = "постановление о назначении административного наказания",
                            Template = Properties.Resources.POSTAN1_UR_REP
                    },
                    new TemplateInfo
                    {
                            Name = "ResolutionGJI",
                            Code = "POSTAN2_UR_REP",
                            Description = "о прекращениии производства",
                            Template = Properties.Resources.POSTAN2_UR_REP
                    },
                    new TemplateInfo
                    {
                            Name = "ResolutionGJI",
                            Code = "POSTAN3_UR_REP",
                            Description = "о прекращениии исполнения постановления",
                            Template = Properties.Resources.POSTAN3_UR_REP
                    },
                };
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            try
            {
                var resolution = Container.Resolve<IDomainService<Resolution>>().Load(DocumentId);
                var document = Container.Resolve<IDomainService<DocumentGji>>().Load(DocumentId);
               var disposal = Container.Resolve<IDomainService<Disposal>>().Load(DisposalId);
                if (resolution == null)
                {
                    throw new ReportProviderException("Не удалось получить постановление");
                }

                if (resolution.Sanction == null)
                {
                    throw new ReportProviderException("Не указана санкция");
                }
                this.Report["Id"] = this.DocumentId;
                var r = resolution.ConcederationResult;
 
            
            
            }

            catch { }
            finally
            {
                //this.FillRegionParams(reportParams, definition);
            }
            
        }

        protected virtual void FillRegionParams(ReportParams reportParams, ResolutionDefinition definition)
        { }


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
        public override Stream GetTemplate()
        {
            var resolution = Container.Resolve<IDomainService<Resolution>>().Load(DocumentId);
            var document = Container.Resolve<IDomainService<DocumentGjiChildren>>();
           
            if (resolution == null)
            {
                throw new ReportProviderException("Не удалось получить постановление");
            }

            if (resolution.Sanction == null)
            {
                throw new ReportProviderException("Не указана санкция");
            }

            //В общем тут вытягивается из справочника наименование решения по постановлению и исходя из данного решения выбирается нужный шаблон для печати  
            var r = resolution.ConcederationResult;
            //Report["Id"] = Definition.Id;


                switch (r.Name)
                {
                    case "постановление о назначении административного наказания":
                        CodeTemplate = "POSTAN1_REP";
                        break;
                    case "постановление о прекращении производства":
                        CodeTemplate = "POSTAN2_REP";
                        break;
                    case "постановление о прекращении исполнения постановления":
                        CodeTemplate = "POSTAN3_REP";

                        break;

                }




            //Definition = Container.Resolve<IDomainService<ResolutionDefinition>>().Load(DocumentId);

            //if (Definition == null)
            //{
            //    throw new ReportProviderException("Не удалось получить определение");
            //}

            //Report["Id"] = Definition.Id;

            //switch (definition.TypeDefinition)
            //{
            //    case TypeDefinitionResolution.PostanRep:
            //        CodeTemplate = "POSTAN1_REP";
            //        break;
            //    case TypeDefinitionResolution.Postan1Rep:
            //        CodeTemplate = "POSTAN2_REP";
            //        break;
            //    case TypeDefinitionResolution.Postan1UrRep:
            //        CodeTemplate = "POSTAN3_REP";

            //        break;
            //    case TypeDefinitionResolution.PostanUrRep:
            //        CodeTemplate = "POSTAN3_REP";
            //        break;
            //}
            var templateDomain = Container.Resolve<IDomainService<TemplateReplacement>>();
            var fileManager = Container.Resolve<IFileManager>();

            try
            {
                var code = CodeTemplate;

                var listTemplatesInfo = GetTemplateInfo();

                //if (string.IsNullOrEmpty(code))
                //{
                //    if (listTemplatesInfo != null && listTemplatesInfo.Count > 0)
                //    {
                //        code = listTemplatesInfo.FirstOrDefault().Return(x => x.Code);
                //    }
                //}

                var templateReplace = templateDomain.GetAll().FirstOrDefault(x => x.Code == code);

                if (templateReplace != null)
                {

                    if (fileManager != null)
                    {
                        return fileManager.GetFile(templateReplace.File);
                    }
                }

                TemplateInfo template = null;
                if (listTemplatesInfo != null && listTemplatesInfo.Count > 0)
                {
                    template = listTemplatesInfo.FirstOrDefault(x => x.Code == code);
                }

                if (template != null && template.Template != null)
                {
                    return new MemoryStream(template.Template);
                }

                return null;
            }
            catch (FileNotFoundException)
            {
                throw new ReportProviderException("Не удалось получить шаблон замены");
            }
            finally
            {
                Container.Release(templateDomain);
                Container.Release(fileManager);
            }
        }
    }
}
