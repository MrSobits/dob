namespace Bars.GkhCr.DomainService
{
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.DomainService.BaseParams;
    using Entities;

    public class ObjectCrDomainService : BaseDomainService<Entities.ObjectCr>
    {
       

        public override IDataResult Delete(BaseParams baseParams)
        {
            var ids = Converter.ToLongArray(baseParams.Params, "records");

            var objectCrList = Container.ResolveDomain<Entities.ObjectCr>().GetAll()
                .Where(x => ids.Contains(x.Id))
                .ToList();

            InTransaction(() =>
            {
                foreach (var objectCr in objectCrList)
                {
                    objectCr.BeforeDeleteProgramCr = objectCr.ProgramCr;
                    objectCr.ProgramCr = null;

                    IDataResult result = null;
                    var interceptors = Container.ResolveAll<IDomainServiceInterceptor<Entities.ObjectCr>>();

                    try
                    {
                        CallBeforeDeleteInterceptors(objectCr, ref result, interceptors);
                        UpdateEntityInternal(objectCr);
                        CallAfterDeleteInterceptors(objectCr, ref result, interceptors);
                    }
                    finally
                    {
                        ReleaseInterceptors(interceptors);
                    }
                }
            });

            return new BaseDataResult(ids);
        }
    }
}