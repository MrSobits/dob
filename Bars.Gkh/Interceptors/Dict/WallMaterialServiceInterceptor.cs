﻿namespace Bars.Gkh.Interceptors.Dict
{
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;

    public class WallMaterialServiceInterceptor : EmptyDomainInterceptor<WallMaterial>
    {
        public override IDataResult BeforeCreateAction(IDomainService<WallMaterial> service, WallMaterial entity)
        {
            return CheckForm(entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<WallMaterial> service, WallMaterial entity)
        {
            return CheckForm(entity);
        }

        public override IDataResult BeforeDeleteAction(IDomainService<WallMaterial> service, WallMaterial entity)
        {
            if (Container.Resolve<IDomainService<RealityObject>>().GetAll().Any(x => x.WallMaterial.Id == entity.Id))
            {
                return Failure("Существуют связанные записи в следующих таблицах: Реестр жилых домов;");
            }

            return Success();
        }

        private IDataResult CheckForm(WallMaterial entity)
        {
            if (entity.Name.IsNotEmpty() && entity.Name.Length > 300)
            {
                return Failure("Количество знаков в поле Наименование не должно превышать 300 символов");
            }

            return Success();
        }
    }
}
