namespace Bars.GkhGji.DomainService
{
    using System.Collections.Generic;
    using System.Linq;

    using B4;
    using B4.Utils;

    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Enums;

    using Entities;

    using Castle.Windsor;
    using Bars.GkhGji.Entities.Dict;
    using Bars.B4.Modules.Analytics.Reports.Extensions;
    using Bars.Gkh.Enums;

    public class DisposalViolService : IDisposalViolService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<DisposalViolation> ViolDomain { get; set; }

        public IDomainService<Disposal> DisposalDomain { get; set; }

        public IDomainService<ViolationGji> DictViolationDomain { get; set; }

        public IDomainService<InspectionGjiViol> InspectionViolDomain { get; set; }

        public virtual IDataResult ListRealityObject(BaseParams baseParams)
        {
            /*
             * Параметры могут входить следующие6
             * 1. documentId - идентификатор приказа
             * Тут мы по id приказа группируем нарушения по Домума и выводим
             * Адрес Дома и напротив Количество нарушений
             */

            var service = Container.Resolve<IDomainService<DisposalViolation>>();

            try
            {
                var loadParam = baseParams.GetLoadParam();

                var documentId = baseParams.Params.GetAs<long>("documentId");

                var list = service.GetAll()
                    .Where(x => x.Document.Id == documentId)
                    .Select(x => new
                        {
                            x.Id,
                            Municipality = x.InspectionViolation.RealityObject != null ? x.InspectionViolation.RealityObject.Municipality.Name : "",
                            RealityObject = x.InspectionViolation.RealityObject != null ? x.InspectionViolation.RealityObject.Address : "",
                            RealityObjectId = x.InspectionViolation.RealityObject != null ? x.InspectionViolation.RealityObject.Id : 0
                        })
                    .OrderIf(loadParam.Order.Length == 0, true, x => x.Municipality)
                    .OrderThenIf(loadParam.Order.Length == 0, true, x => x.RealityObject)
                    .Filter(loadParam, Container)
                    .ToList();

                var data = list
                    .GroupBy(x => new { x.RealityObjectId, x.Municipality, x.RealityObject })
                    .Select(x => new
                        {
                            Id = x.Key.RealityObjectId,
                            ViolationCount = x.Count(),
                            x.Key.Municipality,
                            RealityObject = x.Key.RealityObject == "" ? "Дом отсутствует" : x.Key.RealityObject
                        })
                    .AsQueryable();

                int totalCount = data.Count();

                return new ListDataResult(
                    data.Order(loadParam).ToList(), totalCount);
            }
            finally 
            {
                Container.Release(service);
            }
           
        }



