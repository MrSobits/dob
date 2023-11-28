﻿namespace Bars.GkhGji.Regions.Chelyabinsk.Report
{
    using System.Collections.Generic;
    using System.Linq;
    using B4.DataAccess;
    using B4.Modules.Reports;
    using B4.Utils;
    using Entities;
    using Bars.GkhGji.Entities;
    using Stimulsoft.Report;
    using Utils;
    using Gkh.Report;
    using System;
    using B4;

    /// <summary>
    /// Извещение
    /// </summary>
    public class SMEVEGRULReport : GkhBaseStimulReport
    {
        #region .ctor

        /// <summary>
        /// .ctor
        /// </summary>
        public SMEVEGRULReport()
            : base(new ReportTemplateBinary(Properties.Resources.SMEVEGRULReport))
        {
        }

        #endregion .ctor

        #region Private fields

        private long _SMEVEGRULId;

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
            get { return "Выписка"; }
        }

        /// <summary>
        /// Описание отчета
        /// </summary>
        public override string Description
        {
            get { return "Выписка"; }
        }

        /// <summary>
        /// Идентификатор отчета
        /// </summary>
        public override string Id
        {
            get { return "SMEVEGRUL"; }
        }

        /// <summary>
        /// Код формы, на которой находится кнопка печати
        /// </summary>
        public override string CodeForm
        {
            get { return "SMEVEGRUL"; }
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
            var SMEVEGRULDomain = Container.ResolveDomain<SMEVEGRUL>();

            var data = SMEVEGRULDomain.GetAll()
                .Where(x => x.Id == _SMEVEGRULId).FirstOrDefault();

            try
            {
                Report["ДатаВыписки"] = data.ResponceDate.HasValue? data.ResponceDate.Value.ToShortDateString():"";
                Report["НомерВыписки"] = data.MessageId;
                Report["ПолноеНаименование"] = data.Name;
                Report["СокрНаим"] = data.ShortName;
                Report["ОГРН"] = data.OGRN;
                Report["ДатаОГРН"] = data.OGRNDate.HasValue? data.OGRNDate.Value.ToShortDateString(): "";
                Report["ИНН"] = data.INN;
                Report["КПП"] = data.KPP;
                Report["ОПФ"] = data.OPFName;
                Report["АдресРегистрации"] = data.AddressUL;
                Report["ТипУК"] = data.AuthorizedCapitalType;
                Report["РазмерУК"] = data.AuthorizedCapitalAmmount.ToString();
                Report["ВидРег"] = data.CreateWayName;
                Report["КодОргана"] = data.CodeRegOrg;
                Report["РегОрган"] = data.RegOrgName;
                Report["АдресРегОрг"] = data.AddressRegOrg;
                Report["Должность"] = data.Pozition;
                Report["ФИО"] = data.FIO;
                Report["КодОквэд"] = data.OKVEDCodes;
                Report["ОКВЭД"] = data.OKVEDNames;
                Report["Статус"] = data.StateNameUL;
            }
            finally
            {

            }

            
        }

        /// <summary>
        /// Установить пользовательские параметры
        /// </summary>
        /// <param name="userParamsValues"></param>
        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            _SMEVEGRULId = userParamsValues.GetValue<long>("Id");
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
                    Name = "Выписка",
                    Description = "Выписка ЕГРЮЛ",
                    Code = "SMEVEGRULReport",
                    Template = Properties.Resources.SMEVEGRULReport
                }
            };
        }

        #endregion Public methods
    }
}