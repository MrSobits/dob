namespace Bars.GkhGji.Regions.BaseChelyabinsk.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Interceptors;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol197;

    public class ResolutionServiceInterceptor: ResolutionServiceInterceptor<Resolution>
    {
    }

    public class ResolutionServiceInterceptor<T> : DocumentGjiInterceptor<T>
        where T : Resolution
    {
        public override IDataResult BeforeUpdateAction(IDomainService<T> service, T entity)
        {
            
            var payService = Container.Resolve<IDomainService<ResolutionPayFine>>();
            var documentgji = Container.Resolve<IDomainService<DocumentGjiChildren>>();
            var protocolservice = Container.Resolve<IDomainService<Bars.GkhGji.Entities.Protocol>>();
            var protocol197service = Container.Resolve<IDomainService<Protocol197>>();
            var disposalservice = Container.Resolve<IDomainService<Bars.GkhGji.Entities.Disposal>>();

            try
            {
                var parentDoc = documentgji.GetAll().FirstOrDefault(x => x.Children == entity)?.Parent;
                if (parentDoc != null)
                {
                    IndividualPerson ipers = protocol197service.Get(parentDoc.Id)?.IndividualPerson;
                    if (ipers == null)
                    {
                        ipers = protocolservice.Get(parentDoc.Id)?.IndividualPerson;
                    }
                    if (ipers != null)
                    {
                        entity.IndividualPerson = ipers;
                    }
                }



                // Если во вкладке "Оплаты штрафов" поле "Итого" равно или больше значения "Сумма штрафа" во вкладке "Реквизиты", 
                // то Поле "Штраф оплачен" должно принимать значение "Да"
                try
                {
                    var resolutionPayFineSum = payService
                                 .GetAll()
                                 .Where(x => x.Resolution.Id == entity.Id)
                                 .Sum(x => x.Amount)
                                 .ToDecimal();

                    if (entity.PenaltyAmount.HasValue && resolutionPayFineSum >= entity.PenaltyAmount)
                    {
                        entity.Paided = YesNoNotSet.Yes;
                    }
                    if (entity.InLawDate.HasValue && !entity.DueDate.HasValue)
                    {
                        if (entity.PenaltyAmount > 0)
                        {
                            entity.DueDate = entity.DeliveryDate.Value.AddDays(60);
                        }
                    }
                }
                catch
                { }

                return base.BeforeUpdateAction(service, entity);
            }
            finally
            {
                Container.Release(payService);
                Container.Release(documentgji);
                Container.Release(protocolservice);
                Container.Release(protocol197service);
                Container.Release(disposalservice);

            }
        }

        public override IDataResult BeforeDeleteAction(IDomainService<T> service, T entity)
        {
            var result = base.BeforeDeleteAction(service, entity);

            if (!result.Success)
            {
                return Failure(result.Message);
            }

            var annexService = this.Container.Resolve<IDomainService<ResolutionAnnex>>();
            var defService = this.Container.Resolve<IDomainService<ResolutionDefinition>>();
            var disputeService = this.Container.Resolve<IDomainService<ResolutionDispute>>();
            var payService = this.Container.Resolve<IDomainService<ResolutionPayFine>>();
            var ltService = this.Container.Resolve<IDomainService<ResolutionLongText>>();

            try
            {
                annexService.GetAll().Where(x => x.Resolution.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => annexService.Delete(x));
                ltService.GetAll().Where(x => x.Resolution.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => ltService.Delete(x));

                defService.GetAll().Where(x => x.Resolution.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => defService.Delete(x));

                disputeService.GetAll().Where(x => x.Resolution.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => disputeService.Delete(x));

                payService.GetAll().Where(x => x.Resolution.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => payService.Delete(x));

                return Success();
            }
            finally
            {
                Container.Release(annexService);
                Container.Release(defService);
                Container.Release(disputeService);
                Container.Release(payService);
            }
        }
    }
}