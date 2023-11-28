namespace Bars.GkhGji.Regions.Voronezh.Interceptors
{
    using System;
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using System.Linq;
    using Bars.GkhGji.Regions.Voronezh.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol197;
    using System.Text;

    class Protocol197ViolationInterceptor : EmptyDomainInterceptor<Protocol197Violation>
    {
        public IGkhUserManager UserManager { get; set; }

        public IDomainService<Protocol197LongText> Protocol197LongTextDomain { get; set; }

        public IDomainService<Protocol197> Protocol197Domain { get; set; }
        public IDomainService<Protocol197ArticleLaw> Protocol197ArticleLawDomain { get; set; }
        public IDomainService<ViolationGji> ViolationGjiDomain { get; set; }

        public override IDataResult AfterCreateAction(IDomainService<Protocol197Violation> service, Protocol197Violation entity)
        {
            try
            {
                //ставим статью закона
                if (entity.InspectionViolation.Violation.ArticleLaw != null)
                {
                    var existsArtLaw = Protocol197ArticleLawDomain.GetAll().FirstOrDefault(x => x.Protocol197.Id == entity.Document.Id && x.ArticleLaw == entity.InspectionViolation.Violation.ArticleLaw);
                    if (existsArtLaw == null)
                    {
                        Protocol197ArticleLawDomain.Save(new Protocol197ArticleLaw
                        {
                            ArticleLaw = entity.InspectionViolation.Violation.ArticleLaw,
                            Description = "Добавлена автоматически",
                            Protocol197 = new Protocol197 {Id = entity.Document.Id }
                        });
                    }
                }
                //Encoding.UTF8.GetBytes(value)
                var prot = Protocol197Domain.Get(entity.Document.Id);
                var violation = ViolationGjiDomain.Get(entity.InspectionViolation.Violation.Id);
                string violDate = prot.DateOfViolation.HasValue ? prot.DateOfViolation.Value.ToString("dd.MM.yyyy") : "Указать дату нарушения";
                string violTimeH = prot.HourOfViolation.HasValue ? prot.HourOfViolation.Value.ToString().PadLeft(2, '0') : "--";
                string violTimeM = prot.MinuteOfViolation.HasValue ? prot.MinuteOfViolation.Value.ToString().PadLeft(2, '0') : "00";
                var violatorName = GetViolatorName(prot);
                string addressviol = prot.FiasPlaceAddress != null ? prot.FiasPlaceAddress.AddressName : "<b>Адрес</b>";
                string violtext = $"{violDate} в {violTimeH}:{violTimeM} {prot.AddressPlace} по адресу: {addressviol}, {violatorName} допустил(а) {violation.Name.ToLower()}, ";
                string am = string.Empty;// $"{prot.NameTransport} г/н {prot.NamberTransport}";
                if (prot.Transport != null)
                {
                    am = $"{prot.Transport.Transport.NameTransport} г/н {prot.Transport.Transport.NamberTransport} ";
                }
                if (!string.IsNullOrEmpty(am))
                {
                    violtext = violtext.Replace("транспортное средство", $"транспортное средство {am}");
                    violtext = violtext.Replace("транспортного средства", $"транспортного средства {am}");
                    violtext = violtext.Replace("транспортным средством", $"транспортным средством {am}");
                    violtext = violtext.Replace("транспортном средстве", $"транспортном средстве {am}");
                }
                violtext += $"что повлекло нарушение {violation.NormativeDocNames}.";

                if (violation.ParentViolationGji != null)
                {
                    var pv = ViolationGjiDomain.Get(violation.ParentViolationGji.Id);
                    violtext += $" {pv.Name} нарушает {pv.NormativeDocNames}";
                }
                var plt = Protocol197LongTextDomain.GetAll().FirstOrDefault(x => x.Protocol197 == prot);
                if (plt != null)
                {
                    plt.Violations = Encoding.UTF8.GetBytes(violtext);
                    Protocol197LongTextDomain.Update(plt);
                }
                else
                {
                    Protocol197LongTextDomain.Save(new Protocol197LongText
                    {
                        Protocol197 = prot,
                        Violations = Encoding.UTF8.GetBytes(violtext)
                    });
                }

                return Success();
            }
            catch (Exception e)
            {
                return Failure("Ошибка создания текста нарушения");
            }

        }

        private string GetViolatorName(Protocol197 prot)
        {
            switch (prot.Executant.Code)
            {
                case "8":
                    return prot.IndividualPerson.Fio;
                case "0":
                    return $"{prot.Contragent.Name}, являясь юридическим лицом ";
                case "1":
                    return $"{prot.IndividualPerson.Fio}, являясь должностным лицом - {prot.PersonPosition} {prot.Contragent.Name} ";
                case "13":
                    return $"{prot.IndividualPerson.Fio}, являясь индивидуальным предпринимателем,";
                default:
                    return prot.IndividualPerson.Fio;

            }
        }
    }
}
