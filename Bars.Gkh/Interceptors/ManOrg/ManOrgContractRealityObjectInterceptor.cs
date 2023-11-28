namespace Bars.Gkh.Interceptors.ManOrg
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Utils;

    public class ManOrgContractRealityObjectInterceptor : EmptyDomainInterceptor<ManOrgContractRealityObject>
    {
        public override IDataResult AfterCreateAction(IDomainService<ManOrgContractRealityObject> service, ManOrgContractRealityObject entity)
        {
            this.UpdateRealityObject(service, entity);

            return this.Success();
        }

        public override IDataResult AfterDeleteAction(IDomainService<ManOrgContractRealityObject> service, ManOrgContractRealityObject entity)
        {
            this.UpdateRealityObject(service, entity);

            return this.Success();
        }

        private void UpdateRealityObject(IDomainService<ManOrgContractRealityObject> service, ManOrgContractRealityObject entity)
        {
            var roRepository = this.Container.ResolveRepository<RealityObject>();

            using (this.Container.Using(roRepository))
            {
                var now = DateTime.Now.Date;

                var contract = service.GetAll()
                    .Where(x => x.RealityObject.Id == entity.RealityObject.Id)
                    .Where(x => x.ManOrgContract.StartDate <= now)
                    .Where(x => !x.ManOrgContract.EndDate.HasValue || x.ManOrgContract.EndDate >= now)
                    .Select(
                        x => new
                        {
                            RoId = x.RealityObject.Id,
                            ManOrgName = x.ManOrgContract.ManagingOrganization.Contragent.Name,
                            x.ManOrgContract.TypeContractManOrgRealObj
                        })
                    .AsEnumerable()
                    .GroupBy(x => x.RoId)
                    .Select(
                        x => new
                        {
                            ManOrgs = x.AggregateWithSeparator(y => y.ManOrgName, ", "),
                            TypesContract = x.AggregateWithSeparator(y => y.TypeContractManOrgRealObj.GetEnumMeta().Display, ", ")
                        })
                    .FirstOrDefault();

                var ro = entity.RealityObject;

                if (contract != null)
                {
                    ro.ManOrgs = contract.ManOrgs;
                    ro.TypesContract = contract.TypesContract;
                }
                else
                {
                    ro.ManOrgs = null;
                    ro.TypesContract = null;
                }

                roRepository.Update(ro);
            }
            
        }
    }
}