using Bars.B4;
using Bars.B4.DataAccess;
using Bars.B4.Utils;
using Bars.Gkh.Entities;
using Bars.GkhGji.Entities;
using Castle.Windsor;
using System.Collections.Generic;
using System.Linq;


namespace Bars.GkhGji.DomainService
{
    public class ViolationGjiMunicipalityService : IViolationGjiMunicipality
    {

        public IWindsorContainer Container { get; set; }

        public IDataResult AddMunicipalities(BaseParams baseParams)
        {
            using (var transaction = this.Container.Resolve<IDataTransaction>())
            {
                var serviceViolGji = this.Container.Resolve<IDomainService<ViolationGji>>();
                var serviceViolMunicipality = this.Container.Resolve<IDomainService<ViolationGjiMunicipality>>();
                var serviceMunicipality = this.Container.Resolve<IDomainService<Municipality>>();

                try
                {
                    var violationid = baseParams.Params.GetAs<long>("violationId");
                    var objectIds = baseParams.Params.GetAs<long[]>("objectIds") ?? new long[0];

                    if (objectIds.Length < 1)
                        return new BaseDataResult(false, "Необходимо выбрать муниципальные образования");

                    var listObjects =
                        serviceViolMunicipality.GetAll()
                            .Where(x => x.ViolationGji.Id == violationid)
                            .Select(x => x.Municipality.Id)
                            .Where(x => objectIds.Contains(x))
                            .Distinct()
                            .ToList();

                    var violationgji = serviceViolGji.Load(violationid);

                    foreach (var id in objectIds)
                    {
                        if (listObjects.Contains(id))
                            continue;

                        var newRecord = new ViolationGjiMunicipality
                        {
                            ViolationGji = violationgji,
                            Municipality = serviceMunicipality.Load(id)
                        };

                        serviceViolMunicipality.Save(newRecord);
                    }

                    transaction.Commit();
                    return new BaseDataResult();
                }
                catch (ValidationException exc)
                {
                    transaction.Rollback();
                    return new BaseDataResult { Success = false, Message = exc.Message };
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
                finally
                {
                    this.Container.Release(serviceViolGji);
                    this.Container.Release(serviceMunicipality);
                    this.Container.Release(serviceViolMunicipality);
                }
            }
        }

    }
}
