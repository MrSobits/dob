namespace Bars.GkhGji.Interceptors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;

    public class ArticleLawGjiInterceptor : EmptyDomainInterceptor<ArticleLawGji>
    {
        public override IDataResult BeforeUpdateAction(IDomainService<ArticleLawGji> service, ArticleLawGji entity)
        {
            switch (entity.OmsRegion)
            {
                case Enums.OmsRegionBelonging.OMS:
                    entity.OMS = "ОМС";
                    break;
                case Enums.OmsRegionBelonging.Region:
                    entity.OMS = "РЕГИОН";
                    break;
            }

            return Success();
        }

        public override IDataResult BeforeDeleteAction(IDomainService<ArticleLawGji> service, ArticleLawGji entity)
        {
            var refFuncs = new List<Func<long, string>>
                               {
                                  id => this.Container.Resolve<IDomainService<PrescriptionArticleLaw>>().GetAll().Any(x => x.ArticleLaw.Id == id) ? "Статьи закона  в предписании" : null,
                                  id => this.Container.Resolve<IDomainService<ProtocolArticleLaw>>().GetAll().Any(x => x.ArticleLaw.Id == id) ? "Статьи закона в протоколе" : null,
                                  id => this.Container.Resolve<IDomainService<ResolProsArticleLaw>>().GetAll().Any(x => x.ArticleLaw.Id == id) ? "Статьи закона в постановлении прокуратуры" : null
                               };

            var refs = refFuncs.Select(x => x(entity.Id)).Where(x => x != null).ToArray();

            var message = string.Empty;

            if (refs.Length > 0)
            {
                message = refs.Aggregate(message, (current, str) => current + string.Format(" {0}; ", str));
                message = string.Format("Существуют связанные записи в следующих таблицах: {0}", message);
                return Failure(message);
            }

            return Success();
        }
    }
}