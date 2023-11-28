namespace Bars.GkhGji.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Такую пустышку навсякий случай нужно чтобы в регионах (Там где уже заменили или отнаследовались от этого класса) непопадало и можно было бы изменять методы как сущности Protocol
    /// </summary>
    public class ProtocolServiceInterceptor : ProtocolServiceInterceptor<Protocol>
    {
        // Внимание !! Код override нужно писать не в этом классе, а в ProtocolServiceInterceptor<T>
    }

    /// <summary>
    /// Короче такой поворот событий делается для того чтобы в Модулях регионов  с помошью 
    /// SubClass расширять сущность Protocol + не переписывать код который регистрируется по сущности
    /// то есть в Protocol добавляеться поля, но интерцептор поскольку Generic просто наследуется  
    /// </summary>
    public class ProtocolServiceInterceptor<T> : DocumentGjiInterceptor<T>
       where T : Protocol
    {
        public override IDataResult BeforeDeleteAction(IDomainService<T> service, T entity)
        {
            // Перед удалением проверяем есть ли дочерние документы
            var annexService = this.Container.Resolve<IDomainService<ProtocolAnnex>>();
            var lawService = this.Container.Resolve<IDomainService<ProtocolArticleLaw>>();
            var definitionService = this.Container.Resolve<IDomainService<ProtocolDefinition>>();
            var domainServiceViolation = Container.Resolve<IDomainService<ProtocolViolation>>();

            try
            {
                var result = base.BeforeDeleteAction(service, entity);

                if (!result.Success)
                {
                    return Failure(result.Message);
                }

                annexService.GetAll().Where(x => x.Protocol.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => annexService.Delete(x));

                lawService.GetAll().Where(x => x.Protocol.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => lawService.Delete(x));

                definitionService.GetAll().Where(x => x.Protocol.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => definitionService.Delete(x));

                domainServiceViolation.GetAll().Where(x => x.Document.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => domainServiceViolation.Delete(x));

                return result;
            }
            finally
            {
                Container.Release(annexService);
                Container.Release(lawService);
                Container.Release(definitionService);
                Container.Release(domainServiceViolation);
            }
        }
    }
}