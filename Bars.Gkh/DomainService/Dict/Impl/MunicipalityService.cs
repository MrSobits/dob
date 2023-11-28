namespace Bars.Gkh.DomainService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FIAS;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Config;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Extensions;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Сервис "Муниципальный район и Муниципальный образование"
    /// </summary>
    public class MunicipalityService : IMunicipalityService
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Домен сервис "Муниципальное образование
        /// </summary>
        public IDomainService<Municipality> DomainService { get; set; }

        /// <summary>
        /// Репозиторий "Муниципальное образование
        /// </summary>
        public IRepository<Municipality> Repository { get; set; }

        /// <summary>
        /// Установить родителский МО для указанного
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        public IDataResult SetMoParent(BaseParams baseParams)
        {
            var moId = baseParams.Params.GetAs<string>("moId").ToInt();

            var parentMoId = baseParams.Params.GetAs<string>("parentMoId").ToInt();

            var mo = this.DomainService.Get(moId);

            var parentMo = this.DomainService.Get(parentMoId);

            if (mo != null)
            {
                mo.ParentMo = parentMo;

                this.DomainService.Update(mo);
            }
            else
            {
                return new BaseDataResult(false, "Не найдено муниципального образования с таким id");
            }

            return new BaseDataResult();
        }

        /// <summary>
        /// Вернуть МО без пагинации
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        public IDataResult ListWithoutPaging(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var userManager = this.Container.Resolve<IGkhUserManager>();
            
            try
            {
                var municipalityIds = userManager.GetMunicipalityIds();
                
                var parents = this.GetAll()
                    .Where(x => x.ParentMo != null)
                    .Select(x => x.ParentMo.Id)
                    .Distinct()
                    .ToArray();

                var data = this.GetAll()
                    .WhereIf(municipalityIds.Count > 0, x => municipalityIds.Contains(x.Id))
                    .Where(x => x.ParentMo == null)
                    // .Where(x => !parents.Contains(x.Id))
                    .Select(x => new {x.Id, x.Name})
                    .OrderBy(x => x.Name)
                    .Filter(loadParams, this.Container);

                return new ListDataResult(data.Order(loadParams).ToList(), data.Count());
            }

            finally
            {
                this.Container.Release(userManager);
            }
        }

        /// <summary>
        /// Список по идентификаторам
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        public IDataResult ListByReg(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var munIds = baseParams.Params.GetAs("munIds", string.Empty);

            var munIdsList = !string.IsNullOrEmpty(munIds) ? munIds.Split(',').Select(id => id.ToLong()).ToArray() : new long[0];

            var data = this.DomainService.GetAll()
                .Where(x => munIdsList.Contains(x.Id))
                .Select(x => new
                {
                    x.Id,
                    x.Name
                })
                .Filter(loadParams, this.Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

        /// <summary>
        /// Возвращает список МО в зависимости от оператора
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Список МО</returns>
        public IDataResult ListByOperator(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var userManager = this.Container.Resolve<IGkhUserManager>();

            var municipalityIds = userManager.GetMunicipalityIds();

            var data = this.DomainService.GetAll()
                .WhereIf(municipalityIds.Count > 0, x => municipalityIds.Contains(x.Id))
                .Select(x => new {x.Id, x.Name})
                .Filter(loadParams, this.Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

        /// <summary>
        /// Добавить МО по номеру ФИАС
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        public IDataResult AddMoFromFias(BaseParams baseParams)
        {
            var fiasIds = baseParams.Params.GetAs<string>("fiasIds");

            var ids = !string.IsNullOrEmpty(fiasIds) ? fiasIds.Split(',').Select(id => id.ToLong()).ToArray() : new long[0];

            if (!string.IsNullOrEmpty(fiasIds))
            {
                var fiasService = this.Container.Resolve<IDomainService<Fias>>();

                foreach (var fiasId in ids)
                {
                    var fiasObj = fiasService.Load(fiasId);

                    var moExists = this.DomainService.GetAll().Any(x => x.FiasId == fiasObj.AOGuid || x.Okato == fiasObj.OKATO);
                    Municipality parentMO = null;
                    if (!string.IsNullOrEmpty(fiasObj.ParentGuid))
                    {
                        parentMO = this.DomainService.GetAll()
                            .FirstOrDefault(x => x.FiasId == fiasObj.ParentGuid);
                    }

                    if (fiasObj != null && !moExists)
                    {
                        var mo = new Municipality
                        {
                            FiasId = fiasObj.AOGuid,
                            Okato = fiasObj.OKATO,
                            Oktmo = fiasObj.OKTMO,
                            ParentMo = parentMO != null? parentMO:null,
                            Name = fiasObj.OffName + " " + fiasObj.ShortName
                        };

                        this.DomainService.Save(mo);
                    }
                }
            }

            return new BaseDataResult();
        }

        /// <summary>
        /// Вернуть дерево МО
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        public IDataResult ListTree(BaseParams baseParams)
        {
            var name = baseParams.Params.GetAs<string>("name");

            var parentMoId = baseParams.Params.GetAs<long>("parentMoId");

            var data = this.GetAll().ToList();

            var child =
                data.Where(x => x.ParentMo != null && (string.IsNullOrEmpty(name) || x.Name.ToUpper().Contains(name.ToUpper()))).ToList();

            var parent = data
                .Where(
                    x => (parentMoId == 0 || x.Id == parentMoId) &&
                        x.ParentMo == null && (string.IsNullOrEmpty(name) ||
                            x.Name.ToUpper().Contains(name.ToUpper()) ||
                            child.Any(y => y.ParentMo.Id == x.Id)))
                .OrderBy(x => x.Name).ToList();

            var tree = this.ConvertDictToTree(parent, child);

            return new BaseDataResult(tree["children"]);
        }

        /// <summary>
        /// Вернуть дерево по поисковому запросу
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        public IDataResult ListSelectTree(BaseParams baseParams)
        {
            var searchString = baseParams.Params.Get("search", string.Empty).ToLower();
            var data = this.GetAll().ToList();

            var parent = data
                .Where(x => x.ParentMo == null && x.Name.ToLower().Contains(searchString)).OrderBy(x => x.Name).ToList();
            var child = data.Where(x => x.ParentMo != null && x.Name.ToLower().Contains(searchString)).ToList();

            var tree = this.ConvertDictToTree(parent, child, true);

            return new BaseDataResult(tree["children"]);
        }

        /// <summary>
        /// Вернуть МО в соответствии с настройками приложения
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        public IDataResult ListByParam(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var appParams = this.Container.Resolve<IGkhParams>().GetParams();

            var moLevel = appParams.ContainsKey("MoLevel") && !string.IsNullOrEmpty(appParams["MoLevel"].To<string>())
                ? appParams["MoLevel"].To<MoLevel>()
                : MoLevel.MunicipalUnion;

            var data = this.Container.Resolve<IRepository<Municipality>>().GetAll()
                .OrderBy(x => x.Name)
                .Select(x => new
                {
                    x.Id,
                    x.Level,
                    x.Name
                })
                .Filter(loadParams, this.Container)
                /*  .AsEnumerable()
                 * 
                 * Так делать нельзя, asEnumerable делает отложенный запрос к бд, т.е. при вызове ToList и т.п.
                 * он каждый раз будет делать запрос с условиями до asEnumerable и потом уже работать с коллекцией в памяти;
                 * из-за этого при вызове и data.ToList() и data.Count() он сделает по запросу
                 * при чем data.Count() он посчитает корректно, а data.ToList() он сделает без условия Where после asEnumerable
                */
                .ToArray()
                .Where(x => x.Level.ToMoLevel(this.Container) == moLevel);

            return new ListDataResult(data.ToList(), data.Count());
        }

        /// <summary>
        /// Вернуть МО в соответствии с настройками приложения (с пагинацией)
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        public IDataResult ListByParamPaging(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var appParams = this.Container.Resolve<IGkhParams>().GetParams();

            var moLevel = appParams.ContainsKey("MoLevel") && !string.IsNullOrEmpty(appParams["MoLevel"].To<string>())
                ? appParams["MoLevel"].To<MoLevel>()
                : MoLevel.MunicipalUnion;

            var data = this.Container.Resolve<IRepository<Municipality>>().GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.Level,
                    x.Name
                })
                .Filter(loadParams, this.Container)
                .ToArray()
                .Where(x => x.Level.ToMoLevel(this.Container) == moLevel)
                .AsQueryable();

            return new ListDataResult(
                data.Order(loadParams).OrderIf(loadParams.Order.Length == 0, true, x => x.Name).Paging(loadParams).ToList(),
                data.Count());
        }

        /// <summary>
        /// Вернуть список МР по настройкам
        /// </summary>
        /// <returns>Результат операции</returns>
        public IQueryable<MunicipalityProxy> ListByParamAndOperatorQueryable()
        {
            return this.ListByParamAndOperatorInternal();
        }

        /// <summary>
        /// Вернуть по уровню МО
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        public IDataResult ListByWorkpriceParam(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var appParams = this.Container.Resolve<IGkhParams>().GetParams();

            var workPriceMoLevel = appParams.ContainsKey("WorkPriceMoLevel") && !string.IsNullOrEmpty(appParams["WorkPriceMoLevel"].To<string>())
                ? appParams["WorkPriceMoLevel"].To<WorkpriceMoLevel>()
                : WorkpriceMoLevel.MunicipalUnion;

            var data = this.Container.Resolve<IRepository<Municipality>>().GetAll()
                .OrderBy(x => x.Name)
                .WhereIf(
                    workPriceMoLevel == WorkpriceMoLevel.MunicipalUnion,
                    x => x.Level == TypeMunicipality.MunicipalArea)
                .WhereIf(
                    workPriceMoLevel == WorkpriceMoLevel.Settlement,
                    x => x.Level != TypeMunicipality.MunicipalArea && x.Level != TypeMunicipality.UrbanArea)
                .Select(x => new
                {
                    x.Id,
                    x.Level,
                    x.Name
                })
                .Filter(loadParams, this.Container);
                
            return new ListDataResult(data.ToList(), data.Count());
        }

        /// <summary>
        /// Вернуть Муниципальные районы
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        public IDataResult ListMoArea(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var data = this.GetAll().Where(x => x.ParentMo == null || x.Level == TypeMunicipality.MunicipalArea)
                .Select(x => new
                {
                    x.Id,
                    x.Code,
                    x.FiasId,
                    x.Group,
                    x.Name,
                    x.Okato,
                    x.Oktmo,
                    x.Description,
                    x.FederalNumber,
                    x.Cut,
                    HasChildren = this.GetAll().Any(y => y.ParentMo.Id == x.Id)
                })
                .Filter(loadParams, this.Container)
                .OrderIf(loadParams.Order.Length == 0, true, x => x.Name);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

        /// <summary>
        /// Вернуть Муниципальные районы (без пагинации)
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        public IDataResult ListMoAreaWithoutPaging(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var data = this.GetAll().Where(x => x.ParentMo == null || x.Level == TypeMunicipality.MunicipalArea)
                .Select(x => new
                {
                    x.Id,
                    x.Code,
                    x.FiasId,
                    x.Group,
                    x.Name,
                    x.Okato,
                    x.Oktmo,
                    x.Description,
                    x.FederalNumber,
                    x.Cut
                })
                .Filter(loadParams, this.Container)
                .OrderIf(loadParams.Order.Length == 0, true, x => x.Name);

            return new ListDataResult(data.Order(loadParams).ToList(), data.Count());
        }

        /// <summary>
        /// Вернуть городские округи и муниципальные образования
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        public IDataResult ListSettlementAndUrban(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var settlIds = baseParams.Params.GetAs("Id", string.Empty).ToLongArray();
            var muId = loadParams.Filter.GetAs<long?>("muId");
            var moIds = baseParams.Params.GetAs("moIds", string.Empty).ToLongArray();
            var data = this.GetAll()
                .Where(x => x.Level != TypeMunicipality.MunicipalArea)
                .WhereIf(moIds.Any(), x => moIds.Contains(x.ParentMo.Id) || moIds.Contains(x.Id))
                .WhereIf(settlIds.Any() && settlIds.First() != 0, x => settlIds.Contains(x.Id))
                .WhereIf(muId.HasValue, x => x.ParentMo.Id == muId)
                .Select(x => new
                {
                    x.Id,
                    x.Code,
                    x.FiasId,
                    x.Group,
                    x.Name,
                    x.Okato,
                    x.Oktmo,
                    x.Description,
                    x.FederalNumber,
                    x.Level,
                    x.Cut
                })
                .Filter(loadParams, this.Container)
                .OrderIf(loadParams.Order.Length == 0, true, x => x.Name);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

        /// <summary>
        /// Вернуть муниципальные образования
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        public IDataResult ListSettlement(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var settlIds = baseParams.Params.GetAs("Id", string.Empty).ToLongArray();
            var muId = loadParams.Filter.GetAs<long?>("muId");
            var data = this.GetAll()
                .Where(x => x.ParentMo != null)
                .Where(x => x.Level != TypeMunicipality.MunicipalArea)
                .Where(x => x.Level != TypeMunicipality.UrbanArea)
                .WhereIf(settlIds.Any() && settlIds.First() != 0, x => settlIds.Contains(x.Id))
                .WhereIf(muId.HasValue, x => x.ParentMo.Id == muId)
                .Select(x => new
                {
                    x.Id,
                    x.Code,
                    x.FiasId,
                    x.Group,
                    x.Name,
                    x.Okato,
                    x.Oktmo,
                    x.Description,
                    x.FederalNumber,
                    parentMoId = x.ParentMo.Id,
                    x.Cut
                })
                .Filter(loadParams, this.Container)
                .OrderIf(loadParams.Order.Length == 0, true, x => x.Name);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

        /// <summary>
        /// Вернуть муниципальные образования с названием родительского мун. района и гор. округи
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        public IDataResult ListSettlementWithParentMo(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var parents = this.GetAll()
                .Where(x => x.ParentMo != null)
               .Select(x => x.ParentMo.Id)
               .Distinct();
            
            var data = this.GetAll()
             //   .Where(x => !parents.Contains(x.Id))
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    ParentMo = parents.Contains(x.Id)? x.Name: x.ParentMo.Name,
                })
                .Filter(loadParams, this.Container)
                .OrderIf(loadParams.Order.Length == 0, true, x => x.ParentMo);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

        /// <summary>
        /// Вернуть муниципальные образования с названием родительского мун. района и гор. округи
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        public IDataResult ListAllWithParent(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var data = this.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Oktmo,
                    x.Level,
                    ParentMo = x.ParentMo != null? x.ParentMo.Name: x.Name,
                })
                .Filter(loadParams, this.Container)
                .OrderIf(loadParams.Order.Length == 0, true, x => x.ParentMo);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }


        /// <summary>
        /// Вернуть муниципальные образования (без пагинации)
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        public IDataResult ListSettlementWithoutPaging(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var data = this.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Level
                })
                .AsEnumerable()
                .Where(x => x.Level.ToRealEstTypeMoLevel(this.Container) == MoLevel.Settlement)
                .AsQueryable()
                .Filter(loadParams, this.Container);

            return new ListDataResult(data.OrderIfHasParams(loadParams, true, x => x.Name).ToList(), data.Count());
        }

        /// <summary>
        /// Вернуть список МР по настройкам
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        public IDataResult ListByParamAndOperator(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var data = this.ListByParamAndOperatorInternal()
                .Filter(loadParams, this.Container)
                .Order(loadParams)
                .OrderIf(loadParams.Order.Length == 0, true, x => x.ParentMo)
                .OrderThenIf(loadParams.Order.Length == 0, true, x => x.Name);

            var count = data.Count();

            return new ListDataResult(data.Paging(loadParams).ToList(), count);
        }

        private IQueryable<MunicipalityProxy> ListByParamAndOperatorInternal()
        {
            var appParams = this.Container.Resolve<IGkhParams>().GetParams();

            var hasMoParam = appParams.ContainsKey("MoLevel") && !string.IsNullOrEmpty(appParams["MoLevel"].To<string>());

            var moLevel = hasMoParam
                ? appParams["MoLevel"].To<MoLevel>()
                : MoLevel.MunicipalUnion;

            var userManager = this.Container.Resolve<IGkhUserManager>();

            var municipalityIds = userManager.GetMunicipalityIds();

            var query = this.Container.Resolve<IRepository<Municipality>>().GetAll()
                .WhereIf(municipalityIds.Count > 0, x => municipalityIds.Contains(x.Id))
                .WhereIf(hasMoParam == false, x => x.ParentMo == null || x.Level == TypeMunicipality.MunicipalArea)
                .Select(
                    x => new MunicipalityProxy
                    {
                        Id = x.Id,
                        Level = x.Level,
                        Name = x.Name,
                        ParentMo = x.ParentMo.Name ?? x.Name
                    })
                .AsEnumerable()
                .Where(x => !hasMoParam || x.Level.ToMoLevel(this.Container) == moLevel)
                .AsQueryable();

            return query;
        }

        /// <summary>
        /// конвертация словаря в дерево
        /// </summary>
        private JObject ConvertDictToTree(IEnumerable<Municipality> parents, List<Municipality> child, bool withCheckbox = false)
        {
            var root = new JObject();

            var groups = new JArray();

            foreach (var pair in parents)
            {
                var group = new JObject();

                if (pair.Name != null)
                {
                    var id = pair.Id;

                    group["id"] = pair.Id;
                    group["Name"] = pair.Name;
                    group["text"] = pair.Name;
                    group["Code"] = pair.Code;
                    group["Oktmo"] = pair.Oktmo;
                    group["Okato"] = pair.Okato;
                    group["ParentMoId"] = pair.ParentMo.ReturnSafe(x => x.Id);
                    group["FederalNumber"] = pair.FederalNumber;
                    group["Group"] = pair.Group;
                    group["Level"] = ((int) pair.Level);

                    if (withCheckbox)
                    {
                        group["checked"] = false;
                    }

                    var children = new JArray();

                    var hisChild = child.Where(x => x.ParentMo.Id == id).OrderBy(x => x.Name).ToList();

                    foreach (var rec in hisChild)
                    {
                        var leaf = new JObject();

                        leaf["id"] = rec.Id;
                        leaf["Name"] = rec.Name;
                        leaf["text"] = rec.Name;
                        leaf["leaf"] = true;
                        leaf["Code"] = rec.Code;
                        leaf["Oktmo"] = rec.Oktmo;
                        leaf["Okato"] = rec.Okato;
                        leaf["ParentMoId"] = pair.ParentMo.ReturnSafe(x => x.Id);
                        leaf["FederalNumber"] = rec.FederalNumber;
                        leaf["Group"] = rec.Group;
                        leaf["Level"] = ((int) rec.Level);

                        if (withCheckbox)
                        {
                            leaf["checked"] = false;
                        }

                        children.Add(leaf);
                    }

                    if (hisChild.Any())
                    {
                        group["children"] = children;
                    }
                    else
                    {
                        group["leaf"] = true;
                    }

                    groups.Add(group);
                }
            }

            root["children"] = groups;

            return root;
        }

        /// <summary>
        /// Времмено, т.к. переопределен GetAll
        /// </summary>
        /// <returns>IQueryable для муниципальных образований</returns>
        public IQueryable<Municipality> GetAll()
        {
            var query = this.Repository.GetAll();
            var interceptors = this.Container.ResolveAll<IDomainServiceReadInterceptor<Municipality>>();

            return interceptors.Aggregate(query, (current, interceptor) => interceptor.BeforeGetAll(current));
        }
    }
}