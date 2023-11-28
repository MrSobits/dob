namespace Bars.GkhGji.Regions.Tatarstan.Interceptors
{
    using Bars.B4;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Interceptors;

    public class ActCheckServiceInterceptor : ActCheckServiceInterceptor<ActCheck>
    {
        public override IDataResult BeforeCreateAction(IDomainService<ActCheck> service, ActCheck entity)
        {
            Utils.SaveFiasAddress(this.Container, entity.DocumentPlaceFias);

            return base.BeforeCreateAction(service, entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<ActCheck> service, ActCheck entity)
        {
            Utils.SaveFiasAddress(this.Container, entity.DocumentPlaceFias);

            return base.BeforeUpdateAction(service, entity);
        }
    }
}