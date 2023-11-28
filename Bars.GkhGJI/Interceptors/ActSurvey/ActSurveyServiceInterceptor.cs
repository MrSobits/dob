namespace Bars.GkhGji.Interceptors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;

    public class ActSurveyServiceInterceptor : ActSurveyServiceInterceptor<ActSurvey>
    {
    }

    public class ActSurveyServiceInterceptor<T> : DocumentGjiInterceptor<T>
        where T: ActSurvey
    {
        public override IDataResult BeforeDeleteAction(IDomainService<T> service, T entity)
        {
            var annexService = this.Container.Resolve<IDomainService<ActSurveyAnnex>>();
            var partService = this.Container.Resolve<IDomainService<ActSurveyInspectedPart>>();
            var ownerService = this.Container.Resolve<IDomainService<ActSurveyOwner>>();
            var photoService = this.Container.Resolve<IDomainService<ActSurveyPhoto>>();
            var domainServiceObject = Container.Resolve<IDomainService<ActSurveyRealityObject>>();

            // Перед удалением проверяем есть ли дочерние документы
            try
            {
                var result = base.BeforeDeleteAction(service, entity);

                if (!result.Success)
                {
                    return Failure(result.Message);
                }

                var refFuncs = new List<Func<long, string>>
                               {
                                  id => annexService.GetAll().Any(x => x.ActSurvey.Id == id) ? "Приложения" : null,
                                  id => partService.GetAll().Any(x => x.ActSurvey.Id == id) ? "Инспектируемые части" : null,
                                  id => ownerService.GetAll().Any(x => x.ActSurvey.Id == id) ? "Сведения о собсвенниках" : null,
                                  id => photoService.GetAll().Any(x => x.ActSurvey.Id == id) ? "Фото в акте обследования" : null
                               };

                var refs = refFuncs.Select(x => x(entity.Id)).Where(x => x != null).ToArray();

                var message = string.Empty;

                if (refs.Length > 0)
                {
                    message = refs.Aggregate(message, (current, str) => current + string.Format(" {0}; ", str));
                    message = string.Format("Существуют связанные записи в следующих таблицах: {0}", message);
                    return Failure(message);
                }

                // Удаляем всех дочерних Дома
                var objectIds = domainServiceObject.GetAll().Where(x => x.ActSurvey.Id == entity.Id)
                .Select(x => x.Id).ToList();

                foreach (var objectId in objectIds)
                {
                    domainServiceObject.Delete(objectId);
                }

                return result;
            }
            finally 
            {
                Container.Release(annexService);
                Container.Release(partService);
                Container.Release(ownerService);
                Container.Release(photoService);
                Container.Release(domainServiceObject);
            }
        }
    }
}