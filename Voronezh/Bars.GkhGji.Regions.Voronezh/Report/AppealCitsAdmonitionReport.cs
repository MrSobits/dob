namespace Bars.GkhGji.Regions.Voronezh.Report
{
    using System.Collections.Generic;
    using System.Linq;
    using B4.DataAccess;
    using B4.Modules.Reports;
    using B4.Utils;
    using Entities;
    using Stimulsoft.Report;
    using Utils;
    using Gkh.Report;
    using System;
    using B4;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Извещение
    /// </summary>
    public class AppealCitsAdmonitionReport : GkhBaseStimulReport
    {
        #region .ctor

        /// <summary>
        /// .ctor
        /// </summary>
        public AppealCitsAdmonitionReport()
            : base(new ReportTemplateBinary(Properties.Resources.AppealCitsAdmonitionReport))
        {
        }

        #endregion .ctor

        #region Private fields

        private long _appealCitsAdmonitionId;

        #endregion Private fields

        #region Protected properties

        /// <summary>
        /// Код шаблона (файла)
        /// </summary>
        protected override string CodeTemplate { get; set; }

        #endregion Protected properties

        #region Public properties

        /// <summary>
        /// Наименование отчета
        /// </summary>
        public override string Name
        {
            get { return "Предостережение"; }
        }

        /// <summary>
        /// Описание отчета
        /// </summary>
        public override string Description
        {
            get { return "Предостережение"; }
        }

        /// <summary>
        /// Идентификатор отчета
        /// </summary>
        public override string Id
        {
            get { return "AppealCitsAdmonition"; }
        }

        /// <summary>
        /// Код формы, на которой находится кнопка печати
        /// </summary>
        public override string CodeForm
        {
            get { return "AppealCitsAdmonition"; }
        }

        /// <summary>Формат печатной формы</summary>
        public override StiExportFormat ExportFormat
        {
            get { return StiExportFormat.Word2007; }
            set { }
        }

        #endregion Public properties

        #region Public methods

        /// <summary>
        /// Подготовить параметры отчета
        /// </summary>
        /// <param name="reportParams"></param>
        public override void PrepareReport(ReportParams reportParams)
        {
            var appealCitsAdmonitionDomain = Container.ResolveDomain<AppealCitsAdmonition>();

            var data = appealCitsAdmonitionDomain.GetAll()
                .Where(x => x.Id == _appealCitsAdmonitionId).FirstOrDefault();

            try
            {
                Report["Обр_Номер"] = data.AppealCits.NumberGji;
                Report["Обр_Дата"] = data.AppealCits.DateFrom.HasValue ? data.AppealCits.DateFrom.Value.ToString("dd.MM.yyyy") : "";
                Report["Контрагент"] = data.Contragent.Name;
                Report["Id"] = data.Id;
                Report["Контрагент_Адр"] = data.Contragent.JuridicalAddress;
                Report["Контрагент_ИНН"] = data.Contragent.Inn;
                Report["Контрагент_ОГРН"] = data.Contragent.Ogrn;
                Report["Номер"] = data.DocumentNumber;
                Report["Дата"] = data.DocumentDate.HasValue ? data.DocumentDate.Value.ToString("dd.MM.yyyy") : "";
                Report["Дата_Исполнения"] = data.PerfomanceDate.HasValue ? data.PerfomanceDate.Value.ToString("dd.MM.yyyy") : "";
                Report["Дата_Факт_Исполнения"] = data.PerfomanceFactDate.HasValue ? data.PerfomanceFactDate.Value.ToString("dd.MM.yyyy") : "";
                Report["Должнострое_лицо"] = data.Inspector != null ? data.Inspector.Fio : "";
                Report["Должность"] = data.Inspector != null ? data.Inspector.Position : "";
            }
            finally
            {

            }

            var appCitAdmonVoilationDomain = Container.ResolveDomain<AppCitAdmonVoilation>();
            Int64 id = data.Id;

            var appCitAdmonVoilation = appCitAdmonVoilationDomain.GetAll()
                 .Where(x => x.AppealCitsAdmonition.Id == id)
                 .Select(x => new
                 {
                     ViolationGji = x.ViolationGji.Name,
                     PlanedDate = x.PlanedDate,
                     FactDate = x.FactDate
                 });

            Report["Срок"] = appCitAdmonVoilation.OrderByDescending(x => x.PlanedDate).Select(x => x.PlanedDate).FirstOrDefault().HasValue 
                                ? appCitAdmonVoilation.OrderByDescending(x => x.PlanedDate).Select(x => x.PlanedDate).FirstOrDefault().Value.ToString("dd.MM.yyyy") : "";


            var appCitAdmonVoilationResult = appCitAdmonVoilation
                 .Select(x => new
                 {
                     ViolationGji = x.ViolationGji,
                     PlanedDate = x.PlanedDate.HasValue ? x.PlanedDate.Value.ToString("dd.MM.yyyy") : "",
                     FactDate = x.FactDate.HasValue ? x.FactDate.Value.ToString("dd.MM.yyyy") : ""
                 }).ToList();

            Report.RegData("AppCitAdmonVoilation", appCitAdmonVoilationResult);
            var appCitRODomain = Container.ResolveDomain<AppealCitsRealityObject>();
            var appCitRO = appCitRODomain.GetAll()
                .Where(x => x.AppealCits.Id == data.AppealCits.Id)
                .Select(x => x.RealityObject.Municipality.Name + ", " + x.RealityObject.Address).FirstOrDefault();
            Report["АдресМКД"] = appCitRO;
        }

        /// <summary>
        /// Установить пользовательские параметры
        /// </summary>
        /// <param name="userParamsValues"></param>
        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            _appealCitsAdmonitionId = userParamsValues.GetValue<long>("Id");
        }

        /// <summary>
        /// Получить список шаблонов
        /// </summary>
        /// <returns></returns>
        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
            {
                new TemplateInfo
                {
                    Name = "Предостережение",
                    Description = "Предостережение",
                    Code = "AppealCitsAdmonitionReport",
                    Template = Properties.Resources.AppealCitsAdmonitionReport
                }
            };
        }

        #endregion Public methods
    }
}