namespace Bars.B4.Modules.Analytics.Reports
{
    using System.Collections.Generic;
    using System.IO;
    using Bars.B4.Modules.Analytics.Data;
    using Bars.B4.Modules.Analytics.Reports.Enums;

    using Stimulsoft.Report.Export;

    public abstract class BaseCodedReport : ICodedReport
    {
        /// <summary>
        /// 
        /// </summary>
        protected abstract byte[] Template { get; }

        public virtual string Key
        {
            get
            {
                var type = GetType();
                return type.Name;
            } 
        }

        public abstract string Name { get; }
        public abstract string Description { get; set; }
        public Stream GetTemplate()
        {
            return new MemoryStream(Template);
        }

        public abstract IEnumerable<IDataSource> GetDataSources();
        public IEnumerable<IParam> GetParams()
        {
            // TODO
            throw new System.NotImplementedException();
        }

        public virtual StiExportSettings GetExportSettings(ReportPrintFormat format)
        {
            if (format == ReportPrintFormat.text)
            {
                return new StiTxtExportSettings
                {
                    DrawBorder = false,
                    PutFeedPageCode = false,
                    CutLongLines = false
                };
            }
            return null;
        }
    }
}
