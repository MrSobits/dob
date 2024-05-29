namespace Bars.GkhGji.Regions.BaseChelyabinsk.DomainService.Impl
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.Dict;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol197;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Views;
    using Bars.GkhGji;

    using Castle.Windsor;
    using System.Collections.Generic;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Enums;
    using Bars.Gkh.Enums;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Report;
    using Bars.GkhGji.DomainService;
    using Bars.B4.Modules.Reports;
    using System.IO;
    using Bars.Gkh.StimulReport;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.FIAS;
    using System.Text;

    public class Protocol197Service : IProtocol197Service
    {
        public IWindsorContainer Container { get; set; }
        public IDomainService<DocumentGjiInspector> ServiceInspectorDomain { get; set; }
		public IDomainService<DocumentGji> DocumentGjiDomain { get; set; }
		public IDomainService<Resolution> ResolutionDomain { get; set; }
		public IDomainService<DocumentGjiChildren> ServiceChildrenDomain { get; set; }
		public IDomainService<ComissionMeetingDocument> ComissionMeetingDocumentDomain { get; set; }
        public IDomainService<ConcederationResult> ConcederationResultDomain { get; set; }
        public IDomainService<ComissionMeeting> ComissionMeetingDomain { get; set; }
		public IRepository<Protocol197> Protocol197Domain { get; set; }
        public IRepository<ViolationGji> ViolationGjiDomain { get; set; }
		public IRepository<Protocol197Violation> Protocol197ViolationDomain { get; set; }
		public IDomainService<Protocol197Annex> Protocol197AnnexDomain { get; set; }
        public IDomainService<Protocol197LongText> Protocol197LongTextDomain { get; set; }
        public IDomainService<DocumentGjiChildren> DocumentGjiChildrenDomain { get; set; }
		public IDomainService<Protocol197ArticleLaw> Protocol197ArticleLawDomain { get; set; }
		public IDomainService<ProtocolArticleLaw> ProtocolArticleLawDomain { get; set; }
		public IDomainService<Protocol197AnotherResolution> Protocol197AnotherResolutionDomain { get; set; }
		public IDomainService<Protocol197SurveySubjectRequirement> Protocol197SurveySubjectRequirementDomain { get; set; }
        public IDomainService<Protocol197ActivityDirection> Protocol197ActivityDirectionDomain { get; set; }
		public IDomainService<InspectionGjiStage> InspectionStageDomain { get; set; }
		public IDomainService<DocumentGjiInspector> DocumentInspectorDomain { get; set; }
		public IDomainService<DocumentGjiChildren> ChildrenDomain { get; set; }


		public virtual List<IndividualPerson> GetNameList(BaseParams baseParams)
		{
			var serviceContragentContact = Container.Resolve<IDomainService<ContragentContact>>();
			var servicePerson = Container.Resolve<IDomainService<IndividualPerson>>();
			var loadParam = baseParams.GetLoadParam();
			var filter = baseParams.Params.GetAs<string>("filter");
			var contragentid = baseParams.Params.GetAs<long>("contragentid");
			var enumid = baseParams.Params.GetAs<long>("enumid");

			if (enumid == 4 || enumid == 1)
			{
				var data = serviceContragentContact.GetAll()
					.Where(x => x.Contragent.Id == contragentid)
					.Where(x => x.IndividualPerson != null && x.IndividualPerson.Fio.ToLower().StartsWith(filter.ToLower()))
					.Select(x => x.IndividualPerson)
					.ToList();

				return data;
			}
			else
			{
				var data = servicePerson.GetAll()
					.Where(x => x.Fio.ToLower().StartsWith(filter.ToLower()))
					.Select(x=> new IndividualPerson
					{
						Id = x.Id,
						Fio = x.Fio,
						DateBirthTxt = x.DateBirth.HasValue? x.DateBirth.Value.ToString("dd.MM.yyyy"):"",
						DateBirth = x.DateBirth,
						DateIssue = x.DateIssue,
						BirthPlace = x.BirthPlace,
						DepartmentCode = x.DepartmentCode,
						PassportIssued = x.PassportIssued,
						PassportNumber = x.PassportNumber,
						PassportSeries = x.PassportSeries,
						PlaceResidence = x.IsPlaceResidenceOutState? x.PlaceResidenceOutState:x.PlaceResidence,
						IsActuallyResidenceOutState = x.IsActuallyResidenceOutState,
						IsPlaceResidenceOutState = x.IsPlaceResidenceOutState,
						PlaceResidenceOutState = x.PlaceResidenceOutState,
						ActuallyResidenceOutState = x.ActuallyResidenceOutState,
						Job = x.Job,
						FamilyStatus = x.FamilyStatus,
						FiasFactAddress = x.FiasFactAddress,
						FiasRegistrationAddress = x.FiasRegistrationAddress,
						SocialStatus = x.SocialStatus,
						DependentsNumber = x.DependentsNumber,
						PhoneNumber = x.PhoneNumber
					})
					.ToList();

				return data;
			}


		}

		public virtual IDataResult ListView(BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            /*
             * В качестве фильтров приходят следующие параметры
             * dateStart - Необходимо получить документы больше даты начала
             * dateEnd - Необходимо получить документы меньше даты окончания
             * realityObjectId - Необходимо получить документы по дому
            */

            var dateStart = baseParams.Params.GetAs("dateStart", DateTime.MinValue);
            var dateEnd = baseParams.Params.GetAs("dateEnd", DateTime.MinValue);
            var realityObjectId = baseParams.Params.GetAs<long>("realityObjectId");

            var data = this.GetViewList()
                .WhereIf(dateStart != DateTime.MinValue, x => x.DocumentDate >= dateStart)
                .WhereIf(dateEnd != DateTime.MinValue, x => x.DocumentDate <= dateEnd)
                .WhereIf(realityObjectId > 0, x => x.RealityObjectIds.Contains("/" + realityObjectId.ToString() + "/"))
                .Select(x => new
                {
                    x.Id,
                    x.State,
                    x.ContragentName,
                    MunicipalityNames = x.MunicipalityId != null ? x.MunicipalityNames : x.ContragentMuName,
                    MoSettlement = x.MoNames,
                    PlaceName = x.PlaceNames,
                    MunicipalityId = x.MunicipalityId ?? x.ContragentMuId,
                    x.TypeExecutant,
                    x.CountViolation,
                    x.InspectorNames,
                    x.DocumentDate,
                    x.DocumentNumber,
                    x.DocumentNum,
                    x.InspectionId,
                    x.TypeBase,
                    x.TypeDocumentGji,
                    x.ArticleLaw,
                    x.ControlType,
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
                    x.FamilyStatus,
					x.TypeViolator,
                    HasResolution = GetResolution(x.Id),
					Penalty = GetPenalty(x.Id) ?? 0,
					x.DateViolation,
					x.InspectorPosition,
					x.PhysicalPerson,
					x.PhoneNumber
				})
                .Filter(loadParam, this.Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }

        private bool GetResolution(long protocolId)
        {
            var childrenId = this.Container.Resolve<IDomainService<DocumentGjiChildren>>().GetAll()
				.Where(x => x.Parent.Id == protocolId && x.Children.TypeDocumentGji == TypeDocumentGji.Resolution)
				.Select(x => x.Children.Id).FirstOrDefault();
			//var dateToCourt = this.Container.Resolve<IDomainService<Resolution>>().Get(childrenId);
			
   //         if (!dateToCourt.HasValue)
   //         {
   //             return true;
   //         }
   //         if (dateToCourt.Value.AddMonths(2) > DateTime.Now)
   //         {
   //             return true;
   //         }
			
            return childrenId > 0;
        }

		private decimal? GetPenalty(long protocolId)
		{
			decimal? penalty = 0;
			var resolIdList = this.Container.Resolve<IDomainService<DocumentGjiChildren>>().GetAll()
			 .Where(x => x.Parent.Id == protocolId && x.Children.TypeDocumentGji == TypeDocumentGji.Resolution)
			 .Select(x => x.Children.Id).ToList();

			var penaltyList = this.Container.Resolve<IDomainService<Resolution>>().GetAll()
				.Where(x => resolIdList.Contains(x.Id))
				.Select(x => x.PenaltyAmount).ToList();

			foreach (decimal? pen in penaltyList)
			{
				penalty += pen;
			}

			return penalty;
		}

		public virtual IQueryable<ViewProtocol197> GetViewList()
        {
            var userManager = this.Container.Resolve<IGkhUserManager>();
            var serviceDocumentInspector = this.Container.Resolve<IDomainService<DocumentGjiInspector>>();
            var serviceViewProtocol = this.Container.Resolve<IDomainService<ViewProtocol197>>();
			var zonalDomain = this.Container.Resolve<IDomainService<ZonalInspectionInspector>>();

			try
            {
				Operator thisOperator = userManager.GetActiveOperator();
				var inspectorList = userManager.GetInspectorIds();
              
                inspectorList.Clear();//убираем проверку на инспектора
				if (thisOperator?.Inspector == null)
				{
					return null;
				}

                if (thisOperator?.Inspector.NotMemberPosition != null && thisOperator?.Inspector.NotMemberPosition.Name == "Администратор доходов")
                {
                    var zonalInspSubIds = Container.Resolve<IDomainService<InspectorZonalInspSubscription>>().GetAll()
                    .Where(x => x.Inspector.Id == thisOperator.Inspector.Id)
                    .Select(x => x.ZonalInspection.Id)
                    .ToList();

                    if (zonalInspSubIds.Count() > 0)
                    {
                        return serviceViewProtocol.GetAll()
                            .Where(x => zonalInspSubIds.Contains(x.ZonalInspectionId));
                    }
                }

                var zonalId = zonalDomain.GetAll().FirstOrDefault(x => x.Inspector == thisOperator.Inspector).ZonalInspection?.Id;
				if (!zonalId.HasValue)
				{
					return null;
				}
				return serviceViewProtocol.GetAll()
					.Where(x=> x.ZonalInspectionId == zonalId.Value)
                    .WhereIf(inspectorList.Count > 0, y => serviceDocumentInspector.GetAll()
                                    .Any(x => y.Id == x.DocumentGji.Id
                                        && inspectorList.Contains(x.Inspector.Id)));
            }
            finally
            {
                this.Container.Release(userManager);
				this.Container.Release(zonalDomain);
				this.Container.Release(serviceDocumentInspector);
                this.Container.Release(serviceViewProtocol);
            }
        }

		public virtual IDataResult GetInfo(long? documentId)
		{

			try
			{
				var inspectorNames = string.Empty;
				var inspectorIds = string.Empty;

				// Сначала пробегаемся по инспекторам и формируем итоговую строку наименований и строку идентификаторов
				var inspectors =
					this.ServiceInspectorDomain.GetAll()
						.Where(x => x.DocumentGji.Id == documentId)
						.Select(x => new {x.Inspector.Id, x.Inspector.Fio})
						.ToList();



				foreach (var item in inspectors)
				{
					if (!string.IsNullOrEmpty(inspectorNames))
					{
						inspectorNames += ", ";
					}

					inspectorNames += item.Fio;

					if (!string.IsNullOrEmpty(inspectorIds))
					{
						inspectorIds += ", ";
					}

					inspectorIds += item.Id;
				}

				var reqs = this.Protocol197SurveySubjectRequirementDomain.GetAll()
					.Where(x => x.Protocol197.Id == documentId)
					.Select(x => x.Requirement)
					.Select(x => new {x.Id, x.Name})
					.ToArray();

				var dirs = this.Protocol197ActivityDirectionDomain.GetAll()
					.Where(x => x.Protocol197.Id == documentId)
					.Select(x => x.ActivityDirection)
					.Select(x => new {x.Id, x.Name})
					.ToArray();

				return
					new BaseDataResult(
						new Protocol197GetInfoProxy
						{
							inspectorNames = inspectorNames,
							inspectorIds = inspectorIds,
							requirementIds = string.Join(", ", reqs.Select(x => x.Id)),
							requirementNames = string.Join(", ", reqs.Select(x => x.Name)),
							directionIds = string.Join(", ", dirs.Select(x => x.Id)),
							directionNames = string.Join(", ", dirs.Select(x => x.Name)),
						});
			}
			catch (ValidationException e)
			{
				return new BaseDataResult {Success = false, Message = e.Message};
			}
		}

		public virtual IDataResult GetRepeatInfo(long? documentId)
		{
			if (!documentId.HasValue)
			{
				return new BaseDataResult { Success = false, Message = "Протокол не найден" };
			}
			try
			{
				Protocol197AnotherResolutionDomain.GetAll()
					.Where(x => x.Protocol197.Id == documentId.Value)
					.Select(x => x.Id).ToList()
					.ForEach(x =>
					{
						Protocol197AnotherResolutionDomain.Delete(x);
					});
				var artlaw = Protocol197ArticleLawDomain.GetAll().FirstOrDefault(x => x.Protocol197.Id == documentId.Value)?.ArticleLaw;
				var artlawrepeat = ViolationGjiDomain.GetAll().FirstOrDefault(x => x.ArticleLaw == artlaw)?.ArticleLawRepeatative;
				List<long> artList = new List<long>();
				artList.Add(artlaw.Id);
				if (artlawrepeat != null)
				{
                    artList.Add(artlawrepeat.Id);
                }
                var protV = Protocol197Domain.Get(documentId.Value).DateOfViolation;
				if (!protV.HasValue)
				{
					return new BaseDataResult { Success = false, Message = "Не указана дата нарушения" };
				}
				if (artlaw == null )
				{
					return new BaseDataResult { Success = false, Message = "Не указана статья закона" };
				}
				var intruder = Protocol197Domain.Get(documentId.Value)?.IndividualPerson;
				if (intruder != null)
				{
					//ищем по физлицу
					var protsWithThisArtlaw = Protocol197ArticleLawDomain.GetAll()
						.Where(x => x.Protocol197.Id != documentId.Value && artList.Contains(x.ArticleLaw.Id) && x.Protocol197.IndividualPerson != null && x.Protocol197.IndividualPerson == intruder)
						.Select(x => x.Protocol197.Id).ToList();
					protsWithThisArtlaw.AddRange(ProtocolArticleLawDomain.GetAll()
						.Where(x => x.Protocol.Id != documentId.Value && artList.Contains(x.ArticleLaw.Id) && x.Protocol.IndividualPerson != null && x.Protocol.IndividualPerson == intruder)
						.Select(x => x.Protocol.Id).ToList());
					if (protsWithThisArtlaw.Count == 0)
					{
						return
						new BaseDataResult(
						new
						{
							hasRepeated = false
						});
					}
					var resIds = DocumentGjiChildrenDomain.GetAll()
						.Where(x => protsWithThisArtlaw.Contains(x.Parent.Id) && x.Children.TypeDocumentGji == TypeDocumentGji.Resolution)
						.Select(x => x.Children.Id).ToList();
					if (resIds.Count == 0)
					{
						return
						new BaseDataResult(
						new
						{
							hasRepeated = false
						});
					}
					var resWithNotPayded3y = ResolutionDomain.GetAll()
						.Where(x => x.InLawDate.HasValue)
						.Where(x => resIds.Contains(x.Id) && x.InLawDate > protV.Value.AddYears(-3) && x.InLawDate< protV.Value)
						.Where(x => x.PenaltyAmount > 0 && x.Paided != YesNoNotSet.Yes).ToList();

					var resWithWarning = ResolutionDomain.GetAll()
						.Where(x=> x.InLawDate.HasValue && resIds.Contains(x.Id))
						.Where(x => x.Sanction.Code == "3" && x.InLawDate > protV.Value.AddYears(-1) && x.InLawDate < protV.Value).ToList();

					var resWithYearPayment = ResolutionDomain.GetAll()
						.Where(x => x.InLawDate.HasValue)
						.Where(x => resIds.Contains(x.Id) && x.InLawDate > protV.Value.AddYears(-3) && x.InLawDate < protV.Value)
						.Where(x => x.PenaltyAmount > 0 && x.Paided == YesNoNotSet.Yes && x.PaymentDate.HasValue && x.PaymentDate.Value > protV.Value.AddYears(-1) && x.PaymentDate.Value < protV.Value).ToList();
					List<Resolution> lisrResolutions = resWithNotPayded3y;
					lisrResolutions.AddRange(resWithWarning);
					lisrResolutions.AddRange(resWithYearPayment);


					if (lisrResolutions.Count == 0)
					{
						return
						new BaseDataResult(
						new
						{
							hasRepeated = false
						});
					}
					List<long> addedRes = new List<long>();
					lisrResolutions.ForEach(x =>
					{
						if (!addedRes.Contains(x.Id))
						{
							Protocol197AnotherResolutionDomain.Save(new Protocol197AnotherResolution
							{
								ArticleLaw = artlaw,
								DocumentGji = x,
								Protocol197 = new Protocol197 { Id = documentId.Value }
							});
							addedRes.Add(x.Id);
						}					
					
					});
					//печатка
					try
					{
						var userParam = new UserParamsValues();
						userParam.AddValue("DocumentId", documentId.Value);
						var claimWorkCodedReportDomain = this.Container.ResolveAll<IComissionMeetingCodedReport>();
						var report = claimWorkCodedReportDomain.FirstOrDefault(x => x.ReportId == "SpravkaPovtorProtocol197");
						report.SetUserParams(userParam);
						MemoryStream stream;
						var reportProvider = Container.Resolve<IGkhReportProvider>();
						if (report is IReportGenerator && report.GetType().IsSubclassOf(typeof(StimulReport)))
						{
							//Вот такой вот костыльный этот метод Все над опеределывать
							stream = (report as StimulReport).GetGeneratedReport();
						}
						else
						{
							var reportParams = new ReportParams();
							report.PrepareReport(reportParams);

							// получаем Генератор отчета
							var generatorName = report.GetReportGenerator();

							stream = new MemoryStream();
							var generator = Container.Resolve<IReportGenerator>(generatorName);
							reportProvider.GenerateReport(report, stream, generator, reportParams);
						}
						report.ReportInfo = new ComissionMeetingReportInfo();

						report.DocId = documentId.ToString();
						var fileManager = this.Container.Resolve<IFileManager>();
						report.GenerateMassReport();
						var file = fileManager.SaveFile(stream, report.OutputFileName);
						var annx = Protocol197AnnexDomain.GetAll()
							.Where(x => x.Protocol197.Id == documentId.Value)
							.Where(x => x.Description == "Справка об административных правонарушениях").FirstOrDefault();
						if (annx != null)
						{
							Protocol197AnnexDomain.Delete(annx);
						}
						Protocol197AnnexDomain.Save(new Protocol197Annex
						{
							Description = "Справка об административных правонарушениях",
							Protocol197 = new Protocol197 { Id = documentId.Value },
							TypeAnnex = TypeAnnex.Disposal,
							File = file,
							DocumentDate = DateTime.Now,
							Name = "Справка об административных правонарушениях",
						});
					}
					catch
					{
						
					}

					UpdateArtLaw(documentId.Value);
					return
					new BaseDataResult(
						new
						{
							hasRepeated = true
						});

				}
				var ctrg = Protocol197Domain.Get(documentId.Value)?.Contragent;
				if (ctrg != null)
				{
					//ищем по физлицу
					var protsWithThisArtlaw = Protocol197ArticleLawDomain.GetAll()
						.Where(x => x.ArticleLaw == artlaw && x.Protocol197.Contragent != null && x.Protocol197.Contragent == ctrg)
						.Select(x => x.Protocol197.Id).ToList();
					protsWithThisArtlaw.AddRange(ProtocolArticleLawDomain.GetAll()
						.Where(x => x.ArticleLaw == artlaw && x.Protocol.Contragent != null && x.Protocol.Contragent == ctrg)
						.Select(x => x.Protocol.Id).ToList());
					if (protsWithThisArtlaw.Count == 0)
					{
						return
						new BaseDataResult(
						new
						{
							hasRepeated = false
						});
					}
					var resIds = DocumentGjiChildrenDomain.GetAll()
						.Where(x => protsWithThisArtlaw.Contains(x.Parent.Id) && x.Children.TypeDocumentGji == TypeDocumentGji.Resolution)
						.Select(x => x.Children.Id).ToList();
					if (resIds.Count == 0)
					{
						return
						new BaseDataResult(
						new
						{
							hasRepeated = false
						});
					}
					var resWithNotPayded3y = ResolutionDomain.GetAll()
							.Where(x => x.InLawDate.HasValue)
							.Where(x => resIds.Contains(x.Id) && x.DocumentDate > DateTime.Now.AddYears(-3))
							.Where(x => x.PenaltyAmount > 0 && x.Paided != YesNoNotSet.Yes).ToList();

					var resWithWarning = ResolutionDomain.GetAll()
						.Where(x => x.InLawDate.HasValue && resIds.Contains(x.Id))
						.Where(x => x.Sanction.Code == "3" && x.DocumentDate > DateTime.Now.AddYears(-1)).ToList();

					var resWithYearPayment = ResolutionDomain.GetAll()
						.Where(x => x.InLawDate.HasValue)
						.Where(x => resIds.Contains(x.Id) && x.DocumentDate > DateTime.Now.AddYears(-3))
						.Where(x => x.PenaltyAmount > 0 && x.Paided == YesNoNotSet.Yes && x.PaymentDate.HasValue && x.PaymentDate.Value > DateTime.Now.AddYears(-1)).ToList();
					List<Resolution> lisrResolutions = resWithNotPayded3y;
					lisrResolutions.AddRange(resWithWarning);
					lisrResolutions.AddRange(resWithYearPayment);


					if (lisrResolutions.Count == 0)
					{
						return
						new BaseDataResult(
						new
						{
							hasRepeated = false
						});
					}
					List<long> addedRes = new List<long>();
					lisrResolutions.ForEach(x =>
					{
						if (!addedRes.Contains(x.Id))
						{
							Protocol197AnotherResolutionDomain.Save(new Protocol197AnotherResolution
							{
								ArticleLaw = artlaw,
								DocumentGji = x,
								Protocol197 = new Protocol197 { Id = documentId.Value }
							});
							addedRes.Add(x.Id);
						}

					});
					UpdateArtLaw(documentId.Value);
					return	new BaseDataResult(
						new
						{
							hasRepeated = true
						});

				}
				return	new BaseDataResult(
						new
						{
							hasRepeated = false
						});
			}
			catch (ValidationException e)
			{
				return new BaseDataResult { Success = false, Message = e.Message };
			}
		}

		public IDataResult AddRequirements(BaseParams baseParams)
		{
			var documentId = baseParams.Params.GetAsId("documentId");
			var reqIds = baseParams.Params.GetAs<string>("requirementIds").Return(x => x.Split(',').Select(y => y.ToLong()));

			var reqs = this.Protocol197SurveySubjectRequirementDomain.GetAll()
				.Where(x => x.Protocol197.Id == documentId)
				.Select(x => new {x.Id, ProtocolId = x.Protocol197.Id, RequirementId = x.Requirement.Id})
				.ToArray();

			this.Container.InTransaction(() =>
			{
				foreach (var req in reqs.Where(x => !reqIds.Contains(x.RequirementId)))
				{
					this.Protocol197SurveySubjectRequirementDomain.Delete(req.Id);
				}

				foreach (var reqId in reqIds)
				{
					if (reqs.Any(x => x.RequirementId == reqId))
					{
						continue;
					}

					this.Protocol197SurveySubjectRequirementDomain.Save(
						new Protocol197SurveySubjectRequirement
						{
							Protocol197 = new Protocol197 {Id = documentId},
							Requirement = new SurveySubjectRequirement {Id = reqId}
						});
				}
			});

			return new BaseDataResult();
		}

		public IDataResult UpdateComissionDocumentsState(BaseParams baseParams)
		{			
			var documentIds = baseParams.Params.GetAs<long[]>("documentIds");
			var comissionId = baseParams.Params.GetAsId("comissionId");
			var decision = baseParams.Params.GetAs<ComissionDocumentDecision>("decValue");
			var nextCommisDate = baseParams.Params.GetAs<DateTime?>("nextCommisDate");
			var descript = baseParams.Params.GetAs<string>("description");
			var hourOfProceedings = baseParams.Params.GetAs<int>("hourOfProceedings");
			var minuteOfProceedings = baseParams.Params.GetAs<int>("minuteOfProceedings");

			foreach (long documentId in documentIds)
			{
				var commdoc = ComissionMeetingDocumentDomain.GetAll()
				.FirstOrDefault(x => x.ComissionMeeting.Id == comissionId && x.DocumentGji.Id == documentId);
				if (commdoc != null && decision != 0)
				{
					commdoc.ComissionDocumentDecision = decision;
                    commdoc.Description = descript;
                    using (var tr = Container.Resolve<IDataTransaction>())
					{
						try
						{
							ComissionMeetingDocumentDomain.Update(commdoc);
							tr.Commit();
						}
						catch
						{
							tr.Rollback();
						}
					}
				}
				if (nextCommisDate.HasValue && decision == ComissionDocumentDecision.NewComisison)
				{
					var comission = ComissionMeetingDomain.Get(comissionId);
					if (comission != null && comission.ZonalInspection != null)
					{
						var documentGji = Protocol197Domain.Get(documentId);
						var commeeting = ComissionMeetingDomain.GetAll()
							.Where(x => x.CommissionDate == nextCommisDate.Value && x.ZonalInspection == comission.ZonalInspection).FirstOrDefault();
						if (commeeting == null)
						{
							commeeting = new ComissionMeeting
							{
								ZonalInspection = new ZonalInspection { Id = comission.ZonalInspection.Id },
								CommissionDate = nextCommisDate.Value,
								Description = $"создано решением о рассмотрении протокола {documentGji.DocumentNumber} от {documentGji.DocumentDate.Value.ToString("dd.MM.yyyy")}",
								CommissionNumber = "б/н"
							};
							ComissionMeetingDomain.Save(commeeting);
						}
						var existsDoc = ComissionMeetingDocumentDomain.GetAll()
							.FirstOrDefault(x => x.ComissionMeeting == commeeting && x.DocumentGji == documentGji);
						if (existsDoc == null)
						{
							documentGji.ComissionMeeting = commeeting;
							documentGji.HourOfProceedings = hourOfProceedings;
							documentGji.MinuteOfProceedings = minuteOfProceedings;
							Protocol197Domain.Update(documentGji);
							ComissionMeetingDocumentDomain.Save(new ComissionMeetingDocument
							{
								ComissionDocumentDecision = ComissionDocumentDecision.NotSet,
								Description = $"Решение о рассмотрении принято на комиссии {comission.CommissionNumber} от {comission.CommissionDate.ToString("dd.MM.yyyy")}",
								DocumentGji = new DocumentGji { Id = documentGji.Id },
								ComissionMeeting = commeeting
							});
						}

					}
				}
				else if (nextCommisDate.HasValue && decision == ComissionDocumentDecision.Decline)
				{
					var comission = ComissionMeetingDomain.Get(comissionId);
					if (comission != null && comission.ZonalInspection != null)
					{
						var documentGji = Protocol197Domain.Get(documentId);
						var commeeting = ComissionMeetingDomain.GetAll()
							.Where(x => x.CommissionDate == nextCommisDate.Value && x.ZonalInspection == comission.ZonalInspection).FirstOrDefault();
						if (commeeting == null)
						{
							commeeting = new ComissionMeeting
							{
								ZonalInspection = new ZonalInspection { Id = comission.ZonalInspection.Id },
								CommissionDate = nextCommisDate.Value,
								Description = $"создано решением об отложении рассмотрения протокола {documentGji.DocumentNumber} от {documentGji.DocumentDate.Value.ToString("dd.MM.yyyy")}",
								CommissionNumber = "б/н"
							};
							ComissionMeetingDomain.Save(commeeting);
						}
						var existsDoc = ComissionMeetingDocumentDomain.GetAll()
							.FirstOrDefault(x => x.ComissionMeeting == commeeting && x.DocumentGji == documentGji);
						if (existsDoc == null)
						{
							documentGji.ComissionMeeting = commeeting;
							documentGji.HourOfProceedings = hourOfProceedings;
							documentGji.MinuteOfProceedings = minuteOfProceedings;
							Protocol197Domain.Update(documentGji);
							ComissionMeetingDocumentDomain.Save(new ComissionMeetingDocument
							{
								ComissionDocumentDecision = ComissionDocumentDecision.NotSet,
								Description = string.IsNullOrEmpty(descript) ? $"Решение об отложении рассмотрения принято на комиссии {comission.CommissionNumber} от {comission.CommissionDate.ToString("dd.MM.yyyy")}" : descript,
								DocumentGji = new DocumentGji { Id = documentGji.Id },
								ComissionMeeting = commeeting
							});
						}

					}
				}
				else if (decision == ComissionDocumentDecision.Resolution)
				{
					var resolPros = Protocol197Domain.GetAll()
									   .Where(x => x.Id == documentId)
									   .FirstOrDefault();
					var existsresolution = ChildrenDomain.GetAll()
						.FirstOrDefault(x => x.Parent.Id == resolPros.Id && x.Children.TypeDocumentGji == TypeDocumentGji.Resolution);

					if (resolPros == null)
					{
						throw new Exception("Не удалось получить протокол 19.7");
					}
					if (existsresolution != null)
					{
						throw new Exception($"По протоколу {resolPros.DocumentNumber} уже создано постановление");
					}
					if (resolPros.ComissionMeeting == null)
					{
                        throw new Exception($"Протокол {resolPros.DocumentNumber} не привязан к комиссии");
                    }

					Resolution resolution = null;
					//учитывать номер
					int docnum = resolPros.Inspection.InspectionNum.HasValue ? resolPros.Inspection.InspectionNum.Value : GetNextNumber();
					if (resolPros.IndividualPerson != null)
					{
						resolution = new Resolution()
						{
							Inspection = resolPros.Inspection,
							TypeDocumentGji = TypeDocumentGji.Resolution,
							GisUin = resolPros.UIN,
							Contragent = resolPros.Contragent,
							Executant = resolPros.Executant,
							Position = resolPros.PersonPosition,
							Surname = resolPros.IndividualPerson.Fio.Split(' ').Length > 2 ? resolPros.IndividualPerson.Fio.Split(' ')[0] : "",
							FirstName = resolPros.IndividualPerson.Fio.Split(' ').Length > 2 ? resolPros.IndividualPerson.Fio.Split(' ')[1] : "",
							Patronymic = resolPros.IndividualPerson.Fio.Split(' ').Length > 2 ? resolPros.IndividualPerson.Fio.Split(' ')[2] : "",
							Fio = resolPros.IndividualPerson?.Fio,
							PhysicalPerson = resolPros.IndividualPerson?.Fio,
							PhysicalPersonInfo = resolPros.PhysicalPersonInfo,
							PhysicalPersonDocType = resolPros.PhysicalPersonDocType,
							PhysicalPersonDocumentNumber = resolPros.PhysicalPersonDocumentNumber,
							PhysicalPersonDocumentSerial = resolPros.PhysicalPersonDocumentSerial,
							PhysicalPersonIsNotRF = resolPros.PhysicalPersonIsNotRF,
							TypeInitiativeOrg = TypeInitiativeOrgGji.HousingInspection,
							Sanction = new SanctionGji { Id = 2, Name = "Административный штраф" },
							Paided = YesNoNotSet.NotSet,
							DocumentDate = resolPros.ComissionMeeting != null ? resolPros.ComissionMeeting.CommissionDate : DateTime.Now,
							FineMunicipality = GetFineMO(resolPros.Id),
							DocumentNum = docnum,
							DocumentNumber = docnum.ToString(),
							Job = resolPros.Job,
							FamilyStatus = resolPros.FamilyStatus,
							IndividualPerson = resolPros.IndividualPerson,
                            ConcederationResult = ConcederationResultDomain.GetAll().FirstOrDefault(x => x.Name == "постановление о назначении административного наказания")
                        };
					}
					else
					{
						resolution = new Resolution()
						{
							Inspection = resolPros.Inspection,
							TypeDocumentGji = TypeDocumentGji.Resolution,
							Contragent = resolPros.Contragent,
							Executant = resolPros.Executant,
							PhysicalPersonInfo = resolPros.PhysicalPersonInfo,
							PhysicalPersonDocType = resolPros.PhysicalPersonDocType,
							PhysicalPersonDocumentNumber = resolPros.PhysicalPersonDocumentNumber,
							PhysicalPersonDocumentSerial = resolPros.PhysicalPersonDocumentSerial,
							PhysicalPersonIsNotRF = resolPros.PhysicalPersonIsNotRF,
							TypeInitiativeOrg = TypeInitiativeOrgGji.HousingInspection,
							Sanction = new SanctionGji { Id = 2, Name = "Административный штраф" },
							Paided = YesNoNotSet.NotSet,
							DocumentDate = resolPros.ComissionMeeting != null ? resolPros.ComissionMeeting.CommissionDate : DateTime.Now,
							FineMunicipality = GetFineMO(resolPros.Id),
							DocumentNum = docnum,
							DocumentNumber = docnum.ToString(),
                            ConcederationResult = ConcederationResultDomain.GetAll().FirstOrDefault(x => x.Name == "постановление о назначении административного наказания")
                        };
					}
					#region Формируем этап проверки
					// Если у родительского документа есть этап у которого есть родитель
					// тогда берем именно родительский Этап (Просто для красоты в дереве, чтобы не плодить дочерние узлы)
					var parentStage = resolPros.Stage;
					if (parentStage != null && parentStage.Parent != null)
					{
						parentStage = parentStage.Parent;
					}

					InspectionGjiStage newStage = null;

					var currentStage = InspectionStageDomain.GetAll().FirstOrDefault(x => x.Parent == parentStage && x.TypeStage == TypeStage.Resolution);

					if (currentStage == null)
					{
						// Если этап ненайден, то создаем новый этап
						currentStage = new InspectionGjiStage
						{
							Inspection = resolPros.Inspection,
							TypeStage = TypeStage.Resolution,
							Parent = parentStage,
							Position = 1
						};
						var stageMaxPosition = InspectionStageDomain.GetAll().Where(x => x.Inspection.Id == resolPros.Inspection.Id)
											 .OrderByDescending(x => x.Position).FirstOrDefault();

						if (stageMaxPosition != null)
						{
							currentStage.Position = stageMaxPosition.Position + 1;
						}

						// Фиксируем новый этап чтобы потом незабыть сохранить 
						newStage = currentStage;
					}

					resolution.Stage = currentStage;
					#endregion

					#region формируем связь с родителем
					var parentChildren = new DocumentGjiChildren
					{
						Parent = resolPros,
						Children = resolution
					};
					#endregion

					#region Сохранение
					using (var tr = Container.Resolve<IDataTransaction>())
					{
						try
						{
							if (newStage != null)
							{
								this.InspectionStageDomain.Save(newStage);
							}

							this.ResolutionDomain.Save(resolution);

							this.ChildrenDomain.Save(parentChildren);

							tr.Commit();
						}
						catch
						{
							tr.Rollback();
							throw;
						}
					}
					#endregion


				}
			}

			

			return new BaseDataResult();
		}

		public IDataResult UpdateComissionDocumentState(BaseParams baseParams)
		{
			var ismass = baseParams.Params.GetAs<bool>("massOperation", false);
			if (ismass)
			{
				return DoMassAction(baseParams);
			}
			else
			{
				return DoSimpleAction(baseParams);
			}
			
		}



		/// <summary>
		/// Добавить Массово Расценки по работе
		/// </summary>
		/// <param name="baseParams"></param>
		/// <returns>Результат выполнения</returns>
		public IDataResult DoMassAddition(BaseParams baseParams)
		{
			var TypeAnnex = baseParams.Params.GetAs<TypeAnnex>("TypeAnnex");
			var cnt = baseParams.Params.GetAs<int>("Count");
			var protId = baseParams.Params.GetAs<long>("protId");
			var IsProof = baseParams.Params.GetAs<bool>("IsProof", false);
			var annexDomain = Container.Resolve<IDomainService<Protocol197Annex>>();
			if (protId == 0)
			{
				return new BaseDataResult { Success = false, Message = "Сервер не получил параметры операции" };
			}
			using (var transaction = this.Container.Resolve<IDataTransaction>())
			{
				try
				{
					for (int c=0; c< cnt;c++)
					{
						annexDomain.Save(new Protocol197Annex
						{
							IsProof = IsProof,
							Description = "Добавлен массово",
							TypeAnnex = TypeAnnex,
							Protocol197 = new Protocol197 {Id = protId }
						});
					}

					transaction.Commit();
					return new BaseDataResult();
				}
				catch (ValidationException e)
				{
					transaction.Rollback();
					return new BaseDataResult { Success = false, Message = e.Message };
				}
				finally
				{
					Container.Release(annexDomain);
				}
			}

		}

		/// <summary>
		/// Добавить Массово Расценки по работе
		/// </summary>
		/// <param name="baseParams"></param>
		/// <returns>Результат выполнения</returns>
		public object GetFiasProperty(BaseParams baseParams)
		{
			
			var fiasid = baseParams.Params.GetAs<long>("fiasid");
			
			if (fiasid == 0)
			{
				return new BaseDataResult { Success = false, Message = "Сервер не получил параметры операции" };
			}
			var FiasDomain = Container.Resolve<IDomainService<FiasAddress>>();
			return FiasDomain.Get(fiasid);

		}

		public IDataResult AddDirections(BaseParams baseParams)
		{
			var documentId = baseParams.Params.GetAsId("documentId");
			var dirIds = baseParams.Params.GetAs<string>("directionIds").Return(x => x.Split(',').Select(y => y.ToLong()));

			var reqs = this.Protocol197ActivityDirectionDomain.GetAll()
				.Where(x => x.Protocol197.Id == documentId)
				.Select(x => new {x.Id, ProtocolId = x.Protocol197.Id, DirectionId = x.ActivityDirection.Id})
				.ToArray();

			this.Container.InTransaction(() =>
			{
				foreach (var dir in reqs.Where(x => !dirIds.Contains(x.DirectionId)))
				{
					this.Protocol197ActivityDirectionDomain.Delete(dir.Id);
				}

				foreach (var dirId in dirIds)
				{
					if (reqs.Any(x => x.DirectionId == dirId))
					{
						continue;
					}

					this.Protocol197ActivityDirectionDomain.Save(
						new Protocol197ActivityDirection
						{
							Protocol197 = new Protocol197 {Id = documentId},
							ActivityDirection = new ActivityDirection {Id = dirId}
						});
				}
			});

			return new BaseDataResult();
		}

		public class Protocol197GetInfoProxy
		{
			public string inspectorNames { get; set; }
			public string inspectorIds { get; set; }
			public string requirementIds { get; set; }
			public string requirementNames { get; set; }
			public string directionIds { get; set; }
			public string directionNames { get; set; }
		}

		private int GetNextNumber()
		{
			var maxNumber = ResolutionDomain.GetAll()
				.Max(x => x.DocumentNum);
			if (maxNumber.HasValue)
			{
				return maxNumber.Value + 1;
			}
			return 1;
		}

		private IDataResult DoSimpleAction(BaseParams baseParams)
		{
			var documentId = baseParams.Params.GetAsId("documentId");
			var comissionId = baseParams.Params.GetAsId("comissionId");
			var decision = baseParams.Params.GetAs<ComissionDocumentDecision>("decValue");
			var nextCommisDate = baseParams.Params.GetAs<DateTime?>("nextCommisDate");
			var descript = baseParams.Params.GetAs<string>("description");
			var hourOfProceedings = baseParams.Params.GetAs<int>("hourOfProceedings");
			var minuteOfProceedings = baseParams.Params.GetAs<int>("minuteOfProceedings");
			var commdoc = ComissionMeetingDocumentDomain.GetAll()
				.FirstOrDefault(x => x.ComissionMeeting.Id == comissionId && x.DocumentGji.Id == documentId);
			if (commdoc != null && decision != 0)
			{
				commdoc.ComissionDocumentDecision = decision;
				commdoc.Description = descript;

                using (var tr = Container.Resolve<IDataTransaction>())
				{
					try
					{
						ComissionMeetingDocumentDomain.Update(commdoc);
						tr.Commit();
					}
					catch
					{
						tr.Rollback();
					}
				}
			}
			if (nextCommisDate.HasValue && decision == ComissionDocumentDecision.NewComisison)
			{
				var comission = ComissionMeetingDomain.Get(comissionId);
				if (comission != null && comission.ZonalInspection != null)
				{
					var documentGji = Protocol197Domain.Get(documentId);
					var commeeting = ComissionMeetingDomain.GetAll()
						.Where(x => x.CommissionDate == nextCommisDate.Value && x.ZonalInspection == comission.ZonalInspection).FirstOrDefault();
					if (commeeting == null)
					{
						commeeting = new ComissionMeeting
						{
							ZonalInspection = new ZonalInspection { Id = comission.ZonalInspection.Id },
							CommissionDate = nextCommisDate.Value,
							Description = $"создано решением о рассмотрении протокола {documentGji.DocumentNumber} от {documentGji.DocumentDate.Value.ToString("dd.MM.yyyy")}",
							CommissionNumber = "б/н"
						};
						ComissionMeetingDomain.Save(commeeting);
					}
					var existsDoc = ComissionMeetingDocumentDomain.GetAll()
						.FirstOrDefault(x => x.ComissionMeeting == commeeting && x.DocumentGji == documentGji);
					if (existsDoc == null)
					{
						documentGji.ComissionMeeting = commeeting;
						documentGji.HourOfProceedings = hourOfProceedings;
						documentGji.MinuteOfProceedings = minuteOfProceedings;
						Protocol197Domain.Update(documentGji);
						ComissionMeetingDocumentDomain.Save(new ComissionMeetingDocument
						{
							ComissionDocumentDecision = ComissionDocumentDecision.NotSet,
							Description = $"Решение о рассмотрении принято на комиссии {comission.CommissionNumber} от {comission.CommissionDate.ToString("dd.MM.yyyy")}",
							DocumentGji = new DocumentGji { Id = documentGji.Id },
							ComissionMeeting = commeeting
						});
					}

				}
			}
			else if (nextCommisDate.HasValue && decision == ComissionDocumentDecision.Decline)
			{
				var comission = ComissionMeetingDomain.Get(comissionId);
				if (comission != null && comission.ZonalInspection != null)
				{
					var documentGji = Protocol197Domain.Get(documentId);
					var commeeting = ComissionMeetingDomain.GetAll()
						.Where(x => x.CommissionDate == nextCommisDate.Value && x.ZonalInspection == comission.ZonalInspection).FirstOrDefault();
					if (commeeting == null)
					{
						commeeting = new ComissionMeeting
						{
							ZonalInspection = new ZonalInspection { Id = comission.ZonalInspection.Id },
							CommissionDate = nextCommisDate.Value,
							Description = $"создано решением об отложении рассмотрения протокола {documentGji.DocumentNumber} от {documentGji.DocumentDate.Value.ToString("dd.MM.yyyy")}",
							CommissionNumber = "б/н"
						};
						ComissionMeetingDomain.Save(commeeting);
					}
					var existsDoc = ComissionMeetingDocumentDomain.GetAll()
						.FirstOrDefault(x => x.ComissionMeeting == commeeting && x.DocumentGji == documentGji);
					if (existsDoc == null)
					{
						documentGji.ComissionMeeting = commeeting;
						documentGji.HourOfProceedings = hourOfProceedings;
						documentGji.MinuteOfProceedings = minuteOfProceedings;
						Protocol197Domain.Update(documentGji);
						ComissionMeetingDocumentDomain.Save(new ComissionMeetingDocument
						{
							ComissionDocumentDecision = ComissionDocumentDecision.NotSet,
							Description = string.IsNullOrEmpty(descript) ? $"Решение об отложении рассмотрения принято на комиссии {comission.CommissionNumber} от {comission.CommissionDate.ToString("dd.MM.yyyy")}" : descript,
							DocumentGji = new DocumentGji { Id = documentGji.Id },
							ComissionMeeting = commeeting
						});
					}

				}
			}
			else if (decision == ComissionDocumentDecision.Resolution)
			{
				var resolPros = Protocol197Domain.GetAll()
								   .Where(x => x.Id == documentId)
								   .FirstOrDefault();
				var existsresolution = ChildrenDomain.GetAll()
					.FirstOrDefault(x => x.Parent.Id == resolPros.Id && x.Children.TypeDocumentGji == TypeDocumentGji.Resolution);

				if (resolPros == null)
				{
					throw new Exception("Не удалось получить протокол 19.7");
				}
				if (existsresolution != null)
				{
					throw new Exception("По данному документу уже создано постановление");
				}
                if (resolPros.ComissionMeeting == null)
                {
                    throw new Exception($"Протокол {resolPros.DocumentNumber} не привязан к комиссии");
                }

                Resolution resolution = null;
				//учитывать номер
				int docnum = GetNextNumber();
				if (resolPros.IndividualPerson != null)
				{
					resolution = new Resolution()
					{
						Inspection = resolPros.Inspection,
						TypeDocumentGji = TypeDocumentGji.Resolution,
						GisUin = resolPros.UIN,
						Contragent = resolPros.Contragent,
						Executant = resolPros.Executant,
						Position = resolPros.PersonPosition,
						Surname = resolPros.IndividualPerson.Fio.Split(' ').Length > 2 ? resolPros.IndividualPerson.Fio.Split(' ')[0] : "",
						FirstName = resolPros.IndividualPerson.Fio.Split(' ').Length > 2 ? resolPros.IndividualPerson.Fio.Split(' ')[1] : "",
						Patronymic = resolPros.IndividualPerson.Fio.Split(' ').Length > 2 ? resolPros.IndividualPerson.Fio.Split(' ')[2] : "",
						Fio = resolPros.IndividualPerson?.Fio,
						PhysicalPerson = resolPros.IndividualPerson?.Fio,
						PhysicalPersonInfo = resolPros.PhysicalPersonInfo,
						PhysicalPersonDocType = resolPros.PhysicalPersonDocType,
						PhysicalPersonDocumentNumber = resolPros.PhysicalPersonDocumentNumber,
						PhysicalPersonDocumentSerial = resolPros.PhysicalPersonDocumentSerial,
						PhysicalPersonIsNotRF = resolPros.PhysicalPersonIsNotRF,
						TypeInitiativeOrg = TypeInitiativeOrgGji.HousingInspection,
						Sanction = new SanctionGji { Id = 2, Name = "Административный штраф" },
						Paided = YesNoNotSet.NotSet,
						DocumentDate = resolPros.ComissionMeeting != null ? resolPros.ComissionMeeting.CommissionDate : DateTime.Now,
						FineMunicipality = GetFineMO(resolPros.Id),
						DocumentNum = docnum,
						DocumentNumber = docnum.ToString(),
						Job = resolPros.Job,
						FamilyStatus = resolPros.FamilyStatus,
						IndividualPerson = resolPros.IndividualPerson,
                        ConcederationResult = ConcederationResultDomain.GetAll().FirstOrDefault(x => x.Name == "постановление о назначении административного наказания")
                    };
				}
				else
				{
					resolution = new Resolution()
					{
						Inspection = resolPros.Inspection,
						TypeDocumentGji = TypeDocumentGji.Resolution,
						Contragent = resolPros.Contragent,
						Executant = resolPros.Executant,
						PhysicalPersonInfo = resolPros.PhysicalPersonInfo,
						PhysicalPersonDocType = resolPros.PhysicalPersonDocType,
						PhysicalPersonDocumentNumber = resolPros.PhysicalPersonDocumentNumber,
						PhysicalPersonDocumentSerial = resolPros.PhysicalPersonDocumentSerial,
						PhysicalPersonIsNotRF = resolPros.PhysicalPersonIsNotRF,
						TypeInitiativeOrg = TypeInitiativeOrgGji.HousingInspection,
						Sanction = new SanctionGji { Id = 2, Name = "Административный штраф" },
						Paided = YesNoNotSet.NotSet,
						DocumentDate = resolPros.ComissionMeeting != null ? resolPros.ComissionMeeting.CommissionDate : DateTime.Now,
						FineMunicipality = GetFineMO(resolPros.Id),
						DocumentNum = docnum,
						DocumentNumber = docnum.ToString(),
                        ConcederationResult = ConcederationResultDomain.GetAll().FirstOrDefault(x => x.Name == "постановление о назначении административного наказания")
                    };
				}
				#region Формируем этап проверки
				// Если у родительского документа есть этап у которого есть родитель
				// тогда берем именно родительский Этап (Просто для красоты в дереве, чтобы не плодить дочерние узлы)
				var parentStage = resolPros.Stage;
				if (parentStage != null && parentStage.Parent != null)
				{
					parentStage = parentStage.Parent;
				}

				InspectionGjiStage newStage = null;

				var currentStage = InspectionStageDomain.GetAll().FirstOrDefault(x => x.Parent == parentStage && x.TypeStage == TypeStage.Resolution);

				if (currentStage == null)
				{
					// Если этап ненайден, то создаем новый этап
					currentStage = new InspectionGjiStage
					{
						Inspection = resolPros.Inspection,
						TypeStage = TypeStage.Resolution,
						Parent = parentStage,
						Position = 1
					};
					var stageMaxPosition = InspectionStageDomain.GetAll().Where(x => x.Inspection.Id == resolPros.Inspection.Id)
										 .OrderByDescending(x => x.Position).FirstOrDefault();

					if (stageMaxPosition != null)
					{
						currentStage.Position = stageMaxPosition.Position + 1;
					}

					// Фиксируем новый этап чтобы потом незабыть сохранить 
					newStage = currentStage;
				}

				resolution.Stage = currentStage;
				#endregion

				#region формируем связь с родителем
				var parentChildren = new DocumentGjiChildren
				{
					Parent = resolPros,
					Children = resolution
				};
				#endregion

				#region Сохранение
				using (var tr = Container.Resolve<IDataTransaction>())
				{
					try
					{
						if (newStage != null)
						{
							this.InspectionStageDomain.Save(newStage);
						}

						this.ResolutionDomain.Save(resolution);

						this.ChildrenDomain.Save(parentChildren);

						tr.Commit();
					}
					catch
					{
						tr.Rollback();
						throw;
					}
				}
				#endregion


			}

			return new BaseDataResult();
		}

		private void UpdateArtLaw(long documentId)
		{
			try
			{
				var vilationList = Protocol197ViolationDomain.GetAll().Where(x => x.Document.Id == documentId).ToList();
				foreach (var viol in vilationList)
				{
					if (viol.InspectionViolation != null && viol.InspectionViolation.Violation.ArticleLaw != null && viol.InspectionViolation.Violation.ArticleLawRepeatative != null)
					{
						var artLaw = viol.InspectionViolation.Violation.ArticleLaw;
						var artLawRep = viol.InspectionViolation.Violation.ArticleLawRepeatative;
						var protArt = Protocol197ArticleLawDomain.GetAll().FirstOrDefault(x => x.Protocol197.Id == documentId && x.ArticleLaw == artLaw);
						protArt.ArticleLaw = artLawRep;
						Protocol197ArticleLawDomain.Update(protArt);
						UpdateViolationLargeText(protArt);
					}
				}
			}
			catch (Exception e)
			{
				
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

        private string GetCorrectWord(Protocol197 prot)
        {
            switch (prot.Executant.Code)
            {
                case "0":
                    return $"привлекалось";
                default:
                    return "привлекался/привлекалась";

            }
        }

        private string GetStringDt(DateTime? docDate)
        {
			if (docDate.HasValue)
			{
				return docDate.Value.ToString("dd.MM.yyyy");
			}
			else
			{
				return "н/а";
            }
         
        }

        private string GetRepeatResolution(Protocol197 prot)
        {
			var anotherRes = Protocol197AnotherResolutionDomain.GetAll().Where(x => x.Protocol197 == prot).OrderByDescending(x => x.DocumentGji.DocumentDate).FirstOrDefault();
			var comissionName = prot.ComissionMeeting.ZonalInspection.Name;
			if (anotherRes != null)
			{
				return $"постановлением {comissionName.Replace("Административная комиссия", "административной комиссии")} N{anotherRes.DocumentGji.DocumentNumber} от {GetStringDt(anotherRes.DocumentGji.DocumentDate)}, вступившим в законную силу {GetStringDt(anotherRes.DocumentGji.InLawDate)}.";
			}
			else
			{
                return $"постановлением {comissionName.Replace("Административная комиссия", "административной комиссии")}";
            }
        }

        private void UpdateViolationLargeText(Protocol197ArticleLaw protArt)
        {
            try
            {
                var violation = Protocol197ViolationDomain.GetAll().Where(x => x.Document.Id == protArt.Protocol197.Id).FirstOrDefault().InspectionViolation.Violation;
                var prot = Protocol197Domain.Get(protArt.Protocol197.Id);
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

				if (violation.ParentViolationGji != null)
				{
                    violtext += $"что повлекло нарушение {violation.NormativeDocNames}.";
                }
				else
				{
                    violtext += $"что повлекло нарушение {protArt.ArticleLaw.Name}.";
                }               

                if (violation.ParentViolationGji != null)
                {
                    var pv = ViolationGjiDomain.Get(violation.ParentViolationGji.Id);
                    violtext += $" {pv.Name} нарушает {protArt.ArticleLaw.Name}.";
                }
                violtext += $" Ранее {violatorName} уже {GetCorrectWord(prot)} к административной ответственности за аналогичное правонарушение {GetRepeatResolution(prot)}";
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

            }
            catch (Exception e)
            {

            }
        }

        private IDataResult DoMassAction(BaseParams baseParams)
		{
			var documentIds = baseParams.Params.GetAs<long[]>("documentIds");
			var comissionId = baseParams.Params.GetAsId("comissionId");
			var decision = baseParams.Params.GetAs<ComissionDocumentDecision>("decValue");
			var nextCommisDate = baseParams.Params.GetAs<DateTime?>("nextCommisDate");
			var descript = baseParams.Params.GetAs<string>("description");
			var hourOfProceedings = baseParams.Params.GetAs<int>("hourOfProceedings");
			var minuteOfProceedings = baseParams.Params.GetAs<int>("minuteOfProceedings");
			foreach (long documentId in documentIds.ToList())
			{

				var commdoc = ComissionMeetingDocumentDomain.GetAll()
					.FirstOrDefault(x => x.ComissionMeeting.Id == comissionId && x.DocumentGji.Id == documentId);
				if (commdoc != null && decision != 0)
				{
					commdoc.ComissionDocumentDecision = decision;
                    commdoc.Description = descript;
                    using (var tr = Container.Resolve<IDataTransaction>())
					{
						try
						{
							ComissionMeetingDocumentDomain.Update(commdoc);
							tr.Commit();
						}
						catch
						{
							tr.Rollback();			
						}
					}
					
				}
				if (nextCommisDate.HasValue && decision == ComissionDocumentDecision.NewComisison)
				{
					var comission = ComissionMeetingDomain.Get(comissionId);
					if (comission != null && comission.ZonalInspection != null)
					{
						var documentGji = Protocol197Domain.Get(documentId);
						var commeeting = ComissionMeetingDomain.GetAll()
							.Where(x => x.CommissionDate == nextCommisDate.Value && x.ZonalInspection == comission.ZonalInspection).FirstOrDefault();
						if (commeeting == null)
						{
							commeeting = new ComissionMeeting
							{
								ZonalInspection = new ZonalInspection { Id = comission.ZonalInspection.Id },
								CommissionDate = nextCommisDate.Value,
								Description = $"создано решением о рассмотрении протокола {documentGji.DocumentNumber} от {documentGji.DocumentDate.Value.ToString("dd.MM.yyyy")}",
								CommissionNumber = "б/н"
							};
							ComissionMeetingDomain.Save(commeeting);
						}
						var existsDoc = ComissionMeetingDocumentDomain.GetAll()
							.FirstOrDefault(x => x.ComissionMeeting == commeeting && x.DocumentGji == documentGji);
						if (existsDoc == null)
						{
							documentGji.ComissionMeeting = commeeting;
							documentGji.HourOfProceedings = hourOfProceedings;
							documentGji.MinuteOfProceedings = minuteOfProceedings;
							Protocol197Domain.Update(documentGji);
							ComissionMeetingDocumentDomain.Save(new ComissionMeetingDocument
							{
								ComissionDocumentDecision = ComissionDocumentDecision.NotSet,
								Description = $"Решение о рассмотрении принято на комиссии {comission.CommissionNumber} от {comission.CommissionDate.ToString("dd.MM.yyyy")}",
								DocumentGji = new DocumentGji { Id = documentGji.Id },
								ComissionMeeting = commeeting
							});
						}

					}
				}
				else if (nextCommisDate.HasValue && decision == ComissionDocumentDecision.Decline)
				{
					var comission = ComissionMeetingDomain.Get(comissionId);
					if (comission != null && comission.ZonalInspection != null)
					{
						var documentGji = Protocol197Domain.Get(documentId);
						var commeeting = ComissionMeetingDomain.GetAll()
							.Where(x => x.CommissionDate == nextCommisDate.Value && x.ZonalInspection == comission.ZonalInspection).FirstOrDefault();
						if (commeeting == null)
						{
							commeeting = new ComissionMeeting
							{
								ZonalInspection = new ZonalInspection { Id = comission.ZonalInspection.Id },
								CommissionDate = nextCommisDate.Value,
								Description = $"создано решением об отложении рассмотрения протокола {documentGji.DocumentNumber} от {documentGji.DocumentDate.Value.ToString("dd.MM.yyyy")}",
								CommissionNumber = "б/н"
							};
							ComissionMeetingDomain.Save(commeeting);
						}
						var existsDoc = ComissionMeetingDocumentDomain.GetAll()
							.FirstOrDefault(x => x.ComissionMeeting == commeeting && x.DocumentGji == documentGji);
						if (existsDoc == null)
						{
							documentGji.ComissionMeeting = commeeting;
							documentGji.HourOfProceedings = hourOfProceedings;
							documentGji.MinuteOfProceedings = minuteOfProceedings;
							Protocol197Domain.Update(documentGji);
							ComissionMeetingDocumentDomain.Save(new ComissionMeetingDocument
							{
								ComissionDocumentDecision = ComissionDocumentDecision.NotSet,
								Description = string.IsNullOrEmpty(descript) ? $"Решение об отложении рассмотрения принято на комиссии {comission.CommissionNumber} от {comission.CommissionDate.ToString("dd.MM.yyyy")}" : descript,
								DocumentGji = new DocumentGji { Id = documentGji.Id },
								ComissionMeeting = commeeting
							});
						}

					}
				}
				else if (decision == ComissionDocumentDecision.Resolution)
				{
					var resolPros = Protocol197Domain.GetAll()
									   .Where(x => x.Id == documentId)
									   .FirstOrDefault();
					var existsresolution = ChildrenDomain.GetAll()
						.FirstOrDefault(x => x.Parent.Id == resolPros.Id && x.Children.TypeDocumentGji == TypeDocumentGji.Resolution);

					if (resolPros == null)
					{
						throw new Exception("Не удалось получить протокол 19.7");
					}
					if (existsresolution != null)
					{
						throw new Exception("По данному документу уже создано постановление");
					}
                    if (resolPros.ComissionMeeting == null)
                    {
                        throw new Exception($"Протокол {resolPros.DocumentNumber} не привязан к комиссии");
                    }

                    Resolution resolution = null;
					//учитывать номер
					int docnum = GetNextNumber();
					if (resolPros.IndividualPerson != null)
					{
						resolution = new Resolution()
						{
							Inspection = resolPros.Inspection,
							TypeDocumentGji = TypeDocumentGji.Resolution,
							GisUin = resolPros.UIN,
							Contragent = resolPros.Contragent,
							Executant = resolPros.Executant,
							Position = resolPros.PersonPosition,
							Surname = resolPros.IndividualPerson.Fio.Split(' ').Length > 2 ? resolPros.IndividualPerson.Fio.Split(' ')[0] : "",
							FirstName = resolPros.IndividualPerson.Fio.Split(' ').Length > 2 ? resolPros.IndividualPerson.Fio.Split(' ')[1] : "",
							Patronymic = resolPros.IndividualPerson.Fio.Split(' ').Length > 2 ? resolPros.IndividualPerson.Fio.Split(' ')[2] : "",
							Fio = resolPros.IndividualPerson?.Fio,
							PhysicalPerson = resolPros.IndividualPerson?.Fio,
							PhysicalPersonInfo = resolPros.PhysicalPersonInfo,
							PhysicalPersonDocType = resolPros.PhysicalPersonDocType,
							PhysicalPersonDocumentNumber = resolPros.PhysicalPersonDocumentNumber,
							PhysicalPersonDocumentSerial = resolPros.PhysicalPersonDocumentSerial,
							PhysicalPersonIsNotRF = resolPros.PhysicalPersonIsNotRF,
							TypeInitiativeOrg = TypeInitiativeOrgGji.HousingInspection,
							Sanction = new SanctionGji { Id = 2, Name = "Административный штраф" },
							Paided = YesNoNotSet.NotSet,
							DocumentDate = resolPros.ComissionMeeting != null ? resolPros.ComissionMeeting.CommissionDate : DateTime.Now,
							DocumentNum = docnum,
							DocumentNumber = docnum.ToString(),
							FineMunicipality = GetFineMO(resolPros.Id),
							Job = resolPros.Job,
							FamilyStatus = resolPros.FamilyStatus,
							IndividualPerson = resolPros.IndividualPerson
						};
					}
					else
					{
						resolution = new Resolution()
						{
							Inspection = resolPros.Inspection,
							TypeDocumentGji = TypeDocumentGji.Resolution,
							Contragent = resolPros.Contragent,
							Executant = resolPros.Executant,
							PhysicalPersonInfo = resolPros.PhysicalPersonInfo,
							PhysicalPersonDocType = resolPros.PhysicalPersonDocType,
							PhysicalPersonDocumentNumber = resolPros.PhysicalPersonDocumentNumber,
							PhysicalPersonDocumentSerial = resolPros.PhysicalPersonDocumentSerial,
							PhysicalPersonIsNotRF = resolPros.PhysicalPersonIsNotRF,
							TypeInitiativeOrg = TypeInitiativeOrgGji.HousingInspection,
							Sanction = new SanctionGji { Id = 2, Name = "Административный штраф" },
							Paided = YesNoNotSet.NotSet,
							DocumentDate = resolPros.ComissionMeeting != null ? resolPros.ComissionMeeting.CommissionDate : DateTime.Now,
							FineMunicipality = GetFineMO(resolPros.Id),
							DocumentNum = docnum,
							DocumentNumber = docnum.ToString()
						};
					}
					#region Формируем этап проверки
					// Если у родительского документа есть этап у которого есть родитель
					// тогда берем именно родительский Этап (Просто для красоты в дереве, чтобы не плодить дочерние узлы)
					var parentStage = resolPros.Stage;
					if (parentStage != null && parentStage.Parent != null)
					{
						parentStage = parentStage.Parent;
					}

					InspectionGjiStage newStage = null;

					var currentStage = InspectionStageDomain.GetAll().FirstOrDefault(x => x.Parent == parentStage && x.TypeStage == TypeStage.Resolution);

					if (currentStage == null)
					{
						// Если этап ненайден, то создаем новый этап
						currentStage = new InspectionGjiStage
						{
							Inspection = resolPros.Inspection,
							TypeStage = TypeStage.Resolution,
							Parent = parentStage,
							Position = 1
						};
						var stageMaxPosition = InspectionStageDomain.GetAll().Where(x => x.Inspection.Id == resolPros.Inspection.Id)
											 .OrderByDescending(x => x.Position).FirstOrDefault();

						if (stageMaxPosition != null)
						{
							currentStage.Position = stageMaxPosition.Position + 1;
						}

						// Фиксируем новый этап чтобы потом незабыть сохранить 
						newStage = currentStage;
					}

					resolution.Stage = currentStage;
					#endregion

					#region формируем связь с родителем
					var parentChildren = new DocumentGjiChildren
					{
						Parent = resolPros,
						Children = resolution
					};
					#endregion

					#region Сохранение
					using (var tr = Container.Resolve<IDataTransaction>())
					{
						try
						{
							if (newStage != null)
							{
								this.InspectionStageDomain.Save(newStage);
							}

							this.ResolutionDomain.Save(resolution);

							this.ChildrenDomain.Save(parentChildren);

							tr.Commit();
						}
						catch
						{
							tr.Rollback();
							throw;
						}
					}
					#endregion


				}
			}


			return new BaseDataResult();
		}

		private Municipality GetFineMO(long protocolId)
		{
			var articleLawType = Container.Resolve<IDomainService<Protocol197ArticleLaw>>().GetAll()
			 .Where(x => x.Protocol197.Id == protocolId)
			 .Select(x => x.ArticleLaw.OmsRegion).FirstOrDefault();

			if (articleLawType == OmsRegionBelonging.OMS)
			{
				var zonalInspId = Container.Resolve<IDomainService<ComissionMeetingDocument>>().GetAll()
				.Where(x => x.DocumentGji.Id == protocolId)
				.Select(x => x.ComissionMeeting.ZonalInspection.Id).FirstOrDefault();

				var municipality = Container.Resolve<IDomainService<ZonalInspectionMunicipality>>().GetAll()
				 .Where(x => x.ZonalInspection.Id == zonalInspId)
				 .Select(x => x.Municipality).FirstOrDefault();

				return municipality;
			}
			else
			{
				return null;
			}
		}

	}
}