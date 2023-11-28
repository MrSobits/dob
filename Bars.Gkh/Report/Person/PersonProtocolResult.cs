using Bars.Gkh.Utils;
using Stimulsoft.Report;

namespace Bars.Gkh.Report
{
    using Bars.B4;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using B4.Modules.Reports;
    using System.Linq;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Config;
    using Slepov.Russian.Morpher;
    using Bars.B4.Utils;

    class PersonProtocolResult : GkhBaseStimulReport
    {

        #region Fields
        private long requestId;

        protected Person Person { get; set; }

        protected PersonRequestToExam PersonRequestToExam { get; set; }
        #endregion

        public PersonProtocolResult()
            : base(new ReportTemplateBinary(Properties.Resources.PersonProtocolResult))
        {
        }
        
        public IDomainService<QExamQuestion> ExamQuestionDomain { get; set; }

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            this.requestId = userParamsValues.GetValue<object>("PersonId").ToLong();
        }

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
            {
                new TemplateInfo
                    {
                        Code = "PersonProtocolResult_1",
                        Name = "PersonProtocolResult",
                        Description =
                            "Протокол результатов компьютерного тестирования",
                        Template = Properties.Resources.PersonRequestToExamReport
                    }
            };
        }

        public override string CodeForm
        {
            get { return "Person"; }
        }

        public override string Name
        {
            get { return "Протокол результатов компьютерного тестирования"; }
        }

        public override string Description
        {
            get { return "Протокол результатов компьютерного тестирования"; }
        }

        protected override string CodeTemplate { get; set; }

        public override string Id
        {
            get { return "PersonProtocolResult"; }
        }

        public override string Extention
        {
            get { return "mrt"; }
        }

        public override StiExportFormat ExportFormat
        {
            get
            {
                return StiExportFormat.Word2007;
            }
            set { }
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var endExamTime = ExamQuestionDomain.GetAll()
                            .Where(x => x.PersonRequestToExam.Id == this.requestId)
                            .OrderByDescending(x => x.ObjectEditDate).FirstOrDefault();

            double questionCount = ExamQuestionDomain.GetAll()
                                .Where(x => x.PersonRequestToExam.Id == requestId)
                                .Count();

            TimeSpan examTime = endExamTime.ObjectEditDate - endExamTime.ObjectCreateDate;

            Report["ЯвкаНаТестирование"] = PersonRequestToExam.CorrectAnswersPercent == null ? "+" : "-";
            Report["ВремяНачала"] = endExamTime.ObjectCreateDate.ToString("hh:mm");
            Report["ВремяОкончания"] = endExamTime.ObjectEditDate.ToString("hh:mm");
            Report["ЗатратыВремени"] = examTime;
            Report["ОценкаВбаллах"] = PersonRequestToExam.CorrectAnswersPercent;
            Report["ОценкаВпроцентах"] = Math.Round(Convert.ToDouble(PersonRequestToExam.CorrectAnswersPercent) / (questionCount * 2) * 100, 2);
            Report["Фамилия"] = Person.Surname;
            Report["Имя"] = Person.Name;
            Report["Отчество"] = Person.Patronymic;
            Report["ДатаТестирования"] = PersonRequestToExam.ExamDate;
        }
    }
}
