namespace Bars.GkhGji.Interceptors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Contracts;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class ActCheckServiceInterceptor : ActCheckServiceInterceptor<ActCheck>
    {
        // Внимание !!! override делать в Generic классе
    }
    
    public class ActCheckServiceInterceptor<T> : DocumentGjiInterceptor<T>
        where T : ActCheck
    {
        public override IDataResult BeforeUpdateAction(IDomainService<T> service, T entity)
        {
            var serviceNumRule = Container.Resolve<IDomainService<DocNumValidationRule>>();
            var domainDocumentRef = Container.Resolve<IDomainService<DocumentGjiReference>>();
            var domainDocumentGji = Container.Resolve<IDomainService<DocumentGjiChildren>>();
            var actRoDomain = Container.Resolve<IDomainService<ActCheckRealityObject>>();
            var roDomain = Container.Resolve<IDomainService<RealityObject>>();

            try
            {
                //проверяем чтоб дата акта была не меньше даты распоряжения по которому создан данный акт
                var parentDisposal = domainDocumentGji
                .GetAll()
                .Where(x => x.Children.Id == entity.Id)
                .Select(x => new { x.Parent.Id, x.Parent.DocumentDate })
                .ToList();

                if (parentDisposal.Any(item => item.DocumentDate.HasValue && entity.DocumentDate.HasValue && item.DocumentDate.Value > entity.DocumentDate.Value))
                {
                    return this.Failure(string.Format("Дата акта проверки должна быть больше или равна дате родительского {0}",
                                                    Container.Resolve<IDisposalText>().GenetiveCase.ToLower()));
                }

                // перед обновлением сохраняем связь с Постановлением прокуратуры

                if (entity.ResolutionProsecutor != null)
                {
                    var docResolutionProsecutor = domainDocumentRef.GetAll()
                        .FirstOrDefault(x => x.TypeReference == TypeDocumentReferenceGji.ActCheckToProsecutor
                            && (x.Document1.Id == entity.Id || x.Document2.Id == entity.Id));

                    if (docResolutionProsecutor == null)
                    {
                        // Если связи небыло, в обновляемом объекте есть документ прокуратуры, то создаем связб
                        var newRef = new DocumentGjiReference
                        {
                            TypeReference = TypeDocumentReferenceGji.ActCheckToProsecutor,
                            Document1 = entity,
                            Document2 = entity.ResolutionProsecutor
                        };

                        domainDocumentRef.Save(newRef);
                    }
                    else
                    {
                        // Если связь уже существует и также пришел документ в обновляемой сущности то перезаписываем
                        docResolutionProsecutor.Document1 = entity;
                        docResolutionProsecutor.Document2 = entity.ResolutionProsecutor;

                        domainDocumentRef.Update(docResolutionProsecutor);
                    }
                }

                // проверяем изменилась ли площадь по указанным домам

                var roIds =
                    actRoDomain.GetAll()
                               .Where(x => x.ActCheck.Id == entity.Id)
                               .Where(x => x.RealityObject != null)
                               .Select(x => x.RealityObject.Id)
                               .Distinct()
                               .ToList();

                decimal? area = 0m;
                
                if (roIds.Any())
                {
                    area = roDomain.GetAll().Where(x => roIds.Contains(x.Id)).Sum(x => x.AreaMkd);
                }

                if (entity.Area != area && area > 0)
                {
                    entity.Area = area;
                }

                return base.BeforeUpdateAction(service, entity);
            }
            finally
            {
                Container.Release(serviceNumRule);
                Container.Release(domainDocumentRef);
                Container.Release(domainDocumentGji);
                Container.Release(actRoDomain);
                Container.Release(roDomain);
            }
        }

        public override IDataResult BeforeDeleteAction(IDomainService<T> service, T entity)
        {
            var annexService = this.Container.Resolve<IDomainService<ActCheckAnnex>>();
            var definitionService = this.Container.Resolve<IDomainService<ActCheckDefinition>>();
            var partService = this.Container.Resolve<IDomainService<ActCheckInspectedPart>>();
            var witnessService = this.Container.Resolve<IDomainService<ActCheckWitness>>();
            var domainChildren = Container.Resolve<IDomainService<DocumentGjiChildren>>();
            var domainServiceActRemoval = Container.Resolve<IDomainService<ActRemoval>>();
            var domainServiceObject = Container.Resolve<IDomainService<ActCheckRealityObject>>();
            var actPeriodService = Container.Resolve<IDomainService<ActCheckPeriod>>();

            try
            {

                var result = base.BeforeDeleteAction(service, entity);

                if (!result.Success)
                {
                    return Failure(result.Message);
                }

                var refFuncs = new List<Func<long, string>>
                {
                    id => annexService.GetAll().Any(x => x.ActCheck.Id == id) ? "Приложения" : null,
                    id => definitionService.GetAll().Any(x => x.ActCheck.Id == id) ? "Определение" : null,
                    id => partService.GetAll().Any(x => x.ActCheck.Id == id) ? "Инспектируемые части" : null,
                    id => witnessService.GetAll().Any(x => x.ActCheck.Id == id) ? "Лица, присутсвующие в акте" : null
                };

                var refs = refFuncs.Select(x => x(entity.Id)).Where(x => x != null).ToArray();

                var message = string.Empty;

                if (refs.Length > 0)
                {
                    message = refs.Aggregate(message, (current, str) => current + string.Format(" {0}; ", str));
                    message = string.Format("Существуют связанные записи в следующих таблицах: {0}", message);
                    return Failure(message);
                }
                // удаляем дочерние акты
                var actRemovalIds =
                    domainChildren.GetAll()
                                                 .Where(x => x.Parent.Id == entity.Id && x.Children.TypeDocumentGji == TypeDocumentGji.ActRemoval)
                                                 .Select(x => x.Children.Id)
                                                 .ToArray();

                foreach (var actRemovalId in actRemovalIds)
                {
                    domainServiceActRemoval.Delete(actRemovalId);
                }

                // Удаляем все дочерние Дома акта
                var objectIds = domainServiceObject.GetAll()
                    .Where(x => x.ActCheck.Id == entity.Id)
                    .Select(x => x.Id)
                    .ToList();

                foreach (var objId in objectIds)
                {
                    domainServiceObject.Delete(objId);
                }

                //удаляем даты/время проведения проверки
                actPeriodService.GetAll()
                    .Where(x => x.ActCheck.Id == entity.Id)
                    .Select(x => x.Id)
                    .ForEach(x => actPeriodService.Delete(x));

                return result;
            }
            finally 
            {
                Container.Release(annexService);
                Container.Release(definitionService);
                Container.Release(partService);
                Container.Release(witnessService);
                Container.Release(domainChildren);
                Container.Release(domainServiceActRemoval);
                Container.Release(domainServiceObject);
                Container.Release(actPeriodService);
            }
            
        }
    }
}