namespace Bars.GkhGji.DomainService
{
    using System;
    using System.Linq;
    using Castle.Windsor;

    using B4;
    using B4.Utils;
    using Enums;
    using Entities;
    using Gkh.Domain;
    using Gkh.Enums;
    using Gkh.Authentification;

	/// <summary>
	/// Сервис для работы с Акт проверки
	/// </summary>
    public class ActCheckService : IActCheckService
    {
		/// <summary>
		/// Контейнер
		/// </summary>
        public IWindsorContainer Container { get; set; }

		/// <summary>
		/// Домен сервис для Инспектор документа ГЖИ
		/// </summary>
        public IDocumentGjiInspectorService InspectorDomain { get; set; }

		/// <summary>
		/// Домен сервис для Дом акта проверки
		/// </summary>
        public IDomainService<ActCheckRealityObject> ActCheckRoDomain { get; set; }

		/// <summary>
		/// Домен сервис для Нарушение акта проверки
		/// </summary>
		public IDomainService<ActCheckViolation> ActCheckViolDomain { get; set; }

		/// <summary>
		/// Домен сервис для Акт проверки
		/// </summary>
		public IDomainService<ActCheck> ActCheckDomain { get; set; }

		/// <summary>
		/// Получить информацию
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
        public virtual IDataResult GetInfo(BaseParams baseParams)
        {
            var documentId = baseParams.Params.GetAsId("documentId");
                
            // получаем адрес дома и количество домов
            var realityObjAddress = string.Empty;
            var realityObjCount = 0;
	        var personsWhoHaveViolated = string.Empty;
			var officialsGuiltyActions = string.Empty; 

            var actCheckRealityObjs = ActCheckRoDomain.GetAll().Where(x => x.ActCheck.Id == documentId).ToList();
            if (actCheckRealityObjs.Count == 1)
            {
                var actCheckRealityObj = actCheckRealityObjs.FirstOrDefault();
                realityObjCount = 1;
                if (actCheckRealityObj != null)
                {
                    realityObjAddress = actCheckRealityObj.RealityObject != null && actCheckRealityObj.RealityObject.FiasAddress != null ? actCheckRealityObj.RealityObject.FiasAddress.AddressName : string.Empty;
	                personsWhoHaveViolated = actCheckRealityObj.PersonsWhoHaveViolated;
					officialsGuiltyActions = actCheckRealityObj.OfficialsGuiltyActions;
                }
            }

            // поулчаем основание проверки
            var typeBase = ActCheckDomain.GetAll().Where(x => x.Id == documentId).Select(x => x.Inspection.TypeBase).FirstOrDefault();

            var inspectorNames = string.Empty;
            var inspectorIds = string.Empty;
            
            var dataInspectors = InspectorDomain.GetInspectorsByDocumentId(documentId)
                .Select(x => new
                {
                    InspectorId = x.Inspector.Id,
                    x.Inspector.Fio
                })
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

            // Получаем Признак выявлены или не выявлены нарушения?
            var isExistViolation = ActCheckViolDomain.GetAll().Any(x => x.Document.Id == documentId);


            // получаем вид проверки из приказа (распоряжения)
            var parentDisposalKindCheck = Container.Resolve<IDomainService<DocumentGjiChildren>>().GetAll()
                .Where(x => x.Children.Id == documentId && x.Parent.TypeDocumentGji == TypeDocumentGji.Disposal)
                .Select(x => (x.Parent as Disposal).KindCheck)
                .FirstOrDefault();

            var typeCheck = parentDisposalKindCheck != null ? parentDisposalKindCheck.Code : 0;

	        return
		        new BaseDataResult(
			        new
			        {
				        inspectorNames,
				        inspectorIds,
				        typeBase,
				        realityObjAddress,
				        realityObjCount,
				        isExistViolation,
				        typeCheck,
				        personsWhoHaveViolated,
				        officialsGuiltyActions
			        });
        }

		/// <summary>
		/// Получить список из вьюхи
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
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

            var serviceActCheckRobject = Container.Resolve<IDomainService<ActCheckRealityObject>>();
            var serviceDocumentChildren = Container.Resolve<IDomainService<DocumentGjiChildren>>();

