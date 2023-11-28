namespace Bars.GkhGji.Regions.BaseChelyabinsk.Report.Protocol
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using B4.Modules.Reports;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Report;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Contracts;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Properties;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities;
    using Bars.GkhGji.Report;
    using Stimulsoft.Report;
    using Bars.B4;
    using System.IO;
    using Bars.B4.Modules.FileStorage;
    using System;

    public class ChelyabinskProtocolNotificationStimulReport : GkhBaseStimulReport
    {
        public ChelyabinskProtocolNotificationStimulReport()
            : base(new ReportTemplateBinary(Resources.Извещение_REP_IZV_REP_))
        {
            ExportFormat = StiExportFormat.Word2007;
        }

        private long DocumentId { get; set; }

        private Protocol protocol;
        public Protocol Protocol
        {
            get { return protocol; }
            set { protocol = value; }
        }


        public override string Id
        {
            get { return "ProtocolNotification"; }
        }         
        public override string CodeForm
        {
            get { return "ProtocolDefinition"; }
        }

        public override string Name
        {
            get { return "Уведомление на комиссию"; }
        }

        public override string Description
        {
            get { return "Уведомление на комиссию из протокола"; }
        }

        protected override string CodeTemplate { get; set; }

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
                {
                    new TemplateInfo
                        {
                            Code = "Извещение_REP_IZV_REP_",
                            Name = "ProtocolGJI",
                            Description = "Любой случай",
                            Template = Resources.Извещение_REP_IZV_REP_
                        }
                };
        }

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            DocumentId = userParamsValues.GetValue<object>("DocumentId").ToLong();
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            Protocol = Container.Resolve<IDomainService<Protocol>>().Load(DocumentId);


            try
            {               
                if (Protocol == null)
                {
                    throw new ReportProviderException("Не удалось получить протокол");
                }
                //Report["Id"] = Convert.ToInt32(Protocol.Id);  
                CodeTemplate = "Извещение_REP_IZV_REP_";
            }
            finally
            {
              
            }
        }
             public override Stream GetTemplate()
          {

            Protocol = Container.Resolve<IDomainService<Protocol>>().Load(DocumentId);

            if (Protocol == null)
            {
                throw new ReportProviderException("Не удалось получить протокол");
            }

            Report["Id"] = Protocol.Id;


            CodeTemplate = "Извещение_REP_IZV_REP_";



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
        protected virtual void FillRegionParams(ReportParams reportParams, Protocol protocol)
        {

        }



    }
    }

