using Bars.B4.Modules.States;
using Bars.Gkh.Domain.CollectionExtensions;

namespace Bars.Gkh.Interceptors
{
    using System;
    using System.Linq;

    using B4;

    using Bars.B4.Utils;

    using Entities;

    public class ManOrgLicenseInterceptor : EmptyDomainInterceptor<ManOrgLicense>
    {
        public override IDataResult BeforeCreateAction(IDomainService<ManOrgLicense> service, ManOrgLicense entity)
        {
            var stateProvider = Container.Resolve<IStateProvider>();
            stateProvider.SetDefaultState(entity);

            return CreateNumber(service, entity);
        }

        private IDataResult CreateNumber(IDomainService<ManOrgLicense> service, ManOrgLicense entity)
        {
            if (!entity.LicNum.HasValue)
            {
                var lasNum = service.GetAll()
                        .Where(x => x.LicNum.HasValue && x.Id != entity.Id)
                        .Select(x => x.LicNum.Value)
                        .SafeMax(x => x);

                entity.LicNum = lasNum + 1;
                entity.LicNumber = entity.LicNum.ToString();
            }
            
            if (service.GetAll().Any(x => x.LicNum == entity.LicNum && entity.Id != x.Id))
            {
                return Failure(string.Format("Лицензия с номером {0} уже существует", entity.LicNum));
            }
            return Success();
        }
        
        public override IDataResult BeforeDeleteAction(IDomainService<ManOrgLicense> service, ManOrgLicense entity)
        {
            var docDomain = Container.Resolve<IDomainService<ManOrgLicenseDoc>>();

            try
            {
                docDomain.GetAll().Where(x => x.ManOrgLicense.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => docDomain.Delete(x));

                return Success();
            }
            catch (Exception exc)
            {
                return Failure("Не удалось удалить связанные записи "+ exc.Message);
            }
            finally
            {
                Container.Release(docDomain);
            }
        }
    }
}