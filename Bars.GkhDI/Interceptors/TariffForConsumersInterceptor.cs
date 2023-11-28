namespace Bars.GkhDi.Interceptors
{
    using System.Linq;
    using Bars.B4;
    using Bars.GkhDi.Entities;

    public class TariffForConsumersInterceptor : EmptyDomainInterceptor<TariffForConsumers>
    {
        public override IDataResult AfterCreateAction(IDomainService<TariffForConsumers> service, TariffForConsumers entity)
        {
            if ((entity.DateStart.HasValue && entity.DateStart.Value >= entity.BaseService.DateStartTariff) 
                || !entity.BaseService.DateStartTariff.HasValue)
            {
                entity.BaseService.TariffForConsumers = entity.Cost;
                entity.BaseService.TariffIsSetForDi = entity.TariffIsSetFor;
                entity.BaseService.DateStartTariff = entity.DateStart;
            }

            var baseServService = this.Container.Resolve<IDomainService<BaseService>>();
            baseServService.Update(entity.BaseService);

            return this.Success();
        }

        public override IDataResult AfterUpdateAction(IDomainService<TariffForConsumers> service, TariffForConsumers entity)
        {
            return this.Calculatetariff(service, entity);
        }

        public override IDataResult AfterDeleteAction(IDomainService<TariffForConsumers> service, TariffForConsumers entity)
        {
            return this.Calculatetariff(service, entity);
        }

        private IDataResult Calculatetariff(IDomainService<TariffForConsumers> service, TariffForConsumers entity)
        {
            var tariffForConsumers = service.GetAll().Where(x => x.BaseService.Id == entity.BaseService.Id).OrderByDescending(x => x.DateStart).ThenByDescending(x => x.ObjectEditDate).FirstOrDefault();

            if (tariffForConsumers != null)
            {
                entity.BaseService.TariffForConsumers = tariffForConsumers.Cost;
                entity.BaseService.TariffIsSetForDi = tariffForConsumers.TariffIsSetFor;
                entity.BaseService.DateStartTariff = tariffForConsumers.DateStart;

                var baseServService = this.Container.Resolve<IDomainService<BaseService>>();
                baseServService.Update(entity.BaseService);
            }

            return this.Success();
        }
    }
}
