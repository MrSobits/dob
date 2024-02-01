namespace Bars.GkhGji.DomainService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using B4;
    using B4.Utils;
    using Gkh.Authentification;
    using Entities;
    using Enums;
    using Utils;

    using Castle.Windsor;
    using Bars.Gkh.Entities;

    public class ResolutionService : IResolutionService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<ProtocolMvdRealityObject> ProtocolMvdRealityObjectDomain { get; set; }
        
        public IDataResult GetInfo(long? documentId)
        {
            try
            {
                //var documentId = baseParams.Params.ContainsKey("documentId") ? baseParams.Params["documentId"].ToLong() : 0;
                var baseName = "";

                // Пробегаемся по документам на основе которого создано постановление
                var parents = Container.Resolve<IDomainService<DocumentGjiChildren>>().GetAll()
                    .Where(x => x.Children.Id == documentId)
                    .Select(x => new
                        {
                            parentId = x.Parent.Id,
                            x.Parent.TypeDocumentGji,
                            x.Parent.DocumentDate,
                            x.Parent.DocumentNumber
                        })
                    .ToList();

                foreach (var doc in parents)
                {
                    var docName = Utils.GetDocumentName(doc.TypeDocumentGji);

                    if (!string.IsNullOrEmpty(baseName))
                    {
                        baseName += ", ";
                    }

                    baseName += string.Format("{0} №{1} от {2}", docName, doc.DocumentNumber, doc.DocumentDate.ToDateTime().ToShortDateString());
                }

                return new BaseDataResult(new { success = true, baseName });
            }
            catch (ValidationException e)
            {
                return new BaseDataResult(new { success = false, message = e.Message });
            }
        }

        public IDataResult ListResolutionIndividualPerson(BaseParams baseParams) 
        {
            var loadParam = baseParams.GetLoadParam();

            var documentId = baseParams.Params.GetAs<int>("documentId");

           var childservice = Container.Resolve<IDomainService<DocumentGjiChildren>>();
           var dsposalservice = Container.Resolve<IDomainService<Disposal>>();
            var indivservice = Container.Resolve<IDomainService<IndividualPerson>>();

            var childdata = childservice.GetAll().Where(x => x.Children.Id == documentId).Select(x => x.Parent.Id).ToList();
            var disdata = childservice.GetAll().Where(x => x.Children.Id == childdata[0]).Select(x => x.Parent.Id).ToList();
            var disposaldata = dsposalservice.GetAll().Where(x => x.Id == disdata[0]).Select(x => new {x.Fio, x.DateBirth }).ToList();
            var data = indivservice.GetAll().Where(x => x.Fio == disposaldata[0].Fio && x.DateBirth == disposaldata[0].DateBirth).Select(x => new {
                x.Id,
                x.Fio,
                x.PlaceResidence,
                x.ActuallyResidence,
                x.BirthPlace,
                x.Job,
                x.DateBirth,
                x.PassportNumber,
                x.PassportSeries,
                x.PassportIssued,
                x.DepartmentCode,
                x.DateIssue,
                x.INN,
                x.FamilyStatus
            }).Filter(loadParam, Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }

        public IDataResult ListIndividualPerson(BaseParams baseParams) 
        {
            var loadParam = baseParams.GetLoadParam();

             var individualPersonid = baseParams.Params.GetAs<int>("individualPersonid");


            var service = Container.Resolve<IDomainService<Resolution>>();

            var data = service.GetAll()
                .Where(x => x.IndividualPerson.Id == individualPersonid)
                .Select(x => new
                {
                    x.Id,
                    x.DocumentDate,
                    x.DocumentNum,
                    x.InLawDate,
                    x.DocumentNumber,
                    x.Violation,
                    x.PenaltyAmount,
                    x.Paided
                }).AsEnumerable()
                .Select(x=> new
                {
                    x.Id,
                    x.DocumentDate,
                    x.DocumentNum,
                    x.InLawDate,
                    x.DocumentNumber,
                    x.Violation,
                    x.PenaltyAmount,
                    x.Paided,
                    PayDay = GetPayDay(x.Id)
                }).AsQueryable()
                .Filter(loadParam, Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }

        private DateTime? GetPayDay(long resId)
        {
            var service = Container.Resolve<IDomainService<ResolutionPayFine>>();
            return service.GetAll()
                .Where(x => x.Resolution.Id == resId)
                .Max(x => x.DocumentDate);

        }


        public IDataResult ListView(BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            /*
             * В качестве фильтров приходят следующие параметры
             * dateStart - Необходимо получить документы больше даты начала
             * dateEnd - Необходимо получить документы меньше даты окончания
             * dateNotPayStart - Необходимо получить документы больше даты неоплаты
             * dateNotPayEnd - Необходимо получить документы меньше даты неоплаты
             * realityObjectId - Необходимо получить документы по дому
            */

            var dateStart = baseParams.Params.GetAs<DateTime>("dateStart");
            var dateEnd = baseParams.Params.GetAs<DateTime>("dateEnd");
            var dateNotPayStart = baseParams.Params.GetAs<DateTime>("dateNotPayStart");
            var dateNotPayEnd = baseParams.Params.GetAs<DateTime>("dateNotPayEnd");
            var realityObjectId = baseParams.Params.GetAs<long>("realityObjectId");

            var predata = GetViewList()
                .WhereIf(dateStart != DateTime.MinValue, x => x.DocumentDate >= dateStart)
                .WhereIf(dateEnd != DateTime.MinValue, x => x.DocumentDate <= dateEnd)
                .WhereIf(dateNotPayStart != DateTime.MinValue, x => x.Protocol205Date >= dateNotPayStart)
                .WhereIf(dateNotPayEnd != DateTime.MinValue, x => x.Protocol205Date <= dateNotPayEnd)
                .WhereIf(realityObjectId > 0, x => x.RealityObjectIds.Contains("/" + realityObjectId.ToString() + "/"))
                .AsEnumerable()
                .Select(x => new
                {
                    x.Id,
                    x.State,
                    x.ContragentName,
                    x.TypeExecutant,
                    MunicipalityNames = x.TypeBase == TypeBase.ProtocolMvd ? GetProtocolMvdMuName(x.InspectionId.ToLong()) : x.MunicipalityId != null ? x.MunicipalityNames : x.ContragentMuName,
                    MoSettlement = x.MoNames,
                    PlaceName = x.PlaceNames,
                    MunicipalityId = x.MunicipalityId ?? x.ContragentMuId,
                    x.DocumentDate,
                    x.DocumentNumber,
                    x.DocumentNum,
                    x.OfficialName,
                    x.OfficialPosition,
                    x.TypeInitiativeOrg,
                    x.PenaltyAmount,
                    x.Protocol205Date,
                    x.Sanction,
                    x.SumPays,
                    x.InspectionId,
                    x.TypeBase,
                    x.ConcederationResult,
                    x.TypeDocumentGji,
                    x.DeliveryDate,
                    x.Paided,
                    x.ControlType,
                    x.BecameLegal,
                    x.InLawDate,
                    x.DueDate,
                    x.PaymentDate,
                    x.RoAddress,
                    x.ArticleLaw,
                    HasProtocol = GetProtocol(x.Id),
                    x.SentToOSP,
                    x.PhysicalPerson
                })
                .ToList();

            var plusdata = GetViewList()
                .WhereIf(dateStart != DateTime.MinValue, x => x.DocumentDate >= dateStart)
                .WhereIf(dateEnd != DateTime.MinValue, x => x.DocumentDate <= dateEnd)
                .WhereIf(realityObjectId > 0, x => x.RealityObjectIds.Contains("/" + realityObjectId.ToString() + "/"))
                .Where(x => x.Protocol205Date == null)
                .AsEnumerable()
                .Select(x => new
                {
                    x.Id,
                    x.State,
                    x.ContragentName,
                    x.TypeExecutant,
                    MunicipalityNames = x.TypeBase == TypeBase.ProtocolMvd ? GetProtocolMvdMuName(x.InspectionId.ToLong()) : x.MunicipalityId != null ? x.MunicipalityNames : x.ContragentMuName,
                    MoSettlement = x.MoNames,
                    PlaceName = x.PlaceNames,
                    MunicipalityId = x.MunicipalityId ?? x.ContragentMuId,
                    x.DocumentDate,
                    x.DocumentNumber,
                    x.DocumentNum,
                    x.OfficialName,
                    x.OfficialPosition,
                    x.TypeInitiativeOrg,
                    x.PenaltyAmount,
                    x.Protocol205Date,
                    x.Sanction,
                    x.SumPays,
                    x.InspectionId,
                    x.TypeBase,
                    x.ConcederationResult,
                    x.TypeDocumentGji,
                    x.DeliveryDate,
                    x.Paided,
                    x.ControlType,
                    x.BecameLegal,
                    x.InLawDate,
                    x.DueDate,
                    x.PaymentDate,
                    x.RoAddress,
                    x.ArticleLaw,
                    HasProtocol = GetProtocol(x.Id),
                    x.SentToOSP,
                    x.PhysicalPerson
                })
                .ToList();

            var result = predata.Concat(plusdata);

            var data = result.Select(x => new
                {
                x.Id,
                x.State,
                x.ContragentName,
                x.TypeExecutant,
                x.MunicipalityNames,
                x.MoSettlement,
                x.PlaceName,
                x.MunicipalityId,
                x.DocumentDate,
                x.DocumentNumber,
                x.DocumentNum,
                x.OfficialName,
                x.OfficialPosition,
                x.TypeInitiativeOrg,
                x.PenaltyAmount,
                x.Protocol205Date,
                x.Sanction,
                x.SumPays,
                x.InspectionId,
                x.TypeBase,
                x.ConcederationResult,
                x.TypeDocumentGji,
                x.DeliveryDate,
                x.Paided,
                x.ControlType,
                x.BecameLegal,
                x.InLawDate,
                x.DueDate,
                x.PaymentDate,
                x.RoAddress,
                x.ArticleLaw,
                HasProtocol = GetProtocol(x.Id),
                x.SentToOSP,
                x.PhysicalPerson
                })
                .AsQueryable()
                .Filter(loadParam, Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }

        private bool GetProtocol(long resolutionId)
        {
            var childrenId = this.Container.Resolve<IDomainService<DocumentGjiChildren>>().GetAll()
             .Where(x => x.Parent.Id == resolutionId && x.Children.TypeDocumentGji == TypeDocumentGji.Protocol)
             .Select(x => x.Children.Id).FirstOrDefault();

            return childrenId > 0;
        }

        //private Municipality GetFineMO(long resolutionId)
        //{
        //    var parentId = Container.Resolve<IDomainService<DocumentGjiChildren>>().GetAll()
        //     .Where(x => x.Children.Id == resolutionId)
        //     .Select(x => x.Parent.Id).FirstOrDefault();

        //    var zonalInspId = Container.Resolve<IDomainService<ComissionMeetingDocument>>().GetAll()
        //     .Where(x => x.DocumentGji.Id == parentId)
        //     .Select(x => x.ComissionMeeting.ZonalInspection.Id).FirstOrDefault();

        //    var municipality = Container.Resolve<IDomainService<ZonalInspectionMunicipality>>().GetAll()
        //     .Where(x => x.ZonalInspection.Id == zonalInspId)
        //     .Select(x => x.Municipality).FirstOrDefault();

        //    return municipality;
        //}

        public virtual string GetProtocolMvdMuName(long? resolInspId)
        {
            if (resolInspId == null)
            {
                return string.Empty;
            }

            return ProtocolMvdRealityObjectDomain.GetAll().Where(x => x.ProtocolMvd.Inspection.Id == resolInspId).Select(x => x.RealityObject.Municipality.Name).FirstOrDefault();
        }
        
        public IQueryable<ViewResolution> GetViewList()
        {
            Operator thisOperator = Container.Resolve<IGkhUserManager>().GetActiveOperator();
            var zonalDomain = this.Container.Resolve<IDomainService<ZonalInspectionInspector>>();

            if (thisOperator?.Inspector == null)
            {
                return null;
            }

            if (thisOperator?.Inspector.NotMemberPosition.Name == "Администратор доходов")
            {
                var zonalInspSubIds = Container.Resolve<IDomainService<InspectorZonalInspSubscription>>().GetAll()
                .Where(x => x.Inspector.Id == thisOperator.Inspector.Id)
                .Select(x => x.ZonalInspection.Id)
                .ToList();

                if (zonalInspSubIds.Count() > 0)
                {
                    return Container.Resolve<IDomainService<ViewResolution>>().GetAll()
                        .Where(x => zonalInspSubIds.Contains(x.ZonalInspectionId));
                }
            }

            var zonalId = zonalDomain.GetAll().FirstOrDefault(x => x.Inspector == thisOperator.Inspector).ZonalInspection?.Id;
            if (!zonalId.HasValue)
            {
                return null;
            }
            return Container.Resolve<IDomainService<ViewResolution>>().GetAll().Where(x => x.ZonalInspectionId == zonalId);
        }

        public IDataResult GetResolutionInfo(BaseParams baseParams)
        {
#warning переделать получение агрегированной информации
            var listResAnonymObj = new List<object>();

            var resolutionIds = baseParams.Params.GetAs<long[]>("objectIds");
            var resolutionList =
                Container.Resolve<IDomainService<Resolution>>().GetAll()
                    .Where(x => resolutionIds.Contains(x.Id))
                    .ToList();

            foreach (var resolutionObj in resolutionList)
            {
                var protocolViolationsCount = this.GetCountProtocolViolationsByResolution(resolutionObj);

                var payFineResolutionDate = Container.Resolve<IDomainService<ResolutionPayFine>>().GetAll()
                    .Where(x => x.Resolution.Id == resolutionObj.Id)
                    .Max(x => x.DocumentDate);

                listResAnonymObj.Add(new
                {
                    resolutionObj.DocumentDate,
                    resolutionObj.PenaltyAmount,
                    SupervisoryOrgCode = resolutionObj.TypeInitiativeOrg == TypeInitiativeOrgGji.HousingInspection ? "1" : "6",
                    ProtocolViolationsCount = protocolViolationsCount,
                    PayFineResolutionDate = payFineResolutionDate
                });
            }

            return new BaseDataResult(listResAnonymObj);
        }

        public virtual string GetTakingDecisionAuthorityName()
        {
            return "ГОСУДАРСТВЕННАЯ Административная комиссия";
        }

        private int GetCountProtocolViolationsByResolution(Resolution resolution)
        {
            var serviceDocumentGjiChildren = Container.Resolve<IDomainService<DocumentGjiChildren>>();
            var protocol = serviceDocumentGjiChildren
                .GetAll()
                .Where(x => x.Children.Id == resolution.Id && x.Parent.TypeDocumentGji == TypeDocumentGji.Protocol)
                .Select(x => x.Parent)
                .ToList()
                .FirstOrDefault()
                .To<Protocol>();

            var serviceProtocolGjiIViolation = Container.Resolve<IDomainService<ProtocolViolation>>();
            var result = serviceProtocolGjiIViolation
                .GetAll()
                .Where(x => x.Document.Id == protocol.Id)
                .Select(x => x.InspectionViolation.Id)
                .Distinct()
                .Count();

            return result;
        }
    }
}