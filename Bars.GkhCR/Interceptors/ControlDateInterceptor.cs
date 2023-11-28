namespace Bars.GkhCr.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhCr.Entities;

    public class ControlDateInterceptor : EmptyDomainInterceptor<ControlDate>
    {
        public override IDataResult BeforeCreateAction(IDomainService<ControlDate> service, ControlDate entity)
        {
            if (Container.Resolve<IDomainService<ControlDate>>().GetAll()
                         .Any(x => x.ProgramCr.Id == entity.ProgramCr.Id && x.Work.Id == entity.Work.Id))
            {
                return Failure("Данный вид работ уже есть в программе.");
            }

            return Success();
        }

        public override IDataResult BeforeDeleteAction(IDomainService<ControlDate> service, ControlDate entity)
        {
            if (Container.Resolve<IDomainService<ControlDateStageWork>>().GetAll().Any(x => x.ControlDate.Id == entity.Id))
            {
                return Failure("Существуют связанные записи в следующих таблицах: Этапы работ контрольного срока;");
            }

            return this.Success();
        }
    }
}