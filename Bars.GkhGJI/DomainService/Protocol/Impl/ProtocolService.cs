namespace Bars.GkhGji.DomainService
{
    using System;
    using System.Linq;

    using B4;
    using B4.Utils;
    using Gkh.Authentification;
    using Entities;
    using Utils;
    using Castle.Windsor;
    using Enums;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Сервис протоколов
    /// </summary>
    public class ProtocolService : ProtocolService<Protocol>
    {
    }

    /// <summary>
    /// Базовый сервис протоколов
    /// </summary>
    /// <typeparam name="T">Тип</typeparam>
    public class ProtocolService<T> : IProtocolService
        where T : Protocol
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Получение информации о протоколе
        /// </summary>
        /// <param name="documentId">Идентификатор документа</param>
        /// <returns>Информации о протоколе</returns>
        public virtual IDataResult GetInfo(long? documentId)
        {
            var serviceInspector = this.Container.Resolve<IDomainService<DocumentGjiInspector>>();
            var serviceChildren = this.Container.Resolve<IDomainService<DocumentGjiChildren>>();
            var serviceDisposal = this.Container.Resolve<IDomainService<Disposal>>();

            try
            {
                var inspectorNames = string.Empty;
                var inspectorIds = string.Empty;
                var baseName = string.Empty;
                int? passportNumber = 0;
                int? passportSeries = 0;
                var placeResidence = string.Empty;
                var actuallyResidence = string.Empty;
                DateTime? dataBirth = DateTime.Now;
                long kusp = 0;

                // Сначала пробегаемся по инспекторам и формируем итоговую строку наименований и строку идентификаторов
                var inspectors =
                    serviceInspector.GetAll()
                                    .Where(x => x.DocumentGji.Id == documentId)
                                    .Select(x => new { x.Inspector.Id, x.Inspector.Fio })
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

                // Пробегаемся по документам на основе которого создано предписание
                var parents = serviceChildren
                             .GetAll()
                             .Where(x => x.Children.Id == documentId)
                             .Select(
                                 x =>
                                 new
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

                    baseName += string.Format(
                        "{0} №{1} от {2}",
                        docName,
                        doc.DocumentNumber,
                        doc.DocumentDate.ToDateTime().ToShortDateString());
                }
                //Вытягиваем данные из материалов
                var disposalcontent = serviceDisposal
                    .GetAll()
                    .Where(x => x.Id == parents[0].parentId)
                    .Select(x => new
                    {
                        x.PassportNumber,
                        x.PassportSeries,
                        x.PlaceResidence,
                        x.ActuallyResidence,
                        x.DateBirth,
                        x.NumberKUSP
                    });

                foreach (var content in disposalcontent) 
                {
                    passportNumber = content.PassportNumber;
                    passportSeries = content.PassportSeries;
                    placeResidence = content.PlaceResidence;
                    actuallyResidence = content.ActuallyResidence;
                    dataBirth = content.DateBirth;
                    kusp = content.NumberKUSP;
                }

                return new BaseDataResult(new ProtocolGetInfoProxy { 
                    InspectorNames = inspectorNames, 
                    InspectorIds = inspectorIds, 
                    BaseName = baseName, 
                    PassportNumber = passportNumber, 
                    PassportSeries = passportSeries, 
                    ActuallyResidence = actuallyResidence,
                    DataBirth = dataBirth,
                    PlaceResidence = placeResidence,
                    NumberKUSP = kusp
                });
            }
            catch (ValidationException e)
            {
                return new BaseDataResult { Success = false, Message = e.Message };
            }
            finally
            {
                this.Container.Release(serviceInspector);
                this.Container.Release(serviceChildren);
                this.Container.Release(serviceDisposal);
            }
        }

        /// <summary>
        /// Получение списка протоколов
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Список протоколов</returns>
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
                    x.ControlType,
                    x.TypeDocumentGji,
                    x.ArticleLaw,
                    HasResolution = GetResolution(x.Id, x.DateToCourt),
                    x.InspectorPosition,
                    x.ToCourt,
                    x.JudSectorName,
                    x.DateToCourt,
                    Penalty = GetPenalty(x.Id) ?? 0,
                    x.DateViolation,
                    x.PhysicalPerson
                })
                .Filter(loadParam, this.Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }


        private bool GetResolution(long protocolId, DateTime? dateToCourt) 
        {
            if (!dateToCourt.HasValue)
            {
                return true;
            }
            if (dateToCourt.Value.AddMonths(2) > DateTime.Now) 
            {
                return true;
            }
            var childrenId = this.Container.Resolve<IDomainService<DocumentGjiChildren>>().GetAll()
             .Where(x => x.Parent.Id == protocolId && x.Children.TypeDocumentGji == TypeDocumentGji.Resolution)
            .Select(x => x.Children.Id).FirstOrDefault();


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



        /// <summary>
        /// В данном методе идет получение списка актов по Этапу проверки (в этапе может быть больше 1 акта)
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат выполнения запроса</returns>
        public virtual IDataResult ListForStage(BaseParams baseParams)
        {
            var protService = this.Container.Resolve<IDomainService<T>>();
            var protViolService = this.Container.Resolve<IDomainService<ProtocolViolation>>();

            try
            {
                var loadParam = baseParams.GetLoadParam();
                var stageId = baseParams.Params.GetAs("stageId", 0L);

                var dictRoAddress =
                    protViolService.GetAll()
                                       .Where(x => x.Document.Stage.Id == stageId)
                                       .Where(x => x.InspectionViolation.RealityObject != null)
                                       .Select(
                                           x =>
                                           new
                                           {
                                               actId = x.Document.Id,
                                               address = x.InspectionViolation.RealityObject.Address
                                           })
                                       .AsEnumerable()
                                       .GroupBy(x => x.actId)
                                       .ToDictionary(x => x.Key, y => y.Select(x => x.address).FirstOrDefault());


                var data =
                    protService.GetAll()
                                   .Where(x => x.Stage.Id == stageId)
                                   .Select(x => new { x.Id, x.TypeDocumentGji, x.DocumentDate, x.DocumentNumber })
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
                                           Address = dictRoAddress.ContainsKey(x.Id) ? dictRoAddress[x.Id] : null
                                       })
                                   .AsQueryable()
                                   .Filter(loadParam, this.Container)
                                   .Order(loadParam)
                                   .ToList();


                var totalCount = data.Count();

                return new ListDataResult(data, totalCount);
            }
            finally
            {
                this.Container.Release(protService);
                this.Container.Release(protViolService);
            }
        }

        /// <summary>
        /// Получение запроса представлений протоколов
        /// </summary>
        /// <returns>Запрос представлений протоколов</returns>
        public virtual IQueryable<ViewProtocol> GetViewList()
        {
            var userManager = this.Container.Resolve<IGkhUserManager>();
            var serviceDocumentInspector = this.Container.Resolve<IDomainService<DocumentGjiInspector>>();
            var serviceViewProtocol = this.Container.Resolve<IDomainService<ViewProtocol>>();
            var zonalDomain = this.Container.Resolve<IDomainService<ZonalInspectionInspector>>();

            try
            {
                Operator thisOperator = userManager.GetActiveOperator();
                var municipalityList = userManager.GetMunicipalityIds();
                if (thisOperator?.Inspector == null)
                {
                    return null;
                }

                if (thisOperator?.Inspector.NotMemberPosition != null)
                {
                    if (thisOperator?.Inspector.NotMemberPosition.Name == "Администратор доходов")
                    {
                        var zonalInspSubIds = Container.Resolve<IDomainService<InspectorZonalInspSubscription>>().GetAll()
                        .Where(x => x.Inspector.Id == thisOperator.Inspector.Id)
                        .Select(x => x.ZonalInspection.Id)
                        .ToList();

                        if (zonalInspSubIds.Count() > 0)
                        {
                            return serviceViewProtocol.GetAll()
                                .WhereIf(municipalityList.Count > 0, x => x.MunicipalityId.HasValue && municipalityList.Contains(x.MunicipalityId.Value))
                                .Where(x => zonalInspSubIds.Contains(x.ZonalInspectionId));
                        }
                    }
                }

                var zonalId = zonalDomain.GetAll().FirstOrDefault(x => x.Inspector == thisOperator.Inspector).ZonalInspection?.Id;
                if (!zonalId.HasValue)
                {
                    return null;
                }
                return serviceViewProtocol.GetAll()
                        .WhereIf(municipalityList.Count > 0, x => x.MunicipalityId.HasValue && municipalityList.Contains(x.MunicipalityId.Value))
                        .Where(x => x.ZonalInspectionId == zonalId);
            }
            finally
            {
                this.Container.Release(zonalDomain);
                this.Container.Release(userManager);
                this.Container.Release(serviceDocumentInspector);
                this.Container.Release(serviceViewProtocol);
            }
        }
    }

    /// <summary>
    /// Вспомогательный прокси протокола
    /// </summary>
    public class ProtocolGetInfoProxy
    {
        /// <summary>
        /// Имена инспекторов
        /// </summary>
        public string InspectorNames { get; set; }

        /// <summary>
        /// Идентификаторы инспекторов
        /// </summary>
        public string InspectorIds { get; set; }

        /// <summary>
        /// Базовое имя
        /// </summary>
        public string BaseName { get; set; }

        public int? PassportNumber { get; set; }

        public int? PassportSeries {get; set;}

        public string PlaceResidence { get; set; }

        public string ActuallyResidence { get; set; }

        public DateTime? DataBirth { get; set; }

        public long NumberKUSP { get; set; }
        
    }
}