namespace Bars.GkhGji.Report
{
    using B4.Utils;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Reports;
    using Entities;
    using Gkh.Report;
    using Stimulsoft.Report;
    using System.Collections.Generic;

    public class NotificationAttendanceByRepresentativeCheckResultsReport : GkhBaseStimulReport
    {
        private long requestId;

        public IDomainService<ActCheck> RequestDomain { get; set; }

        public IDbConfigProvider dbConfigProvider { get; set; }

        public NotificationAttendanceByRepresentativeCheckResultsReport() : base(new ReportTemplateBinary(Properties.Resources.NotificationAttendanceByRepresentativeCheckResults))
        {
        }

        public override string Permission
        {
            get { return "Reports.GJI.NotificationAttendanceByRepresentativeCheckResultsReport"; }
        }

        /// <summary>
        /// Подготовить параметры отчета
        /// </summary>
        /// <param name="reportParams"></param>
        public override void PrepareReport(ReportParams reportParams)
        {
            var request = this.RequestDomain.Get(this.requestId);

            this.Report["ИдентификаторДокументаГЖИ"] = request.Id.ToString();
            this.Report["СтрокаПодключениякБД"] = this.dbConfigProvider.ConnectionString;
        }

        public override string Id
        {
            get { return "NotificationAttendanceByRepresentativeCheckResultsReport"; }
        }

        public override string CodeForm
        {
            get { return "ActCheck"; }
        }

        public override string Name
        {
            get { return "Уведомление"; }
        }

        public override string Description
        {
            get { return "Уведомление о явке представителя по результатам проверки"; }
        }

        protected override string CodeTemplate { get; set; }

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            this.requestId = userParamsValues.GetValue<object>("DocumentId").ToLong();
        }

        public override string Extention
        {
            get { return "mrt"; }
        }

        /// <summary>Формат печатной формы</summary>
        public override StiExportFormat ExportFormat
        {
            get { return StiExportFormat.Word2007; }
            set { }
        }

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
            {
                new TemplateInfo
                {
                    Code = "NotificationAttendanceByRepresentativeCheckResultsReport",
                    Description = "Уведомление о явке представителя по результатам проверки",
                    Name = "Уведомление",
                    Template = Properties.Resources.NotificationAttendanceByRepresentativeCheckResults
                }
            };
        }
    }
}