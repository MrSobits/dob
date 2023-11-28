namespace Bars.GkhGji.Regions.BaseChelyabinsk.Interceptors.ActRemoval
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Interceptors;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.ActRemoval;

    public class ChelyabinskActRemovalServiceInterceptor : ActRemovalServiceInterceptor<ChelyabinskActRemoval>
    {
		public override IDataResult BeforeDeleteAction(IDomainService<ChelyabinskActRemoval> service, ChelyabinskActRemoval entity)
		{
			var provDocService = this.Container.Resolve<IDomainService<ActRemovalProvidedDoc>>();

			try
			{
				var refFuncs = new List<Func<long, string>>
                {
                    id => provDocService.GetAll().Any(x => x.ActRemoval.Id == id) ? "Предоставленные документы" : null
                };

				var refs = refFuncs.Select(x => x(entity.Id)).Where(x => x != null).ToArray();

				var message = string.Empty;

				if (refs.Length > 0)
				{
					message = refs.Aggregate(message, (current, str) => current + string.Format(" {0}; ", str));
					message = string.Format("Существуют связанные записи в следующих таблицах: {0}", message);
					return this.Failure(message);
				}

				return base.BeforeDeleteAction(service, entity);
			}
			finally
			{
				this.Container.Release(provDocService);
			}
		}
    }
}