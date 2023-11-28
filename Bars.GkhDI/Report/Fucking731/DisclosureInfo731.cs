﻿namespace Bars.GkhDi.Report.Fucking731
{
    using System.Collections.Generic;
    using System.IO;
    using B4.Modules.Reports;
    using Gkh.Report;
    using Stimulsoft.Report;

    public partial class DisclosureInfo731 : GkhBaseStimulReport
    {
        #region .ctor

        public DisclosureInfo731()
            : base(new ReportTemplateBinary(Properties.Resources.DisclosureInfo731))
        {
        }

        #endregion

        private long _manorgId;
        private long _periodId;

        public override void PrepareReport(ReportParams reportParams)
        {
            var record = new ManOrgRecord();

            var dinfo = GetDisclosureInfo();

            FillManOrgInfo(record, dinfo);

            FillRobjectInfo(record, dinfo);

            Report.CacheAllData = true;

            Report.RegBusinessObject("ManOrgRecord", record);
        }

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            _manorgId = userParamsValues.GetValue<long>("manorgId");
            _periodId = userParamsValues.GetValue<long>("periodId");
        }

        public override string Extention
        {
            get { return "xlsx"; }
        }

        public override StiExportFormat ExportFormat
        {
            get
            {
                return StiExportFormat.Excel2007;
            }
            set { }
        }

        public override Stream GetTemplate()
        {
            return new MemoryStream(Properties.Resources.DisclosureInfo731);
        }

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>();
        }

        public override string CodeForm
        {
            get { return "DisclosureInfo"; }
        }

        public override string Name
        {
            get { return "Отчет по раскрытию информации (731 пост.)"; }
        }

        public override string Description
        {
            get { return ""; }
        }

        protected override string CodeTemplate { get; set; }

        public override string Id
        {
            get { return "DisclosureInfo731"; }
        }
    }
}