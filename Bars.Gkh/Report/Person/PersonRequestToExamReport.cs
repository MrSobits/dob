﻿using Bars.Gkh.Utils;
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

    class PersonRequestToExamReport : GkhBaseStimulReport
    {

        #region Fields
        private long personId;

        protected Person Person { get; set; }
        #endregion

        public PersonRequestToExamReport()
            : base(new ReportTemplateBinary(Properties.Resources.PersonRequestToExamReport))
        {
        }

        public IDomainService<Person> PersonDomain { get; set; }

        public IDomainService<PersonPlaceWork> PersonPlaceWorkDomain { get; set; }

        public IGkhParams GkhParams { get; set; }

        private Склонятель _morpher;

        protected Склонятель GetMorpher()
        {
            return _morpher ?? (_morpher = new Склонятель("SonFhyB1DbaxkkAQ4tfrhQ=="));
        }

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            personId = userParamsValues.GetValue<object>("PersonId").ToLong();

            Person = PersonDomain.GetAll().FirstOrDefault(x => x.Id == personId);

            if (Person == null)
            {
                throw new Exception(string.Format("Не удалось определить должностное лицо по Id {0}", personId));
            }
        }

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
            {
                new TemplateInfo
                    {
                        Code = "PersonRequestToExamReport_1",
                        Name = "Person",
                        Description =
                            "Заявление на прохождение экзамена",
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
            get { return "Заявление на прохождение экзамена"; }
        }

        public override string Description
        {
            get { return "Должностное лицо - Заявление на прохождение экзамена"; }
        }

        protected override string CodeTemplate { get; set; }

        public override string Id
        {
            get { return "PersonRequestToExamReport"; }
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
            var morpher = GetMorpher();
            var surname = morpher.Проанализировать(Person.Surname ?? "");
            var name = morpher.Проанализировать(Person.Name ?? "");
            var patronymic = morpher.Проанализировать(Person.Patronymic ?? "");

            Report["ФИОРодП"] = "{0} {1} {2}".FormatUsing(
                surname != null ? surname.Винительный : "",
                name != null ? name.Винительный : "",
                patronymic != null ? patronymic.Винительный : "");

            Report["ДокументСерия"] = Person.IdSerial;
            Report["ДокументНомер"] = Person.IdNumber;
            Report["ДокументВыдан"] = Person.IdIssuedBy;
            Report["ДатаВыдачи"] = Person.IdIssuedDate.HasValue ? Person.IdIssuedDate.Value : DateTime.MinValue;

            Report["ДокументВыданТворП"] = string.Empty;
            if (!string.IsNullOrEmpty(Person.IdIssuedBy))
            {
                var issuedBy = morpher.Проанализировать(Person.IdIssuedBy);

                Report["ДокументВыданТворП"] = issuedBy != null ? issuedBy.Творительный : "";
            }

            Report["Email"] = Person.Email;
            Report["Фамилия"] = Person.Surname;
            Report["Имя"] = Person.Name;
            Report["Отчество"] = Person.Patronymic;
            Report["ТипДокумента"] = Person.TypeIdentityDocument.GetEnumMeta().Display;
            Report["АдресРегистрации"] = Person.AddressReg;
            Report["Телефон"] = Person.Phone;
            Report["ДатаРождения"] = Person.Birthdate.ToDateString();
            Report["МестоРождения"] = Person.AddressBirth;
            Report["ИНН"] = Person.Inn;

            var fioSokr = string.Empty;
            
            if( !string.IsNullOrEmpty(Person.Name) && !string.IsNullOrEmpty(Person.Name.Trim()))
            {
                fioSokr += Person.Name.Trim().Substring(0, 1).ToUpper() + ".";
            }

            if (!string.IsNullOrEmpty(Person.Patronymic) && !string.IsNullOrEmpty(Person.Patronymic.Trim()))
            {
                fioSokr += Person.Patronymic.Trim().Substring(0, 1).ToUpper() + ".";
            }

            if (!string.IsNullOrEmpty(Person.Surname) && !string.IsNullOrEmpty(Person.Surname.Trim()))
            {
                fioSokr = string.Format("{0} {1}", fioSokr, Person.Surname.Trim());
            }

            Report["ИОФамилия"] = fioSokr.Trim();

            var currentDate = DateTime.Now;
            var placeWork = string.Empty;
            var contragentNames =  PersonPlaceWorkDomain.GetAll()
                    .Where(
                        x =>
                            x.Person.Id == Person.Id && x.StartDate <= currentDate &&
                            (!x.EndDate.HasValue || x.EndDate >= currentDate))
                    .Select(x => x.Contragent.Name)
                    .Distinct()
                    .ToList();

            if (contragentNames.Any())
            {
                placeWork = contragentNames.AggregateWithSeparator(", ");
            }

            Report["МестоРаботы"] = placeWork;
        }
    }
}
