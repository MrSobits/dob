using System.Collections.Generic;
using System.Linq;
using Bars.B4;
using Bars.B4.Modules.Reports;
using Bars.B4.Utils;
using Bars.Gkh.Entities;
using Bars.Gkh.Report;
using Bars.GkhGji.Entities;
using Bars.GkhGji.Enums;
using System.Globalization;
using System.Text;
using Bars.B4.DataAccess;
using Bars.B4.IoC;
using Bars.Gkh.Utils;
using Bars.GkhGji.Contracts;
using Bars.GkhGji.Report;
using Stimulsoft.Report;
using Bars.GkhGji.Properties;
using System.IO;
using Bars.B4.Modules.FileStorage;

namespace Bars.GkhGji.Report
{
    public class ProtocolGjiDefinitionReport : GkhBaseStimulReport
    {
        public ProtocolGjiDefinitionReport()
          : base(new ReportTemplateBinary(Resources.OPRED3_REP))
        {
            ExportFormat = StiExportFormat.Word2007;
        }


        //public override StiExportFormat ExportFormat
        //{
        //    get { return StiExportFormat.Word2007; }
        //}

        //private long DefinitionId { get; set; }

        private long DefinitionId { get; set; }
        private ProtocolDefinition definition;
        public ProtocolDefinition Definition
        {
            get { return definition; }
            set { definition = value; }
        }

        protected override string CodeTemplate { get; set; }

      



        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            DefinitionId = userParamsValues.GetValue<object>("DefinitionId").ToLong();
        }



        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
            {
                new TemplateInfo
                {
                    Name = "Protocol_Definition_1",
                    Code = "OPRED3_REP",
                    Description =
                        "В порядке подготовки к рассмотрению дела об административном правонарушении",
                    Template = Properties.Resources.OPRED3_REP
                },
                new TemplateInfo
                {
                    Name = "Protocol_Definition_3",
                    Code = "OPRED6_REP",
                    Description = "Об отказе в удовлетворении ходатайства",
                    Template = Properties.Resources.OPRED6_REP
                },
                new TemplateInfo
                {
                    Name = "Protocol_Definition_4",
                    Code = "OPRED8_REP",
                    Description = "Об отложении рассмотрения дела",
                    Template = Properties.Resources.OPRED8_REP
                },
                new TemplateInfo
                {
                    Name = "Protocol_Definition_6",
                    Code = "OPRED9_REP",
                    Description = "В порядке подготовки к рассмотрению дела об административном правонарушении",
                    Template = Properties.Resources.OPRED9_REP
                }
           
            };
        }

        public override string Id
        {
            get { return "ProtocolDefinition"; }
        }

        public override string CodeForm
        {
            get { return "ProtocolDefinition"; }
        }

        public override string Name
        {
            get { return "Определение протокола"; }
        }

        public override string Description
        {
            get { return "Определение протокола"; }
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            try
            {
                Definition = Container.Resolve<IDomainService<ProtocolDefinition>>().Load(DefinitionId);

                if (Definition == null)
                {
                    throw new ReportProviderException("Не удалось получить определение");
                }

                Report["Id"] = Definition.Id;

                switch (definition.TypeDefinition)
                {
                    case TypeDefinitionProtocol.Opred3Rep:
                        CodeTemplate = "OPRED3_REP";
                        break;
                    case TypeDefinitionProtocol.Opred6Rep:
                        CodeTemplate = "OPRED6_REP";
                        break;
                    case TypeDefinitionProtocol.Opred8Rep:
                        CodeTemplate = "OPRED8_REP";
                        break;
                    case TypeDefinitionProtocol.Opred9Rep:
                        CodeTemplate = "OPRED9_REP";
                        break;
                }

            }
            catch { }

            finally
            {
                this.FillRegionParams(reportParams, definition);
            }
        }
        public override Stream GetTemplate()
        {

            Definition = Container.Resolve<IDomainService<ProtocolDefinition>>().Load(DefinitionId);

            if (Definition == null)
            {
                throw new ReportProviderException("Не удалось получить определение");
            }

            Report["Id"] = Definition.Id;


            switch (definition.TypeDefinition)
            {
                case TypeDefinitionProtocol.Opred3Rep:
                    CodeTemplate = "OPRED3_REP";
                    break;
                case TypeDefinitionProtocol.Opred6Rep:
                    CodeTemplate = "OPRED6_REP";
                    break;
                case TypeDefinitionProtocol.Opred8Rep:
                    CodeTemplate = "OPRED8_REP";
                    break;
                case TypeDefinitionProtocol.Opred9Rep:
                    CodeTemplate = "OPRED9_REP";
                    break;
            }



            var templateDomain = Container.Resolve<IDomainService<TemplateReplacement>>();
            var fileManager = Container.Resolve<IFileManager>();

            try
            {
                var code = CodeTemplate;

                var listTemplatesInfo = GetTemplateInfo();

                if (string.IsNullOrEmpty(code))
                {
                    if (listTemplatesInfo != null && listTemplatesInfo.Count > 0)
                    {
                        code = listTemplatesInfo.FirstOrDefault().Return(x => x.Code);
                    }
                }

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
        protected virtual void FillRegionParams(ReportParams reportParams, ProtocolDefinition definition)
        {

        }

   

    }
}