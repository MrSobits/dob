namespace Bars.GkhGji.Regions.BaseChelyabinsk.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Interceptors;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol197;
    using B4.DataAccess;
    using System;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.Gkh.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Authentification;
    using Bars.B4.Modules.States;

    public class Protocol197ServiceInterceptor : DocumentGjiInterceptor<Protocol197>
    {
        public IGkhUserManager UserManager { get; set; }
        public override IDataResult BeforeCreateAction(IDomainService<Protocol197> service, Protocol197 entity)
        {
            var domainServiceInspection = this.Container.Resolve<IDomainService<BaseDefault>>();
            var domainStage = this.Container.Resolve<IDomainService<InspectionGjiStage>>();
            var personDocService = this.Container.Resolve<IDomainService<PhysicalPersonDocType>>();

            try
            {
                var inspectionNum = GetNextNum();
                var newInspection = new BaseDefault()
                {
                    TypeBase = TypeBase.Protocol197,
                    InspectionNum = inspectionNum,
                    InspectionNumber = inspectionNum.ToString()
                };

                domainServiceInspection.Save(newInspection);

                var newStage = new InspectionGjiStage
                {
                    Inspection = newInspection,
                    TypeStage = TypeStage.Protocol197,
                    Position = 0
                };

                domainStage.Save(newStage);

                entity.TypeDocumentGji = TypeDocumentGji.Protocol197;
                entity.Inspection = newInspection;
                entity.Stage = newStage;
                entity.CaseNumber = inspectionNum.ToString();
                entity.PhysicalPersonDocType = personDocService.GetAll().Where(x => x.Name == "Паспорт гражданина Российской Федерации").Select(x => x).FirstOrDefault();

                if (entity.DocumentNum != null)
                {
                    string UIN = GetUIN();
                    //string UIN = "39645f";
                    if (UIN != null)
                    {
                        string s1 = Convert.ToInt32(UIN, 16).ToString().PadLeft(8, '0');
                        string s2 = (entity.DocumentDate?.ToString("yyyyMMdd") ?? "00000000");
                        string s3 = "";
                        if (entity.Inspection.InspectionNumber.Contains("-"))
                        {
                            if (entity.Inspection.InspectionNumber.Split('-').Count() > 2)
                            {
                                s3 = (entity.Inspection.InspectionNumber.Split('-')[1] + entity.Inspection.InspectionNumber.Split('-')[2]).PadRight(8, '0');
                            }
                            else if (entity.Inspection.InspectionNumber.Split('-').Count() == 2)
                            {
                                s3 = entity.Inspection.InspectionNumber.Split('-')[1].PadRight(8, '0');
                            }
                            else
                            {
                                s3 = entity.Inspection.InspectionNumber.Replace("-", "").PadRight(8, '0');
                            }
                        }
                        else
                        {
                            s3 = entity.Inspection.InspectionNumber.PadRight(8, '0');
                        }
                        s3 = s3.Replace('/', '1');
                        s3 = s3.Replace('\\', '0');
                        s3 = s3.Replace('№', '4');
                        char[] charsS3 = s3.ToCharArray();
                        for (int i = 0; i < s3.Length; i++)
                        {
                            if (!char.IsDigit(charsS3[i]))
                            {
                                s3 = s3.Replace(charsS3[i], '0');
                            }
                        }
                        entity.UIN = (s1 + s2 + s3).Substring(0, 24);
                        entity.UIN += CheckSum(entity.UIN);

                    }
                }

                return base.BeforeCreateAction(service, entity);
            }
            finally
            {
                this.Container.Release(domainServiceInspection);
                this.Container.Release(domainStage);
            }
        }

        private Int32 CheckSum(String number)
        {
            Int32 result = CheckSum(number, 1);

            return result != 10 ? result : CheckSum(number, 3) % 10;
        }

        private Int32 CheckSum(String number, Int32 ves)
        {
            int sum = 0;
            for (int i = 0; i < number.Length; i++)
            {
                int t = (int)Char.GetNumericValue(number[i]);
                int rrr = ((ves % 10) == 0 ? 10 : ves % 10);

                sum += t * rrr;
                ves++;
            }

            return sum % 11;
        }

        public override IDataResult BeforeUpdateAction(IDomainService<Protocol197> service, Protocol197 entity)
        {
            if (entity.DateOfProceedings.HasValue && entity.DateOfProceedings.Value > entity.DocumentDate.Value.AddDays(60))
            {
                return Failure("Дата рассмотрения превышает 60 дней с даты составления протокола, рассмотрение дела должно быть назначено на более ранний срок");
            }

            if (entity.Transport == null && !string.IsNullOrEmpty(entity.NameTransport) && !string.IsNullOrEmpty(entity.NamberTransport))
            {
                if (entity.IndividualPerson != null)
                {
                    var ownerDomain = Container.Resolve<IDomainService<Owner>>();
                    var owner = ownerDomain.GetAll()
                        .Where(x => x.IndividualPerson == entity.IndividualPerson && x.NamberTransport == entity.NamberTransport).FirstOrDefault();
                    if (owner != null)
                    {
                        entity.Transport = owner;
                    }
                    else
                    {
                        var newowner = new Owner
                        {
                            IndividualPerson = entity.IndividualPerson,
                            NamberTransport = entity.NamberTransport,
                            NameTransport = entity.NameTransport,
                            TypeViolator = TypeViolator.PhisicalPerson,
                            DataOwnerStart = DateTime.Now.AddMonths(-1)
                        };
                        ownerDomain.Save(newowner);
                        entity.Transport = newowner;
                    }
                }
            }

            if (entity.FiasPlaceAddress != null)
            {
                Utils.SaveFiasAddress(this.Container, entity.FiasPlaceAddress);
                //   return base.BeforeUpdateAction(service, entity);
            }
            if (entity.FiasRegistrationAddress != null)
            {
                Utils.SaveFiasAddress(this.Container, entity.FiasRegistrationAddress);
                //   return base.BeforeUpdateAction(service, entity);
            }
            if (entity.FiasFactAddress != null)
            {
                Utils.SaveFiasAddress(this.Container, entity.FiasFactAddress);
                //   return base.BeforeUpdateAction(service, entity);
            }
            var servStateProvider = this.Container.Resolve<IStateProvider>();
            var ComissionMeetingDomain = this.Container.Resolve<IDomainService<ComissionMeeting>>();
            var ComissionMeetingDocumentDomain = this.Container.Resolve<IDomainService<ComissionMeetingDocument>>();
            var ZonalInspectionInspectorDomain = this.Container.Resolve<IDomainService<ZonalInspectionInspector>>();
            Operator thisOperator = UserManager.GetActiveOperator();
            if (thisOperator?.Inspector == null)
                return Failure("Создание документов комиссии доступно только членам комиссии");

            var inspector = thisOperator.Inspector;
            var zonal = ZonalInspectionInspectorDomain.GetAll()
                .FirstOrDefault(x => x.Inspector == inspector)?.ZonalInspection;
            entity.ProceedingsPlace = zonal.Address;
            if (zonal == null)
            {
                return Failure("Создание документов комиссии доступно только членам комиссии");
            }
            //проставляем комиссию, либо создаем новую
            if (entity.ComissionMeeting == null && entity.DateOfProceedings.HasValue)
            {
                var comission = ComissionMeetingDomain.GetAll()
                    .FirstOrDefault(x => x.CommissionDate == entity.DateOfProceedings && x.ZonalInspection == zonal);
                if (comission != null)
                {
                    entity.ComissionMeeting = comission;
                }
                else
                {

                    comission = new ComissionMeeting
                    {
                        CommissionDate = entity.DateOfProceedings.Value,
                        ComissionName = $"Комиссия от {entity.DateOfProceedings.Value.ToString("dd.MM.yyyy")}",
                        Description = "Создано из протокола",
                        ZonalInspection = zonal,
                        CommissionNumber = "б/н"
                    };
                    servStateProvider.SetDefaultState(comission);
                    ComissionMeetingDomain.Save(comission);
                    entity.ComissionMeeting = comission;
                }
                entity.ProceedingsPlace = comission.ZonalInspection.Address;
            }
            else if (entity.ComissionMeeting != null)
            {
                entity.DateOfProceedings = entity.ComissionMeeting.CommissionDate;
            }
            var existsDoc = ComissionMeetingDocumentDomain.GetAll()
                .Where(x => x.ComissionMeeting == entity.ComissionMeeting && x.DocumentGji.Id == entity.Id).FirstOrDefault();
            if (existsDoc == null)
            {
                ComissionMeetingDocumentDomain.Save(new ComissionMeetingDocument
                {
                    ComissionDocumentDecision = ComissionDocumentDecision.NotSet,
                    ComissionMeeting = entity.ComissionMeeting,
                    DocumentGji = new DocumentGji { Id = entity.Id },
                    Description = "Добавлено из протокола"
                });
            }

            var PersonService = Container.Resolve<IDomainService<IndividualPerson>>();
            if (entity.IndividualPerson == null && !string.IsNullOrEmpty(entity.Fio) || (entity.IndividualPerson != null && entity.IndividualPerson.Fio != entity.Fio))
            {

                try
                {
                    var existsPerson = PersonService.GetAll()
                    .Where(x => x.Fio == entity.Fio && x.PassportSeries.ToString() == entity.PhysicalPersonDocumentSerial && x.PassportNumber.ToString() == entity.PhysicalPersonDocumentNumber).FirstOrDefault();

                    if (existsPerson == null)
                    {
                        IndividualPerson person = new IndividualPerson();
                        person.Fio = entity.Fio;
                        person.PlaceResidence = entity.FiasRegistrationAddress != null ? entity.FiasRegistrationAddress.AddressName : "";
                        person.ActuallyResidence = entity.FiasFactAddress != null ? entity.FiasFactAddress.AddressName : "";
                        person.FiasRegistrationAddress = entity.FiasRegistrationAddress;
                        person.FiasFactAddress = entity.FiasFactAddress;
                        person.IsActuallyResidenceOutState = entity.IsActuallyResidenceOutState;
                        person.IsPlaceResidenceOutState = entity.IsActuallyResidenceOutState;
                        person.PlaceResidenceOutState = entity.PlaceResidenceOutState;
                        person.ActuallyResidenceOutState = entity.ActuallyResidenceOutState;
                        person.BirthPlace = entity.BirthPlace;
                        person.Job = entity.Job;
                        person.FamilyStatus = entity.FamilyStatus;
                        person.DateBirth = entity.DateBirth;
                        person.PassportNumber = entity.PhysicalPersonDocumentNumber;
                        person.PassportSeries = entity.PhysicalPersonDocumentSerial;
                        person.DepartmentCode = entity.DepartmentCode.HasValue ? entity.DepartmentCode.Value.ToString() : "";
                        person.DateIssue = entity.DateIssue;
                        person.PassportIssued = entity.PassportIssued;
                        person.INN = entity.INN;
                        person.SocialStatus = entity.SocialStatus;
                        person.DependentsNumber = entity.DependentsNumber;


                        PersonService.Save(person);
                        entity.IndividualPerson = person;
                    }
                    else
                    {
                        entity.IndividualPerson = existsPerson;
                    }
                }
                catch (Exception e)
                {

                }
                finally
                {
                    Container.Release(PersonService);
                    Container.Release(ComissionMeetingDomain);
                    Container.Release(ComissionMeetingDocumentDomain);
                }

            }
            else if (entity.IndividualPerson != null)
            {
                var person = PersonService.Get(entity.IndividualPerson.Id);

                person.PassportNumber = entity.PhysicalPersonDocumentNumber;
                person.PassportSeries = entity.PhysicalPersonDocumentSerial;

                if (entity.FiasFactAddress != null)
                {
                    person.FiasFactAddress = entity.FiasFactAddress;
                    person.ActuallyResidence = entity.FiasFactAddress != null ? entity.FiasFactAddress.AddressName : "";
                }
                if (entity.FiasRegistrationAddress != null)
                {
                    person.FiasRegistrationAddress = entity.FiasRegistrationAddress;
                    person.PlaceResidence = entity.FiasRegistrationAddress != null ? entity.FiasRegistrationAddress.AddressName : "";
                }
                if (entity.DependentsNumber.HasValue)
                {
                    person.DependentsNumber = entity.DependentsNumber;
                }
                if (entity.SocialStatus != null)
                {
                    person.SocialStatus = entity.SocialStatus;
                }
                person.Job = entity.Job;
                person.FamilyStatus = entity.FamilyStatus;



            }
            //доделать добавление строки в транспорт

            if (entity.JudSector == null)
            {
                if (entity.PlaceOffense == PlaceOffense.AddressUr)
                {
                    if (entity.Contragent != null)
                    {
                        // entity.FiasPlaceAddress = entity.Contragent.FiasJuridicalAddress;
                    }
                }
                else if (entity.PlaceOffense == PlaceOffense.PlaceFact)
                {
                    var data = Container.Resolve<IDomainService<ProtocolViolation>>().GetAll()
                            .Where(x => x.Document.Id == entity.Id)
                            .Select(x => new
                            {
                                x.InspectionViolation.RealityObject
                            }).FirstOrDefault();
                    if (data != null)
                    {
                        entity.FiasPlaceAddress = data.RealityObject.FiasAddress;
                    }

                }
                if (entity.FiasPlaceAddress != null)
                {
                    var violationHouse = entity.FiasPlaceAddress.House;
                    string houseDigits = string.Empty;
                    char[] violationHouseChar = violationHouse.ToCharArray();
                    for (int i = 0; i < violationHouse.Length; i++)
                    {
                        var ch = violationHouseChar[i];
                        if (char.IsDigit(violationHouseChar[i]))
                        {
                            houseDigits += ch;
                        }
                    }
                    //автоподстановка суд.участка по адресу места совершения правонарушения
                    var violationHouseInt = int.Parse(houseDigits);
                    Utils.SaveFiasAddress(Container, entity.FiasPlaceAddress);
                    int checkedMath = 0;
                    long? jurId = null;
                    Container.Resolve<IDomainService<JurInstitutionRealObj>>().GetAll()
                    .Where(x => x.RealityObject.FiasAddress.StreetGuidId == entity.FiasPlaceAddress.StreetGuidId)
                    .OrderBy(x => x.RealityObject.FiasAddress.House)
                    .Select(x => new
                    {
                        x.RealityObject.FiasAddress.House,
                        x.JurInstitution.Id
                    })
                    .ToList().ForEach(x =>
                    {
                        var house = x.House;
                        char[] houseChar = house.ToCharArray();
                        for (int i = 0; i < houseChar.Length; i++)
                        {
                            if (!char.IsDigit(houseChar[i]))
                            {
                                house = house.Remove(i, 1);

                            }
                        }
                        var houseCharInt = int.Parse(house);
                        var module = Math.Abs(houseCharInt - violationHouseInt);
                        if (houseCharInt != violationHouseInt)
                        {
                            if (checkedMath == 0)
                            {
                                checkedMath = module;
                            }
                            else
                            {
                                if (checkedMath > module)
                                {
                                    checkedMath = module;
                                    jurId = x.Id;
                                }
                            }
                        }
                        else
                        {
                            jurId = x.Id;
                        }
                    });

                    if (jurId.HasValue)
                        entity.JudSector = new JurInstitution { Id = jurId.Value };
                }
            }

            if (entity.DocumentNumber != null)
            {
                string UIN = GetUIN();
                //string UIN = "39645f";
                if (UIN != null)
                {
                    string s1 = Convert.ToInt32(UIN, 16).ToString().PadLeft(8, '0');
                    string s2 = (entity.DocumentDate?.ToString("yyyyMMdd") ?? "00000000");
                    string s3 = "";
                    if (entity.Inspection.InspectionNumber.Contains("-"))
                    {
                        if (entity.Inspection.InspectionNumber.Split('-').Count() > 2)
                        {
                            s3 = (entity.Inspection.InspectionNumber.Split('-')[1] + entity.Inspection.InspectionNumber.Split('-')[2]).PadRight(8, '0');
                        }
                        else if (entity.Inspection.InspectionNumber.Split('-').Count() == 2)
                        {
                            s3 = entity.Inspection.InspectionNumber.Split('-')[1].PadRight(8, '0');
                        }
                        else
                        {
                            s3 = entity.Inspection.InspectionNumber.Replace("-", "").PadRight(8, '0');
                        }
                    }
                    else
                    {
                        s3 = entity.Inspection.InspectionNumber.PadRight(8, '0');
                    }
                    entity.UIN = (s1 + s2 + s3).Substring(0, 24);
                    entity.UIN += CheckSum(entity.UIN);
                }
            }

            return Success();
        }

        public override IDataResult BeforeDeleteAction(IDomainService<Protocol197> service, Protocol197 entity)
        {
            var annexService = this.Container.Resolve<IDomainService<Protocol197Annex>>();
            var lawService = this.Container.Resolve<IDomainService<Protocol197ArticleLaw>>();
            var violationService = this.Container.Resolve<IDomainService<Protocol197Violation>>();
            var activitiyDirectionService = this.Container.Resolve<IDomainService<Protocol197ActivityDirection>>();
            var surveySubjectReqService = this.Container.Resolve<IDomainService<Protocol197SurveySubjectRequirement>>();
            var longTextService = this.Container.Resolve<IDomainService<Protocol197LongText>>();

            try
            {
                var result = base.BeforeDeleteAction(service, entity);

                if (!result.Success)
                {
                    return this.Failure(result.Message);
                }

                annexService.GetAll().Where(x => x.Protocol197.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => annexService.Delete(x));

                lawService.GetAll().Where(x => x.Protocol197.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => lawService.Delete(x));

                violationService.GetAll().Where(x => x.Document.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => violationService.Delete(x));

                activitiyDirectionService.GetAll().Where(x => x.Protocol197.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => activitiyDirectionService.Delete(x));

                surveySubjectReqService.GetAll().Where(x => x.Protocol197.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => surveySubjectReqService.Delete(x));

                longTextService.GetAll().Where(x => x.Protocol197.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => longTextService.Delete(x));

                return result;
            }
            finally
            {
                this.Container.Release(annexService);
                this.Container.Release(lawService);
                this.Container.Release(violationService);
                this.Container.Release(activitiyDirectionService);
                this.Container.Release(surveySubjectReqService);
            }
        }

        public override IDataResult AfterDeleteAction(IDomainService<Protocol197> service, Protocol197 entity)
        {
            return this.Success();
        }


        private int GetNextNum()
        {
            var domainServiceInspection = this.Container.Resolve<IDomainService<BaseDefault>>();
            var maxnum = domainServiceInspection.GetAll()
                .Where(x => x.InspectionNum.HasValue)
                .Max(x => x.InspectionNum);
            if (maxnum.HasValue)
            {
                return maxnum.Value + 1;
            }
            return 1;
        }

        private string GetUIN()
        {
            return this.Container.Resolve<IDomainService<ZonalInspectionInspector>>().GetAll()
                                .Where(x => x.Inspector == this.Container.Resolve<IGkhUserManager>().GetActiveOperator().Inspector)
                                .Select(x => x.ZonalInspection.GisGmpId)
                                .FirstOrDefault();
        }
    }
}