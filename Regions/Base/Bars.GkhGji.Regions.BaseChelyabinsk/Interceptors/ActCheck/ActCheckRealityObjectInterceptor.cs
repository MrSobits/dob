namespace Bars.GkhGji.Regions.BaseChelyabinsk.Interceptors.ActCheck
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.ActCheck;

    public class ActCheckRealityObjectInterceptor : EmptyDomainInterceptor<ActCheckRealityObject>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<ActCheckRealityObject> service, ActCheckRealityObject entity)
        {
            var domainLongText = this.Container.Resolve<IDomainService<ActCheckRoLongDescription>>();
            try
            {
                // Удаляем все дочерние Нарушения акта
                var ids = domainLongText.GetAll()
                    .Where(x => x.ActCheckRo.Id == entity.Id)
                    .Select(x => x.Id)
                    .ToList();

                foreach (var id in ids)
                {
                    domainLongText.Delete(id);
                }
            }
            finally
            {
                this.Container.Release(domainLongText);
            }
            return base.BeforeDeleteAction(service, entity);
        }
    }
}