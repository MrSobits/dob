namespace Bars.GkhGji.Interceptors
{
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.Dict;

    public class RiskCategoryInterceptor : EmptyDomainInterceptor<RiskCategory>
    {
        public override IDataResult BeforeCreateAction(IDomainService<RiskCategory> service, RiskCategory entity)
        {
            return this.CheckFields(entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<RiskCategory> service, RiskCategory entity)
        {
            return this.CheckFields(entity);
        }

        private IDataResult CheckFields(RiskCategory entity)
        {
            if (entity.Name.IsEmpty())
            {
                return this.Failure("В поле 'Наименование' введены некорректные данные");
            }
            
            if (entity.RiskFrom == null)
            {

                return this.Failure("В поле 'Показатель риска от' введены некорректные данные");
            }

            if (entity.RiskTo == null)
            {

                return this.Failure("В поле 'Показатель риска до' введены некорректные данные");
            }

            if (entity.Code.IsEmpty())
            {
                return this.Failure("В поле 'Код' введены некорректные данные");
            }
            
            return this.Success();
        }
    }
}