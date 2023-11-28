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

    public class LicenseRenewalApplicationReport : GkhBaseStimulReport
    {
        private long _requestId;

        private ManOrgLicenseRequest Request { get; set; }

        public IDomainService<ManOrgLicenseRequest> RequestDomain { get; set; }
        public IDomainService<ManOrgLicense> LicenseDomain { get; set; }

        public IDomainService<ManOrgRequestPerson> RequestPersonDomain { get; set; }

        public IDomainService<PersonQualificationCertificate> CertificateDomain { get; set; }

        public IDomainService<PersonPlaceWork> PlaceWorkDomain { get; set; }

        public LicenseRenewalApplicationReport()
            : base(new ReportTemplateBinary(Resources.LicenseRenewalApplication))
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

            var licenses = LicenseDomain.GetAll().Where(x => x.Request != null && x.Request.Id == _requestId)
                .Select(x => new
                {
                    x.LicNumber,
                    x.DateRegister
                })
                .ToArray()
                .Select(x =>
                {
                    var result = "№ " + x.LicNumber;
                    if (x.DateRegister.HasValue)
                    {
                        result += " от " + x.DateRegister.Value.ToShortDateString();
                    }

                    return result;
                });
            var licensesString = string.Join(", ", licenses);


            Report["СоискательЛицензииСокр"] = Request.Contragent.ShortName;
            Report["РеквизитыЛицензии"] = licensesString;
            Report["НаименованиеЛицОрганаДатПадеж"] = Request.Contragent.NameDative;
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
        }

        public override string Id
        {
            get { return "LicenseRenewalApplication"; }
        }

        public override string CodeForm
        {
            get { return "ManOrgLicenseRequest"; }
        }

        public override string Permission
        {
            get { return "Reports.GKH.ManOrgLicenseRequestLicenseRenewalApplication"; }
        }

        public override string Name
        {
            get { return "Заявление о переоформлении лицензии"; }
        }

        public override string Description
        {
            get { return "Заявление о переоформлении лицензии"; }
        }

        /// <summary>Формат печатной формы</summary>
        public override StiExportFormat ExportFormat
        {
            get { return StiExportFormat.Word2007; }
            set { }
        }

        protected override string CodeTemplate { get; set; }

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            _requestId = userParamsValues.GetValue<long>("RequestId");
            Request = RequestDomain.Get(_requestId);
        }

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
            {
                new TemplateInfo
                {
                    Code = "LicenseRenewalApplication",
                    Description = "Заявление о переоформлении лицензии",
                    Name = "LicenseRenewalApplication",
                    Template = Resources.LicenseRenewalApplication
                }
            };
        }
    }
}