namespace Bars.GkhGji.Interceptors
{
    using B4;
    using B4.Modules.States;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Entities;
    using System.Linq;

    public class AppealCitsRealityObjectInterceptor : EmptyDomainInterceptor<AppealCitsRealityObject>
    {
        public IDomainService<AppealCits> AppealCitsDomain { get; set; }

        public override IDataResult AfterCreateAction(IDomainService<AppealCitsRealityObject> service, AppealCitsRealityObject entity)
        {

            if (entity.RealityObject != null)
            {
                var realityContainer = this.Container.Resolve<IDomainService<RealityObject>>();
                RealityObject newRO = realityContainer.Get(entity.RealityObject.Id);
                string address = newRO.Address;
                var appcitCont = this.Container.Resolve<IRepository<AppealCits>>();
                AppealCits thisAppeal = appcitCont.Get(entity.AppealCits.Id);
                var realityCont = this.Container.Resolve<IDomainService<AppealCitsRealityObject>>();
              
                var realities = realityCont.GetAll()
                    .Where(x => x.AppealCits.Id == thisAppeal.Id)
                    .Where(x => x.Id != entity.Id)
                    .Select(x => x.RealityObject.Address).ToList();
                foreach (string thisreality in realities)
                {
                    if (!string.IsNullOrEmpty(thisreality))
                    {
                        address += "; " + thisreality;
                    }
                }
                if (address.Length > 500)
                {
                    address = address.Substring(0, 450);
                }
                thisAppeal.RealityAddresses = address;

               
                if (newRO != null)
                {
                    try
                    {
                        thisAppeal.Municipality = newRO.Municipality.Name;
                        thisAppeal.MunicipalityId = newRO.Municipality.Id;
                    }
                    catch
                    {
                        thisAppeal.Municipality = "Не определено";
                    }
                }
                appcitCont.Update(thisAppeal);

            }

            return this.Success();

        }  


    }
}
