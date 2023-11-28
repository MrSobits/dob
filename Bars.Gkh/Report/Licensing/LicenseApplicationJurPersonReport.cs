﻿namespace Bars.Gkh.Report.Licensing
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.Reports;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Properties;
    using Bars.Gkh.Report;
    using Stimulsoft.Report;

    public class LicenseApplicationJurPersonReport : GkhBaseStimulReport
    {
        private long _requestId;

        private ManOrgLicenseRequest Request { get; set; }

        public IDomainService<ManOrgLicenseRequest> RequestDomain { get; set; }

        public IDomainService<ManOrgRequestPerson> RequestPersonDomain { get; set; }

        public IDomainService<PersonQualificationCertificate> CertificateDomain { get; set; }

        public IDomainService<PersonPlaceWork> PlaceWorkDomain { get; set; }

        public LicenseApplicationJurPersonReport()
            : base(new ReportTemplateBinary(Resources.LicenseApplicationJurPerson))
        {
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var persons = RequestPersonDomain.GetAll().Where(x => x.LicRequest.Id == _requestId).Select(x => x.Person).ToArray();
            var personsIds = persons.Select(x => x.Id).ToArray();
            var certificates = CertificateDomain.GetAll().Where(x => personsIds.Contains(x.Person.Id)).Select(x => new
            {
                x.Number,
                x.IssuedDate
            }).ToArray().Select(x =>
            {
                var result = "№ " + x.Number;
                if (x.IssuedDate.HasValue)
                {
                    result += " от " + x.IssuedDate.Value.ToShortDateString();
                }

                return result;
            });

            var certificateString = string.Join(", ", certificates);

            var placeWorkDict = PlaceWorkDomain.GetAll()
               .Where(x => x.StartDate.HasValue)
               .Where(x => personsIds.Contains(x.Person.Id))
               .Select(x => new
               {
                   personId = x.Person.Id,
                   position = x.Position.Name,
                   date = x.StartDate.Value
               })
               .AsEnumerable()
               .GroupBy(x => x.personId)
               .ToDictionary(x => x.Key, y => y.OrderByDescending(z => z.date).Select(z => z.position).First());


            Report["СоискательЛицензииСокр"] = Request.Contragent.ShortName;
            Report["СоискательЛицензииПолн"] = Request.Contragent.Name;
            Report["ОПФ"] = Request.Contragent.OrganizationForm.Name;
            Report["ФактАдрес"] = Request.Contragent.FactAddress;
            Report["ОГРН"] = Request.Contragent.Ogrn;
            Report["РеквизитыЕГРЮЛ"] = Request.Contragent.OgrnRegistration;
            Report["ИНН"] = Request.Contragent.Inn;
            Report["КвалАттестат"] = certificateString;
            Report["Сайт"] = Request.Contragent.OfficialWebsite;
            Report["УплатаГосПошлины"] = Request.ConfirmationOfDuty;
            Report["Телефон"] = Request.Contragent.Phone;
            Report["ЭлПочта"] = Request.Contragent.Email;
            Report["ФИОСоискателя"] = string.Join(", ", persons.Select(x => x.FullName));
            Report["ДолжностьСоискателя"] = string.Join(", ", persons.Select(x => placeWorkDict.ContainsKey(x.Id) ? placeWorkDict[x.Id] : string.Empty));
            Report["РеквизитыДокументаОПостановкеНаУчет"] =
                string.Format("Свидетельство о постановке на учет юридического лица серии {0} № {1} от {2}., выданное {3}",
                    Request.Contragent.TaxRegistrationSeries,
                    Request.Contragent.TaxRegistrationNumber,
                    Request.Contragent.TaxRegistrationDate.HasValue
                        ? " от " + Request.Contragent.TaxRegistrationDate.Value.ToShortDateString()
                        : string.Empty,
                    Request.Contragent.TaxRegistrationIssuedBy);
        }

        public override string Id
        {
            get { return "LicenseApplicationJurPerson"; }
        }

        public override string CodeForm
        {
            get { return "ManOrgLicenseRequest"; }
        }

        public override string Permission
        {
            get { return "Reports.GKH.ManOrgLicenseRequestLicenseApplicationJurPerson"; }
        }

        public override string Name
        {
            get { return "Заявление о предоставлении лицензии (реорганизация юр. лица)"; }
        }

        public override string Description
        {
            get { return "Заявление о предоставлении лицензии (реорганизация юр. лица)"; }
        }

        protected override string CodeTemplate { get; set; }

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            _requestId = userParamsValues.GetValue<long>("RequestId");
            Request = RequestDomain.Get(_requestId);
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
                    Code = "LicenseApplicationJurPerson",
                    Description = "Заявление о предоставлении лицензии (реорганизация юр. лица)",
                    Name = "LicenseApplicationJurPerson",
                    Template = Resources.LicenseApplicationJurPerson
                }
            };
        }
    }
}