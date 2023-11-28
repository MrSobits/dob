namespace Bars.GkhGji.DomainService
{
    using System;
    using System.Linq;

    using B4;
    using B4.Utils;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.DomainService.Contracts;
    using Gkh.Authentification;
    using Entities;
    using Gkh.Entities;
    using Enums;
    using Castle.Windsor;
    using Utils = Bars.GkhGji.Utils.Utils;
    using System.Collections.Generic;

    /// <summary>
    /// Сервис для Приказ
    /// </summary>
    public class DisposalService : IDisposalService
    {
		/// <summary>
		/// Контейнер
		/// </summary>
        public IWindsorContainer Container { get; set; }

		/// <summary>
		/// Получить информацию
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
        public IDataResult GetInfo(BaseParams baseParams)
        {
            try
            {
                var documentId = baseParams.Params.GetAs<long>("documentId");
                var info = GetInfo(documentId);
                return new BaseDataResult(new { inspectorNames = info.InspectorNames, inspectorIds = info.InspectorIds, baseName = info.BaseName, planName = info.PlanName, toattracted = info.ToAttracted});
            }
            catch (ValidationException e)
            {
                return new BaseDataResult(false, e.Message);
            }
        }

        public virtual List<RealityObject> GetAddressList(BaseParams baseParams)
        {
            var service = Container.Resolve<IDomainService<Room>>();
            var servicereal = Container.Resolve<IDomainService<RealityObject>>();
            var serviceContragentContact = Container.Resolve<IDomainService<ContragentContact>>();
            var servicePerson = Container.Resolve<IDomainService<IndividualPerson>>();
            var loadParam = baseParams.GetLoadParam();
            var filter = baseParams.Params.GetAs<string>("filter");
            var contragentid = baseParams.Params.GetAs<long>("contragentid");
            var enumid = baseParams.Params.GetAs<long>("enumid");

            if (enumid == 4 || enumid == 1)
            {
                var data = service.GetAll()
                    .Where(x => x.RealityObject != null && x.RealityObject.Address.ToLower().StartsWith(filter.ToLower()))
                    .Select(x => x.RealityObject)
                    .ToList();

                return data;
            }
            else
            {
                var data = service.GetAll()
                    .Where(x => x.RealityObject != null && x.RealityObject.Address.ToLower().StartsWith(filter.ToLower()))
                    .Select(x => x.RealityObject)
                    .ToList();

                return data;
            }
        }

        public virtual List<RealityObject> GetFactAddressList(BaseParams baseParams)
        {
            var service = Container.Resolve<IDomainService<Room>>();
            var servicereal = Container.Resolve<IDomainService<RealityObject>>();
            var serviceContragentContact = Container.Resolve<IDomainService<ContragentContact>>();
            var servicePerson = Container.Resolve<IDomainService<IndividualPerson>>();
            var loadParam = baseParams.GetLoadParam();
            var filter = baseParams.Params.GetAs<string>("filter");
            var contragentid = baseParams.Params.GetAs<long>("contragentid");
            var enumid = baseParams.Params.GetAs<long>("enumid");

            if (enumid == 4 || enumid == 1)
            {
                var data = service.GetAll()
                    .Where(x => x.RealityObject != null && x.RealityObject.Address.ToLower().StartsWith(filter.ToLower()))
                    .Select(x => x.RealityObject)
                    .ToList();

                return data;
            }
            else
            {
                var data = servicereal.GetAll()
                    .Where(x => x.Address.ToLower().StartsWith(filter.ToLower())).ToList();

                return data;
            }
        }

        public virtual List<IndividualPerson>  GetNameList(BaseParams baseParams) 
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
                    .Where(x => x.Fio.ToLower().StartsWith(filter.ToLower())).ToList();

                return data;
            }

        }

		/// <summary>
		/// Получить список
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
        public virtual IDataResult ListView(BaseParams baseParams)
        {
            var docGjiCildrenServ = Container.Resolve<IDomainService<DocumentGjiChildren>>();

            try
            {
                var loadParam = baseParams.GetLoadParam();

                /*
                 * В качестве фильтров приходят следующие параметры
                 * dateStart - Необходимо получить документы больше даты начала
                 * dateEnd - Необходимо получить документы меньше даты окончания
                 * realityObjectId - Необходимо получить документы по дому
                */

                var dateStart = baseParams.Params.GetAs<DateTime>("dateStart");
                var dateEnd = baseParams.Params.GetAs<DateTime>("dateEnd");
                var realityObjectId = baseParams.Params.GetAs<long>("realityObjectId");


                var data = GetViewList()
                    .WhereIf(dateStart != DateTime.MinValue, x => x.DocumentDate >= dateStart)
                    .WhereIf(dateEnd != DateTime.MinValue, x => x.DocumentDate <= dateEnd)
                    .WhereIf(realityObjectId > 0, x => x.RealityObjectIds.Contains("/" + realityObjectId.ToString() + "/"))
                    .Select(x => new
                    {
                        x.Id,
                        x.State,
                        x.DateEnd,
                        x.DateStart,
                        x.DocumentDate,
                        x.DocumentNumber,
                        x.DocumentNum,
                        x.ERPID,
                        x.TypeBase,
                        x.IssuedDisposal,
                        KindCheck = x.KindCheckName,
                        x.ContragentName,
                        x.TypeDisposal,
                        MunicipalityNames = x.MunicipalityId != null ? x.MunicipalityNames : x.ContragentMuName,
                        MoSettlement = x.MoNames,
                        PlaceName = x.PlaceNames,
                        MunicipalityId = x.MunicipalityId ?? x.ContragentMuId,
                        x.IsActCheckExist,
                        x.RealityObjectCount,
                        x.TypeSurveyNames,
                        x.InspectorNames,
                        x.InspectionId,
                        x.TypeDocumentGji,
                        x.TypeAgreementProsecutor,
                        x.ControlType,
                        x.KindKNDGJI,
                        HasActSurvey = docGjiCildrenServ.GetAll().Any(y => y.Parent.Id == x.Id && y.Children.TypeDocumentGji == TypeDocumentGji.ActSurvey)

                    })
                    .Filter(loadParam, Container);

                int totalCount = data.Count();

                return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
            }
            finally
            {
                Container.Release(docGjiCildrenServ);
            }

        }

		/// <summary>
		/// Получить список пустых инспекций
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
		public IDataResult ListNullInspection(BaseParams baseParams)
        {
            var serviceDisposal = Container.Resolve<IDomainService<Disposal>>();

            try
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


                var data = serviceDisposal.GetAll()
                    .Where(x => x.TypeDisposal == TypeDisposalGji.NullInspection)
                    .WhereIf(dateStart != DateTime.MinValue, x => x.DocumentDate >= dateStart)
                    .WhereIf(dateEnd != DateTime.MinValue, x => x.DocumentDate <= dateEnd)
                    .Select(x => new
                        {
                            x.Id,
                            x.State,
                            IssuedDisposal = x.IssuedDisposal.Fio,
                            ResponsibleExecution = x.ResponsibleExecution.Fio,
                            x.DocumentNum,
                            x.DocumentNumber,
                            x.DocumentDate
                        })
                    .Filter(loadParam, Container);

                int totalCount = data.Count();

                return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
            }
            finally
            {
                Container.Release(serviceDisposal);
            }
        }

		/// <summary>
		/// Получить список из вью
		/// </summary>
		/// <returns>Модифицированных запрос</returns>
        public virtual IQueryable<ViewDisposal> GetViewList()
        {
            var userManager = Container.Resolve<IGkhUserManager>();
            var docGjiInspectorService = Container.Resolve<IDomainService<DocumentGjiInspector>>();
            var disposalService = Container.Resolve<IDomainService<Disposal>>();
            var insRealObjService = Container.Resolve<IDomainService<InspectionGjiRealityObject>>();
            var serviceViewDisposal = Container.Resolve<IDomainService<ViewDisposal>>();

            try
            {
                var inspectorList = userManager.GetInspectorIds();
                var municipalityList = userManager.GetMunicipalityIds();
                inspectorList.Clear();// убираем проверки на инспекторов
                return serviceViewDisposal.GetAll()
                    .WhereIf(inspectorList.Count > 0, x => docGjiInspectorService.GetAll().Any(y => x.Id == y.DocumentGji.Id && inspectorList.Contains(y.Inspector.Id) && y.DocumentGji.TypeDocumentGji == TypeDocumentGji.Disposal)
                                                       || disposalService.GetAll().Any(y => x.Id == y.Id && (inspectorList.Contains(y.IssuedDisposal.Id) || inspectorList.Contains(y.ResponsibleExecution.Id))))
                    .WhereIf(municipalityList.Count > 0, x => insRealObjService.GetAll().Any(y => y.Inspection.Id == x.InspectionId && municipalityList.Contains(y.RealityObject.Municipality.Id)));
            }
            finally
            {
                Container.Release(userManager);
                Container.Release(docGjiInspectorService);
                Container.Release(disposalService);
                Container.Release(insRealObjService);
                Container.Release(serviceViewDisposal);
            }
        }

		/// <summary>
		/// Получить информацию о приказе
		/// </summary>
		/// <param name="documentId">Идентификатор документа</param>
		/// <returns>Результат выполнения запроса</returns>
        public DisposalInfo GetInfo(long documentId)
        {
            var serviceDocInspector = Container.Resolve<IDocumentGjiInspectorService>();
            var serviceDisposal = Container.Resolve<IDomainService<Entities.Disposal>>();
            var serviceChildren = Container.Resolve<IDomainService<DocumentGjiChildren>>();
            var serviceviolstage = Container.Resolve<IDomainService<Entities.InspectionGjiViolStage>>();
            var serviceresolution = Container.Resolve<IDomainService<Resolution>>();


            try
            {
                var inspectorNames = string.Empty;
                var inspectorIds = string.Empty;
                var baseName = string.Empty;
                var planName = string.Empty;
                var toattracted = false;

                //вытащить все нарушения у нарушителя
                var dis = serviceDisposal.GetAll().Where(x=> x.Id == documentId).Select(x => x.Fio).ToList();

                var disperson = serviceDisposal.GetAll().Where(x => dis.Contains(x.Fio) && x.Id != documentId).Select(x => x.Id).ToList();


                var resviolation = serviceviolstage.GetAll().Where(x => disperson.Contains(x.Document.Id)).Select(x => x.InspectionViolation.Violation.Name).ToList();
                //вытащить все нарушения у диспосала
                var disposalviolation = serviceviolstage.GetAll().Where(x => x.Document.Id == documentId).Select(x => x.InspectionViolation.Violation.Name).ToList();

                var result = disposalviolation.Where(x => resviolation.Contains(x)).ToList();

                if (result.Count != 0)
                {
                    toattracted = true;
                }


                // Сначала пробегаемся по инспекторам и формируем итоговую строку наименований и строку идентификаторов
                var dataInspectors =
                    serviceDocInspector.GetInspectorsByDocumentId(documentId)
                        .Select(x => new {InspectorId = x.Inspector.Id, x.Inspector.Fio})
                        .ToList();

                foreach (var item in dataInspectors)
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

                    inspectorIds += item.InspectorId.ToString();
                }

                if (documentId > 0)
                {
                    #region Выставляем Доп свойства

                    // Получаем распоряжение
                    var disposal = serviceDisposal.GetAll().FirstOrDefault(x => x.Id == documentId);

                    if (disposal.Inspection != null)
                    {
                        switch (disposal.Inspection.TypeBase)
                        {
                            case TypeBase.PlanJuridicalPerson:
                                {
                                    GetInfoJurPerson(ref baseName, ref planName, disposal.Inspection.Id);
                                }
                                break;

                            case TypeBase.DisposalHead:
                                {
                                    GetInfoDispHead(ref baseName, ref planName, disposal.Inspection.Id);
                                }
                                break;

                            case TypeBase.ProsecutorsClaim:
                                {
                                    GetInfoProsClaim(ref baseName, ref planName, disposal.Inspection.Id);
                                }
                                break;

                            case TypeBase.Inspection:
                                {
                                    GetInfoInsCheck(ref baseName, ref planName, disposal.Inspection.Id);
                                }
                                break;

                            case TypeBase.CitizenStatement:
                                {
                                    // если распоряжение создано на основе обращения граждан
                                    GetInfoCitizenStatement(ref baseName, ref planName, disposal.Inspection.Id);
                                }

                                break;

                            case TypeBase.ActivityTsj:
                                baseName = "Проверка деятельности ТСЖ";
                                break;

                            case TypeBase.HeatingSeason:
                                baseName = "Подготовка к отопительному сезону";
                                break;

                            case TypeBase.PlanAction:
                                {
                                    GetInfoPlanAction(ref baseName, ref planName, disposal.Inspection.Id);
                                }
                                break;

							case TypeBase.LicenseApplicants:
								{
									GetInfoLicenseApplicants(ref baseName, ref planName, disposal.Inspection.Id);
								}
								break;
						}

                        if (disposal.TypeDisposal == TypeDisposalGji.DocumentGji)
                        {
                            baseName = "Проверка исполнения предписаний";
                            planName = string.Empty;

                            var data = serviceChildren.GetAll()
                                .Where(x => x.Children.Id == disposal.Id)
                                .Select(x => new
                                {
                                    x.Parent.DocumentDate,
                                    x.Parent.DocumentNumber,
                                    x.Parent.TypeDocumentGji
                                })
                                .ToList();

                            foreach (var item in data)
                            {
                                var docName = Utils.GetDocumentName(item.TypeDocumentGji);

                                if (!string.IsNullOrEmpty(planName)) planName += ", ";

                                planName += string.Format(
                                    "{0} №{1} от {2}",
                                    docName,
                                    item.DocumentNumber,
                                    item.DocumentDate.ToDateTime().ToShortDateString());
                            }
                        }
                        else if (disposal.TypeDisposal == TypeDisposalGji.Base || disposal.TypeDisposal == TypeDisposalGji.Licensing)
                        {
                            if (serviceChildren.GetAll().Any(x => x.Children.Id == documentId))
                            {
                                baseName = "Документарная проверка";
                                planName = string.Empty;

                                var data =
                                    serviceChildren.GetAll()
                                        .Where(x => x.Children.Id == disposal.Id)
                                        .Select(x => new
                                        {
                                            x.Parent.DocumentDate,
                                            x.Parent.DocumentNumber,
                                            x.Parent.TypeDocumentGji
                                        })
                                        .ToList();

                                foreach (var item in data)
                                {
                                    var docName = Utils.GetDocumentName(item.TypeDocumentGji);

                                    if (!string.IsNullOrEmpty(planName)) planName += ", ";

                                    planName += string.Format(
                                        "{0} №{1} от {2}",
                                        docName,
                                        item.DocumentNumber,
                                        item.DocumentDate.ToDateTime().ToShortDateString());
                                }
                            }
                        }
                    }

                    #endregion
                }

                return new DisposalInfo(inspectorNames, inspectorIds, baseName, planName, toattracted);
            }
            catch (ValidationException e)
            {
                throw e;
            }
            finally
            {
                Container.Release(serviceDocInspector);
                Container.Release(serviceDisposal);
                Container.Release(serviceChildren);
            }
        }

        protected virtual void GetInfoCitizenStatement(ref string baseName, ref string planName, long inspectionId)
        {
            // распоряжение создано на основе обращения граждан,
            // поле planName = "Обращение № Номер комиссии"

            var serviceAppealCits = Container.Resolve<IDomainService<InspectionAppealCits>>();
            try
            {
                baseName = "Обращение граждан";

                // Получаем из основания наименование плана
                var baseStatement = serviceAppealCits
                    .GetAll()
                        .Where(x => x.Inspection.Id == inspectionId)
                        .Select(x => x.AppealCits.DocumentNumber + " (" + x.AppealCits.NumberGji + ")")
                        .AggregateWithSeparator(", ");

                if (!string.IsNullOrWhiteSpace(baseStatement))
                {
                    planName = string.Format("Обращение № {0}", baseStatement);
                }
            }
            finally
            {
                Container.Release(serviceAppealCits);
            }
        }

		/// <summary>
		/// Добавить предоставляемые документы автоматически (для Сахи)
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
		public virtual IDataResult AutoAddProvidedDocuments(BaseParams baseParams)
        {
            return null;
        }

        protected virtual void GetInfoPlanAction(ref string baseName, ref string planName, long inspectionId)
        {
            var serviceBasePlanAction = Container.Resolve<IDomainService<BasePlanAction>>();

            try
            {
                baseName = "Проверка по плану мероприятий";

                // Получаем из Проверки юр лиц Наименование Плана
                planName = serviceBasePlanAction.GetAll()
                                    .Where(x => x.Id == inspectionId)
                                    .Select(x => x.Plan.Name)
                                    .FirstOrDefault();
            }
            finally
            {
                Container.Release(serviceBasePlanAction);
            }

        }

        protected virtual void GetInfoInsCheck(ref string baseName, ref string planName, long inspectionId)
        {
            var serviceBaseInsCheck = Container.Resolve<IDomainService<BaseInsCheck>>();

            try
            {
                baseName = "План инспекционных проверок";

                // Получаем из Проверки юр лиц Наименование Плана
                planName = serviceBaseInsCheck.GetAll()
                                    .Where(x => x.Id == inspectionId)
                                    .Select(x => x.Plan.Name)
                                    .FirstOrDefault();
            }
            finally
            {
                Container.Release(serviceBaseInsCheck);
            }

        }

        protected virtual void GetInfoProsClaim(ref string baseName, ref string planName, long inspectionId)
        {
            var serviceBaseProsClaim = Container.Resolve<IDomainService<BaseProsClaim>>();

            try
            {
                baseName = "Требование прокуратуры";

                // Получаем из проверкок по распоряжению руководства значения документа
                var dispHead = serviceBaseProsClaim.GetAll()
                                        .Where(x => x.Id == inspectionId)
                                        .Select(x => new
                                        {
                                            x.DocumentName,
                                            x.DocumentNumber,
                                            x.DocumentDate
                                        })
                                        .FirstOrDefault();

                if (dispHead != null)
                {
                    planName = string.Format("{0} №{1} от {2}", dispHead.DocumentName,
                                             dispHead.DocumentNumber,
                                             dispHead.DocumentDate !=null ? dispHead.DocumentDate.ToDateTime().ToShortDateString() : "Не заполнено");
                }
            }
            finally
            {
                Container.Release(serviceBaseProsClaim);
            }

        }

        protected virtual void GetInfoDispHead(ref string baseName, ref string planName, long inspectionId)
        {
            var serviceBaseDispHead = Container.Resolve<IDomainService<BaseDispHead>>();

            try
            {
                baseName = "Выявленное правонарушение";

                // Получаем из проверкок по распоряжению руководства значения документа
                var dispHead = serviceBaseDispHead.GetAll()
                                        .Where(x => x.Id == inspectionId)
                                        .Select(x => new
                                        {
                                            x.DocumentName,
                                            x.DocumentNumber,
                                            x.DocumentDate
                                        })
                                        .FirstOrDefault();

                if (dispHead != null)
                {
                    planName = string.Format("{0} №{1} от {2}", dispHead.DocumentName,
                                             dispHead.DocumentNumber,
                                             dispHead.DocumentDate.ToDateTime().ToShortDateString());
                }
            }
            finally
            {
                Container.Release(serviceBaseDispHead);
            }

        }

        protected virtual void GetInfoJurPerson(ref string baseName, ref string planName, long inspectionId)
        {
            var serviceBaseJurPerson = Container.Resolve<IDomainService<BaseJurPerson>>();

            try
            {
                baseName = "Плановая проверка юр.лица";

                // Получаем из Проверки юр лиц Наименование Плана
                planName = serviceBaseJurPerson.GetAll()
                                    .Where(x => x.Id == inspectionId)
                                    .Select(x => x.Plan.Name)
                                    .FirstOrDefault();
            }
            finally
            {
                Container.Release(serviceBaseJurPerson);
            }
        }

		protected virtual void GetInfoLicenseApplicants(ref string baseName, ref string planName, long inspectionId)
		{
			var serviceBaseLicenseApplicants = Container.Resolve<IDomainService<BaseLicenseApplicants>>();

			try
			{
				baseName = "Проверка соискателей лицензии";

				var request = serviceBaseLicenseApplicants.GetAll()
					.FirstOrDefault(x => x.Id == inspectionId);

				if (request != null && request.ManOrgLicenseRequest != null)
				{
					planName = "Обращение за выдачей лицензии № {0}".FormatUsing(request.ManOrgLicenseRequest.RegisterNumber);
				}
			}
			finally
			{
				Container.Release(serviceBaseLicenseApplicants);
			}
		}
	}
}