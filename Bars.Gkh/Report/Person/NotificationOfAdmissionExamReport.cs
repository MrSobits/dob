namespace Bars.Gkh.Report
{
    using B4.Utils;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Reports;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Properties;
    using Stimulsoft.Report;
    using System.Collections.Generic;

    public class NotificationOfAdmissionExamReport : GkhBaseStimulReport
    {
        private long requestId;
        
        public IDomainService<PersonRequestToExam> RequestDomain { get; set; }

        public IDbConfigProvider dbConfigProvider { get; set; }

        public NotificationOfAdmissionExamReport() : base(new ReportTemplateBinary(Resources.NotificationOfAdmissionExam))
        {
        }

        public override string Permission
        {
            get { return "Reports.GKH.NotificationOfAdmissionExamReport"; }
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
            get { return "NotificationOfAdmissionExamReport"; }
        }

        public override string CodeForm
        {
            get { return "Person"; }
        }

        public override string Name
        {
            get { return "Уведомление о допуске к экзамену"; }
        }

        public override string Description
        {
            get { return "Уведомление о допуске к экзамену"; }
        }

        public override string Extention
        {
            get { return "mrt"; }
        }

        protected override string CodeTemplate { get; set; }

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            this.requestId = userParamsValues.GetValue<object>("PersonId").ToLong();
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
                    Code = "NotificationOfAdmissionExamReport",
                    Description = "Уведомление о допуске к экзамену",
                    Name = "Уведомление о допуске к экзамену",
                    Template = Resources.NotificationOfAdmissionExam
                }
            };
        }
    }
}