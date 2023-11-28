namespace Bars.GkhGji.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Utils;

    public class BaseDispHeadServiceInterceptor : BaseDispHeadServiceInterceptor<BaseDispHead>
    {
    }

    public class BaseDispHeadServiceInterceptor<T> : InspectionGjiInterceptor<T>
        where T: BaseDispHead
    {
        public override IDataResult BeforeCreateAction(IDomainService<T> service, T entity)
        {
            // Перед сохранением формируем номер основания проверки
            Utils.CreateInspectionNumber(Container, entity, entity.DispHeadDate.ToDateTime().Year);

            return base.BeforeCreateAction(service, entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<T> service, T entity)
        {
#warning Данный код работает только при сохранении с клиент. Тоесть добавили в Entity не хранимое поле и в Интерцепторе предполагают что оно должно прийти
#warning Данный код не сработает когда будет например Перевод Статуса которая не склиента запускается а ссервера
            // Здесь вообщем в данно проверки возможно создание неявной ссылки на документ
            var serviceInspDocReference = Container.Resolve<IDomainService<InspectionDocGjiReference>>();

            try
            {
                if (entity.PrevDocument != null)
                {
                    var prevDocs = serviceInspDocReference.GetAll().Where(x => x.Inspection.Id == entity.Id)
                                                      .Select(x => x.Id)
                                                      .ToList();

                    foreach (var prevDoc in prevDocs)
                    {
                        serviceInspDocReference.Delete(prevDoc);
                    }

                    serviceInspDocReference.Save(new InspectionDocGjiReference
                    {
                        Document = entity.PrevDocument,
                        Inspection = entity,
                        TypeReference = TypeInspectionDocGjiReference.DispHeadPrevDocument
                    });
                }

                return base.BeforeUpdateAction(service, entity);
            }
            finally
            {
                Container.Release(serviceInspDocReference);
            }
        }
        
    }
}
