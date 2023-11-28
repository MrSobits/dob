namespace Bars.Gkh.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4.DataAccess;
    using B4.Modules.Reports;
    using B4.Utils;
    using Entities;
    using Stimulsoft.Report;
    using Utils;

    /// <summary>
    /// Приказ запроса на получение лицензии
    /// </summary>
    public class ManOrgLicenseRequestOrderReport : GkhBaseStimulReport
    {
        #region .ctor

        /// <summary>
        /// .ctor
        /// </summary>
        public ManOrgLicenseRequestOrderReport()
            : base(new ReportTemplateBinary(Properties.Resources.ManOrgLicenseRequestOrderReport))
        {
        }

        #endregion .ctor

        #region Private fields

        private long requestId;

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
            get { return "Материалы правонарушения"; }
        }

        /// <summary>
        /// Описание отчета
        /// </summary>
        public override string Description
        {
            get { return "Приказ запроса на получение лицензии"; }
        }

        /// <summary>
        /// Идентификатор отчета
        /// </summary>
        public override string Id
        {
            get { return "ManOrgLicenseRequestOrderReport"; }
        }

        /// <summary>
        /// Код формы, на которой находится кнопка печати
        /// </summary>
        public override string CodeForm
        {
            get { return "ManOrgLicense"; }
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
            var requestDomain = Container.ResolveDomain<ManOrgLicenseRequest>();
            var licenseDomain = Container.ResolveDomain<ManOrgLicense>();
            var dbConfigProvider = Container.Resolve<IDbConfigProvider>();

            try
            {
                var license = licenseDomain.Get(this.requestId);
                var request = license.Request;
                if(request == null) return;

                Report["УоИнн"] = request.Contragent.Return(x => x.Inn);
                Report["УоОгрн"] = request.Contragent.Return(x => x.Ogrn);
                Report["УоНаименование"] = request.Contragent.Return(x => x.Name);
                Report["УоКраткоеНаименование"] = request.Contragent.Return(x => x.ShortName);

                Report["ИдентификаторДокументаГЖИ"] = request.Id.ToString();
                Report["СтрокаПодключениякБД"] = dbConfigProvider.ConnectionString;



                if (license.IsNull()) { return;}

                Report["ЛицензияДатаПриказа"] = license.DateDisposal.ToDateString();
                Report["ЛицензияНомерПриказа"] = license.DisposalNumber;

            }
            finally
            {
                Container.Release(requestDomain);
                Container.Release(licenseDomain);
                Container.Release(dbConfigProvider);
            }
        }

        /// <summary>
        /// Установить пользовательские параметры
        /// </summary>
        /// <param name="userParamsValues"></param>
        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            this.requestId = userParamsValues.GetValue<long>("LicenseId");
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
                    Name = "Материалы правонарушения",
                    Description = "Приказ запроса на получение лицензии",
                    Code = "ManOrgLicenseRequestOrderReport",
                    Template = Properties.Resources.ManOrgLicenseRequestOrderReport
                }
            };
        }

        #endregion Public methods
    }
}