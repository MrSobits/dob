using Bars.B4.Utils;

namespace Bars.GkhCr.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhCr.Entities;

    public class ProgramCrInterceptor : EmptyDomainInterceptor<ProgramCr>
    {
        public IDomainService<ProgramCrChangeJournal> ProgramChangeJournalDomain { get; set; }

        public override IDataResult BeforeCreateAction(IDomainService<ProgramCr> service, ProgramCr entity)
        {
            return CheckEntity(entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<ProgramCr> service, ProgramCr entity)
        {
            return CheckEntity(entity);
        }

        public override IDataResult BeforeDeleteAction(IDomainService<ProgramCr> service, ProgramCr entity)
        {
            ProgramChangeJournalDomain.GetAll()
                .Where(x => x.ProgramCr.Id == entity.Id)
                .Select(x => x.Id).AsEnumerable()
                .ForEach(x => ProgramChangeJournalDomain.Delete(x));

            return this.Success();
        }

        private IDataResult CheckEntity(ProgramCr entity)
        {
            if (!string.IsNullOrEmpty(entity.Code) && entity.Code.Length > 200)
            {
                return Failure("Количество знаков поля Код не должно превышать 200 символов");
            }

            if (string.IsNullOrEmpty(entity.Name))
            {
                return Failure("Не заполнено обязательное поле: Наименование");
            }

            if (entity.Name.Length > 300)
            {
                return Failure("Количество знаков поля Наименование не должно превышать 200 символов");
            }

            if (entity.Period == null)
            {
                return Failure("Не заполнено обязательное поле: Период");
            }

            if (!string.IsNullOrEmpty(entity.Description) && entity.Description.Length > 2000)
            {
                return Failure("Количество знаков в поле Примечание не должно превышать 2000 символов");
            }

            return Success();
        }
    }
}