            var data = GetViewList()
                .WhereIf(dateStart > DateTime.MinValue, x => x.DocumentDate >= dateStart)
                .WhereIf(dateEnd > DateTime.MinValue, x => x.DocumentDate <= dateEnd)
                .WhereIf(realityObjectId > 0, y => serviceActCheckRobject.GetAll().Any(x => x.ActCheck.Id == y.Id && x.RealityObject.Id == realityObjectId))
                .Select(x => new
                    {
                        x.Id,
                        x.State,
                        x.DocumentDate,
                        x.DocumentNumber,
                        x.DocumentNum,
                        MunicipalityNames = x.MunicipalityId != null ? x.MunicipalityNames : x.ContragentMuName,
                        MoSettlement = x.MoNames,
                        PlaceName = x.PlaceNames,
                        MunicipalityId = x.MunicipalityId ?? x.ContragentMuId,
                        x.ContragentName,
                        RealityObjectCount = serviceActCheckRobject.GetAll().Count(y => y.ActCheck.Id == x.Id),
                        CountExecDoc = serviceDocumentChildren.GetAll()
                            .Count(y => y.Parent.Id == x.Id && (y.Children.TypeDocumentGji == TypeDocumentGji.Protocol || y.Children.TypeDocumentGji == TypeDocumentGji.Prescription)),
                        HasViolation = serviceActCheckRobject.GetAll().Any(y => y.ActCheck.Id == x.Id && y.HaveViolation == YesNoNotSet.Yes),
                        x.InspectorNames,
                        x.InspectionId,
                        x.TypeBase,
                        x.TypeDocumentGji,
                        x.ControlType
                    })
                .Filter(loadParam, Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }

        /// <summary>
        /// В данном методе идет получение списка актов по Этапу проверки (в этапе может быть больше 1 акта)
        /// </summary>
        public IDataResult ListForStage(BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();
            var actCheckService = Container.Resolve<IDomainService<ActCheck>>();
            var actCheckViolService = Container.Resolve<IDomainService<ActCheckRealityObject>>();

            try
            {
                var stageId = baseParams.Params.GetAs("stageId", 0L);

                var dictRoAddress =
                    actCheckViolService.GetAll()
                                       .Where(x => x.ActCheck.Stage.Id == stageId)
                                       .Where(x => x.RealityObject != null)
                                       .Select(
                                           x =>
                                           new
                                               {
                                                   actId = x.ActCheck.Id,
                                                   address = x.RealityObject.Address
                                               })
                                       .AsEnumerable()
                                       .GroupBy(x => x.actId)
                                       .ToDictionary(x => x.Key, y => y.Select(x => x.address).FirstOrDefault());


                var data =
                    actCheckService.GetAll()
                                   .Where(x => x.Stage.Id == stageId)
                                   .Select(x => new { x.Id, x.TypeDocumentGji, x.DocumentDate, x.DocumentNumber, x.State })
                                   .AsEnumerable()
                                   .Select(
                                       x =>
                                       new
                                           {
                                               x.Id,
                                               DocumentId = x.Id,
                                               x.TypeDocumentGji,
                                               x.DocumentDate,
                                               x.DocumentNumber,
											   x.State,
                                               Address = dictRoAddress.ContainsKey(x.Id) ? dictRoAddress[x.Id] : null
                                           })
                                   .AsQueryable()
                                   .Filter(loadParam, Container)
                                   .Order(loadParam)
                                   .ToList();
                    

                int totalCount = data.Count();

                return new ListDataResult(data, totalCount);
            }
            finally
            {
                Container.Release(actCheckService);
                Container.Release(actCheckViolService);
            }
        }

		/// <summary>
		/// Получить список из вьюхи
		/// </summary>
		/// <returns>Результат выполнения</returns>
        public IQueryable<ViewActCheck> GetViewList()
        {
            var userManager = Container.Resolve<IGkhUserManager>();
            var inspectorIds = userManager.GetInspectorIds();
            var municipalityids = userManager.GetMunicipalityIds();

            var serviceDocumentInspector = Container.Resolve<IDomainService<DocumentGjiInspector>>();
            var serviceActCheckRobject = Container.Resolve<IDomainService<ActCheckRealityObject>>();
            var serviceViewActCheck = Container.Resolve<IDomainService<ViewActCheck>>();

            try
            {
                inspectorIds.Clear(); //убираем проверки на инспекторов
                return serviceViewActCheck.GetAll()
               .WhereIf(inspectorIds.Count > 0, actCheck => serviceDocumentInspector.GetAll()
                   .Any(docInsp => docInsp.DocumentGji.Id == actCheck.Id && inspectorIds.Contains(docInsp.Inspector.Id)))
               .WhereIf(municipalityids.Count > 0, y => serviceActCheckRobject.GetAll()
                   .Any(x => x.ActCheck.Id == y.Id && municipalityids.Contains(x.RealityObject.Municipality.Id)));
            }
            finally
            {
                Container.Release(serviceDocumentInspector);
                Container.Release(serviceActCheckRobject);
                Container.Release(serviceViewActCheck);
            }
        }
    }
}