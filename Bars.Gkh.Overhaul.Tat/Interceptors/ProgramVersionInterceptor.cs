namespace Bars.Gkh.Overhaul.Tat.Interceptors
{
    using System.Linq;
    using Bars.B4;
    using Entities;

    public class ProgramVersionInterceptor : EmptyDomainInterceptor<ProgramVersion>
    {
        public override IDataResult BeforeCreateAction(IDomainService<ProgramVersion> service, ProgramVersion entity)
        {
            return this.CheckVersions(service, entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<ProgramVersion> service, ProgramVersion entity)
        {
            return this.CheckVersions(service, entity);
        }

        private BaseDataResult CheckVersions(IDomainService<ProgramVersion> service, ProgramVersion entity)
        {
#warning Совсем неожидаемый результат работы интерцептора
            if (entity.IsMain && service.GetAll().Any(x => x.Id != entity.Id && x.IsMain && x.Municipality.Id == entity.Municipality.Id))
            {
                return Failure("Основная версия программы уже выбрана! Уберите отметку \"Основная\" у старой версии и после этого выберите новую основную версию");
            }

            return Success();
        }
    }
}