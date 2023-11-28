namespace Bars.Gkh.Interceptors.Dict
{
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Entities.Dicts;

    public class ZonalInspectionInterceptor : EmptyDomainInterceptor<ZonalInspection>
    {
        public override IDataResult BeforeCreateAction(IDomainService<ZonalInspection> service, ZonalInspection entity)
        {
            return CheckForm(entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<ZonalInspection> service, ZonalInspection entity)
        {
            return CheckForm(entity);
        }

        private IDataResult CheckForm(ZonalInspection entity)
        {
            if (entity.Name.IsEmpty())
            {
                return Failure("Не заполнены обязательные поля: Наименование");
            }

            if (entity.Name.Length > 300)
            {
                return Failure("Количество знаков в поле Наименование не должно превышать 300 символов");
            }

            if (entity.Address.IsNotEmpty() && entity.Address.Length > 300)
            {
                return Failure("Количество знаков в поле Адрес не должно превышать 300 символов");
            }

            if (entity.ZoneName.IsNotEmpty() && entity.ZoneName.Length > 300)
            {
                return Failure("Количество знаков в поле Зональное наименование не должно превышать 300 символов");
            }

            if (entity.BlankName.IsNotEmpty() && entity.BlankName.Length > 300)
            {
                return Failure("Количество знаков в поле Наименование для бланка не должно превышать 300 символов");
            }

            if (entity.Okato.IsNotEmpty() && entity.Okato.Length > 30)
            {
                return Failure("Количество знаков в поле ОКАТО не должно превышать 30 символов");
            }

            if (entity.DepartmentCode.IsNotEmpty() && entity.DepartmentCode.Length > 30)
            {
                return Failure("Количество знаков в поле Код отдела не должно превышать 30 символов");
            }

            if (entity.Email.IsNotEmpty() && entity.Email.Length > 50)
            {
                return Failure("Количество знаков в поле Почта не должно превышать 50 символов");
            }

            if (entity.Phone.IsNotEmpty() && entity.Phone.Length > 50)
            {
                return Failure("Количество знаков в поле Телефон не должно превышать 50 символов");
            }

            return Success();
        }
    }
}