        public virtual IDataResult ListViolationforMunicipality(BaseParams baseParams) 
        {
            var loadParam = baseParams.GetLoadParam();
            var domainService = Container.ResolveDomain<ViolationGji>();
            var violFeatureDomain = Container.ResolveDomain<ViolationFeatureGji>();
            var violActionsRemovDomain = Container.ResolveDomain<ViolationActionsRemovGji>();
            var violNormativeDocItemDomain = Container.ResolveDomain<ViolationNormativeDocItemGji>();
            var violGjiMun = Container.ResolveDomain<ViolationGjiMunicipality>();

            var municipalityId = baseParams.Params.GetAs<long>("municipalityId");

            try
            {
                var featDict = violFeatureDomain
                    .GetAll()
                    .Select(x => new
                    {
                        x.ViolationGji.Id,
                        Name = x.FeatureViolGji.FullName ?? x.FeatureViolGji.Name
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => y.Select(x => x.Name).AggregateWithSeparator(", "));

                var actRemViolDict = violActionsRemovDomain
                    .GetAll()
                    .Select(x => new
                    {
                        x.ViolationGji.Id,
                        x.ActionsRemovViol.Name
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => y.Select(x => x.Name).AggregateWithSeparator(", "));

                var violGjiId = violGjiMun.GetAll().Where(x => x.Municipality.Id == municipalityId).Select(x => x.ViolationGji.Id).ToList();

                        var data =
                        domainService.GetAll()
                    //    .Where(x => x.TypeMunicipality == TypeMunicipality.InterCityArea || (x.TypeMunicipality != TypeMunicipality.InterCityArea && violGjiId.Contains(x.Id)))
                            .Select(x => new
                            {
                                x.Id,
                                x.Name,
                                x.PpRf25,
                                x.PpRf307,
                                x.PpRf491,
                                x.CodePin,
                                x.PpRf170,
                                x.OtherNormativeDocs,
                                x.NormativeDocNames,
                                x.Municipality,
                                x.TypeMunicipality
                            })
                            .AsEnumerable()
                            .Select(
                                x =>
                                    new
                                    {
                                        x.Id,
                                        Name = x.Name ?? string.Empty,
                                        PpRf25 = x.PpRf25 ?? string.Empty,
                                        PpRf307 = x.PpRf307 ?? string.Empty,
                                        PpRf491 = x.PpRf491 ?? string.Empty,
                                        CodePin = x.CodePin ?? string.Empty,
                                        FeatViol = featDict.Get(x.Id) ?? string.Empty,
                                        ActRemViol = actRemViolDict.Get(x.Id) ?? string.Empty,
                                        NormDocNum = x.NormativeDocNames ?? string.Empty,
                                        Municipality = x.Municipality,
                                        TypeMunicipality = x.TypeMunicipality
                                    })
                            .AsQueryable()
                            .Filter(loadParam, Container);

                        var totalCount = data.Count();

                return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
            }
            finally
            {
                Container.Release(violFeatureDomain);
                Container.Release(violActionsRemovDomain);
                Container.Release(violNormativeDocItemDomain);
            }
        }


        public virtual IDataResult AddViolations(BaseParams baseParams)
        {
            var documentId = baseParams.Params.GetAs<long>("documentId");
            var realObjId = baseParams.Params.GetAs<long>("realObjId");
            var violIds = baseParams.Params.GetAs<long[]>("violIds");
            var insViolIds = baseParams.Params.GetAs<long[]>("insViolIds");

            // в этом списке будут id нарушений, которые уже связаны с этим предписанием
            // (чтобы недобавлять несколько одинаковых документов в одно и тоже предписание)
            var listIds =
                ViolDomain.GetAll()
                          .Where(x => x.Document.Id == documentId)
                          .Where(x => violIds.Contains(x.InspectionViolation.Violation.Id))
                          .Select(
                              x =>
                              new
                                  {
                                      x.InspectionViolation.Violation.Id,
                                      roId = x.InspectionViolation != null && x.InspectionViolation.RealityObject != null ? x.InspectionViolation.RealityObject.Id : 0
                                  })
                          .ToList()
                          .GroupBy(x => x.Id + "_" + x.roId)
                          .ToDictionary(x => x.Key, y => y.FirstOrDefault());

            var disposal = this.DisposalDomain.GetAll().FirstOrDefault(x => x.Id == documentId);

            var listInspectionViols = new List<InspectionGjiViol>();
            var listDisposalViols = new List<DisposalViolation>();

            var dictInsViols = InspectionViolDomain.GetAll()
                                    .Where(x => insViolIds.Contains(x.Id))
                                    .Select(x => new { x.Id, violId = x.Violation.Id, roId = x.RealityObject.Id })
                                    .ToList()
                                    .GroupBy(x => x.violId)
                                    .ToDictionary(x => x.Key, y => y.ToList());
            if (dictInsViols.Any())
            {
                violIds = dictInsViols.Keys.ToArray();
            }
            
            // получаю словарь на случай если будет выбрано мног онарушений будет работат ьбыстрее чем получение по отдельности Нарушения
            var dictViolations = DictViolationDomain.GetAll()
                                .Where(x => violIds.Contains(x.Id))
                                .Select(x => new { x.Id, x.Name })
                                .ToList()
                                .GroupBy(x => x.Id)
                                .ToDictionary(x => x.Key, y => y.FirstOrDefault());

            foreach (var id in violIds)
            {
                // если нет в словаре то тоже выходим
                if (!dictViolations.ContainsKey(id))
                {
                    continue;
                }
                
                if (insViolIds.Any())
                {
                    // В этом случае имеем дело с нарушениями котоыревыбраны для приказа
                    // из родительского документа, а значит бежим по каждому нарушению и создаем для него комбинацию
                    if (!dictInsViols.ContainsKey(id))
                    {
                        continue;
                    }

                    foreach (var insViol in dictInsViols[id])
                    {
                        // В этом случае имеем дело с нарушениями котоыре выбраны просто из справочника
                        // Если для дома такая комбинация уже есть то тогда пролетаем мимо 
                        if (listIds.ContainsKey(id+ "_" + insViol.roId))
                        {
                            continue;
                        }

                        var viol = dictViolations[id];

                        var inspViol = new InspectionGjiViol
                        {
                            Inspection = disposal.Inspection,
                            RealityObject = insViol.roId > 0 ? new RealityObject { Id = insViol.roId } : null,
                            Violation = new ViolationGji { Id = id },
                            Description = viol.Name
                        };

                        listInspectionViols.Add(inspViol);

                        var disposalViol = new DisposalViolation
                        {
                            Document = disposal,
                            InspectionViolation = inspViol,
                            TypeViolationStage = TypeViolationStage.Detection
                        };

                        listDisposalViols.Add(disposalViol); 
                    }
                }
                else
                {
                    // В этом случае имеем дело с нарушениями котоыре выбраны просто из справочника
                    // Если для дома такая комбинация уже есть то тогда пролетаем мимо 
                    if ( listIds.ContainsKey(id+"_"+realObjId))
                    {
                        continue;
                    }

                    var viol = dictViolations[id];

                    var inspViol = new InspectionGjiViol
                    {
                        Inspection = disposal.Inspection,
                        RealityObject = realObjId > 0 ? new RealityObject { Id = realObjId } : null,
                        Violation = new ViolationGji { Id = id },
                        Description = viol.Name
                    };

                    listInspectionViols.Add(inspViol);

                    var disposalViol = new DisposalViolation
                    {
                        Document = disposal,
                        InspectionViolation = inspViol,
                        TypeViolationStage = TypeViolationStage.Detection
                    };

                    listDisposalViols.Add(disposalViol);    
                }
            }

            using (var transaction = Container.Resolve<IDataTransaction>())
            {

                try
                {
                    listInspectionViols.ForEach(x => InspectionViolDomain.Save(x));

                    listDisposalViols.ForEach(x => ViolDomain.Save(x));

                    transaction.Commit();

                    return new BaseDataResult();
                }
                catch (ValidationException e)
                {
                    transaction.Rollback();
                    return new BaseDataResult(new { success = false, message = e.Message });
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }

            }
        }
    }
}