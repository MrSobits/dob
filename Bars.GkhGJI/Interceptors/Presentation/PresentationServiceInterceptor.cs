namespace Bars.GkhGji.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    public class PresentationServiceInterceptor : PresentationServiceInterceptor<Presentation>
    {
    }
    
    public class PresentationServiceInterceptor<T> : DocumentGjiInterceptor<T>
        where T : Presentation
    {
        public override IDataResult BeforeDeleteAction(IDomainService<T> service, T entity)
        {
            var annexService = Container.Resolve<IDomainService<PresentationAnnex>>();

            try
            {
                annexService.GetAll().Where(x => x.Presentation.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => annexService.Delete(x));

                return base.BeforeDeleteAction(service, entity);
            }
            finally
            {
                Container.Release(annexService);
            }
        }
    }
}
