namespace Bars.GkhGji.Regions.Smolensk.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.GkhGji.Interceptors;
    using Bars.GkhGji.Regions.Smolensk.Entities;

    public class DisposalInterceptor : DisposalServiceInterceptor<DisposalSmol>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<DisposalSmol> service, DisposalSmol entity)
        {
            var disposalControlMeasuresDomain = this.Container.Resolve<IDomainService<DisposalControlMeasures>>();

            using (this.Container.Using(disposalControlMeasuresDomain))
            {
                var list = disposalControlMeasuresDomain.GetAll()
                    .Where(x => x.Disposal.Id == entity.Id)
                    .Select(x => x.Id).ToList();

                list.ForEach(x => disposalControlMeasuresDomain.Delete(x));
            }

            return base.BeforeDeleteAction(service, entity);
        }
    }
}